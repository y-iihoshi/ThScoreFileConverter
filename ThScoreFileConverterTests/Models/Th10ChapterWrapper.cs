using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;

namespace ThScoreFileConverter.Models.Tests
{
    public sealed class Th10ChapterWrapper<TParent>
    {
        private static Type ParentType = typeof(TParent);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+Chapter";

        private PrivateObject pobj = null;

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static Th10ChapterWrapper<TParent> Create(byte[] array)
        {
            var chapter = new Th10ChapterWrapper<TParent>();

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

        private Th10ChapterWrapper(params object[] args)
        {
            this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, args);
        }

        public Th10ChapterWrapper()
            : this(new object[] { })
        {
        }

        public Th10ChapterWrapper(Th10ChapterWrapper<TParent> chapter)
            : this(new object[] { chapter?.Target })
        {
        }

        public object Target => this.pobj.Target;

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

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(
                nameof(this.ReadFrom),
                new object[] { reader },
                CultureInfo.InvariantCulture);
    }
}
