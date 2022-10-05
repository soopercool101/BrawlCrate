using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class Common2MiscDataNode : ARCEntryNode
    {
        public override ResourceType ResourceFileType => ResourceType.Container;
        internal Common2TblHeader* Header => (Common2TblHeader*) WorkingUncompressed.Address;

        // Header variables
        private bint _offCount = 0;

        public override bool OnInitialize()
        {
            base.OnInitialize();

            _offCount = Header->_OffCount;

            return true;
        }

        private class OffsetPair
        {
            public int dataOffset;
            public int nameOffset;
            public int dataEnd;

            public override string ToString()
            {
                return dataOffset + "-" + dataEnd + ", " + nameOffset;
            }
        }

        public override void OnPopulate()
        {
            VoidPtr baseAddress = WorkingUncompressed.Address + sizeof(Common2TblHeader);

            int dataLength = Header->_DataLength;
            VoidPtr offsetTable = baseAddress + dataLength + Header->_OffCount * 4;
            VoidPtr stringList = offsetTable + Header->_DataTable * 8;
            List<OffsetPair> offsets = new List<OffsetPair>();

            bint* ptr = (bint*) offsetTable;
            for (int i = 0; i < Header->_DataTable; i++)
            {
                OffsetPair o = new OffsetPair
                {
                    dataOffset = *ptr++,
                    nameOffset = *ptr++
                };
                offsets.Add(o);
            }

            offsets = offsets.OrderBy(o => o.dataOffset).ToList();
            for (int i = 1; i < offsets.Count; i++)
            {
                offsets[i - 1].dataEnd = offsets[i].dataOffset;
            }

            offsets[offsets.Count - 1].dataEnd = dataLength;

            foreach (OffsetPair o in offsets)
            {
                if (o.dataEnd <= o.dataOffset)
                {
                    throw new Exception("Invalid data length (less than data offset) in common2 data");
                }

                DataSource source = new DataSource(baseAddress + o.dataOffset, o.dataEnd - o.dataOffset);
                string name = new string((sbyte*) stringList + o.nameOffset);
                ResourceNode node =
                    name.StartsWith("eventStage") ? new EventMatchNode()
                    : name.StartsWith("allstar") ? new AllstarStageTblNode()
                    : name.StartsWith("simpleStage") ? new ClassicStageTblNode()
                    : name.StartsWith("muEventTbl") ? new MuEventTblNode()
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
            VoidPtr baseAddress = address + sizeof(Common2TblHeader);

            // Initiate header struct
            Common2TblHeader* Header = (Common2TblHeader*) address;
            *Header = new Common2TblHeader();
            Header->_OffCount = _offCount;
            Header->_DataTable = Children.Count;
            Header->_pad0 = Header->_pad1 =
                Header->_pad2 = Header->_pad3 = 0;

            Dictionary<ResourceNode, VoidPtr> dataLocations = new Dictionary<ResourceNode, VoidPtr>();

            VoidPtr ptr = baseAddress;
            foreach (ResourceNode child in Children)
            {
                int size = child.CalculateSize(false);
                dataLocations.Add(child, ptr);
                if (child is ClassicStageTblSizeTblNode && size == 48)
                {
                    // Rebuild
                    Dictionary<string, int> sizes = Children.ToDictionary(c => c.Name, c =>
                    {
                        int fullSize = c.CalculateSize(false);
                        int paddingInts = (c as ClassicStageTblNode)?.Padding?.Length ?? 0;
                        return fullSize - sizeof(bint) * paddingInts;
                    });

                    bint[] newTbl = new bint[12];
                    fixed (bint* newTblPtr = newTbl)
                    {
                        foreach (string key in new[]
                        {
                            "simpleStageB1Tbl",
                            "simpleStageB2Tbl",
                            "simpleStage11Tbl"
                        })
                        {
                            if (!sizes.TryGetValue(key, out int s) || s != 0x104)
                            {
                                MessageBox.Show(
                                    $"Changing the size of {key} may not work properly (BrawlCrate doesn't know yet which size entry to update)");
                            }
                        }

                        bint* bptr = newTblPtr;
                        foreach (string key in new[]
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
                            if (sizes.TryGetValue(key, out int s))
                            {
                                *bptr = s;
                            }
                            else
                            {
                                MessageBox.Show($"Cannot get size of {key}");
                            }

                            bptr++;
                        }

                        child.ReplaceRaw(newTblPtr, 48);
                    }
                }

                child.Rebuild(ptr, size, false);
                ptr += size;
            }

            Header->_DataLength = ptr - baseAddress;

            bint* dataPointers = (bint*) ptr;
            bint* stringPointers = dataPointers + 1;
            byte* strings = (byte*) (dataPointers + Children.Count + Children.Count);
            byte* currentString = strings;

            List<ResourceNode> childrenAlphabetical = Children.OrderBy(o => o.Name).ToList();

            foreach (ResourceNode child in childrenAlphabetical)
            {
                *dataPointers = dataLocations[child] - baseAddress;
                dataPointers += 2;
                *stringPointers = (int) (currentString - strings);
                stringPointers += 2;

                byte[] text = Encoding.UTF8.GetBytes(child.Name);
                fixed (byte* from = text)
                {
                    Memory.Move(currentString, from, (uint) text.Length);
                    currentString += text.Length;
                    *currentString = 0;
                    currentString++;
                }
            }

            if (currentString - address > length)
            {
                throw new Exception($"Wrong amount of memory allocated for rebuild of common2 data (Expected: {currentString - address} | Actual: {length})");
            }

            Header->_Length = length;
        }

        public override int OnCalculateSize(bool force)
        {
            int size = sizeof(Common2TblHeader);
            foreach (ResourceNode node in Children)
            {
                size += node.CalculateSize(true);
                size += Encoding.UTF8.GetByteCount(node.Name) + 1;
            }

            size += _offCount * 4 + Children.Count * 8;
            return size;
        }

        internal static ResourceNode TryParseGeneric(DataSource source, ResourceNode parent)
        {
            Common2TblHeader* header = (Common2TblHeader*) source.Address;
            return header->_Length == source.Length &&
                   header->_DataLength < source.Length &&
                   header->_DataTable > 0 &&
                   header->_OffCount == 0 // BrawlLib cannot properly rebuild nodes with _OffCount != 0 yet
                ? new Common2MiscDataNode()
                : null;
        }
    }
}