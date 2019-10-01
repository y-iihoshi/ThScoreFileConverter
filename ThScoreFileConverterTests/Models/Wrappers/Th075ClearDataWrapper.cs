using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th075;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    internal sealed class Th075ClearDataWrapper
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
        public Th075ClearDataWrapper(object original)
            => this.pobj = new PrivateObject(original);

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
        public IReadOnlyList<HighScore> Ranking
            => this.pobj.GetProperty(nameof(this.Ranking)) as List<HighScore>;

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
