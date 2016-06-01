#region Usings

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

#endregion

namespace WinPos
{
    /// <summary>Contains functionality to get all the open windows.</summary>
    public static class OpenWindowGetter
    {
        /// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
        /// <returns>A dictionary that contains the handle and title of all the open windows.</returns>
        public static IDictionary<IntPtr, String> GetOpenWindows()
        {
            var shellWindow = GetShellWindow();
            var windows = new Dictionary<IntPtr, String>();

            EnumWindows( delegate( IntPtr hWnd, Int32 lParam )
            {
                if ( hWnd == shellWindow )
                    return true;
                if ( !IsWindowVisible( hWnd ) )
                    return true;

                var length = GetWindowTextLength( hWnd );
                if ( length == 0 )
                    return true;

                var builder = new StringBuilder( length );
                GetWindowText( hWnd, builder, length + 1 );

                windows[hWnd] = builder.ToString();
                return true;
            },
                         0 );

            return windows;
        }

        [DllImport( "USER32.DLL" )]
        private static extern Boolean EnumWindows( EnumWindowsProc enumFunc, Int32 lParam );

        [DllImport( "USER32.DLL" )]
        private static extern IntPtr GetShellWindow();

        [DllImport( "USER32.DLL" )]
        private static extern Int32 GetWindowText( IntPtr hWnd, StringBuilder lpString, Int32 nMaxCount );

        [DllImport( "USER32.DLL" )]
        private static extern Int32 GetWindowTextLength( IntPtr hWnd );

        [DllImport( "USER32.DLL" )]
        private static extern Boolean IsWindowVisible( IntPtr hWnd );

        #region Nested Types

        private delegate Boolean EnumWindowsProc( IntPtr hWnd, Int32 lParam );

        #endregion
    }
}