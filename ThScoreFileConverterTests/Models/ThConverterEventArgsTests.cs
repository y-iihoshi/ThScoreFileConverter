using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class ThConverterEventArgsTests
    {
        [TestMethod]
        public void ThConverterEventArgsTest()
        {
            var args = new ThConverterEventArgs(@"path\to\file", 2, 5);

            Assert.AreEqual(@"path\to\file", args.Path);
            Assert.AreEqual(2, args.Current);
            Assert.AreEqual(5, args.Total);

            var message = args.Message;

            StringAssert.Contains(message, "file");
            StringAssert.Contains(message, "2");
            StringAssert.Contains(message, "5");
        }

        [TestMethod]
        public void ThConverterEventArgsTestDefault()
        {
            var args = new ThConverterEventArgs();

            Assert.AreEqual(string.Empty, args.Path);
            Assert.AreEqual(default, args.Current);
            Assert.AreEqual(default, args.Total);
        }

        [TestMethod]
        public void ThConverterEventArgsTestEmptyPath()
        {
            _ = Assert.ThrowsException<ArgumentException>(() => new ThConverterEventArgs(string.Empty, 2, 5));
        }

        [TestMethod]
        public void ThConverterEventArgsTestNegativeCurrent()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => new ThConverterEventArgs(@"path\to\file", -1, 5));
        }

        [TestMethod]
        public void ThConverterEventArgsTestZeroCurrent()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => new ThConverterEventArgs(@"path\to\file", 0, 5));
        }

        [TestMethod]
        public void ThConverterEventArgsTestExtendedCurrent()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => new ThConverterEventArgs(@"path\to\file", 6, 5));
        }

        [TestMethod]
        public void ThConverterEventArgsTestNegativeTotal()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => new ThConverterEventArgs(@"path\to\file", 2, -1));
        }

        [TestMethod]
        public void ThConverterEventArgsTestZeroTotal()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => new ThConverterEventArgs(@"path\to\file", 2, 0));
        }
    }
}
