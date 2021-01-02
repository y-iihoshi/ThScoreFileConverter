﻿//-----------------------------------------------------------------------
// <copyright file="ScoreData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;

namespace ThScoreFileConverter.Models.Th11
{
    internal class ScoreData : Th10.ScoreDataBase<Th10.StageProgress>
    {
        public override void ReadFrom(BinaryReader reader)
        {
            base.ReadFrom(reader);
            _ = reader.ReadUInt32();
        }
    }
}
