//-----------------------------------------------------------------------
// <copyright file="Day.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th165;

/// <summary>
/// Represents days of VD.
/// </summary>
public enum Day
{
    /// <summary>
    /// Sunday of week 1.
    /// </summary>
    [EnumAltName("01", LongName = "01")]
    [Pattern("01")]
    Sunday,

    /// <summary>
    /// Monday of week 1.
    /// </summary>
    [EnumAltName("02", LongName = "02")]
    [Pattern("02")]
    Monday,

    /// <summary>
    /// Tuesday of week 1.
    /// </summary>
    [EnumAltName("03", LongName = "03")]
    [Pattern("03")]
    Tuesday,

    /// <summary>
    /// Wednesday of week 1.
    /// </summary>
    [EnumAltName("04", LongName = "04")]
    [Pattern("04")]
    Wednesday,

    /// <summary>
    /// Thursday of week 1.
    /// </summary>
    [EnumAltName("05", LongName = "05")]
    [Pattern("05")]
    Thursday,

    /// <summary>
    /// Friday of week 1.
    /// </summary>
    [EnumAltName("06", LongName = "06")]
    [Pattern("06")]
    Friday,

    /// <summary>
    /// Saturday of week 1.
    /// </summary>
    [EnumAltName("07", LongName = "07")]
    [Pattern("07")]
    Saturday,

    /// <summary>
    /// Wrong Sunday of week 2.
    /// </summary>
    [EnumAltName("W1", LongName = "08")]
    [Pattern("W1")]
    WrongSunday,

    /// <summary>
    /// Wrong Monday of week 2.
    /// </summary>
    [EnumAltName("W2", LongName = "09")]
    [Pattern("W2")]
    WrongMonday,

    /// <summary>
    /// Wrong Tuesday of week 2.
    /// </summary>
    [EnumAltName("W3", LongName = "10")]
    [Pattern("W3")]
    WrongTuesday,

    /// <summary>
    /// Wrong Wednesday of week 2.
    /// </summary>
    [EnumAltName("W4", LongName = "11")]
    [Pattern("W4")]
    WrongWednesday,

    /// <summary>
    /// Wrong Thursday of week 2.
    /// </summary>
    [EnumAltName("W5", LongName = "12")]
    [Pattern("W5")]
    WrongThursday,

    /// <summary>
    /// Wrong Friday of week 2.
    /// </summary>
    [EnumAltName("W6", LongName = "13")]
    [Pattern("W6")]
    WrongFriday,

    /// <summary>
    /// Wrong Saturday of week 2.
    /// </summary>
    [EnumAltName("W7", LongName = "14")]
    [Pattern("W7")]
    WrongSaturday,

    /// <summary>
    /// Nightmare Sunday of week 3.
    /// </summary>
    [EnumAltName("N1", LongName = "15")]
    [Pattern("N1")]
    NightmareSunday,

    /// <summary>
    /// Nightmare Monday of week 3.
    /// </summary>
    [EnumAltName("N2", LongName = "16")]
    [Pattern("N2")]
    NightmareMonday,

    /// <summary>
    /// Nightmare Tuesday of week 3.
    /// </summary>
    [EnumAltName("N3", LongName = "17")]
    [Pattern("N3")]
    NightmareTuesday,

    /// <summary>
    /// Nightmare Wednesday of week 3.
    /// </summary>
    [EnumAltName("N4", LongName = "18")]
    [Pattern("N4")]
    NightmareWednesday,

    /// <summary>
    /// Nightmare Thursday of week 3.
    /// </summary>
    [EnumAltName("N5", LongName = "19")]
    [Pattern("N5")]
    NightmareThursday,

    /// <summary>
    /// Nightmare Friday of week 3.
    /// </summary>
    [EnumAltName("N6", LongName = "20")]
    [Pattern("N6")]
    NightmareFriday,

    /// <summary>
    /// Nightmare Saturday of week 3.
    /// </summary>
    [EnumAltName("N7", LongName = "21")]
    [Pattern("N7")]
    NightmareSaturday,

    /// <summary>
    /// Nightmare Diary.
    /// </summary>
    [EnumAltName("ND", LongName = "22")]
    [Pattern("ND")]
    NightmareDiary,
}
