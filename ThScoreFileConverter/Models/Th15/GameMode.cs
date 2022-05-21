﻿//-----------------------------------------------------------------------
// <copyright file="GameMode.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Models;

namespace ThScoreFileConverter.Models.Th15;

/// <summary>
/// Represents game modes of LoLK.
/// </summary>
public enum GameMode
{
    /// <summary>
    /// Represents the Pointdevice Mode.
    /// </summary>
    [EnumAltName("P")]
    Pointdevice,

    /// <summary>
    /// Represents the Legacy Mode.
    /// </summary>
    [EnumAltName("L")]
    Legacy,
}
