//-----------------------------------------------------------------------
// <copyright file="SettingWindowViewModel.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using ThScoreFileConverter.Actions;
using ThScoreFileConverter.Models;
using SysDraw = System.Drawing;

namespace ThScoreFileConverter.ViewModels
{
    /// <summary>
    /// The view model class for <see cref="Views.SettingWindow"/>.
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Instantiated via ViewModelLocator.")]
    internal class SettingWindowViewModel : BindableBase, IDialogAware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingWindowViewModel"/> class.
        /// </summary>
        public SettingWindowViewModel()
        {
            this.Title = "Settings";    // FIXME

            var encodings = Settings.ValidCodePageIds
                .ToDictionary(id => id, id => Utils.GetEncoding(id).EncodingName);
            this.InputEncodings = encodings;
            this.OutputEncodings = encodings;

            this.FontDialogOkCommand = new DelegateCommand<FontDialogActionResult>(this.ApplyFont);
            this.FontDialogApplyCommand = new DelegateCommand<FontDialogActionResult>(this.ApplyFont);
            this.FontDialogCancelCommand = new DelegateCommand<FontDialogActionResult>(this.ApplyFont);
            this.ResetFontCommand = new DelegateCommand(this.ResetFont);
        }

        /// <inheritdoc/>
#pragma warning disable CS0067
        public event Action<IDialogResult>? RequestClose;
#pragma warning restore CS0067

        #region Properties to bind a view

        /// <summary>
        /// Gets a title of the Settings window.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Gets the current font.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "For binding.")]
        public SysDraw.Font Font => new SysDraw.Font(
            App.Current.Resources["FontFamilyKey"].ToString(),
            Convert.ToSingle(App.Current.Resources["FontSizeKey"], CultureInfo.InvariantCulture));

        /// <summary>
        /// Gets or sets a value indicating whether numeric values is output with thousand separator
        /// characters.
        /// </summary>
        public bool OutputNumberGroupSeparator
        {
            get => Settings.Instance.OutputNumberGroupSeparator!.Value;

            set
            {
                if (Settings.Instance.OutputNumberGroupSeparator != value)
                {
                    Settings.Instance.OutputNumberGroupSeparator = value;
                    this.RaisePropertyChanged(nameof(this.OutputNumberGroupSeparator));
                }
            }
        }

        /// <summary>
        /// Gets a dictionary, which key is a code page identifier and the value is the correspond name, for
        /// input files.
        /// </summary>
        public IDictionary<int, string> InputEncodings { get; private set; }

        /// <summary>
        /// Gets or sets the code page identifier for input files.
        /// </summary>
        public int InputCodePageId
        {
            get => Settings.Instance.InputCodePageId!.Value;

            set
            {
                if (Settings.Instance.InputCodePageId != value)
                {
                    Settings.Instance.InputCodePageId = value;
                    this.RaisePropertyChanged(nameof(this.InputCodePageId));
                }
            }
        }

        /// <summary>
        /// Gets a dictionary, which key is a code page identifier and the value is the correspond name, for
        /// output files.
        /// </summary>
        public IDictionary<int, string> OutputEncodings { get; private set; }

        /// <summary>
        /// Gets or sets the code page identifier for output files.
        /// </summary>
        public int OutputCodePageId
        {
            get => Settings.Instance.OutputCodePageId!.Value;

            set
            {
                if (Settings.Instance.OutputCodePageId != value)
                {
                    Settings.Instance.OutputCodePageId = value;
                    this.RaisePropertyChanged(nameof(this.OutputCodePageId));
                }
            }
        }

        #region Commands

        /// <summary>
        /// Gets the command invoked when the user clicks an <c>OK</c> button of the font dialog box.
        /// </summary>
        public DelegateCommand<FontDialogActionResult> FontDialogOkCommand { get; private set; }

        /// <summary>
        /// Gets the command invoked when the user clicks an <c>Apply</c> button of the font dialog box.
        /// </summary>
        public DelegateCommand<FontDialogActionResult> FontDialogApplyCommand { get; private set; }

        /// <summary>
        /// Gets the command invoked when the user cancels the font choice.
        /// </summary>
        public DelegateCommand<FontDialogActionResult> FontDialogCancelCommand { get; private set; }

        /// <summary>
        /// Gets the command to reset the UI font.
        /// </summary>
        public DelegateCommand ResetFontCommand { get; private set; }

        #endregion

        #endregion

        /// <inheritdoc/>
        public bool CanCloseDialog()
        {
            return true;
        }

        /// <inheritdoc/>
        public void OnDialogClosed()
        {
        }

        /// <inheritdoc/>
        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        #region Methods for command implementation

        /// <summary>
        /// Applies the UI font change.
        /// </summary>
        /// <param name="result">A result of <see cref="FontDialogAction"/>.</param>
        private void ApplyFont(FontDialogActionResult result)
        {
            if (App.Current is App app)
            {
                app.UpdateResources(result.Font.FontFamily.Name, result.Font.Size);
                this.RaisePropertyChanged(nameof(this.Font));
            }
        }

        /// <summary>
        /// Resets the UI font.
        /// </summary>
        private void ResetFont()
        {
            if (App.Current is App app)
            {
                app.UpdateResources(SystemFonts.MessageFontFamily, SystemFonts.MessageFontSize);
                this.RaisePropertyChanged(nameof(this.Font));
            }
        }

        #endregion
    }
}
