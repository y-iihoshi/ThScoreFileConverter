using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    public sealed class Th125BonusFieldsWrapper
    {
        private static readonly Type ConverterType = typeof(Th125Converter);
        private static readonly string AssemblyNameToTest = ConverterType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ConverterType.FullName + "+BestShotHeader+BonusFields";

        private readonly PrivateObject pobj = null;

        public Th125BonusFieldsWrapper(int data)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { data });
        public Th125BonusFieldsWrapper(object original)
            => this.pobj = new PrivateObject(original);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public int? Data
            => this.pobj.GetProperty(nameof(this.Data)) as int?;
        public bool? TwoShot
            => this.pobj.GetProperty(nameof(this.TwoShot)) as bool?;
        public bool? NiceShot
            => this.pobj.GetProperty(nameof(this.NiceShot)) as bool?;
        public bool? RiskBonus
            => this.pobj.GetProperty(nameof(this.RiskBonus)) as bool?;
        public bool? RedShot
            => this.pobj.GetProperty(nameof(this.RedShot)) as bool?;
        public bool? PurpleShot
            => this.pobj.GetProperty(nameof(this.PurpleShot)) as bool?;
        public bool? BlueShot
            => this.pobj.GetProperty(nameof(this.BlueShot)) as bool?;
        public bool? CyanShot
            => this.pobj.GetProperty(nameof(this.CyanShot)) as bool?;
        public bool? GreenShot
            => this.pobj.GetProperty(nameof(this.GreenShot)) as bool?;
        public bool? YellowShot
            => this.pobj.GetProperty(nameof(this.YellowShot)) as bool?;
        public bool? OrangeShot
            => this.pobj.GetProperty(nameof(this.OrangeShot)) as bool?;
        public bool? ColorfulShot
            => this.pobj.GetProperty(nameof(this.ColorfulShot)) as bool?;
        public bool? RainbowShot
            => this.pobj.GetProperty(nameof(this.RainbowShot)) as bool?;
        public bool? SoloShot
            => this.pobj.GetProperty(nameof(this.SoloShot)) as bool?;
        public bool? MacroBonus
            => this.pobj.GetProperty(nameof(this.MacroBonus)) as bool?;
        public bool? FrontShot
            => this.pobj.GetProperty(nameof(this.FrontShot)) as bool?;
        public bool? BackShot
            => this.pobj.GetProperty(nameof(this.BackShot)) as bool?;
        public bool? SideShot
            => this.pobj.GetProperty(nameof(this.SideShot)) as bool?;
        public bool? ClearShot
            => this.pobj.GetProperty(nameof(this.ClearShot)) as bool?;
        public bool? CatBonus
            => this.pobj.GetProperty(nameof(this.CatBonus)) as bool?;
    }
}
