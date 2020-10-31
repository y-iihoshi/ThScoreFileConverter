using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prism.Services.Dialogs;
using Reactive.Bindings.Extensions;
using ThScoreFileConverter;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Interactivity;
using ThScoreFileConverter.Properties;
using ThScoreFileConverter.ViewModels;
using ThScoreFileConverter.Wrappers;
using ThScoreFileConverterTests.Extensions;
using WPFLocalizeExtension.Engine;
using Utils = ThScoreFileConverter.Models.Utils;

namespace ThScoreFileConverterTests.ViewModels
{
    [TestClass]
    public class MainWindowViewModelTests
    {
        private static Mock<IDialogService> MockDialogService() => new Mock<IDialogService>();

        private static Mock<ISettings> MockSettings()
        {
            var mock = new Mock<ISettings>();
            _ = mock.SetupProperty(m => m.LastTitle, string.Empty);
            _ = mock.SetupGet(m => m.Dictionary).Returns(new Dictionary<string, SettingsPerTitle>());
            return mock;
        }

        private static Mock<IDispatcherAdapter> MockDispatcherAdapter() => new Mock<IDispatcherAdapter>();

        private static MainWindowViewModel CreateViewModel()
        {
            var dialogServiceMock = MockDialogService();
            var settingsMock = MockSettings();
            var dispatcherAdapterMock = MockDispatcherAdapter();
            return new MainWindowViewModel(dialogServiceMock.Object, settingsMock.Object, dispatcherAdapterMock.Object);
        }

        private static DragEventArgs? CreateDragEventArgs(IDataObject data, RoutedEvent routedEvent)
        {
            var types = new[]
            {
                typeof(IDataObject),
                typeof(DragDropKeyStates),
                typeof(DragDropEffects),
                typeof(DependencyObject),
                typeof(Point),
            };
            var parameters = new object[]
            {
                data,
                DragDropKeyStates.None,
                DragDropEffects.None,
                new DependencyObject(),
                default(Point),
            };

            var constructor = typeof(DragEventArgs)
                .GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, types, null);
            if (constructor is null)
                return null;

            var dragEventArgs = (DragEventArgs)constructor.Invoke(parameters);
            dragEventArgs.RoutedEvent = routedEvent;

            return dragEventArgs;
        }

        [TestMethod]
        public void MainWindowViewModelTestNullDialogService()
        {
            var settingsMock = MockSettings();
            var dispatcherAdapterMock = MockDispatcherAdapter();
            _ = Assert.ThrowsException<ArgumentNullException>(
                () => new MainWindowViewModel(null!, settingsMock.Object, dispatcherAdapterMock.Object));
        }

        [TestMethod]
        public void MainWindowViewModelTestNullSettings()
        {
            var dialogServiceMock = MockDialogService();
            var dispatcherAdapterMock = MockDispatcherAdapter();
            _ = Assert.ThrowsException<ArgumentNullException>(
                () => new MainWindowViewModel(dialogServiceMock.Object, null!, dispatcherAdapterMock.Object));
        }

        [TestMethod]
        public void MainWindowViewModelTestNullDispatcherAdapter()
        {
            var dialogServiceMock = MockDialogService();
            var settingsMock = MockSettings();
            _ = Assert.ThrowsException<ArgumentNullException>(
                () => new MainWindowViewModel(dialogServiceMock.Object, settingsMock.Object, null!));
        }

        [TestMethod]
        public void TitleTest()
        {
            using var window = CreateViewModel();
            Assert.IsFalse(string.IsNullOrEmpty(window.Title));
        }

        [TestMethod]
        public void WorksTest()
        {
            using var window = CreateViewModel();
            Assert.IsTrue(window.Works.Any());
        }

        [TestMethod]
        public void IsIdleTest()
        {
            using var window = CreateViewModel();
            Assert.IsTrue(window.IsIdle.Value);
        }

        [TestMethod]
        public void CanHandleBestShotTest()
        {
            using var window = CreateViewModel();
            Assert.IsFalse(window.CanHandleBestShot);
        }

        [TestMethod]
        public void LastWorkNumberTest()
        {
            var dialogServiceMock = MockDialogService();
            var settingsMock = MockSettings();
            var initialLastTitle = settingsMock.Object.LastTitle;
            var dispatcherAdapterMock = MockDispatcherAdapter();
            using var window = new MainWindowViewModel(
                dialogServiceMock.Object, settingsMock.Object, dispatcherAdapterMock.Object);
            Assert.AreEqual(settingsMock.Object.LastTitle, window.LastWorkNumber);
            Assert.AreNotEqual(initialLastTitle, settingsMock.Object.LastTitle);

            var numChanged = 0;
            using var disposed = window.ObserveProperty(w => w.LastWorkNumber, false).Subscribe(_ => ++numChanged);

            var expected = "abc";
            window.LastWorkNumber = expected;
            Assert.AreEqual(1, numChanged);
            Assert.AreEqual(expected, window.LastWorkNumber);
            Assert.AreEqual(expected, settingsMock.Object.LastTitle);
        }

