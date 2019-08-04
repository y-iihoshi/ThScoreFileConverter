using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Th10.Wrappers;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th143StatusWrapper
    {
        private static readonly Type ParentType = typeof(Th143Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+Status";
        private static readonly PrivateType PrivateType = new PrivateType(AssemblyNameToTest, TypeNameToTest);

        private readonly PrivateObject pobj = null;

        public Th143StatusWrapper(ChapterWrapper chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });
        public Th143StatusWrapper(object obj)
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
        public IReadOnlyCollection<byte> LastName
            => this.pobj.GetProperty(nameof(this.LastName)) as byte[];
        public IReadOnlyCollection<byte> BgmFlags
            => this.pobj.GetProperty(nameof(this.BgmFlags)) as byte[];
        public int? TotalPlayTime
            => this.pobj.GetProperty(nameof(this.TotalPlayTime)) as int?;
        public Th143Converter.ItemWithTotal? LastMainItem
            => this.pobj.GetProperty(nameof(this.LastMainItem)) as Th143Converter.ItemWithTotal?;
        public Th143Converter.ItemWithTotal? LastSubItem
            => this.pobj.GetProperty(nameof(this.LastSubItem)) as Th143Converter.ItemWithTotal?;
        public IReadOnlyCollection<byte> NicknameFlags
            => this.pobj.GetProperty(nameof(this.NicknameFlags)) as byte[];

        public static bool CanInitialize(ChapterWrapper chapter)
            => (bool)PrivateType.InvokeStatic(
                nameof(CanInitialize), new object[] { chapter.Target }, CultureInfo.InvariantCulture);
    }
}
