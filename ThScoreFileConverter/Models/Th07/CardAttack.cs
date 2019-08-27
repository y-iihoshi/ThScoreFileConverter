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
    internal class CardAttack : Th06.Chapter    // per card
    {
        public const string ValidSignature = "CATK";
        public const short ValidSize = 0x0078;

        public CardAttack(Th06.Chapter chapter)
            : base(chapter, ValidSignature, ValidSize)
        {
            var charas = Utils.GetEnumerator<Th07Converter.CharaWithTotal>();
            var numCharas = charas.Count();
            this.MaxBonuses = new Dictionary<Th07Converter.CharaWithTotal, uint>(numCharas);
            this.TrialCounts = new Dictionary<Th07Converter.CharaWithTotal, ushort>(numCharas);
            this.ClearCounts = new Dictionary<Th07Converter.CharaWithTotal, ushort>(numCharas);

            using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
            {
                reader.ReadUInt32();    // always 0x00000001?
                foreach (var chara in charas)
                    this.MaxBonuses.Add(chara, reader.ReadUInt32());
                this.CardId = (short)(reader.ReadInt16() + 1);
                reader.ReadByte();
                this.CardName = reader.ReadExactBytes(0x30);
                reader.ReadByte();      // always 0x00?
                foreach (var chara in charas)
                    this.TrialCounts.Add(chara, reader.ReadUInt16());
                foreach (var chara in charas)
                    this.ClearCounts.Add(chara, reader.ReadUInt16());
            }
        }

        public Dictionary<Th07Converter.CharaWithTotal, uint> MaxBonuses { get; }

        public short CardId { get; }    // 1-based

        public byte[] CardName { get; }

        public Dictionary<Th07Converter.CharaWithTotal, ushort> TrialCounts { get; }

        public Dictionary<Th07Converter.CharaWithTotal, ushort> ClearCounts { get; }

        public bool HasTried() => this.TrialCounts[Th07Converter.CharaWithTotal.Total] > 0;
    }
}
