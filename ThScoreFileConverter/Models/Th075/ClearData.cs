//-----------------------------------------------------------------------
// <copyright file="ClearData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th075;

internal sealed class ClearData : IBinaryReadable, IClearData  // per character, level
{
    public ClearData()
    {
        this.MaxBonuses = new List<int>(100);
        this.CardGotCount = new List<short>(100);
        this.CardTrialCount = new List<short>(100);
        this.CardTrulyGot = new List<byte>(100);
        this.Ranking = new List<IHighScore>(10);
    }

    public int UseCount { get; private set; }

    public int ClearCount { get; private set; }

    public int MaxCombo { get; private set; }

    public int MaxDamage { get; private set; }

    public IReadOnlyList<int> MaxBonuses { get; private set; }

    public IReadOnlyList<short> CardGotCount { get; private set; }

    public IReadOnlyList<short> CardTrialCount { get; private set; }

    public IReadOnlyList<byte> CardTrulyGot { get; private set; }

    public IReadOnlyList<IHighScore> Ranking { get; private set; }

    public void ReadFrom(BinaryReader reader)
    {
        var numbers = Enumerable.Range(1, 100).ToArray();

        this.UseCount = reader.ReadInt32();
        this.ClearCount = reader.ReadInt32();
        this.MaxCombo = reader.ReadInt32();
        this.MaxDamage = reader.ReadInt32();
        this.MaxBonuses = numbers.Select(_ => reader.ReadInt32()).ToList();
        _ = reader.ReadExactBytes(0xC8);
        this.CardGotCount = numbers.Select(_ => reader.ReadInt16()).ToList();
        _ = reader.ReadExactBytes(0x64);
        this.CardTrialCount = numbers.Select(_ => reader.ReadInt16()).ToList();
        _ = reader.ReadExactBytes(0x64);
        this.CardTrulyGot = numbers.Select(_ => reader.ReadByte()).ToList();
        _ = reader.ReadExactBytes(0x32);
        _ = reader.ReadExactBytes(6);   // 07 00 00 00 00 00
        this.Ranking = Enumerable.Range(1, 10).Select(_ => BinaryReadableHelper.Create<HighScore>(reader)).ToList();
    }
}
