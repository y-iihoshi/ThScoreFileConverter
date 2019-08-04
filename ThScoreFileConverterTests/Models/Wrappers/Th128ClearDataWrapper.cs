using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Th10.Wrappers;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051 and CS0053.
    internal sealed class Th128ClearDataWrapper
    {
        private static readonly Type ParentType = typeof(Th128Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+ClearData";
        private static readonly PrivateType PrivateType = new PrivateType(AssemblyNameToTest, TypeNameToTest);

        private readonly PrivateObject pobj = null;

        public Th128ClearDataWrapper(ChapterWrapper chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });
        public Th128ClearDataWrapper(object obj)
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
        public Th128Converter.RouteWithTotal? Route
            => this.pobj.GetProperty(nameof(Route)) as Th128Converter.RouteWithTotal?;
        // NOTE: Th128Converter.ScoreData is a private class.
        // public IReadOnlyDictionary<Level, ScoreData[]> Rankings
        //     => this.pobj.GetProperty(nameof(Rankings)) as Dictionary<Level, ScoreData[]>;
        public object Rankings
            => this.pobj.GetProperty(nameof(Rankings));
        public object[] Ranking(ThConverter.Level level)
            => this.Rankings.GetType().GetProperty("Item").GetValue(this.Rankings, new object[] { level }) as object[];
        public Th10ScoreDataWrapper<Th128Converter, Th128Converter.StageProgress>
            RankingItem(ThConverter.Level level, int index)
            => new Th10ScoreDataWrapper<Th128Converter, Th128Converter.StageProgress>(this.Ranking(level)[index]);
        public int? TotalPlayCount
            => this.pobj.GetProperty(nameof(TotalPlayCount)) as int?;
        public int? PlayTime
            => this.pobj.GetProperty(nameof(PlayTime)) as int?;
        public IReadOnlyDictionary<ThConverter.Level, int> ClearCounts
            => this.pobj.GetProperty(nameof(ClearCounts)) as Dictionary<ThConverter.Level, int>;

        public static bool CanInitialize(ChapterWrapper chapter)
            => (bool)PrivateType.InvokeStatic(
                nameof(CanInitialize), new object[] { chapter.Target }, CultureInfo.InvariantCulture);
    }
}
