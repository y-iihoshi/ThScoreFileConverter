//-----------------------------------------------------------------------
// <copyright file="Day.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace ThScoreFileConverter.Core.Models.Th165;

/// <summary>
/// Represents days of VD.
/// </summary>
public enum Day
{
    /// <summary>
    /// Sunday of week 1.
    /// </summary>
    [Display(Name = "日曜日")]
    [Pattern("01")]
    Sunday,

    /// <summary>
    /// Monday of week 1.
    /// </summary>
    [Display(Name = "月曜日")]
    [Pattern("02")]
    Monday,

    /// <summary>
    /// Tuesday of week 1.
    /// </summary>
    [Display(Name = "火曜日")]
    [Pattern("03")]
    Tuesday,

    /// <summary>
    /// Wednesday of week 1.
    /// </summary>
    [Display(Name = "水曜日")]
    [Pattern("04")]
    Wednesday,

    /// <summary>
    /// Thursday of week 1.
    /// </summary>
    [Display(Name = "木曜日")]
    [Pattern("05")]
    Thursday,

    /// <summary>
    /// Friday of week 1.
    /// </summary>
    [Display(Name = "金曜日")]
    [Pattern("06")]
    Friday,

    /// <summary>
    /// Saturday of week 1.
    /// </summary>
    [Display(Name = "土曜日")]
    [Pattern("07")]
    Saturday,

    /// <summary>
    /// Wrong Sunday of week 2.
    /// </summary>
    [Display(Name = "裏・日曜日")]
    [Pattern("W1")]
    WrongSunday,

    /// <summary>
    /// Wrong Monday of week 2.
    /// </summary>
    [Display(Name = "裏・月曜日")]
    [Pattern("W2")]
    WrongMonday,

    /// <summary>
    /// Wrong Tuesday of week 2.
    /// </summary>
    [Display(Name = "裏・火曜日")]
    [Pattern("W3")]
    WrongTuesday,

    /// <summary>
    /// Wrong Wednesday of week 2.
    /// </summary>
    [Display(Name = "裏・水曜日")]
    [Pattern("W4")]
    WrongWednesday,

    /// <summary>
    /// Wrong Thursday of week 2.
    /// </summary>
    [Display(Name = "裏・木曜日")]
    [Pattern("W5")]
    WrongThursday,

    /// <summary>
    /// Wrong Friday of week 2.
    /// </summary>
    [Display(Name = "裏・金曜日")]
    [Pattern("W6")]
    WrongFriday,

    /// <summary>
    /// Wrong Saturday of week 2.
    /// </summary>
    [Display(Name = "裏・土曜日")]
    [Pattern("W7")]
    WrongSaturday,

    /// <summary>
    /// Nightmare Sunday of week 3.
    /// </summary>
    [Display(Name = "悪夢日曜")]
    [Pattern("N1")]
    NightmareSunday,

    /// <summary>
    /// Nightmare Monday of week 3.
    /// </summary>
    [Display(Name = "悪夢月曜")]
    [Pattern("N2")]
    NightmareMonday,

    /// <summary>
    /// Nightmare Tuesday of week 3.
    /// </summary>
    [Display(Name = "悪夢火曜")]
    [Pattern("N3")]
    NightmareTuesday,

    /// <summary>
    /// Nightmare Wednesday of week 3.
    /// </summary>
    [Display(Name = "悪夢水曜")]
    [Pattern("N4")]
    NightmareWednesday,

    /// <summary>
    /// Nightmare Thursday of week 3.
    /// </summary>
    [Display(Name = "悪夢木曜")]
    [Pattern("N5")]
    NightmareThursday,

    /// <summary>
    /// Nightmare Friday of week 3.
    /// </summary>
    [Display(Name = "悪夢金曜")]
    [Pattern("N6")]
    NightmareFriday,

    /// <summary>
    /// Nightmare Saturday of week 3.
    /// </summary>
    [Display(Name = "悪夢土曜")]
    [Pattern("N7")]
    NightmareSaturday,

    /// <summary>
    /// Nightmare Diary.
    /// </summary>
    [Display(Name = "ナイトメアダイアリー")]
    [Pattern("ND")]
    NightmareDiary,
}
