//-----------------------------------------------------------------------
// <copyright file="Season.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Models;

namespace ThScoreFileConverter.Models.Th16;

/// <summary>
/// Represents seasons of HSiFS.
/// </summary>
public enum Season
{
    /// <summary>
    /// Spring.
    /// </summary>
    [EnumAltName("春")]
    Spring,

    /// <summary>
    /// Summer.
    /// </summary>
    [EnumAltName("夏")]
    Summer,

    /// <summary>
    /// Autumn.
    /// </summary>
    [EnumAltName("秋")]
    Autumn,

    /// <summary>
    /// Winter.
    /// </summary>
    [EnumAltName("冬")]
    Winter,

    /// <summary>
    /// Doyou, the fifth season.
    /// </summary>
    [EnumAltName("土用")]
    Full,
}
