//-----------------------------------------------------------------------
// <copyright file="Win32Window.cs" company="None">
//     (c) 2013-2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models
{
    using System;
    using System.Windows;
    using System.Windows.Interop;
    using WinForms = System.Windows.Forms;

    /// <summary>
    /// Converter from <see cref="System.Windows.Window"/> to <see cref="System.Windows.Forms.IWin32Window"/>.
    /// </summary>
    public class Win32Window : WinForms.IWin32Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Win32Window"/> class.
        /// </summary>
        /// <param name="window">The instance of <see cref="System.Windows.Window"/>.</param>
        public Win32Window(Window window)
        {
            this.Handle = new WindowInteropHelper(window).Handle;
        }

        /// <summary>
        /// Gets the window handle for the current instance.
        /// </summary>
        public IntPtr Handle { get; private set; }
    }
}
