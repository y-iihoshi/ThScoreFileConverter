using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th13;
using ThScoreFileConverterTests.Models.Th10.Wrappers;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051 and CS0703.
    internal sealed class Th13ClearDataWrapper<
        TParent, TCharaWithTotal, TLevel, TLevelPractice, TLevelPracticeWithTotal, TStagePractice, TStageProgress>
        where TParent : ThConverter
        where TCharaWithTotal : struct, Enum
        where TLevel : struct, Enum
        where TLevelPractice : struct, Enum
        where TLevelPracticeWithTotal : struct, Enum
        where TStagePractice : struct, Enum
        where TStageProgress : struct, Enum
    {
        private static readonly Type ParentType = typeof(TParent);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+ClearData";
        private static readonly PrivateType PrivateType = new PrivateType(AssemblyNameToTest, TypeNameToTest);

        private readonly PrivateObject pobj = null;

        public Th13ClearDataWrapper(ChapterWrapper chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });
        public Th13ClearDataWrapper(object original)
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
        public IReadOnlyDictionary<
            TLevelPracticeWithTotal,
            IReadOnlyList<ThScoreFileConverter.Models.Th10.IScoreData<TStageProgress>>> Rankings
            => this.pobj.GetProperty(nameof(this.Rankings))
                as IReadOnlyDictionary<
                    TLevelPracticeWithTotal,
                    IReadOnlyList<ThScoreFileConverter.Models.Th10.IScoreData<TStageProgress>>>;
        public int? TotalPlayCount
            => this.pobj.GetProperty(nameof(this.TotalPlayCount)) as int?;
        public int? PlayTime
            => this.pobj.GetProperty(nameof(this.PlayTime)) as int?;
        public IReadOnlyDictionary<TLevelPracticeWithTotal, int> ClearCounts
            => this.pobj.GetProperty(nameof(this.ClearCounts)) as IReadOnlyDictionary<TLevelPracticeWithTotal, int>;
        public IReadOnlyDictionary<TLevelPracticeWithTotal, int> ClearFlags
            => this.pobj.GetProperty(nameof(this.ClearFlags)) as IReadOnlyDictionary<TLevelPracticeWithTotal, int>;
        public IReadOnlyDictionary<(TLevelPractice, TStagePractice), IPractice> Practices
            => this.pobj.GetProperty(nameof(this.Practices))
                as IReadOnlyDictionary<(TLevelPractice, TStagePractice), IPractice>;
        public IReadOnlyDictionary<int, ISpellCard<TLevel>> Cards
            => this.pobj.GetProperty(nameof(this.Cards)) as IReadOnlyDictionary<int, ISpellCard<TLevel>>;

        public static bool CanInitialize(ChapterWrapper chapter)
            => (bool)PrivateType.InvokeStatic(
                nameof(CanInitialize), new object[] { chapter.Target }, CultureInfo.InvariantCulture);
    }
}
