using System.IO;

namespace BrawlLib.Internal.IO
{
    public static unsafe class StreamExtension
    {
        public static long AlignPosition(this Stream stream, int align)
        {
            return stream.Position = stream.Position.Align(align);
        }

        public static int Read(this Stream stream, VoidPtr dstAddr, int length)
        {
            byte[] arr = new byte[length];

            int numRead = stream.Read(arr, 0, length);

            fixed (byte* ptr = arr)
            {
                Memory.Move(dstAddr, ptr, (uint) numRead);
            }

            return numRead;
        }

        public static void Write(this Stream stream, VoidPtr srcAddr, int length)
        {
            byte[] arr = new byte[length];

            fixed (byte* ptr = arr)
            {
                Memory.Move(ptr, srcAddr, (uint) length);
            }

            stream.Write(arr, 0, length);
        }

        //public static FileView MapView(this FileStream stream)
        //{
        //    return MapView(stream, 0, (uint)stream.Length, FileMapProtect.ReadWrite);
        //}
        //public static FileView MapView(this FileStream stream, FileMapProtect protect)
        //{
        //    return MapView(stream, 0, (uint)stream.Length, protect);
        //}
        //public static FileView MapView(this FileStream stream, uint length, FileMapProtect protect)
        //{
        //    return MapView(stream, 0, length, protect);
        //}
        //public static FileView MapView(this FileStream stream, long position, uint length, FileMapProtect protect)
        //{
        //    return FileView.FromFileStream(stream, position, length, protect);
        //}

        //public static uint Read(this FileStream stream, VoidPtr address, uint length)
        //{
        //    if (!stream.CanRead)
        //        Util.InvokeStatic("System.IO.__Error", "ReadNotSupported");

        //    SafeFileHandle handle = stream.GetField<SafeFileHandle>("_handle");
        //    if (handle.IsClosed)
        //        Util.InvokeStatic("System.IO.__Error", "FileNotOpen");

        //    stream.Flush();
        //    if (stream.GetField<bool>("_exposedHandle"))
        //        stream.InvokeMethod("VerifyOSHandlePosition");

        //    uint numRead = 0;
        //    if (!ReadFile(handle, address, length, &numRead, null))
        //    {
        //        int hr = Marshal.GetLastWin32Error();
        //        if ((hr == 6) && !handle.IsInvalid)
        //            handle.Dispose();
        //        else if (hr == 0x6d)
        //            return 0;
        //        if (hr == 0x57)
        //            throw new IOException(Util.InvokeStatic<string>("System.Environment", "GetResourceString", "Arg_HandleNotSync"));

        //        Util.InvokeStatic("System.IO.__Error", "WinIOError", hr, string.Empty);
        //    }
        //    stream.SetField("_pos", stream.GetField<long>("_pos") + numRead);
        //    return numRead;
        //}
        //public static void Write(this FileStream stream, void* address, uint length)
        //{
        //    if (!stream.CanWrite)
        //        Util.InvokeStatic("System.IO.__Error", "WriteNotSupported");

        //    SafeFileHandle handle = stream.GetField<SafeFileHandle>("_handle");
        //    if (handle.IsClosed)
        //        Util.InvokeStatic("System.IO.__Error", "FileNotOpen");

        //    stream.Flush();
        //    if (stream.GetField<bool>("_exposedHandle"))
        //        stream.InvokeMethod("VerifyOSHandlePosition");

        //    uint numWritten = 0;
        //    if (!WriteFile(handle, address, length, &numWritten, null))
        //    {
        //        int hr = Marshal.GetLastWin32Error();
        //        if ((hr == 6) && !handle.IsInvalid)
        //            handle.Dispose();
        //        else if (hr == 0xe8)
        //            return;
        //        else if (hr == 0x57)
        //            throw new IOException(Util.InvokeStatic<string>("System.Environment", "GetResourceString", "IO.IO_FileTooLongOrHandleNotSync"));

        //        Util.InvokeStatic("System.IO.__Error", "WinIOError", hr, string.Empty);
        //    }
        //    stream.SetField("_pos", stream.GetField<long>("_pos") + numWritten);
        //}

        //[DllImport("Kernel32.dll", SetLastError = true)]
        //private static extern bool WriteFile(SafeFileHandle hFile, void* lpBuffer, uint nNumberOfBytesToWrite, uint* lpNumberOfBytesWritten, void* lpOverlapped);

        //[DllImport("Kernel32.dll", SetLastError = true)]
        //private static extern bool ReadFile(SafeFileHandle hFile, void* lpBuffer, uint nNumberOfBytesToRead, uint* lpNumberOfBytesRead, void* lpOverlapped);
    }
}