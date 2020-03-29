using System;
using System.IO;

namespace BrawlLib.Internal.IO
{
    public unsafe class UnsafeStream : Stream
    {
        private readonly byte* _address;
        private readonly uint _length;
        private uint _position;

        public VoidPtr Address => _address;

        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => true;

        public override void Flush()
        {
        }

        public override long Length => _length;

        public override long Position
        {
            get => _position;
            set => _position = (uint) value.Clamp(0, _length);
        }

        public UnsafeStream(VoidPtr address, uint length)
        {
            _address = (byte*) address;
            _length = length;
            _position = 0;
        }

        public UnsafeStream(VoidPtr address, int length)
        {
            _address = (byte*) address;
            _length = (uint) length;
            _position = 0;
        }

        public override int ReadByte()
        {
            if (_position >= _length)
            {
                return -1;
            }

            return _address[_position++];
        }

        public int Read(byte* dst, int length)
        {
            uint size = Math.Min((uint) length, _length - _position);
            Memory.Move(dst, &_address[_position], size);
            _position += size;
            return (int) size;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (offset + count > buffer.Length)
            {
                throw new IndexOutOfRangeException();
            }

            fixed (byte* ptr = buffer)
            {
                return Read(&ptr[offset], count);
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                {
                    Position = offset;
                    break;
                }

                case SeekOrigin.Current:
                {
                    Position = _position + offset;
                    break;
                }

                case SeekOrigin.End:
                {
                    Position = _length + offset;
                    break;
                }
            }

            return _position;
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void WriteByte(byte value)
        {
            if (_position < _length)
            {
                _address[_position++] = value;
            }
        }

        public void Write(byte* src, int length)
        {
            uint size = Math.Min((uint) length, _length - _position);
            Memory.Move(_address, src, size);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (offset + count > buffer.Length)
            {
                throw new IndexOutOfRangeException();
            }

            fixed (byte* ptr = buffer)
            {
                Write(&ptr[offset], count);
            }
        }
    }
}