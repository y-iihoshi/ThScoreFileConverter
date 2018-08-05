using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    // NOTE: Setting the accessibility as public causes CS0703.
    internal sealed class Th08FlspWrapper
    {
        private static Type ParentType = typeof(Th08Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+FLSP";

        private readonly PrivateObject pobj = null;

        public Th08FlspWrapper(Th06ChapterWrapper<Th08Converter> chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
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
    }
}
