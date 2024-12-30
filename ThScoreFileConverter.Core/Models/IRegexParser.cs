//-----------------------------------------------------------------------
// <copyright file="IRegexParser.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Text.RegularExpressions;

namespace ThScoreFileConverter.Core.Models;

/// <summary>
/// Provides a parser for a type.
/// </summary>
/// <typeparam name="T">The non-null type which value is parsed from a string.</typeparam>
public interface IRegexParser<T>
    where T : notnull
{
    /// <summary>
    /// Gets the regular expression used for parsing.
    /// </summary>
    string Pattern { get; }

    /// <summary>
    /// Converts from the group matched with the pattern to a value of <typeparamref name="T"/>.
    /// </summary>
    /// <param name="group">The group matched by <see cref="Pattern"/>.</param>
    /// <returns>The parsed value of <typeparamref name="T"/>.</returns>
    T Parse(Group group);
}
