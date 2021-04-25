using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverterTests.Helpers
{
    [TestClass]
    public class DateTimeHelperTests
    {
        [TestMethod]
        public void UnixEpochTest()
        {
            var expected = new DateTime(1970, 1, 1);
            Assert.AreEqual(expected, DateTimeHelper.UnixEpoch);
        }

        [TestMethod]
        public void FormatTest()
        {
            Assert.IsTrue(DateTimeHelper.ValidFormat.Length == DateTimeHelper.InvalidFormat.Length);
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(1234567)]
        public void GetStringTest(int unixTime)
        {
            var expected = DateTimeHelper.UnixEpoch.AddSeconds(unixTime).ToLocalTime()
                .ToString(DateTimeHelper.ValidFormat, CultureInfo.CurrentCulture);
            Assert.AreEqual(expected, DateTimeHelper.GetString(unixTime));
        }

        [TestMethod]
        public void GetStringTestNull()
        {
            Assert.AreEqual(DateTimeHelper.InvalidFormat, DateTimeHelper.GetString(null));
        }
    }
}
