using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th06HighScoreWrapper
    {
        private static Type ParentType = typeof(Th06Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+HighScore";

        private readonly PrivateObject pobj = null;

        public Th06HighScoreWrapper(Th06ChapterWrapper<Th06Converter> chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });
        public Th06HighScoreWrapper(uint score)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { score });

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public string Signature
            => this.pobj.GetProperty(nameof(Signature)) as string;
        public short? Size1
            => this.pobj.GetProperty(nameof(Size1)) as short?;
        public short? Size2
            => this.pobj.GetProperty(nameof(Size2)) as short?;
        public byte? FirstByteOfData
            => this.pobj.GetProperty(nameof(FirstByteOfData)) as byte?;
        public IReadOnlyCollection<byte> Data
            => this.pobj.GetProperty(nameof(Data)) as byte[];
        public uint? Score
            => this.pobj.GetProperty(nameof(Score)) as uint?;
        public Th06Converter.Chara? Chara
            => this.pobj.GetProperty(nameof(Chara)) as Th06Converter.Chara?;
        public ThConverter.Level? Level
            => this.pobj.GetProperty(nameof(Level)) as ThConverter.Level?;
        public Th06Converter.StageProgress? StageProgress
            => this.pobj.GetProperty(nameof(StageProgress)) as Th06Converter.StageProgress?;
        public IReadOnlyCollection<byte> Name
            => this.pobj.GetProperty(nameof(Name)) as byte[];
    }
}
