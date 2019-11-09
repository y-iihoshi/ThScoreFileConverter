using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th13;
using ThScoreFileConverter.Models.Th16;
using ThScoreFileConverterTests.Models.Th10.Wrappers;

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
        public Th16ClearDataWrapper(object original)
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
        public Th16Converter.CharaWithTotal? Chara
            => this.pobj.GetProperty(nameof(this.Chara)) as Th16Converter.CharaWithTotal?;
        public IReadOnlyDictionary<LevelWithTotal, IReadOnlyList<IScoreData>> Rankings
            => this.pobj.GetProperty(nameof(this.Rankings))
                as IReadOnlyDictionary<LevelWithTotal, IReadOnlyList<IScoreData>>;
        public int? TotalPlayCount
            => this.pobj.GetProperty(nameof(this.TotalPlayCount)) as int?;
        public int? PlayTime
            => this.pobj.GetProperty(nameof(this.PlayTime)) as int?;
        public IReadOnlyDictionary<LevelWithTotal, int> ClearCounts
            => this.pobj.GetProperty(nameof(this.ClearCounts)) as IReadOnlyDictionary<LevelWithTotal, int>;
        public IReadOnlyDictionary<LevelWithTotal, int> ClearFlags
            => this.pobj.GetProperty(nameof(this.ClearFlags)) as IReadOnlyDictionary<LevelWithTotal, int>;
        public IReadOnlyDictionary<(Level, Th16Converter.StagePractice), IPractice> Practices
            => this.pobj.GetProperty(nameof(this.Practices))
                as IReadOnlyDictionary<(Level, Th16Converter.StagePractice), IPractice>;
        public IReadOnlyDictionary<int, ISpellCard<Level>> Cards
            => this.pobj.GetProperty(nameof(this.Cards)) as IReadOnlyDictionary<int, ISpellCard<Level>>;

        public static bool CanInitialize(ChapterWrapper chapter)
            => (bool)PrivateType.InvokeStatic(
                nameof(CanInitialize), new object[] { chapter.Target }, CultureInfo.InvariantCulture);
    }
}
