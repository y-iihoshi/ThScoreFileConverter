using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models.Th095;

namespace ThScoreFileConverterTests.Models.Th095.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0703.
    internal sealed class ChapterWrapper
    {
        private static readonly Type TypeToTest = typeof(Chapter);
        private static readonly string AssemblyNameToTest = TypeToTest.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = TypeToTest.FullName;

        private readonly PrivateObject pobj = null;

        public static ChapterWrapper Create(byte[] array)
        {
            var chapter = new ChapterWrapper();

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

        public ChapterWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);
        public ChapterWrapper(ChapterWrapper chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });

        public Chapter Target
            => this.pobj.Target as Chapter;
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

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(this.ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
