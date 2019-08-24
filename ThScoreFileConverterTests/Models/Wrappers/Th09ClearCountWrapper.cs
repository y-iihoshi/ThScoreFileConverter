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
        private static readonly Type ParentType = typeof(Th09Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+ClearCount";

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
        public Th09ClearCountWrapper(object original)
            => this.pobj = new PrivateObject(original);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public IReadOnlyDictionary<Level, int> Counts
            => this.pobj.GetProperty(nameof(this.Counts)) as Dictionary<Level, int>;

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
