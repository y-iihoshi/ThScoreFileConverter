using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050 and CS0703.
    internal sealed class Th105AllScoreDataWrapper<TParent, TChara, TLevel>
        where TParent : ThConverter
        where TChara : struct, Enum
        where TLevel : struct, Enum
    {
        private static Type ParentType = typeof(TParent);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+AllScoreData";

        private readonly PrivateObject pobj = null;

        public static Th105AllScoreDataWrapper<TParent, TChara, TLevel> Create(byte[] array)
        {
            var allScoreData = new Th105AllScoreDataWrapper<TParent, TChara, TLevel>();

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    allScoreData.ReadFrom(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return allScoreData;
        }

        public Th105AllScoreDataWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;

        public IReadOnlyDictionary<TChara, byte> StoryClearCounts
            => this.pobj.GetProperty(nameof(StoryClearCounts)) as Dictionary<TChara, byte>;

        // NOTE: Th105Converter.CardForDeck is a private class.
        // public IReadOnlyDictionary<int, CardForDeck> SystemCards
        //     => this.pobj.GetProperty(nameof(System)) as Dictionary<int, CardForDeck>;
        public object SystemCards
            => this.pobj.GetProperty(nameof(SystemCards));
        public int? SystemCardsCount
            => this.SystemCards.GetType().GetProperty("Count").GetValue(this.SystemCards) as int?;
        public Th105CardForDeckWrapper<TParent> SystemCardsItem(int id)
            => new Th105CardForDeckWrapper<TParent>(
                this.SystemCards.GetType().GetProperty("Item").GetValue(this.SystemCards, new object[] { id }));

        // NOTE: Th105Converter.ClearData is a private class.
        // public IReadOnlyDictionary<Chara, ClearData> ClearData
        //     => this.pobj.GetProperty(nameof(ClearData)) as Dictionary<Chara, ClearData>;
        public object ClearData
            => this.pobj.GetProperty(nameof(ClearData));
        public int? ClearDataCount
            => this.ClearData.GetType().GetProperty("Count").GetValue(this.ClearData) as int?;
        public Th105ClearDataWrapper<TParent, TChara, TLevel> ClearDataItem(TChara chara)
            => new Th105ClearDataWrapper<TParent, TChara, TLevel>(
                this.ClearData.GetType().GetProperty("Item").GetValue(this.ClearData, new object[] { chara }));

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
