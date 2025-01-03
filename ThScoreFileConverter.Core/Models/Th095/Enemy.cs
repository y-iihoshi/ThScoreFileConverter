//-----------------------------------------------------------------------
// <copyright file="Enemy.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th095;

/// <summary>
/// Represents enemy characters of StB.
/// </summary>
public enum Enemy
{
    /// <summary>
    /// Wriggle Nightbug.
    /// </summary>
    [Character(nameof(Wriggle))]
    Wriggle,

    /// <summary>
    /// Rumia.
    /// </summary>
    [Character(nameof(Rumia))]
    Rumia,

    /// <summary>
    /// Cirno.
    /// </summary>
    [Character(nameof(Cirno))]
    Cirno,

    /// <summary>
    /// Letty Whiterock.
    /// </summary>
    [Character(nameof(Letty))]
    Letty,

    /// <summary>
    /// Alice Margatroid.
    /// </summary>
    [Character(nameof(Alice))]
    Alice,

    /// <summary>
    /// Kamishirasawa Keine.
    /// </summary>
    [Character(nameof(Keine))]
    Keine,

    /// <summary>
    /// Medicine Melancholy.
    /// </summary>
    [Character(nameof(Medicine))]
    Medicine,

    /// <summary>
    /// Inaba Tewi.
    /// </summary>
    [Character(nameof(Tewi))]
    Tewi,

    /// <summary>
    /// Reisen Udongein Inaba.
    /// </summary>
    [Character(nameof(Reisen))]
    Reisen,

    /// <summary>
    /// Hong Meiling.
    /// </summary>
    [Character(nameof(Meiling))]
    Meiling,

    /// <summary>
    /// Patchouli Knowledge.
    /// </summary>
    [Character(nameof(Patchouli))]
    Patchouli,

    /// <summary>
    /// Chen.
    /// </summary>
    [Character(nameof(Chen))]
    Chen,

    /// <summary>
    /// Konpaku Youmu.
    /// </summary>
    [Character(nameof(Youmu))]
    Youmu,

    /// <summary>
    /// Izayoi Sakuya.
    /// </summary>
    [Character(nameof(Sakuya))]
    Sakuya,

    /// <summary>
    /// Remilia Scarlet.
    /// </summary>
    [Character(nameof(Remilia))]
    Remilia,

    /// <summary>
    /// Yakumo Ran.
    /// </summary>
    [Character(nameof(Ran))]
    Ran,

    /// <summary>
    /// Saigyouji Yuyuko.
    /// </summary>
    [Character(nameof(Yuyuko))]
    Yuyuko,

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
    /// Onozuka Komachi.
    /// </summary>
    [Character(nameof(Komachi))]
    Komachi,

    /// <summary>
    /// Shiki Eiki, Yamaxanadu.
    /// </summary>
    [Character(nameof(Eiki))]
    Eiki,

    /// <summary>
    /// Flandre Scarlet.
    /// </summary>
    [Character(nameof(Flandre))]
    Flandre,

    /// <summary>
    /// Yakumo Yukari.
    /// </summary>
    [Character(nameof(Yukari))]
    Yukari,

    /// <summary>
    /// Fujiwara no Mokou.
    /// </summary>
    [Character(nameof(Mokou))]
    Mokou,

    /// <summary>
    /// Ibuki Suika.
    /// </summary>
    [Character(nameof(Suika))]
    Suika,
}
