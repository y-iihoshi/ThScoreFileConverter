using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prism.Services.Dialogs;
using Reactive.Bindings.Extensions;
using ThScoreFileConverter;
using ThScoreFileConverter.Actions;
using ThScoreFileConverter.Adapters;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Properties;
using ThScoreFileConverter.ViewModels;
using ThScoreFileConverterTests.Extensions;
using WPFLocalizeExtension.Engine;
using SysDraw = System.Drawing;

namespace ThScoreFileConverterTests.ViewModels
{
    [TestClass]
    public class SettingWindowViewModelTests
    {
        private static Mock<ISettings> DefaultSettingsMock { get; } = new Mock<ISettings>()
            .SetupProperty(m => m.OutputNumberGroupSeparator, default(bool))
            .SetupProperty(m => m.InputCodePageId, default(int))
            .SetupProperty(m => m.OutputCodePageId, default(int));

        private static Mock<IResourceDictionaryAdapter> MockResourceDictionaryAdapter()
        {
            var mock = new Mock<IResourceDictionaryAdapter>();
            _ = mock.SetupGet(m => m.FontFamily).Returns(new FontFamily());
            _ = mock.SetupGet(m => m.FontSize).Returns(default(double));
            return mock;
        }

        [TestMethod]
        public void SettingWindowViewModelTest()
        {
            var adapterMock = MockResourceDictionaryAdapter();
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);
        }

        [TestMethod]
        public void SettingWindowViewModelTestNullSettings()
        {
            var adapterMock = MockResourceDictionaryAdapter();
            _ = Assert.ThrowsException<ArgumentNullException>(
                () => new SettingWindowViewModel(null!, adapterMock.Object));
        }

        [TestMethod]
        public void SettingWindowViewModelTestNullAdapter()
        {
            _ = Assert.ThrowsException<ArgumentNullException>(
                () => new SettingWindowViewModel(DefaultSettingsMock.Object!, null!));
        }

        [TestMethod]
        public void SettingWindowViewModelTestNullOutputNumberGroupSeparator()
        {
            var settingsMock = new Mock<ISettings>()
                .SetupProperty(m => m.InputCodePageId, default(int))
                .SetupProperty(m => m.OutputCodePageId, default(int));
            var adapterMock = MockResourceDictionaryAdapter();
            _ = Assert.ThrowsException<ArgumentException>(
                () => new SettingWindowViewModel(settingsMock.Object, adapterMock.Object));
        }

        [TestMethod]
        public void SettingWindowViewModelTestNullInputCodePageId()
        {
            var settingsMock = new Mock<ISettings>()
                .SetupProperty(m => m.OutputNumberGroupSeparator, default(bool))
                .SetupProperty(m => m.OutputCodePageId, default(int));
            var adapterMock = MockResourceDictionaryAdapter();
            _ = Assert.ThrowsException<ArgumentException>(
                () => new SettingWindowViewModel(settingsMock.Object, adapterMock.Object));
        }

        [TestMethod]
        public void SettingWindowViewModelTestNullOutputCodePageId()
        {
            var settingsMock = new Mock<ISettings>()
                .SetupProperty(m => m.OutputNumberGroupSeparator, default(bool))
                .SetupProperty(m => m.InputCodePageId, default(int));
            var adapterMock = MockResourceDictionaryAdapter();
            _ = Assert.ThrowsException<ArgumentException>(
                () => new SettingWindowViewModel(settingsMock.Object, adapterMock.Object));
        }

