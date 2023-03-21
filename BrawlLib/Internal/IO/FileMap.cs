using BrawlLib.Platform;
using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace BrawlLib.Internal.IO
{
    public abstract class FileMap : IDisposable
    {
        protected VoidPtr _addr;
        protected int _length;
        protected string _path;
        protected FileStream _baseStream;

        public VoidPtr Address => _addr;

        public int Length
        {
            get => _length;
            set => _length = value;
        }

        public string FilePath => _path;

        public FileStream BaseStream => _baseStream;

        ~FileMap()
        {
            Dispose();
        }

        public virtual void Dispose()
        {
            if (_baseStream != null)
            {
                _baseStream.Close();
                _baseStream.Dispose();
                _baseStream = null;
            }

            //#if DEBUG
            //            Console.WriteLine("Closing file map: {0}", _path);
            //#endif
            GC.SuppressFinalize(this);
        }

        public static FileMap FromFile(string path)
        {
            return FromFile(path, FileMapProtect.ReadWrite, 0, 0);
        }

        public static FileMap FromFile(string path, FileMapProtect prot)
        {
            return FromFile(path, prot, 0, 0);
        }

        public static FileMap FromFile(string path, FileMapProtect prot, int offset, int length)
        {
            return FromFile(path, prot, 0, 0, FileOptions.RandomAccess);
        }

        public static FileMap FromFile(string path, FileMapProtect prot, int offset, int length, FileOptions options)
        {
            FileStream stream;
            FileMap map;

            try // Use a temp file in order to prevent writelocks
            {
                string tempPath = Path.GetTempFileName();
                File.Copy(path, tempPath, true);
                stream = new FileStream(tempPath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read, 8,
                    options | FileOptions.DeleteOnClose);
            }
            catch // Just open the file normally
            {
                stream = new FileStream(path, FileMode.Open,
                    prot == FileMapProtect.ReadWrite ? FileAccess.ReadWrite : FileAccess.Read, FileShare.Read, 8,
                    options);
            }

            try
            {
                map = FromStreamInternal(stream, prot, offset, length);
            }
            catch (Exception)
            {
                stream.Dispose();
                throw;
            }

            map._path = path; // Preserve true path
            return map;
        }

        public static FileMap FromTempFile(int length)
        {
            return FromTempFile(length, out string path);
        }

        public static FileMap FromTempFile(int length, out string path)
        {
            path = Path.GetTempFileName();
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite,
                FileShare.Read, 8, FileOptions.RandomAccess | FileOptions.DeleteOnClose);
            try
            {
                return FromStreamInternal(stream, FileMapProtect.ReadWrite, 0, length);
            }
            catch (Exception)
            {
                stream.Dispose();
                throw;
            }
        }

        public static FileMap FromStream(FileStream stream)
        {
            return FromStream(stream, FileMapProtect.ReadWrite, 0, 0);
        }

        public static FileMap FromStream(FileStream stream, FileMapProtect prot)
        {
            return FromStream(stream, prot, 0, 0);
        }

        public static FileMap FromStream(FileStream stream, FileMapProtect prot, int offset, int length)
        {
            //FileStream newStream = new FileStream(stream.Name, FileMode.Open, prot == FileMapProtect.Read ? FileAccess.Read : FileAccess.ReadWrite, FileShare.Read, 8, FileOptions.RandomAccess);
            //try { return FromStreamInternal(newStream, prot, offset, length); }
            //catch (Exception x) { newStream.Dispose(); throw x; }

            if (length == 0)
            {
                length = (int) stream.Length;
            }

            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    return new wFileMap(stream.SafeFileHandle.DangerousGetHandle(), prot, offset, (uint) length)
                        {_path = stream.Name};
                default:
                    return new cFileMap(stream, prot, offset, length) {_path = stream.Name};
            }

            //#if DEBUG
            //            Console.WriteLine("Opening file map: {0}", stream.Name);
            //#endif
        }

        public static FileMap FromStreamInternal(FileStream stream, FileMapProtect prot, int offset, int length)
        {
            if (length == 0)
            {
                length = (int) stream.Length;
            }

            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    return new wFileMap(stream.SafeFileHandle.DangerousGetHandle(), prot, offset, (uint) length)
                        {_baseStream = stream, _path = stream.Name};
                default:
                    return new cFileMap(stream, prot, offset, length) {_baseStream = stream, _path = stream.Name};
            }

            //#if DEBUG
            //            Console.WriteLine("Opening file map: {0}", stream.Name);
            //#endif
        }
    }

    public enum FileMapProtect : uint
    {
        Read = 0x01,
        ReadWrite = 0x02
    }

    public class wFileMap : FileMap
    {
        internal wFileMap(VoidPtr hFile, FileMapProtect protect, long offset, uint length)
        {
            long maxSize = offset + length;
            uint maxHigh = (uint) (maxSize >> 32);
            uint maxLow = (uint) maxSize;
            Win32._FileMapProtect mProtect;
            Win32._FileMapAccess mAccess;
            if (protect == FileMapProtect.ReadWrite)
            {
                mProtect = Win32._FileMapProtect.ReadWrite;
                mAccess = Win32._FileMapAccess.Write;
            }
            else
            {
                mProtect = Win32._FileMapProtect.ReadOnly;
                mAccess = Win32._FileMapAccess.Read;
            }

            if (length > 0)
            {
                using (Win32.SafeHandle h = Win32.CreateFileMapping(hFile, null, mProtect, maxHigh, maxLow, null))
                {
                    h.ErrorCheck();
                    _addr = Win32.MapViewOfFile(h.Handle, mAccess, (uint)(offset >> 32), (uint)offset, length);
                    if (!_addr)
                    {
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                    }

                    _length = (int)length;
                }
            }
        }

        public override void Dispose()
        {
            if (_addr)
            {
                Win32.FlushViewOfFile(_addr, 0);
                Win32.UnmapViewOfFile(_addr);
                _addr = null;
            }

            base.Dispose();
        }
    }


    public class cFileMap : FileMap
    {
        protected MemoryMappedFile _mappedFile;
        protected MemoryMappedViewAccessor _mappedFileAccessor;

        public cFileMap(FileStream stream, FileMapProtect protect, int offset, int length)
        {
            MemoryMappedFileAccess cProtect = protect == FileMapProtect.ReadWrite
                ? MemoryMappedFileAccess.ReadWrite
                : MemoryMappedFileAccess.Read;
            _length = length;
            _mappedFile = MemoryMappedFile.CreateFromFile(stream, stream.Name, _length, cProtect, null,
                HandleInheritability.None, true);
            _mappedFileAccessor = _mappedFile.CreateViewAccessor(offset, _length, cProtect);
            _addr = _mappedFileAccessor.SafeMemoryMappedViewHandle.DangerousGetHandle();
        }

        public override void Dispose()
        {
            _mappedFile?.Dispose();

            _mappedFileAccessor?.Dispose();

            base.Dispose();
        }
    }
}