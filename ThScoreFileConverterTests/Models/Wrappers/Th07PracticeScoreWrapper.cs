using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th07PracticeScoreWrapper
    {
        private static Type ParentType = typeof(Th07Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+PracticeScore";

        private readonly PrivateObject pobj = null;

        public Th07PracticeScoreWrapper(Th06ChapterWrapper chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });
        public Th07PracticeScoreWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

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
        public int? TrialCount
            => this.pobj.GetProperty(nameof(TrialCount)) as int?;
        public int? HighScore
            => this.pobj.GetProperty(nameof(HighScore)) as int?;
        public Th07Converter.Chara? Chara
            => this.pobj.GetProperty(nameof(Chara)) as Th07Converter.Chara?;
        public Th07Converter.Level? Level
            => this.pobj.GetProperty(nameof(Level)) as Th07Converter.Level?;
        public Th07Converter.Stage? Stage
            => this.pobj.GetProperty(nameof(Stage)) as Th07Converter.Stage?;
    }
}
