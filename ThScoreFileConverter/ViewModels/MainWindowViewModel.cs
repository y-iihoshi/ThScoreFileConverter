//-----------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using ThScoreFileConverter.Adapters;
using ThScoreFileConverter.Interactivity;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Properties;
using WPFLocalizeExtension.Engine;

namespace ThScoreFileConverter.ViewModels;

/// <summary>
/// The view model class for <see cref="Views.MainWindow"/>.
/// </summary>
#if !DEBUG
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Instantiated by the DI container.")]
#endif
internal sealed partial class MainWindowViewModel : ObservableObject, IDisposable
{
    /// <summary>
    /// An <see cref="IDialogService"/>.
    /// </summary>
    private readonly IDialogService dialogService;

    /// <summary>
    /// An <see cref="IDispatcherAdapter"/> that should wrap <see cref="Application.Current"/>.Dispatcher.
    /// </summary>
    private readonly IDispatcherAdapter dispatcher;

    /// <summary>
    /// An <see cref="IResourceDictionaryAdapter"/> that should wrap <see cref="Application.Current"/>.Resources.
    /// </summary>
    private readonly IResourceDictionaryAdapter resourceDictionaryAdapter;

    /// <summary>
    /// The settings of this application.
    /// </summary>
    private readonly Settings settings;

    /// <summary>
    /// An <see cref="INumberFormatter"/>.
    /// </summary>
    private readonly INumberFormatter formatter;

    /// <summary>
    /// A group of disposable resources.
    /// </summary>
    private readonly CompositeDisposable disposables;

    /// <summary>
    /// <see langword="true"/> if the current instance has been disposed.
    /// </summary>
    private bool disposed;

