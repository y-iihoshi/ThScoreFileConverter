using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace ThScoreFileConverter.Models.Tests
{
    public sealed class Th075ClearDataWrapper
    {
        private static Type ParentType = typeof(Th075Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+ClearData";

        private readonly PrivateObject pobj = null;

        public Th075ClearDataWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { });

        public object Target
            => this.pobj.Target;
        public int? UseCount
            => this.pobj.GetProperty(nameof(UseCount)) as int?;
        public int? ClearCount
            => this.pobj.GetProperty(nameof(ClearCount)) as int?;
        public int? MaxCombo
            => this.pobj.GetProperty(nameof(MaxCombo)) as int?;
        public int? MaxDamage
            => this.pobj.GetProperty(nameof(MaxDamage)) as int?;
        public IReadOnlyList<int> MaxBonuses
            => this.pobj.GetProperty(nameof(MaxBonuses)) as List<int>;
        public IReadOnlyList<short> CardGotCount
            => this.pobj.GetProperty(nameof(CardGotCount)) as List<short>;
        public IReadOnlyList<short> CardTrialCount
            => this.pobj.GetProperty(nameof(CardTrialCount)) as List<short>;
        public IReadOnlyList<byte> CardTrulyGot
            => this.pobj.GetProperty(nameof(CardTrulyGot)) as List<byte>;
        // NOTE: Th075Converter.HighScore is a private class.
        // public IReadOnlyList<HighScore> Ranking
        //     => this.pobj.GetProperty(nameof(Ranking)) as List<HighScore>;
        public object Ranking
            => this.pobj.GetProperty(nameof(Ranking));
        public int RankingCount
            => (int)this.Ranking.GetType().GetProperty("Count").GetValue(this.Ranking);
        public Th075HighScoreWrapper RankingItem(int index)
            => new Th075HighScoreWrapper(
                this.Ranking.GetType().GetProperty("Item").GetValue(this.Ranking, new object[] { index }));

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
