//-----------------------------------------------------------------------
// <copyright file="SpellCardResult.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th105;

namespace ThScoreFileConverter.Models.Th105;

internal sealed class SpellCardResult<TChara> : IBinaryReadable, ISpellCardResult<TChara>
    where TChara : struct, Enum
{
    public SpellCardResult()
    {
    }

    public TChara Enemy { get; private set; }

    public Level Level { get; private set; }

    public int Id { get; private set; } // 0-based

    public int TrialCount { get; private set; }

    public int GotCount { get; private set; }

    public uint Frames { get; private set; }

    public void ReadFrom(BinaryReader reader)
    {
        this.Enemy = EnumHelper.To<TChara>(reader.ReadInt32());
        this.Level = EnumHelper.To<Level>(reader.ReadInt32());
        this.Id = reader.ReadInt32();
        this.TrialCount = reader.ReadInt32();
        this.GotCount = reader.ReadInt32();
        this.Frames = reader.ReadUInt32();
    }
}
