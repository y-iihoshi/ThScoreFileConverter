using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Th06.Wrappers;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th06HighScoreWrapper
    {
        private static readonly Type ParentType = typeof(Th06Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+HighScore";

        private readonly PrivateObject pobj = null;

        public Th06HighScoreWrapper(ChapterWrapper chapter)
            => this.pobj = new PrivateObject(
                AssemblyNameToTest,
                TypeNameToTest,
                new Type[] { (chapter ?? new ChapterWrapper()).Target.GetType() },
                new object[] { chapter?.Target });
        public Th06HighScoreWrapper(uint score)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { score });
        public Th06HighScoreWrapper(object original)
            => this.pobj = new PrivateObject(original);

        public object Target
            => this.pobj.Target;
        public string Signature
            => this.pobj.GetProperty(nameof(this.Signature)) as string;
        public short? Size1
            => this.pobj.GetProperty(nameof(this.Size1)) as short?;
        public short? Size2
            => this.pobj.GetProperty(nameof(this.Size2)) as short?;
        public byte? FirstByteOfData
            => this.pobj.GetProperty(nameof(this.FirstByteOfData)) as byte?;
        public IReadOnlyCollection<byte> Data
            => this.pobj.GetProperty(nameof(this.Data)) as byte[];
        public uint? Score
            => this.pobj.GetProperty(nameof(this.Score)) as uint?;
        public Th06Converter.Chara? Chara
            => this.pobj.GetProperty(nameof(this.Chara)) as Th06Converter.Chara?;
        public ThConverter.Level? Level
            => this.pobj.GetProperty(nameof(this.Level)) as ThConverter.Level?;
        public Th06Converter.StageProgress? StageProgress
            => this.pobj.GetProperty(nameof(this.StageProgress)) as Th06Converter.StageProgress?;
        public IReadOnlyCollection<byte> Name
            => this.pobj.GetProperty(nameof(this.Name)) as byte[];
    }
}
