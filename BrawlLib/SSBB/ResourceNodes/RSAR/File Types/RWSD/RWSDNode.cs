using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using BrawlLib.SSBB.Types.Audio;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RWSDNode : RSARFileNode
    {
        internal RWSDHeader* Header => (RWSDHeader*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.RWSD;

        public void InitGroups()
        {
            RWSDDataGroupNode group0 = new RWSDDataGroupNode
            {
                Parent = this
            };
            RWSDSoundGroupNode group1 = new RWSDSoundGroupNode
            {
                Parent = this
            };
        }

        protected override void GetStrings(LabelBuilder builder)
        {
            foreach (RWSDDataNode node in Children[0].Children)
            {
                builder.Add((uint) node.Index, node._name);
            }
        }

        private void ParseBlocks()
        {
            VoidPtr dataAddr = Header;
            int len = Header->_header._length;
            int total = WorkingUncompressed.Length;

            SetSizeInternal(len);

            //Look for labl block
            LABLHeader* labl = (LABLHeader*) (dataAddr + len);
            if (total > len && labl->_tag == LABLHeader.Tag)
            {
                int count = labl->_numEntries;
                _labels = new LabelItem[count];
                count = labl->_numEntries;
                for (int i = 0; i < count; i++)
                {
                    LABLEntry* entry = labl->Get(i);
                    _labels[i] = new LabelItem {String = entry->Name, Tag = entry->_id};
                }

                len += labl->_size;
            }

            //Set data source
            if (total > len)
            {
                _audioSource = new DataSource(dataAddr + len, total - len);
            }
        }

        public override void GetName()
        {
            base.GetName();
            //string closestOverallMatch = "";
            foreach (RWSDDataNode n in Children[0].Children)
            {
                n.GetName();

                //if (closestOverallMatch == "")
                //    closestOverallMatch = n._name;
                //else
                //{
                //    int one = closestOverallMatch.Length;
                //    int two = n._name.Length;
                //    int min = Math.Min(one, two);
                //    for (int i = 0; i < min; i++)
                //        if (Char.ToLower(n._name[i]) != Char.ToLower(closestOverallMatch[i]) && i > 1)
                //        {
                //            closestOverallMatch = closestOverallMatch.Substring(0, i - 1);
                //            break;
                //        }
                //}
            }

            //_name = String.Format("[{0}] {1}", _fileIndex, closestOverallMatch);
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            ParseBlocks();

            return true;
        }

        public override void OnPopulate()
        {
            RWSDHeader* rwsd = Header;
            RWSD_DATAHeader* data = rwsd->Data;
            RuintList* list = &data->_list;
            int count = list->_numEntries;

            new RWSDDataGroupNode().Initialize(this, Header->Data, Header->_dataLength);
            if (Header->_waveOffset > 0 && VersionMinor < 3)
            {
                new RWSDSoundGroupNode().Initialize(this, Header->Wave, Header->_waveLength);
            }
            else if (VersionMinor >= 3)
            {
                new RWARNode {_name = "Audio"}.Initialize(this, _audioSource.Address, _audioSource.Length);
            }

            //Get labels
            RSARNode parent;
            if (_labels == null && (parent = RSARNode) != null)
            {
                //Get them from RSAR
                SYMBHeader* symb2 = parent.Header->SYMBBlock;
                INFOHeader* info = parent.Header->INFOBlock;

                VoidPtr offset = &info->_collection;
                RuintList* soundList = info->Sounds;
                int count2 = soundList->_numEntries;

                _labels = new LabelItem[count2];

                INFOSoundEntry* entry;
                for (uint i = 0; i < count2; i++)
                {
                    if ((entry = (INFOSoundEntry*) soundList->Get(offset, (int) i))->_fileId == _fileIndex)
                    {
                        int x = ((WaveSoundInfo*) entry->GetSoundInfoRef(offset))->_soundIndex;
                        if (x >= 0 && x < count2)
                        {
                            _labels[x] = new LabelItem {Tag = i, String = symb2->GetStringEntry(entry->_stringId)};
                        }
                    }
                }
            }

            for (int i = 0; i < count; i++)
            {
                string name = "Entry" + i;
                if (_labels != null && i < _labels.Length)
                {
                    name = _labels[i].String;
                }

                RWSD_DATAEntry* entry = (RWSD_DATAEntry*) list->Get(list, i);
                RWSDDataNode node = new RWSDDataNode {_name = name};
                node._offset = list;
                node.Initialize(Children[0], entry, 0);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            _audioLen = 0;
            _headerLen = RWSDHeader.Size;
            if (VersionMinor >= 3)
            {
                _headerLen += Children[0].CalculateSize(true);
                _audioLen = Children[1].CalculateSize(true);
            }
            else
            {
                foreach (ResourceNode g in Children)
                {
                    _headerLen += g.CalculateSize(true);
                }

                foreach (WAVESoundNode s in Children[1].Children)
                {
                    _audioLen += s._streamBuffer.Length;
                }
            }

            return _headerLen + _audioLen;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            VoidPtr addr = address + 0x20;

            RWSDHeader* header = (RWSDHeader*) address;
            header->_header._length = _headerLen;
            header->_header._tag = RWSDHeader.Tag;
            header->_header._numEntries = (ushort) (VersionMinor >= 3 ? 1 : 2);
            header->_header._firstOffset = 0x20;
            header->_header.Endian = Endian.Big;
            header->_header._version = (ushort) (0x100 + VersionMinor);
            header->_dataOffset = 0x20;
            header->_dataLength = Children[0]._calcSize;
            header->_waveOffset = 0x20 + Children[0]._calcSize;
            header->_waveLength = Children[1]._calcSize;

            Children[0].Rebuild(addr, Children[0]._calcSize, true);
            addr += Children[0]._calcSize;

            SetSizeInternal(_headerLen);

            if (VersionMinor <= 2)
            {
                header->_waveOffset = 0x20 + Children[0]._calcSize;
                header->_waveLength = Children[1]._calcSize;

                VoidPtr audio = addr;
                if (RSARNode == null)
                {
                    audio += Children[1]._calcSize;
                }
                else
                {
                    audio = _rebuildAudioAddr;
                }

                (Children[1] as RWSDSoundGroupNode)._audioAddr = audio;
                _audioSource = new DataSource(audio, _audioLen);

                Children[1].Rebuild(addr, Children[1]._calcSize, true);
                addr += Children[1]._calcSize;
            }
            else
            {
                header->_waveOffset = 0;
                header->_waveLength = 0;

                VoidPtr audio = addr;
                if (RSARNode != null)
                {
                    audio = _rebuildAudioAddr;
                }

                _audioSource = new DataSource(audio, _audioLen);
                Children[1].Rebuild(audio, Children[1]._calcSize, true);
            }
        }

        public override void Remove()
        {
            RSARNode?.Files.Remove(this);

            base.Remove();
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((RWSDHeader*) source.Address)->_header._tag == RWSDHeader.Tag ? new RWSDNode() : null;
        }
    }
}