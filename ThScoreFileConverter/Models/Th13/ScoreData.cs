//-----------------------------------------------------------------------
// <copyright file="ScoreData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.IO;

namespace ThScoreFileConverter.Models.Th13
{
    internal class ScoreData : Th10.ScoreDataBase<StageProgress>
    {
        public override void ReadFrom(BinaryReader reader)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            base.ReadFrom(reader);
            _ = reader.ReadUInt32();
        }
    }
}
