using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th15;
using ThScoreFileConverterTests.Models.Th13.Wrappers;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051 and CS0053.
    internal sealed class Th15ClearDataPerGameModeWrapper
    {
        private static readonly Type ParentType = typeof(Th15Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+ClearDataPerGameMode";

        private readonly PrivateObject pobj = null;

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
        public Th15ClearDataPerGameModeWrapper(object original)
            => this.pobj = new PrivateObject(original);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public IReadOnlyDictionary<LevelWithTotal, IReadOnlyList<IScoreData>> Rankings
            => this.pobj.GetProperty(nameof(this.Rankings))
                as IReadOnlyDictionary<LevelWithTotal, IReadOnlyList<IScoreData>>;
        public int? TotalPlayCount
            => this.pobj.GetProperty(nameof(this.TotalPlayCount)) as int?;
        public int? PlayTime
            => this.pobj.GetProperty(nameof(this.PlayTime)) as int?;
        public IReadOnlyDictionary<LevelWithTotal, int> ClearCounts
            => this.pobj.GetProperty(nameof(this.ClearCounts)) as IReadOnlyDictionary<LevelWithTotal, int>;
        public IReadOnlyDictionary<LevelWithTotal, int> ClearFlags
            => this.pobj.GetProperty(nameof(this.ClearFlags)) as IReadOnlyDictionary<LevelWithTotal, int>;

        // NOTE: Th15Converter.SpellCard is a private class.
        // public IReadOnlyDictionary<int, SpellCard> Cards
        //     => this.pobj.GetProperty(nameof(this.Cards)) as IReadOnlyDictionary<int, SpellCard>;
        public object Cards
            => this.pobj.GetProperty(nameof(this.Cards));
        public SpellCardWrapper<Th15Converter, Level> CardsItem(int id)
            => new SpellCardWrapper<Th15Converter, Level>(
                this.Cards.GetType().GetProperty("Item").GetValue(this.Cards, new object[] { id }));

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
