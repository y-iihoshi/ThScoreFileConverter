using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;

namespace ThScoreFileConverter.Models.Tests
{
    // NOTE: Setting the accessibility as public causes CS0053.
    internal sealed class Th10SpellCardWrapper<TParent>
    {
        private static Type ParentType = typeof(TParent);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+SpellCard";

        private readonly PrivateObject pobj = null;

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static Th10SpellCardWrapper<TParent> Create(byte[] array)
        {
            var spellCard = new Th10SpellCardWrapper<TParent>();

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    spellCard.ReadFrom(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return spellCard;
        }

        public Th10SpellCardWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);

        // NOTE: Enabling the following causes CA1811.
        // public object Target => this.pobj.Target;

        public IReadOnlyCollection<byte> Name
            => this.pobj.GetProperty(nameof(Name)) as byte[];
        public int? ClearCount
            => this.pobj.GetProperty(nameof(ClearCount)) as int?;
        public int? TrialCount
            => this.pobj.GetProperty(nameof(TrialCount)) as int?;
        public int? Id
            => this.pobj.GetProperty(nameof(Id)) as int?;
        public ThConverter.Level? Level
            => this.pobj.GetProperty(nameof(Level)) as ThConverter.Level?;

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
