//-----------------------------------------------------------------------
// <copyright file="LevelPractice.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models.Th08
{
    /// <summary>
    /// Represents level of IN spell card practice.
    /// </summary>
    public enum LevelPractice
    {
        /// <summary>
        /// Represents level Easy.
        /// </summary>
        [EnumAltName("E")]
        Easy,

        /// <summary>
        /// Represents level Normal.
        /// </summary>
        [EnumAltName("N")]
        Normal,

        /// <summary>
        /// Represents level Hard.
        /// </summary>
        [EnumAltName("H")]
        Hard,

        /// <summary>
        /// Represents level Lunatic.
        /// </summary>
        [EnumAltName("L")]
        Lunatic,

        /// <summary>
        /// Represents level Extra.
        /// </summary>
        [EnumAltName("X")]
        Extra,

        /// <summary>
        /// Represents level Last Word.
        /// </summary>
        [EnumAltName("W", LongName = "Last Word")]
        LastWord,
    }
}
