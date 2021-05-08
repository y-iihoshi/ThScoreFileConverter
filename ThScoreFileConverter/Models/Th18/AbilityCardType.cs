//-----------------------------------------------------------------------
// <copyright file="AbilityCardType.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models.Th18
{
    /// <summary>
    /// Represents ability card types of UM.
    /// </summary>
    public enum AbilityCardType
    {
        /// <summary>
        /// Unknown type.
        /// </summary>
        Unknown,

        /// <summary>
        /// Item card.
        /// </summary>
        Item,

        /// <summary>
        /// Equipment card.
        /// </summary>
        Equipment,

        /// <summary>
        /// Passive card.
        /// </summary>
        Passive,

        /// <summary>
        /// Active card.
        /// </summary>
        Active,
    }
}
