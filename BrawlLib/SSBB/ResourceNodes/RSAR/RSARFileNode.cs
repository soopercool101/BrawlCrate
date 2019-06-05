using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using BrawlLib.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSARFileNode : NW4RNode
    {
        public DataSource _audioSource;

        internal int _entryNumber;

        internal int _fileIndex;

        public List<RSARGroupNode> _groupRefs = new List<RSARGroupNode>();
        public int _headerLen = 0, _audioLen = 0;
        public VoidPtr _infoHdr;
        internal LabelItem[] _labels;

        public VoidPtr _rebuildAudioAddr = null;

        public List<string> _references = new List<string>();

        public List<RSARSoundNode> _rsarSoundEntries = new List<RSARSoundNode>();
        internal VoidPtr Data => WorkingUncompressed.Address;

        internal RSARNode RSARNode
        {
            get
            {
                ResourceNode n = this;
                while ((n = n.Parent) != null && !(n is RSARNode)) ;

                return n as RSARNode;
            }
        }

        [Browsable(false)] public RSARGroupNode[] GroupRefNodes => _groupRefs.ToArray();

        public virtual string[] GroupRefs => _groupRefs.Select(x => x.TreePath).ToArray();
        public virtual string[] EntryRefs => _references.ToArray();

        [Browsable(false)] public RSARSoundNode[] Sounds => _rsarSoundEntries.ToArray();

        [Category("File Node")]
        [Browsable(true)]
        public virtual int FileNodeIndex => _fileIndex;

        [Category("Data")] [Browsable(false)] public int EntryNumber => _entryNumber;

        [Category("Data")]
        [Browsable(false)]
        public virtual string InfoHeaderOffset
        {
            get
            {
                if (RSARNode != null && _infoHdr != null)
                    return ((uint) (_infoHdr - RSARNode.Header)).ToString("X");
                return "0";
            }
        }

        [Category("Data")] [Browsable(true)] public virtual string AudioLength => _audioSource.Length.ToString("X");

        [Category("Data")]
        [Browsable(false)]
        public virtual string AudioOffset
        {
            get
            {
                if (RSARNode != null && _audioSource.Address != null)
                    return ((uint) (_audioSource.Address - RSARNode.Header)).ToString("X");
                return "0";
            }
        }

        [Category("Data")]
        [Browsable(true)]
        public virtual string DataLength => WorkingUncompressed.Length.ToString("X");

        [Category("Data")]
        [Browsable(false)]
        public virtual string DataOffset
        {
            get
            {
                if (RSARNode != null && Data != null)
                    return ((uint) (Data - RSARNode.Header)).ToString("X");
                return "0";
            }
        }

        public virtual void GetName()
        {
            var closestMatch = "";
            foreach (var s in GroupRefs)
                if (closestMatch == "")
                {
                    closestMatch = s;
                }
                else
                {
                    var one = closestMatch.Length;
                    var two = s.Length;
                    var min = Math.Min(one, two);
                    for (var i = 0; i < min; i++)
                        if (char.ToLower(s[i]) != char.ToLower(closestMatch[i]) && i > 1)
                        {
                            closestMatch = closestMatch.Substring(0, i - 1);
                            break;
                        }
                }

            _name = string.Format("[{0}] {1}", _fileIndex, closestMatch);
        }

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

        protected virtual void GetStrings(LabelBuilder builder)
        {
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            if (!_replaced) _groupRefs = new List<RSARGroupNode>();

            if (_name == null)
            {
                if (_parent == null)
                    _name = Path.GetFileNameWithoutExtension(_origPath);
                else
                    _name = string.Format("[{0}] {1}", _fileIndex, ResourceFileType.ToString());
            }

            return false;
        }

        public override void Export(string outPath)
        {
            LabelBuilder labl;
            int lablLen, size;
            VoidPtr addr;

            Rebuild();

            //Get strings
            labl = new LabelBuilder();
            GetStrings(labl);
            lablLen = labl.Count == 0 ? 0 : labl.GetSize();
            size = WorkingUncompressed.Length + lablLen + _audioSource.Length;

            using (var stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                stream.SetLength(size);
                using (var map = FileMap.FromStreamInternal(stream, FileMapProtect.ReadWrite, 0, size))
                {
                    addr = map.Address;

                    //Write headers
                    MoveRawUncompressed(addr, WorkingUncompressed.Length);
                    addr += WorkingUncompressed.Length;

                    //Write strings
                    if (lablLen > 0) labl.Write(addr);

                    addr += lablLen;

                    //Write sound data
                    var audioLen = _audioSource.Length;
                    Memory.Move(addr, _audioSource.Address, (uint) audioLen);
                    _audioSource.Close();
                    _audioSource = new DataSource(addr, audioLen);
                }
            }
        }

        protected internal virtual void PostProcess(VoidPtr audioAddr, VoidPtr dataAddr)
        {
        }
    }
}