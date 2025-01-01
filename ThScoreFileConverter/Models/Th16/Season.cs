//-----------------------------------------------------------------------
// <copyright file="Season.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace ThScoreFileConverter.Models.Th16;

/// <summary>
/// Represents seasons of HSiFS.
/// </summary>
public enum Season
{
    /// <summary>
    /// Spring.
    /// </summary>
    [Display(Name = "春")]
    Spring,

    /// <summary>
    /// Summer.
    /// </summary>
    [Display(Name = "夏")]
    Summer,

    /// <summary>
    /// Autumn.
    /// </summary>
    [Display(Name = "秋")]
    Autumn,

    /// <summary>
    /// Winter.
    /// </summary>
    [Display(Name = "冬")]
    Winter,

    /// <summary>
    /// Doyou, the fifth season.
    /// </summary>
    [Display(Name = "土用")]
    Full,
}
