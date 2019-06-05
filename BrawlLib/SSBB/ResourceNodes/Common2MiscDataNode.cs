using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class Common2MiscDataNode : ARCEntryNode
    {
        // Header variables
        private bint _offCount = 0;
        public override ResourceType ResourceFileType => ResourceType.Container;
        internal Common2TblHeader* Header => (Common2TblHeader*) WorkingUncompressed.Address;

        public override bool OnInitialize()
        {
            base.OnInitialize();

            _offCount = Header->_OffCount;

            return true;
        }

        public override void OnPopulate()
        {
            var baseAddress = WorkingUncompressed.Address + sizeof(Common2TblHeader);

            int dataLength = Header->_DataLength;
            var offsetTable = baseAddress + dataLength + Header->_OffCount * 4;
            var stringList = offsetTable + Header->_DataTable * 8;
            var offsets = new List<OffsetPair>();

            var ptr = (bint*) offsetTable;
            for (var i = 0; i < Header->_DataTable; i++)
            {
                var o = new OffsetPair
                {
                    dataOffset = *ptr++,
                    nameOffset = *ptr++
                };
                offsets.Add(o);
            }

            offsets = offsets.OrderBy(o => o.dataOffset).ToList();
            for (var i = 1; i < offsets.Count; i++) offsets[i - 1].dataEnd = offsets[i].dataOffset;
            offsets[offsets.Count - 1].dataEnd = dataLength;

            foreach (var o in offsets)
            {
                if (o.dataEnd <= o.dataOffset)
                    throw new Exception("Invalid data length (less than data offset) in common2 data");

                var source = new DataSource(baseAddress + o.dataOffset, o.dataEnd - o.dataOffset);
                var name = new string((sbyte*) stringList + o.nameOffset);
                var node =
                    name.StartsWith("eventStage") ? new EventMatchNode()
                    : name.StartsWith("allstar") ? new AllstarStageTblNode()
                    : name.StartsWith("simpleStage") ? new ClassicStageTblNode()
                    : name == "sndBgmTitleData" ? new SndBgmTitleDataNode()
                    : (ResourceNode) new ClassicStageTblSizeTblNode();
                node.Initialize(this, source);
                node.Name = name;
                node.HasChanged = false;
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            // Update base address for children.
            var baseAddress = address + sizeof(Common2TblHeader);

            // Initiate header struct
            var Header = (Common2TblHeader*) address;
            *Header = new Common2TblHeader();
            Header->_OffCount = _offCount;
            Header->_DataTable = Children.Count;
            Header->_pad0 = Header->_pad1 =
                Header->_pad2 = Header->_pad3 = 0;

            var dataLocations = new Dictionary<ResourceNode, VoidPtr>();

            var ptr = baseAddress;
            foreach (var child in Children)
            {
                var size = child.CalculateSize(false);
                dataLocations.Add(child, ptr);
                if (child is ClassicStageTblSizeTblNode && size == 48)
                {
                    // Rebuild
                    var sizes = Children.ToDictionary(c => c.Name, c =>
                    {
                        var fullSize = c.CalculateSize(false);
                        var paddingInts = (c as ClassicStageTblNode)?.Padding?.Length ?? 0;
                        return fullSize - sizeof(bint) * paddingInts;
                    });

                    var newTbl = new bint[12];
                    fixed (bint* newTblPtr = newTbl)
                    {
                        foreach (var key in new[]
                        {
                            "simpleStageB1Tbl",
                            "simpleStageB2Tbl",
                            "simpleStage11Tbl"
                        })
                            if (!sizes.TryGetValue(key, out var s) || s != 0x104)
                                MessageBox.Show(
                                    $"Changing the size of {key} may not work properly (BrawlCrate doesn't know yet which size entry to update)");

                        var bptr = newTblPtr;
                        foreach (var key in new[]
                        {
                            "simpleStage1Tbl",
                            "simpleStage2Tbl",
                            "simpleStage3Tbl",
                            "simpleStage4Tbl",
                            "simpleStageB1Tbl",
                            "simpleStage5Tbl",
                            "simpleStage6Tbl",
                            "simpleStage7Tbl",
                            "simpleStage8Tbl",
                            "simpleStage9Tbl",
                            "simpleStage10Tbl",
                            "simpleStageB2Tbl"
                        })
                        {
                            if (sizes.TryGetValue(key, out var s))
                                *bptr = s;
                            else
                                MessageBox.Show($"Cannot get size of {key}");
                            bptr++;
                        }

                        child.ReplaceRaw(newTblPtr, 48);
                    }
                }

                child.Rebuild(ptr, size, false);
                ptr += size;
            }

            Header->_DataLength = ptr - baseAddress;

            var dataPointers = (bint*) ptr;
            var stringPointers = dataPointers + 1;
            var strings = (byte*) (dataPointers + Children.Count + Children.Count);
            var currentString = strings;

            foreach (var child in Children)
            {
                *dataPointers = dataLocations[child] - baseAddress;
                dataPointers += 2;
                *stringPointers = (int) (currentString - strings);
                stringPointers += 2;

                var text = Encoding.UTF8.GetBytes(child.Name);
                fixed (byte* from = text)
                {
                    Memory.Move(currentString, from, (uint) text.Length);
                    currentString += text.Length;
                    *currentString = 0;
                    currentString++;
                }
            }

            Header->_Length = currentString - address;

            if (Header->_Length != length)
                throw new Exception("Wrong amount of memory allocated for rebuild of common2 data");
        }

        public override int OnCalculateSize(bool force)
        {
            var size = sizeof(Common2TblHeader);
            foreach (var node in Children)
            {
                size += node.CalculateSize(true);
                size += Encoding.UTF8.GetByteCount(node.Name) + 1;
            }

            size += _offCount * 4 + Children.Count * 8;
            return size;
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            var header = (Common2TblHeader*) source.Address;
            return header->_Length == source.Length &&
                   header->_DataLength < source.Length &&
                   header->_OffCount == 0 // BrawlLib cannot properly rebuild nodes with _OffCount != 0 yet
                ? new Common2MiscDataNode()
                : null;
        }

        private class OffsetPair
        {
            public int dataEnd;
            public int dataOffset;
            public int nameOffset;

            public override string ToString()
            {
                return dataOffset + "-" + dataEnd + ", " + nameOffset;
            }
        }
    }
}