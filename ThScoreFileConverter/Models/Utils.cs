//-----------------------------------------------------------------------
// <copyright file="Utils.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Resources;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Extensions;
using WPFLocalizeExtension.Providers;

namespace ThScoreFileConverter.Models;

/// <summary>
/// Provides static methods for convenience.
/// </summary>
public static class Utils
{
    /// <summary>
    /// Gets a localized value.
    /// </summary>
    /// <typeparam name="T">The type of the returned value.</typeparam>
    /// <param name="key">The key.</param>
    /// <returns>The resolved localized object.</returns>
    public static T GetLocalizedValues<T>(string key)
    {
        return LocalizeDictionary.Instance.DefaultProvider is ResxLocalizationProvider
            ? LocExtension.GetLocalizedValue<T>($"{nameof(ThScoreFileConverter)}:{nameof(StringResources)}:{key}")
            : LocExtension.GetLocalizedValue<T>(key);
    }
}
