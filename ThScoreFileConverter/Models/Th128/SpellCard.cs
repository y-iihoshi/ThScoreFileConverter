//-----------------------------------------------------------------------
// <copyright file="SpellCard.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.IO;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Models.Th128;

internal sealed class SpellCard : IBinaryReadable, ISpellCard
{
    public IEnumerable<byte> Name { get; private set; } = ReadOnlyCP932Bytes.Empty;

    public int NoMissCount { get; private set; }

    public int NoIceCount { get; private set; }

    public int TrialCount { get; private set; }

    public int Id { get; private set; } // 1-based

    public Level Level { get; private set; }

    public bool HasTried => this.TrialCount > 0;

    public void ReadFrom(BinaryReader reader)
    {
        this.Name = new ReadOnlyCP932Bytes(reader.ReadExactBytes(0x80));
        this.NoMissCount = reader.ReadInt32();
        this.NoIceCount = reader.ReadInt32();
        _ = reader.ReadUInt32();
        this.TrialCount = reader.ReadInt32();
        this.Id = reader.ReadInt32() + 1;
        this.Level = EnumHelper.To<Level>(reader.ReadInt32());
    }
}
