#region Usings

using System;
using System.Drawing;
using System.Runtime.InteropServices;

#endregion

namespace WinPos
{
    public static class GetWindowPlacementHelper
    {
        /// <summary>
        ///     Get the <see cref="WindowPlacement" /> for a specific window
        /// </summary>
        /// <param name="handle"><see cref="IntPtr" /> of window handle</param>
        /// <returns><see cref="WindowPlacement" /> details</returns>
        public static WindowPlacement GetPlacement( IntPtr handle )
        {
            var placement = new WindowPlacement();
            placement.Length = Marshal.SizeOf( placement );
            GetWindowPlacement( handle, ref placement );

            if ( placement.ShowCmd != 1 )
                return placement;

            // If ShowCmd == 1 SW_SHOWNORMAL position via GetWindowRect
            // This workaround is for better positioning with Aero Snap
            Rectangle rectangle;
            if ( GetWindowRect( handle, out rectangle ) )
                placement.RcNormalPosition = rectangle;
            return placement;
        }

        /// <summary>
        ///     Retrieves the show state and the restored, minimized, and maximized positions of the specified window.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        /// <param name="lpwndpl">
        ///     A pointer to the WINDOWPLACEMENT structure that receives the show state and position information.
        ///     <para>
        ///         Before calling GetWindowPlacement, set the length member to sizeof(WINDOWPLACEMENT). GetWindowPlacement fail if
        ///         lpwndpl-> length is not set correctly.
        ///     </para>
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is nonzero.
        ///     <para>
        ///         If the function fails, the return value is zero. To get extended error information, call GetLastError.
        ///     </para>
        /// </returns>
        [DllImport( "user32.dll" )]
        [return: MarshalAs( UnmanagedType.Bool )]
        private static extern Boolean GetWindowPlacement( IntPtr hWnd, ref WindowPlacement lpwndpl );

        [DllImport( "user32.dll" )]
        [return: MarshalAs( UnmanagedType.Bool )]
        private static extern Boolean GetWindowRect( IntPtr hWnd, out Rectangle lpRect );
    }
}