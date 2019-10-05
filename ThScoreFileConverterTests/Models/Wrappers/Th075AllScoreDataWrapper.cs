using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th075;
using Level = ThScoreFileConverter.Models.Th075.Level;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th075AllScoreDataWrapper
    {
        private static readonly Type ParentType = typeof(Th075Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+AllScoreData";

        private readonly PrivateObject pobj = null;

        public static Th075AllScoreDataWrapper Create(byte[] array)
        {
            var allScoreData = new Th075AllScoreDataWrapper();

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    allScoreData.ReadFrom(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return allScoreData;
        }

        public Th075AllScoreDataWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;

        public IReadOnlyDictionary<(Chara, Level), ClearData> ClearData
            => this.pobj.GetProperty(nameof(this.ClearData)) as IReadOnlyDictionary<(Chara, Level), ClearData>;

        public Status Status
            => this.pobj.GetProperty(nameof(this.Status)) as Status;

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
