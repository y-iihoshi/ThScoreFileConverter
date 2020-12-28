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
using System.Reactive.Disposables;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings.Extensions;
using ThScoreFileConverter.Adapters;
using ThScoreFileConverter.Interactivity;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Properties;
using WPFLocalizeExtension.Engine;
using SysDraw = System.Drawing;

namespace ThScoreFileConverter.ViewModels
{
    /// <summary>
    /// The view model class for <see cref="Views.SettingWindow"/>.
    /// </summary>
#if !DEBUG
    [SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Instantiated by the DI container.")]
#endif
    internal class SettingWindowViewModel : BindableBase, IDialogAware, IDisposable
    {
        private readonly ISettings settings;
        private readonly IResourceDictionaryAdapter resourceDictionaryAdapter;
        private readonly CompositeDisposable disposables;
        private bool disposed;
        private SysDraw.Font? font;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingWindowViewModel"/> class.
        /// </summary>
        /// <param name="settings">The settings of this application.</param>
        /// <param name="adapter">An adapter of the resource dictionary of this application.</param>
        public SettingWindowViewModel(ISettings settings, IResourceDictionaryAdapter adapter)
        {
            if (settings is null)
                throw new ArgumentNullException(nameof(settings));

            if (adapter is null)
                throw new ArgumentNullException(nameof(adapter));

            if (!settings.OutputNumberGroupSeparator.HasValue)
            {
                throw new ArgumentException(
                    $"{nameof(settings.OutputNumberGroupSeparator)} has no value", nameof(settings));
            }

            if (!settings.InputCodePageId.HasValue)
                throw new ArgumentException($"{nameof(settings.InputCodePageId)} has no value", nameof(settings));

            if (!settings.OutputCodePageId.HasValue)
                throw new ArgumentException($"{nameof(settings.OutputCodePageId)} has no value", nameof(settings));

            this.settings = settings;
            this.resourceDictionaryAdapter = adapter;
            this.disposables = new CompositeDisposable();
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

            this.disposables.Add(
                LocalizeDictionary.Instance.ObserveProperty(instance => instance.Culture)
                    .Subscribe(_ => this.RaisePropertyChanged(nameof(this.Title))));
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
                this.font?.Dispose();
                this.font = new SysDraw.Font(
                    this.resourceDictionaryAdapter.FontFamily.ToString(),
                    (float)this.resourceDictionaryAdapter.FontSize);
                return this.font;
            }
        }

        /// <summary>
        /// Gets the maximum font size.
        /// </summary>
#pragma warning disable CA1822 // Mark members as static
        public int MaxFontSize => (int)Settings.MaxFontSize;
#pragma warning restore CA1822 // Mark members as static

        /// <summary>
        /// Gets or sets a value indicating whether numeric values is output with thousand separator
        /// characters.
        /// </summary>
        public bool OutputNumberGroupSeparator
        {
            get => this.settings.OutputNumberGroupSeparator!.Value;

            set
            {
                if (this.settings.OutputNumberGroupSeparator != value)
                {
                    this.settings.OutputNumberGroupSeparator = value;
                    this.RaisePropertyChanged(nameof(this.OutputNumberGroupSeparator));
                }
            }
        }

        /// <summary>
        /// Gets a dictionary, which key is a code page identifier and the value is the correspond name, for
        /// input files.
        /// </summary>
        public IDictionary<int, string> InputEncodings { get; }

        /// <summary>
        /// Gets or sets the code page identifier for input files.
        /// </summary>
        public int InputCodePageId
        {
            get => this.settings.InputCodePageId!.Value;

            set
            {
                if (this.settings.InputCodePageId != value)
                {
                    this.settings.InputCodePageId = value;
                    this.RaisePropertyChanged(nameof(this.InputCodePageId));
                }
            }
        }

        /// <summary>
        /// Gets a dictionary, which key is a code page identifier and the value is the correspond name, for
        /// output files.
        /// </summary>
        public IDictionary<int, string> OutputEncodings { get; }

        /// <summary>
        /// Gets or sets the code page identifier for output files.
        /// </summary>
        public int OutputCodePageId
        {
            get => this.settings.OutputCodePageId!.Value;

            set
            {
                if (this.settings.OutputCodePageId != value)
                {
                    this.settings.OutputCodePageId = value;
                    this.RaisePropertyChanged(nameof(this.OutputCodePageId));
                }
            }
        }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        public CultureInfo Culture
        {
            get => LocalizeDictionary.Instance.Culture;

            set
            {
                if (!LocalizeDictionary.Instance.Culture.Equals(value))
                {
                    LocalizeDictionary.Instance.Culture = value;
                    this.RaisePropertyChanged(nameof(this.Culture));
                }
            }
        }

        #region Commands

        /// <summary>
        /// Gets the command invoked when the user clicks an <c>OK</c> button of the font dialog box.
        /// </summary>
        public DelegateCommand<FontDialogActionResult> FontDialogOkCommand { get; }

        /// <summary>
        /// Gets the command invoked when the user clicks an <c>Apply</c> button of the font dialog box.
        /// </summary>
        public DelegateCommand<FontDialogActionResult> FontDialogApplyCommand { get; }

        /// <summary>
        /// Gets the command invoked when the user cancels the font choice.
        /// </summary>
        public DelegateCommand<FontDialogActionResult> FontDialogCancelCommand { get; }

        /// <summary>
        /// Gets the command to reset the UI font.
        /// </summary>
        public DelegateCommand ResetFontCommand { get; }

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
                this.disposables.Dispose();
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
            this.resourceDictionaryAdapter.UpdateResources(result.Font.FontFamily.Name, result.Font.Size);
            this.RaisePropertyChanged(nameof(this.Font));
        }

        /// <summary>
        /// Resets the UI font.
        /// </summary>
        private void ResetFont()
        {
            this.ThrowIfDisposed();
            this.resourceDictionaryAdapter.UpdateResources(SystemFonts.MessageFontFamily, SystemFonts.MessageFontSize);
            this.RaisePropertyChanged(nameof(this.Font));
        }

        #endregion
    }
}
