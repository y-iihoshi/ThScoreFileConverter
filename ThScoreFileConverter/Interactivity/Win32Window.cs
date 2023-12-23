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

namespace ThScoreFileConverter.Interactivity;

/// <summary>
/// Converter from <see cref="Window"/> to <see cref="WinForms.IWin32Window"/>.
/// </summary>
/// <param name="window">The instance of <see cref="Window"/>.</param>
public class Win32Window(Window? window) : WinForms.IWin32Window
{
    /// <summary>
    /// Gets the window handle for the current instance.
    /// </summary>
    public IntPtr Handle { get; } = (window is null) ? IntPtr.Zero : new WindowInteropHelper(window).Handle;
}
