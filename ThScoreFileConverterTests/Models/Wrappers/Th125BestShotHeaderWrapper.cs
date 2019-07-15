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
    internal sealed class Th125BestShotHeaderWrapper
    {
        private static readonly Type ParentType = typeof(Th125Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+BestShotHeader";

        private readonly PrivateObject pobj = null;

        public static Th125BestShotHeaderWrapper Create(byte[] array)
        {
            var header = new Th125BestShotHeaderWrapper();

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

        public Th125BestShotHeaderWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);
        public Th125BestShotHeaderWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public string Signature
            => this.pobj.GetProperty(nameof(this.Signature)) as string;
        public Th125Converter.Level? Level
            => this.pobj.GetProperty(nameof(this.Level)) as Th125Converter.Level?;
        public short? Scene
            => this.pobj.GetProperty(nameof(this.Scene)) as short?;
        public short? Width
            => this.pobj.GetProperty(nameof(this.Width)) as short?;
        public short? Height
            => this.pobj.GetProperty(nameof(this.Height)) as short?;
        public short? Width2
            => this.pobj.GetProperty(nameof(this.Width2)) as short?;
        public short? Height2
            => this.pobj.GetProperty(nameof(this.Height2)) as short?;
        public short? HalfWidth
            => this.pobj.GetProperty(nameof(this.HalfWidth)) as short?;
        public short? HalfHeight
            => this.pobj.GetProperty(nameof(this.HalfHeight)) as short?;
        public uint? DateTime
            => this.pobj.GetProperty(nameof(this.DateTime)) as uint?;
        public float? SlowRate
            => this.pobj.GetProperty(nameof(this.SlowRate)) as float?;
        // NOTE: Th125Converter.BestShotHeader.BonusFields is a private struct.
        // public BonusFields Fields
        //     => this.pobj.GetProperty(nameof(this.Fields)) as BonusFields;
        public Th125BonusFieldsWrapper Fields
            => new Th125BonusFieldsWrapper(this.pobj.GetProperty(nameof(this.Fields)));
        public int? ResultScore
            => this.pobj.GetProperty(nameof(this.ResultScore)) as int?;
        public int? BasePoint
            => this.pobj.GetProperty(nameof(this.BasePoint)) as int?;
        public int? RiskBonus
            => this.pobj.GetProperty(nameof(this.RiskBonus)) as int?;
        public float? BossShot
            => this.pobj.GetProperty(nameof(this.BossShot)) as float?;
        public float? NiceShot
            => this.pobj.GetProperty(nameof(this.NiceShot)) as float?;
        public float? AngleBonus
            => this.pobj.GetProperty(nameof(this.AngleBonus)) as float?;
        public int? MacroBonus
            => this.pobj.GetProperty(nameof(this.MacroBonus)) as int?;
        public int? FrontSideBackShot
            => this.pobj.GetProperty(nameof(this.FrontSideBackShot)) as int?;
        public int? ClearShot
            => this.pobj.GetProperty(nameof(this.ClearShot)) as int?;
        public float? Angle
            => this.pobj.GetProperty(nameof(this.Angle)) as float?;
        public int? ResultScore2
            => this.pobj.GetProperty(nameof(this.ResultScore2)) as int?;
        public IReadOnlyCollection<byte> CardName
            => this.pobj.GetProperty(nameof(this.CardName)) as byte[];

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(this.ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
