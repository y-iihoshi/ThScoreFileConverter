//-----------------------------------------------------------------------
// <copyright file="IAbilityCardHolder.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models.Th18;

/// <summary>
/// Provides the ability card related get-only properties.
/// </summary>
internal interface IAbilityCardHolder
{
    /// <summary>
    /// Gets the ability card unlocked flags.
    /// Each element represents that the corresponded card is unlocked (0x01) or not (0x00).
    /// </summary>
    IEnumerable<byte> AbilityCards { get; }

    /// <summary>
    /// Gets identifiers of the ability cards that are equipped initially.
    /// </summary>
    IEnumerable<byte> InitialHoldAbilityCards { get; }
}
