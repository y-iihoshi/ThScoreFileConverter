using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Tests
{
    public sealed class Th06HeaderWrapper<TParent>
    {
        private static Type ParentType = typeof(TParent);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+Header";

        private readonly PrivateObject pobj = null;

        private Th06HeaderWrapper(params object[] args)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, args);

        public Th06HeaderWrapper(Th06ChapterWrapper<TParent> chapter)
            : this(new object[] { chapter?.Target })
        {
        }

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
