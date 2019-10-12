using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Th06.Wrappers;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th08PracticeScoreWrapper
    {
        private static readonly Type ParentType = typeof(Th08Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+PracticeScore";

        private readonly PrivateObject pobj = null;

        public Th08PracticeScoreWrapper(ChapterWrapper chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });
        public Th08PracticeScoreWrapper(object original)
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
        public IReadOnlyDictionary<(Th08Converter.Stage, Level), int> PlayCounts
            => this.pobj.GetProperty(nameof(this.PlayCounts)) as IReadOnlyDictionary<(Th08Converter.Stage, Level), int>;
        public IReadOnlyDictionary<(Th08Converter.Stage, Level), int> HighScores
            => this.pobj.GetProperty(nameof(this.HighScores)) as IReadOnlyDictionary<(Th08Converter.Stage, Level), int>;
        public Th08Converter.Chara? Chara
            => this.pobj.GetProperty(nameof(this.Chara)) as Th08Converter.Chara?;
    }
}
