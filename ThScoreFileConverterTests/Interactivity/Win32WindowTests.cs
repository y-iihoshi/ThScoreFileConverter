using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Actions;
using ThScoreFileConverterTests.Models;

namespace ThScoreFileConverterTests.Interactivity
{
    [TestClass]
    public class Win32WindowTests
    {
        [TestMethod]
        public void Win32WindowTest()
        {
            var window = new Window();
            var helper = new WindowInteropHelper(window);
            var handle = helper.EnsureHandle();

            var win32window = new Win32Window(window);

            Assert.AreEqual(handle, win32window.Handle);
        }

        [TestMethod]
        public void Win32WindowTestDefault()
        {
            var window = new Window();

            var win32window = new Win32Window(window);

            Assert.AreEqual(IntPtr.Zero, win32window.Handle);
        }

        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "ThScoreFileConverter.Actions.Win32Window")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Win32WindowTestNull()
        {
            _ = new Win32Window(null);

            Assert.Fail(TestUtils.Unreachable);
        }
    }
}
