//-----------------------------------------------------------------------
// <copyright file="IAchievementHolder.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th17;

/// <summary>
/// Provides the achievement related get-only properties.
/// </summary>
internal interface IAchievementHolder
{
    /// <summary>
    /// Gets the achievement unlocked flags.
    /// Each element represents that the corresponded achievement is unlocked (0x01) or not (0x00).
    /// </summary>
    IEnumerable<byte> Achievements { get; }
}
