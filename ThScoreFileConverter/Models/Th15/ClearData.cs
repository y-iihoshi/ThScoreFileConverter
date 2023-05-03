//-----------------------------------------------------------------------
// <copyright file="ClearData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th15;
using ThScoreFileConverter.Helpers;
using GameMode = ThScoreFileConverter.Core.Models.Th15.GameMode;

namespace ThScoreFileConverter.Models.Th15;

internal class ClearData : Th10.Chapter, IClearData // per character
{
    public const string ValidSignature = "CR";
    public const ushort ValidVersion = 0x0001;
    public const int ValidSize = 0x0000A4A0;

    public ClearData(Th10.Chapter chapter)
        : base(chapter, ValidSignature, ValidVersion, ValidSize)
    {
        using var stream = new MemoryStream(this.Data, false);
        using var reader = new BinaryReader(stream);

        this.Chara = (CharaWithTotal)reader.ReadInt32();

        this.GameModeData = EnumHelper<GameMode>.Enumerable.ToDictionary(
            mode => mode, _ => BinaryReadableHelper.Create<ClearDataPerGameMode>(reader) as IClearDataPerGameMode);

        this.Practices = EnumHelper.Cartesian<Level, Th14.StagePractice>()
            .ToDictionary(pair => pair, _ => BinaryReadableHelper.Create<Th10.Practice>(reader) as Th10.IPractice);
    }

    public CharaWithTotal Chara { get; }

    public IReadOnlyDictionary<GameMode, IClearDataPerGameMode> GameModeData { get; }

    public IReadOnlyDictionary<(Level Level, Th14.StagePractice Stage), Th10.IPractice> Practices { get; }

    public static bool CanInitialize(Th10.Chapter chapter)
    {
        return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
            && (chapter.Version == ValidVersion)
            && (chapter.Size == ValidSize);
    }
}
