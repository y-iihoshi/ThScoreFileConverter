//-----------------------------------------------------------------------
// <copyright file="EnumAltNameAttribute.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace ThScoreFileConverter.Core.Models;

/// <summary>
/// Provides alternative names of enumeration fields.
/// </summary>
/// <param name="shortName">A short name of the enumeration field.</param>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public sealed class EnumAltNameAttribute(string shortName) : Attribute
{

    /// <summary>
    /// Gets a short name of the enumeration field.
    /// </summary>
    public string ShortName { get; } = shortName;

    /// <summary>
    /// Gets or sets a long name of the enumeration field.
    /// </summary>
    public string LongName { get; set; } = string.Empty;
}
