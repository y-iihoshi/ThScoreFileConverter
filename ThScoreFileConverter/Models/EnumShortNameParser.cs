//-----------------------------------------------------------------------
// <copyright file="EnumShortNameParser.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Linq;
using System.Text.RegularExpressions;
using CommunityToolkit.Diagnostics;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;

namespace ThScoreFileConverter.Models;

/// <summary>
/// Provides a parser for the enumeration type which fields have short names.
/// </summary>
/// <typeparam name="TEnum">The enumeration type which fields have short names.</typeparam>
public sealed class EnumShortNameParser<TEnum> : Core.Models.IRegexParser<TEnum>
    where TEnum : struct, Enum
{
    /// <summary>
    /// A regular expression of the short names of <typeparamref name="TEnum"/>.
    /// </summary>
    private static readonly string PatternImpl =
        string.Join("|", EnumHelper<TEnum>.Enumerable.Select(elem => elem.ToShortName()).Distinct());

    /// <summary>
    /// Gets a regular expression of the short names of <typeparamref name="TEnum"/>.
    /// </summary>
    public string Pattern => PatternImpl;

    /// <summary>
    /// Converts from the string matched with the pattern to a value of <typeparamref name="TEnum"/>.
    /// </summary>
    /// <param name="shortName">The string matched with the pattern.</param>
    /// <returns>A value of <typeparamref name="TEnum"/>.</returns>
    public TEnum Parse(string shortName)
    {
        return EnumHelper<TEnum>.Enumerable.First(
            elem => elem.ToShortName().Equals(shortName, StringComparison.OrdinalIgnoreCase));
    }

    /// <inheritdoc/>
    public TEnum Parse(Group group)
    {
        Guard.IsNotNull(group);
        return this.Parse(group.Value);
    }
}
