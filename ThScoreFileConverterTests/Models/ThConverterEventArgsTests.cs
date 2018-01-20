using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ThScoreFileConverter.Models.Tests
{
    [TestClass()]
    public class ThConverterEventArgsTests
    {
        [TestMethod()]
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

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "args")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThConverterEventArgsTestNullPath()
        {
            var args = new ThConverterEventArgs(null, 2, 5);
            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "args")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ThConverterEventArgsTestEmptyPath()
        {
            var args = new ThConverterEventArgs(string.Empty, 2, 5);
            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "args")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThConverterEventArgsTestNegativeCurrent()
        {
            var args = new ThConverterEventArgs(@"path\to\file", -1, 5);
            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "args")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThConverterEventArgsTestZeroCurrent()
        {
            var args = new ThConverterEventArgs(@"path\to\file", 0, 5);
            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "args")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThConverterEventArgsTestExtendedCurrent()
        {
            var args = new ThConverterEventArgs(@"path\to\file", 6, 5);
            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "args")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThConverterEventArgsTestNegativeTotal()
        {
            var args = new ThConverterEventArgs(@"path\to\file", 2, -1);
            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "args")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThConverterEventArgsTestZeroTotal()
        {
            var args = new ThConverterEventArgs(@"path\to\file", 2, 0);
            Assert.Fail(TestUtils.Unreachable);
        }
    }
}
