﻿using System;
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
        public void ToZeroBasedTest()
        {
            Assert.AreEqual(0, Utils.ToZeroBased(1));
            Assert.AreEqual(1, Utils.ToZeroBased(2));
            Assert.AreEqual(8, Utils.ToZeroBased(9));
            Assert.AreEqual(9, Utils.ToZeroBased(0));   // Hmm...
        }

        [TestMethod]
        public void ToZeroBasedTestNegative()
            => _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = Utils.ToZeroBased(-1));

        [TestMethod]
        public void ToZeroBasedTestExceeded()
            => _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = Utils.ToZeroBased(10));

        [TestMethod]
        public void ToOneBasedTest()
        {
            Assert.AreEqual(1, Utils.ToOneBased(0));
            Assert.AreEqual(2, Utils.ToOneBased(1));
            Assert.AreEqual(9, Utils.ToOneBased(8));
            Assert.AreEqual(0, Utils.ToOneBased(9));    // Hmm...
        }

        [TestMethod]
        public void ToOneBasedTestNegative()
            => _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = Utils.ToOneBased(-1));

        [TestMethod]
        public void ToOneBasedTestExceeded()
            => _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = Utils.ToOneBased(10));

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
