using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverterTests.Models.Th10.Wrappers;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051, CS0053 and CS0703.
    internal sealed class Th10ClearDataWrapper<TParent, TCharaWithTotal, TStageProgress>
        where TParent : ThConverter
        where TCharaWithTotal : struct, Enum
        where TStageProgress : struct, Enum
    {
        private static readonly Type ParentType = typeof(TParent);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+ClearData";
        private static readonly PrivateType PrivateType = new PrivateType(AssemblyNameToTest, TypeNameToTest);

        private readonly PrivateObject pobj = null;

        public Th10ClearDataWrapper(ChapterWrapper chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });
        public Th10ClearDataWrapper(object original)
            => this.pobj = new PrivateObject(original);

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
        public TCharaWithTotal? Chara
            => this.pobj.GetProperty(nameof(this.Chara)) as TCharaWithTotal?;
        // NOTE: Th10Converter.ScoreData is a private class.
        // public IReadOnlyDictionary<Level, ScoreData[]> Rankings
        //     => this.pobj.GetProperty(nameof(this.Rankings)) as Dictionary<Level, ScoreData[]>;
        public object Rankings
            => this.pobj.GetProperty(nameof(this.Rankings));
        public object[] Ranking(Level level)
            => this.Rankings.GetType().GetProperty("Item").GetValue(this.Rankings, new object[] { level }) as object[];
        public ScoreDataWrapper<TParent, TStageProgress> RankingItem(Level level, int index)
            => new ScoreDataWrapper<TParent, TStageProgress>(this.Ranking(level)[index]);
        public int? TotalPlayCount
            => this.pobj.GetProperty(nameof(this.TotalPlayCount)) as int?;
        public int? PlayTime
            => this.pobj.GetProperty(nameof(this.PlayTime)) as int?;
        public IReadOnlyDictionary<Level, int> ClearCounts
            => this.pobj.GetProperty(nameof(this.ClearCounts)) as Dictionary<Level, int>;
        public IReadOnlyDictionary<(Level, ThConverter.Stage), Practice> Practices
            => this.pobj.GetProperty(nameof(this.Practices)) as Dictionary<(Level, ThConverter.Stage), Practice>;
        public IReadOnlyDictionary<int, SpellCard> Cards
            => this.pobj.GetProperty(nameof(this.Cards)) as Dictionary<int, SpellCard>;

        public static bool CanInitialize(ChapterWrapper chapter)
            => (bool)PrivateType.InvokeStatic(
                nameof(CanInitialize), new object[] { chapter.Target }, CultureInfo.InvariantCulture);
    }
}
