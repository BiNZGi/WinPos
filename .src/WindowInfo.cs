#region Usings

using System;

#endregion

namespace WinPos
{
    public class WindowInfo
    {
        #region Properties

        public IntPtr Handle { set; get; }
        public String Title { set; get; }
        public WindowPlacement WindowPlacement { set; get; }

        #endregion
    }
}