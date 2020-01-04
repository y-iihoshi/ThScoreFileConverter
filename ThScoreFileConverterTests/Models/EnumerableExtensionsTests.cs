using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverterTests.Extensions;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class EnumerableExtensionsTests
    {
        [TestMethod]
        public void SkipLastTest()
        {
            var array = new int[] { 1, 2, 3, 4, 5 };
            var expected = array.Take(3);
            var actual = array.SkipLast(2);
            CollectionAssert.That.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SkipLastTestNull()
        {
            int[] array = null!;
            _ = array.SkipLast(2);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void SkipLastTestNegative()
        {
            var array = new int[] { 1, 2, 3, 4, 5 };
            var actual = array.SkipLast(-1);
            CollectionAssert.That.AreEqual(array, actual);
        }

        [TestMethod]
        public void SkipLastTestZero()
        {
            var array = new int[] { 1, 2, 3, 4, 5 };
            var actual = array.SkipLast(0);
            CollectionAssert.That.AreEqual(array, actual);
        }

        [TestMethod]
        public void SkipLastTestExceeded()
        {
            var array = new int[] { 1, 2, 3, 4, 5 };
            var expected = new int[] { };
            var actual = array.SkipLast(array.Length + 1);
            CollectionAssert.That.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TakeLastTest()
        {
            var array = new int[] { 1, 2, 3, 4, 5 };
            var expected = array.Skip(3);
            var actual = array.TakeLast(2);
            CollectionAssert.That.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TakeLastTestNull()
        {
            int[] array = null!;
            _ = array.TakeLast(2);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void TakeLastTestNegative()
        {
            var array = new int[] { 1, 2, 3, 4, 5 };
            var expected = new int[] { };
            var actual = array.TakeLast(-1);
            CollectionAssert.That.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TakeLastTestZero()
        {
            var array = new int[] { 1, 2, 3, 4, 5 };
            var expected = new int[] { };
            var actual = array.TakeLast(0);
            CollectionAssert.That.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TakeLastTestExceeded()
        {
            var array = new int[] { 1, 2, 3, 4, 5 };
            var actual = array.TakeLast(array.Length + 1);
            CollectionAssert.That.AreEqual(array, actual);
        }
    }
}
