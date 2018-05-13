using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Tests
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th07PlayStatusWrapper
    {
        private static Type ParentType = typeof(Th07Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+PlayStatus";

        private readonly PrivateObject pobj = null;

        public Th07PlayStatusWrapper(Th06ChapterWrapper<Th07Converter> chapter)
        {
            if (chapter == null)
            {
                var ch = new Th06ChapterWrapper<Th07Converter>();
                this.pobj = new PrivateObject(
                    AssemblyNameToTest,
                    TypeNameToTest,
                    new Type[] { ch.Target.GetType() },
                    new object[] { null });
            }
            else
            {
                this.pobj = new PrivateObject(
                    AssemblyNameToTest,
                    TypeNameToTest,
                    new Type[] { chapter.Target.GetType() },
                    new object[] { chapter.Target });
            }
        }

        // NOTE: Enabling the following causes CA1811.
        // public object Target => this.pobj.Target;

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
        public Time TotalRunningTime
            => this.pobj.GetProperty(nameof(TotalRunningTime)) as Time;
        public Time TotalPlayTime
            => this.pobj.GetProperty(nameof(TotalPlayTime)) as Time;
        // NOTE: Th07Converter.PlayCount is a private class.
        // public IReadOnlyDictionary<Th07Converter.LevelWithTotal, PlayCount> PlayCounts
        //     => this.pobj.GetProperty(nameof(PlayCounts)) as Dictionary<Th07Converter.LevelWithTotal, PlayCount>;
        public object PlayCounts
            => this.pobj.GetProperty(nameof(PlayCounts));
        public Th07PlayCountWrapper PlayCountsItem(Th07Converter.LevelWithTotal level)
            => new Th07PlayCountWrapper(
                this.PlayCounts.GetType().GetProperty("Item").GetValue(this.PlayCounts, new object[] { level }));
    }
}
