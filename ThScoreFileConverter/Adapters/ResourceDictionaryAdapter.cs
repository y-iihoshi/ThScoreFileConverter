//-----------------------------------------------------------------------
// <copyright file="ResourceDictionaryAdapter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Media;

namespace ThScoreFileConverter.Adapters
{
    /// <summary>
    /// The adapter for <see cref="ResourceDictionary"/>.
    /// </summary>
    internal class ResourceDictionaryAdapter : IResourceDictionaryAdapter
    {
        private readonly ResourceDictionary dictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceDictionaryAdapter"/> class.
        /// </summary>
        /// <param name="dictionary">A resource dictionary to be adapted.</param>
        public ResourceDictionaryAdapter(ResourceDictionary dictionary)
        {
            if (dictionary is null)
                throw new ArgumentNullException(nameof(dictionary));

            this.dictionary = dictionary;
        }

        /// <summary>
        /// Gets a key of the font family.
        /// </summary>
        public static string FontFamilyKey { get; } = nameof(FontFamilyKey);

        /// <summary>
        /// Gets a key of the font size.
        /// </summary>
        public static string FontSizeKey { get; } = nameof(FontSizeKey);

        /// <inheritdoc/>
        public FontFamily FontFamily
        {
            get => this.dictionary[FontFamilyKey] is FontFamily family ? family : new FontFamily();
            private set => this.dictionary[FontFamilyKey] = value;
        }

        /// <inheritdoc/>
        public double FontSize
        {
            get => this.dictionary[FontSizeKey] is double size ? size : default;
            private set => this.dictionary[FontSizeKey] = value;
        }

        /// <inheritdoc/>
        public void UpdateResources(FontFamily fontFamily, double? fontSize)
        {
            if (fontFamily is { })
                this.FontFamily = fontFamily;
            if (fontSize.HasValue)
                this.FontSize = fontSize.Value;
        }

        /// <inheritdoc/>
        public void UpdateResources(string fontFamilyName, double? fontSize)
        {
            this.UpdateResources(new FontFamily(fontFamilyName), fontSize);
        }
    }
}
