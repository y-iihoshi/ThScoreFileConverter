//-----------------------------------------------------------------------
// <copyright file="IntegerParser.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Globalization;
using System.Text.RegularExpressions;
using CommunityToolkit.Diagnostics;

namespace ThScoreFileConverter.Core.Models;

/// <summary>
/// Provides a parser for an integer value.
/// </summary>
/// <param name="pattern">The regular expression used for parsing.</param>
public sealed class IntegerParser(string pattern) : IRegexParser<int>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IntegerParser"/> class.
    /// </summary>
    public IntegerParser()
        : this(@"\d+")
    {
    }

    /// <inheritdoc/>
    public string Pattern { get; } = pattern;

    /// <inheritdoc/>
    public int Parse(Group group)
    {
        Guard.IsNotNull(group);
        return int.Parse(group.Value, CultureInfo.InvariantCulture);
    }
}
