using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BrawlLib.IO;
using BrawlLib.SSBB.Types;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class ClassicStageBlockNode : ResourceNode
    {
        private ClassicStageBlockStageData data;

        public override ResourceType ResourceFileType => ResourceType.Container;

        [TypeConverter(typeof(DropDownListStageIDs))]
        public int StageID1
        {
            get => data._stageID1;
            set
            {
                data._stageID1 = (ushort) value;
                SignalPropertyChange();
            }
        }

        [TypeConverter(typeof(DropDownListStageIDs))]
        public int StageID2
        {
            get => data._stageID2;
            set
            {
                data._stageID2 = (ushort) value;
                SignalPropertyChange();
            }
        }

        [TypeConverter(typeof(DropDownListStageIDs))]
        public int StageID3
        {
            get => data._stageID3;
            set
            {
                data._stageID3 = (ushort) value;
                SignalPropertyChange();
            }
        }

        [TypeConverter(typeof(DropDownListStageIDs))]
        public int StageID4
        {
            get => data._stageID4;
            set
            {
                data._stageID4 = (ushort) value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            if (WorkingUncompressed.Length != sizeof(ClassicStageBlock))
                throw new Exception("Wrong size for ClassicStageBlockNode");

            // Copy the data from the address
            data = ((ClassicStageBlock*) WorkingUncompressed.Address)->_stages;

            var stageList = new List<string>();
            foreach (var stageID in new[] {StageID1, StageID2, StageID3, StageID4})
            {
                if (stageID == 255) continue;

                var found = Stage.Stages.FirstOrDefault(s => s.ID == stageID);
                stageList.Add(found == null ? stageID.ToString() : found.PacBasename);
            }

            _name = "Classic Stage Block (" + string.Join(", ", stageList) + ")";

            return true;
        }

        public override void OnPopulate()
        {
            var ptr = &((ClassicStageBlock*) WorkingUncompressed.Address)->_opponent1;
            for (var i = 0; i < 3; i++)
            {
                var source = new DataSource(ptr, sizeof(AllstarFighterData));
                new AllstarFighterNode().Initialize(this, source);
                ptr++;
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            // Copy the data back to the address
            var dataPtr = (ClassicStageBlock*) address;
            dataPtr->_stages._unknown00 = data._unknown00;
            dataPtr->_stages._stageID1 = data._stageID1;
            dataPtr->_stages._stageID2 = data._stageID2;
            dataPtr->_stages._stageID3 = data._stageID3;
            dataPtr->_stages._stageID4 = data._stageID4;

            // Rebuild children using new address
            var ptr = &((ClassicStageBlock*) address)->_opponent1;
            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Rebuild(ptr, sizeof(AllstarFighterData), true);
                ptr++;
            }
        }

        public override int OnCalculateSize(bool force)
        {
            // Constant size (260 bytes)
            return sizeof(ClassicStageBlock);
        }
    }

    public unsafe class ClassicStageTblNode : ResourceNode
    {
        private List<int> _padding;
        public override ResourceType ResourceFileType => ResourceType.ClassicStageTbl;

        public string Padding => string.Join(", ", _padding);

        public override bool OnInitialize()
        {
            base.OnInitialize();

            var ptr = WorkingUncompressed.Address;
            var numEntries = WorkingUncompressed.Length / sizeof(ClassicStageBlock);
            for (var i = 0; i < numEntries; i++) ptr += sizeof(ClassicStageBlock);

            _padding = new List<int>();
            var ptr2 = (bint*) ptr;
            while (ptr2 < WorkingUncompressed.Address + WorkingUncompressed.Length) _padding.Add(*ptr2++);

            return true;
        }

        public override void OnPopulate()
        {
            var numEntries = WorkingUncompressed.Length / sizeof(ClassicStageBlock);

            var ptr = (ClassicStageBlock*) WorkingUncompressed.Address;
            for (var i = 0; i < numEntries; i++)
            {
                var source = new DataSource(ptr, sizeof(ClassicStageBlock));
                new ClassicStageBlockNode().Initialize(this, source);
                ptr++;
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            // Rebuild children using new address
            var ptr = (ClassicStageBlock*) address;
            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Rebuild(ptr, sizeof(ClassicStageBlock), true);
                ptr++;
            }

            var ptr2 = (bint*) ptr;
            foreach (int pad in Padding) *ptr2++ = pad;
        }

        public override int OnCalculateSize(bool force)
        {
            return sizeof(ClassicStageBlock) * Children.Count + Padding.Length * sizeof(bint);
        }

        public void CreateEntry()
        {
            var tempFile = FileMap.FromTempFile(sizeof(ClassicStageBlock));
            // Is this the right way to add a new child node?
            var node = new ClassicStageBlockNode();
            node.Initialize(this, tempFile);
            AddChild(node, true);
        }
    }

    public class ClassicStageTblSizeTblNode : RawDataNode
    {
    }
}