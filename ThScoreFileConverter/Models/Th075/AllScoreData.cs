﻿//-----------------------------------------------------------------------
// <copyright file="AllScoreData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th075;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th075;

internal sealed class AllScoreData : IBinaryReadable
{
    public AllScoreData()
    {
        var numCharas = EnumHelper<CharaWithReserved>.NumValues;
        var numLevels = EnumHelper<Level>.NumValues;
        this.ClearData = new Dictionary<(CharaWithReserved, Level), IClearData>(numCharas * numLevels);
    }

    public IReadOnlyDictionary<(CharaWithReserved Chara, Level Level), IClearData> ClearData { get; private set; }

    public Status? Status { get; private set; }

    public void ReadFrom(BinaryReader reader)
    {
        this.ClearData = EnumHelper.Cartesian<CharaWithReserved, Level>()
            .ToDictionary(pair => pair, _ => BinaryReadableHelper.Create<ClearData>(reader) as IClearData);

        this.Status = BinaryReadableHelper.Create<Status>(reader);
    }
}
