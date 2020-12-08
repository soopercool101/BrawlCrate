using BrawlLib.Internal;
using BrawlLib.SSBB.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class ISONode : ISOEntryNode
    {
        internal static byte[] LoadedKey;

        internal ISOPartitionHeader* Header => (ISOPartitionHeader*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.DiscImage;
        public override string DataSize => "0x" + WorkingUncompressed.Map.BaseStream.Length.ToString("X");

        private string _gameName;
        public bool _isGC;
        protected ISOCommonPartInfo _partInfo;

        [Category("ISO Disc Image")]
        public string GameName
        {
            get => _gameName;
            set => _gameName = value.Substring(0, Math.Max(0x60, value.Length));
        }

        public override bool OnInitialize()
        {
            _name = Header->GameID;
            _gameName = Header->GameName;
            _partInfo = Get<ISOCommonPartInfo>(0x400);
            ISOPartLists p = Get<ISOPartLists>(0x40000);
            return p._partitionCount > 0 || p._channelCount > 0;
        }

        public override void OnPopulate()
        {
            ISOPartLists p = Get<ISOPartLists>(0x40000);
            int pCount = p._partitionCount;
            int total = p._channelCount + pCount;

            for (int i = 0; i < total; ++i)
            {
                long offset = i < pCount ? p.PartitionOffset + i * 8L : p.ChannelOffset + (i - pCount) * 8L;
                PartitionTableEntry e = Get<PartitionTableEntry>(offset);
                new ISOPartitionNode(e, i >= pCount).Create(this, e._offset * OffMult, 0x8000, false);
            }
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            ISOPartitionHeader* header = (ISOPartitionHeader*) source.Address;
            bool GCMatch = header->_tagGC == ISOPartitionHeader.GCTag;
            bool WiiMatch = header->_tagWii == ISOPartitionHeader.WiiTag;
            if ((GCMatch || WiiMatch) && (LoadedKey != null || LoadKey()))
            {
                return new ISONode {_isGC = GCMatch};
            }

            return null;
        }

        private static bool LoadKey()
        {
            if (File.Exists("key.bin"))
            {
                FileStream s = File.OpenRead("key.bin");
                if (s.Length == 16)
                {
                    LoadedKey = new byte[16];
                    s.Read(LoadedKey, 0, 16);
                    return true;
                }
            }

            MessageBox.Show("Unable to load key.bin");
            return false;
        }
    }

    public class ISOPartitionNode : ISOEntryNode, IBufferNode
    {
        internal VoidPtr Header => WorkingUncompressed.Address;

        public override ResourceType ResourceFileType => ResourceType.DiscImagePartition;

        private PartitionTableEntry.Type _type;
        private string _vcID;
        private RSAType _rsaType;
        private byte[] _rsaSig;
        private TMDInfo _tmd;
        private List<TMDEntry> _tmdEntries;
        private uint _cachedBlock = uint.MaxValue;
        private PartitionInfo _info;

        private byte[] _titleKey, _iv;
        //private ISOPartitionHeader _header;

        [Category("TMD")] public RSAType RSA => _rsaType;
        [Category("TMD")] public byte[] RSASig => _rsaSig;
        [Category("TMD")] public byte Version => _tmd._version;
        [Category("TMD")] public byte CaCrlVersion => _tmd._caCrlVersion;
        [Category("TMD")] public byte SignerCrlVersion => _tmd._signerCrlVersion;
        [Category("TMD")] public int SysVersionLo => _tmd._sysVersionLo;
        [Category("TMD")] public int SysVersionHi => _tmd._sysVersionHi;
        [Category("TMD")] public short TitleID0 => _tmd._titleID0;
        [Category("TMD")] public short TitleID1 => _tmd._titleID1;
        [Category("TMD")] public string TitleTag => _tmd._titleTag;
        [Category("TMD")] public int TitleType => _tmd._titleType;
        [Category("TMD")] public short GroupID => _tmd._groupID;
        [Category("TMD")] public int AccessRights => _tmd._accessRights;
        [Category("TMD")] public short TitleVersion => _tmd._titleVersion;
        [Category("TMD")] public short NumContents => _tmd._numContents;
        [Category("TMD")] public short BootIndex => _tmd._bootIndex;
        [Category("TMD")] public TMDEntry[] Entries => _tmdEntries != null ? _tmdEntries.ToArray() : null;
        [Category("TMD")] public byte[] TitleKey => _titleKey;
        [Category("TMD")] public byte[] IV => _iv;

        [Category("ISO Partition")]
        public PartitionTableEntry.Type PartitionType
        {
            get => _type;
            set
            {
                _type = value;
                SignalPropertyChange();
            }
        }

        [Category("ISO Partition")]
        public string VirtualConsoleID
        {
            get => _vcID;
            set
            {
                if (PartitionType == PartitionTableEntry.Type.VirtualConsole)
                {
                    _vcID = value.PadLeft(4, ' ').Substring(value.Length - 4, value.Length);
                    SignalPropertyChange();
                }
                else
                {
                    _vcID = "N/A";
                }
            }
        }

        public ISOPartitionNode(PartitionTableEntry entry, bool VC)
        {
            if (VC)
            {
                _vcID = entry.GameID;
                _type = PartitionTableEntry.Type.VirtualConsole;
            }
            else
            {
                _vcID = "N/A";
                _type = entry.PartitionType;
            }
        }

        public enum RSAType : uint
        {
            None = 0,
            RSA2048 = 0x00010001,
            RSA4096 = 0x00010000
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            _name = $"[{Index}] {PartitionType.ToString()}";
            _info = Get<PartitionInfo>(0x2A4, true);

            long tmdOffset = _info._tmdOffset * OffMult;
            uint sigType = Get<buint>(tmdOffset, true);
            _rsaType = (RSAType) sigType;
            if (sigType != 0)
            {
                tmdOffset += 4L;

                int size = 0x200 - ((int) sigType & 1) * 0x100;

                _rsaSig = GetBytes(tmdOffset, size, true);

                tmdOffset += size;
                tmdOffset = (tmdOffset + 64 - 1) & ~(64 - 1);

                _tmd = Get<TMDInfo>(tmdOffset, true);
                tmdOffset += TMDInfo.Size;

                _tmdEntries = new List<TMDEntry>();
                for (int i = 0; i < _tmd._numContents; ++i)
                {
                    _tmdEntries.Add(Get<TMDEntry>(tmdOffset + i * TMDEntry.Size, true));
                }

                byte[] encryptedKey = GetBytes(0x1BF, 16, true);
                _iv = GetBytes(0x1DC, 8, true);
                Array.Resize(ref _iv, 16);

                using (AesManaged aesAlg = new AesManaged())
                {
                    aesAlg.Key = ISONode.LoadedKey;
                    aesAlg.IV = _iv;
                    aesAlg.Mode = CipherMode.CBC;
                    aesAlg.Padding = PaddingMode.Zeros;

                    _titleKey = new byte[16];
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (MemoryStream msDecrypt = new MemoryStream(encryptedKey))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            csDecrypt.Read(_titleKey, 0, 16);
                        }
                    }
                }
            }

            //byte[] b = GetPartitionData(0, 0x440);
            //_header = ToStruct<ISOPartitionHeader>(b);

            //byte[] dol = GetPartitionData(_header._dolOffset * OffMult, 0x100);
            //DOLHeader dolHdr = ToStruct<DOLHeader>(dol);

            //byte[] fullDol = GetPartitionData(_header._dolOffset * OffMult, (int)dolHdr.GetSize());
            //UnsafeBuffer dolBuf = GetBuffer(fullDol);
            //_childBuffers.Add(dolBuf);
            //new DOLNode().Initialize(this, dolBuf.Address, dolBuf.Length);

            return true;
        }

        private const int BlockHeaderSize = 0x400;
        private const int BlockDataSize = 0x7C00;
        private const int BlockSize = BlockHeaderSize + BlockDataSize;

        public void GetDC(long nOffset, long nSize, out long decOffset, out long decSize)
        {
            if (_rsaType != RSAType.None)
            {
                decOffset = nOffset / BlockDataSize * BlockSize;
                decSize = (nSize / BlockDataSize + 1) * BlockSize + nOffset % BlockDataSize;
            }
            else
            {
                decOffset = nOffset;
                decSize = nSize;
            }
        }

        public byte[] DecryptBlock(uint block)
        {
            if (block == _cachedBlock)
            {
                return null;
            }

            byte[] blockData = GetBytes(_info._dataOffset + BlockSize * block, BlockSize, true);
            byte[] enc = blockData.SubArray(BlockHeaderSize, BlockDataSize);
            byte[] dec = new byte[BlockDataSize];

            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = _titleKey;
                aesAlg.IV = blockData.SubArray(0x3D0, 0x10);
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.Zeros;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(enc))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        csDecrypt.Read(dec, 0, BlockDataSize);
                    }
                }
            }

            _cachedBlock = block;

            return dec;
        }

        public byte[] GetPartitionData(long offset, int size)
        {
            if (_rsaType == RSAType.None)
            {
                return GetBytes(offset, size, true);
            }

            List<byte> buffer = new List<byte>(size);
            uint block = (uint) (offset / BlockDataSize);
            int cacheOffset = (int) (offset % BlockDataSize);
            int cacheSize;

            while (size > 0)
            {
                byte[] cache = DecryptBlock(block);

                cacheSize = size;
                if (cacheSize + cacheOffset > BlockDataSize)
                {
                    cacheSize = BlockDataSize - cacheOffset;
                }

                buffer.AddRange(cache.SubArray(cacheOffset, cacheSize));

                size -= cacheSize;
                cacheOffset = 0;

                block++;
            }

            return buffer.ToArray();
        }

        public bool IsValid()
        {
            return GetLength() > 0;
        }

        public VoidPtr GetAddress()
        {
            return WorkingUncompressed.Address;
        }

        public int GetLength()
        {
            return WorkingUncompressed.Length;
        }
    }

    public abstract unsafe class ISOEntryNode : ResourceNode
    {
        //ISOs are too big for file mapping,
        //so we need to use some special functions to get data from the base stream.
        //NOTE: some ISOs are too big even for streams (file size bigger than long.MaxValue)
        //Need to split data into stream chunks
        public override ResourceType ResourceFileType => ResourceType.DiscImageEntry;

        public long _rootOffset;

        //Store the buffers the children are initialized on so that they are not disposed of before the node is
        protected List<UnsafeBuffer> _childBuffers = new List<UnsafeBuffer>();

        public virtual string DataSize => "0x" + WorkingUncompressed.Length.ToString("X");
        protected ISONode ISORoot => RootNode as ISONode;
        protected long OffMult => ISORoot._isGC ? 0L : 4L;

        public T ToStruct<T>(byte[] bytes) where T : struct
        {
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T dataStruct = (T) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();

            return dataStruct;
        }

        public T Get<T>(long offset, bool relative = false) where T : struct
        {
            return ToStruct<T>(GetBytes(offset, Marshal.SizeOf(default(T)), relative));
        }

        public UnsafeBuffer GetBuffer(long offset, int size, bool relative = false)
        {
            return GetBuffer(GetBytes(offset, size, relative));
        }

        public UnsafeBuffer GetBuffer(byte[] bytes)
        {
            UnsafeBuffer buffer = new UnsafeBuffer(bytes.Length);
            fixed (byte* b = &bytes[0])
            {
                Memory.Move(buffer.Address, b, (uint) bytes.Length);
            }

            return buffer;
        }

        public byte[] GetBytes(long offset, int size, bool relative = false)
        {
            FileStream s = RootNode.WorkingUncompressed.Map.BaseStream;
            s.Seek(relative ? _rootOffset + offset : offset, SeekOrigin.Begin);
            byte[] data = new byte[size];
            s.Read(data, 0, size);
            return data;
        }

        public void Create(ISOEntryNode parent, long offset, int size, bool relativeOffset)
        {
            _rootOffset = relativeOffset ? parent._rootOffset + offset : offset;

            _parent = parent;
            UnsafeBuffer buffer = GetBuffer(_rootOffset, size);
            _parent = null;

            Initialize(parent, buffer.Address, buffer.Length);

            parent._childBuffers.Add(buffer);
        }

        public void CreateChild(ResourceNode child, long offset, int size, bool relativeOffset)
        {
            if (child is ISOEntryNode)
            {
                ((ISOEntryNode) child)._rootOffset = relativeOffset ? _rootOffset + offset : offset;
            }

            child._parent = this;
            UnsafeBuffer buffer = GetBuffer(_rootOffset, size);
            child._parent = null;

            Initialize(this, buffer.Address, buffer.Length);

            _childBuffers.Add(buffer);
        }
    }
}