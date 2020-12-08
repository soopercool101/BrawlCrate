using BrawlLib.Internal;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BrawlLib.Platform
{
    internal static partial class Win32
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class SafeHandle : IDisposable
        {
            private uint _handle;
            public VoidPtr Handle => _handle;

            public SafeHandle(VoidPtr handle)
            {
                _handle = handle;
            }

            ~SafeHandle()
            {
                Dispose();
            }

            public void Dispose()
            {
                if (_handle != 0)
                {
                    CloseHandle(_handle);
                    _handle = 0;
                }
            }

            public void ErrorCheck()
            {
                if (_handle == 0)
                {
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                }
            }

            public static implicit operator SafeHandle(VoidPtr handle)
            {
                return new SafeHandle(handle);
            }

            internal static SafeHandle Duplicate(VoidPtr hFile)
            {
                VoidPtr hProc = Process.GetCurrentProcess().Handle;
                if (!DuplicateHandle(hProc, hFile, hProc, out hFile, 0, false, 2))
                {
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                }

                return new SafeHandle(hFile);
            }
        }

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool CloseHandle(VoidPtr hObject);

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool DuplicateHandle(VoidPtr hSourceProcessHandle, VoidPtr hSourceHandle,
                                                  VoidPtr hTargetProcessHandle, out VoidPtr lpTargetHandle,
                                                  uint dwDesiredAccess, bool bInheritHandle, uint dwOptions);


        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
        public static extern void MoveMemory(VoidPtr dest, VoidPtr src, uint size);

        [DllImport("Kernel32.dll", EntryPoint = "RtlFillMemory", SetLastError = false)]
        public static extern void FillMemory(VoidPtr dest, uint length, byte value);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern VoidPtr GetDC(VoidPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(VoidPtr hWnd, VoidPtr hDC);

        #region File Mapping

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern VoidPtr CreateFileMapping(VoidPtr hFile, VoidPtr lpAttributes, _FileMapProtect flProtect,
                                                       uint dwMaximumSizeHigh, uint dwMaximumSizeLow, string lpName);

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool FlushViewOfFile(VoidPtr lpBaseAddress, uint dwNumberOfBytesToFlush);

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern VoidPtr MapViewOfFile(VoidPtr hFileMappingObject, _FileMapAccess dwDesiredAccess,
                                                   uint dwFileOffsetHigh, uint dwFileOffsetLow,
                                                   uint dwNumberOfBytesToMap);

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern VoidPtr MapViewOfFileEx(VoidPtr hFileMappingObject, _FileMapAccess dwDesiredAccess,
                                                     uint dwFileOffsetHigh, uint dwFileOffsetLow,
                                                     uint dwNumberOfBytesToMap, VoidPtr lpBaseAddress);

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern VoidPtr
            OpenFileMapping(_FileMapAccess dwDesiredAccess, bool bInheritHandle, string lpName);

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool UnmapViewOfFile(VoidPtr lpBaseAddress);


        //private class FILEMAP : IDisposable
        //{
        //    public VoidPtr _handle;

        //    //private FILEMAP(VoidPtr handle) { _handle = handle; }
        //    public FILEMAP(IntPtr hFile, VoidPtr lpAttributes, _FileMapProtect flProtect, uint dwMaximumSizeHigh, uint dwMaximumSizeLow, string lpName)
        //    {
        //        _handle = CreateFileMappingW(hFile, lpAttributes, flProtect, dwMaximumSizeHigh, dwMaximumSizeLow, lpName);
        //        if (!_handle)
        //            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        //    }
        //    ~FILEMAP() { Dispose(); }
        //    public void Dispose()
        //    {
        //        if (_handle)
        //        {
        //            CloseHandle(_handle);
        //            _handle = null;
        //        }
        //    }
        //    //public static FILEMAP Open(_FileMapAccess dwDesiredAccess, bool bInheritHandle, string lpName)
        //    //{
        //    //    VoidPtr _handle = OpenFileMappingW(dwDesiredAccess, bInheritHandle, lpName);
        //    //    if (!_handle)
        //    //        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        //    //    return new FILEMAP(_handle);
        //    //}
        //}

        [Flags]
        public enum _FileMapProtect : uint
        {
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,

            Commit = 0x8000000,
            Image = 0x1000000,
            LargePages = 0x80000000,
            NoCache = 0x10000000,
            Reserve = 0x4000000,
            WriteCombine = 0x40000000
        }

        [Flags]
        public enum _FileMapAccess : uint
        {
            Copy = 0x01,
            Write = 0x02,
            Read = 0x04,
            Execute = 0x20,
            All = 0x000F001F
        }

        #endregion
    }
}