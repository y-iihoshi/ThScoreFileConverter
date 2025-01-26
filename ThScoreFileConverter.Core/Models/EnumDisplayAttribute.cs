//-----------------------------------------------------------------------
// <copyright file="EnumDisplayAttribute.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.ComponentModel;
using System.Globalization;
using System.Resources;
using CommunityToolkit.Diagnostics;

namespace ThScoreFileConverter.Core.Models;

/// <summary>
/// Provides localized strings for an enumeration field.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public abstract class EnumDisplayAttribute : Attribute
{
    private readonly ResourceManager resourceManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnumDisplayAttribute"/> class.
    /// </summary>
    /// <param name="name">The name of the playable character's shot type.</param>
    /// <param name="resourceType">The type of a resource providing localized playable character's shot type names.</param>
    protected EnumDisplayAttribute([Localizable(false)] string name, Type resourceType)
        : this(name, $"{name}{nameof(FullName)}", resourceType)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EnumDisplayAttribute"/> class.
    /// </summary>
    /// <param name="name">The name of the playable character's shot type.</param>
    /// <param name="fullName">The full name of the playable character's shot type.</param>
    /// <param name="resourceType">The type of a resource providing localized playable character's shot type names.</param>
    protected EnumDisplayAttribute([Localizable(false)] string name, [Localizable(false)] string fullName, Type resourceType)
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
    /// Gets the name of the playable character's shot type.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the full name of the playable character's shot type.
    /// </summary>
    public string FullName { get; }

    /// <summary>
    /// Gets the type of a resource providing localized playable character's shot type names.
    /// </summary>
    public Type ResourceType { get; }

    /// <summary>
    /// Gets the localized name of the playable character's shot type.
    /// </summary>
    /// <returns>The localized name.</returns>
    public string GetLocalizedName()
    {
        return this.resourceManager.GetString(this.Name, CultureInfo.CurrentCulture) ?? this.Name;
    }

    /// <summary>
    /// Gets the localized full name of the playable character's shot type.
    /// </summary>
    /// <returns>The localized full name.</returns>
    public string GetLocalizedFullName()
    {
        return this.resourceManager.GetString(this.FullName, CultureInfo.CurrentCulture) ?? this.FullName;
    }
}
