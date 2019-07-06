using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th08PracticeScoreWrapper
    {
        private static Type ParentType = typeof(Th08Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+PracticeScore";

        private readonly PrivateObject pobj = null;

        public Th08PracticeScoreWrapper(Th06ChapterWrapper chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });
        public Th08PracticeScoreWrapper(object obj)
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
        // NOTE: Th08Converter.StageLevelPair is a private class.
        // public IReadOnlyDictionary<StageLevelPair, int> PlayCounts
        //     => this.pobj.GetProperty(nameof(PlayCounts)) as Dictionary<StageLevelPair, int>;
        public object PlayCounts
            => this.pobj.GetProperty(nameof(PlayCounts));
        public IReadOnlyCollection<int> PlayCountsValues
            => this.PlayCounts.GetType().GetProperty("Values").GetValue(this.PlayCounts) as IReadOnlyCollection<int>;
        // NOTE: Th08Converter.StageLevelPair is a private class.
        // public IReadOnlyDictionary<StageLevelPair, int> HighScores
        //     => this.pobj.GetProperty(nameof(HighScores)) as Dictionary<StageLevelPair, int>;
        public object HighScores
            => this.pobj.GetProperty(nameof(HighScores));
        public IReadOnlyCollection<int> HighScoresValues
            => this.HighScores.GetType().GetProperty("Values").GetValue(this.HighScores) as IReadOnlyCollection<int>;
        public Th08Converter.Chara? Chara
            => this.pobj.GetProperty(nameof(Chara)) as Th08Converter.Chara?;
    }
}
