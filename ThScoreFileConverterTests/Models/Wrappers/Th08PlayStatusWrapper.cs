﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Th06.Wrappers;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051 and CS0053.
    internal sealed class Th08PlayStatusWrapper
    {
        private static readonly Type ParentType = typeof(Th08Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+PlayStatus";

        private readonly PrivateObject pobj = null;

        public Th08PlayStatusWrapper(ChapterWrapper chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });
        public Th08PlayStatusWrapper(object original)
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
        public Time TotalRunningTime
            => this.pobj.GetProperty(nameof(this.TotalRunningTime)) as Time;
        public Time TotalPlayTime
            => this.pobj.GetProperty(nameof(this.TotalPlayTime)) as Time;
        // NOTE: Th08Converter.PlayCount is a private class.
        // public IReadOnlyDictionary<ThConverter.Level, PlayCount> PlayCounts
        //     => this.pobj.GetProperty(nameof(this.PlayCounts)) as Dictionary<ThConverter.Level, PlayCount>;
        public object PlayCounts
            => this.pobj.GetProperty(nameof(this.PlayCounts));
        public Th08PlayCountWrapper PlayCountsItem(Level level)
            => new Th08PlayCountWrapper(
                this.PlayCounts.GetType().GetProperty("Item").GetValue(this.PlayCounts, new object[] { level }));
        // NOTE: Th08Converter.PlayCount is a private class.
        // public PlayCount TotalPlayCounts
        //     => this.pobj.GetProperty(nameof(this.TotalPlayCounts)) as PlayCount;
        public Th08PlayCountWrapper TotalPlayCount
            => new Th08PlayCountWrapper(this.pobj.GetProperty(nameof(this.TotalPlayCount)));
        public IEnumerable<byte> BgmFlags
            => this.pobj.GetProperty(nameof(this.BgmFlags)) as IEnumerable<byte>;
    }
}
