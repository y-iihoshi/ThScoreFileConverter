using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Th07.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0703.
    internal sealed class FileHeaderWrapper<TParent>
        where TParent : ThConverter
    {
        private static readonly Type ParentType = typeof(TParent);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+FileHeader";

        private readonly PrivateObject pobj = null;

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static FileHeaderWrapper<TParent> Create(byte[] array)
        {
            var chapter = new FileHeaderWrapper<TParent>();

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

        public FileHeaderWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);

        public object Target
            => this.pobj.Target;
        public ushort? Checksum
            => this.pobj.GetProperty(nameof(this.Checksum)) as ushort?;
        public short? Version
            => this.pobj.GetProperty(nameof(this.Version)) as short?;
        public int? Size
            => this.pobj.GetProperty(nameof(this.Size)) as int?;
        public int? DecodedAllSize
            => this.pobj.GetProperty(nameof(this.DecodedAllSize)) as int?;
        public int? DecodedBodySize
            => this.pobj.GetProperty(nameof(this.DecodedBodySize)) as int?;
        public int? EncodedBodySize
            => this.pobj.GetProperty(nameof(this.EncodedBodySize)) as int?;
        public bool? IsValid
            => this.pobj.GetProperty(nameof(this.IsValid)) as bool?;

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
        public void WriteTo(BinaryWriter writer)
            => this.pobj.Invoke(nameof(WriteTo), new object[] { writer }, CultureInfo.InvariantCulture);
    }
}
