﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th13;
using ThScoreFileConverterTests.Models.Th10.Wrappers;
using ThScoreFileConverterTests.Models.Th13.Wrappers;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051 and CS0053.
    internal sealed class Th16ClearDataWrapper
    {
        private static readonly Type ParentType = typeof(Th16Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+ClearData";
        private static readonly PrivateType PrivateType = new PrivateType(AssemblyNameToTest, TypeNameToTest);

        private readonly PrivateObject pobj = null;

        public Th16ClearDataWrapper(ChapterWrapper chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });
        public Th16ClearDataWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

        public object Target
            => this.pobj.Target;
        public string Signature
            => this.pobj.GetProperty(nameof(this.Signature)) as string;
        public ushort? Version
            => this.pobj.GetProperty(nameof(this.Version)) as ushort?;
        public uint? Checksum
            => this.pobj.GetProperty(nameof(this.Checksum)) as uint?;
        public int? Size
            => this.pobj.GetProperty(nameof(this.Size)) as int?;
        public bool? IsValid
            => this.pobj.GetProperty(nameof(this.IsValid)) as bool?;
        public IReadOnlyCollection<byte> Data
            => this.pobj.GetProperty(nameof(this.Data)) as byte[];
        public Th16Converter.CharaWithTotal? Chara
            => this.pobj.GetProperty(nameof(this.Chara)) as Th16Converter.CharaWithTotal?;
        // NOTE: Th16Converter.ScoreData is a private class.
        // public IReadOnlyDictionary<LevelWithTotal, ScoreData[]> Rankings
        //     => this.pobj.GetProperty(nameof(this.Rankings)) as Dictionary<LevelWithTotal, ScoreData[]>;
        public object Rankings
            => this.pobj.GetProperty(nameof(this.Rankings));
        public object[] Ranking(ThConverter.LevelWithTotal level)
            => this.Rankings.GetType().GetProperty("Item").GetValue(this.Rankings, new object[] { level }) as object[];
        public Th16ScoreDataWrapper RankingItem(ThConverter.LevelWithTotal level, int index)
            => new Th16ScoreDataWrapper(this.Ranking(level)[index]);
        public int? TotalPlayCount
            => this.pobj.GetProperty(nameof(this.TotalPlayCount)) as int?;
        public int? PlayTime
            => this.pobj.GetProperty(nameof(this.PlayTime)) as int?;
        public IReadOnlyDictionary<ThConverter.LevelWithTotal, int> ClearCounts
            => this.pobj.GetProperty(nameof(this.ClearCounts)) as Dictionary<ThConverter.LevelWithTotal, int>;
        public IReadOnlyDictionary<ThConverter.LevelWithTotal, int> ClearFlags
            => this.pobj.GetProperty(nameof(this.ClearFlags)) as Dictionary<ThConverter.LevelWithTotal, int>;
        public IReadOnlyDictionary<(ThConverter.Level, Th16Converter.StagePractice), Practice> Practices
            => this.pobj.GetProperty(nameof(this.Practices))
                as Dictionary<(ThConverter.Level, Th16Converter.StagePractice), Practice>;
        // NOTE: Th16Converter.SpellCard is a private class.
        // public IReadOnlyDictionary<int, SpellCard> Cards
        //     => this.pobj.GetProperty(nameof(this.Cards)) as Dictionary<int, SpellCard>;
        public object Cards
            => this.pobj.GetProperty(nameof(this.Cards));
        public SpellCardWrapper<Th16Converter, ThConverter.Level> CardsItem(int id)
            => new SpellCardWrapper<Th16Converter, ThConverter.Level>(
                this.Cards.GetType().GetProperty("Item").GetValue(this.Cards, new object[] { id }));

        public static bool CanInitialize(ChapterWrapper chapter)
            => (bool)PrivateType.InvokeStatic(
                nameof(CanInitialize), new object[] { chapter.Target }, CultureInfo.InvariantCulture);
    }
}
