using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    // NOTE: Setting the accessibility as public causes CS0051.
    internal sealed class Th125ScoreWrapper
    {
        private static Type ParentType = typeof(Th125Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+Score";
        private static readonly PrivateType PrivateType = new PrivateType(AssemblyNameToTest, TypeNameToTest);

        private readonly PrivateObject pobj = null;

        public Th125ScoreWrapper(Th095ChapterWrapper<Th125Converter> chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public string Signature
            => this.pobj.GetProperty(nameof(this.Signature)) as string;
        public ushort? Version
            => this.pobj.GetProperty(nameof(this.Version)) as ushort?;
        public int? Size
            => this.pobj.GetProperty(nameof(this.Size)) as int?;
        public uint? Checksum
            => this.pobj.GetProperty(nameof(this.Checksum)) as uint?;
        public bool? IsValid
            => this.pobj.GetProperty(nameof(this.IsValid)) as bool?;
        public IReadOnlyCollection<byte> Data
            => this.pobj.GetProperty(nameof(this.Data)) as byte[];
        public Th095LevelScenePairWrapper<Th125Converter, Th125Converter.Level> LevelScene
            => new Th095LevelScenePairWrapper<Th125Converter, Th125Converter.Level>(
                this.pobj.GetProperty(nameof(this.LevelScene)));
        public int? HighScore
            => this.pobj.GetProperty(nameof(this.HighScore)) as int?;
        public Th125Converter.Chara? Chara
            => this.pobj.GetProperty(nameof(this.Chara)) as Th125Converter.Chara?;
        public int? TrialCount
            => this.pobj.GetProperty(nameof(this.TrialCount)) as int?;
        public int? FirstSuccess
            => this.pobj.GetProperty(nameof(this.FirstSuccess)) as int?;
        public uint? DateTime
            => this.pobj.GetProperty(nameof(this.DateTime)) as uint?;
        public int? BestshotScore
            => this.pobj.GetProperty(nameof(this.BestshotScore)) as int?;

        public static bool CanInitialize(Th095ChapterWrapper<Th125Converter> chapter)
            => (bool)PrivateType.InvokeStatic(
                nameof(CanInitialize), new object[] { chapter.Target }, CultureInfo.InvariantCulture);
    }
}
