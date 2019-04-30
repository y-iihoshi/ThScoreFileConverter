using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Actions;
using ThScoreFileConverterTests.Models;

namespace ThScoreFileConverterTests.Actions
{
    [TestClass]
    public class Win32WindowTests
    {
        [TestMethod]
        public void Win32WindowTest()
        {
            var window = new Window();
            var win32window = new Win32Window(window);

            Assert.IsNotNull(win32window);
            Assert.AreNotEqual(0, win32window.Handle);
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "win32window")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Win32WindowTestNull()
        {
            var win32window = new Win32Window(null);

            Assert.Fail(TestUtils.Unreachable);
        }
    }
}