    /// <summary>
    /// The instance that executes a conversion process.
    /// </summary>
    private ThConverter? converter;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
    /// </summary>
    /// <param name="dialogService">An <see cref="IDialogService"/>.</param>
    /// <param name="dispatcher">
    /// An <see cref="IDispatcherAdapter"/> that should wrap <see cref="Application.Current"/>.Dispatcher.
    /// </param>
    /// <param name="adapter">
    /// An <see cref="IResourceDictionaryAdapter"/> that should wrap <see cref="Application.Current"/>.Resources.
    /// </param>
    /// <param name="settings">The settings of this application.</param>
    /// <param name="formatter">An <see cref="INumberFormatter"/>.</param>
    public MainWindowViewModel(
        IDialogService dialogService, IDispatcherAdapter dispatcher, IResourceDictionaryAdapter adapter, Settings settings, INumberFormatter formatter)
    {
        this.dialogService = dialogService;
        this.dispatcher = dispatcher;
        this.resourceDictionaryAdapter = adapter;
        this.settings = settings;
        this.formatter = formatter;
        this.disposables = [];
        this.disposed = false;
        this.converter = null;

        var rpMode = ReactivePropertyMode.DistinctUntilChanged;

        this.MinWidth = Settings.WindowMinWidth;
        this.Width = this.settings.ToReactivePropertySlimAsSynchronized(
            x => x.WindowWidth, value => (double)value!, value => value, rpMode);
        this.MinHeight = Settings.WindowMinHeight;
        this.Height = this.settings.ToReactivePropertySlimAsSynchronized(
            x => x.WindowHeight, value => (double)value!, value => value, rpMode);

        this.MainContentMinHeight = Settings.MainContentMinHeight;
        this.MainContentHeight = this.settings.ToReactivePropertySlimAsSynchronized(
            x => x.MainContentHeight, value => new GridLength((double)value!, GridUnitType.Star), value => value.Value, rpMode);
        this.SubContentMinHeight = Settings.SubContentMinHeight;
        this.SubContentHeight = this.settings.ToReactivePropertySlimAsSynchronized(
            x => x.SubContentHeight, value => new GridLength((double)value!, GridUnitType.Star), value => value.Value, rpMode);

        this.Title = Assembly.GetExecutingAssembly().GetName().Name ?? nameof(ThScoreFileConverter);
        this.Works = Definitions.Works;
        this.IsIdle = new ReactivePropertySlim<bool>(true);
        this.LastWorkNumber = this.settings.ToReactivePropertySlimAsSynchronized(x => x.LastTitle, rpMode);

        this.CurrentSetting = this.LastWorkNumber
            .Select(this.settings.GetSettingsPerTitle)
            .ToReadOnlyReactivePropertySlim(new SettingsPerTitle(), mode: rpMode);

        this.ScoreFile = this.CurrentSetting.ToReactivePropertySlimAsSynchronized(x => x.Value.ScoreFile, rpMode);
        this.OpenScoreFileDialogInitialDirectory = this.ScoreFile
            .Select(file => Path.GetDirectoryName(file) ?? string.Empty)
            .ToReadOnlyReactivePropertySlim(string.Empty, rpMode);
        this.BestShotDirectory = this.CurrentSetting
            .ToReactivePropertySlimAsSynchronized(x => x.Value.BestShotDirectory, rpMode);
        this.TemplateFiles = this.CurrentSetting
            .ToReactivePropertySlimAsSynchronized(x => x.Value.TemplateFiles, rpMode);
        this.OpenTemplateFilesDialogInitialDirectory = this.TemplateFiles
            .Select(files => Path.GetDirectoryName(files.LastOrDefault()) ?? string.Empty)
            .ToReadOnlyReactivePropertySlim(string.Empty, rpMode);
        this.OutputDirectory = this.CurrentSetting
            .ToReactivePropertySlimAsSynchronized(x => x.Value.OutputDirectory, rpMode);
        this.ImageOutputDirectory = this.CurrentSetting
            .ToReactivePropertySlimAsSynchronized(x => x.Value.ImageOutputDirectory, rpMode);
        this.HidesUntriedCards = this.CurrentSetting
            .ToReactivePropertySlimAsSynchronized(x => x.Value.HideUntriedCards, rpMode);
        this.Log = new ReactivePropertySlim<string>(string.Empty);

        this.disposables.Add(
            LocalizeDictionary.Instance.ObserveProperty(instance => instance.Culture)
                .Subscribe(_ => this.OnPropertyChanged(nameof(this.SupportedVersions))));

        this.disposables.Add(this.IsIdle.Subscribe(idle => this.OverrideCursor(idle ? null : Cursors.Wait)));
        this.disposables.Add(
            this.LastWorkNumber.Subscribe(value =>
            {
                this.converter = ThConverterFactory.Create(value);
                if (this.converter is null)
                {
                    this.Log.Value = $"Failed to create a converter: {nameof(this.LastWorkNumber)} = {value}"
                        + Environment.NewLine;
                }
                else
                {
                    this.converter.ConvertFinished += this.OnConvertFinished;
                    this.converter.ConvertAllFinished += this.OnConvertAllFinished;
                    this.converter.ExceptionOccurred += this.OnExceptionOccurred;
                    this.Log.Value = string.Empty;
                }

                this.IsIdle.Value = true;

                this.OnPropertyChanged(nameof(this.SupportedVersions));
                this.OnPropertyChanged(nameof(this.CanHandleBestShot));
                this.OnPropertyChanged(nameof(this.CanReplaceCardNames));

                this.ConvertCommand.NotifyCanExecuteChanged();
            }));
        this.disposables.Add(this.ScoreFile.Subscribe(value => this.ConvertCommand.NotifyCanExecuteChanged()));
        this.disposables.Add(
            this.BestShotDirectory.Subscribe(value => this.ConvertCommand.NotifyCanExecuteChanged()));
        this.disposables.Add(
            this.TemplateFiles.Subscribe(value =>
            {
                this.DeleteTemplateFilesCommand.NotifyCanExecuteChanged();
                this.DeleteAllTemplateFilesCommand.NotifyCanExecuteChanged();
                this.ConvertCommand.NotifyCanExecuteChanged();
            }));
        this.disposables.Add(
            this.OutputDirectory.Subscribe(value => this.ConvertCommand.NotifyCanExecuteChanged()));
        this.disposables.Add(
            this.ImageOutputDirectory.Subscribe(value => this.ConvertCommand.NotifyCanExecuteChanged()));

        if (string.IsNullOrEmpty(this.LastWorkNumber.Value))
            this.LastWorkNumber.Value = this.Works.First().Number;
        else
            this.LastWorkNumber.ForceNotify();
    }

