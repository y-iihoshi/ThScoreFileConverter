using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    public sealed class Th07VersionInfoWrapper
    {
        private static Type VersionInfoType = typeof(ThScoreFileConverter.Models.Th07.VersionInfo);
        private static string AssemblyNameToTest = VersionInfoType.Assembly.GetName().Name;
        private static string TypeNameToTest = VersionInfoType.FullName;

        private readonly PrivateObject pobj = null;

        public Th07VersionInfoWrapper(Th06ChapterWrapper chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });
        public Th07VersionInfoWrapper(object original)
            => this.pobj = new PrivateObject(original);

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
        public IReadOnlyCollection<byte> Version
            => this.pobj.GetProperty(nameof(Version)) as byte[];
    }
}
