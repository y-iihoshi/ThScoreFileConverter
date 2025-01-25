//-----------------------------------------------------------------------
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
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using MvvmDialogs;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Resources;
using WPFLocalizeExtension.Engine;

namespace ThScoreFileConverter.ViewModels;

/// <summary>
/// The view model class for <see cref="Views.SettingWindow"/>.
/// </summary>
#if !DEBUG
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Instantiated by the DI container.")]
#endif
internal sealed partial class SettingWindowViewModel : ObservableObject, IModalDialogViewModel, IDisposable
{
    private readonly CompositeDisposable disposables;

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingWindowViewModel"/> class.
    /// </summary>
    /// <param name="settings">The settings of this application.</param>
    public SettingWindowViewModel(Settings settings)
    {
        Guard.IsTrue(settings.OutputNumberGroupSeparator.HasValue, nameof(settings), $"{nameof(settings.OutputNumberGroupSeparator)} has no value");
        Guard.IsTrue(settings.InputCodePageId.HasValue, nameof(settings), $"{nameof(settings.InputCodePageId)} has no value");
        Guard.IsTrue(settings.OutputCodePageId.HasValue, nameof(settings), $"{nameof(settings.OutputCodePageId)} has no value");

        this.disposables = [];

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
    public string Title => Utils.GetLocalizedValues<string>(nameof(StringResources.SettingWindowTitle));
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
        this.OutputCodePageId.Dispose();
        this.InputCodePageId.Dispose();
        this.OutputNumberGroupSeparator.Dispose();
        this.disposables.Dispose();
    }
}
