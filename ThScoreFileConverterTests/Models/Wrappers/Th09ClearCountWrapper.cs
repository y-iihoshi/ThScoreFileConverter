using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    // NOTE: Setting the accessibility as public causes CS0053.
    internal sealed class Th09ClearCountWrapper
    {
        private static Type ParentType = typeof(Th09Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+ClearCount";

        private readonly PrivateObject pobj = null;

        public static Th09ClearCountWrapper Create(byte[] array)
        {
            var clearCount = new Th09ClearCountWrapper();

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    clearCount.ReadFrom(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return clearCount;
        }

        public Th09ClearCountWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);
        public Th09ClearCountWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public IReadOnlyDictionary<ThConverter.Level, int> Counts
            => this.pobj.GetProperty(nameof(Counts)) as Dictionary<ThConverter.Level, int>;

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
