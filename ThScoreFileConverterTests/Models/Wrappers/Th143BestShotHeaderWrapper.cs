using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0053.
    internal sealed class Th143BestShotHeaderWrapper
    {
        private static readonly Type ParentType = typeof(Th143Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+BestShotHeader";

        private readonly PrivateObject pobj = null;

        public static Th143BestShotHeaderWrapper Create(byte[] array)
        {
            var header = new Th143BestShotHeaderWrapper();

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    header.ReadFrom(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return header;
        }

        public Th143BestShotHeaderWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);
        public Th143BestShotHeaderWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public string Signature
            => this.pobj.GetProperty(nameof(this.Signature)) as string;
        public Th143Converter.Day? Day
            => this.pobj.GetProperty(nameof(this.Day)) as Th143Converter.Day?;
        public short? Scene
            => this.pobj.GetProperty(nameof(this.Scene)) as short?;
        public short? Width
            => this.pobj.GetProperty(nameof(this.Width)) as short?;
        public short? Height
            => this.pobj.GetProperty(nameof(this.Height)) as short?;
        public uint? DateTime
            => this.pobj.GetProperty(nameof(this.DateTime)) as uint?;
        public float? SlowRate
            => this.pobj.GetProperty(nameof(this.SlowRate)) as float?;

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(this.ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
