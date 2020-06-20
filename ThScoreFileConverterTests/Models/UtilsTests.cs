using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverter.Models;
using WPFLocalizeExtension.Engine;
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
            _ = Utils.ParseEnum<DayOfWeek>("Sun");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseEnumTestEmpty()
        {
            _ = Utils.ParseEnum<DayOfWeek>(string.Empty);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ParseEnumTestNull()
        {
            _ = Utils.ParseEnum<DayOfWeek>(null!);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ParseEnumTestCaseSensitiveValidName()
            => Assert.AreEqual(DayOfWeek.Sunday, Utils.ParseEnum<DayOfWeek>("Sunday", false));

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseEnumTestCaseSensitiveInvalidName()
        {
            _ = Utils.ParseEnum<DayOfWeek>("sunday", false);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ParseEnumTestCaseInsensitiveValidName()
        {
            Assert.AreEqual(DayOfWeek.Sunday, Utils.ParseEnum<DayOfWeek>("Sunday", true));
            Assert.AreEqual(DayOfWeek.Sunday, Utils.ParseEnum<DayOfWeek>("sunday", true));
        }

        [TestMethod]
        public void GetEnumerableTest()
        {
            var enumerable = Utils.GetEnumerable<DayOfWeek>();
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
        public void TrueTest()
        {
            Assert.IsTrue(Utils.True(false));
            Assert.IsTrue(Utils.True(true));
            Assert.IsTrue(Utils.True(0));
            Assert.IsTrue(Utils.True(1));
            Assert.IsTrue(Utils.True(new int?()));
            Assert.IsTrue(Utils.True(new object()));
            Assert.IsTrue(Utils.True<object?>(null));
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
            _ = Utils.ToZeroBased(-1);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ToZeroBasedTestExceeded()
        {
            _ = Utils.ToZeroBased(10);
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
            _ = Utils.ToOneBased(-1);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ToOneBasedTestExceeded()
        {
            _ = Utils.ToOneBased(10);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void GetLocalizedValueTest()
        {
            var key = "SettingWindowTitle";
            var culture = LocalizeDictionary.Instance.Culture;
            var provider = LocalizeDictionary.Instance.DefaultProvider;

            try
            {
                LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo("en-US");
                Assert.AreEqual("Settings", Utils.GetLocalizedValues<string>(key));

                LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo("ja-JP");
                Assert.AreEqual("設定", Utils.GetLocalizedValues<string>(key));

                LocalizeDictionary.Instance.Culture = CultureInfo.InvariantCulture;
                Assert.AreEqual("Settings", Utils.GetLocalizedValues<string>(key));

                LocalizeDictionary.Instance.DefaultProvider = LocalizationProvider.Instance;

                LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo("en-US");
                Assert.AreEqual("Settings", Utils.GetLocalizedValues<string>(key));

                LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo("ja-JP");
                Assert.AreEqual("設定", Utils.GetLocalizedValues<string>(key));

                LocalizeDictionary.Instance.Culture = CultureInfo.InvariantCulture;
                Assert.AreEqual("Settings", Utils.GetLocalizedValues<string>(key));
            }
            finally
            {
                LocalizeDictionary.Instance.DefaultProvider = provider;
                LocalizeDictionary.Instance.Culture = culture;
            }
        }
    }
}
