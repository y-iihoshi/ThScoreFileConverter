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
