#region Usings

using System;
using System.Runtime.InteropServices;

#endregion

namespace WinPos
{
    public static class SetWindowPlacementHelper
    {
        /// <summary>
        ///     Sets the placement of window
        /// </summary>
        /// <param name="windowInfo"><see cref="WindowInfo" /> with the details to set</param>
        public static Boolean SetPlacement( WindowInfo windowInfo )
        {
            var windowPlacement = windowInfo.WindowPlacement;
            return SetWindowPlacement( windowInfo.Handle, ref windowPlacement );
        }

        // Import the DLL
        [DllImport( "user32.dll" )]
        private static extern Boolean SetWindowPlacement( IntPtr hWnd, [In] ref WindowPlacement lpwndpl );
    }
}