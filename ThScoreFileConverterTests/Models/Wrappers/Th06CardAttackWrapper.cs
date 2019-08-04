using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Th06.Wrappers;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0051.
    internal sealed class Th06CardAttackWrapper
    {
        private static readonly Type ParentType = typeof(Th06Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+CardAttack";

        private readonly PrivateObject pobj = null;

        public Th06CardAttackWrapper(ChapterWrapper chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });
        public Th06CardAttackWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

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
        public short? CardId
            => this.pobj.GetProperty(nameof(CardId)) as short?;
        public IReadOnlyCollection<byte> CardName
            => this.pobj.GetProperty(nameof(CardName)) as byte[];
        public ushort? TrialCount
            => this.pobj.GetProperty(nameof(TrialCount)) as ushort?;
        public ushort? ClearCount
            => this.pobj.GetProperty(nameof(ClearCount)) as ushort?;

        public bool? HasTried()
            => this.pobj.Invoke(nameof(HasTried), new object[] { }, CultureInfo.InvariantCulture) as bool?;
    }
}
