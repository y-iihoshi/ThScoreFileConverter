using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0703.
    internal sealed class Th07FileHeaderWrapper<TParent>
        where TParent : ThConverter
    {
        private static readonly Type ParentType = typeof(TParent);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+FileHeader";

        private readonly PrivateObject pobj = null;

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static Th07FileHeaderWrapper<TParent> Create(byte[] array)
        {
            var chapter = new Th07FileHeaderWrapper<TParent>();

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

        public Th07FileHeaderWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);

        public object Target
            => this.pobj.Target;
        public ushort? Checksum
            => this.pobj.GetProperty(nameof(Checksum)) as ushort?;
        public short? Version
            => this.pobj.GetProperty(nameof(Version)) as short?;
        public int? Size
            => this.pobj.GetProperty(nameof(Size)) as int?;
        public int? DecodedAllSize
            => this.pobj.GetProperty(nameof(DecodedAllSize)) as int?;
        public int? DecodedBodySize
            => this.pobj.GetProperty(nameof(DecodedBodySize)) as int?;
        public int? EncodedBodySize
            => this.pobj.GetProperty(nameof(EncodedBodySize)) as int?;
        public bool? IsValid
            => this.pobj.GetProperty(nameof(IsValid)) as bool?;

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
        public void WriteTo(BinaryWriter writer)
            => this.pobj.Invoke(nameof(WriteTo), new object[] { writer }, CultureInfo.InvariantCulture);
    }
}
