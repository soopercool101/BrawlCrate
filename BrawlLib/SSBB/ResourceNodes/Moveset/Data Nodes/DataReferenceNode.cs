using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MoveDefReferenceNode : MoveDefEntryNode
    {
        internal FDefStringEntry* Header => (FDefStringEntry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.MDefRefList;

        private FDefStringTable* stringTable;

        private Dictionary<string, FDefStringEntry> exSubRoutineTable = new Dictionary<string, FDefStringEntry>();

        public MoveDefReferenceNode(VoidPtr table)
        {
            stringTable = (FDefStringTable*) table;
        }

        private bool populated;

        public override bool OnInitialize()
        {
            _name = "References";
            for (int i = 0; i < WorkingUncompressed.Length / 8; i++)
            {
                if (!exSubRoutineTable.ContainsKey(stringTable->GetString(Header[i]._stringOffset)))
                {
                    exSubRoutineTable.Add(stringTable->GetString(Header[i]._stringOffset), Header[i]);
                }
            }

            OnPopulate();
            return true;
        }

        public override void OnPopulate()
        {
            if (!populated)
            {
                populated = true;
                foreach (KeyValuePair<string, FDefStringEntry> ex in exSubRoutineTable)
                {
                    new MoveDefReferenceEntryNode {_name = ex.Key}.Initialize(this,
                        new DataSource(BaseAddress + ex.Value._dataOffset, 4));
                }
            }
        }
    }

    public unsafe class MoveDefReferenceEntryNode : MoveDefExternalNode
    {
        internal bint* Header => (bint*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.Unknown;

        public int[] Offsets => _offsets.ToArray();

        [HandleProcessCorruptedStateExceptions]
        public override bool OnInitialize()
        {
            _offsets = new List<int>();
            _offsets.Add(_offset);
            int offset = *Header;
            while (offset > 0)
            {
                _offsets.Add(offset);
                offset = *(bint*) (BaseAddress + offset);
                if (_offsets.Contains(offset))
                {
                    break;
                }
            }

            //_offsets.Add(offset);
            //Root._externalRefs.Add(this);
            return false;
        }
    }
}