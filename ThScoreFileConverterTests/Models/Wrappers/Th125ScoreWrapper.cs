using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th125;
using ThScoreFileConverterTests.Models.Th095.Wrappers;
using Level = ThScoreFileConverter.Models.Th125.Level;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th125ScoreWrapper
    {
        private static readonly Type ParentType = typeof(Th125Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+Score";
        private static readonly PrivateType PrivateType = new PrivateType(AssemblyNameToTest, TypeNameToTest);

        private readonly PrivateObject pobj = null;

        public Th125ScoreWrapper(ChapterWrapper chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });
        public Th125ScoreWrapper(object original)
            => this.pobj = new PrivateObject(original);

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
        public (Level Level, int Scene)? LevelScene
            => this.pobj.GetProperty(nameof(this.LevelScene)) as (Level, int)?;
        public int? HighScore
            => this.pobj.GetProperty(nameof(this.HighScore)) as int?;
        public Chara? Chara
            => this.pobj.GetProperty(nameof(this.Chara)) as Chara?;
        public int? TrialCount
            => this.pobj.GetProperty(nameof(this.TrialCount)) as int?;
        public int? FirstSuccess
            => this.pobj.GetProperty(nameof(this.FirstSuccess)) as int?;
        public uint? DateTime
            => this.pobj.GetProperty(nameof(this.DateTime)) as uint?;
        public int? BestshotScore
            => this.pobj.GetProperty(nameof(this.BestshotScore)) as int?;

        public static bool CanInitialize(ChapterWrapper chapter)
            => (bool)PrivateType.InvokeStatic(
                nameof(CanInitialize), new object[] { chapter.Target }, CultureInfo.InvariantCulture);
    }
}
