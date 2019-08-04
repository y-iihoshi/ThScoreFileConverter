using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Th06.Wrappers;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051 and CS0053.
    internal sealed class Th09PlayStatusWrapper
    {
        private static readonly Type ParentType = typeof(Th09Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+PlayStatus";

        private readonly PrivateObject pobj = null;

        public Th09PlayStatusWrapper(ChapterWrapper chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });
        public Th09PlayStatusWrapper(object obj)
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
        public Time TotalRunningTime
            => this.pobj.GetProperty(nameof(this.TotalRunningTime)) as Time;
        public Time TotalPlayTime
            => this.pobj.GetProperty(nameof(this.TotalPlayTime)) as Time;
        public IReadOnlyCollection<byte> BgmFlags
            => this.pobj.GetProperty(nameof(this.BgmFlags)) as byte[];
        public IReadOnlyDictionary<Th09Converter.Chara, byte> MatchFlags
            => this.pobj.GetProperty(nameof(this.MatchFlags)) as Dictionary<Th09Converter.Chara, byte>;
        public IReadOnlyDictionary<Th09Converter.Chara, byte> StoryFlags
            => this.pobj.GetProperty(nameof(this.StoryFlags)) as Dictionary<Th09Converter.Chara, byte>;
        public IReadOnlyDictionary<Th09Converter.Chara, byte> ExtraFlags
            => this.pobj.GetProperty(nameof(this.ExtraFlags)) as Dictionary<Th09Converter.Chara, byte>;
        // NOTE: Th09Converter.ClearCount is a private class.
        // public IReadOnlyDictionary<Th09Converter.Chara, ClearCount> ClearCounts
        //     => this.pobj.GetProperty(nameof(this.ClearCounts)) as Dictionary<Th09Converter.Chara, ClearCount>;
        public object ClearCounts
            => this.pobj.GetProperty(nameof(this.ClearCounts));
        public Th09ClearCountWrapper ClearCountsItem(Th09Converter.Chara chara)
            => new Th09ClearCountWrapper(
                this.ClearCounts.GetType().GetProperty("Item").GetValue(this.ClearCounts, new object[] { chara }));
    }
}
