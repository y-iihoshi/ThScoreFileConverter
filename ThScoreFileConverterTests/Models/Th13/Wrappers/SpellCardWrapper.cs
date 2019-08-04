using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Th13.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0703.
    internal sealed class SpellCardWrapper<TParent, TLevel>
        where TParent : ThConverter
        where TLevel : struct, Enum
    {
        private static readonly Type ParentType = typeof(TParent);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+SpellCard";

        private readonly PrivateObject pobj = null;

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static SpellCardWrapper<TParent, TLevel> Create(byte[] array)
        {
            var spellCard = new SpellCardWrapper<TParent, TLevel>();

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

        public SpellCardWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);
        public SpellCardWrapper(object original)
            => this.pobj = new PrivateObject(original);

        public object Target
            => this.pobj.Target;
        public IReadOnlyCollection<byte> Name
            => this.pobj.GetProperty(nameof(this.Name)) as byte[];
        public int? ClearCount
            => this.pobj.GetProperty(nameof(this.ClearCount)) as int?;
        public int? PracticeClearCount
            => this.pobj.GetProperty(nameof(this.PracticeClearCount)) as int?;
        public int? TrialCount
            => this.pobj.GetProperty(nameof(this.TrialCount)) as int?;
        public int? PracticeTrialCount
            => this.pobj.GetProperty(nameof(this.PracticeTrialCount)) as int?;
        public int? Id
            => this.pobj.GetProperty(nameof(this.Id)) as int?;
        public TLevel? Level
            => this.pobj.GetProperty(nameof(this.Level)) as TLevel?;
        public int? PracticeScore
            => this.pobj.GetProperty(nameof(this.PracticeScore)) as int?;
        public bool? HasTried
            => this.pobj.GetProperty(nameof(this.HasTried)) as bool?;

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
