using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class SndBgmTitleDataNode : ResourceNode
    {
        public override ResourceType ResourceFileType => ResourceType.SndBgmTitleDataFolder;

        public override bool OnInitialize()
        {
            base.OnInitialize();

            return true;
        }

        public override void OnPopulate()
        {
            for (int i = 0; i < WorkingUncompressed.Length; i += sizeof(SndBgmTitleEntry))
            {
                DataSource source = new DataSource(WorkingUncompressed.Address + i, sizeof(SndBgmTitleEntry));
                new SndBgmTitleEntryNode().Initialize(this, source);
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            // Rebuild children using new address
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Rebuild(address + i * sizeof(SndBgmTitleEntry), sizeof(SndBgmTitleEntry), true);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            int size = 0;
            foreach (SndBgmTitleEntryNode node in Children)
            {
                size += node.CalculateSize(true);
            }

            return size;
        }

        public void CreateEntry()
        {
            int maxID = 0, maxSongIndex = 0;
            foreach (SndBgmTitleEntryNode entry in Children)
            {
                maxID = Math.Max(entry.ID, maxID);
                maxSongIndex = Math.Max(entry.SongTitleIndex, maxSongIndex);
            }

            SndBgmTitleEntryNode n = new SndBgmTitleEntryNode
            {
                ID = maxID + 1,
                SongTitleIndex = maxSongIndex + 1
            };
            AddChild(n);
        }
    }

    public unsafe class SndBgmTitleEntryNode : ResourceNode
    {
        [Browsable(false)] public SndBgmTitleDataNode Root => _parent as SndBgmTitleDataNode;

        [Browsable(true)] [Category("Entry")] public string Length => sizeof(SndBgmTitleEntry).ToString("x");

        //internal SndBgmTitleEntry* Header { get { return (SndBgmTitleEntry*)WorkingUncompressed.Address; } }
        public SndBgmTitleEntry Data;

        public override ResourceType ResourceFileType => ResourceType.SndBgmTitleDataEntry;

        [Category("Song")]
        [DisplayName("Song ID")]
        [Description("The ID of the song to show the title for.")]
        public string SongID
        {
            get => "0x" + ((int) Data._ID).ToString("X8");
            set
            {
                string field0 = (value ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase) ? 16 : 10;
                Data._ID = Convert.ToInt32(field0, fromBase);
                SignalPropertyChange();
                UpdateName();
            }
        }

        [Browsable(false)]
        public int ID
        {
            get => Data._ID;
            set
            {
                Data._ID = value;
                SignalPropertyChange();
                UpdateName();
            }
        }

        [Category("Song")]
        [DisplayName("Song Title Index")]
        [Description("The index of the song title in info.pac (MiscData[140]) and other files.")]
        public int SongTitleIndex
        {
            get => Data._SongTitleIndex;
            set
            {
                Data._SongTitleIndex = value;
                SignalPropertyChange();
                UpdateName();
            }
        }

        public SndBgmTitleEntryNode()
        {
            Data._unknown08 = 0x13;
            Data._unknown18 = -1;
            Data._unknown1c = -1;
            Data._unknown20 = -1;
            Data._unknown24 = -1;
            Data._unknown28 = -1;
            Data._unknown2c = 0x0B;
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            if (WorkingUncompressed.Length != sizeof(SndBgmTitleEntry))
            {
                throw new Exception("Wrong size for SndBgmTitleEntryNode");
            }

            // Copy the data from the address
            Data = *(SndBgmTitleEntry*) WorkingUncompressed.Address;

            if (_name == null)
            {
                bool changed = HasChanged;
                UpdateName();
                HasChanged = changed;
            }

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            // Copy the data back to the address
            *(SndBgmTitleEntry*) address = Data;
        }

        public override int OnCalculateSize(bool force)
        {
            // Constant size (48 bytes)
            return sizeof(SndBgmTitleEntry);
        }

        public void UpdateName()
        {
            Name = ID.ToString("X4") + " --> " + SongTitleIndex;
        }
    }
}