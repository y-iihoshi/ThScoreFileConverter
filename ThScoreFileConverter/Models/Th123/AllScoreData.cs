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
        private Dictionary<int, Th105.ICardForDeck> systemCards;
        private Dictionary<Chara, Th105.IClearData<Chara>> clearData;

        public AllScoreData()
        {
            var validNumCharas = Utils.GetEnumerator<Chara>().Where(chara => chara != Chara.Oonamazu).Count();
            this.storyClearCounts = new Dictionary<Chara, byte>(validNumCharas);
            this.systemCards = null;
            this.clearData = null;
        }

        public IReadOnlyDictionary<Chara, byte> StoryClearCounts => this.storyClearCounts;

        public IReadOnlyDictionary<int, Th105.ICardForDeck> SystemCards => this.systemCards;

        public IReadOnlyDictionary<Chara, Th105.IClearData<Chara>> ClearData => this.clearData;

        public void ReadFrom(BinaryReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            var validNumCharas = Utils.GetEnumerator<Chara>().Where(chara => chara != Chara.Oonamazu).Count();

            reader.ReadUInt32();            // version? (0xD2 == 210 --> ver.1.10?)
            reader.ReadUInt32();

            for (var index = 0; index < 0x14; index++)
            {
                var count = reader.ReadByte();
                if (index < validNumCharas)
                {
                    var chara = (Chara)index;
                    if (!this.storyClearCounts.ContainsKey(chara))
                        this.storyClearCounts.Add(chara, count);    // really...?
                }
            }

            reader.ReadExactBytes(0x14);    // flags of story playable characters?
            reader.ReadExactBytes(0x14);    // flags of versus/arcade playable characters?

            var numBgmFlags = reader.ReadInt32();
            for (var index = 0; index < numBgmFlags; index++)
                reader.ReadUInt32();        // signature of an unlocked bgm?

            var num = reader.ReadInt32();
            for (var index = 0; index < num; index++)
                reader.ReadUInt32();

            var numSystemCards = reader.ReadInt32();
            this.systemCards = new Dictionary<int, Th105.ICardForDeck>(numSystemCards);
            for (var index = 0; index < numSystemCards; index++)
            {
                var card = new Th105.CardForDeck();
                card.ReadFrom(reader);
                if (!this.systemCards.ContainsKey(card.Id))
                    this.systemCards.Add(card.Id, card);
            }

            this.clearData = new Dictionary<Chara, Th105.IClearData<Chara>>(validNumCharas);
            var numCharas = reader.ReadInt32();
            for (var index = 0; index < numCharas; index++)
            {
                var data = new Th105.ClearData<Chara>();
                data.ReadFrom(reader);
                if (index < validNumCharas)
                {
                    var chara = (Chara)index;
                    if (!this.clearData.ContainsKey(chara))
                        this.clearData.Add(chara, data);
                }
            }
        }
    }
}
