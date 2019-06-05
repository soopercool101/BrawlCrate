using System;
using System.ComponentModel;
using System.IO;
using BrawlLib.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public interface IBufferNode
    {
        bool IsValid();
        VoidPtr GetAddress();
        int GetLength();
    }

    public unsafe class RawDataNode : ResourceNode, IBufferNode
    {
        public UnsafeBuffer _buffer;

        public RawDataNode()
        {
        }

        public RawDataNode(string name)
        {
            _name = name;
        }

        internal byte* Header => (byte*) WorkingUncompressed.Address;

        public int Size => WorkingUncompressed.Length;

        [Browsable(true)]
        [TypeConverter(typeof(DropDownListCompression))]
        public override string Compression
        {
            get => base.Compression;
            set => base.Compression = value;
        }

        public VoidPtr GetAddress()
        {
            return _buffer.Address;
        }

        public int GetLength()
        {
            return _buffer.Length;
        }

        public bool IsValid()
        {
            return _buffer != null && _buffer.Length > 0;
        }

        public override bool OnInitialize()
        {
            _buffer = new UnsafeBuffer(WorkingUncompressed.Length);

            Memory.Move(_buffer.Address, Header, (uint) _buffer.Length);

            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return _buffer.Length;
        }

        public override void Export(string outPath)
        {
            using (var stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 8,
                FileOptions.SequentialScan))
            {
                stream.SetLength(_buffer.Length);
                using (var map = FileMap.FromStream(stream))
                {
                    Memory.Move(map.Address, _buffer.Address, (uint) _buffer.Length);
                }
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            var header = address;
            Memory.Move(header, _buffer.Address, (uint) length);
        }
    }
}