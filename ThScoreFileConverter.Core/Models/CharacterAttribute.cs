//-----------------------------------------------------------------------
// <copyright file="CharacterAttribute.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.ComponentModel;
using CommunityToolkit.Diagnostics;
using ThScoreFileConverter.Core.Resources;

namespace ThScoreFileConverter.Core.Models;

/// <summary>
/// Provides names of the character represented as an enumeration field.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
public sealed class CharacterAttribute : EnumDisplayAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CharacterAttribute"/> class.
    /// </summary>
    /// <param name="name">The name of the character.</param>
    /// <param name="index">The index specifying the created instance of <see cref="CharacterAttribute"/>.</param>
    public CharacterAttribute([Localizable(false)] string name, int index = 0)
        : base(name, $"{name}{nameof(FullName)}", typeof(CharacterNames))
    {
        Guard.IsGreaterThanOrEqualTo(index, 0);
        this.Index = index;
    }

    /// <summary>
    /// Gets the index specifying this instance.
    /// </summary>
    public int Index { get; }
}
