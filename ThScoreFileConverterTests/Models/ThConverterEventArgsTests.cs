using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
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

        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "ThScoreFileConverter.Models.ThConverterEventArgs")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThConverterEventArgsTestNullPath()
        {
            _ = new ThConverterEventArgs(null, 2, 5);
            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "ThScoreFileConverter.Models.ThConverterEventArgs")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThConverterEventArgsTestEmptyPath()
        {
            _ = new ThConverterEventArgs(string.Empty, 2, 5);
            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "ThScoreFileConverter.Models.ThConverterEventArgs")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThConverterEventArgsTestNegativeCurrent()
        {
            _ = new ThConverterEventArgs(@"path\to\file", -1, 5);
            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "ThScoreFileConverter.Models.ThConverterEventArgs")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThConverterEventArgsTestZeroCurrent()
        {
            _ = new ThConverterEventArgs(@"path\to\file", 0, 5);
            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "ThScoreFileConverter.Models.ThConverterEventArgs")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThConverterEventArgsTestExtendedCurrent()
        {
            _ = new ThConverterEventArgs(@"path\to\file", 6, 5);
            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "ThScoreFileConverter.Models.ThConverterEventArgs")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThConverterEventArgsTestNegativeTotal()
        {
            _ = new ThConverterEventArgs(@"path\to\file", 2, -1);
            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "ThScoreFileConverter.Models.ThConverterEventArgs")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThConverterEventArgsTestZeroTotal()
        {
            _ = new ThConverterEventArgs(@"path\to\file", 2, 0);
            Assert.Fail(TestUtils.Unreachable);
        }
    }
}
