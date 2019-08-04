using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Th06.Wrappers;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th07ClearDataWrapper
    {
        private static readonly Type ParentType = typeof(Th07Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+ClearData";

        private readonly PrivateObject pobj = null;

        public Th07ClearDataWrapper(ChapterWrapper chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });
        public Th07ClearDataWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

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
        public IReadOnlyDictionary<Th07Converter.Level, byte> StoryFlags
            => this.pobj.GetProperty(nameof(this.StoryFlags)) as Dictionary<Th07Converter.Level, byte>;
        public IReadOnlyDictionary<Th07Converter.Level, byte> PracticeFlags
            => this.pobj.GetProperty(nameof(this.PracticeFlags)) as Dictionary<Th07Converter.Level, byte>;
        public Th07Converter.Chara? Chara
            => this.pobj.GetProperty(nameof(this.Chara)) as Th07Converter.Chara?;
    }
}
