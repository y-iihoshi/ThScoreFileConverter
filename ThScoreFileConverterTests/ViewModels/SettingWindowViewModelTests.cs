using System;
using System.Globalization;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Services.Dialogs;
using Reactive.Bindings.Extensions;
using ThScoreFileConverter;
using ThScoreFileConverter.Actions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Properties;
using ThScoreFileConverter.ViewModels;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models;
using WPFLocalizeExtension.Engine;
using SysDraw = System.Drawing;

namespace ThScoreFileConverterTests.ViewModels
{
    [TestClass]
    public class SettingWindowViewModelTests
    {
        private static App SetupApp()
        {
            if (!(Application.Current is App app))
            {
                app = new App { ShutdownMode = ShutdownMode.OnExplicitShutdown };
                app.InitializeComponent();
            }

            return app;
        }

        [TestMethod]
        public void TitleTest()
        {
            using var window = new SettingWindowViewModel();
            Assert.AreEqual(Utils.GetLocalizedValues<string>(nameof(Resources.SettingWindowTitle)), window.Title);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void FontTest()
        {
            using var window = new SettingWindowViewModel();
            _ = window.Font;
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void WithAppFontTest()
        {
            var app = SetupApp();
            try
            {
                using var window = new SettingWindowViewModel();
                Assert.AreEqual(app.FontFamily.ToString(), window.Font.OriginalFontName);
                Assert.AreEqual(app.FontSize, window.Font.Size);
            }
            finally
            {
                app.Shutdown();
            }
        }

        [TestMethod]
        public void MaxFontSizeTest()
        {
            using var window = new SettingWindowViewModel();
            Assert.AreEqual((int)Settings.MaxFontSize, window.MaxFontSize);
        }

        [TestMethod]
        public void OutputNumberGroupSeparatorTest()
        {
            var separator = Settings.Instance.OutputNumberGroupSeparator;
            try
            {
                using var window = new SettingWindowViewModel();
                Assert.AreEqual(separator, window.OutputNumberGroupSeparator);

                var numChanged = 0;
                using var _ =
                    window.ObserveProperty(w => w.OutputNumberGroupSeparator, false).Subscribe(_ => ++numChanged);

                var expected = false;
                window.OutputNumberGroupSeparator = expected;
                Assert.AreEqual((expected != separator) ? 1 : 0, numChanged);
                Assert.AreEqual(expected, window.OutputNumberGroupSeparator);
                Assert.AreEqual(expected, Settings.Instance.OutputNumberGroupSeparator);

                numChanged = 0;
                expected = !expected;
                window.OutputNumberGroupSeparator = expected;
                Assert.AreEqual(1, numChanged);
                Assert.AreEqual(expected, window.OutputNumberGroupSeparator);
                Assert.AreEqual(expected, Settings.Instance.OutputNumberGroupSeparator);
            }
            finally
            {
                Settings.Instance.OutputNumberGroupSeparator = separator;
            }
        }

        [TestMethod]
        public void InputEncodingsTest()
        {
            using var window = new SettingWindowViewModel();
            CollectionAssert.That.AreEqual(Settings.ValidCodePageIds, window.InputEncodings.Keys);
        }

        [TestMethod]
        public void InputCodePageIdTest()
        {
            var id = Settings.Instance.InputCodePageId;
            try
            {
                using var window = new SettingWindowViewModel();
                Assert.AreEqual(id, window.InputCodePageId);

                var numChanged = 0;
                using var _ = window.ObserveProperty(w => w.InputCodePageId, false).Subscribe(_ => ++numChanged);

                var expected = 12345;
                window.InputCodePageId = expected;
                Assert.AreEqual((expected != id) ? 1 : 0, numChanged);
                Assert.AreEqual(expected, window.InputCodePageId);
                Assert.AreEqual(expected, Settings.Instance.InputCodePageId);
            }
            finally
            {
                Settings.Instance.InputCodePageId = id;
            }
        }

        [TestMethod]
        public void OutputEncodingsTest()
        {
            using var window = new SettingWindowViewModel();
            CollectionAssert.That.AreEqual(Settings.ValidCodePageIds, window.OutputEncodings.Keys);
        }

        [TestMethod]
        public void OutputCodePageIdTest()
        {
            var id = Settings.Instance.OutputCodePageId;
            try
            {
                using var window = new SettingWindowViewModel();
                Assert.AreEqual(id, window.OutputCodePageId);

                var numChanged = 0;
                using var _ = window.ObserveProperty(w => w.OutputCodePageId, false).Subscribe(_ => ++numChanged);

                var expected = 12345;
                window.OutputCodePageId = expected;
                Assert.AreEqual((expected != id) ? 1 : 0, numChanged);
                Assert.AreEqual(expected, window.OutputCodePageId);
                Assert.AreEqual(expected, Settings.Instance.OutputCodePageId);
            }
            finally
            {
                Settings.Instance.OutputCodePageId = id;
            }
        }

        [TestMethod]
        public void CultureTest()
        {
            var culture = LocalizeDictionary.Instance.Culture;
            try
            {
                using var window = new SettingWindowViewModel();
                Assert.AreEqual(culture, window.Culture);

                var numCultureChanged = 0;
                var numTitleChanged = 0;
                using var _1 = window.ObserveProperty(w => w.Culture, false).Subscribe(_ => ++numCultureChanged);
                using var _2 = window.ObserveProperty(w => w.Title, false).Subscribe(_ => ++numTitleChanged);

                var expected = CultureInfo.CurrentCulture;
                window.Culture = expected;
                Assert.AreEqual((expected != culture) ? 1 : 0, numCultureChanged);
                Assert.AreEqual((expected != culture) ? 1 : 0, numTitleChanged);
                Assert.AreEqual(expected, window.Culture);
                Assert.AreEqual(expected, LocalizeDictionary.Instance.Culture);
            }
            finally
            {
                LocalizeDictionary.Instance.Culture = culture;
            }
        }

        [TestMethod]
        public void FontDialogOkCommandTest()
        {
            using var window = new SettingWindowViewModel();

            var command = window.FontDialogOkCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var _ = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

            var font = SysDraw.SystemFonts.DefaultFont;
            var color = default(SysDraw.Color);
            var result = new FontDialogActionResult(font, color);
            Assert.IsTrue(command.CanExecute(result));
            Assert.AreEqual(0, numChanged);

            command.Execute(result);
            Assert.AreEqual(0, numChanged);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void FontDialogOkCommandTestDisposed()
        {
            using var window = new SettingWindowViewModel();

            var command = window.FontDialogOkCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var _ = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

            window.Dispose();

            var font = SysDraw.SystemFonts.DefaultFont;
            var color = default(SysDraw.Color);
            var result = new FontDialogActionResult(font, color);
            Assert.IsTrue(command.CanExecute(result));
            Assert.AreEqual(0, numChanged);

            command.Execute(result);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void WithAppFontDialogOkCommandTest()
        {
            var app = SetupApp();
            try
            {
                using var window = new SettingWindowViewModel();

                var command = window.FontDialogOkCommand;
                Assert.IsNotNull(command);

                var numChanged = 0;
                using var _ = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

                var font = SysDraw.SystemFonts.DefaultFont;
                var color = default(SysDraw.Color);
                var result = new FontDialogActionResult(font, color);
                Assert.IsTrue(command.CanExecute(result));
                Assert.AreEqual(0, numChanged);

                command.Execute(result);
                Assert.AreEqual(1, numChanged);
                Assert.AreEqual(app.FontFamily.ToString(), font.Name);
                Assert.AreEqual(app.FontSize, font.Size);
            }
            finally
            {
                app.Shutdown();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void WithAppFontDialogOkCommandTestNull()
        {
            var app = SetupApp();
            try
            {
                using var window = new SettingWindowViewModel();

                var command = window.FontDialogOkCommand;
                Assert.IsNotNull(command);

                var numChanged = 0;
                using var _ = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

                Assert.IsTrue(command.CanExecute(null!));
                Assert.AreEqual(0, numChanged);

                command.Execute(null!);
                Assert.Fail(TestUtils.Unreachable);
            }
            finally
            {
                app.Shutdown();
            }
        }

        [TestMethod]
        public void FontDialogApplyCommandTest()
        {
            using var window = new SettingWindowViewModel();

            var command = window.FontDialogApplyCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var _ = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

            var font = SysDraw.SystemFonts.DefaultFont;
            var color = default(SysDraw.Color);
            var result = new FontDialogActionResult(font, color);
            Assert.IsTrue(command.CanExecute(result));
            Assert.AreEqual(0, numChanged);

            command.Execute(result);
            Assert.AreEqual(0, numChanged);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void FontDialogApplyCommandTestDisposed()
        {
            using var window = new SettingWindowViewModel();

            var command = window.FontDialogApplyCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var _ = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

            window.Dispose();

            var font = SysDraw.SystemFonts.DefaultFont;
            var color = default(SysDraw.Color);
            var result = new FontDialogActionResult(font, color);
            Assert.IsTrue(command.CanExecute(result));
            Assert.AreEqual(0, numChanged);

            command.Execute(result);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void WithAppFontDialogApplyCommandTest()
        {
            var app = SetupApp();
            try
            {
                using var window = new SettingWindowViewModel();

                var command = window.FontDialogApplyCommand;
                Assert.IsNotNull(command);

                var numChanged = 0;
                using var _ = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

                var font = SysDraw.SystemFonts.DefaultFont;
                var color = default(SysDraw.Color);
                var result = new FontDialogActionResult(font, color);
                Assert.IsTrue(command.CanExecute(result));
                Assert.AreEqual(0, numChanged);

                command.Execute(result);
                Assert.AreEqual(1, numChanged);
                Assert.AreEqual(app.FontFamily.ToString(), font.Name);
                Assert.AreEqual(app.FontSize, font.Size);
            }
            finally
            {
                app.Shutdown();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void WithAppFontDialogApplyCommandTestNull()
        {
            var app = SetupApp();
            try
            {
                using var window = new SettingWindowViewModel();

                var command = window.FontDialogApplyCommand;
                Assert.IsNotNull(command);

                var numChanged = 0;
                using var _ = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

                Assert.IsTrue(command.CanExecute(null!));
                Assert.AreEqual(0, numChanged);

                command.Execute(null!);
                Assert.Fail(TestUtils.Unreachable);
            }
            finally
            {
                app.Shutdown();
            }
        }

        [TestMethod]
        public void FontDialogCancelCommandTest()
        {
            using var window = new SettingWindowViewModel();

            var command = window.FontDialogCancelCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var _ = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

            var font = SysDraw.SystemFonts.DefaultFont;
            var color = default(SysDraw.Color);
            var result = new FontDialogActionResult(font, color);
            Assert.IsTrue(command.CanExecute(result));
            Assert.AreEqual(0, numChanged);

            command.Execute(result);
            Assert.AreEqual(0, numChanged);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void FontDialogCancelCommandTestDisposed()
        {
            using var window = new SettingWindowViewModel();

            var command = window.FontDialogCancelCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var _ = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

            window.Dispose();

            var font = SysDraw.SystemFonts.DefaultFont;
            var color = default(SysDraw.Color);
            var result = new FontDialogActionResult(font, color);
            Assert.IsTrue(command.CanExecute(result));
            Assert.AreEqual(0, numChanged);

            command.Execute(result);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void WithAppFontDialogCancelCommandTest()
        {
            var app = SetupApp();
            try
            {
                using var window = new SettingWindowViewModel();

                var command = window.FontDialogCancelCommand;
                Assert.IsNotNull(command);

                var numChanged = 0;
                using var _ = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

                var font = SysDraw.SystemFonts.DefaultFont;
                var color = default(SysDraw.Color);
                var result = new FontDialogActionResult(font, color);
                Assert.IsTrue(command.CanExecute(result));
                Assert.AreEqual(0, numChanged);

                command.Execute(result);
                Assert.AreEqual(1, numChanged);
                Assert.AreEqual(app.FontFamily.ToString(), font.Name);
                Assert.AreEqual(app.FontSize, font.Size);
            }
            finally
            {
                app.Shutdown();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void WithAppFontDialogCancelCommandTestNull()
        {
            var app = SetupApp();
            try
            {
                using var window = new SettingWindowViewModel();

                var command = window.FontDialogCancelCommand;
                Assert.IsNotNull(command);

                var numChanged = 0;
                using var _ = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

                Assert.IsTrue(command.CanExecute(null!));
                Assert.AreEqual(0, numChanged);

                command.Execute(null!);
                Assert.Fail(TestUtils.Unreachable);
            }
            finally
            {
                app.Shutdown();
            }
        }

        [TestMethod]
        public void ResetFontCommandTest()
        {
            using var window = new SettingWindowViewModel();

            var command = window.ResetFontCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var _ = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

            Assert.IsTrue(command.CanExecute());
            Assert.AreEqual(0, numChanged);

            command.Execute();
            Assert.AreEqual(0, numChanged);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void ResetFontCommandTestDisposed()
        {
            using var window = new SettingWindowViewModel();

            var command = window.ResetFontCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var _ = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

            window.Dispose();

            Assert.IsTrue(command.CanExecute());
            Assert.AreEqual(0, numChanged);

            command.Execute();
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void WithAppResetFontCommandTest()
        {
            var app = SetupApp();
            try
            {
                using var window = new SettingWindowViewModel();

                var command = window.ResetFontCommand;
                Assert.IsNotNull(command);

                var numChanged = 0;
                using var _ = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

                Assert.IsTrue(command.CanExecute());
                Assert.AreEqual(0, numChanged);

                command.Execute();
                Assert.AreEqual(1, numChanged);
                Assert.AreEqual(app.FontFamily, SystemFonts.MessageFontFamily);
                Assert.AreEqual(app.FontSize, SystemFonts.MessageFontSize);
            }
            finally
            {
                app.Shutdown();
            }
        }

        [TestMethod]
        public void CanCloseDialogTest()
        {
            using var window = new SettingWindowViewModel();
            Assert.IsTrue(window.CanCloseDialog());
        }

        [TestMethod]
        public void OnDialogClosedTest()
        {
            using var window = new SettingWindowViewModel();
            window.OnDialogClosed();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void OnDialogOpenedTest()
        {
            using var window = new SettingWindowViewModel();
            var parameters = new DialogParameters();
            window.OnDialogOpened(parameters);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void OnDialogOpenedTestNull()
        {
            using var window = new SettingWindowViewModel();
            window.OnDialogOpened(null!);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void DisposeTest()
        {
            using var window = new SettingWindowViewModel();
            window.Dispose();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void DisposeTestTwice()
        {
            using var window = new SettingWindowViewModel();
            window.Dispose();
            window.Dispose();
            Assert.IsTrue(true);
        }
    }
}
