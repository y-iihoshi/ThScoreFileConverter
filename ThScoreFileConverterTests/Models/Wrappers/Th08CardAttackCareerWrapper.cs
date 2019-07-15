using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0053.
    internal sealed class Th08CardAttackCareerWrapper
    {
        private static readonly Type ParentType = typeof(Th08Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+CardAttackCareer";

        private readonly PrivateObject pobj = null;

        public static Th08CardAttackCareerWrapper Create(byte[] array)
        {
            var career = new Th08CardAttackCareerWrapper();

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    career.ReadFrom(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return career;
        }

        public Th08CardAttackCareerWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);
        public Th08CardAttackCareerWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public IReadOnlyDictionary<Th08Converter.CharaWithTotal, uint> MaxBonuses
            => this.pobj.GetProperty(nameof(MaxBonuses)) as Dictionary<Th08Converter.CharaWithTotal, uint>;
        public IReadOnlyDictionary<Th08Converter.CharaWithTotal, int> TrialCounts
            => this.pobj.GetProperty(nameof(TrialCounts)) as Dictionary<Th08Converter.CharaWithTotal, int>;
        public IReadOnlyDictionary<Th08Converter.CharaWithTotal, int> ClearCounts
            => this.pobj.GetProperty(nameof(ClearCounts)) as Dictionary<Th08Converter.CharaWithTotal, int>;

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
