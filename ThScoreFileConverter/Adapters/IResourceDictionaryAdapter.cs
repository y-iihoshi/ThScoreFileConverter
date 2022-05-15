//-----------------------------------------------------------------------
// <copyright file="IResourceDictionaryAdapter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Windows;
using System.Windows.Media;

namespace ThScoreFileConverter.Adapters;

/// <summary>
/// Defines the interface of an adapter for <see cref="ResourceDictionary"/>.
/// </summary>
public interface IResourceDictionaryAdapter
{
    /// <summary>
    /// Gets the font family.
    /// </summary>
    FontFamily FontFamily { get; }

    /// <summary>
    /// Gets the font size.
    /// </summary>
    double FontSize { get; }

    /// <summary>
    /// Updates the resources.
    /// </summary>
    /// <param name="fontFamily">A new font family.</param>
    /// <param name="fontSize">A new font size.</param>
    void UpdateResources(FontFamily fontFamily, double? fontSize);

    /// <summary>
    /// Updates the resources.
    /// </summary>
    /// <param name="fontFamilyName">The name of a new font family.</param>
    /// <param name="fontSize">A new font size.</param>
    void UpdateResources(string fontFamilyName, double? fontSize);
}
