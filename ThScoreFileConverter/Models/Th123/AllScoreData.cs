//-----------------------------------------------------------------------
// <copyright file="AllScoreData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Models.Th123
{
    internal class AllScoreData : IBinaryReadable
    {
        private readonly Dictionary<Chara, byte> storyClearCounts;
        private readonly Dictionary<int, Th105.ICardForDeck> systemCards;
        private readonly Dictionary<Chara, Th105.IClearData<Chara>> clearData;

        public AllScoreData()
        {
            var validNumCharas = Utils.GetEnumerable<Chara>().Where(chara => chara != Chara.Oonamazu).Count();
            this.storyClearCounts = new Dictionary<Chara, byte>(validNumCharas);
            this.systemCards = new Dictionary<int, Th105.ICardForDeck>(Definitions.SystemCardNameTable.Count);
            this.clearData = new Dictionary<Chara, Th105.IClearData<Chara>>(validNumCharas);
        }

        public IReadOnlyDictionary<Chara, byte> StoryClearCounts => this.storyClearCounts;

        public IReadOnlyDictionary<int, Th105.ICardForDeck> SystemCards => this.systemCards;

        public IReadOnlyDictionary<Chara, Th105.IClearData<Chara>> ClearData => this.clearData;

        public void ReadFrom(BinaryReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            var validNumCharas = Utils.GetEnumerable<Chara>().Where(chara => chara != Chara.Oonamazu).Count();

            _ = reader.ReadUInt32();            // version? (0xD2 == 210 --> ver.1.10?)
            _ = reader.ReadUInt32();

            for (var index = 0; index < validNumCharas; index++)
            {
                var count = reader.ReadByte();
                this.storyClearCounts.Add((Chara)index, count); // really...?
            }

            _ = reader.ReadExactBytes(validNumCharas);  // flags of story playable characters?
            _ = reader.ReadExactBytes(validNumCharas);  // flags of versus/arcade playable characters?

            var numBgmFlags = reader.ReadInt32();
            for (var index = 0; index < numBgmFlags; index++)
                _ = reader.ReadUInt32();        // signature of an unlocked bgm?

            var num = reader.ReadInt32();
            for (var index = 0; index < num; index++)
                _ = reader.ReadUInt32();

            var numSystemCards = reader.ReadInt32();
            for (var index = 0; index < numSystemCards; index++)
            {
                var card = new Th105.CardForDeck();
                card.ReadFrom(reader);
                if (!this.systemCards.ContainsKey(card.Id))
                    this.systemCards.Add(card.Id, card);
            }

            var numCharas = reader.ReadInt32();
            for (var index = 0; index < numCharas; index++)
            {
                var data = new Th105.ClearData<Chara>();
                data.ReadFrom(reader);
                if (index < validNumCharas)
                    this.clearData.Add((Chara)index, data);
            }
        }
    }
}
