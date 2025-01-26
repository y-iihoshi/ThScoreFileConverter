//-----------------------------------------------------------------------
// <copyright file="StringHelper.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Globalization;
using System.Runtime.CompilerServices;

namespace ThScoreFileConverter.Helpers;

/// <summary>
/// Provides helper functions for strings.
/// </summary>
public static class StringHelper
{
    /// <summary>
    /// Wraps the <see cref="string.Create(IFormatProvider, ref DefaultInterpolatedStringHandler)"/>
    /// to specify an <see cref="IFormatProvider"/> instance.
    /// </summary>
    /// <param name="handler">The interpolated string, passed by reference.</param>
    /// <returns>
    /// The string that results for formatting the interpolated string using the format provider of
    /// <see cref="CultureInfo.CurrentCulture"/>.
    /// </returns>
    public static string Create(ref DefaultInterpolatedStringHandler handler)
    {
        return string.Create(CultureInfo.CurrentCulture, ref handler);
    }

    /// <summary>
    /// Wraps the <see cref="string.Format(IFormatProvider, string, object[])"/> to specify an <see cref="IFormatProvider"/> instance.
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
}
