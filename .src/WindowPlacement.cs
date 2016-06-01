#region Usings

using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.InteropServices;

#endregion

namespace WinPos
{
    // Definition for Window Placement Structure
    [StructLayout( LayoutKind.Sequential )]
    [SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" )]
    public struct WindowPlacement
    {
        public Int32 Length;
        public Int32 Flags;
        public Int32 ShowCmd;
        public Point PtMinPosition;
        public Point PtMaxPosition;
        public Rectangle RcNormalPosition;
    }
}