//-----------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using ThScoreFileConverter.Adapters;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Interactivity;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Properties;
using WPFLocalizeExtension.Engine;

namespace ThScoreFileConverter.ViewModels
{
    /// <summary>
    /// The view model class for <see cref="Views.MainWindow"/>.
    /// </summary>
#if !DEBUG
    [SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Instantiated by the DI container.")]
#endif
    internal class MainWindowViewModel : BindableBase, IDisposable
    {
        /// <summary>
        /// A list of the Touhou works.
        /// </summary>
        private static readonly IEnumerable<Work> WorksImpl = new[]
        {
            new Work { Number = nameof(Resources.TH06),  IsSupported = true },
            new Work { Number = nameof(Resources.TH07),  IsSupported = true },
            new Work { Number = nameof(Resources.TH08),  IsSupported = true },
            new Work { Number = nameof(Resources.TH09),  IsSupported = true },
            new Work { Number = nameof(Resources.TH095), IsSupported = true },
            new Work { Number = nameof(Resources.TH10),  IsSupported = true },
            new Work { Number = nameof(Resources.TH11),  IsSupported = true },
            new Work { Number = nameof(Resources.TH12),  IsSupported = true },
            new Work { Number = nameof(Resources.TH125), IsSupported = true },
            new Work { Number = nameof(Resources.TH128), IsSupported = true },
            new Work { Number = nameof(Resources.TH13),  IsSupported = true },
            new Work { Number = nameof(Resources.TH14),  IsSupported = true },
            new Work { Number = nameof(Resources.TH143), IsSupported = true },
            new Work { Number = nameof(Resources.TH15),  IsSupported = true },
            new Work { Number = nameof(Resources.TH16),  IsSupported = true },
            new Work { Number = nameof(Resources.TH165), IsSupported = true },
            new Work { Number = nameof(Resources.TH17),  IsSupported = true },
            new Work { },
            new Work { Number = nameof(Resources.TH075), IsSupported = true },
            new Work { Number = nameof(Resources.TH105), IsSupported = true },
            new Work { Number = nameof(Resources.TH123), IsSupported = true },
            new Work { Number = nameof(Resources.TH135), IsSupported = true },
            new Work { Number = nameof(Resources.TH145), IsSupported = true },
            new Work { Number = nameof(Resources.TH155), IsSupported = true },
            new Work { Number = nameof(Resources.TH175), IsSupported = false },
        };

        /// <summary>
        /// An <see cref="IDialogService"/>.
        /// </summary>
        private readonly IDialogService dialogService;

        /// <summary>
        /// An <see cref="IDispatcherAdapter"/> that should wrap <see cref="Application.Current"/>.Dispatcher.
        /// </summary>
        private readonly IDispatcherAdapter dispatcher;

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
        /// <c>true</c> if the current instance has been disposed.
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
        /// <param name="settings">The settings of this application.</param>
        /// <param name="formatter">An <see cref="INumberFormatter"/>.</param>
        public MainWindowViewModel(
            IDialogService dialogService, IDispatcherAdapter dispatcher, Settings settings, INumberFormatter formatter)
        {
            this.dialogService = dialogService;
            this.dispatcher = dispatcher;
            this.settings = settings;
            this.formatter = formatter;
            this.disposables = new CompositeDisposable();
            this.disposed = false;
            this.converter = null;

            var rpMode = ReactivePropertyMode.DistinctUntilChanged;

            this.Title = Assembly.GetExecutingAssembly().GetName().Name ?? nameof(ThScoreFileConverter);
            this.Works = WorksImpl;
            this.IsIdle = new ReactivePropertySlim<bool>(true);
            this.LastWorkNumber = this.settings.ToReactivePropertySlimAsSynchronized(x => x.LastTitle, rpMode);
            this.disposables.Add(
                this.LastWorkNumber.Subscribe(
                    value => _ = this.settings.Dictionary.TryAdd(value, new SettingsPerTitle())));

            this.CurrentSetting = this.LastWorkNumber
                .Select(title => this.settings.Dictionary[title])
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

            this.SelectScoreFileCommand =
                new DelegateCommand<OpenFileDialogActionResult>(this.SelectScoreFile);
            this.SelectBestShotDirectoryCommand =
                new DelegateCommand<FolderBrowserDialogActionResult>(this.SelectBestShotDirectory);
            this.TemplateFilesSelectionChangedCommand =
                new DelegateCommand(this.OnTemplateFilesSelectionChanged);
            this.AddTemplateFilesCommand =
                new DelegateCommand<OpenFileDialogActionResult>(this.AddTemplateFiles);
            this.DeleteTemplateFilesCommand =
                new DelegateCommand<IList?>(this.DeleteTemplateFiles, this.CanDeleteTemplateFiles);
            this.DeleteAllTemplateFilesCommand =
                new DelegateCommand(this.DeleteAllTemplateFiles, this.CanDeleteAllTemplateFiles);
            this.SelectOutputDirectoryCommand =
                new DelegateCommand<FolderBrowserDialogActionResult>(this.SelectOutputDirectory);
            this.ConvertCommand =
                new DelegateCommand(this.Convert, this.CanConvert);

            this.DraggingCommand =
                new DelegateCommand<DragEventArgs>(this.OnDragging);
            this.DropScoreFileCommand =
                new DelegateCommand<DragEventArgs>(this.OnDropScoreFile);
            this.DropBestShotDirectoryCommand =
                new DelegateCommand<DragEventArgs>(this.OnDropBestShotDirectory);
            this.DropTemplateFilesCommand =
                new DelegateCommand<DragEventArgs>(this.OnDropTemplateFiles);
            this.DropOutputDirectoryCommand =
                new DelegateCommand<DragEventArgs>(this.OnDropOutputDirectory);

            this.OpenAboutWindowCommand = new DelegateCommand(this.OpenAboutWindow);
            this.OpenSettingWindowCommand = new DelegateCommand(this.OpenSettingWindow);

            this.disposables.Add(
                LocalizeDictionary.Instance.ObserveProperty(instance => instance.Culture)
                    .Subscribe(_ => this.RaisePropertyChanged(nameof(this.SupportedVersions))));

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

                    this.RaisePropertyChanged(nameof(this.SupportedVersions));
                    this.RaisePropertyChanged(nameof(this.CanHandleBestShot));
                    this.RaisePropertyChanged(nameof(this.CanReplaceCardNames));

                    this.ConvertCommand.RaiseCanExecuteChanged();
                }));
            this.disposables.Add(this.ScoreFile.Subscribe(value => this.ConvertCommand.RaiseCanExecuteChanged()));
            this.disposables.Add(
                this.BestShotDirectory.Subscribe(value => this.ConvertCommand.RaiseCanExecuteChanged()));
            this.disposables.Add(
                this.TemplateFiles.Subscribe(value =>
                {
                    this.DeleteTemplateFilesCommand.RaiseCanExecuteChanged();
                    this.DeleteAllTemplateFilesCommand.RaiseCanExecuteChanged();
                    this.ConvertCommand.RaiseCanExecuteChanged();
                }));
            this.disposables.Add(
                this.OutputDirectory.Subscribe(value => this.ConvertCommand.RaiseCanExecuteChanged()));
            this.disposables.Add(
                this.ImageOutputDirectory.Subscribe(value => this.ConvertCommand.RaiseCanExecuteChanged()));

