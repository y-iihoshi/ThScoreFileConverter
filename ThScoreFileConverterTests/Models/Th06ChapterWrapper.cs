using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;

namespace ThScoreFileConverter.Models.Tests
{
    public sealed class Th06ChapterWrapper<TParent>
    {
        private static Type ParentType = typeof(TParent);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+Chapter";

        private readonly PrivateObject pobj = null;

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static Th06ChapterWrapper<TParent> Create(byte[] array)
        {
            var chapter = new Th06ChapterWrapper<TParent>();

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    chapter.ReadFrom(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return chapter;
        }

        private Th06ChapterWrapper(params object[] args)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, args);

        public Th06ChapterWrapper()
            : this(new object[] { })
        {
        }

        public Th06ChapterWrapper(Th06ChapterWrapper<TParent> chapter)
            : this(new object[] { chapter?.Target })
        {
        }

        public object Target => this.pobj.Target;

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

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(
                nameof(ReadFrom),
                new object[] { reader },
                CultureInfo.InvariantCulture);
    }
}
