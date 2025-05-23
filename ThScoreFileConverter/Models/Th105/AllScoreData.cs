﻿//-----------------------------------------------------------------------
// <copyright file="AllScoreData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th105;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th105;

internal sealed class AllScoreData : IBinaryReadable
{
    private readonly Dictionary<Chara, byte> storyClearCounts;
    private readonly Dictionary<int, ICardForDeck> systemCards;
    private readonly Dictionary<Chara, IClearData<Chara>> clearData;

    public AllScoreData()
    {
        var numCharas = EnumHelper<Chara>.NumValues;
        this.storyClearCounts = new Dictionary<Chara, byte>(numCharas);
        this.systemCards = new Dictionary<int, ICardForDeck>(Definitions.SystemCardNameTable.Count);
        this.clearData = new Dictionary<Chara, IClearData<Chara>>(numCharas);
    }

    public IReadOnlyDictionary<Chara, byte> StoryClearCounts => this.storyClearCounts;

    public IReadOnlyDictionary<int, ICardForDeck> SystemCards => this.systemCards;

    public IReadOnlyDictionary<Chara, IClearData<Chara>> ClearData => this.clearData;

    public void ReadFrom(BinaryReader reader)
    {
        var validNumCharas = EnumHelper<Chara>.NumValues;

        _ = reader.ReadUInt32();            // version? (0x6A == 106 --> ver.1.06?)
        _ = reader.ReadUInt32();

        for (var index = 0; index < 0x14; index++)
        {
            var count = reader.ReadByte();
            if (index < validNumCharas)
                this.storyClearCounts.Add((Chara)index, count); // really...?
        }

        _ = reader.ReadExactBytes(0x14);    // flags of story playable characters?
        _ = reader.ReadExactBytes(0x14);    // flags of versus/arcade playable characters?

        var numBgmFlags = reader.ReadInt32();
        for (var index = 0; index < numBgmFlags; index++)
            _ = reader.ReadUInt32();        // signature of an unlocked bgm?

        var num = reader.ReadInt32();   // always 2?
        for (var index = 0; index < num; index++)
            _ = reader.ReadUInt32();        // always 0x0000000A and 0x0000000B?

        var numSystemCards = reader.ReadInt32();
        for (var index = 0; index < numSystemCards; index++)
        {
            var card = BinaryReadableHelper.Create<CardForDeck>(reader);
            _ = this.systemCards.TryAdd(card.Id, card);
        }

        var numCharas = reader.ReadInt32();
        for (var index = 0; index < numCharas; index++)
        {
            var data = BinaryReadableHelper.Create<ClearData<Chara>>(reader);
            if (index < validNumCharas)
                this.clearData.Add((Chara)index, data);
        }
    }
}
