using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051 and CS0053.
    internal sealed class Th15ClearDataPerGameModeWrapper
    {
        private static Type ParentType = typeof(Th15Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+ClearDataPerGameMode";

        private readonly PrivateObject pobj = null;

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static Th15ClearDataPerGameModeWrapper Create(byte[] array)
        {
            var clearData = new Th15ClearDataPerGameModeWrapper();

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

        public Th15ClearDataPerGameModeWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);
        public Th15ClearDataPerGameModeWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        // NOTE: Th15Converter.ScoreData is a private class.
        // public IReadOnlyDictionary<LevelWithTotal, ScoreData[]> Rankings
        //     => this.pobj.GetProperty(nameof(Rankings)) as Dictionary<LevelWithTotal, ScoreData[]>;
        public object Rankings
            => this.pobj.GetProperty(nameof(Rankings));
        public object[] Ranking(ThConverter.LevelWithTotal level)
            => this.Rankings.GetType().GetProperty("Item").GetValue(this.Rankings, new object[] { level }) as object[];
        public Th15ScoreDataWrapper RankingItem(ThConverter.LevelWithTotal level, int index)
            => new Th15ScoreDataWrapper(this.Ranking(level)[index]);
        public int? TotalPlayCount
            => this.pobj.GetProperty(nameof(TotalPlayCount)) as int?;
        public int? PlayTime
            => this.pobj.GetProperty(nameof(PlayTime)) as int?;
        public IReadOnlyDictionary<ThConverter.LevelWithTotal, int> ClearCounts
            => this.pobj.GetProperty(nameof(ClearCounts)) as Dictionary<ThConverter.LevelWithTotal, int>;
        public IReadOnlyDictionary<ThConverter.LevelWithTotal, int> ClearFlags
            => this.pobj.GetProperty(nameof(ClearFlags)) as Dictionary<ThConverter.LevelWithTotal, int>;
        // NOTE: Th15Converter.SpellCard is a private class.
        // public IReadOnlyDictionary<int, SpellCard> Cards
        //     => this.pobj.GetProperty(nameof(Cards)) as Dictionary<int, SpellCard>;
        public object Cards
            => this.pobj.GetProperty(nameof(Cards));
        public Th13SpellCardWrapper<Th15Converter, ThConverter.Level> CardsItem(int id)
            => new Th13SpellCardWrapper<Th15Converter, ThConverter.Level>(
                this.Cards.GetType().GetProperty("Item").GetValue(this.Cards, new object[] { id }));

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