    #region Properties to bind a view

    /// <summary>
    /// Gets the minimum width of the window.
    /// </summary>
    public double MinWidth { get; }

    /// <summary>
    /// Gets the current width of the window.
    /// </summary>
    public ReactivePropertySlim<double> Width { get; }

    /// <summary>
    /// Gets the minimum height of the window.
    /// </summary>
    public double MinHeight { get; }

    /// <summary>
    /// Gets the current height of the window.
    /// </summary>
    public ReactivePropertySlim<double> Height { get; }

    /// <summary>
    /// Gets the minimum height of the main content.
    /// </summary>
    public double MainContentMinHeight { get; }

    /// <summary>
    /// Gets the current height of the main content.
    /// </summary>
    public ReactivePropertySlim<GridLength> MainContentHeight { get; }

    /// <summary>
    /// Gets the minimum height of the sub content.
    /// </summary>
    public double SubContentMinHeight { get; }

    /// <summary>
    /// Gets the current height of the sub content.
    /// </summary>
    public ReactivePropertySlim<GridLength> SubContentHeight { get; }

    /// <summary>
    /// Gets a title string.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets a list of the Touhou works.
    /// </summary>
    public IEnumerable<Work> Works { get; }

    /// <summary>
    /// Gets a value indicating whether the conversion process is idle.
    /// </summary>
    public ReactivePropertySlim<bool> IsIdle { get; }

    /// <summary>
    /// Gets a value indicating whether the conversion process can handle best shot files.
    /// </summary>
    public bool CanHandleBestShot => this.converter?.HasBestShotConverter ?? false;

    /// <summary>
    /// Gets a number string indicating the last selected work.
    /// </summary>
    public ReactivePropertySlim<string> LastWorkNumber { get; }

    /// <summary>
    /// Gets a string indicating the supported versions of the score file to convert.
    /// </summary>
    public string SupportedVersions => this.converter is null
        ? string.Empty
        : Utils.GetLocalizedValues<string>(nameof(Resources.SupportedVersion)) + this.converter.SupportedVersions;

    /// <summary>
    /// Gets a path of the score file.
    /// </summary>
    public ReactivePropertySlim<string> ScoreFile { get; }

    /// <summary>
    /// Gets the initial directory to select a score file.
    /// </summary>
    public ReadOnlyReactivePropertySlim<string> OpenScoreFileDialogInitialDirectory { get; }

    /// <summary>
    /// Gets a path of the best shot directory.
    /// </summary>
    public ReactivePropertySlim<string> BestShotDirectory { get; }

    /// <summary>
    /// Gets a list of the paths of template files.
    /// </summary>
    public ReactivePropertySlim<IEnumerable<string>> TemplateFiles { get; }

    /// <summary>
    /// Gets the initial directory to select template files.
    /// </summary>
    public ReadOnlyReactivePropertySlim<string> OpenTemplateFilesDialogInitialDirectory { get; }

    /// <summary>
    /// Gets a path of the output directory.
    /// </summary>
    public ReactivePropertySlim<string> OutputDirectory { get; }

    /// <summary>
    /// Gets a name of the output directory for image files.
    /// </summary>
    public ReactivePropertySlim<string> ImageOutputDirectory { get; }

    /// <summary>
    /// Gets a value indicating whether the conversion process can replace spell card names.
    /// </summary>
    public bool CanReplaceCardNames => this.converter?.HasCardReplacer ?? false;

