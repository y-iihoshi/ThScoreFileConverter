//-----------------------------------------------------------------------
// <copyright file="PatternAttribute.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace ThScoreFileConverter.Core.Models;

/// <summary>
/// Provides a string used as a regular expression pattern for an enumeration field.
/// </summary>
/// <param name="pattern">A string used as a regular expression pattern for the enumeration field.</param>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public sealed class PatternAttribute(string pattern) : Attribute
{
    /// <summary>
    /// Gets a string used as a regular expression pattern.
    /// </summary>
    public string Pattern { get; } = pattern;
}
