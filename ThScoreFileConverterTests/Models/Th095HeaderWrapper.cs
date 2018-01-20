using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;

namespace ThScoreFileConverter.Models.Tests
{
    public sealed class Th095HeaderWrapper<TParent>
    {
        private static Type ParentType = typeof(TParent);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+Header";

        private PrivateObject pobj = null;

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static Th095HeaderWrapper<TParent> Create(byte[] array)
        {
            var header = new Th095HeaderWrapper<TParent>();

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

        private Th095HeaderWrapper(params object[] args)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, args);

        public Th095HeaderWrapper()
            : this(new object[] { })
        {
        }

        public object Target => this.pobj.Target;

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
            => this.pobj.Invoke(
                nameof(this.ReadFrom),
                new object[] { reader },
                CultureInfo.InvariantCulture);

        public void WriteTo(BinaryWriter writer)
            => this.pobj.Invoke(
                nameof(this.WriteTo),
                new object[] { writer },
                CultureInfo.InvariantCulture);
    }
}
