//-----------------------------------------------------------------------
// <copyright file="ScoreData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;

namespace ThScoreFileConverter.Models.Th17
{
    internal class ScoreData : Th10.ScoreDataBase<Th13.StageProgress>
    {
        public override void ReadFrom(BinaryReader reader)
        {
            base.ReadFrom(reader);
            this.SlowRate = reader.ReadSingle();
            _ = reader.ReadUInt32();
        }
    }
}
