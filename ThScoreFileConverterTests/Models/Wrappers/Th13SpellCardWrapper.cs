using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0703.
    internal sealed class Th13SpellCardWrapper<TParent, TLevel>
        where TParent : ThConverter
        where TLevel : struct, Enum
    {
        private static readonly Type ParentType = typeof(TParent);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+SpellCard";

        private readonly PrivateObject pobj = null;

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static Th13SpellCardWrapper<TParent, TLevel> Create(byte[] array)
        {
            var spellCard = new Th13SpellCardWrapper<TParent, TLevel>();

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

        public Th13SpellCardWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);
        public Th13SpellCardWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

        public object Target
            => this.pobj.Target;
        public IReadOnlyCollection<byte> Name
            => this.pobj.GetProperty(nameof(Name)) as byte[];
        public int? ClearCount
            => this.pobj.GetProperty(nameof(ClearCount)) as int?;
        public int? PracticeClearCount
            => this.pobj.GetProperty(nameof(PracticeClearCount)) as int?;
        public int? TrialCount
            => this.pobj.GetProperty(nameof(TrialCount)) as int?;
        public int? PracticeTrialCount
            => this.pobj.GetProperty(nameof(PracticeTrialCount)) as int?;
        public int? Id
            => this.pobj.GetProperty(nameof(Id)) as int?;
        public TLevel? Level
            => this.pobj.GetProperty(nameof(Level)) as TLevel?;
        public int? PracticeScore
            => this.pobj.GetProperty(nameof(PracticeScore)) as int?;

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
        public bool? HasTried()
            => this.pobj.Invoke(nameof(HasTried), new object[] { }, CultureInfo.InvariantCulture) as bool?;
    }
}
