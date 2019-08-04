using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models;
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
        public Th08ClearDataWrapper(object obj)
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
        // NOTE: Th08Converter.PlayableStages is a private enum and its underlying type is int.
        // public IReadOnlyDictionary<ThConverter.Level, Th08Converter.PlayableStages> StoryFlags
        //     => this.pobj.GetProperty(nameof(this.StoryFlags))
        //         as Dictionary<ThConverter.Level, Th08Converter.PlayableStages>;
        public object StoryFlags
            => this.pobj.GetProperty(nameof(this.StoryFlags));
        public int[] ValuesOfStoryFlags
            => ((IEnumerable)this.StoryFlags.GetType().GetProperty("Values").GetValue(this.StoryFlags))
                .Cast<int>().ToArray();
        // NOTE: Th08Converter.PlayableStages is a private enum and its underlying type is int.
        // public IReadOnlyDictionary<ThConverter.Level, Th08Converter.PlayableStages> PracticeFlags
        //     => this.pobj.GetProperty(nameof(this.PracticeFlags))
        //         as Dictionary<ThConverter.Level, Th08Converter.PlayableStages>;
        public object PracticeFlags
            => this.pobj.GetProperty(nameof(this.PracticeFlags));
        public int[] ValuesOfPracticeFlags
            => ((IEnumerable)this.PracticeFlags.GetType().GetProperty("Values").GetValue(this.PracticeFlags))
                .Cast<int>().ToArray();
        public Th08Converter.CharaWithTotal? Chara
            => this.pobj.GetProperty(nameof(this.Chara)) as Th08Converter.CharaWithTotal?;
    }
}