        [TestMethod]
        public void SupportedVersionsTest()
        {
            using var window = CreateViewModel();
            StringAssert.StartsWith(
                window.SupportedVersions, Utils.GetLocalizedValues<string>(nameof(Resources.SupportedVersion)));
        }

        [TestMethod]
        public void ScoreFileTest()
        {
            using var window = CreateViewModel();
            Assert.AreEqual(string.Empty, window.ScoreFile);
        }

        [TestMethod]
        public void OpenScoreFileDialogInitialDirectoryTest()
        {
            using var window = CreateViewModel();
            Assert.AreEqual(string.Empty, window.OpenScoreFileDialogInitialDirectory);
        }

        [TestMethod]
        public void BestShotDirectoryTest()
        {
            using var window = CreateViewModel();
            Assert.AreEqual(string.Empty, window.BestShotDirectory);
        }

        [TestMethod]
        public void TemplateFilesTest()
        {
            using var window = CreateViewModel();
            Assert.AreEqual(0, window.TemplateFiles.Count());
        }

        [TestMethod]
        public void OpenTemplateFilesDialogInitialDirectoryTest()
        {
            using var window = CreateViewModel();
            Assert.AreEqual(string.Empty, window.OpenTemplateFilesDialogInitialDirectory);
        }

        [TestMethod]
        public void OutputDirectoryTest()
        {
            using var window = CreateViewModel();
            Assert.AreEqual(string.Empty, window.OutputDirectory);
        }

        [TestMethod]
        public void ImageOutputDirectoryTest()
        {
            var dialogServiceMock = MockDialogService();
            var settingsMock = MockSettings();
            var dispatcherAdapterMock = MockDispatcherAdapter();
            using var window = new MainWindowViewModel(
                dialogServiceMock.Object, settingsMock.Object, dispatcherAdapterMock.Object);
            Assert.AreEqual(string.Empty, window.ImageOutputDirectory);

            var numChanged = 0;
            using var disposed =
                window.ObserveProperty(w => w.ImageOutputDirectory, false).Subscribe(_ => ++numChanged);

            var expected = "abc";
            window.ImageOutputDirectory = expected;
            Assert.AreEqual(1, numChanged);
            Assert.AreEqual(expected, window.ImageOutputDirectory);
            Assert.AreEqual(expected, settingsMock.Object.Dictionary[window.Works.First().Number].ImageOutputDirectory);
        }

        [TestMethod]
        public void CanReplaceCardNamesTest()
        {
            using var window = CreateViewModel();
            Assert.IsTrue(window.CanReplaceCardNames);
        }

        [TestMethod]
        public void HidesUntriedCardsTestWithoutConverter()
        {
            var dialogServiceMock = MockDialogService();
            var settingsMock = MockSettings();
            var dispatcherAdapterMock = MockDispatcherAdapter();
            using var window = new MainWindowViewModel(
                dialogServiceMock.Object, settingsMock.Object, dispatcherAdapterMock.Object);
            Assert.IsTrue(window.HidesUntriedCards);

            var numChanged = 0;
            using var disposed = window.ObserveProperty(w => w.HidesUntriedCards, false).Subscribe(_ => ++numChanged);

            var expected = false;
            window.HidesUntriedCards = expected;
            Assert.AreEqual(1, numChanged);
            Assert.AreEqual(expected, window.HidesUntriedCards);
            Assert.AreEqual(expected, settingsMock.Object.Dictionary[window.Works.First().Number].HideUntriedCards);
        }

        [TestMethod]
        public void LogTest()
        {
            using var window = CreateViewModel();
            Assert.AreEqual(string.Empty, window.Log.Value);
        }

