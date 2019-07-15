using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    public sealed class Th07LastNameWrapper
    {
        private static Type LastNameType = typeof(ThScoreFileConverter.Models.Th07.LastName);
        private static string AssemblyNameToTest = LastNameType.Assembly.GetName().Name;
        private static string TypeNameToTest = LastNameType.FullName;

        private readonly PrivateObject pobj = null;

        public Th07LastNameWrapper(Th06ChapterWrapper chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });
        public Th07LastNameWrapper(object original)
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
        public IReadOnlyCollection<byte> Name
            => this.pobj.GetProperty(nameof(this.Name)) as byte[];
    }
}
