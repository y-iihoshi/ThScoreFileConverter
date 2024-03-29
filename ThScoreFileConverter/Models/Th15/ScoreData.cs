﻿//-----------------------------------------------------------------------
// <copyright file="ScoreData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;

namespace ThScoreFileConverter.Models.Th15;

internal sealed class ScoreData : Th10.ScoreDataBase<Th13.StageProgress>, IScoreData
{
    public uint RetryCount { get; private set; }

    public override void ReadFrom(BinaryReader reader)
    {
        base.ReadFrom(reader);
        this.SlowRate = reader.ReadSingle();
        this.RetryCount = reader.ReadUInt32();
    }
}
