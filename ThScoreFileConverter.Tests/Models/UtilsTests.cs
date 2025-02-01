using System.Globalization;
using ThScoreFileConverter.Models;
using WPFLocalizeExtension.Engine;

namespace ThScoreFileConverter.Tests.Models;

[TestClass]
public class UtilsTests
{
    [TestMethod]
    public void GetLocalizedValueTest()
    {
        var key = "SettingWindowTitle";
        var culture = LocalizeDictionary.Instance.Culture;
        var provider = LocalizeDictionary.Instance.DefaultProvider;

        try
        {
            LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo("en-US");
            Utils.GetLocalizedValues<string>(key).ShouldBe("Settings");

            LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo("ja-JP");
            Utils.GetLocalizedValues<string>(key).ShouldBe("設定");

            LocalizeDictionary.Instance.Culture = CultureInfo.InvariantCulture;
            Utils.GetLocalizedValues<string>(key).ShouldBe("Settings");

            LocalizeDictionary.Instance.DefaultProvider = LocalizationProvider.Instance;

            LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo("en-US");
            Utils.GetLocalizedValues<string>(key).ShouldBe("Settings");

            LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo("ja-JP");
            Utils.GetLocalizedValues<string>(key).ShouldBe("設定");

            LocalizeDictionary.Instance.Culture = CultureInfo.InvariantCulture;
            Utils.GetLocalizedValues<string>(key).ShouldBe("Settings");
        }
        finally
        {
            LocalizeDictionary.Instance.DefaultProvider = provider;
            LocalizeDictionary.Instance.Culture = culture;
        }
    }
}
