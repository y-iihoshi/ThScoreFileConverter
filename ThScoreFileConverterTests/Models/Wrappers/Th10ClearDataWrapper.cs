using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051, CS0053 and CS0703.
    internal sealed class Th10ClearDataWrapper<TParent, TCharaWithTotal, TStageProgress>
        where TParent : ThConverter
        where TCharaWithTotal : struct, Enum
        where TStageProgress : struct, Enum
    {
        private static Type ParentType = typeof(TParent);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+ClearData";
        private static readonly PrivateType PrivateType = new PrivateType(AssemblyNameToTest, TypeNameToTest);

        private readonly PrivateObject pobj = null;

        public Th10ClearDataWrapper(Th10ChapterWrapper chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });
        public Th10ClearDataWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

        public object Target
            => this.pobj.Target;
        public string Signature
            => this.pobj.GetProperty(nameof(Signature)) as string;
        public ushort? Version
            => this.pobj.GetProperty(nameof(Version)) as ushort?;
        public uint? Checksum
            => this.pobj.GetProperty(nameof(Checksum)) as uint?;
        public int? Size
            => this.pobj.GetProperty(nameof(Size)) as int?;
        public bool? IsValid
            => this.pobj.GetProperty(nameof(IsValid)) as bool?;
        public IReadOnlyCollection<byte> Data
            => this.pobj.GetProperty(nameof(Data)) as byte[];
        public TCharaWithTotal? Chara
            => this.pobj.GetProperty(nameof(Chara)) as TCharaWithTotal?;
        // NOTE: Th10Converter.ScoreData is a private class.
        // public IReadOnlyDictionary<Level, ScoreData[]> Rankings
        //     => this.pobj.GetProperty(nameof(Rankings)) as Dictionary<Level, ScoreData[]>;
        public object Rankings
            => this.pobj.GetProperty(nameof(Rankings));
        public object[] Ranking(ThConverter.Level level)
            => this.Rankings.GetType().GetProperty("Item").GetValue(this.Rankings, new object[] { level }) as object[];
        public Th10ScoreDataWrapper<TParent, TStageProgress> RankingItem(ThConverter.Level level, int index)
            => new Th10ScoreDataWrapper<TParent, TStageProgress>(this.Ranking(level)[index]);
        public int? TotalPlayCount
            => this.pobj.GetProperty(nameof(TotalPlayCount)) as int?;
        public int? PlayTime
            => this.pobj.GetProperty(nameof(PlayTime)) as int?;
        public IReadOnlyDictionary<ThConverter.Level, int> ClearCounts
            => this.pobj.GetProperty(nameof(ClearCounts)) as Dictionary<ThConverter.Level, int>;
        // NOTE: Th10Converter.{LevelStagePair,Practice} are private classes.
        // public IReadOnlyDictionary<LevelStagePair, Practice> Practices
        //     => this.pobj.GetProperty(nameof(Practices)) as Dictionary<LevelStagePair, Practice>;
        public object Practices
            => this.pobj.GetProperty(nameof(Practices));
        public Th10PracticeWrapper<TParent> PracticesItem(
            Th10LevelStagePairWrapper<TParent, ThConverter.Level, ThConverter.Stage> levelStagePair)
            => new Th10PracticeWrapper<TParent>(
                this.Practices.GetType().GetProperty("Item").GetValue(
                    this.Practices, new object[] { levelStagePair.Target }));
        // NOTE: Th10Converter.SpellCard is a private class.
        // public IReadOnlyDictionary<int, SpellCard> Cards
        //     => this.pobj.GetProperty(nameof(Cards)) as Dictionary<int, SpellCard>;
        public object Cards
            => this.pobj.GetProperty(nameof(Cards));
        public Th10SpellCardWrapper<TParent> CardsItem(int id)
            => new Th10SpellCardWrapper<TParent>(
                this.Cards.GetType().GetProperty("Item").GetValue(this.Cards, new object[] { id }));

        public static bool CanInitialize(Th10ChapterWrapper chapter)
            => (bool)PrivateType.InvokeStatic(
                nameof(CanInitialize), new object[] { chapter.Target }, CultureInfo.InvariantCulture);
    }
}
