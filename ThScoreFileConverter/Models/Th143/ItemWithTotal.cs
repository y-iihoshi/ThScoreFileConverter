//-----------------------------------------------------------------------
// <copyright file="ItemWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models.Th143
{
    /// <summary>
    /// Represents items of ISC and total.
    /// </summary>
    public enum ItemWithTotal
    {
        /// <summary>
        /// Nimble Fabric.
        /// </summary>
        [EnumAltName("1", LongName = "ひらり布")]
        Fablic,

        /// <summary>
        /// Tengu's Toy Camera.
        /// </summary>
        [EnumAltName("2", LongName = "天狗のトイカメラ")]
        Camera,

        /// <summary>
        /// Gap Folding Umbrella.
        /// </summary>
        [EnumAltName("3", LongName = "隙間の折りたたみ傘")]
        Umbrella,

        /// <summary>
        /// Ghastly Send-Off Lantern.
        /// </summary>
        [EnumAltName("4", LongName = "亡霊の送り提灯")]
        Lantern,

        /// <summary>
        /// Bloodthirsty Yin-Yang Orb.
        /// </summary>
        [EnumAltName("5", LongName = "血に飢えた陰陽玉")]
        Orb,

        /// <summary>
        /// Four-Foot Magic Bomb.
        /// </summary>
        [EnumAltName("6", LongName = "四尺マジックボム")]
        Bomb,

        /// <summary>
        /// Substitute Jizou.
        /// </summary>
        [EnumAltName("7", LongName = "身代わり地蔵")]
        Jizou,

        /// <summary>
        /// Cursed Decoy Doll.
        /// </summary>
        [EnumAltName("8", LongName = "呪いのデコイ人形")]
        Doll,

        /// <summary>
        /// A Miracle Mallet Replica.
        /// </summary>
        [EnumAltName("9", LongName = "打ち出の小槌（レプリカ）")]
        Mallet,

        /// <summary>
        /// No item.
        /// </summary>
        [EnumAltName("0", LongName = "ノーアイテム")]
        NoItem,

        /// <summary>
        /// Represents total across items.
        /// </summary>
        [EnumAltName("T", LongName = "合計")]
        Total,
    }
}
