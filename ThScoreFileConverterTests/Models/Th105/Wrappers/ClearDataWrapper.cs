using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ThScoreFileConverter.Models.Th105;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0053.
    internal sealed class Th105ClearDataWrapper<TChara, TLevel>
        where TChara : struct, Enum
        where TLevel : struct, Enum
    {
        private readonly ClearData<TChara, TLevel> original = null;

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static Th105ClearDataWrapper<TChara, TLevel> Create(byte[] array)
        {
            var clearData = new Th105ClearDataWrapper<TChara, TLevel>();

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    clearData.ReadFrom(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return clearData;
        }

        public Th105ClearDataWrapper() => this.original = new ClearData<TChara, TLevel>();
        public Th105ClearDataWrapper(object original) => this.original = original as ClearData<TChara, TLevel>;

        public object Target => this.original;
        public IReadOnlyDictionary<int, CardForDeck> CardsForDeck => this.original.CardsForDeck;
        public IReadOnlyDictionary<(TChara Chara, int CardId), SpellCardResult<TChara, TLevel>> SpellCardResults
            => this.original.SpellCardResults;

        public void ReadFrom(BinaryReader reader) => this.original.ReadFrom(reader);
    }
}
