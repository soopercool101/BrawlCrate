using System;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Forms.Ookii.Dialogs.Interop
{
    internal class WindowHandleWrapper : IWin32Window
    {
        public WindowHandleWrapper(IntPtr handle)
        {
            Handle = handle;
        }

        #region IWin32Window Members

        public IntPtr Handle { get; }

        #endregion
    }
}