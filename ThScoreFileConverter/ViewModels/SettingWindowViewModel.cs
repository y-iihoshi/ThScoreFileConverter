﻿//-----------------------------------------------------------------------
// <copyright file="SettingWindowViewModel.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using ThScoreFileConverter.Adapters;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Interactivity;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Properties;
using WPFLocalizeExtension.Engine;
using SysDraw = System.Drawing;

namespace ThScoreFileConverter.ViewModels;

/// <summary>
/// The view model class for <see cref="Views.SettingWindow"/>.
/// </summary>
#if !DEBUG
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Instantiated by the DI container.")]
#endif
internal sealed partial class SettingWindowViewModel : ObservableObject, IModalDialogViewModel, IDisposable
{
    private readonly IResourceDictionaryAdapter resourceDictionaryAdapter;
    private readonly CompositeDisposable disposables;
    private bool disposed;
    private SysDraw.Font? font;

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingWindowViewModel"/> class.
    /// </summary>
    /// <param name="settings">The settings of this application.</param>
    /// <param name="adapter">An adapter of the resource dictionary of this application.</param>
    public SettingWindowViewModel(Settings settings, IResourceDictionaryAdapter adapter)
    {
        Guard.IsTrue(settings.OutputNumberGroupSeparator.HasValue, nameof(settings), $"{nameof(settings.OutputNumberGroupSeparator)} has no value");
        Guard.IsTrue(settings.InputCodePageId.HasValue, nameof(settings), $"{nameof(settings.InputCodePageId)} has no value");
        Guard.IsTrue(settings.OutputCodePageId.HasValue, nameof(settings), $"{nameof(settings.OutputCodePageId)} has no value");

        this.resourceDictionaryAdapter = adapter;
        this.disposables = [];
        this.disposed = false;
        this.font = null;

        this.OutputNumberGroupSeparator = settings.ToReactivePropertyAsSynchronized(
            x => x.OutputNumberGroupSeparator, value => (bool)value!, value => value);
        this.InputCodePageId = settings.ToReactivePropertyAsSynchronized(
            x => x.InputCodePageId, value => (int)value!, value => value);
        this.OutputCodePageId = settings.ToReactivePropertyAsSynchronized(
            x => x.OutputCodePageId, value => (int)value!, value => value);

        var encodings = Settings.ValidCodePageIds
            .ToDictionary(id => id, id => EncodingHelper.GetEncoding(id).EncodingName);
        this.InputEncodings = encodings;
        this.OutputEncodings = encodings;

        this.disposables.Add(
            LocalizeDictionary.Instance.ObserveProperty(instance => instance.Culture)
                .Subscribe(_ => this.OnPropertyChanged(nameof(this.Title))));
    }

    #region Properties to bind a view

    /// <summary>
    /// Gets a title of the Settings window.
    /// </summary>
#pragma warning disable CA1822 // Mark members as static
    public string Title => Utils.GetLocalizedValues<string>(nameof(Resources.SettingWindowTitle));
#pragma warning restore CA1822 // Mark members as static

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
    /// Gets a value indicating whether numeric values is output with thousand separator
    /// characters.
    /// </summary>
    public ReactiveProperty<bool> OutputNumberGroupSeparator { get; }

    /// <summary>
    /// Gets a dictionary, which key is a code page identifier and the value is the correspond name, for
    /// input files.
    /// </summary>
    public IReadOnlyDictionary<int, string> InputEncodings { get; }

    /// <summary>
    /// Gets the code page identifier for input files.
    /// </summary>
    public ReactiveProperty<int> InputCodePageId { get; }

    /// <summary>
    /// Gets a dictionary, which key is a code page identifier and the value is the correspond name, for
    /// output files.
    /// </summary>
    public IReadOnlyDictionary<int, string> OutputEncodings { get; }

    /// <summary>
    /// Gets the code page identifier for output files.
    /// </summary>
    public ReactiveProperty<int> OutputCodePageId { get; }

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
                this.OnPropertyChanged(nameof(this.Culture));
            }
        }
    }

    #endregion

    /// <inheritdoc/>
    public bool? DialogResult { get; } = true;

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
            this.OutputCodePageId.Dispose();
            this.InputCodePageId.Dispose();
            this.OutputNumberGroupSeparator.Dispose();

            this.font?.Dispose();
            this.disposables.Dispose();
        }

        this.disposed = true;
    }

    /// <summary>
    /// Throws <see cref="ObjectDisposedException"/> if the current instance has already been disposed.
    /// </summary>
    private void ThrowIfDisposed()
    {
        if (this.disposed)
        {
            ThrowHelper.ThrowObjectDisposedException(this.GetType().FullName);
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
        this.OnPropertyChanged(nameof(this.Font));
    }

    /// <summary>
    /// Invoked when the user clicks an <c>OK</c> button of the font dialog box.
    /// </summary>
    /// <param name="result">The result of the font dialog box.</param>
    [RelayCommand]
    private void FontDialogOk(FontDialogActionResult result)
    {
        this.ApplyFont(result);
    }

    /// <summary>
    /// Invoked when the user clicks an <c>Apply</c> button of the font dialog box.
    /// </summary>
    /// <param name="result">The result of the font dialog box.</param>
    [RelayCommand]
    private void FontDialogApply(FontDialogActionResult result)
    {
        this.ApplyFont(result);
    }

    /// <summary>
    /// Invoked when the user cancels the font choice.
    /// </summary>
    /// <param name="result">The result of the font dialog box.</param>
    [RelayCommand]
    private void FontDialogCancel(FontDialogActionResult result)
    {
        this.ApplyFont(result);
    }

    /// <summary>
    /// Resets the UI font.
    /// </summary>
    [RelayCommand]
    private void ResetFont()
    {
        this.ThrowIfDisposed();
        this.resourceDictionaryAdapter.UpdateResources(SystemFonts.MessageFontFamily, SystemFonts.MessageFontSize);
        this.OnPropertyChanged(nameof(this.Font));
    }

    #endregion
}
