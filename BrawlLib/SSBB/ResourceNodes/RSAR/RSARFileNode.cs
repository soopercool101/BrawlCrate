using System;
using System.IO;
using BrawlLib.IO;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSARFileNode : NW4RNode
    {
        internal VoidPtr Data { get { return (VoidPtr)WorkingUncompressed.Address; } }
        
        public DataSource _audioSource;
        
        internal RSARNode RSARNode
        {
            get
            {
                ResourceNode n = this;
                while (((n = n.Parent) != null) && !(n is RSARNode)) ;
                return n as RSARNode;
            }
        }

        public virtual void GetName()
        {
            string closestMatch = "";
            foreach (string s in GroupRefs)
            {
                if (closestMatch == "")
                    closestMatch = s;
                else
                {
                    int one = closestMatch.Length;
                    int two = s.Length;
                    int min = Math.Min(one, two);
                    for (int i = 0; i < min; i++)
                        if (Char.ToLower(s[i]) != Char.ToLower(closestMatch[i]) && i > 1)
                        {
                            closestMatch = closestMatch.Substring(0, i - 1);
                            break;
                        }
                }
            }
            _name = String.Format("[{0}] {1}", _fileIndex, closestMatch);
        }

        public List<RSARGroupNode> _groupRefs = new List<RSARGroupNode>();
        [Browsable(false)]
        public RSARGroupNode[] GroupRefNodes { get { return _groupRefs.ToArray(); } }

        public virtual string[] GroupRefs { get { return _groupRefs.Select(x => x.TreePath).ToArray(); } }

        public List<string> _references = new List<string>();
        public virtual string[] EntryRefs { get { return _references.ToArray(); } }

        public List<RSARSoundNode> _rsarSoundEntries = new List<RSARSoundNode>();
        [Browsable(false)]
        public RSARSoundNode[] Sounds { get { return _rsarSoundEntries.ToArray(); } }
        public void AddSoundRef(RSARSoundNode n)
        {
            if (!_rsarSoundEntries.Contains(n))
            {
                _rsarSoundEntries.Add(n);
                _references.Add(n.TreePath);
            }
        }
        public void RemoveSoundRef(RSARSoundNode n)
        {
            if (_rsarSoundEntries.Contains(n))
            {
                _rsarSoundEntries.Remove(n);
                _references.Remove(n.TreePath);
            }
        }

        internal int _fileIndex;
        internal LabelItem[] _labels;

        [Category("File Node"), Browsable(true)]
        public virtual int FileNodeIndex { get { return _fileIndex; } }
        
        internal int _entryNumber;
        [Category("Data"), Browsable(false)]
        public int EntryNumber
        {
            get { return _entryNumber; }
        }

        [Category("Data"), Browsable(false)]
        public virtual string InfoHeaderOffset { get { if (RSARNode != null && _infoHdr != null) return ((uint)(_infoHdr - (VoidPtr)RSARNode.Header)).ToString("X"); else return "0"; } }
        public VoidPtr _infoHdr;

        [Category("Data"), Browsable(true)]
        public virtual string AudioLength { get { return _audioSource.Length.ToString("X"); } }
        [Category("Data"), Browsable(false)]
        public virtual string AudioOffset { get { if (RSARNode != null && _audioSource.Address != null) return ((uint)(_audioSource.Address - (VoidPtr)RSARNode.Header)).ToString("X"); else return "0"; } }
        [Category("Data"), Browsable(true)]
        public virtual string DataLength { get { return WorkingUncompressed.Length.ToString("X"); } }
        [Category("Data"), Browsable(false)]
        public virtual string DataOffset { get { if (RSARNode != null && Data != null) return ((uint)(Data - (VoidPtr)RSARNode.Header)).ToString("X"); else return "0"; } }

        protected virtual void GetStrings(LabelBuilder builder) { }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            if (!_replaced)
                _groupRefs = new List<RSARGroupNode>();

            if (_name == null)
                if (_parent == null)
                    _name = Path.GetFileNameWithoutExtension(_origPath);
                else
                    _name = String.Format("[{0}] {1}", _fileIndex, ResourceType.ToString());

            return false;
        }

        public VoidPtr _rebuildAudioAddr = null;
        public int _headerLen = 0, _audioLen = 0;

        public override unsafe void Export(string outPath)
        {
            LabelBuilder labl;
            int lablLen, size;
            VoidPtr addr;

            Rebuild();

            //Get strings
            labl = new LabelBuilder();
            GetStrings(labl);
            lablLen = (labl.Count == 0) ? 0 : labl.GetSize();
            size = WorkingUncompressed.Length + lablLen + _audioSource.Length;

            using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                stream.SetLength(size);
                using (FileMap map = FileMap.FromStreamInternal(stream, FileMapProtect.ReadWrite, 0, size))
                {
                    addr = map.Address;

                    //Write headers
                    MoveRawUncompressed(addr, WorkingUncompressed.Length);
                    addr += WorkingUncompressed.Length;

                    //Write strings
                    if (lablLen > 0)
                        labl.Write(addr);
                    addr += lablLen;

                    //Write sound data
                    int audioLen = _audioSource.Length;
                    Memory.Move(addr, _audioSource.Address, (uint)audioLen);
                    _audioSource.Close();
                    _audioSource = new DataSource(addr, audioLen);
                }
            }
        }

        internal protected virtual void PostProcess(VoidPtr audioAddr, VoidPtr dataAddr)
        {

        }
    }
}
