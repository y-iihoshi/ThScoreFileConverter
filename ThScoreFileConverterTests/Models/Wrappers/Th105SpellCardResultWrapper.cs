using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0703.
    internal sealed class Th105SpellCardResultWrapper<TParent, TChara, TLevel>
        where TParent : ThConverter
        where TChara : struct, Enum
        where TLevel : struct, Enum
    {
        private static Type ParentType = typeof(TParent);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+SpellCardResult";

        private readonly PrivateObject pobj = null;

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static Th105SpellCardResultWrapper<TParent, TChara, TLevel> Create(byte[] array)
        {
            var spellCardResult = new Th105SpellCardResultWrapper<TParent, TChara, TLevel>();

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    spellCardResult.ReadFrom(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return spellCardResult;
        }

        public Th105SpellCardResultWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);
        public Th105SpellCardResultWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public TChara? Enemy
            => this.pobj.GetProperty(nameof(this.Enemy)) as TChara?;
        public TLevel? Level
            => this.pobj.GetProperty(nameof(this.Level)) as TLevel?;
        public int? Id
            => this.pobj.GetProperty(nameof(this.Id)) as int?;
        public int? TrialCount
            => this.pobj.GetProperty(nameof(this.TrialCount)) as int?;
        public int? GotCount
            => this.pobj.GetProperty(nameof(this.GotCount)) as int?;
        public uint? Frames
            => this.pobj.GetProperty(nameof(this.Frames)) as uint?;

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(this.ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
