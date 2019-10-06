//-----------------------------------------------------------------------
// <copyright file="IClearData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th15
{
    internal interface IClearData : Th095.IChapter
    {
        Th15Converter.CharaWithTotal Chara { get; }

        IReadOnlyDictionary<Th15Converter.GameMode, IClearDataPerGameMode> GameModeData { get; }

        IReadOnlyDictionary<(Level, Th15Converter.StagePractice), Th13.IPractice> Practices { get; }
    }
}
