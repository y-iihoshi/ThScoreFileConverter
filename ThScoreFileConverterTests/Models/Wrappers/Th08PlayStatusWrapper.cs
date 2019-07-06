using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051 and CS0053.
    internal sealed class Th08PlayStatusWrapper
    {
        private static Type ParentType = typeof(Th08Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+PlayStatus";

        private readonly PrivateObject pobj = null;

        public Th08PlayStatusWrapper(Th06ChapterWrapper chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });
        public Th08PlayStatusWrapper(object obj)
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
        public Time TotalRunningTime
            => this.pobj.GetProperty(nameof(TotalRunningTime)) as Time;
        public Time TotalPlayTime
            => this.pobj.GetProperty(nameof(TotalPlayTime)) as Time;
        // NOTE: Th08Converter.PlayCount is a private class.
        // public IReadOnlyDictionary<ThConverter.Level, PlayCount> PlayCounts
        //     => this.pobj.GetProperty(nameof(PlayCounts)) as Dictionary<ThConverter.Level, PlayCount>;
        public object PlayCounts
            => this.pobj.GetProperty(nameof(PlayCounts));
        public Th08PlayCountWrapper PlayCountsItem(ThConverter.Level level)
            => new Th08PlayCountWrapper(
                this.PlayCounts.GetType().GetProperty("Item").GetValue(this.PlayCounts, new object[] { level }));
        // NOTE: Th08Converter.PlayCount is a private class.
        // public PlayCount TotalPlayCounts
        //     => this.pobj.GetProperty(nameof(TotalPlayCounts)) as PlayCount;
        public Th08PlayCountWrapper TotalPlayCount
            => new Th08PlayCountWrapper(this.pobj.GetProperty(nameof(TotalPlayCount)));
        public IReadOnlyCollection<byte> BgmFlags
            => this.pobj.GetProperty(nameof(BgmFlags)) as byte[];
    }
}