        [TestMethod]
        public void SelectScoreFileCommandTest()
        {
            using var window = CreateViewModel();

            var command = window.SelectScoreFileCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var disposed = window.ObserveProperty(w => w.ScoreFile, false).Subscribe(_ => ++numChanged);

            var fileName = Path.GetTempFileName();
            try
            {
                var result = new OpenFileDialogActionResult(fileName, new string[] { });
                Assert.IsTrue(command.CanExecute(result));
                Assert.AreEqual(0, numChanged);

                command.Execute(result);
                Assert.AreEqual(1, numChanged);
                Assert.AreEqual(fileName, window.ScoreFile);
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void SelectScoreFileCommandTestNoChange()
        {
            using var window = CreateViewModel();

            var command = window.SelectScoreFileCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var disposed = window.ObserveProperty(w => w.ScoreFile, false).Subscribe(_ => ++numChanged);

            var fileName = window.ScoreFile;
            var result = new OpenFileDialogActionResult(fileName, new string[] { });
            Assert.IsTrue(command.CanExecute(result));
            Assert.AreEqual(0, numChanged);

            command.Execute(result);
            Assert.AreEqual(0, numChanged);
        }

        [TestMethod]
        public void SelectScoreFileCommandTestNonexistent()
        {
            using var window = CreateViewModel();

            var command = window.SelectScoreFileCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var disposed = window.ObserveProperty(w => w.ScoreFile, false).Subscribe(_ => ++numChanged);

            var fileName = "nonexistent.txt";
            var result = new OpenFileDialogActionResult(fileName, new string[] { });
            Assert.IsTrue(command.CanExecute(result));
            Assert.AreEqual(0, numChanged);

            command.Execute(result);
            Assert.AreEqual(0, numChanged);
        }

        [TestMethod]
        public void SelectBestShotDirectoryCommandTest()
        {
            using var window = CreateViewModel();

            var command = window.SelectBestShotDirectoryCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var disposed = window.ObserveProperty(w => w.BestShotDirectory, false).Subscribe(_ => ++numChanged);

            var path = Path.GetTempPath();
            var result = new FolderBrowserDialogActionResult(path);
            Assert.IsTrue(command.CanExecute(result));
            Assert.AreEqual(0, numChanged);

            command.Execute(result);
            Assert.AreEqual(1, numChanged);
            Assert.AreEqual(path, window.BestShotDirectory);
        }

        [TestMethod]
        public void SelectBestShotDirectoryCommandTestNoChange()
        {
            using var window = CreateViewModel();

            var command = window.SelectBestShotDirectoryCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var disposed = window.ObserveProperty(w => w.BestShotDirectory, false).Subscribe(_ => ++numChanged);

            var path = window.BestShotDirectory;
            var result = new FolderBrowserDialogActionResult(path);
            Assert.IsTrue(command.CanExecute(result));
            Assert.AreEqual(0, numChanged);

            command.Execute(result);
            Assert.AreEqual(0, numChanged);
        }

        [TestMethod]
        public void SelectBestShotDirectoryCommandTestNonexistent()
        {
            using var window = CreateViewModel();

            var command = window.SelectBestShotDirectoryCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var disposed = window.ObserveProperty(w => w.BestShotDirectory, false).Subscribe(_ => ++numChanged);

            var path = "nonexistent";
            var result = new FolderBrowserDialogActionResult(path);
            Assert.IsTrue(command.CanExecute(result));
            Assert.AreEqual(0, numChanged);

            command.Execute(result);
            Assert.AreEqual(0, numChanged);
        }

        [TestMethod]
        public void TemplateFilesSelectionChangedCommandTest()
        {
            using var window = CreateViewModel();

            var command = window.TemplateFilesSelectionChangedCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var disposed = window.DeleteTemplateFilesCommand
                .CanExecuteChangedAsObservable().Subscribe(_ => ++numChanged);

            Assert.IsTrue(command.CanExecute());
            Assert.AreEqual(0, numChanged);

            command.Execute();
            Assert.AreEqual(1, numChanged);
        }

        [TestMethod]
        public void AddTemplateFilesCommandTest()
        {
            using var window = CreateViewModel();

            var command = window.AddTemplateFilesCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var disposed = window.ObserveProperty(w => w.TemplateFiles, false).Subscribe(_ => ++numChanged);

            var fileNames = Enumerable.Range(1, 3).Select(_ => Path.GetTempFileName()).ToArray();
            try
            {
                var result = new OpenFileDialogActionResult(string.Empty, fileNames);
                Assert.IsTrue(command.CanExecute(result));
                Assert.AreEqual(0, numChanged);

                command.Execute(result);
                Assert.AreEqual(1, numChanged);
                CollectionAssert.That.AreEqual(fileNames, window.TemplateFiles);
            }
            finally
            {
                foreach (var fileName in fileNames)
                    File.Delete(fileName);
            }
        }

        [TestMethod]
        public void AddTemplateFilesCommandTestAppended()
        {
            using var window = CreateViewModel();

            var command = window.AddTemplateFilesCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var disposed = window.ObserveProperty(w => w.TemplateFiles, false).Subscribe(_ => ++numChanged);

            var fileNames = Enumerable.Range(1, 5).Select(_ => Path.GetTempFileName()).ToArray();
            try
            {
                var fileNames1 = fileNames.Take(3);
                var fileNames2 = fileNames.TakeLast(3);

                var result = new OpenFileDialogActionResult(string.Empty, fileNames1);
                Assert.IsTrue(command.CanExecute(result));
                Assert.AreEqual(0, numChanged);

                command.Execute(result);
                Assert.AreEqual(1, numChanged);
                CollectionAssert.That.AreEqual(fileNames1, window.TemplateFiles);

                result = new OpenFileDialogActionResult(string.Empty, fileNames2);
                Assert.IsTrue(command.CanExecute(result));
                Assert.AreEqual(1, numChanged);

                command.Execute(result);
                Assert.AreEqual(2, numChanged);
                CollectionAssert.That.AreEqual(fileNames, window.TemplateFiles);
            }
            finally
            {
                foreach (var fileName in fileNames)
                    File.Delete(fileName);
            }
        }

        [TestMethod]
        public void AddTemplateFilesCommandTestNoChange()
        {
            using var window = CreateViewModel();

            var command = window.AddTemplateFilesCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var disposed = window.ObserveProperty(w => w.TemplateFiles, false).Subscribe(_ => ++numChanged);

            var fileNames = window.TemplateFiles.ToArray();
            var result = new OpenFileDialogActionResult(string.Empty, fileNames);
            Assert.IsTrue(command.CanExecute(result));
            Assert.AreEqual(0, numChanged);

            command.Execute(result);
            Assert.AreEqual(1, numChanged);
            CollectionAssert.That.AreEqual(fileNames, window.TemplateFiles);
        }

        [TestMethod]
        public void AddTemplateFilesCommandTestNonexistent()
        {
            using var window = CreateViewModel();

            var command = window.AddTemplateFilesCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var disposed = window.ObserveProperty(w => w.TemplateFiles, false).Subscribe(_ => ++numChanged);

            var fileNames = new[] { "nonexistent.txt" };
            var result = new OpenFileDialogActionResult(string.Empty, fileNames);
            Assert.IsTrue(command.CanExecute(result));
            Assert.AreEqual(0, numChanged);

            command.Execute(result);
            Assert.AreEqual(1, numChanged);
            Assert.AreEqual(0, window.TemplateFiles.Count());
        }

        [TestMethod]
        public void DeleteTemplateFilesCommandTest()
        {
            using var window = CreateViewModel();

            var command = window.DeleteTemplateFilesCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var disposed = window.ObserveProperty(w => w.TemplateFiles, false).Subscribe(_ => ++numChanged);

            var fileNames = Enumerable.Range(1, 3).Select(_ => Path.GetTempFileName()).ToArray();
            try
            {
                var result = new OpenFileDialogActionResult(string.Empty, fileNames);
                window.AddTemplateFilesCommand.Execute(result);
                Assert.AreEqual(1, numChanged);
                CollectionAssert.That.AreEqual(fileNames, window.TemplateFiles);

                Assert.IsTrue(command.CanExecute(fileNames));
                Assert.AreEqual(1, numChanged);

                command.Execute(fileNames.Take(2).ToList());
                Assert.AreEqual(2, numChanged);
                CollectionAssert.That.AreEqual(fileNames.TakeLast(1), window.TemplateFiles);
            }
            finally
            {
                foreach (var fileName in fileNames)
                    File.Delete(fileName);
            }
        }

        [TestMethod]
        public void DeleteTemplateFilesCommandTestAll()
        {
            using var window = CreateViewModel();

            var command = window.DeleteTemplateFilesCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var disposed = window.ObserveProperty(w => w.TemplateFiles, false).Subscribe(_ => ++numChanged);

            var fileNames = Enumerable.Range(1, 3).Select(_ => Path.GetTempFileName()).ToArray();
            try
            {
                var result = new OpenFileDialogActionResult(string.Empty, fileNames);
                window.AddTemplateFilesCommand.Execute(result);
                Assert.AreEqual(1, numChanged);
                CollectionAssert.That.AreEqual(fileNames, window.TemplateFiles);

                Assert.IsTrue(command.CanExecute(fileNames));
                Assert.AreEqual(1, numChanged);

                command.Execute(fileNames.ToList());
                Assert.AreEqual(2, numChanged);
                Assert.AreEqual(0, window.TemplateFiles.Count());
            }
            finally
            {
                foreach (var fileName in fileNames)
                    File.Delete(fileName);
            }
        }

        [TestMethod]
        public void DeleteTemplateFilesCommandTestNull()
        {
            using var window = CreateViewModel();

            var command = window.DeleteTemplateFilesCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var disposed = window.ObserveProperty(w => w.TemplateFiles, false).Subscribe(_ => ++numChanged);

            var fileNames = Enumerable.Range(1, 3).Select(_ => Path.GetTempFileName()).ToArray();
            try
            {
                var result = new OpenFileDialogActionResult(string.Empty, fileNames);
                window.AddTemplateFilesCommand.Execute(result);
                Assert.AreEqual(1, numChanged);
                CollectionAssert.That.AreEqual(fileNames, window.TemplateFiles);

                Assert.IsFalse(command.CanExecute(null));
                Assert.AreEqual(1, numChanged);

                command.Execute(null);
                Assert.AreEqual(1, numChanged);
                CollectionAssert.That.AreEqual(fileNames, window.TemplateFiles);
            }
            finally
            {
                foreach (var fileName in fileNames)
                    File.Delete(fileName);
            }
        }

        [TestMethod]
        public void DeleteTemplateFilesCommandTestEmpty()
        {
            using var window = CreateViewModel();

            var command = window.DeleteTemplateFilesCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var disposed = window.ObserveProperty(w => w.TemplateFiles, false).Subscribe(_ => ++numChanged);

            var fileNames = Enumerable.Range(1, 3).Select(_ => Path.GetTempFileName()).ToArray();
            try
            {
                var result = new OpenFileDialogActionResult(string.Empty, fileNames);
                window.AddTemplateFilesCommand.Execute(result);
                Assert.AreEqual(1, numChanged);
                CollectionAssert.That.AreEqual(fileNames, window.TemplateFiles);

                var empty = new string[] { };

                Assert.IsFalse(command.CanExecute(empty));
                Assert.AreEqual(1, numChanged);

                command.Execute(empty);
                Assert.AreEqual(2, numChanged);
                CollectionAssert.That.AreEqual(fileNames, window.TemplateFiles);
            }
            finally
            {
                foreach (var fileName in fileNames)
                    File.Delete(fileName);
            }
        }

        [TestMethod]
        public void DeleteAllTemplateFilesCommandTest()
        {
            using var window = CreateViewModel();

            var command = window.DeleteAllTemplateFilesCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var disposed = window.ObserveProperty(w => w.TemplateFiles, false).Subscribe(_ => ++numChanged);

            var fileNames = Enumerable.Range(1, 3).Select(_ => Path.GetTempFileName()).ToArray();
            try
            {
                var result = new OpenFileDialogActionResult(string.Empty, fileNames);
                window.AddTemplateFilesCommand.Execute(result);
                Assert.AreEqual(1, numChanged);
                CollectionAssert.That.AreEqual(fileNames, window.TemplateFiles);

                Assert.IsTrue(command.CanExecute());
                Assert.AreEqual(1, numChanged);

                command.Execute();
                Assert.AreEqual(2, numChanged);
                Assert.AreEqual(0, window.TemplateFiles.Count());
            }
            finally
            {
                foreach (var fileName in fileNames)
                    File.Delete(fileName);
            }
        }

        [TestMethod]
        public void DeleteAllTemplateFilesCommandTestEmpty()
        {
            using var window = CreateViewModel();

            var command = window.DeleteAllTemplateFilesCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var disposed = window.ObserveProperty(w => w.TemplateFiles, false).Subscribe(_ => ++numChanged);

            Assert.IsFalse(command.CanExecute());
            Assert.AreEqual(0, numChanged);

            command.Execute();
            Assert.AreEqual(1, numChanged);
            Assert.AreEqual(0, window.TemplateFiles.Count());
        }

        [TestMethod]
        public void SelectOutputDirectoryCommandTest()
        {
            using var window = CreateViewModel();

            var command = window.SelectOutputDirectoryCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var disposed = window.ObserveProperty(w => w.OutputDirectory, false).Subscribe(_ => ++numChanged);

            var path = Path.GetTempPath();
            var result = new FolderBrowserDialogActionResult(path);
            Assert.IsTrue(command.CanExecute(result));
            Assert.AreEqual(0, numChanged);

            command.Execute(result);
            Assert.AreEqual(1, numChanged);
            Assert.AreEqual(path, window.OutputDirectory);
        }

        [TestMethod]
        public void SelectOutputDirectoryCommandTestNoChange()
        {
            using var window = CreateViewModel();

            var command = window.SelectOutputDirectoryCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var disposed = window.ObserveProperty(w => w.OutputDirectory, false).Subscribe(_ => ++numChanged);

            var path = window.OutputDirectory;
            var result = new FolderBrowserDialogActionResult(path);
            Assert.IsTrue(command.CanExecute(result));
            Assert.AreEqual(0, numChanged);

            command.Execute(result);
            Assert.AreEqual(0, numChanged);
        }

        [TestMethod]
        public void SelectOutputDirectoryCommandTestNonexistent()
        {
            using var window = CreateViewModel();

            var command = window.SelectOutputDirectoryCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var disposed = window.ObserveProperty(w => w.OutputDirectory, false).Subscribe(_ => ++numChanged);

            var path = "nonexistent";
            var result = new FolderBrowserDialogActionResult(path);
            Assert.IsTrue(command.CanExecute(result));
            Assert.AreEqual(0, numChanged);

            command.Execute(result);
            Assert.AreEqual(0, numChanged);
        }

        [TestMethod]
        public void ConvertCommandTest()
        {
            using var window = CreateViewModel();

            var command = window.ConvertCommand;
            Assert.IsNotNull(command);

            Assert.IsFalse(command.CanExecute());

            command.Execute();
            Assert.IsTrue(window.IsIdle.Value);
            Assert.AreEqual(string.Empty, window.Log.Value);
        }

        [TestMethod]
        public void DraggingCommandTest()
        {
            using var window = CreateViewModel();

            var command = window.DraggingCommand;
            Assert.IsNotNull(command);

            var args = CreateDragEventArgs(
                new DataObject(DataFormats.FileDrop, new object()), UIElement.PreviewDragEnterEvent);
            Assert.IsNotNull(args);
            Assert.IsTrue(command.CanExecute(args!));

            command.Execute(args!);
            Assert.AreEqual(DragDropEffects.Copy, args!.Effects);
            Assert.IsTrue(args.Handled);
        }

        [TestMethod]
        public void DraggingCommandTestNone()
        {
            using var window = CreateViewModel();

            var command = window.DraggingCommand;
            Assert.IsNotNull(command);

            var args = CreateDragEventArgs(
                new DataObject(DataFormats.Text, new object()), UIElement.PreviewDragEnterEvent);
            Assert.IsNotNull(args);
            Assert.IsTrue(command.CanExecute(args!));

            command.Execute(args!);
            Assert.AreEqual(DragDropEffects.None, args!.Effects);
            Assert.IsFalse(args.Handled);
        }

        [TestMethod]
        public void DropScoreFileCommandTest()
        {
            using var window = CreateViewModel();

            var command = window.DropScoreFileCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var _ = window.ObserveProperty(w => w.ScoreFile, false).Subscribe(_ => ++numChanged);

            var fileNames = Enumerable.Range(1, 3).Select(_ => Path.GetTempFileName()).ToArray();
            try
            {
                var args = CreateDragEventArgs(new DataObject(DataFormats.FileDrop, fileNames), UIElement.DropEvent);
                Assert.IsNotNull(args);
                Assert.IsTrue(command.CanExecute(args!));

                command.Execute(args!);
                Assert.AreEqual(1, numChanged);
                Assert.AreEqual(fileNames[0], window.ScoreFile);
            }
            finally
            {
                foreach (var fileName in fileNames)
                    File.Delete(fileName);
            }
        }

        [TestMethod]
        public void DropScoreFileCommandTestNonexistent()
        {
            using var window = CreateViewModel();

            var command = window.DropScoreFileCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var _ = window.ObserveProperty(w => w.ScoreFile, false).Subscribe(_ => ++numChanged);

            var fileNames = new[] { "nonexistent.txt" };

            var args = CreateDragEventArgs(new DataObject(DataFormats.FileDrop, fileNames), UIElement.DropEvent);
            Assert.IsNotNull(args);
            Assert.IsTrue(command.CanExecute(args!));

            command.Execute(args!);
            Assert.AreEqual(0, numChanged);
            Assert.AreEqual(string.Empty, window.ScoreFile);
        }

        [TestMethod]
        public void DropScoreFileCommandTestInvalidData()
        {
            using var window = CreateViewModel();

            var command = window.DropScoreFileCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var _ = window.ObserveProperty(w => w.ScoreFile, false).Subscribe(_ => ++numChanged);

            var args = CreateDragEventArgs(new DataObject(DataFormats.FileDrop, default(int)), UIElement.DropEvent);
            Assert.IsNotNull(args);
            Assert.IsTrue(command.CanExecute(args!));

            command.Execute(args!);
            Assert.AreEqual(0, numChanged);
            Assert.AreEqual(string.Empty, window.ScoreFile);
        }

        [TestMethod]
        public void DropScoreFileCommandTestInvalidDataFormat()
        {
            using var window = CreateViewModel();

            var command = window.DropScoreFileCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var _ = window.ObserveProperty(w => w.ScoreFile, false).Subscribe(_ => ++numChanged);

            var args = CreateDragEventArgs(new DataObject(DataFormats.Text, string.Empty), UIElement.DropEvent);
            Assert.IsNotNull(args);
            Assert.IsTrue(command.CanExecute(args!));

            command.Execute(args!);
            Assert.AreEqual(0, numChanged);
            Assert.AreEqual(string.Empty, window.ScoreFile);
        }

        [TestMethod]
        public void DropBestShotDirectoryCommandTest()
        {
            using var window = CreateViewModel();

            var command = window.DropBestShotDirectoryCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            _ = window.ObserveProperty(w => w.BestShotDirectory, false).Subscribe(_ => ++numChanged);

            var dirNames = Enumerable.Range(1, 3).Select(index =>
            {
                var path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                _ = Directory.CreateDirectory(path);
                return path;
            }).ToArray();

            try
            {
                var args = CreateDragEventArgs(new DataObject(DataFormats.FileDrop, dirNames), UIElement.DropEvent);
                Assert.IsNotNull(args);
                Assert.IsTrue(command.CanExecute(args!));

                command.Execute(args!);
                Assert.AreEqual(1, numChanged);
                Assert.AreEqual(dirNames[0], window.BestShotDirectory);
            }
            finally
            {
                foreach (var dirName in dirNames)
                    Directory.Delete(dirName);
            }
        }

        [TestMethod]
        public void DropBestShotDirectoryCommandTestNonexistent()
        {
            using var window = CreateViewModel();

            var command = window.DropBestShotDirectoryCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var _ = window.ObserveProperty(w => w.BestShotDirectory, false).Subscribe(_ => ++numChanged);

            var dirNames = new[] { "nonexistent" };

            var args = CreateDragEventArgs(new DataObject(DataFormats.FileDrop, dirNames), UIElement.DropEvent);
            Assert.IsNotNull(args);
            Assert.IsTrue(command.CanExecute(args!));

            command.Execute(args!);
            Assert.AreEqual(0, numChanged);
            Assert.AreEqual(string.Empty, window.BestShotDirectory);
        }

        [TestMethod]
        public void DropBestShotDirectoryCommandTestInvalidData()
        {
            using var window = CreateViewModel();

            var command = window.DropBestShotDirectoryCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var _ = window.ObserveProperty(w => w.BestShotDirectory, false).Subscribe(_ => ++numChanged);

            var args = CreateDragEventArgs(new DataObject(DataFormats.FileDrop, default(int)), UIElement.DropEvent);
            Assert.IsNotNull(args);
            Assert.IsTrue(command.CanExecute(args!));

            command.Execute(args!);
            Assert.AreEqual(0, numChanged);
            Assert.AreEqual(string.Empty, window.BestShotDirectory);
        }

        [TestMethod]
        public void DropBestShotDirectoryCommandTestInvalidDataFormat()
        {
            using var window = CreateViewModel();

            var command = window.DropBestShotDirectoryCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var _ = window.ObserveProperty(w => w.BestShotDirectory, false).Subscribe(_ => ++numChanged);

            var args = CreateDragEventArgs(new DataObject(DataFormats.Text, string.Empty), UIElement.DropEvent);
            Assert.IsNotNull(args);
            Assert.IsTrue(command.CanExecute(args!));

            command.Execute(args!);
            Assert.AreEqual(0, numChanged);
            Assert.AreEqual(string.Empty, window.BestShotDirectory);
        }

        [TestMethod]
        public void DropTemplateFilesCommandTest()
        {
            using var window = CreateViewModel();

            var command = window.DropTemplateFilesCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var _ = window.ObserveProperty(w => w.TemplateFiles, false).Subscribe(_ => ++numChanged);

            var fileNames = Enumerable.Range(1, 3).Select(_ => Path.GetTempFileName()).ToArray();
            try
            {
                var args = CreateDragEventArgs(
                    new DataObject(DataFormats.FileDrop, fileNames.Append("nonexistent.txt").ToArray()),
                    UIElement.DropEvent);
                Assert.IsNotNull(args);
                Assert.IsTrue(command.CanExecute(args!));

                command.Execute(args!);
                Assert.AreEqual(1, numChanged);
                CollectionAssert.That.AreEqual(fileNames, window.TemplateFiles);
            }
            finally
            {
                foreach (var fileName in fileNames)
                    File.Delete(fileName);
            }
        }

        [TestMethod]
        public void DropTemplateFilesCommandTestInvalidData()
        {
            using var window = CreateViewModel();

            var command = window.DropTemplateFilesCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var _ = window.ObserveProperty(w => w.TemplateFiles, false).Subscribe(_ => ++numChanged);

            var args = CreateDragEventArgs(new DataObject(DataFormats.FileDrop, default(int)), UIElement.DropEvent);
            Assert.IsNotNull(args);
            Assert.IsTrue(command.CanExecute(args!));

            command.Execute(args!);
            Assert.AreEqual(0, numChanged);
            Assert.AreEqual(0, window.TemplateFiles.Count());
        }

        [TestMethod]
        public void DropTemplateFilesCommandTestInvalidDataFormat()
        {
            using var window = CreateViewModel();

            var command = window.DropTemplateFilesCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var _ = window.ObserveProperty(w => w.TemplateFiles, false).Subscribe(_ => ++numChanged);

            var args = CreateDragEventArgs(new DataObject(DataFormats.Text, string.Empty), UIElement.DropEvent);
            Assert.IsNotNull(args);
            Assert.IsTrue(command.CanExecute(args!));

            command.Execute(args!);
            Assert.AreEqual(0, numChanged);
            Assert.AreEqual(0, window.TemplateFiles.Count());
        }

        [TestMethod]
        public void DropOutputDirectoryCommandTest()
        {
            using var window = CreateViewModel();

            var command = window.DropOutputDirectoryCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            _ = window.ObserveProperty(w => w.OutputDirectory, false).Subscribe(_ => ++numChanged);

            var dirNames = Enumerable.Range(1, 3).Select(index =>
            {
                var path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                _ = Directory.CreateDirectory(path);
                return path;
            }).ToArray();

            try
            {
                var args = CreateDragEventArgs(new DataObject(DataFormats.FileDrop, dirNames), UIElement.DropEvent);
                Assert.IsNotNull(args);
                Assert.IsTrue(command.CanExecute(args!));

                command.Execute(args!);
                Assert.AreEqual(1, numChanged);
                Assert.AreEqual(dirNames[0], window.OutputDirectory);
            }
            finally
            {
                foreach (var dirName in dirNames)
                    Directory.Delete(dirName);
            }
        }

        [TestMethod]
        public void DropOutputDirectoryCommandTestNonexistent()
        {
            using var window = CreateViewModel();

            var command = window.DropOutputDirectoryCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var _ = window.ObserveProperty(w => w.OutputDirectory, false).Subscribe(_ => ++numChanged);

            var dirNames = new[] { "nonexistent" };

            var args = CreateDragEventArgs(new DataObject(DataFormats.FileDrop, dirNames), UIElement.DropEvent);
            Assert.IsNotNull(args);
            Assert.IsTrue(command.CanExecute(args!));

            command.Execute(args!);
            Assert.AreEqual(0, numChanged);
            Assert.AreEqual(string.Empty, window.OutputDirectory);
        }

        [TestMethod]
        public void DropOutputDirectoryCommandTestInvalidData()
        {
            using var window = CreateViewModel();

            var command = window.DropOutputDirectoryCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var _ = window.ObserveProperty(w => w.OutputDirectory, false).Subscribe(_ => ++numChanged);

            var args = CreateDragEventArgs(new DataObject(DataFormats.FileDrop, default(int)), UIElement.DropEvent);
            Assert.IsNotNull(args);
            Assert.IsTrue(command.CanExecute(args!));

            command.Execute(args!);
            Assert.AreEqual(0, numChanged);
            Assert.AreEqual(string.Empty, window.OutputDirectory);
        }

        [TestMethod]
        public void DropOutputDirectoryCommandTestInvalidDataFormat()
        {
            using var window = CreateViewModel();

            var command = window.DropOutputDirectoryCommand;
            Assert.IsNotNull(command);

            var numChanged = 0;
            using var _ = window.ObserveProperty(w => w.OutputDirectory, false).Subscribe(_ => ++numChanged);

            var args = CreateDragEventArgs(new DataObject(DataFormats.Text, string.Empty), UIElement.DropEvent);
            Assert.IsNotNull(args);
            Assert.IsTrue(command.CanExecute(args!));

            command.Execute(args!);
            Assert.AreEqual(0, numChanged);
            Assert.AreEqual(string.Empty, window.OutputDirectory);
        }

        [TestMethod]
        public void OpenAboutWindowCommandTest()
        {
            var dialogServiceMock = MockDialogService();
            var settingsMock = MockSettings();
            var dispatcherAdapterMock = MockDispatcherAdapter();

            using var window = new MainWindowViewModel(
                dialogServiceMock.Object, settingsMock.Object, dispatcherAdapterMock.Object);

            var command = window.OpenAboutWindowCommand;
            Assert.IsNotNull(command);

            Assert.IsTrue(command.CanExecute());

            command.Execute();
            dialogServiceMock.Verify(dialogService => dialogService.ShowDialog(
                nameof(AboutWindowViewModel), It.IsAny<DialogParameters>(), It.IsAny<Action<IDialogResult>>()));
        }

        [TestMethod]
        public void OpenSettingWindowCommandTest()
        {
            var dialogServiceMock = MockDialogService();
            var settingsMock = MockSettings();
            var dispatcherAdapterMock = MockDispatcherAdapter();

            using var window = new MainWindowViewModel(
                dialogServiceMock.Object, settingsMock.Object, dispatcherAdapterMock.Object);

            var command = window.OpenSettingWindowCommand;
            Assert.IsNotNull(command);

            Assert.IsTrue(command.CanExecute());

            command.Execute();
            dialogServiceMock.Verify(dialogService => dialogService.ShowDialog(
                nameof(SettingWindowViewModel), It.IsAny<DialogParameters>(), It.IsAny<Action<IDialogResult>>()));
        }

        [TestMethod]
        public void DisposeTest()
        {
            using var window = CreateViewModel();
            window.Dispose();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void DisposeTestTwice()
        {
            using var window = CreateViewModel();
            window.Dispose();
            window.Dispose();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void FinalizerTest()
        {
            {
                _ = CreateViewModel();
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
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

                var numChanged = 0;
                using var _ = window.ObserveProperty(w => w.SupportedVersions, false).Subscribe(_ => ++numChanged);

                var expected = CultureInfo.GetCultureInfo("ja-JP");
                LocalizeDictionary.Instance.Culture = expected;
                Assert.AreEqual(1, numChanged);
            }
            finally
            {
                LocalizeDictionary.Instance.Culture = backupCulture;
            }
        }
    }
}
