using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace ThScoreFileConverter.Models.Tests
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th143StatusWrapper
    {
        private static Type ParentType = typeof(Th143Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+Status";
        private static readonly PrivateType PrivateType = new PrivateType(AssemblyNameToTest, TypeNameToTest);

        private readonly PrivateObject pobj = null;

        public Th143StatusWrapper(Th095ChapterWrapper<Th143Converter> chapter)
            => this.pobj = new PrivateObject(
                AssemblyNameToTest,
                TypeNameToTest,
                new Type[] { (chapter ?? new Th095ChapterWrapper<Th143Converter>()).Target.GetType() },
                new object[] { chapter?.Target });

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public string Signature
            => this.pobj.GetProperty(nameof(this.Signature)) as string;
        public ushort? Version
            => this.pobj.GetProperty(nameof(this.Version)) as ushort?;
        public int? Size
            => this.pobj.GetProperty(nameof(this.Size)) as int?;
        public uint? Checksum
            => this.pobj.GetProperty(nameof(this.Checksum)) as uint?;
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

        public static bool CanInitialize(Th095ChapterWrapper<Th143Converter> chapter)
            => (bool)PrivateType.InvokeStatic(
                nameof(CanInitialize), new object[] { chapter.Target }, CultureInfo.InvariantCulture);
    }
}
