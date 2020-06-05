//-----------------------------------------------------------------------
// <copyright file="SettingWindowViewModel.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using ThScoreFileConverter.Actions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Properties;
using WPFLocalizeExtension.Engine;
using SysDraw = System.Drawing;

namespace ThScoreFileConverter.ViewModels
{
    /// <summary>
    /// The view model class for <see cref="Views.SettingWindow"/>.
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Instantiated via ViewModelLocator.")]
    internal class SettingWindowViewModel : BindableBase, IDialogAware, IDisposable
    {
        private bool disposed;
        private SysDraw.Font? font;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingWindowViewModel"/> class.
        /// </summary>
        public SettingWindowViewModel()
        {
            this.disposed = false;
            this.font = null;

            var encodings = Settings.ValidCodePageIds
                .ToDictionary(id => id, id => Encoding.GetEncoding(id).EncodingName);
            this.InputEncodings = encodings;
            this.OutputEncodings = encodings;

            this.FontDialogOkCommand = new DelegateCommand<FontDialogActionResult>(this.ApplyFont);
            this.FontDialogApplyCommand = new DelegateCommand<FontDialogActionResult>(this.ApplyFont);
            this.FontDialogCancelCommand = new DelegateCommand<FontDialogActionResult>(this.ApplyFont);
            this.ResetFontCommand = new DelegateCommand(this.ResetFont);

            LocalizeDictionary.Instance.PropertyChanged += this.OnLocalizeDictionaryPropertyChanged;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="SettingWindowViewModel"/> class.
        /// </summary>
        ~SettingWindowViewModel()
        {
            this.Dispose(false);
        }

        /// <inheritdoc/>
#pragma warning disable CS0067
        public event Action<IDialogResult>? RequestClose;
#pragma warning restore CS0067

        #region Properties to bind a view

        /// <summary>
        /// Gets a title of the Settings window.
        /// </summary>
        public string Title => Utils.GetLocalizedValues<string>(nameof(Resources.SettingWindowTitle));

        /// <summary>
        /// Gets the current font.
        /// </summary>
        public SysDraw.Font Font
        {
            get
            {
                var app = (App)Application.Current;
                this.font?.Dispose();
                this.font = new SysDraw.Font(app.FontFamily.ToString(), (float)app.FontSize);
                return this.font;
            }
        }

        /// <summary>
        /// Gets the maximum font size.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "For binding.")]
        public int MaxFontSize => (int)Settings.MaxFontSize;

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
            this.Dispose();
        }

        /// <inheritdoc/>
        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

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
                this.font?.Dispose();
            }

            this.disposed = true;
        }

        /// <summary>
        /// Throws <see cref="ObjectDisposedException"/> if the current instance has already been disposed.
        /// </summary>
        protected virtual void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }

        #region Methods for command implementation

        /// <summary>
        /// Applies the UI font change.
        /// </summary>
        /// <param name="result">A result of <see cref="FontDialogAction"/>.</param>
        private void ApplyFont(FontDialogActionResult result)
        {
            this.ThrowIfDisposed();
            if (Application.Current is App app)
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
            this.ThrowIfDisposed();
            if (Application.Current is App app)
            {
                app.UpdateResources(SystemFonts.MessageFontFamily, SystemFonts.MessageFontSize);
                this.RaisePropertyChanged(nameof(this.Font));
            }
        }

        #endregion

        /// <summary>
        /// Handles the event indicating a property value of <see cref="LocalizeDictionary.Instance"/> is changed.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void OnLocalizeDictionaryPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LocalizeDictionary.Instance.Culture))
            {
                this.RaisePropertyChanged(nameof(this.Title));
            }
        }
    }
}
