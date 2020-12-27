using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverterTests.Helpers
{
    [TestClass]
    public class IntegerHelperTests
    {
        [TestMethod]
        public void ToZeroBasedTest()
        {
            Assert.AreEqual(0, IntegerHelper.ToZeroBased(1));
            Assert.AreEqual(1, IntegerHelper.ToZeroBased(2));
            Assert.AreEqual(8, IntegerHelper.ToZeroBased(9));
            Assert.AreEqual(9, IntegerHelper.ToZeroBased(0));   // Hmm...
        }

        [TestMethod]
        public void ToZeroBasedTestNegative()
            => _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = IntegerHelper.ToZeroBased(-1));

        [TestMethod]
        public void ToZeroBasedTestExceeded()
            => _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = IntegerHelper.ToZeroBased(10));

        [TestMethod]
        public void ToOneBasedTest()
        {
            Assert.AreEqual(1, IntegerHelper.ToOneBased(0));
            Assert.AreEqual(2, IntegerHelper.ToOneBased(1));
            Assert.AreEqual(9, IntegerHelper.ToOneBased(8));
            Assert.AreEqual(0, IntegerHelper.ToOneBased(9));    // Hmm...
        }

        [TestMethod]
        public void ToOneBasedTestNegative()
            => _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = IntegerHelper.ToOneBased(-1));

        [TestMethod]
        public void ToOneBasedTestExceeded()
            => _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = IntegerHelper.ToOneBased(10));

        [TestMethod]
        public void IntegerHelperTest()
        {
            Assert.AreEqual(123, IntegerHelper.Parse("123"));
            _ = Assert.ThrowsException<ArgumentNullException>(() => IntegerHelper.Parse(null!));
            _ = Assert.ThrowsException<FormatException>(() => IntegerHelper.Parse(string.Empty));
            _ = Assert.ThrowsException<FormatException>(() => IntegerHelper.Parse("abc"));
        }
    }
}
