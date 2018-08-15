using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051 and CS0053.
    internal sealed class Th075AllScoreDataWrapper
    {
        private static Type ParentType = typeof(Th075Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+AllScoreData";

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

        // NOTE: Th075Converter.ClearData is a private class.
        // public IReadOnlyDictionary<Chara, Dictionary<Level, ClearData>> ClearData
        //     => this.pobj.GetProperty(nameof(ClearData)) as Dictionary<Chara, Dictionary<Level, ClearData>>;
        public object ClearData
            => this.pobj.GetProperty(nameof(ClearData));
        public int? ClearDataCount
            => this.ClearData.GetType().GetProperty("Count").GetValue(this.ClearData) as int?;
        public object ClearDataPerChara(Th075Converter.Chara chara)
            => this.ClearData.GetType().GetProperty("Item").GetValue(this.ClearData, new object[] { chara });
        public int? ClearDataPerCharaCount(Th075Converter.Chara chara)
        {
            var clearDataPerChara = this.ClearDataPerChara(chara);
            return clearDataPerChara.GetType().GetProperty("Count").GetValue(clearDataPerChara) as int?;
        }

        public Th075ClearDataWrapper ClearDataPerCharaLevel(Th075Converter.Chara chara, Th075Converter.Level level)
        {
            var clearDataPerChara = this.ClearDataPerChara(chara);
            return new Th075ClearDataWrapper(
                clearDataPerChara.GetType().GetProperty("Item").GetValue(clearDataPerChara, new object[] { level }));
        }

        public Th075StatusWrapper Status
        {
            get
            {
                var status = this.pobj.GetProperty(nameof(Status));
                return (status != null) ?  new Th075StatusWrapper(status) : null;
            }
        }

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