    /// <summary>
    /// Gets a value indicating whether the conversion process hides untried cards.
    /// </summary>
    public ReactivePropertySlim<bool> HidesUntriedCards { get; }

    /// <summary>
    /// Gets a log text.
    /// </summary>
    public ReactivePropertySlim<string> Log { get; }

    #endregion

    /// <summary>
    /// Gets the setting for the currently selected Touhou work.
    /// </summary>
    private ReadOnlyReactivePropertySlim<SettingsPerTitle> CurrentSetting { get; }

    /// <inheritdoc/>
    public void Dispose()
    {
        this.Dispose(true);
    }

    /// <summary>
    /// Disposes the resources of the current instance.
    /// </summary>
    /// <param name="disposing">
    /// <see langword="true"/> if calls from the <see cref="Dispose()"/> method; <see langword="false"/> for the finalizer.
    /// </param>
    private void Dispose(bool disposing)
    {
        if (this.disposed)
        {
            return;
        }

        if (disposing)
        {
            this.Log.Dispose();
            this.HidesUntriedCards.Dispose();
            this.ImageOutputDirectory.Dispose();
            this.OutputDirectory.Dispose();
            this.OpenTemplateFilesDialogInitialDirectory.Dispose();
            this.TemplateFiles.Dispose();
            this.BestShotDirectory.Dispose();
            this.OpenScoreFileDialogInitialDirectory.Dispose();
            this.ScoreFile.Dispose();
            this.LastWorkNumber.Dispose();
            this.IsIdle.Dispose();
            this.CurrentSetting.Dispose();
            this.SubContentHeight.Dispose();
            this.MainContentHeight.Dispose();
            this.Height.Dispose();
            this.Width.Dispose();
            this.disposables.Dispose();
        }

        this.disposed = true;
    }

    /// <summary>
    /// Overrides the mouse cursor for the entire application.
    /// </summary>
    /// <param name="cursor">The new cursor or <see langword="null"/>.</param>
    private void OverrideCursor(Cursor? cursor)
    {
        this.dispatcher.Invoke(() => Mouse.OverrideCursor = cursor);
    }

    #region Methods for command implementation

    /// <summary>
    /// Selects a score file.
    /// </summary>
    /// <param name="result">A result of <see cref="OpenFileDialogAction"/>.</param>
    [RelayCommand]
    private void SelectScoreFile(OpenFileDialogActionResult result)
    {
        if (File.Exists(result.FileName))
            this.ScoreFile.Value = result.FileName;
    }

    /// <summary>
    /// Selects a best shot directory.
    /// </summary>
    /// <param name="result">A result of <see cref="OpenFolderDialogAction"/>.</param>
    [RelayCommand]
    private void SelectBestShotDirectory(OpenFolderDialogActionResult result)
    {
        if (Directory.Exists(result.FolderName))
            this.BestShotDirectory.Value = result.FolderName;
    }

