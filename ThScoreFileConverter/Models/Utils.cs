//-----------------------------------------------------------------------
// <copyright file="Utils.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Globalization;
using ThScoreFileConverter.Properties;
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
    /// Wraps the <c>string.Format()</c> method to specify an IFormatProvider instance.
    /// </summary>
    /// <param name="fmt">A composite format string.</param>
    /// <param name="args">An <see cref="object"/> array containing zero or more objects to format.</param>
    /// <returns>
    /// A copy of <paramref name="fmt"/> in which the format items have been replaced by the string
    /// representation of the corresponding objects in <paramref name="args"/>.
    /// </returns>
    public static string Format(string fmt, params object[] args)
    {
        return string.Format(CultureInfo.CurrentCulture, fmt, args);
    }

    /// <summary>
    /// Gets a localized value.
    /// </summary>
    /// <typeparam name="T">The type of the returned value.</typeparam>
    /// <param name="key">The key.</param>
    /// <returns>The resolved localized object.</returns>
    public static T GetLocalizedValues<T>(string key)
    {
        return LocalizeDictionary.Instance.DefaultProvider is ResxLocalizationProvider
            ? LocExtension.GetLocalizedValue<T>($"{nameof(ThScoreFileConverter)}:{nameof(Resources)}:{key}")
            : LocExtension.GetLocalizedValue<T>(key);
    }
}
