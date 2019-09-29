//-----------------------------------------------------------------------
// <copyright file="CardAttack.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ThScoreFileConverter.Models.Th07
{
    internal class CardAttack : Th06.Chapter, ICardAttack   // per card
    {
        public const string ValidSignature = "CATK";
        public const short ValidSize = 0x0078;

        public CardAttack(Th06.Chapter chapter)
            : base(chapter, ValidSignature, ValidSize)
        {
            var charas = Utils.GetEnumerator<CharaWithTotal>();
            var numCharas = charas.Count();
            var maxBonuses = new Dictionary<CharaWithTotal, uint>(numCharas);
            var trialCounts = new Dictionary<CharaWithTotal, ushort>(numCharas);
            var clearCounts = new Dictionary<CharaWithTotal, ushort>(numCharas);

            using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
            {
                reader.ReadUInt32();    // always 0x00000001?

                foreach (var chara in charas)
                    maxBonuses.Add(chara, reader.ReadUInt32());
                this.MaxBonuses = maxBonuses;

                this.CardId = (short)(reader.ReadInt16() + 1);
                reader.ReadByte();
                this.CardName = reader.ReadExactBytes(0x30);
                reader.ReadByte();      // always 0x00?

                foreach (var chara in charas)
                    trialCounts.Add(chara, reader.ReadUInt16());
                this.TrialCounts = trialCounts;

                foreach (var chara in charas)
                    clearCounts.Add(chara, reader.ReadUInt16());
                this.ClearCounts = clearCounts;
            }
        }

        public IReadOnlyDictionary<CharaWithTotal, uint> MaxBonuses { get; }

        public short CardId { get; }    // 1-based

        public IEnumerable<byte> CardName { get; }

        public IReadOnlyDictionary<CharaWithTotal, ushort> TrialCounts { get; }

        public IReadOnlyDictionary<CharaWithTotal, ushort> ClearCounts { get; }

        public bool HasTried() => this.TrialCounts[CharaWithTotal.Total] > 0;
    }
}