    /// <summary>
    /// Invoked when the selection of template files is changed.
    /// </summary>
    [RelayCommand]
    private void OnTemplateFilesSelectionChanged()
    {
        this.DeleteTemplateFilesCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Adds some files to the list of template files.
    /// </summary>
    /// <param name="result">A result of <see cref="OpenFileDialogAction"/>.</param>
    [RelayCommand]
    private void AddTemplateFiles(OpenFileDialogActionResult result)
    {
        this.TemplateFiles.Value = this.TemplateFiles.Value.Union(result.FileNames.Where(File.Exists)).ToArray();
    }

    /// <summary>
    /// Returns a value indicating whether <see cref="DeleteTemplateFiles"/> can be invoked.
    /// </summary>
    /// <param name="selectedItems">A list indicating the path strings which will be deleted.</param>
    /// <returns>
    /// <see langword="true"/> if <see cref="DeleteTemplateFiles"/> can be invoked; otherwise, <see langword="false"/>.
    /// </returns>
#pragma warning disable CA1822 // Mark members as static
    private bool CanDeleteTemplateFiles(IList? selectedItems)
#pragma warning restore CA1822 // Mark members as static
    {
        return selectedItems?.Count > 0;
    }

    /// <summary>
    /// Deletes some files from the list of template files.
    /// </summary>
    /// <param name="selectedItems">A list indicating the path strings which are deleted.</param>
    [RelayCommand(CanExecute = nameof(CanDeleteTemplateFiles))]
    private void DeleteTemplateFiles(IList? selectedItems)
    {
        if (selectedItems is not null)
            this.TemplateFiles.Value = this.TemplateFiles.Value.Except(selectedItems.Cast<string>()).ToArray();
    }

    /// <summary>
    /// Returns a value indicating whether <see cref="DeleteAllTemplateFiles"/> can be invoked.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if <see cref="DeleteAllTemplateFiles"/> can be invoked; otherwise, <see langword="false"/>.
    /// </returns>
    private bool CanDeleteAllTemplateFiles()
    {
        return this.TemplateFiles.Value.Any();
    }

    /// <summary>
    /// Deletes all files from the list of template files.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanDeleteAllTemplateFiles))]
    private void DeleteAllTemplateFiles()
    {
        this.TemplateFiles.Value = Enumerable.Empty<string>();
    }

    /// <summary>
    /// Selects an output directory.
    /// </summary>
    /// <param name="result">A result of <see cref="OpenFolderDialogAction"/>.</param>
    [RelayCommand]
    private void SelectOutputDirectory(OpenFolderDialogActionResult result)
    {
        if (Directory.Exists(result.FolderName))
            this.OutputDirectory.Value = result.FolderName;
    }

    /// <summary>
    /// Returns a value indicating whether <see cref="Convert"/> can be invoked.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if <see cref="Convert"/> can be invoked; otherwise, <see langword="false"/>.
    /// </returns>
    private bool CanConvert()
    {
        return (this.converter is not null)
            && !string.IsNullOrEmpty(this.ScoreFile.Value)
            && this.TemplateFiles.Value.Any()
            && !string.IsNullOrEmpty(this.OutputDirectory.Value)
            && !(this.CanHandleBestShot && string.IsNullOrEmpty(this.BestShotDirectory.Value))
            && !(this.CanHandleBestShot && string.IsNullOrEmpty(this.ImageOutputDirectory.Value));
    }

    /// <summary>
    /// Converts the score file.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanConvert))]
    private void Convert()
    {
        if (this.CanConvert())
        {
            this.IsIdle.Value = false;
            this.Log.Value = Utils.GetLocalizedValues<string>(nameof(Resources.MessageStartConversion))
                + Environment.NewLine;
            var inputCodePageId = this.settings.InputCodePageId!.Value;
            var outputCodePageId = this.settings.OutputCodePageId!.Value;
            new Thread(this.converter!.Convert).Start(
                (this.CurrentSetting.Value, inputCodePageId, outputCodePageId, this.formatter));
        }
    }

    /// <summary>
    /// Invoked when a dragging event is occurred.
    /// </summary>
    /// <param name="e">The event data.</param>
    [RelayCommand]
    private void OnDragging(DragEventArgs e)
    {
        try
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }
        catch (Exception ex)
        {
            this.Log.Value += ex.Message + Environment.NewLine;
            throw;
        }
    }

