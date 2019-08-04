using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Text;
using ThScoreFileConverter;
using Utils = ThScoreFileConverter.Models.Utils;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class UtilsTests
    {
        private bool? outputSeparator = null;

        [TestInitialize]
        public void Initialize()
            => this.outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

        [TestCleanup]
        public void Cleanup()
            => Settings.Instance.OutputNumberGroupSeparator = this.outputSeparator;

        [TestMethod]
        public void ParseEnumTestValidName()
            => Assert.AreEqual(DayOfWeek.Sunday, Utils.ParseEnum<DayOfWeek>("Sunday"));

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseEnumTestInvalidName()
        {
            Utils.ParseEnum<DayOfWeek>("Sun");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseEnumTestEmpty()
        {
            Utils.ParseEnum<DayOfWeek>(string.Empty);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ParseEnumTestNull()
        {
            Utils.ParseEnum<DayOfWeek>(null);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ParseEnumTestCaseSensitiveValidName()
            => Assert.AreEqual(DayOfWeek.Sunday, Utils.ParseEnum<DayOfWeek>("Sunday", false));

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseEnumTestCaseSensitiveInvalidName()
        {
            Utils.ParseEnum<DayOfWeek>("sunday", false);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ParseEnumTestCaseInsensitiveValidName()
        {
            Assert.AreEqual(DayOfWeek.Sunday, Utils.ParseEnum<DayOfWeek>("Sunday", true));
            Assert.AreEqual(DayOfWeek.Sunday, Utils.ParseEnum<DayOfWeek>("sunday", true));
        }

        [TestMethod]
        public void GetEnumeratorTest()
        {
            // FIXME: The method to be tested should be renamed.
            var enumerable = Utils.GetEnumerator<DayOfWeek>();
            var i = (int)DayOfWeek.Sunday;
            foreach (var value in enumerable)
                Assert.AreEqual((DayOfWeek)i++, value);
            Assert.AreEqual(7, i);
        }

        [TestMethod]
        public void ToNumberStringTest()
        {
            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("12,345,678", Utils.ToNumberString(12345678));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("12345678", Utils.ToNumberString(12345678));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ToNumberStringTestUnset()
        {
            Settings.Instance.OutputNumberGroupSeparator = null;
            Utils.ToNumberString(1234);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ToNumberStringTestWithSeparator()
            => Assert.AreEqual("12,345,678", Utils.ToNumberString(12345678, true));

        [TestMethod]
        public void ToNumberStringTestWithoutSeparator()
            => Assert.AreEqual("12345678", Utils.ToNumberString(12345678, false));

        [TestMethod]
        public void FormatTest()
        {
            var now = DateTime.Now;
            Assert.AreEqual(
                string.Format(CultureInfo.CurrentCulture, "{0:F}", now),
                Utils.Format("{0:F}", now));
        }

        [TestMethod]
        public void MakeAndPredicateTest()
        {
            var pred = Utils.MakeAndPredicate<int>(x => (x > 3), x => (x < 5));
            Assert.IsFalse(pred(3));
            Assert.IsTrue(pred(4));
            Assert.IsFalse(pred(5));
        }

        [TestMethod]
        public void ToZeroBasedTest()
        {
            Assert.AreEqual(0, Utils.ToZeroBased(1));
            Assert.AreEqual(1, Utils.ToZeroBased(2));
            Assert.AreEqual(8, Utils.ToZeroBased(9));
            Assert.AreEqual(9, Utils.ToZeroBased(0));   // Hmm...
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ToZeroBasedTestNegative()
        {
            Utils.ToZeroBased(-1);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ToZeroBasedTestExceeded()
        {
            Utils.ToZeroBased(10);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ToOneBasedTest()
        {
            Assert.AreEqual(1, Utils.ToOneBased(0));
            Assert.AreEqual(2, Utils.ToOneBased(1));
            Assert.AreEqual(9, Utils.ToOneBased(8));
            Assert.AreEqual(0, Utils.ToOneBased(9));    // Hmm...
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ToOneBasedTestNegative()
        {
            Utils.ToOneBased(-1);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ToOneBasedTestExceeded()
        {
            Utils.ToOneBased(10);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void GetEncodingTest()
        {
            var utf8 = Utils.GetEncoding(65001);
            Assert.AreNotEqual(Encoding.UTF8, utf8);
            Assert.AreEqual(new UTF8Encoding(false), utf8);
            Assert.AreEqual(Encoding.GetEncoding(932), Utils.GetEncoding(932));
        }
    }
}