            if (string.IsNullOrEmpty(this.LastWorkNumber.Value))
                this.LastWorkNumber.Value = WorksImpl.First().Number;
            else
                this.LastWorkNumber.ForceNotify();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        ~MainWindowViewModel()
        {
            this.Dispose(false);
        }

        #region Properties to bind a view

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

        #region Commands

        /// <summary>
        /// Gets the command to select a score file.
        /// </summary>
        public DelegateCommand<OpenFileDialogActionResult> SelectScoreFileCommand { get; }

        /// <summary>
        /// Gets the command to select a best shot directory.
        /// </summary>
        public DelegateCommand<FolderBrowserDialogActionResult> SelectBestShotDirectoryCommand { get; }

        /// <summary>
        /// Gets the command invoked when the selection of template files is changed.
        /// </summary>
        public DelegateCommand TemplateFilesSelectionChangedCommand { get; }

        /// <summary>
        /// Gets the command to add some files to the list of template files.
        /// </summary>
        public DelegateCommand<OpenFileDialogActionResult> AddTemplateFilesCommand { get; }

        /// <summary>
        /// Gets the command to delete some files from the list of template files.
        /// </summary>
        public DelegateCommand<IList?> DeleteTemplateFilesCommand { get; }

        /// <summary>
        /// Gets the command to delete all files from the list of template files.
        /// </summary>
        public DelegateCommand DeleteAllTemplateFilesCommand { get; }

