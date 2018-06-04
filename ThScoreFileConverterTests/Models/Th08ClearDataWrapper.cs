using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th08ClearDataWrapper
    {
        private static Type ParentType = typeof(Th08Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+ClearData";

        private readonly PrivateObject pobj = null;

        public Th08ClearDataWrapper(Th06ChapterWrapper<Th08Converter> chapter)
            => this.pobj = new PrivateObject(
                AssemblyNameToTest,
                TypeNameToTest,
                new Type[] { (chapter ?? new Th06ChapterWrapper<Th08Converter>()).Target.GetType() },
                new object[] { chapter?.Target });

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
        // NOTE: Th08Converter.PlayableStages is a private enum and its underlying type is int.
        // public IReadOnlyDictionary<ThConverter.Level, Th08Converter.PlayableStages> StoryFlags
        //     => this.pobj.GetProperty(nameof(StoryFlags)) as Dictionary<ThConverter.Level, Th08Converter.PlayableStages>;
        public object StoryFlags
            => this.pobj.GetProperty(nameof(StoryFlags));
        public int[] ValuesOfStoryFlags
            => ((IEnumerable)this.StoryFlags.GetType().GetProperty("Values").GetValue(this.StoryFlags))
                .Cast<int>().ToArray();
        // NOTE: Th08Converter.PlayableStages is a private enum and its underlying type is int.
        // public IReadOnlyDictionary<ThConverter.Level, Th08Converter.PlayableStages> PracticeFlags
        //     => this.pobj.GetProperty(nameof(PracticeFlags)) as Dictionary<ThConverter.Level, Th08Converter.PlayableStages>;
        public object PracticeFlags
            => this.pobj.GetProperty(nameof(PracticeFlags));
        public int[] ValuesOfPracticeFlags
            => ((IEnumerable)this.PracticeFlags.GetType().GetProperty("Values").GetValue(this.PracticeFlags))
                .Cast<int>().ToArray();
        public Th08Converter.CharaWithTotal? Chara
            => this.pobj.GetProperty(nameof(Chara)) as Th08Converter.CharaWithTotal?;
    }
}
