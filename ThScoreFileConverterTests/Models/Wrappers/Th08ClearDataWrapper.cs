using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverterTests.Models.Th06.Wrappers;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th08ClearDataWrapper
    {
        private static readonly Type ParentType = typeof(Th08Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+ClearData";

        private readonly PrivateObject pobj = null;

        public Th08ClearDataWrapper(ChapterWrapper chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });
        public Th08ClearDataWrapper(object original)
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
        public IReadOnlyDictionary<Level, PlayableStages> StoryFlags
            => this.pobj.GetProperty(nameof(this.StoryFlags)) as IReadOnlyDictionary<Level, PlayableStages>;
        public IReadOnlyDictionary<Level, PlayableStages> PracticeFlags
            => this.pobj.GetProperty(nameof(this.PracticeFlags)) as IReadOnlyDictionary<Level, PlayableStages>;
        public CharaWithTotal? Chara
            => this.pobj.GetProperty(nameof(this.Chara)) as CharaWithTotal?;
    }
}