        /// <summary>
        /// Gets the command to select an output directory.
        /// </summary>
        public DelegateCommand<FolderBrowserDialogActionResult> SelectOutputDirectoryCommand { get; }

        /// <summary>
        /// Gets the command to convert the score file.
        /// </summary>
        public DelegateCommand ConvertCommand { get; }

        /// <summary>
        /// Gets the command invoked when a dragging event is occurred on a UI element.
        /// </summary>
        public DelegateCommand<DragEventArgs> DraggingCommand { get; }

        /// <summary>
        /// Gets the command invoked when a score file is dropped on a UI element.
        /// </summary>
        public DelegateCommand<DragEventArgs> DropScoreFileCommand { get; }

        /// <summary>
        /// Gets the command invoked when a best shot directory is dropped on a UI element.
        /// </summary>
        public DelegateCommand<DragEventArgs> DropBestShotDirectoryCommand { get; }

        /// <summary>
        /// Gets the command invoked when some template files are dropped on a UI element.
        /// </summary>
        public DelegateCommand<DragEventArgs> DropTemplateFilesCommand { get; }

        /// <summary>
        /// Gets the command invoked when an output directory is dropped on a UI element.
        /// </summary>
        public DelegateCommand<DragEventArgs> DropOutputDirectoryCommand { get; }

        /// <summary>
        /// Gets the command to open an about window.
        /// </summary>
        public DelegateCommand OpenAboutWindowCommand { get; }

        /// <summary>
        /// Gets the command to open a setting window.
        /// </summary>
        public DelegateCommand OpenSettingWindowCommand { get; }

        #endregion

        #endregion

