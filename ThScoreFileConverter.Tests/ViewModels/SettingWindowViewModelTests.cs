using System;
using System.Globalization;
using Reactive.Bindings.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Properties;
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
        Assert.AreEqual(Utils.GetLocalizedValues<string>(nameof(Resources.SettingWindowTitle)), window.Title);
    }

    [TestMethod]
    public void OutputNumberGroupSeparatorTest()
    {
        var settings = new Settings();
        using var window = new SettingWindowViewModel(settings);
        Assert.AreEqual(settings.OutputNumberGroupSeparator, window.OutputNumberGroupSeparator.Value);

        var numChanged = 0;
        using var _ = window.OutputNumberGroupSeparator.Subscribe(_ => ++numChanged);

        var expected = true;
        window.OutputNumberGroupSeparator.Value = expected;
        Assert.AreEqual(1, numChanged);
        Assert.AreEqual(expected, window.OutputNumberGroupSeparator.Value);
        Assert.AreEqual(expected, settings.OutputNumberGroupSeparator);
    }

    [TestMethod]
    public void InputEncodingsTest()
    {
        using var window = CreateViewModel();
        CollectionAssert.That.AreEqual(Settings.ValidCodePageIds, window.InputEncodings.Keys);
    }

    [TestMethod]
    public void InputCodePageIdTest()
    {
        var settings = new Settings();
        using var window = new SettingWindowViewModel(settings);
        Assert.AreEqual(settings.InputCodePageId, window.InputCodePageId.Value);

        var numChanged = 0;
        using var _ = window.InputCodePageId.Subscribe(_ => ++numChanged);

        var expected = 12345;
        window.InputCodePageId.Value = expected;
        Assert.AreEqual(2, numChanged);
        Assert.AreEqual(expected, window.InputCodePageId.Value);
        Assert.AreEqual(expected, settings.InputCodePageId);
    }

    [TestMethod]
    public void OutputEncodingsTest()
    {
        using var window = CreateViewModel();
        CollectionAssert.That.AreEqual(Settings.ValidCodePageIds, window.OutputEncodings.Keys);
    }

    [TestMethod]
    public void OutputCodePageIdTest()
    {
        var settings = new Settings();
        using var window = new SettingWindowViewModel(settings);
        Assert.AreEqual(settings.OutputCodePageId, window.OutputCodePageId.Value);

        var numChanged = 0;
        using var _ = window.OutputCodePageId.Subscribe(_ => ++numChanged);

        var expected = 12345;
        window.OutputCodePageId.Value = expected;
        Assert.AreEqual(2, numChanged);
        Assert.AreEqual(expected, window.OutputCodePageId.Value);
        Assert.AreEqual(expected, settings.OutputCodePageId);
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
            Assert.AreEqual(culture, window.Culture);

            var numCultureChanged = 0;
            var numTitleChanged = 0;
            using var _1 = window.ObserveProperty(w => w.Culture, false).Subscribe(_ => ++numCultureChanged);
            using var _2 = window.ObserveProperty(w => w.Title, false).Subscribe(_ => ++numTitleChanged);

            var expected = CultureInfo.GetCultureInfo("ja-JP");
            window.Culture = expected;
            Assert.AreEqual(1, numCultureChanged);
            Assert.AreEqual(1, numTitleChanged);
            Assert.AreEqual(expected, window.Culture);
            Assert.AreEqual(expected, LocalizeDictionary.Instance.Culture);
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
