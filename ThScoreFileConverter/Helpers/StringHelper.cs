//-----------------------------------------------------------------------
// <copyright file="StringHelper.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#if NET6_0_OR_GREATER
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
#endif

namespace ThScoreFileConverter.Helpers;

/// <summary>
/// Provides helper functions for strings.
/// </summary>
public static class StringHelper
{
#if NET6_0_OR_GREATER
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
#else
    /// <summary>
    /// Returns the specified interpolated string itself.
    /// The aim of this method is to simulate the <c>string.Create()</c> method added in .NET 6.
    /// </summary>
    /// <param name="interpolated">The interpolated string.</param>
    /// <returns><paramref name="interpolated"/> itself.</returns>
    public static string Create(string interpolated)
    {
        return interpolated;
    }
#endif
}
