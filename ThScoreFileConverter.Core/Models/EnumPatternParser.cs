//-----------------------------------------------------------------------
// <copyright file="EnumPatternParser.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Linq;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;

namespace ThScoreFileConverter.Core.Models;

/// <summary>
/// Provides a parser for the enumeration type which fields have pattern strings.
/// </summary>
/// <typeparam name="TEnum">The enumeration type which fields have pattern strings.</typeparam>
public sealed class EnumPatternParser<TEnum>
    where TEnum : struct, Enum
{
    /// <summary>
    /// A regular expression of <typeparamref name="TEnum"/>.
    /// </summary>
    private static readonly string PatternImpl =
        string.Join("|", EnumHelper<TEnum>.Enumerable.Select(value => value.ToPattern()).Distinct());

    /// <summary>
    /// Gets a regular expression of <typeparamref name="TEnum"/>.
    /// </summary>
    public string Pattern => PatternImpl;

    /// <summary>
    /// Converts from the string matched with the pattern to a value of <typeparamref name="TEnum"/>.
    /// </summary>
    /// <param name="pattern">The string matched with the pattern.</param>
    /// <returns>A value of <typeparamref name="TEnum"/>.</returns>
    public TEnum Parse(string pattern)
    {
        return EnumHelper<TEnum>.Enumerable.First(
            value => value.ToPattern().Equals(pattern, StringComparison.OrdinalIgnoreCase));
    }
}
