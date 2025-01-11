//-----------------------------------------------------------------------
// <copyright file="Enemy.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th165;

/// <summary>
/// Represents enemy characters of VD.
/// </summary>
public enum Enemy
{
    /// <summary>
    /// Hakurei Reimu.
    /// </summary>
    [Character(nameof(Reimu))]
    Reimu,

    /// <summary>
    /// Seiran.
    /// </summary>
    [Character(nameof(Seiran))]
    Seiran,

    /// <summary>
    /// Ringo.
    /// </summary>
    [Character(nameof(Ringo))]
    Ringo,

    /// <summary>
    /// Eternity Larva.
    /// </summary>
    [Character(nameof(Larva))]
    Larva,

    /// <summary>
    /// Kirisame Marisa.
    /// </summary>
    [Character(nameof(Marisa))]
    Marisa,

    /// <summary>
    /// Yatadera Narumi.
    /// </summary>
    [Character(nameof(Narumi))]
    Narumi,

    /// <summary>
    /// Sakata Nemuno.
    /// </summary>
    [Character(nameof(Nemuno))]
    Nemuno,

    /// <summary>
    /// Komano Aun.
    /// </summary>
    [Character(nameof(Aun))]
    Aun,

    /// <summary>
    /// Doremy Sweet.
    /// </summary>
    [Character(nameof(Doremy))]
    Doremy,

    /// <summary>
    /// Clownpiece.
    /// </summary>
    [Character(nameof(Clownpiece))]
    Clownpiece,

    /// <summary>
    /// Kishin Sagume.
    /// </summary>
    [Character(nameof(Sagume))]
    Sagume,

    /// <summary>
    /// Teireida Mai.
    /// </summary>
    [Character(nameof(Mai))]
    Mai,

    /// <summary>
    /// Nishida Satono.
    /// </summary>
    [Character(nameof(Satono))]
    Satono,

    /// <summary>
    /// Hecatia Lapislazuli.
    /// </summary>
    [Character(nameof(Hecatia))]
    Hecatia,

    /// <summary>
    /// Junko.
    /// </summary>
    [Character(nameof(Junko))]
    Junko,

    /// <summary>
    /// Matara Okina.
    /// </summary>
    [Character(nameof(Okina))]
    Okina,

    /// <summary>
    /// Remilia Scarlet.
    /// </summary>
    [Character(nameof(Remilia))]
    Remilia,

    /// <summary>
    /// Flandre Scarlet.
    /// </summary>
    [Character(nameof(Flandre))]
    Flandre,

    /// <summary>
    /// Hijiri Byakuren.
    /// </summary>
    [Character(nameof(Byakuren))]
    Byakuren,

    /// <summary>
    /// Toyosatomimi no Miko.
    /// </summary>
    [Character(nameof(Miko))]
    Miko,

    /// <summary>
    /// Saigyouji Yuyuko.
    /// </summary>
    [Character(nameof(Yuyuko))]
    Yuyuko,

    /// <summary>
    /// Shiki Eiki, Yamaxanadu.
    /// </summary>
    [Character(nameof(Eiki))]
    Eiki,

    /// <summary>
    /// Yasaka Kanako.
    /// </summary>
    [Character(nameof(Kanako))]
    Kanako,

    /// <summary>
    /// Moriya Suwako.
    /// </summary>
    [Character(nameof(Suwako))]
    Suwako,

    /// <summary>
    /// Yagokoro Eirin.
    /// </summary>
    [Character(nameof(Eirin))]
    Eirin,

    /// <summary>
    /// Houraisan Kaguya.
    /// </summary>
    [Character(nameof(Kaguya))]
    Kaguya,

    /// <summary>
    /// Hinanawi Tenshi.
    /// </summary>
    [Character(nameof(Tenshi))]
    Tenshi,

    /// <summary>
    /// Sukuna Shinmyoumaru.
    /// </summary>
    [Character(nameof(Shinmyoumaru))]
    Shinmyoumaru,

    /// <summary>
    /// Komeiji Satori.
    /// </summary>
    [Character(nameof(Satori))]
    Satori,

    /// <summary>
    /// Reiuji Utsuho.
    /// </summary>
    [Character(nameof(Utsuho))]
    Utsuho,

    /// <summary>
    /// Yakumo Ran.
    /// </summary>
    [Character(nameof(Ran))]
    Ran,

    /// <summary>
    /// Komeiji Koishi.
    /// </summary>
    [Character(nameof(Koishi))]
    Koishi,

    /// <summary>
    /// Houjuu Nue.
    /// </summary>
    [Character(nameof(Nue))]
    Nue,

    /// <summary>
    /// Futatsuiwa Mamizou.
    /// </summary>
    [Character(nameof(Mamizou))]
    Mamizou,

    /// <summary>
    /// Nagae Iku.
    /// </summary>
    [Character(nameof(Iku))]
    Iku,

    /// <summary>
    /// Horikawa Raiko.
    /// </summary>
    [Character(nameof(Raiko))]
    Raiko,

    /// <summary>
    /// Ibuki Suika.
    /// </summary>
    [Character(nameof(Suika))]
    Suika,

    /// <summary>
    /// Fujiwara no Mokou.
    /// </summary>
    [Character(nameof(Mokou))]
    Mokou,

    /// <summary>
    /// Yakumo Yukari.
    /// </summary>
    [Character(nameof(Yukari))]
    Yukari,

    /// <summary>
    /// Usami Sumireko.
    /// </summary>
    [Character(nameof(Sumireko))]
    Sumireko,

    /// <summary>
    /// Usami Sumireko (Dream World).
    /// </summary>
    [Character(nameof(DreamSumireko))]
    DreamSumireko,
}
