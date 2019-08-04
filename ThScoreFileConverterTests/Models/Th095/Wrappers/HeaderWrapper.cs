using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Th095.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0703.
    internal sealed class HeaderWrapper<TParent>
        where TParent : ThConverter
    {
        private static readonly Type ParentType = typeof(TParent);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+Header";

        private readonly PrivateObject pobj = null;

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static HeaderWrapper<TParent> Create(byte[] array)
        {
            var header = new HeaderWrapper<TParent>();

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    header.ReadFrom(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return header;
        }

        public HeaderWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);
        public HeaderWrapper(object original)
            => this.pobj = new PrivateObject(original);

        public object Target
            => this.pobj.Target;
        public string Signature
            => this.pobj.GetProperty(nameof(this.Signature)) as string;
        public int? EncodedAllSize
            => this.pobj.GetProperty(nameof(this.EncodedAllSize)) as int?;
        public int? EncodedBodySize
            => this.pobj.GetProperty(nameof(this.EncodedBodySize)) as int?;
        public int? DecodedBodySize
            => this.pobj.GetProperty(nameof(this.DecodedBodySize)) as int?;
        public bool? IsValid
            => this.pobj.GetProperty(nameof(this.IsValid)) as bool?;

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(this.ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
        public void WriteTo(BinaryWriter writer)
            => this.pobj.Invoke(nameof(this.WriteTo), new object[] { writer }, CultureInfo.InvariantCulture);
    }
}
