using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th128;
using ThScoreFileConverterTests.Models.Th10.Wrappers;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050 and CS0051.
    internal sealed class Th128CardDataWrapper
    {
        private static readonly Type ParentType = typeof(Th128Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+CardData";
        private static readonly PrivateType PrivateType = new PrivateType(AssemblyNameToTest, TypeNameToTest);

        private readonly PrivateObject pobj = null;

        public Th128CardDataWrapper(ChapterWrapper chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });
        public Th128CardDataWrapper(object original)
            => this.pobj = new PrivateObject(original);

        public object Target
            => this.pobj.Target;
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

#if true
        public IReadOnlyDictionary<int, ISpellCard> Cards
            => this.pobj.GetProperty(nameof(this.Cards)) as IReadOnlyDictionary<int, ISpellCard>;
#else
        public object Cards
            => this.pobj.GetProperty(nameof(this.Cards));
        public Th128SpellCardWrapper CardsItem(int id)
            => new Th128SpellCardWrapper(
                this.Cards.GetType().GetProperty("Item").GetValue(this.Cards, new object[] { id }));
#endif

        public static bool CanInitialize(ChapterWrapper chapter)
            => (bool)PrivateType.InvokeStatic(
                nameof(CanInitialize), new object[] { chapter.Target }, CultureInfo.InvariantCulture);
    }
}
