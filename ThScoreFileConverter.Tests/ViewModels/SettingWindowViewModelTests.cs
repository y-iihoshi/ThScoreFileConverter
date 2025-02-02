using System.Globalization;
using Reactive.Bindings.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Resources;
using ThScoreFileConverter.ViewModels;
using WPFLocalizeExtension.Engine;

namespace ThScoreFileConverter.Tests.ViewModels;

[TestClass]
public class SettingWindowViewModelTests
{
    private static SettingWindowViewModel CreateViewModel()
    {
        var settings = new Settings();
        return new SettingWindowViewModel(settings);
    }

    [TestMethod]
    public void SettingWindowViewModelTest()
    {
        using var window = CreateViewModel();
    }

    [TestMethod]
    public void TitleTest()
    {
        using var window = CreateViewModel();
        window.Title.ShouldBe(Utils.GetLocalizedValues<string>(nameof(StringResources.SettingWindowTitle)));
    }

    [TestMethod]
    public void OutputNumberGroupSeparatorTest()
    {
        var settings = new Settings();
        using var window = new SettingWindowViewModel(settings);
        window.OutputNumberGroupSeparator.Value.ShouldBe(settings.OutputNumberGroupSeparator.GetValueOrDefault());

        var numChanged = 0;
        using var _ = window.OutputNumberGroupSeparator.Subscribe(_ => ++numChanged);

        var expected = true;
        window.OutputNumberGroupSeparator.Value = expected;
        numChanged.ShouldBe(1);
        window.OutputNumberGroupSeparator.Value.ShouldBe(expected);
        settings.OutputNumberGroupSeparator.ShouldBe(expected);
    }

    [TestMethod]
    public void InputEncodingsTest()
    {
        using var window = CreateViewModel();
        window.InputEncodings.Keys.ShouldBe(Settings.ValidCodePageIds);
    }

    [TestMethod]
    public void InputCodePageIdTest()
    {
        var settings = new Settings();
        using var window = new SettingWindowViewModel(settings);
        window.InputCodePageId.Value.ShouldBe(settings.InputCodePageId.GetValueOrDefault());

        var numChanged = 0;
        using var _ = window.InputCodePageId.Subscribe(_ => ++numChanged);

        var expected = 12345;
        window.InputCodePageId.Value = expected;
        numChanged.ShouldBe(2);
        window.InputCodePageId.Value.ShouldBe(expected);
        settings.InputCodePageId.ShouldBe(expected);
    }

    [TestMethod]
    public void OutputEncodingsTest()
    {
        using var window = CreateViewModel();
        window.OutputEncodings.Keys.ShouldBe(Settings.ValidCodePageIds);
    }

    [TestMethod]
    public void OutputCodePageIdTest()
    {
        var settings = new Settings();
        using var window = new SettingWindowViewModel(settings);
        window.OutputCodePageId.Value.ShouldBe(settings.OutputCodePageId.GetValueOrDefault());

        var numChanged = 0;
        using var _ = window.OutputCodePageId.Subscribe(_ => ++numChanged);

        var expected = 12345;
        window.OutputCodePageId.Value = expected;
        numChanged.ShouldBe(2);
        window.OutputCodePageId.Value.ShouldBe(expected);
        settings.OutputCodePageId.ShouldBe(expected);
    }

    [TestMethod]
    public void CultureTest()
    {
        var backupCulture = LocalizeDictionary.Instance.Culture;
        try
        {
            var culture = CultureInfo.GetCultureInfo("en-US");
            LocalizeDictionary.Instance.Culture = culture;

            using var window = CreateViewModel();
            window.Culture.ShouldBe(culture);

            var numCultureChanged = 0;
            var numTitleChanged = 0;
            using var _1 = window.ObserveProperty(w => w.Culture, false).Subscribe(_ => ++numCultureChanged);
            using var _2 = window.ObserveProperty(w => w.Title, false).Subscribe(_ => ++numTitleChanged);

            var expected = CultureInfo.GetCultureInfo("ja-JP");
            window.Culture = expected;
            numCultureChanged.ShouldBe(1);
            numTitleChanged.ShouldBe(1);
            window.Culture.ShouldBe(expected);
            LocalizeDictionary.Instance.Culture.ShouldBe(expected);
        }
        finally
        {
            LocalizeDictionary.Instance.Culture = backupCulture;
        }
    }

    [TestMethod]
    public void DisposeTest()
    {
        using var window = CreateViewModel();
        window.Dispose();
    }

    [TestMethod]
    public void DisposeTestTwice()
    {
        using var window = CreateViewModel();
        window.Dispose();
        window.Dispose();
    }
}
