using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverterTests.Helpers
{
    [TestClass]
    public class EnumHelperTests
    {
        [TestMethod]
        public void ParseEnumTestValidName()
        {
            Assert.AreEqual(DayOfWeek.Sunday, EnumHelper.Parse<DayOfWeek>("Sunday"));
        }

        [TestMethod]
        public void ParseEnumTestInvalidName()
        {
            _ = Assert.ThrowsException<ArgumentException>(() => _ = EnumHelper.Parse<DayOfWeek>("Sun"));
        }

        [TestMethod]
        public void ParseEnumTestEmpty()
        {
            _ = Assert.ThrowsException<ArgumentException>(() => _ = EnumHelper.Parse<DayOfWeek>(string.Empty));
        }

        [TestMethod]
        public void ParseEnumTestNull()
        {
            _ = Assert.ThrowsException<ArgumentNullException>(() => _ = EnumHelper.Parse<DayOfWeek>(null!));
        }

        [TestMethod]
        public void ParseEnumTestCaseSensitiveValidName()
        {
            Assert.AreEqual(DayOfWeek.Sunday, EnumHelper.Parse<DayOfWeek>("Sunday", false));
        }

        [TestMethod]
        public void ParseEnumTestCaseSensitiveInvalidName()
        {
            _ = Assert.ThrowsException<ArgumentException>(() => _ = EnumHelper.Parse<DayOfWeek>("sunday", false));
        }

        [TestMethod]
        public void ParseEnumTestCaseInsensitiveValidName()
        {
            Assert.AreEqual(DayOfWeek.Sunday, EnumHelper.Parse<DayOfWeek>("Sunday", true));
            Assert.AreEqual(DayOfWeek.Sunday, EnumHelper.Parse<DayOfWeek>("sunday", true));
        }

        [TestMethod]
        public void EnumerableTest()
        {
            var i = (int)DayOfWeek.Sunday;
            foreach (var value in EnumHelper<DayOfWeek>.Enumerable)
                Assert.AreEqual((DayOfWeek)i++, value);
            Assert.AreEqual(7, i);
        }

        [TestMethod]
        public void NumValuesTest()
        {
            Assert.AreEqual(7, EnumHelper<DayOfWeek>.NumValues);
        }
    }
}
