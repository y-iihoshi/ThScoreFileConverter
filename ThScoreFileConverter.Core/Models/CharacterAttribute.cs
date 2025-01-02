//-----------------------------------------------------------------------
// <copyright file="CharacterAttribute.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using CommunityToolkit.Diagnostics;
using ThScoreFileConverter.Core.Resources;

namespace ThScoreFileConverter.Core.Models;

/// <summary>
/// Provides names of the character represented as an enumeration field.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public sealed class CharacterAttribute : Attribute
{
    private readonly ResourceManager resourceManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="CharacterAttribute"/> class.
    /// </summary>
    /// <param name="name">The name of the character.</param>
    public CharacterAttribute([Localizable(false)] string name)
        : this(name, $"{name}{nameof(FullName)}")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CharacterAttribute"/> class.
    /// </summary>
    /// <param name="name">The name of the character.</param>
    /// <param name="fullName">The full name of the character.</param>
    public CharacterAttribute([Localizable(false)] string name, [Localizable(false)] string fullName)
        : this(name, fullName, typeof(CharacterNames))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CharacterAttribute"/> class.
    /// </summary>
    /// <param name="name">The name of the character.</param>
    /// <param name="fullName">The full name of the character.</param>
    /// <param name="resourceType">The type of a resource providing localized character names.</param>
    public CharacterAttribute([Localizable(false)] string name, [Localizable(false)] string fullName, Type resourceType)
    {
        Guard.IsNotNullOrEmpty(name);
        Guard.IsNotNullOrEmpty(fullName);
        Guard.IsNotNull(resourceType);

        this.resourceManager = new(resourceType);
        this.Name = name;
        this.FullName = fullName;
        this.ResourceType = resourceType;
    }

    /// <summary>
    /// Gets the name of the character.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the full name of the character.
    /// </summary>
    public string FullName { get; }

    /// <summary>
    /// Gets the type of a resource providing localized character names.
    /// </summary>
    public Type ResourceType { get; }

    /// <summary>
    /// Gets the localized name of the character.
    /// </summary>
    /// <returns>The localized name.</returns>
    public string GetLocalizedName()
    {
        return this.resourceManager.GetString(this.Name, CultureInfo.CurrentCulture) ?? this.Name;
    }

    /// <summary>
    /// Gets the localized full name of the character.
    /// </summary>
    /// <returns>The localized full name.</returns>
    public string GetLocalizedFullName()
    {
        return this.resourceManager.GetString(this.FullName, CultureInfo.CurrentCulture) ?? this.FullName;
    }
}
