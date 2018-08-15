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
    internal sealed class Th08PlayCountWrapper
    {
        private static Type ParentType = typeof(Th08Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+PlayCount";

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
        public Th08PlayCountWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public int? TotalTrial
            => this.pobj.GetProperty(nameof(TotalTrial)) as int?;
        public IReadOnlyDictionary<Th08Converter.Chara, int> Trials
            => this.pobj.GetProperty(nameof(Trials)) as Dictionary<Th08Converter.Chara, int>;
        public int? TotalClear
            => this.pobj.GetProperty(nameof(TotalClear)) as int?;
        public int? TotalContinue
            => this.pobj.GetProperty(nameof(TotalContinue)) as int?;
        public int? TotalPractice
            => this.pobj.GetProperty(nameof(TotalPractice)) as int?;

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
