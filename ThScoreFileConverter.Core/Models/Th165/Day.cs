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
    [Pattern("01")]
    Sunday,

    /// <summary>
    /// Monday of week 1.
    /// </summary>
    [Pattern("02")]
    Monday,

    /// <summary>
    /// Tuesday of week 1.
    /// </summary>
    [Pattern("03")]
    Tuesday,

    /// <summary>
    /// Wednesday of week 1.
    /// </summary>
    [Pattern("04")]
    Wednesday,

    /// <summary>
    /// Thursday of week 1.
    /// </summary>
    [Pattern("05")]
    Thursday,

    /// <summary>
    /// Friday of week 1.
    /// </summary>
    [Pattern("06")]
    Friday,

    /// <summary>
    /// Saturday of week 1.
    /// </summary>
    [Pattern("07")]
    Saturday,

    /// <summary>
    /// Wrong Sunday of week 2.
    /// </summary>
    [Pattern("W1")]
    WrongSunday,

    /// <summary>
    /// Wrong Monday of week 2.
    /// </summary>
    [Pattern("W2")]
    WrongMonday,

    /// <summary>
    /// Wrong Tuesday of week 2.
    /// </summary>
    [Pattern("W3")]
    WrongTuesday,

    /// <summary>
    /// Wrong Wednesday of week 2.
    /// </summary>
    [Pattern("W4")]
    WrongWednesday,

    /// <summary>
    /// Wrong Thursday of week 2.
    /// </summary>
    [Pattern("W5")]
    WrongThursday,

    /// <summary>
    /// Wrong Friday of week 2.
    /// </summary>
    [Pattern("W6")]
    WrongFriday,

    /// <summary>
    /// Wrong Saturday of week 2.
    /// </summary>
    [Pattern("W7")]
    WrongSaturday,

    /// <summary>
    /// Nightmare Sunday of week 3.
    /// </summary>
    [Pattern("N1")]
    NightmareSunday,

    /// <summary>
    /// Nightmare Monday of week 3.
    /// </summary>
    [Pattern("N2")]
    NightmareMonday,

    /// <summary>
    /// Nightmare Tuesday of week 3.
    /// </summary>
    [Pattern("N3")]
    NightmareTuesday,

    /// <summary>
    /// Nightmare Wednesday of week 3.
    /// </summary>
    [Pattern("N4")]
    NightmareWednesday,

    /// <summary>
    /// Nightmare Thursday of week 3.
    /// </summary>
    [Pattern("N5")]
    NightmareThursday,

    /// <summary>
    /// Nightmare Friday of week 3.
    /// </summary>
    [Pattern("N6")]
    NightmareFriday,

    /// <summary>
    /// Nightmare Saturday of week 3.
    /// </summary>
    [Pattern("N7")]
    NightmareSaturday,

    /// <summary>
    /// Nightmare Diary.
    /// </summary>
    [Pattern("ND")]
    NightmareDiary,
}