        /// <summary>
        /// Gets the setting for the currently selected Touhou work.
        /// </summary>
        private ReadOnlyReactivePropertySlim<SettingsPerTitle> CurrentSetting { get; }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the resources of the current instance.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> if calls from the <see cref="Dispose()"/> method; <c>false</c> for the finalizer.
        /// </param>
        protected virtual void Dispose(bool disposing)
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
                this.disposables.Dispose();
            }

            this.disposed = true;
        }

        /// <summary>
        /// Overrides the mouse cursor for the entire application.
        /// </summary>
        /// <param name="cursor">The new cursor or <c>null</c>.</param>
        private void OverrideCursor(Cursor? cursor)
        {
            this.dispatcher.Invoke(() => Mouse.OverrideCursor = cursor);
        }

        #region Methods for command implementation

        /// <summary>
        /// Selects a score file.
        /// </summary>
        /// <param name="result">A result of <see cref="OpenFileDialogAction"/>.</param>
        private void SelectScoreFile(OpenFileDialogActionResult result)
        {
            if (File.Exists(result.FileName))
                this.ScoreFile.Value = result.FileName;
        }

        /// <summary>
        /// Selects a best shot directory.
        /// </summary>
        /// <param name="result">A result of <see cref="FolderBrowserDialogAction"/>.</param>
        private void SelectBestShotDirectory(FolderBrowserDialogActionResult result)
        {
            if (Directory.Exists(result.SelectedPath))
                this.BestShotDirectory.Value = result.SelectedPath;
        }

        /// <summary>
        /// Invoked when the selection of template files is changed.
        /// </summary>
        private void OnTemplateFilesSelectionChanged()
        {
            this.DeleteTemplateFilesCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Adds some files to the list of template files.
        /// </summary>
        /// <param name="result">A result of <see cref="OpenFileDialogAction"/>.</param>
        private void AddTemplateFiles(OpenFileDialogActionResult result)
        {
            this.TemplateFiles.Value = this.TemplateFiles.Value
                .Union(result.FileNames.Where(file => File.Exists(file))).ToArray();
        }

        /// <summary>
        /// Returns a value indicating whether <see cref="DeleteTemplateFiles"/> can be invoked.
        /// </summary>
        /// <param name="selectedItems">A list indicating the path strings which will be deleted.</param>
        /// <returns>
        /// <c>true</c> if <see cref="DeleteTemplateFiles"/> can be invoked; otherwise, <c>false</c>.
        /// </returns>
        private bool CanDeleteTemplateFiles(IList? selectedItems)
        {
            return selectedItems?.Count > 0;
        }

        /// <summary>
        /// Deletes some files from the list of template files.
        /// </summary>
        /// <param name="selectedItems">A list indicating the path strings which are deleted.</param>
        private void DeleteTemplateFiles(IList? selectedItems)
        {
            if (selectedItems is not null)
                this.TemplateFiles.Value = this.TemplateFiles.Value.Except(selectedItems.Cast<string>()).ToArray();
        }

        /// <summary>
        /// Returns a value indicating whether <see cref="DeleteAllTemplateFiles"/> can be invoked.
        /// </summary>
        /// <returns>
        /// <c>true</c> if <see cref="DeleteAllTemplateFiles"/> can be invoked; otherwise, <c>false</c>.
        /// </returns>
        private bool CanDeleteAllTemplateFiles()
        {
            return this.TemplateFiles.Value.Any();
        }

        /// <summary>
        /// Deletes all files from the list of template files.
        /// </summary>
        private void DeleteAllTemplateFiles()
        {
            this.TemplateFiles.Value = Enumerable.Empty<string>();
        }

        /// <summary>
        /// Selects an output directory.
        /// </summary>
        /// <param name="result">A result of <see cref="FolderBrowserDialogAction"/>.</param>
        private void SelectOutputDirectory(FolderBrowserDialogActionResult result)
        {
            if (Directory.Exists(result.SelectedPath))
                this.OutputDirectory.Value = result.SelectedPath;
        }

        /// <summary>
        /// Returns a value indicating whether <see cref="Convert"/> can be invoked.
        /// </summary>
        /// <returns>
        /// <c>true</c> if <see cref="Convert"/> can be invoked; otherwise, <c>false</c>.
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
        private void OnDropScoreFile(DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    if (e.Data.GetData(DataFormats.FileDrop) is string[] droppedPaths)
                    {
                        var filePath = droppedPaths.FirstOrDefault(path => File.Exists(path));
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
        private void OnDropBestShotDirectory(DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    if (e.Data.GetData(DataFormats.FileDrop) is string[] droppedPaths)
                    {
                        var dirPath = droppedPaths.FirstOrDefault(path => Directory.Exists(path));
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
        private void OnDropTemplateFiles(DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    if (e.Data.GetData(DataFormats.FileDrop) is string[] droppedPaths)
                    {
                        this.TemplateFiles.Value = this.TemplateFiles.Value
                            .Union(droppedPaths.Where(path => File.Exists(path))).ToArray();
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
        private void OnDropOutputDirectory(DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    if (e.Data.GetData(DataFormats.FileDrop) is string[] droppedPaths)
                    {
                        var dirPath = droppedPaths.FirstOrDefault(path => Directory.Exists(path));
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
        private void OpenAboutWindow()
        {
            this.dialogService.ShowDialog(nameof(AboutWindowViewModel), new DialogParameters(), result => { });
        }

        /// <summary>
        /// Invoked when opening a setting window is requested.
        /// </summary>
        private void OpenSettingWindow()
        {
            this.dialogService.ShowDialog(nameof(SettingWindowViewModel), new DialogParameters(), result => { });
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
}
