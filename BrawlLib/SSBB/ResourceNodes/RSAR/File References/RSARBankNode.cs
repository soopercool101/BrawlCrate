using System;
using System.ComponentModel;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSARBankNode : RSAREntryNode
    {
        internal INFOBankEntry* Header => (INFOBankEntry*) WorkingUncompressed.Address;

#if DEBUG
        [Browsable(true), Category("DEBUG")]
#else
        [Browsable(false)]
#endif
        public override int StringId => Header == null ? -1 : (int) Header->_stringId;

        private RSARFileNode _rbnk;

        [Browsable(false)]
        public RSARFileNode BankNode
        {
            get => _rbnk;
            set
            {
                if (_rbnk != value)
                {
                    if (_rbnk != null)
                    {
                        if (_rbnk is RSARExtFileNode)
                        {
                            var ext = _rbnk as RSARExtFileNode;
                            ext.RemoveBankRef(this);
                        }
                        else if (_rbnk is RBNKNode)
                        {
                            var ext = _rbnk as RBNKNode;
                            ext.RemoveBankRef(this);
                        }

                        _rbnk = null;
                    }

                    if (value is RBNKNode || value is RSARExtFileNode)
                    {
                        _rbnk = value;
                        if (_rbnk is RSARExtFileNode)
                        {
                            var ext = _rbnk as RSARExtFileNode;
                            ext.AddBankRef(this);
                        }
                        else if (_rbnk is RBNKNode)
                        {
                            var ext = _rbnk as RBNKNode;
                            ext.AddBankRef(this);
                        }
                    }
                }

                SignalPropertyChange();
            }
        }

        [Category("INFO Bank")]
        [Browsable(true)]
        [TypeConverter(typeof(DropDownListBankFiles))]
        public string BankFile
        {
            get => _rbnk == null ? null : _rbnk._name;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    BankNode = null;
                }
                else
                {
                    var t = 0;
                    RSARFileNode node = null;
                    foreach (var r in RSARNode.Files)
                    {
                        if (r.Name == value && (r is RBNKNode || r is RSARExtFileNode))
                        {
                            node = r;
                            break;
                        }

                        t++;
                    }

                    if (node != null)
                    {
                        BankNode = node;
                        _fileId = t;
                    }
                }
            }
        }

        public override ResourceType ResourceFileType => ResourceType.RSARBank;

        public int _fileId;

        public override bool OnInitialize()
        {
            base.OnInitialize();

            _fileId = Header->_fileId;

            if (_fileId >= 0 && _fileId < RSARNode.Files.Count)
            {
                _rbnk = RSARNode.Files[_fileId];
                if (_rbnk is RSARExtFileNode)
                {
                    var ext = _rbnk as RSARExtFileNode;
                    ext.AddBankRef(this);
                }
                else if (_rbnk is RBNKNode)
                {
                    var ext = _rbnk as RBNKNode;
                    ext.AddBankRef(this);
                }
            }

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return INFOBankEntry.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            var header = (INFOBankEntry*) address;
            header->_stringId = _rebuildStringId;
            header->_fileId = _fileId;
            header->_padding = 0;
        }
    }
}