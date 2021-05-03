//-----------------------------------------------------------------------
// <copyright file="CardAttack.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.IO;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th08
{
    internal class CardAttack : Th06.Chapter, ICardAttack   // per card
    {
        public const string ValidSignature = "CATK";
        public const short ValidSize = 0x022C;

        private readonly CardAttackCareer storyCareer;
        private readonly CardAttackCareer practiceCareer;

        public CardAttack(Th06.Chapter chapter)
            : base(chapter, ValidSignature, ValidSize)
        {
            using var stream = new MemoryStream(this.Data, false);
            using var reader = new BinaryReader(stream);

            _ = reader.ReadUInt32();    // always 0x00000003?
            this.CardId = (short)(reader.ReadInt16() + 1);
            _ = reader.ReadByte();
            this.Level = EnumHelper.To<LevelPracticeWithTotal>(reader.ReadByte());  // Last Word == Normal...
            this.CardName = reader.ReadExactBytes(0x30);
            this.EnemyName = reader.ReadExactBytes(0x30);
            this.Comment = reader.ReadExactBytes(0x80);
            this.storyCareer = BinaryReadableHelper.Create<CardAttackCareer>(reader);
            this.practiceCareer = BinaryReadableHelper.Create<CardAttackCareer>(reader);
            _ = reader.ReadUInt32();    // always 0x00000000?
        }

        public short CardId { get; }    // 1-based

        public LevelPracticeWithTotal Level { get; }

        public IEnumerable<byte> CardName { get; }

        public IEnumerable<byte> EnemyName { get; }

        public IEnumerable<byte> Comment { get; }  // Should be splitted by '\0'

        public ICardAttackCareer StoryCareer => this.storyCareer;

        public ICardAttackCareer PracticeCareer => this.practiceCareer;

        public bool HasTried => (this.StoryCareer.TrialCounts[CharaWithTotal.Total] > 0)
            || (this.PracticeCareer.TrialCounts[CharaWithTotal.Total] > 0);
    }
}
