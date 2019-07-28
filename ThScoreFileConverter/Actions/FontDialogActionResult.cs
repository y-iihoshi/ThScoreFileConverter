//-----------------------------------------------------------------------
// <copyright file="FontDialogActionResult.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Drawing;

namespace ThScoreFileConverter.Actions
{
    /// <summary>
    /// Represents a result of <see cref="FontDialogAction"/>.
    /// </summary>
    public sealed class FontDialogActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FontDialogActionResult"/> class.
        /// </summary>
        /// <param name="font">A font.</param>
        /// <param name="color">A color.</param>
        public FontDialogActionResult(Font font, Color color)
        {
            this.Font = font;
            this.Color = color;
        }

        /// <summary>
        /// Gets the font selected by <see cref="FontDialogAction"/>.
        /// </summary>
        public Font Font { get; }

        /// <summary>
        /// Gets the color selected by <see cref="FontDialogAction"/>.
        /// </summary>
        public Color Color { get; }
    }
}
