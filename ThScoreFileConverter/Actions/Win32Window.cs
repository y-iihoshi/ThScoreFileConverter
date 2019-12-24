//-----------------------------------------------------------------------
// <copyright file="Win32Window.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Interop;
using WinForms = System.Windows.Forms;

namespace ThScoreFileConverter.Actions
{
    /// <summary>
    /// Converter from <see cref="Window"/> to <see cref="WinForms.IWin32Window"/>.
    /// </summary>
    public class Win32Window : WinForms.IWin32Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Win32Window"/> class.
        /// </summary>
        /// <param name="window">The instance of <see cref="Window"/>.</param>
        public Win32Window(Window? window)
        {
            this.Handle = (window is null) ? IntPtr.Zero : new WindowInteropHelper(window).Handle;
        }

        /// <summary>
        /// Gets the window handle for the current instance.
        /// </summary>
        public IntPtr Handle { get; }
    }
}
