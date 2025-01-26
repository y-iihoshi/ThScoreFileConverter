using System.Globalization;
using System.Reflection;
using System.Windows;
using MvvmDialogs;
using NSubstitute;
using Reactive.Bindings.Extensions;
using ThScoreFileConverter.Adapters;
using ThScoreFileConverter.Interactivity;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Resources;
using ThScoreFileConverter.ViewModels;
using WPFLocalizeExtension.Engine;

namespace ThScoreFileConverter.Tests.ViewModels;

[TestClass]
public class MainWindowViewModelTests
{
    private static IDialogService MockDialogService()
    {
        return Substitute.For<IDialogService>();
    }

    private static IDispatcherAdapter MockDispatcherAdapter()
    {
        return Substitute.For<IDispatcherAdapter>();
    }

    private static INumberFormatter MockNumberFormatter()
    {
        return Substitute.For<INumberFormatter>();
    }

    private static MainWindowViewModel CreateViewModel()
    {
        var dialogServiceMock = MockDialogService();
        var dispatcherAdapterMock = MockDispatcherAdapter();
        var settings = new Settings();
        var formatterMock = MockNumberFormatter();
        return new MainWindowViewModel(
            dialogServiceMock, dispatcherAdapterMock, settings, formatterMock);
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
    public void MinWidthTest()
    {
        using var window = CreateViewModel();
        window.MinWidth.ShouldBe(Settings.WindowMinWidth);
    }

    [TestMethod]
    public void MinHeightTest()
    {
        using var window = CreateViewModel();
        window.MinHeight.ShouldBe(Settings.WindowMinHeight);
    }

    [TestMethod]
    public void MainContentMinHeightTest()
    {
        using var window = CreateViewModel();
        window.MainContentMinHeight.ShouldBe(Settings.MainContentMinHeight);
    }

    [TestMethod]
    public void SubContentMinHeightTest()
    {
        using var window = CreateViewModel();
        window.SubContentMinHeight.ShouldBe(Settings.SubContentMinHeight);
    }

    [TestMethod]
    public void TitleTest()
    {
        using var window = CreateViewModel();
        window.Title.ShouldNotBeNullOrEmpty();
    }

    [TestMethod]
    public void WorksTest()
    {
        using var window = CreateViewModel();
        window.Works.ShouldNotBeEmpty();
    }

    [TestMethod]
    public void IsIdleTest()
    {
        using var window = CreateViewModel();
        window.IsIdle.Value.ShouldBeTrue();
    }

    [TestMethod]
    public void CanHandleBestShotTest()
    {
        using var window = CreateViewModel();
        window.CanHandleBestShot.ShouldBeFalse();
    }

    [TestMethod]
    public void LastWorkNumberTest()
    {
        var dialogServiceMock = MockDialogService();
        var dispatcherAdapterMock = MockDispatcherAdapter();
        var settings = new Settings();
        var initialLastTitle = settings.LastTitle;
        var formatterMock = MockNumberFormatter();
        using var window = new MainWindowViewModel(
            dialogServiceMock, dispatcherAdapterMock, settings, formatterMock);
        window.LastWorkNumber.Value.ShouldBe(settings.LastTitle);
        settings.LastTitle.ShouldNotBe(initialLastTitle);

        var numChanged = 0;
        using var disposable = window.LastWorkNumber.Subscribe(_ => ++numChanged);

        var expected = "TH07";
        window.LastWorkNumber.Value = expected;
        numChanged.ShouldBe(1);
        window.LastWorkNumber.Value.ShouldBe(expected);
        settings.LastTitle.ShouldBe(expected);
    }

    [TestMethod]
    public void SupportedVersionsTest()
    {
        using var window = CreateViewModel();
        window.SupportedVersions.ShouldStartWith(Utils.GetLocalizedValues<string>(nameof(StringResources.SupportedVersion)));
    }

    [TestMethod]
    public void ScoreFileTest()
    {
        using var window = CreateViewModel();
        window.ScoreFile.Value.ShouldBeEmpty();
    }

    [TestMethod]
    public void OpenScoreFileDialogInitialDirectoryTest()
    {
        using var window = CreateViewModel();
        window.OpenScoreFileDialogInitialDirectory.Value.ShouldBeEmpty();
    }

    [TestMethod]
    public void BestShotDirectoryTest()
    {
        using var window = CreateViewModel();
        window.BestShotDirectory.Value.ShouldBeEmpty();
    }

    [TestMethod]
    public void TemplateFilesTest()
    {
        using var window = CreateViewModel();
        window.TemplateFiles.Value.ShouldBeEmpty();
    }

    [TestMethod]
    public void OpenTemplateFilesDialogInitialDirectoryTest()
    {
        using var window = CreateViewModel();
        window.OpenTemplateFilesDialogInitialDirectory.Value.ShouldBeEmpty();
    }

    [TestMethod]
    public void OutputDirectoryTest()
    {
        using var window = CreateViewModel();
        window.OutputDirectory.Value.ShouldBeEmpty();
    }

    [TestMethod]
    public void ImageOutputDirectoryTest()
    {
        var dialogServiceMock = MockDialogService();
        var dispatcherAdapterMock = MockDispatcherAdapter();
        var settings = new Settings();
        var formatterMock = MockNumberFormatter();
        using var window = new MainWindowViewModel(
            dialogServiceMock, dispatcherAdapterMock, settings, formatterMock);
        window.ImageOutputDirectory.Value.ShouldBeEmpty();

        var numChanged = 0;
        using var disposable = window.ImageOutputDirectory.Subscribe(_ => ++numChanged);

        var expected = "abc";
        window.ImageOutputDirectory.Value = expected;
        numChanged.ShouldBe(1);
        window.ImageOutputDirectory.Value.ShouldBe(expected);
        settings.GetSettingsPerTitle(window.Works.First().Number).ImageOutputDirectory.ShouldBe(expected);
    }

    [TestMethod]
    public void CanReplaceCardNamesTest()
    {
        using var window = CreateViewModel();
        window.CanReplaceCardNames.ShouldBeTrue();
    }

    [TestMethod]
    public void HidesUntriedCardsTestWithoutConverter()
    {
        var dialogServiceMock = MockDialogService();
        var dispatcherAdapterMock = MockDispatcherAdapter();
        var settings = new Settings();
        var formatterMock = MockNumberFormatter();
        using var window = new MainWindowViewModel(
            dialogServiceMock, dispatcherAdapterMock, settings, formatterMock);
        window.HidesUntriedCards.Value.ShouldBeTrue();

        var numChanged = 0;
        using var disposable = window.HidesUntriedCards.Subscribe(_ => ++numChanged);

        var expected = false;
        window.HidesUntriedCards.Value = expected;
        numChanged.ShouldBe(1);
        window.HidesUntriedCards.Value.ShouldBe(expected);
        settings.GetSettingsPerTitle(window.Works.First().Number).HideUntriedCards.ShouldBe(expected);
    }

    [TestMethod]
    public void LogTest()
    {
        using var window = CreateViewModel();
        window.Log.Value.ShouldBeEmpty();
    }

    [TestMethod]
    public void SelectScoreFileCommandTest()
    {
        using var window = CreateViewModel();

        var command = window.SelectScoreFileCommand;
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.ScoreFile.Subscribe(_ => ++numChanged);

        var fileName = Path.GetTempFileName();
        try
        {
            var result = new OpenFileDialogActionResult(fileName, []);
            command.CanExecute(result).ShouldBeTrue();
            numChanged.ShouldBe(0);

            command.Execute(result);
            numChanged.ShouldBe(1);
            window.ScoreFile.Value.ShouldBe(fileName);
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
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.ScoreFile.Subscribe(_ => ++numChanged);

        var fileName = window.ScoreFile.Value;
        var result = new OpenFileDialogActionResult(fileName, []);
        command.CanExecute(result).ShouldBeTrue();
        numChanged.ShouldBe(0);

        command.Execute(result);
        numChanged.ShouldBe(0);
    }

    [TestMethod]
    public void SelectScoreFileCommandTestNonexistent()
    {
        using var window = CreateViewModel();

        var command = window.SelectScoreFileCommand;
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.ScoreFile.Subscribe(_ => ++numChanged);

        var fileName = "nonexistent.txt";
        var result = new OpenFileDialogActionResult(fileName, []);
        command.CanExecute(result).ShouldBeTrue();
        numChanged.ShouldBe(0);

        command.Execute(result);
        numChanged.ShouldBe(0);
    }

    [TestMethod]
    public void SelectBestShotDirectoryCommandTest()
    {
        using var window = CreateViewModel();

        var command = window.SelectBestShotDirectoryCommand;
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.BestShotDirectory.Subscribe(_ => ++numChanged);

        var path = Path.GetTempPath();
        var result = new OpenFolderDialogActionResult(path);
        command.CanExecute(result).ShouldBeTrue();
        numChanged.ShouldBe(0);

        command.Execute(result);
        numChanged.ShouldBe(1);
        window.BestShotDirectory.Value.ShouldBe(path);
    }

    [TestMethod]
    public void SelectBestShotDirectoryCommandTestNoChange()
    {
        using var window = CreateViewModel();

        var command = window.SelectBestShotDirectoryCommand;
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.BestShotDirectory.Subscribe(_ => ++numChanged);

        var path = window.BestShotDirectory.Value;
        var result = new OpenFolderDialogActionResult(path);
        command.CanExecute(result).ShouldBeTrue();
        numChanged.ShouldBe(0);

        command.Execute(result);
        numChanged.ShouldBe(0);
    }

    [TestMethod]
    public void SelectBestShotDirectoryCommandTestNonexistent()
    {
        using var window = CreateViewModel();

        var command = window.SelectBestShotDirectoryCommand;
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.BestShotDirectory.Subscribe(_ => ++numChanged);

        var path = "nonexistent";
        var result = new OpenFolderDialogActionResult(path);
        command.CanExecute(result).ShouldBeTrue();
        numChanged.ShouldBe(0);

        command.Execute(result);
        numChanged.ShouldBe(0);
    }

    [TestMethod]
    public void TemplateFilesSelectionChangedCommandTest()
    {
        using var window = CreateViewModel();

        var command = window.TemplateFilesSelectionChangedCommand;
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposed = window.DeleteTemplateFilesCommand
            .CanExecuteChangedAsObservable().Subscribe(_ => ++numChanged);

        command.CanExecute(null).ShouldBeTrue();
        numChanged.ShouldBe(0);

        command.Execute(null);
        numChanged.ShouldBe(1);
    }

    [TestMethod]
    public void AddTemplateFilesCommandTest()
    {
        using var window = CreateViewModel();

        var command = window.AddTemplateFilesCommand;
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.TemplateFiles.Subscribe(_ => ++numChanged);

        var fileNames = Enumerable.Range(1, 3).Select(_ => Path.GetTempFileName()).ToArray();
        try
        {
            var result = new OpenFileDialogActionResult(string.Empty, fileNames);
            command.CanExecute(result).ShouldBeTrue();
            numChanged.ShouldBe(0);

            command.Execute(result);
            numChanged.ShouldBe(1);
            window.TemplateFiles.Value.ShouldBe(fileNames);
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
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.TemplateFiles.Subscribe(_ => ++numChanged);

        var fileNames = Enumerable.Range(1, 5).Select(_ => Path.GetTempFileName()).ToArray();
        try
        {
            var fileNames1 = fileNames.Take(3);
            var fileNames2 = fileNames.TakeLast(3);

            var result = new OpenFileDialogActionResult(string.Empty, fileNames1);
            command.CanExecute(result).ShouldBeTrue();
            numChanged.ShouldBe(0);

            command.Execute(result);
            numChanged.ShouldBe(1);
            window.TemplateFiles.Value.ShouldBe(fileNames1);

            result = new OpenFileDialogActionResult(string.Empty, fileNames2);
            command.CanExecute(result).ShouldBeTrue();
            numChanged.ShouldBe(1);

            command.Execute(result);
            numChanged.ShouldBe(2);
            window.TemplateFiles.Value.ShouldBe(fileNames);
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
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.TemplateFiles.Subscribe(_ => ++numChanged);

        var fileNames = window.TemplateFiles.Value.ToArray();
        var result = new OpenFileDialogActionResult(string.Empty, fileNames);
        command.CanExecute(result).ShouldBeTrue();
        numChanged.ShouldBe(0);

        command.Execute(result);
#if NET9_0_OR_GREATER
        // TODO: under investigation
        numChanged.ShouldBe(0);
#else
        numChanged.ShouldBe(1);
#endif
        window.TemplateFiles.Value.ShouldBe(fileNames);
    }

    [TestMethod]
    public void AddTemplateFilesCommandTestNonexistent()
    {
        using var window = CreateViewModel();

        var command = window.AddTemplateFilesCommand;
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.TemplateFiles.Subscribe(_ => ++numChanged);

        var fileNames = new[] { "nonexistent.txt" };
        var result = new OpenFileDialogActionResult(string.Empty, fileNames);
        command.CanExecute(result).ShouldBeTrue();
        numChanged.ShouldBe(0);

        command.Execute(result);
#if NET9_0_OR_GREATER
        // TODO: under investigation
        numChanged.ShouldBe(0);
#else
        numChanged.ShouldBe(1);
#endif
        window.TemplateFiles.Value.ShouldBeEmpty();
    }

    [TestMethod]
    public void DeleteTemplateFilesCommandTest()
    {
        using var window = CreateViewModel();

        var command = window.DeleteTemplateFilesCommand;
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.TemplateFiles.Subscribe(_ => ++numChanged);

        var fileNames = Enumerable.Range(1, 3).Select(_ => Path.GetTempFileName()).ToArray();
        try
        {
            var result = new OpenFileDialogActionResult(string.Empty, fileNames);
            window.AddTemplateFilesCommand.Execute(result);
            numChanged.ShouldBe(1);
            window.TemplateFiles.Value.ShouldBe(fileNames);

            command.CanExecute(fileNames).ShouldBeTrue();
            numChanged.ShouldBe(1);

            command.Execute(fileNames.Take(2).ToList());
            numChanged.ShouldBe(2);
            window.TemplateFiles.Value.ShouldBe(fileNames.TakeLast(1));
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
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.TemplateFiles.Subscribe(_ => ++numChanged);

        var fileNames = Enumerable.Range(1, 3).Select(_ => Path.GetTempFileName()).ToArray();
        try
        {
            var result = new OpenFileDialogActionResult(string.Empty, fileNames);
            window.AddTemplateFilesCommand.Execute(result);
            numChanged.ShouldBe(1);
            window.TemplateFiles.Value.ShouldBe(fileNames);

            command.CanExecute(fileNames).ShouldBeTrue();
            numChanged.ShouldBe(1);

            command.Execute(fileNames.ToList());
            numChanged.ShouldBe(2);
            window.TemplateFiles.Value.ShouldBeEmpty();
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
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.TemplateFiles.Subscribe(_ => ++numChanged);

        var fileNames = Enumerable.Range(1, 3).Select(_ => Path.GetTempFileName()).ToArray();
        try
        {
            var result = new OpenFileDialogActionResult(string.Empty, fileNames);
            window.AddTemplateFilesCommand.Execute(result);
            numChanged.ShouldBe(1);
            window.TemplateFiles.Value.ShouldBe(fileNames);

            command.CanExecute(null).ShouldBeFalse();
            numChanged.ShouldBe(1);

            command.Execute(null);
            numChanged.ShouldBe(1);
            window.TemplateFiles.Value.ShouldBe(fileNames);
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
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.TemplateFiles.Subscribe(_ => ++numChanged);

        var fileNames = Enumerable.Range(1, 3).Select(_ => Path.GetTempFileName()).ToArray();
        try
        {
            var result = new OpenFileDialogActionResult(string.Empty, fileNames);
            window.AddTemplateFilesCommand.Execute(result);
            numChanged.ShouldBe(1);
            window.TemplateFiles.Value.ShouldBe(fileNames);

            var empty = Array.Empty<string>();

            command.CanExecute(empty).ShouldBeFalse();
            numChanged.ShouldBe(1);

            command.Execute(empty);
            numChanged.ShouldBe(2);
            window.TemplateFiles.Value.ShouldBe(fileNames);
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
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.TemplateFiles.Subscribe(_ => ++numChanged);

        var fileNames = Enumerable.Range(1, 3).Select(_ => Path.GetTempFileName()).ToArray();
        try
        {
            var result = new OpenFileDialogActionResult(string.Empty, fileNames);
            window.AddTemplateFilesCommand.Execute(result);
            numChanged.ShouldBe(1);
            window.TemplateFiles.Value.ShouldBe(fileNames);

            command.CanExecute(null).ShouldBeTrue();
            numChanged.ShouldBe(1);

            command.Execute(null);
            numChanged.ShouldBe(2);
            window.TemplateFiles.Value.ShouldBeEmpty();
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
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.TemplateFiles.Subscribe(_ => ++numChanged);

        command.CanExecute(null).ShouldBeFalse();
        numChanged.ShouldBe(0);

        command.Execute(null);
        numChanged.ShouldBe(0);
        window.TemplateFiles.Value.ShouldBeEmpty();
    }

    [TestMethod]
    public void SelectOutputDirectoryCommandTest()
    {
        using var window = CreateViewModel();

        var command = window.SelectOutputDirectoryCommand;
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.OutputDirectory.Subscribe(_ => ++numChanged);

        var path = Path.GetTempPath();
        var result = new OpenFolderDialogActionResult(path);
        command.CanExecute(result).ShouldBeTrue();
        numChanged.ShouldBe(0);

        command.Execute(result);
        numChanged.ShouldBe(1);
        window.OutputDirectory.Value.ShouldBe(path);
    }

    [TestMethod]
    public void SelectOutputDirectoryCommandTestNoChange()
    {
        using var window = CreateViewModel();

        var command = window.SelectOutputDirectoryCommand;
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.OutputDirectory.Subscribe(_ => ++numChanged);

        var path = window.OutputDirectory.Value;
        var result = new OpenFolderDialogActionResult(path);
        command.CanExecute(result).ShouldBeTrue();
        numChanged.ShouldBe(0);

        command.Execute(result);
        numChanged.ShouldBe(0);
    }

    [TestMethod]
    public void SelectOutputDirectoryCommandTestNonexistent()
    {
        using var window = CreateViewModel();

        var command = window.SelectOutputDirectoryCommand;
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.OutputDirectory.Subscribe(_ => ++numChanged);

        var path = "nonexistent";
        var result = new OpenFolderDialogActionResult(path);
        command.CanExecute(result).ShouldBeTrue();
        numChanged.ShouldBe(0);

        command.Execute(result);
        numChanged.ShouldBe(0);
    }

    [TestMethod]
    public void ConvertCommandTest()
    {
        using var window = CreateViewModel();

        var command = window.ConvertCommand;
        _ = command.ShouldNotBeNull();

        command.CanExecute(null).ShouldBeFalse();

        command.Execute(null);
        window.IsIdle.Value.ShouldBeTrue();
        window.Log.Value.ShouldBeEmpty();
    }

    [TestMethod]
    public void DraggingCommandTest()
    {
        using var window = CreateViewModel();

        var command = window.DraggingCommand;
        _ = command.ShouldNotBeNull();

        var args = CreateDragEventArgs(
            new DataObject(DataFormats.FileDrop, new object()), UIElement.PreviewDragEnterEvent);
        _ = args.ShouldNotBeNull();
        command.CanExecute(args).ShouldBeTrue();

        command.Execute(args);
        args.Effects.ShouldBe(DragDropEffects.Copy);
        args.Handled.ShouldBeTrue();
    }

    [TestMethod]
    public void DraggingCommandTestNone()
    {
        using var window = CreateViewModel();

        var command = window.DraggingCommand;
        _ = command.ShouldNotBeNull();

        var args = CreateDragEventArgs(
            new DataObject(DataFormats.Text, new object()), UIElement.PreviewDragEnterEvent);
        _ = args.ShouldNotBeNull();
        command.CanExecute(args).ShouldBeTrue();

        command.Execute(args);
        args.Effects.ShouldBe(DragDropEffects.None);
        args.Handled.ShouldBeFalse();
    }

    [TestMethod]
    public void DropScoreFileCommandTest()
    {
        using var window = CreateViewModel();

        var command = window.DropScoreFileCommand;
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.ScoreFile.Subscribe(_ => ++numChanged);

        var fileNames = Enumerable.Range(1, 3).Select(_ => Path.GetTempFileName()).ToArray();
        try
        {
            var args = CreateDragEventArgs(new DataObject(DataFormats.FileDrop, fileNames), UIElement.DropEvent);
            _ = args.ShouldNotBeNull();
            command.CanExecute(args).ShouldBeTrue();

            command.Execute(args);
            numChanged.ShouldBe(1);
            window.ScoreFile.Value.ShouldBe(fileNames[0]);
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
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.ScoreFile.Subscribe(_ => ++numChanged);

        var fileNames = new[] { "nonexistent.txt" };

        var args = CreateDragEventArgs(new DataObject(DataFormats.FileDrop, fileNames), UIElement.DropEvent);
        _ = args.ShouldNotBeNull();
        command.CanExecute(args).ShouldBeTrue();

        command.Execute(args);
        numChanged.ShouldBe(0);
        window.ScoreFile.Value.ShouldBeEmpty();
    }

    [TestMethod]
    public void DropScoreFileCommandTestInvalidData()
    {
        using var window = CreateViewModel();

        var command = window.DropScoreFileCommand;
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.ScoreFile.Subscribe(_ => ++numChanged);

        var args = CreateDragEventArgs(new DataObject(DataFormats.FileDrop, default(int)), UIElement.DropEvent);
        _ = args.ShouldNotBeNull();
        command.CanExecute(args).ShouldBeTrue();

        command.Execute(args);
        numChanged.ShouldBe(0);
        window.ScoreFile.Value.ShouldBeEmpty();
    }

    [TestMethod]
    public void DropScoreFileCommandTestInvalidDataFormat()
    {
        using var window = CreateViewModel();

        var command = window.DropScoreFileCommand;
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.ScoreFile.Subscribe(_ => ++numChanged);

        var args = CreateDragEventArgs(new DataObject(DataFormats.Text, string.Empty), UIElement.DropEvent);
        _ = args.ShouldNotBeNull();
        command.CanExecute(args).ShouldBeTrue();

        command.Execute(args);
        numChanged.ShouldBe(0);
        window.ScoreFile.Value.ShouldBeEmpty();
    }

    [TestMethod]
    public void DropBestShotDirectoryCommandTest()
    {
        using var window = CreateViewModel();

        var command = window.DropBestShotDirectoryCommand;
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.BestShotDirectory.Subscribe(_ => ++numChanged);

        var dirNames = Enumerable.Range(1, 3).Select(index =>
        {
            var path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            _ = Directory.CreateDirectory(path);
            return path;
        }).ToArray();

        try
        {
            var args = CreateDragEventArgs(new DataObject(DataFormats.FileDrop, dirNames), UIElement.DropEvent);
            _ = args.ShouldNotBeNull();
            command.CanExecute(args).ShouldBeTrue();

            command.Execute(args);
            numChanged.ShouldBe(1);
            window.BestShotDirectory.Value.ShouldBe(dirNames[0]);
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
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.BestShotDirectory.Subscribe(_ => ++numChanged);

        var dirNames = new[] { "nonexistent" };

        var args = CreateDragEventArgs(new DataObject(DataFormats.FileDrop, dirNames), UIElement.DropEvent);
        _ = args.ShouldNotBeNull();
        command.CanExecute(args).ShouldBeTrue();

        command.Execute(args);
        numChanged.ShouldBe(0);
        window.BestShotDirectory.Value.ShouldBeEmpty();
    }

    [TestMethod]
    public void DropBestShotDirectoryCommandTestInvalidData()
    {
        using var window = CreateViewModel();

        var command = window.DropBestShotDirectoryCommand;
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.BestShotDirectory.Subscribe(_ => ++numChanged);

        var args = CreateDragEventArgs(new DataObject(DataFormats.FileDrop, default(int)), UIElement.DropEvent);
        _ = args.ShouldNotBeNull();
        command.CanExecute(args).ShouldBeTrue();

        command.Execute(args);
        numChanged.ShouldBe(0);
        window.BestShotDirectory.Value.ShouldBeEmpty();
    }

    [TestMethod]
    public void DropBestShotDirectoryCommandTestInvalidDataFormat()
    {
        using var window = CreateViewModel();

        var command = window.DropBestShotDirectoryCommand;
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.BestShotDirectory.Subscribe(_ => ++numChanged);

        var args = CreateDragEventArgs(new DataObject(DataFormats.Text, string.Empty), UIElement.DropEvent);
        _ = args.ShouldNotBeNull();
        command.CanExecute(args).ShouldBeTrue();

        command.Execute(args);
        numChanged.ShouldBe(0);
        window.BestShotDirectory.Value.ShouldBeEmpty();
    }

    [TestMethod]
    public void DropTemplateFilesCommandTest()
    {
        using var window = CreateViewModel();

        var command = window.DropTemplateFilesCommand;
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.TemplateFiles.Subscribe(_ => ++numChanged);

        var fileNames = Enumerable.Range(1, 3).Select(_ => Path.GetTempFileName()).ToArray();
        try
        {
            var args = CreateDragEventArgs(
                new DataObject(DataFormats.FileDrop, fileNames.Append("nonexistent.txt").ToArray()),
                UIElement.DropEvent);
            _ = args.ShouldNotBeNull();
            command.CanExecute(args).ShouldBeTrue();

            command.Execute(args);
            numChanged.ShouldBe(1);
            window.TemplateFiles.Value.ShouldBe(fileNames);
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
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.TemplateFiles.Subscribe(_ => ++numChanged);

        var args = CreateDragEventArgs(new DataObject(DataFormats.FileDrop, default(int)), UIElement.DropEvent);
        _ = args.ShouldNotBeNull();
        command.CanExecute(args).ShouldBeTrue();

        command.Execute(args);
        numChanged.ShouldBe(0);
        window.TemplateFiles.Value.ShouldBeEmpty();
    }

    [TestMethod]
    public void DropTemplateFilesCommandTestInvalidDataFormat()
    {
        using var window = CreateViewModel();

        var command = window.DropTemplateFilesCommand;
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.TemplateFiles.Subscribe(_ => ++numChanged);

        var args = CreateDragEventArgs(new DataObject(DataFormats.Text, string.Empty), UIElement.DropEvent);
        _ = args.ShouldNotBeNull();
        command.CanExecute(args).ShouldBeTrue();

        command.Execute(args);
        numChanged.ShouldBe(0);
        window.TemplateFiles.Value.ShouldBeEmpty();
    }

    [TestMethod]
    public void DropOutputDirectoryCommandTest()
    {
        using var window = CreateViewModel();

        var command = window.DropOutputDirectoryCommand;
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.OutputDirectory.Subscribe(_ => ++numChanged);

        var dirNames = Enumerable.Range(1, 3).Select(index =>
        {
            var path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            _ = Directory.CreateDirectory(path);
            return path;
        }).ToArray();

        try
        {
            var args = CreateDragEventArgs(new DataObject(DataFormats.FileDrop, dirNames), UIElement.DropEvent);
            _ = args.ShouldNotBeNull();
            command.CanExecute(args).ShouldBeTrue();

            command.Execute(args);
            numChanged.ShouldBe(1);
            window.OutputDirectory.Value.ShouldBe(dirNames[0]);
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
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.OutputDirectory.Subscribe(_ => ++numChanged);

        var dirNames = new[] { "nonexistent" };

        var args = CreateDragEventArgs(new DataObject(DataFormats.FileDrop, dirNames), UIElement.DropEvent);
        _ = args.ShouldNotBeNull();
        command.CanExecute(args).ShouldBeTrue();

        command.Execute(args);
        numChanged.ShouldBe(0);
        window.OutputDirectory.Value.ShouldBeEmpty();
    }

    [TestMethod]
    public void DropOutputDirectoryCommandTestInvalidData()
    {
        using var window = CreateViewModel();

        var command = window.DropOutputDirectoryCommand;
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.OutputDirectory.Subscribe(_ => ++numChanged);

        var args = CreateDragEventArgs(new DataObject(DataFormats.FileDrop, default(int)), UIElement.DropEvent);
        _ = args.ShouldNotBeNull();
        command.CanExecute(args).ShouldBeTrue();

        command.Execute(args);
        numChanged.ShouldBe(0);
        window.OutputDirectory.Value.ShouldBeEmpty();
    }

    [TestMethod]
    public void DropOutputDirectoryCommandTestInvalidDataFormat()
    {
        using var window = CreateViewModel();

        var command = window.DropOutputDirectoryCommand;
        _ = command.ShouldNotBeNull();

        var numChanged = 0;
        using var disposable = window.OutputDirectory.Subscribe(_ => ++numChanged);

        var args = CreateDragEventArgs(new DataObject(DataFormats.Text, string.Empty), UIElement.DropEvent);
        _ = args.ShouldNotBeNull();
        command.CanExecute(args).ShouldBeTrue();

        command.Execute(args);
        numChanged.ShouldBe(0);
        window.OutputDirectory.Value.ShouldBeEmpty();
    }

    [TestMethod]
    public void OpenAboutWindowCommandTest()
    {
        var dialogServiceMock = MockDialogService();
        var dispatcherAdapterMock = MockDispatcherAdapter();
        var settings = new Settings();
        var formatterMock = MockNumberFormatter();

        using var window = new MainWindowViewModel(
            dialogServiceMock, dispatcherAdapterMock, settings, formatterMock);

        var command = window.OpenAboutWindowCommand;
        _ = command.ShouldNotBeNull();

        command.CanExecute(null).ShouldBeTrue();

        command.Execute(null);
        _ = dialogServiceMock.Received().ShowDialog(Arg.Any<MainWindowViewModel>(), Arg.Any<AboutWindowViewModel>());
    }

    [TestMethod]
    public void OpenSettingWindowCommandTest()
    {
        var dialogServiceMock = MockDialogService();
        var dispatcherAdapterMock = MockDispatcherAdapter();
        var settings = new Settings();
        var formatterMock = MockNumberFormatter();

        using var window = new MainWindowViewModel(
            dialogServiceMock, dispatcherAdapterMock, settings, formatterMock);

        var command = window.OpenSettingWindowCommand;
        _ = command.ShouldNotBeNull();

        command.CanExecute(null).ShouldBeTrue();

        command.Execute(null);
        _ = dialogServiceMock.Received().ShowDialog(Arg.Any<MainWindowViewModel>(), Arg.Any<SettingWindowViewModel>());
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
            numChanged.ShouldBe(1);
        }
        finally
        {
            LocalizeDictionary.Instance.Culture = backupCulture;
        }
    }
}
