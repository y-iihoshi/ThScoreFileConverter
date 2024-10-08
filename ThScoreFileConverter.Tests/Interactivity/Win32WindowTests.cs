﻿using System;
using System.Windows;
using System.Windows.Interop;
using ThScoreFileConverter.Interactivity;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Interactivity;

[TestClass]
public class Win32WindowTests
{
    [SkipOrSTATestMethod]
    public void Win32WindowTest()
    {
        var window = new Window();
        var helper = new WindowInteropHelper(window);
        var handle = helper.EnsureHandle();

        var win32window = new Win32Window(window);

        Assert.AreEqual(handle, win32window.Handle);
    }

    [SkipOrSTATestMethod]
    public void Win32WindowTestDefault()
    {
        var window = new Window();

        var win32window = new Win32Window(window);

        Assert.AreEqual(IntPtr.Zero, win32window.Handle);
    }

    [TestMethod]
    public void Win32WindowTestNull()
    {
        var win32window = new Win32Window(null);

        Assert.AreEqual(IntPtr.Zero, win32window.Handle);
    }
}
