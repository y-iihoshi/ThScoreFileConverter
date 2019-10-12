using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0053.
    internal sealed class Th08PlayCountWrapper
    {
        private static readonly Type ParentType = typeof(Th08Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+PlayCount";

        private readonly PrivateObject pobj = null;

        public static Th08PlayCountWrapper Create(byte[] array)
        {
            var playCount = new Th08PlayCountWrapper();

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    playCount.ReadFrom(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return playCount;
        }

        public Th08PlayCountWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);
        public Th08PlayCountWrapper(object original)
            => this.pobj = new PrivateObject(original);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public int? TotalTrial
            => this.pobj.GetProperty(nameof(this.TotalTrial)) as int?;
        public IReadOnlyDictionary<Chara, int> Trials
            => this.pobj.GetProperty(nameof(this.Trials)) as IReadOnlyDictionary<Chara, int>;
        public int? TotalClear
            => this.pobj.GetProperty(nameof(this.TotalClear)) as int?;
        public int? TotalContinue
            => this.pobj.GetProperty(nameof(this.TotalContinue)) as int?;
        public int? TotalPractice
            => this.pobj.GetProperty(nameof(this.TotalPractice)) as int?;

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
