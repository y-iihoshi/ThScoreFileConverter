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
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Models.Th105
{
    internal class AllScoreData : IBinaryReadable
    {
        private readonly Dictionary<Chara, byte> storyClearCounts;
        private Dictionary<int, ICardForDeck> systemCards;
        private Dictionary<Chara, IClearData<Chara, Level>> clearData;

        public AllScoreData()
        {
            this.storyClearCounts = new Dictionary<Chara, byte>(Enum.GetValues(typeof(Chara)).Length);
            this.systemCards = null;
            this.clearData = null;
        }

        public IReadOnlyDictionary<Chara, byte> StoryClearCounts => this.storyClearCounts;

        public IReadOnlyDictionary<int, ICardForDeck> SystemCards => this.systemCards;

        public IReadOnlyDictionary<Chara, IClearData<Chara, Level>> ClearData => this.clearData;

        public void ReadFrom(BinaryReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            var validNumCharas = Enum.GetValues(typeof(Chara)).Length;

            reader.ReadUInt32();            // version? (0x6A == 106 --> ver.1.06?)
            reader.ReadUInt32();

            for (var index = 0; index < 0x14; index++)
            {
                var count = reader.ReadByte();
                if (index < validNumCharas)
                {
                    this.storyClearCounts.Add((Chara)index, count); // really...?
                }
            }

            reader.ReadExactBytes(0x14);    // flags of story playable characters?
            reader.ReadExactBytes(0x14);    // flags of versus/arcade playable characters?

            var numBgmFlags = reader.ReadInt32();
            for (var index = 0; index < numBgmFlags; index++)
                reader.ReadUInt32();        // signature of an unlocked bgm?

            var num = reader.ReadInt32();   // always 2?
            for (var index = 0; index < num; index++)
                reader.ReadUInt32();        // always 0x0000000A and 0x0000000B?

            var numSystemCards = reader.ReadInt32();
            this.systemCards = new Dictionary<int, ICardForDeck>(numSystemCards);
            for (var index = 0; index < numSystemCards; index++)
            {
                var card = new CardForDeck();
                card.ReadFrom(reader);
                if (!this.systemCards.ContainsKey(card.Id))
                    this.systemCards.Add(card.Id, card);
            }

            this.clearData = new Dictionary<Chara, IClearData<Chara, Level>>(validNumCharas);
            var numCharas = reader.ReadInt32();
            for (var index = 0; index < numCharas; index++)
            {
                var data = new ClearData<Chara, Level>();
                data.ReadFrom(reader);
                if (index < validNumCharas)
                {
                    this.clearData.Add((Chara)index, data);
                }
            }
        }
    }
}
