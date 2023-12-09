//-----------------------------------------------------------------------
// <copyright file="LocalizationProvider.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using ThScoreFileConverter.Core.Resources;
using ThScoreFileConverter.Properties;
using WPFLocalizeExtension.Providers;

namespace ThScoreFileConverter.Models;

/// <summary>
/// A simplified <see cref="ResxLocalizationProvider"/>.
/// </summary>
internal sealed class LocalizationProvider : ILocalizationProvider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LocalizationProvider"/> class.
    /// </summary>
    private LocalizationProvider()
    {
    }

    /// <inheritdoc/>
#pragma warning disable CS0067
    public event ProviderChangedEventHandler? ProviderChanged;
#pragma warning restore CS0067

    /// <inheritdoc/>
    public event ProviderErrorEventHandler? ProviderError;

    /// <inheritdoc/>
#pragma warning disable CS0067
    public event ValueChangedEventHandler? ValueChanged;
#pragma warning restore CS0067

    /// <summary>
    /// Gets the singleton object.
    /// </summary>
    public static LocalizationProvider Instance { get; } = new LocalizationProvider();

    /// <inheritdoc/>
    public ObservableCollection<CultureInfo> AvailableCultures { get; } = new ObservableCollection<CultureInfo>
    {
        CultureInfo.GetCultureInfo("en-US"),
        CultureInfo.GetCultureInfo("ja-JP"),
    };

    /// <inheritdoc/>
    public FullyQualifiedResourceKeyBase GetFullyQualifiedResourceKey(string key, DependencyObject target)
    {
        return new FQAssemblyDictionaryKey(key);
    }

    /// <inheritdoc/>
    public object? GetLocalizedObject(string key, DependencyObject target, CultureInfo culture)
    {
        var fqKey = (FQAssemblyDictionaryKey)this.GetFullyQualifiedResourceKey(key, target);

        if (string.IsNullOrEmpty(fqKey.Key))
        {
            this.OnProviderError(target, key, "No key provided.");
            return null;
        }

        try
        {
            var result = Resources.ResourceManager.GetObject(fqKey.Key, culture)
                ?? StringResources.ResourceManager.GetObject(fqKey.Key, culture);

            if (result is null)
                this.OnProviderError(target, key, "Missing key.");

            return result;
        }
#pragma warning disable CA1031 // Do not catch general exception types.
        catch (Exception e)
        {
            this.OnProviderError(target, key, $"Error retrieving the resource: {e.Message}");
            return null;
        }
#pragma warning restore CA1031 // Do not catch general exception types.
    }

    /// <summary>
    /// Fires a <see cref="ProviderError"/> event.
    /// </summary>
    /// <param name="target">The target object.</param>
    /// <param name="key">The key that caused the error.</param>
    /// <param name="message">The error message.</param>
    private void OnProviderError(DependencyObject target, string key, [Localizable(false)] string message)
    {
        this.ProviderError?.Invoke(this, new ProviderErrorEventArgs(target, key, message));
    }
}
