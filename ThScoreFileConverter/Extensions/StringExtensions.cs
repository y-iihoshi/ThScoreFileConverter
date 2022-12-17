//-----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using CommunityToolkit.Diagnostics;

namespace ThScoreFileConverter.Extensions;

/// <summary>
/// Provides some extension methods for <see cref="object"/>.
/// </summary>
/// <remarks>
/// It would be better to name this class <c>ObjectExtensions</c>, but it could not because of the CA1724 warning;
/// it conflicts with the namespace name <see cref="Reactive.Bindings.ObjectExtensions"/> defined in ReactiveProperty.
/// </remarks>
public static class StringExtensions
{
    /// <summary>
    /// Returns a non-null string that represents the specified object.
    /// </summary>
    /// <param name="obj">The object to get its string representation.</param>
    /// <param name="nonNullStr">The non-null string to be returned when <paramref name="obj"/> is <see langword="null"/>.</param>
    /// <returns>A non-null string that represents <paramref name="obj"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="nonNullStr"/> is <see langword="null"/>.</exception>
    public static string ToNonNullString(this object obj, string nonNullStr = "(null)")
    {
        Guard.IsNotNull(nonNullStr);

        return obj?.ToString() ?? nonNullStr;
    }
}
