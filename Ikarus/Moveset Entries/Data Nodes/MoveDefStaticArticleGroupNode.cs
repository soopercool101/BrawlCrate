using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using Ikarus;

namespace Ikarus.MovesetFile
{
    public unsafe class MoveDefStaticArticleGroupNode : MovesetEntry
    {
        internal FDefListOffset* Header { get { return (FDefListOffset*)WorkingUncompressed.Address; } }
        internal int i = 0;

        public int DataOffset { get { return Header->_startOffset; } }
        public int Count { get { return Header->_listCount; } }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
                _name = "Static Articles";
            return Count > 0;
        }

        public override void Parse(VoidPtr address)
        {
            VoidPtr addr = BaseAddress + DataOffset;
            for (int i = 0; i < Count; i++)
            {
                ArticleEntry d = new ArticleEntry() { Static = true };
                d.Initialize(this, addr + i * 56, 56);
            }
        }

        protected override int OnGetSize()
        {
            _lookupCount = (Children.Count > 0 ? 1 : 0);
            int size = 8;
            foreach (ArticleEntry a in Children)
            {
                size += a.CalculateSize(true);
                _lookupCount += a._lookupCount;
            }
            return size;
        }

        protected override void OnWrite(VoidPtr address)
        {
            VoidPtr addr = address;
            foreach (ArticleEntry b in Children)
            {
                b._buildHeader = false;
                b.Rebuild(addr, b._childLength, true);
                addr += b._childLength;
            }

            VoidPtr start = addr;
            foreach (ArticleEntry b in Children)
            {
                b._rebuildAddr = addr;

                Article* article = (Article*)addr;

                article->_id = b.id;
                article->_boneID = b.charBone;
                article->_arcGroup = b.articleBone;

                article->_actionsStart = b.aStart;
                article->_actionFlagsStart = b.aFlags;
                article->_subactionFlagsStart = b.sFlags;
                article->_subactionMainStart = b.sMStart;
                article->_subactionGFXStart = b.sGStart;
                article->_subactionSFXStart = b.sSStart;
                article->_modelVisibility = b.visStart;
                article->_collisionData = b.off1;
                article->_unknownD2 = b.off2;
                article->_unknownD3 = b.off3;

                bint* ext = (bint*)((VoidPtr)article + 52);
                ext[0] = (b._subActions == null ? 0 : b._subActions.Children.Count);

                //Add all header offsets
                bint* off = (bint*)(addr + 12);
                for (int i = 0; i < 10 + b._extraOffsets.Count; i++)
                    if (off[i] > 1480 && off[i] < _root.dataSize)
                        b._lookupOffsets.Add(&off[i]);

                _lookupOffsets.AddRange(b._lookupOffsets);

                addr += b._entryLength;
            }

            FDefListOffset* header = (FDefListOffset*)addr;

            _rebuildAddr = header;

            if (Children.Count > 0)
            {
                header->_startOffset = (int)start - (int)RebuildBase;
                _lookupOffsets.Add(header->_startOffset.Address);
            }

            header->_listCount = Children.Count;
        }
    }
}
