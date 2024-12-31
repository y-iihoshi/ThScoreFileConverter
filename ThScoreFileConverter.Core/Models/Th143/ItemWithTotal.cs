//-----------------------------------------------------------------------
// <copyright file="ItemWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace ThScoreFileConverter.Core.Models.Th143;

/// <summary>
/// Represents items of ISC and total.
/// </summary>
public enum ItemWithTotal
{
    /// <summary>
    /// Nimble Fabric.
    /// </summary>
    [Display(Name = "ひらり布")]
    [Pattern("1")]
    Fablic,

    /// <summary>
    /// Tengu's Toy Camera.
    /// </summary>
    [Display(Name = "天狗のトイカメラ")]
    [Pattern("2")]
    Camera,

    /// <summary>
    /// Gap Folding Umbrella.
    /// </summary>
    [Display(Name = "隙間の折りたたみ傘")]
    [Pattern("3")]
    Umbrella,

    /// <summary>
    /// Ghastly Send-Off Lantern.
    /// </summary>
    [Display(Name = "亡霊の送り提灯")]
    [Pattern("4")]
    Lantern,

    /// <summary>
    /// Bloodthirsty Yin-Yang Orb.
    /// </summary>
    [Display(Name = "血に飢えた陰陽玉")]
    [Pattern("5")]
    Orb,

    /// <summary>
    /// Four-Foot Magic Bomb.
    /// </summary>
    [Display(Name = "四尺マジックボム")]
    [Pattern("6")]
    Bomb,

    /// <summary>
    /// Substitute Jizou.
    /// </summary>
    [Display(Name = "身代わり地蔵")]
    [Pattern("7")]
    Jizou,

    /// <summary>
    /// Cursed Decoy Doll.
    /// </summary>
    [Display(Name = "呪いのデコイ人形")]
    [Pattern("8")]
    Doll,

    /// <summary>
    /// A Miracle Mallet Replica.
    /// </summary>
    [Display(Name = "打ち出の小槌（レプリカ）")]
    [Pattern("9")]
    Mallet,

    /// <summary>
    /// No item.
    /// </summary>
    [Display(Name = "ノーアイテム")]
    [Pattern("0")]
    NoItem,

    /// <summary>
    /// Represents total across items.
    /// </summary>
    [Display(Name = "合計")]
    [Pattern("T")]
    Total,
}
