using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ThScoreFileConverter.Models.Th07;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    public sealed class Th07VersionInfoWrapper
    {
        private static readonly Type TypeToTest = typeof(VersionInfo);
        private static readonly string AssemblyNameToTest = TypeToTest.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = TypeToTest.FullName;

        private readonly PrivateObject pobj = null;

        public Th07VersionInfoWrapper(Th06ChapterWrapper chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });
        public Th07VersionInfoWrapper(object original)
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
        public IReadOnlyCollection<byte> Version
            => this.pobj.GetProperty(nameof(this.Version)) as byte[];
    }
}
