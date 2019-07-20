using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ThScoreFileConverter.Models.Th105;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0703.
    internal sealed class Th105SpellCardResultWrapper<TChara, TLevel>
        where TChara : struct, Enum
        where TLevel : struct, Enum
    {
        private readonly SpellCardResult<TChara, TLevel> original = null;

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static Th105SpellCardResultWrapper<TChara, TLevel> Create(byte[] array)
        {
            var spellCardResult = new Th105SpellCardResultWrapper<TChara, TLevel>();

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
            => this.original = new SpellCardResult<TChara, TLevel>();
        public Th105SpellCardResultWrapper(object original)
            => this.original = original as SpellCardResult<TChara, TLevel>;

        public object Target => this.original;
        public TChara? Enemy => this.original.Enemy;
        public TLevel? Level => this.original.Level;
        public int? Id => this.original.Id;
        public int? TrialCount => this.original.TrialCount;
        public int? GotCount => this.original.GotCount;
        public uint? Frames => this.original.Frames;

        public void ReadFrom(BinaryReader reader) => this.original.ReadFrom(reader);
    }
}