        [TestMethod]
        public void TitleTest()
        {
            var adapterMock = MockResourceDictionaryAdapter();
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);
            Assert.AreEqual(Utils.GetLocalizedValues<string>(nameof(Resources.SettingWindowTitle)), window.Title);
        }

        [TestMethod]
        public void FontTest()
        {
            var font = SysDraw.SystemFonts.DefaultFont;
            var adapterMock = new Mock<IResourceDictionaryAdapter>();
            _ = adapterMock.SetupGet(m => m.FontFamily).Returns(new FontFamily(font.Name));
            _ = adapterMock.SetupGet(m => m.FontSize).Returns(font.Size);

            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);
            Assert.AreEqual(font.Name, window.Font.Name);
            Assert.AreEqual(font.Size, window.Font.Size);
        }

        [TestMethod]
        public void MaxFontSizeTest()
        {
            var adapterMock = MockResourceDictionaryAdapter();
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);
            Assert.AreEqual((int)Settings.MaxFontSize, window.MaxFontSize);
        }

        [TestMethod]
        public void OutputNumberGroupSeparatorTest()
        {
            var adapterMock = MockResourceDictionaryAdapter();
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);
            Assert.AreEqual(DefaultSettingsMock.Object.OutputNumberGroupSeparator, window.OutputNumberGroupSeparator);

            var numChanged = 0;
            using var _ =
                window.ObserveProperty(w => w.OutputNumberGroupSeparator, false).Subscribe(_ => ++numChanged);

            var expected = true;
            window.OutputNumberGroupSeparator = expected;
            Assert.AreEqual(1, numChanged);
            Assert.AreEqual(expected, window.OutputNumberGroupSeparator);
            Assert.AreEqual(expected, DefaultSettingsMock.Object.OutputNumberGroupSeparator);
        }

        [TestMethod]
        public void InputEncodingsTest()
        {
            var adapterMock = MockResourceDictionaryAdapter();
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);
            CollectionAssert.That.AreEqual(Settings.ValidCodePageIds, window.InputEncodings.Keys);
        }

        [TestMethod]
        public void InputCodePageIdTest()
        {
            var adapterMock = MockResourceDictionaryAdapter();
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);
            Assert.AreEqual(DefaultSettingsMock.Object.InputCodePageId, window.InputCodePageId);

            var numChanged = 0;
            using var _ = window.ObserveProperty(w => w.InputCodePageId, false).Subscribe(_ => ++numChanged);

            var expected = 12345;
            window.InputCodePageId = expected;
            Assert.AreEqual(1, numChanged);
            Assert.AreEqual(expected, window.InputCodePageId);
            Assert.AreEqual(expected, DefaultSettingsMock.Object.InputCodePageId);
        }

        [TestMethod]
        public void OutputEncodingsTest()
        {
            var adapterMock = MockResourceDictionaryAdapter();
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);
            CollectionAssert.That.AreEqual(Settings.ValidCodePageIds, window.OutputEncodings.Keys);
        }

        [TestMethod]
        public void OutputCodePageIdTest()
        {
            var adapterMock = MockResourceDictionaryAdapter();
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);
            Assert.AreEqual(DefaultSettingsMock.Object.OutputCodePageId, window.OutputCodePageId);

            var numChanged = 0;
            using var _ = window.ObserveProperty(w => w.OutputCodePageId, false).Subscribe(_ => ++numChanged);

            var expected = 12345;
            window.OutputCodePageId = expected;
            Assert.AreEqual(1, numChanged);
            Assert.AreEqual(expected, window.OutputCodePageId);
            Assert.AreEqual(expected, DefaultSettingsMock.Object.OutputCodePageId);
        }

        [TestMethod]
        public void CultureTest()
        {
            var backupCulture = LocalizeDictionary.Instance.Culture;
            try
            {
                var culture = new CultureInfo("en-US");
                LocalizeDictionary.Instance.Culture = culture;

                var adapterMock = MockResourceDictionaryAdapter();
                using var window = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);
                Assert.AreEqual(culture, window.Culture);

                var numCultureChanged = 0;
                var numTitleChanged = 0;
                using var _1 = window.ObserveProperty(w => w.Culture, false).Subscribe(_ => ++numCultureChanged);
                using var _2 = window.ObserveProperty(w => w.Title, false).Subscribe(_ => ++numTitleChanged);

                var expected = new CultureInfo("ja-JP");
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
            var font = SysDraw.SystemFonts.DefaultFont;
            var adapterMock = new Mock<IResourceDictionaryAdapter>();
            _ = adapterMock.SetupGet(m => m.FontFamily).Returns(new FontFamily(font.Name));
            _ = adapterMock.SetupGet(m => m.FontSize).Returns(font.Size);
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);

            var command = window.FontDialogOkCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var _1 = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

            var newFont = SysDraw.SystemFonts.SmallCaptionFont;
            var color = default(SysDraw.Color);
            var result = new FontDialogActionResult(newFont, color);
            Assert.IsTrue(command.CanExecute(result));
            Assert.AreEqual(0, numChanged);

            command.Execute(result);
            Assert.AreEqual(1, numChanged);
            adapterMock.Verify(m => m.UpdateResources(result.Font.FontFamily.Name, result.Font.Size), Times.Once);
        }

        [TestMethod]
        public void FontDialogOkCommandTestDisposed()
        {
            var adapterMock = MockResourceDictionaryAdapter();
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);

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
            var adapterMock = MockResourceDictionaryAdapter();
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);

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
            var font = SysDraw.SystemFonts.DefaultFont;
            var adapterMock = new Mock<IResourceDictionaryAdapter>();
            _ = adapterMock.SetupGet(m => m.FontFamily).Returns(new FontFamily(font.Name));
            _ = adapterMock.SetupGet(m => m.FontSize).Returns(font.Size);
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);

            var command = window.FontDialogApplyCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var _1 = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

            var newFont = SysDraw.SystemFonts.SmallCaptionFont;
            var color = default(SysDraw.Color);
            var result = new FontDialogActionResult(newFont, color);
            Assert.IsTrue(command.CanExecute(result));
            Assert.AreEqual(0, numChanged);

            command.Execute(result);
            Assert.AreEqual(1, numChanged);
            adapterMock.Verify(m => m.UpdateResources(result.Font.FontFamily.Name, result.Font.Size), Times.Once);
        }

        [TestMethod]
        public void FontDialogApplyCommandTestDisposed()
        {
            var adapterMock = MockResourceDictionaryAdapter();
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);

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
            var adapterMock = MockResourceDictionaryAdapter();
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);

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
            var font = SysDraw.SystemFonts.DefaultFont;
            var adapterMock = new Mock<IResourceDictionaryAdapter>();
            _ = adapterMock.SetupGet(m => m.FontFamily).Returns(new FontFamily(font.Name));
            _ = adapterMock.SetupGet(m => m.FontSize).Returns(font.Size);
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);

            var command = window.FontDialogCancelCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var _1 = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

            var newFont = SysDraw.SystemFonts.SmallCaptionFont;
            var color = default(SysDraw.Color);
            var result = new FontDialogActionResult(newFont, color);
            Assert.IsTrue(command.CanExecute(result));
            Assert.AreEqual(0, numChanged);

            command.Execute(result);
            Assert.AreEqual(1, numChanged);
            adapterMock.Verify(m => m.UpdateResources(result.Font.FontFamily.Name, result.Font.Size), Times.Once);
        }

        [TestMethod]
        public void FontDialogCancelCommandTestDisposed()
        {
            var adapterMock = MockResourceDictionaryAdapter();
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);

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
            var adapterMock = MockResourceDictionaryAdapter();
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);

            var command = window.FontDialogCancelCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var disposable = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

            Assert.IsTrue(command.CanExecute(null!));
            Assert.AreEqual(0, numChanged);

            _ = Assert.ThrowsException<NullReferenceException>(() => command.Execute(null!));
        }

        [TestMethod]
        public void ResetFontCommandTest()
        {
            var font = SysDraw.SystemFonts.DefaultFont;
            var adapterMock = new Mock<IResourceDictionaryAdapter>();
            _ = adapterMock.SetupGet(m => m.FontFamily).Returns(new FontFamily(font.Name));
            _ = adapterMock.SetupGet(m => m.FontSize).Returns(font.Size);
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);

            var command = window.ResetFontCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var _1 = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

            Assert.IsTrue(command.CanExecute());
            Assert.AreEqual(0, numChanged);

            command.Execute();
            Assert.AreEqual(1, numChanged);
            adapterMock.Verify(
                m => m.UpdateResources(SystemFonts.MessageFontFamily, SystemFonts.MessageFontSize), Times.Once);
        }

        [TestMethod]
        public void ResetFontCommandTestDisposed()
        {
            var adapterMock = MockResourceDictionaryAdapter();
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);

            var command = window.ResetFontCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var disposable = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

            window.Dispose();

            Assert.IsTrue(command.CanExecute());
            Assert.AreEqual(0, numChanged);

            _ = Assert.ThrowsException<ObjectDisposedException>(() => command.Execute());
        }

        [TestMethod]
        public void CanCloseDialogTest()
        {
            var adapterMock = MockResourceDictionaryAdapter();
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);
            Assert.IsTrue(window.CanCloseDialog());
        }

        [TestMethod]
        public void OnDialogClosedTest()
        {
            var adapterMock = MockResourceDictionaryAdapter();
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);
            window.OnDialogClosed();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void OnDialogOpenedTest()
        {
            var adapterMock = MockResourceDictionaryAdapter();
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);
            var parameters = new DialogParameters();
            window.OnDialogOpened(parameters);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void OnDialogOpenedTestNull()
        {
            var adapterMock = MockResourceDictionaryAdapter();
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);
            window.OnDialogOpened(null!);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void DisposeTest()
        {
            var adapterMock = MockResourceDictionaryAdapter();
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);
            window.Dispose();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void DisposeTestTwice()
        {
            var adapterMock = MockResourceDictionaryAdapter();
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);
            window.Dispose();
            window.Dispose();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void FinalizerTest()
        {
            {
                var adapterMock = MockResourceDictionaryAdapter();
                _ = new SettingWindowViewModel(DefaultSettingsMock.Object, adapterMock.Object);
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
