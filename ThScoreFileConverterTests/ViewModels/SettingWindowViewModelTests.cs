using System;
using System.Globalization;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prism.Services.Dialogs;
using Reactive.Bindings.Extensions;
using ThScoreFileConverter;
using ThScoreFileConverter.Actions;
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
        public void SettingWindowViewModelTest()
        {
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);
        }

        [TestMethod]
        public void SettingWindowViewModelTestNull()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => new SettingWindowViewModel(null!));

        [TestMethod]
        public void SettingWindowViewModelTestNullOutputNumberGroupSeparator()
        {
            var mock = new Mock<ISettings>()
                .SetupProperty(m => m.InputCodePageId, default(int))
                .SetupProperty(m => m.OutputCodePageId, default(int));
            Assert.ThrowsException<ArgumentException>(() => new SettingWindowViewModel(mock.Object));
        }

        [TestMethod]
        public void SettingWindowViewModelTestNullInputCodePageId()
        {
            var mock = new Mock<ISettings>()
                .SetupProperty(m => m.OutputNumberGroupSeparator, default(bool))
                .SetupProperty(m => m.OutputCodePageId, default(int));
            Assert.ThrowsException<ArgumentException>(() => new SettingWindowViewModel(mock.Object));
        }

        [TestMethod]
        public void SettingWindowViewModelTestNullOutputCodePageId()
        {
            var mock = new Mock<ISettings>()
                .SetupProperty(m => m.OutputNumberGroupSeparator, default(bool))
                .SetupProperty(m => m.InputCodePageId, default(int));
            _ = Assert.ThrowsException<ArgumentException>(() => new SettingWindowViewModel(mock.Object));
        }

        [TestMethod]
        public void TitleTest()
        {
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);
            Assert.AreEqual(Utils.GetLocalizedValues<string>(nameof(Resources.SettingWindowTitle)), window.Title);
        }

        [TestMethod]
        public void FontTest()
        {
            if (Application.Current is null)
            {
                using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);
                _ = Assert.ThrowsException<NullReferenceException>(() => _ = window.Font);
            }
        }

        [TestMethod]
        public void WithAppFontTest()
        {
            var app = SetupApp();
            try
            {
                using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);
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
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);
            Assert.AreEqual((int)Settings.MaxFontSize, window.MaxFontSize);
        }

        [TestMethod]
        public void OutputNumberGroupSeparatorTest()
        {
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);
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
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);
            CollectionAssert.That.AreEqual(Settings.ValidCodePageIds, window.InputEncodings.Keys);
        }

        [TestMethod]
        public void InputCodePageIdTest()
        {
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);
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
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);
            CollectionAssert.That.AreEqual(Settings.ValidCodePageIds, window.OutputEncodings.Keys);
        }

        [TestMethod]
        public void OutputCodePageIdTest()
        {
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);
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
            var culture = LocalizeDictionary.Instance.Culture;
            try
            {
                using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);
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
            if (Application.Current is null)
            {
                using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);

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
        }

        [TestMethod]
        public void FontDialogOkCommandTestDisposed()
        {
            if (Application.Current is null)
            {
                using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);

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
        }

        [TestMethod]
        public void WithAppFontDialogOkCommandTest()
        {
            var app = SetupApp();
            try
            {
                using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);

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
        public void WithAppFontDialogOkCommandTestDisposed()
        {
            var app = SetupApp();
            try
            {
                using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);

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
            finally
            {
                app.Shutdown();
            }
        }

        [TestMethod]
        public void WithAppFontDialogOkCommandTestNull()
        {
            var app = SetupApp();
            try
            {
                using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);

                var command = window.FontDialogOkCommand;
                Assert.IsNotNull(command);

                var numChanged = 0;
                using var disposable = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

                Assert.IsTrue(command.CanExecute(null!));
                Assert.AreEqual(0, numChanged);

                _ = Assert.ThrowsException<NullReferenceException>(() => command.Execute(null!));
            }
            finally
            {
                app.Shutdown();
            }
        }

        [TestMethod]
        public void FontDialogApplyCommandTest()
        {
            if (Application.Current is null)
            {
                using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);

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
        }

        [TestMethod]
        public void FontDialogApplyCommandTestDisposed()
        {
            if (Application.Current is null)
            {
                using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);

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
        }

        [TestMethod]
        public void WithAppFontDialogApplyCommandTest()
        {
            var app = SetupApp();
            try
            {
                using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);

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
        public void WithAppFontDialogApplyCommandTestDisposed()
        {
            var app = SetupApp();
            try
            {
                using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);

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

                Assert.ThrowsException<ObjectDisposedException>(() => command.Execute(result));
            }
            finally
            {
                app.Shutdown();
            }
        }

        [TestMethod]
        public void WithAppFontDialogApplyCommandTestNull()
        {
            var app = SetupApp();
            try
            {
                using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);

                var command = window.FontDialogApplyCommand;
                Assert.IsNotNull(command);

                var numChanged = 0;
                using var disposable = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

                Assert.IsTrue(command.CanExecute(null!));
                Assert.AreEqual(0, numChanged);

                _ = Assert.ThrowsException<NullReferenceException>(() => command.Execute(null!));
            }
            finally
            {
                app.Shutdown();
            }
        }

        [TestMethod]
        public void FontDialogCancelCommandTest()
        {
            if (Application.Current is null)
            {
                using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);

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
        }

        [TestMethod]
        public void FontDialogCancelCommandTestDisposed()
        {
            if (Application.Current is null)
            {
                using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);

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
        }

        [TestMethod]
        public void WithAppFontDialogCancelCommandTest()
        {
            var app = SetupApp();
            try
            {
                using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);

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
        public void WithAppFontDialogCancelCommandTestDisposed()
        {
            var app = SetupApp();
            try
            {
                using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);

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

                Assert.ThrowsException<ObjectDisposedException>(() => command.Execute(result));
            }
            finally
            {
                app.Shutdown();
            }
        }

        [TestMethod]
        public void WithAppFontDialogCancelCommandTestNull()
        {
            var app = SetupApp();
            try
            {
                using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);

                var command = window.FontDialogCancelCommand;
                Assert.IsNotNull(command);

                var numChanged = 0;
                using var disposable = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

                Assert.IsTrue(command.CanExecute(null!));
                Assert.AreEqual(0, numChanged);

                _ = Assert.ThrowsException<NullReferenceException>(() => command.Execute(null!));
            }
            finally
            {
                app.Shutdown();
            }
        }

        [TestMethod]
        public void ResetFontCommandTest()
        {
            if (Application.Current is null)
            {
                using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);

                var command = window.ResetFontCommand;
                Assert.IsNotNull(command);

                var numChanged = 0;
                using var _ = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

                Assert.IsTrue(command.CanExecute());
                Assert.AreEqual(0, numChanged);

                command.Execute();
                Assert.AreEqual(0, numChanged);
            }
        }

        [TestMethod]
        public void ResetFontCommandTestDisposed()
        {
            if (Application.Current is null)
            {
                using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);

                var command = window.ResetFontCommand;
                Assert.IsNotNull(command);

                var numChanged = 0;
                using var disposable = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

                window.Dispose();

                Assert.IsTrue(command.CanExecute());
                Assert.AreEqual(0, numChanged);

                _ = Assert.ThrowsException<ObjectDisposedException>(() => command.Execute());
            }
        }

        [TestMethod]
        public void WithAppResetFontCommandTest()
        {
            var app = SetupApp();
            try
            {
                using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);

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
        public void WithAppResetFontCommandTestDisposed()
        {
            var app = SetupApp();
            try
            {
                using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);

                var command = window.ResetFontCommand;
                Assert.IsNotNull(command);

                var numChanged = 0;
                using var disposable = window.ObserveProperty(w => w.Font, false).Subscribe(_ => ++numChanged);

                window.Dispose();

                Assert.IsTrue(command.CanExecute());
                Assert.AreEqual(0, numChanged);

                Assert.ThrowsException<ObjectDisposedException>(() => command.Execute());
            }
            finally
            {
                app.Shutdown();
            }
        }

        [TestMethod]
        public void CanCloseDialogTest()
        {
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);
            Assert.IsTrue(window.CanCloseDialog());
        }

        [TestMethod]
        public void OnDialogClosedTest()
        {
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);
            window.OnDialogClosed();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void OnDialogOpenedTest()
        {
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);
            var parameters = new DialogParameters();
            window.OnDialogOpened(parameters);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void OnDialogOpenedTestNull()
        {
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);
            window.OnDialogOpened(null!);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void DisposeTest()
        {
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);
            window.Dispose();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void DisposeTestTwice()
        {
            using var window = new SettingWindowViewModel(DefaultSettingsMock.Object);
            window.Dispose();
            window.Dispose();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void FinalizerTest()
        {
            {
                _ = new SettingWindowViewModel(DefaultSettingsMock.Object);
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
