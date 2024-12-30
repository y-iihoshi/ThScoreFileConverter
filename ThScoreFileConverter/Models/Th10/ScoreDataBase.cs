//-----------------------------------------------------------------------
// <copyright file="ScoreDataBase.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.IO;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Models.Th10;

internal class ScoreDataBase<TStageProgress> : IBinaryReadable, IScoreData<TStageProgress>
    where TStageProgress : struct, Enum
{
    public uint Score { get; protected set; }   // Divided by 10

    public TStageProgress StageProgress { get; protected set; }

    public byte ContinueCount { get; protected set; }

    // The last 2 bytes are always 0x00 ?
    public IEnumerable<byte> Name { get; protected set; } = [];

    public uint DateTime { get; protected set; }    // UNIX time

    public float SlowRate { get; protected set; }   // Really...?

    public virtual void ReadFrom(BinaryReader reader)
    {
        this.Score = reader.ReadUInt32();
        this.StageProgress = EnumHelper.To<TStageProgress>(reader.ReadByte());
        this.ContinueCount = reader.ReadByte();
        this.Name = reader.ReadExactBytes(10);
        this.DateTime = reader.ReadUInt32();
        this.SlowRate = reader.ReadSingle();
    }
}
