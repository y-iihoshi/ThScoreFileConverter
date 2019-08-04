﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    public sealed class Th075ClearDataWrapper
    {
        private static readonly Type ParentType = typeof(Th075Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+ClearData";

        private readonly PrivateObject pobj = null;

        public static Th075ClearDataWrapper Create(byte[] array)
        {
            var clearData = new Th075ClearDataWrapper();

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

        public Th075ClearDataWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "obj")]
        public Th075ClearDataWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

        public object Target
            => this.pobj.Target;
        public int? UseCount
            => this.pobj.GetProperty(nameof(this.UseCount)) as int?;
        public int? ClearCount
            => this.pobj.GetProperty(nameof(this.ClearCount)) as int?;
        public int? MaxCombo
            => this.pobj.GetProperty(nameof(this.MaxCombo)) as int?;
        public int? MaxDamage
            => this.pobj.GetProperty(nameof(this.MaxDamage)) as int?;
        public IReadOnlyList<int> MaxBonuses
            => this.pobj.GetProperty(nameof(this.MaxBonuses)) as List<int>;
        public IReadOnlyList<short> CardGotCount
            => this.pobj.GetProperty(nameof(this.CardGotCount)) as List<short>;
        public IReadOnlyList<short> CardTrialCount
            => this.pobj.GetProperty(nameof(this.CardTrialCount)) as List<short>;
        public IReadOnlyList<byte> CardTrulyGot
            => this.pobj.GetProperty(nameof(this.CardTrulyGot)) as List<byte>;
        // NOTE: Th075Converter.HighScore is a private class.
        // public IReadOnlyList<HighScore> Ranking
        //     => this.pobj.GetProperty(nameof(this.Ranking)) as List<HighScore>;
        public object Ranking
            => this.pobj.GetProperty(nameof(this.Ranking));
        public int RankingCount
            => (int)this.Ranking.GetType().GetProperty("Count").GetValue(this.Ranking);
        public Th075HighScoreWrapper RankingItem(int index)
            => new Th075HighScoreWrapper(
                this.Ranking.GetType().GetProperty("Item").GetValue(this.Ranking, new object[] { index }));

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
