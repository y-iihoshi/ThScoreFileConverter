﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using NSubstitute;
using Reactive.Bindings.Extensions;
using ThScoreFileConverter.Adapters;
using ThScoreFileConverter.Interactivity;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Properties;
using ThScoreFileConverter.ViewModels;
using WPFLocalizeExtension.Engine;
using SysDraw = System.Drawing;

namespace ThScoreFileConverter.Tests.ViewModels;

[TestClass]
public class SettingWindowViewModelTests
{
    private static IResourceDictionaryAdapter MockResourceDictionaryAdapter()
    {
        var mock = Substitute.For<IResourceDictionaryAdapter>();
        _ = mock.FontFamily.Returns(new FontFamily());
        _ = mock.FontSize.Returns(default(double));
        return mock;
    }

    private static SettingWindowViewModel CreateViewModel()
    {
        var settings = new Settings();
        var resourceDictionaryAdapterMock = MockResourceDictionaryAdapter();
        return new SettingWindowViewModel(settings, resourceDictionaryAdapterMock);
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
    public void FontTest()
    {
        var settings = new Settings();
        var font = SysDraw.SystemFonts.DefaultFont;
        var adapterMock = Substitute.For<IResourceDictionaryAdapter>();
        _ = adapterMock.FontFamily.Returns(new FontFamily(font.Name));
        _ = adapterMock.FontSize.Returns(font.Size);

        using var window = new SettingWindowViewModel(settings, adapterMock);
        Assert.AreEqual(font.Name, window.Font.Name);
        Assert.AreEqual(font.Size, window.Font.Size);
    }

    [TestMethod]
    public void MaxFontSizeTest()
    {
        using var window = CreateViewModel();
        Assert.AreEqual((int)Settings.MaxFontSize, window.MaxFontSize);
    }

    [TestMethod]
    public void OutputNumberGroupSeparatorTest()
    {
        var settings = new Settings();
        var adapterMock = MockResourceDictionaryAdapter();
        using var window = new SettingWindowViewModel(settings, adapterMock);
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
        var adapterMock = MockResourceDictionaryAdapter();
        using var window = new SettingWindowViewModel(settings, adapterMock);
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
        var adapterMock = MockResourceDictionaryAdapter();
        using var window = new SettingWindowViewModel(settings, adapterMock);
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
    public void FontDialogOkCommandTest()
    {
        var settings = new Settings();
        var font = SysDraw.SystemFonts.DefaultFont;
        var adapterMock = Substitute.For<IResourceDictionaryAdapter>();
        _ = adapterMock.FontFamily.Returns(new FontFamily(font.Name));
        _ = adapterMock.FontSize.Returns(font.Size);
        using var window = new SettingWindowViewModel(settings, adapterMock);

        var command = window.FontDialogOkCommand;
        Assert.IsNotNull(command);

        var numChanged = 0;
        using var _1 = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

        using var newFont = new SysDraw.Font(font.FontFamily.Name, font.Size + 2);
        var color = default(SysDraw.Color);
        var result = new FontDialogActionResult(newFont, color);
        Assert.IsTrue(command.CanExecute(result));
        Assert.AreEqual(0, numChanged);

        command.Execute(result);
        Assert.AreEqual(1, numChanged);
        adapterMock.Received(1).UpdateResources(result.Font.FontFamily.Name, result.Font.Size);
    }

    [TestMethod]
    public void FontDialogOkCommandTestDisposed()
    {
        using var window = CreateViewModel();

        var command = window.FontDialogOkCommand;
        Assert.IsNotNull(command);

        var numChanged = 0;
        using var disposable = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

        window.Dispose();

        var font = SysDraw.SystemFonts.DefaultFont;
        var color = default(SysDraw.Color);
        var result = new FontDialogActionResult(font, color);
        Assert.IsTrue(command.CanExecute(result));
        Assert.AreEqual(0, numChanged);

        _ = Assert.ThrowsException<ObjectDisposedException>(() => command.Execute(result));
    }

    [TestMethod]
    public void FontDialogOkCommandTestNull()
    {
        using var window = CreateViewModel();

        var command = window.FontDialogOkCommand;
        Assert.IsNotNull(command);

        var numChanged = 0;
        using var disposable = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

        Assert.IsTrue(command.CanExecute(null!));
        Assert.AreEqual(0, numChanged);

        _ = Assert.ThrowsException<NullReferenceException>(() => command.Execute(null!));
    }

    [TestMethod]
    public void FontDialogApplyCommandTest()
    {
        var settings = new Settings();
        var font = SysDraw.SystemFonts.DefaultFont;
        var adapterMock = Substitute.For<IResourceDictionaryAdapter>();
        _ = adapterMock.FontFamily.Returns(new FontFamily(font.Name));
        _ = adapterMock.FontSize.Returns(font.Size);
        using var window = new SettingWindowViewModel(settings, adapterMock);

        var command = window.FontDialogApplyCommand;
        Assert.IsNotNull(command);

        var numChanged = 0;
        using var _1 = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

        using var newFont = new SysDraw.Font(font.FontFamily.Name, font.Size + 2);
        var color = default(SysDraw.Color);
        var result = new FontDialogActionResult(newFont, color);
        Assert.IsTrue(command.CanExecute(result));
        Assert.AreEqual(0, numChanged);

        command.Execute(result);
        Assert.AreEqual(1, numChanged);
        adapterMock.Received(1).UpdateResources(result.Font.FontFamily.Name, result.Font.Size);
    }

    [TestMethod]
    public void FontDialogApplyCommandTestDisposed()
    {
        using var window = CreateViewModel();

        var command = window.FontDialogApplyCommand;
        Assert.IsNotNull(command);

        var numChanged = 0;
        using var disposable = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

        window.Dispose();

        var font = SysDraw.SystemFonts.DefaultFont;
        var color = default(SysDraw.Color);
        var result = new FontDialogActionResult(font, color);
        Assert.IsTrue(command.CanExecute(result));
        Assert.AreEqual(0, numChanged);

        _ = Assert.ThrowsException<ObjectDisposedException>(() => command.Execute(result));
    }

    [TestMethod]
    public void FontDialogApplyCommandTestNull()
    {
        using var window = CreateViewModel();

        var command = window.FontDialogApplyCommand;
        Assert.IsNotNull(command);

        var numChanged = 0;
        using var disposable = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

        Assert.IsTrue(command.CanExecute(null!));
        Assert.AreEqual(0, numChanged);

        _ = Assert.ThrowsException<NullReferenceException>(() => command.Execute(null!));
    }

    [TestMethod]
    public void FontDialogCancelCommandTest()
    {
        var settings = new Settings();
        var font = SysDraw.SystemFonts.DefaultFont;
        var adapterMock = Substitute.For<IResourceDictionaryAdapter>();
        _ = adapterMock.FontFamily.Returns(new FontFamily(font.Name));
        _ = adapterMock.FontSize.Returns(font.Size);
        using var window = new SettingWindowViewModel(settings, adapterMock);

        var command = window.FontDialogCancelCommand;
        Assert.IsNotNull(command);

        var numChanged = 0;
        using var _1 = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

        using var newFont = new SysDraw.Font(font.FontFamily.Name, font.Size + 2);
        var color = default(SysDraw.Color);
        var result = new FontDialogActionResult(newFont, color);
        Assert.IsTrue(command.CanExecute(result));
        Assert.AreEqual(0, numChanged);

        command.Execute(result);
        Assert.AreEqual(1, numChanged);
        adapterMock.Received(1).UpdateResources(result.Font.FontFamily.Name, result.Font.Size);
    }

    [TestMethod]
    public void FontDialogCancelCommandTestDisposed()
    {
        using var window = CreateViewModel();

        var command = window.FontDialogCancelCommand;
        Assert.IsNotNull(command);

        var numChanged = 0;
        using var disposable = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

        window.Dispose();

        var font = SysDraw.SystemFonts.DefaultFont;
        var color = default(SysDraw.Color);
        var result = new FontDialogActionResult(font, color);
        Assert.IsTrue(command.CanExecute(result));
        Assert.AreEqual(0, numChanged);

        _ = Assert.ThrowsException<ObjectDisposedException>(() => command.Execute(result));
    }

    [TestMethod]
    public void FontDialogCancelCommandTestNull()
    {
        using var window = CreateViewModel();

        var command = window.FontDialogCancelCommand;
        Assert.IsNotNull(command);

        var numChanged = 0;
        using var disposable = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

        Assert.IsTrue(command.CanExecute(null));
        Assert.AreEqual(0, numChanged);

        _ = Assert.ThrowsException<NullReferenceException>(() => command.Execute(null));
    }

    [TestMethod]
    public void ResetFontCommandTest()
    {
        var settings = new Settings();
        var font = SysDraw.SystemFonts.DefaultFont;
        var adapterMock = Substitute.For<IResourceDictionaryAdapter>();
        _ = adapterMock.FontFamily.Returns(new FontFamily(font.Name));
        _ = adapterMock.FontSize.Returns(font.Size);
        using var window = new SettingWindowViewModel(settings, adapterMock);

        var command = window.ResetFontCommand;
        Assert.IsNotNull(command);

        var numChanged = 0;
        using var _1 = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

        Assert.IsTrue(command.CanExecute(null));
        Assert.AreEqual(0, numChanged);

        command.Execute(null);
        Assert.AreEqual(1, numChanged);
        adapterMock.Received(1).UpdateResources(SystemFonts.MessageFontFamily, SystemFonts.MessageFontSize);
    }

    [TestMethod]
    public void ResetFontCommandTestDisposed()
    {
        using var window = CreateViewModel();

        var command = window.ResetFontCommand;
        Assert.IsNotNull(command);

        var numChanged = 0;
        using var disposable = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

        window.Dispose();

        Assert.IsTrue(command.CanExecute(null));
        Assert.AreEqual(0, numChanged);

        _ = Assert.ThrowsException<ObjectDisposedException>(() => command.Execute(null));
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
