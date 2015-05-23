//-----------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="None">
//     (c) 2014-2015 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Input;
    using Microsoft.Practices.Prism.Commands;
    using Microsoft.Practices.Prism.Mvvm;
    using ThScoreFileConverter.Models;
    using ThScoreFileConverter.Properties;

    /// <summary>
    /// The view model class for <see cref="ThScoreFileConverter.Views.MainWindow"/>.
    /// </summary>
    internal class MainWindowViewModel : BindableBase
    {
        /// <summary>
        /// A list of the Touhou works.
        /// </summary>
        public static readonly Work[] WorksImpl = new Work[]
        {
            new Work { Number = "TH06", Title = "東方紅魔郷", IsSupported = true },
            new Work { Number = "TH07", Title = "東方妖々夢", IsSupported = true },
            new Work { Number = "TH08", Title = "東方永夜抄", IsSupported = true },
            new Work { Number = "TH09", Title = "東方花映塚", IsSupported = true },
            new Work { Number = "TH095", Title = "東方文花帖", IsSupported = true },
            new Work { Number = "TH10", Title = "東方風神録", IsSupported = true },
            new Work { Number = "TH11", Title = "東方地霊殿", IsSupported = true },
            new Work { Number = "TH12", Title = "東方星蓮船", IsSupported = true },
            new Work { Number = "TH125", Title = "ダブルスポイラー", IsSupported = true },
            new Work { Number = "TH128", Title = "妖精大戦争", IsSupported = true },
            new Work { Number = "TH13", Title = "東方神霊廟", IsSupported = true },
            new Work { Number = "TH14", Title = "東方輝針城", IsSupported = true },
            new Work { Number = "TH143", Title = "弾幕アマノジャク", IsSupported = true },
            new Work { Number = "TH15", Title = "東方紺珠伝", IsSupported = false },
            new Work { Number = string.Empty, Title = string.Empty, IsSupported = false },
            new Work { Number = "TH075", Title = "東方萃夢想", IsSupported = true },
            new Work { Number = "TH105", Title = "東方緋想天", IsSupported = true },
            new Work { Number = "TH123", Title = "東方非想天則", IsSupported = true },
            new Work { Number = "TH135", Title = "東方心綺楼", IsSupported = true },
            new Work { Number = "TH145", Title = "東方深秘録", IsSupported = false },
        };

        /// <summary>
        /// The instance that executes a conversion process.
        /// </summary>
        private ThConverter converter;

        /// <summary>
        /// Indicates whether a conversion process is idle.
        /// </summary>
        private bool isIdle;

        /// <summary>
        /// A log text.
        /// </summary>
        private string log;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
        {
            this.converter = null;

            this.Title = Assembly.GetExecutingAssembly().GetName().Name;
            this.Works = WorksImpl;

            this.SelectScoreFileCommand =
                new DelegateCommand(this.SelectScoreFile, this.CanSelectScoreFile);
            this.SelectBestShotDirectoryCommand =
                new DelegateCommand(this.SelectBestShotDirectory, this.CanSelectBestShotDirectory);
            this.AddTemplateFilesCommand =
                new DelegateCommand(this.AddTemplateFiles, this.CanAddTemplateFiles);
            this.DeleteTemplateFilesCommand =
                new DelegateCommand<IList>(this.DeleteTemplateFiles, this.CanDeleteTemplateFiles);
            this.DeleteAllTemplateFilesCommand =
                new DelegateCommand(this.DeleteAllTemplateFiles, this.CanDeleteAllTemplateFiles);
            this.SelectOutputDirectoryCommand =
                new DelegateCommand(this.SelectOutputDirectory, this.CanSelectOutputDirectory);
            this.ConvertCommand =
                new DelegateCommand(this.Convert, this.CanConvert);

            this.PropertyChanged += this.OnPropertyChanged;

            if (string.IsNullOrEmpty(this.LastWorkNumber))
                this.LastWorkNumber = WorksImpl.First().Number;
            else
                this.OnPropertyChanged(() => this.LastWorkNumber);
        }

        #region Properties to bind a view

        /// <summary>
        /// Gets a title string.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Gets a list of the Touhou works.
        /// </summary>
        public IEnumerable<Work> Works { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the conversion process is idle.
        /// </summary>
        public bool IsIdle
        {
            get { return this.isIdle; }
            private set { this.SetProperty(ref this.isIdle, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the conversion process can handle best shot files.
        /// </summary>
        public bool CanHandleBestShot
        {
            get { return (this.converter != null) && this.converter.HasBestShotConverter; }
        }

        /// <summary>
        /// Gets a number string indicating the last selected work.
        /// </summary>
        public string LastWorkNumber
        {
            get
            {
                return Settings.Instance.LastTitle;
            }

            private set
            {
#if false
                // Note: The following occurs CS0206.
                this.SetProperty(ref Settings.Instance.LastTitle, value);
#else
                if (Settings.Instance.LastTitle != value)
                {
                    Settings.Instance.LastTitle = value;
                    if (!Settings.Instance.Dictionary.ContainsKey(value))
                        Settings.Instance.Dictionary.Add(value, new SettingsPerTitle());
                    this.OnPropertyChanged(() => this.LastWorkNumber);
                }
#endif
            }
        }

        /// <summary>
        /// Gets a string indicating the supported version of the score file to convert.
        /// </summary>
        public string SupportedVersion
        {
            get
            {
                return (this.converter != null) ? this.converter.SupportedVersions : string.Empty;
            }
        }

        /// <summary>
        /// Gets a path of the score file.
        /// </summary>
        public string ScoreFile
        {
            get
            {
                return CurrentSetting.ScoreFile;
            }

            private set
            {
                if ((CurrentSetting.ScoreFile != value) && File.Exists(value))
                {
                    CurrentSetting.ScoreFile = value;
                    this.OnPropertyChanged(() => this.ScoreFile);
                }
            }
        }

        /// <summary>
        /// Gets a path of the best shot directory.
        /// </summary>
        public string BestShotDirectory
        {
            get
            {
                return CurrentSetting.BestShotDirectory;
            }

            private set
            {
                if ((CurrentSetting.BestShotDirectory != value) && Directory.Exists(value))
                {
                    CurrentSetting.BestShotDirectory = value;
                    this.OnPropertyChanged(() => this.BestShotDirectory);
                }
            }
        }

        /// <summary>
        /// Gets a list of the paths of template files.
        /// </summary>
        public IEnumerable<string> TemplateFiles
        {
            get
            {
                return CurrentSetting.TemplateFiles;
            }

            private set
            {
                CurrentSetting.TemplateFiles = value.Where(elem => File.Exists(elem));
                this.OnPropertyChanged(() => this.TemplateFiles);
            }
        }

        /// <summary>
        /// Gets a path of the output directory.
        /// </summary>
        public string OutputDirectory
        {
            get
            {
                return CurrentSetting.OutputDirectory;
            }

            private set
            {
                if ((CurrentSetting.OutputDirectory != value) && Directory.Exists(value))
                {
                    CurrentSetting.OutputDirectory = value;
                    this.OnPropertyChanged(() => this.OutputDirectory);
                }
            }
        }

        /// <summary>
        /// Gets a name of the output directory for image files.
        /// </summary>
        public string ImageOutputDirectory
        {
            get
            {
                return CurrentSetting.ImageOutputDirectory;
            }

            private set
            {
                if (CurrentSetting.ImageOutputDirectory != value)
                {
                    CurrentSetting.ImageOutputDirectory = value;
                    this.OnPropertyChanged(() => this.ImageOutputDirectory);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the conversion process can replace spell card names.
        /// </summary>
        public bool CanReplaceCardNames
        {
            get
            {
                return (this.converter != null) && this.converter.HasCardReplacer;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the conversion process hides untried cards.
        /// </summary>
        public bool HidesUntriedCards
        {
            get
            {
                return CurrentSetting.HideUntriedCards;
            }

            private set
            {
                if (CurrentSetting.HideUntriedCards != value)
                {
                    CurrentSetting.HideUntriedCards = value;
                    this.OnPropertyChanged(() => this.HidesUntriedCards);
                }
            }
        }

        /// <summary>
        /// Gets a log text.
        /// </summary>
        public string Log
        {
            get { return this.log; }
            private set { this.SetProperty(ref this.log, value); }
        }

        #region Commands

        /// <summary>
        /// Gets the command to select a score file.
        /// </summary>
        public DelegateCommand SelectScoreFileCommand { get; private set; }

        /// <summary>
        /// Gets the command to select a best shot directory.
        /// </summary>
        public DelegateCommand SelectBestShotDirectoryCommand { get; private set; }

        /// <summary>
        /// Gets the command to add some files to the list of template files.
        /// </summary>
        public DelegateCommand AddTemplateFilesCommand { get; private set; }

        /// <summary>
        /// Gets the command to delete some files from the list of template files.
        /// </summary>
        public DelegateCommand<IList> DeleteTemplateFilesCommand { get; private set; }

        /// <summary>
        /// Gets the command to delete all files from the list of template files.
        /// </summary>
        public DelegateCommand DeleteAllTemplateFilesCommand { get; private set; }

        /// <summary>
        /// Gets the command to select an output directory.
        /// </summary>
        public DelegateCommand SelectOutputDirectoryCommand { get; private set; }

        /// <summary>
        /// Gets the command to convert the score file.
        /// </summary>
        public DelegateCommand ConvertCommand { get; private set; }

        #endregion

        #endregion

        /// <summary>
        /// Gets the setting for the currently selected Touhou work.
        /// </summary>
        private static SettingsPerTitle CurrentSetting
        {
            get { return Settings.Instance.Dictionary[Settings.Instance.LastTitle]; }
        }

        /// <summary>
        /// Overrides the mouse cursor for the entire application.
        /// </summary>
        /// <param name="cursor">The new cursor or <c>null</c>.</param>
        private void OverrideCursor(Cursor cursor)
        {
            var dispatcher = App.Current.Dispatcher;
            if (dispatcher.CheckAccess())
                Mouse.OverrideCursor = cursor;
            else
                dispatcher.Invoke(() => this.OverrideCursor(cursor));
        }

        #region Methods for command implementation

        /// <summary>
        /// Returns a value indicating whether <see cref="SelectScoreFile"/> can be invoked.
        /// </summary>
        /// <returns>
        /// <c>true</c> if <see cref="SelectScoreFile"/> can be invoked; otherwise, <c>false</c>.
        /// </returns>
        private bool CanSelectScoreFile()
        {
            return true;
        }

        /// <summary>
        /// Selects a score file.
        /// </summary>
        private void SelectScoreFile()
        {
            // FIXME: Implement here instead of MainWindow.BtnScore_Click().
        }

        /// <summary>
        /// Returns a value indicating whether <see cref="SelectBestShotDirectory"/> can be invoked.
        /// </summary>
        /// <returns>
        /// <c>true</c> if <see cref="SelectBestShotDirectory"/> can be invoked; otherwise, <c>false</c>.
        /// </returns>
        private bool CanSelectBestShotDirectory()
        {
            return (this.converter != null) && this.converter.HasBestShotConverter;
        }

        /// <summary>
        /// Selects a best shot directory.
        /// </summary>
        private void SelectBestShotDirectory()
        {
            // FIXME: Implement here instead of MainWindow.BtnBestShot_Click().
        }

        /// <summary>
        /// Returns a value indicating whether <see cref="AddTemplateFiles"/> can be invoked.
        /// </summary>
        /// <returns>
        /// <c>true</c> if <see cref="AddTemplateFiles"/> can be invoked; otherwise, <c>false</c>.
        /// </returns>
        private bool CanAddTemplateFiles()
        {
            return true;
        }

        /// <summary>
        /// Adds some files to the list of template files.
        /// </summary>
        private void AddTemplateFiles()
        {
            // FIXME: Implement here instead of MainWindow.BtnTemplateAdd_Click().
        }

        /// <summary>
        /// Returns a value indicating whether <see cref="DeleteTemplateFiles"/> can be invoked.
        /// </summary>
        /// <param name="selectedItems">A list indicating the path strings which will be deleted.</param>
        /// <returns>
        /// <c>true</c> if <see cref="DeleteTemplateFiles"/> can be invoked; otherwise, <c>false</c>.
        /// </returns>
        private bool CanDeleteTemplateFiles(IList selectedItems)
        {
            return (selectedItems != null) && (selectedItems.Count > 0);
        }

        /// <summary>
        /// Deletes some files from the list of template files.
        /// </summary>
        /// <param name="selectedItems">A list indicating the path strings which are deleted.</param>
        private void DeleteTemplateFiles(IList selectedItems)
        {
            if ((selectedItems != null) && (selectedItems.Count > 0))
                this.TemplateFiles = this.TemplateFiles.Except(selectedItems.Cast<string>());
        }

        /// <summary>
        /// Returns a value indicating whether <see cref="DeleteAllTemplateFiles"/> can be invoked.
        /// </summary>
        /// <returns>
        /// <c>true</c> if <see cref="DeleteAllTemplateFiles"/> can be invoked; otherwise, <c>false</c>.
        /// </returns>
        private bool CanDeleteAllTemplateFiles()
        {
            return this.TemplateFiles.Count() > 0;
        }

        /// <summary>
        /// Deletes all files from the list of template files.
        /// </summary>
        private void DeleteAllTemplateFiles()
        {
            this.TemplateFiles = new string[] { };
        }

        /// <summary>
        /// Returns a value indicating whether <see cref="SelectOutputDirectory"/> can be invoked.
        /// </summary>
        /// <returns>
        /// <c>true</c> if <see cref="SelectOutputDirectory"/> can be invoked; otherwise, <c>false</c>.
        /// </returns>
        private bool CanSelectOutputDirectory()
        {
            return true;
        }

        /// <summary>
        /// Selects an output directory.
        /// </summary>
        private void SelectOutputDirectory()
        {
            // FIXME: Implement here instead of MainWindow.BtnOutput_Click().
        }

        /// <summary>
        /// Returns a value indicating whether <see cref="Convert"/> can be invoked.
        /// </summary>
        /// <returns>
        /// <c>true</c> if <see cref="Convert"/> can be invoked; otherwise, <c>false</c>.
        /// </returns>
        private bool CanConvert()
        {
            return !string.IsNullOrEmpty(this.ScoreFile) &&
                   (this.TemplateFiles.Count() > 0) &&
                   !string.IsNullOrEmpty(this.OutputDirectory) &&
                   !(this.CanHandleBestShot && string.IsNullOrEmpty(this.BestShotDirectory)) &&
                   !(this.CanHandleBestShot && string.IsNullOrEmpty(this.ImageOutputDirectory));
        }

        /// <summary>
        /// Converts the score file.
        /// </summary>
        private void Convert()
        {
            this.IsIdle = false;
            this.Log = Resources.msgStartConversion + Environment.NewLine;
            new Thread(new ParameterizedThreadStart(this.converter.Convert)).Start(CurrentSetting);
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Handles the event indicating a property value is changed.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "IsIdle":
                    this.OverrideCursor(this.IsIdle ? null : Cursors.Wait);
                    break;

                case "LastWorkNumber":
                    this.converter = ThConverterFactory.Create(Settings.Instance.LastTitle);
                    this.converter.ConvertFinished += this.OnConvertFinished;
                    this.converter.ConvertAllFinished += this.OnConvertAllFinished;
                    this.converter.ExceptionOccurred += this.OnExceptionOccurred;
                    this.IsIdle = true;
                    this.Log = string.Empty;

                    this.OnPropertyChanged(() => this.SupportedVersion);
                    this.OnPropertyChanged(() => this.ScoreFile);
                    this.OnPropertyChanged(() => this.CanHandleBestShot);
                    this.OnPropertyChanged(() => this.BestShotDirectory);
                    this.OnPropertyChanged(() => this.TemplateFiles);
                    this.OnPropertyChanged(() => this.OutputDirectory);
                    this.OnPropertyChanged(() => this.ImageOutputDirectory);
                    this.OnPropertyChanged(() => this.CanReplaceCardNames);
                    this.OnPropertyChanged(() => this.HidesUntriedCards);

                    this.ConvertCommand.RaiseCanExecuteChanged();
                    break;

                case "ScoreFile":
                    this.SelectScoreFileCommand.RaiseCanExecuteChanged();
                    this.ConvertCommand.RaiseCanExecuteChanged();
                    break;

                case "BestShotDirectory":
                    this.SelectBestShotDirectoryCommand.RaiseCanExecuteChanged();
                    this.ConvertCommand.RaiseCanExecuteChanged();
                    break;

                case "TemplateFiles":
                    this.DeleteTemplateFilesCommand.RaiseCanExecuteChanged();
                    this.DeleteAllTemplateFilesCommand.RaiseCanExecuteChanged();
                    this.ConvertCommand.RaiseCanExecuteChanged();
                    break;

                case "OutputDirectory":
                    this.SelectOutputDirectoryCommand.RaiseCanExecuteChanged();
                    this.ConvertCommand.RaiseCanExecuteChanged();
                    break;

                case "ImageOutputDirectory":
                    this.ConvertCommand.RaiseCanExecuteChanged();
                    break;
            }
        }

        /// <summary>
        /// Handles the event indicating the conversion process per file has finished.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void OnConvertFinished(object sender, ThConverterEventArgs e)
        {
            this.Log += e.Message + Environment.NewLine;
        }

        /// <summary>
        /// Handles the event indicating the all conversion process has finished.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void OnConvertAllFinished(object sender, ThConverterEventArgs e)
        {
            this.Log += Resources.msgEndConversion + Environment.NewLine;
            this.IsIdle = true;
        }

        /// <summary>
        /// Handles the event indicating an exception has occurred.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void OnExceptionOccurred(object sender, ExceptionOccurredEventArgs e)
        {
#if DEBUG
            this.Log += e.Exception.Message + Environment.NewLine;
#endif
            this.Log += Resources.msgErrUnhandledException + Environment.NewLine;
            this.IsIdle = true;
        }

        #endregion
    }
}
