using System;
using System.Globalization;
using ThScoreFileConverter.Models;
using WPFLocalizeExtension.Engine;

namespace ThScoreFileConverter.Tests.Models;

[TestClass]
public class UtilsTests
{
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