    /// <summary>
    /// Invoked when a score file is dropped on a UI element.
    /// </summary>
    /// <param name="e">The event data.</param>
    [RelayCommand]
    private void OnDropScoreFile(DragEventArgs e)
    {
        try
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (e.Data.GetData(DataFormats.FileDrop) is string[] droppedPaths)
                {
                    var filePath = droppedPaths.FirstOrDefault(File.Exists);
                    if (filePath is not null)
                        this.ScoreFile.Value = filePath;
                }
            }
        }
        catch (Exception ex)
        {
            this.Log.Value += ex.Message + Environment.NewLine;
            throw;
        }
    }

    /// <summary>
    /// Invoked when a best shot directory is dropped on a UI element.
    /// </summary>
    /// <param name="e">The event data.</param>
    [RelayCommand]
    private void OnDropBestShotDirectory(DragEventArgs e)
    {
        try
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (e.Data.GetData(DataFormats.FileDrop) is string[] droppedPaths)
                {
                    var dirPath = droppedPaths.FirstOrDefault(Directory.Exists);
                    if (dirPath is not null)
                        this.BestShotDirectory.Value = dirPath;
                }
            }
        }
        catch (Exception ex)
        {
            this.Log.Value += ex.Message + Environment.NewLine;
            throw;
        }
    }

    /// <summary>
    /// Invoked when some template files are dropped on a UI element.
    /// </summary>
    /// <param name="e">The event data.</param>
    [RelayCommand]
    private void OnDropTemplateFiles(DragEventArgs e)
    {
        try
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (e.Data.GetData(DataFormats.FileDrop) is string[] droppedPaths)
                {
                    this.TemplateFiles.Value = this.TemplateFiles.Value.Union(droppedPaths.Where(File.Exists)).ToArray();
                }
            }
        }
        catch (Exception ex)
        {
            this.Log.Value += ex.Message + Environment.NewLine;
            throw;
        }
    }

    /// <summary>
    /// Invoked when an output directory is dropped on a UI element.
    /// </summary>
    /// <param name="e">The event data.</param>
    [RelayCommand]
    private void OnDropOutputDirectory(DragEventArgs e)
    {
        try
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (e.Data.GetData(DataFormats.FileDrop) is string[] droppedPaths)
                {
                    var dirPath = droppedPaths.FirstOrDefault(Directory.Exists);
                    if (dirPath is not null)
                        this.OutputDirectory.Value = dirPath;
                }
            }
        }
        catch (Exception ex)
        {
            this.Log.Value += ex.Message + Environment.NewLine;
            throw;
        }
    }

    /// <summary>
    /// Invoked when opening an about window is requested.
    /// </summary>
    [RelayCommand]
    private void OpenAboutWindow()
    {
        _ = this.dialogService.ShowDialog(this, new AboutWindowViewModel());
    }

    /// <summary>
    /// Invoked when opening a setting window is requested.
    /// </summary>
    [RelayCommand]
    private void OpenSettingWindow()
    {
        using var viewModel = new SettingWindowViewModel(this.settings, this.resourceDictionaryAdapter);
        _ = this.dialogService.ShowDialog(this, viewModel);
    }

    #endregion

    #region Event handlers

    /// <summary>
    /// Handles the event indicating the conversion process per file has finished.
    /// </summary>
    /// <param name="sender">The instance where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    private void OnConvertFinished(object? sender, ThConverterEventArgs e)
    {
        this.Log.Value += e.Message + Environment.NewLine;
    }

    /// <summary>
    /// Handles the event indicating the all conversion process has finished.
    /// </summary>
    /// <param name="sender">The instance where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    private void OnConvertAllFinished(object? sender, ThConverterEventArgs e)
    {
        this.Log.Value += Utils.GetLocalizedValues<string>(nameof(Resources.MessageConversionFinished))
            + Environment.NewLine;
        this.IsIdle.Value = true;
    }

    /// <summary>
    /// Handles the event indicating an exception has occurred.
    /// </summary>
    /// <param name="sender">The instance where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    private void OnExceptionOccurred(object? sender, ExceptionOccurredEventArgs e)
    {
#if DEBUG
        this.Log.Value += e.Exception.Message + Environment.NewLine;
#endif
        this.Log.Value += Utils.GetLocalizedValues<string>(nameof(Resources.MessageUnhandledExceptionOccurred))
            + Environment.NewLine;
        this.IsIdle.Value = true;
    }

    #endregion
}
