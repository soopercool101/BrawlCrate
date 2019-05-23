using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using System.IO;
using System.Collections.Generic;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSARExtFileNode : RSARFileNode
    {
        internal INFOFileHeader* Header { get { return (INFOFileHeader*)WorkingUncompressed.Address; } }

        [Category("Data"), Browsable(true)]
        public override string DataOffset { get { return "0"; } }
        [Category("Data"), Browsable(true)]
        public override string InfoHeaderOffset { get { if (RSARNode != null && Header != null) return ((uint)(Header - (VoidPtr)RSARNode.Header)).ToString("X"); else return "0"; } }

        public uint _extFileSize = 0;
        internal string _extPath;

        [Browsable(false)]
        public string ExtPath { get { return _extPath; } set { _extPath = value; SignalPropertyChange(); } }
        [Browsable(false)]
        public string FullExtPath
        {
            get { return ExtPath == null ? null : RootNode._origPath.Substring(0, RootNode._origPath.LastIndexOf('\\')) + "\\" + ExtPath.Replace('/', '\\'); }
            set
            {
                if (!value.Contains(".") || !value.Contains("\\"))
                    _extPath = "";
                else
                    _extPath = value.Substring(RootNode._origPath.Substring(0, RootNode._origPath.LastIndexOf('\\')).Length + 1).Replace('\\', '/');

                SignalPropertyChange();
            }
        }
        [Browsable(false), TypeConverter(typeof(ExpandableObjectCustomConverter))]
        public FileInfo ExternalFileInfo
        {
            get { return FullExtPath == null ? null : new FileInfo(FullExtPath); }
        }
        public void GetExtSize()
        {
            if (ExternalFileInfo.Exists)
                _extFileSize = (uint)ExternalFileInfo.Length;
        }
        public override bool OnInitialize()
        {
            RSARNode parent = RSARNode;
            _extPath = Header->GetPath(&RSARNode.Header->INFOBlock->_collection);
            if (_name == null)
                _name = String.Format("[{0}] {1}", _fileIndex, _extPath);
            _extFileSize = Header->_headerLen;
            return false;
        }

        public List<RSARBankNode> _rsarBankEntries = new List<RSARBankNode>();
        internal void AddBankRef(RSARBankNode n)
        {
            if (!_rsarBankEntries.Contains(n))
            {
                _rsarBankEntries.Add(n);
                _references.Add(n.TreePath);
            }
        }

        public void RemoveBankRef(RSARBankNode n)
        {
            if (_rsarBankEntries.Contains(n))
            {
                _rsarBankEntries.Remove(n);
                _references.Remove(n.TreePath);
            }
        }
    }
}
