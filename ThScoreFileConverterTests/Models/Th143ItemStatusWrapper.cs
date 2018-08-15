using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th143ItemStatusWrapper
    {
        private static Type ParentType = typeof(Th143Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+ItemStatus";
        private static readonly PrivateType PrivateType = new PrivateType(AssemblyNameToTest, TypeNameToTest);

        private readonly PrivateObject pobj = null;

        public Th143ItemStatusWrapper(Th10ChapterWrapper<Th143Converter> chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });
        public Th143ItemStatusWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

        public object Target
            => this.pobj.Target;
        public string Signature
            => this.pobj.GetProperty(nameof(this.Signature)) as string;
        public ushort? Version
            => this.pobj.GetProperty(nameof(this.Version)) as ushort?;
        public uint? Checksum
            => this.pobj.GetProperty(nameof(this.Checksum)) as uint?;
        public int? Size
            => this.pobj.GetProperty(nameof(this.Size)) as int?;
        public bool? IsValid
            => this.pobj.GetProperty(nameof(this.IsValid)) as bool?;
        public IReadOnlyCollection<byte> Data
            => this.pobj.GetProperty(nameof(this.Data)) as byte[];
        public Th143Converter.ItemWithTotal? Item
            => this.pobj.GetProperty(nameof(this.Item)) as Th143Converter.ItemWithTotal?;
        public int? UseCount
            => this.pobj.GetProperty(nameof(this.UseCount)) as int?;
        public int? ClearedCount
            => this.pobj.GetProperty(nameof(this.ClearedCount)) as int?;
        public int? ClearedScenes
            => this.pobj.GetProperty(nameof(this.ClearedScenes)) as int?;
        public int? ItemLevel
            => this.pobj.GetProperty(nameof(this.ItemLevel)) as int?;
        public int? AvailableCount
            => this.pobj.GetProperty(nameof(this.AvailableCount)) as int?;
        public int? FramesOrRanges
            => this.pobj.GetProperty(nameof(this.FramesOrRanges)) as int?;

        public static bool CanInitialize(Th10ChapterWrapper<Th143Converter> chapter)
            => (bool)PrivateType.InvokeStatic(
                nameof(CanInitialize), new object[] { chapter.Target }, CultureInfo.InvariantCulture);
    }
}
