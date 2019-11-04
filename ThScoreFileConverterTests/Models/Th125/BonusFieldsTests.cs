using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th125;

namespace ThScoreFileConverterTests.Models.Th125
{
    [TestClass]
    public class BonusFieldsTests
    {
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(12345678)]
        [DataRow(int.MinValue)]
        [DataRow(int.MaxValue)]
        public void DataTest(int data) => TestUtils.Wrap(() =>
        {
            var fields = new BestShotHeader.BonusFields(data);
            Assert.AreEqual(data, fields.Data);
        });

        [DataTestMethod]
        [DataRow(0x4, true)]
        [DataRow(~0x4, false)]
        public void TwoShotTest(int data, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new BestShotHeader.BonusFields(data);
            Assert.AreEqual(expected, fields.TwoShot);
        });

        [DataTestMethod]
        [DataRow(0x8, true)]
        [DataRow(~0x8, false)]
        public void NiceShotTest(int data, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new BestShotHeader.BonusFields(data);
            Assert.AreEqual(expected, fields.NiceShot);
        });

        [DataTestMethod]
        [DataRow(0x10, true)]
        [DataRow(~0x10, false)]
        public void RiskBonusTest(int data, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new BestShotHeader.BonusFields(data);
            Assert.AreEqual(expected, fields.RiskBonus);
        });

        [DataTestMethod]
        [DataRow(0x40, true)]
        [DataRow(~0x40, false)]
        public void RedShotTest(int data, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new BestShotHeader.BonusFields(data);
            Assert.AreEqual(expected, fields.RedShot);
        });

        [DataTestMethod]
        [DataRow(0x80, true)]
        [DataRow(~0x80, false)]
        public void PurpleShotTest(int data, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new BestShotHeader.BonusFields(data);
            Assert.AreEqual(expected, fields.PurpleShot);
        });

        [DataTestMethod]
        [DataRow(0x100, true)]
        [DataRow(~0x100, false)]
        public void BlueShotTest(int data, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new BestShotHeader.BonusFields(data);
            Assert.AreEqual(expected, fields.BlueShot);
        });

        [DataTestMethod]
        [DataRow(0x200, true)]
        [DataRow(~0x200, false)]
        public void CyanShotTest(int data, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new BestShotHeader.BonusFields(data);
            Assert.AreEqual(expected, fields.CyanShot);
        });

        [DataTestMethod]
        [DataRow(0x400, true)]
        [DataRow(~0x400, false)]
        public void GreenShotTest(int data, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new BestShotHeader.BonusFields(data);
            Assert.AreEqual(expected, fields.GreenShot);
        });

        [DataTestMethod]
        [DataRow(0x800, true)]
        [DataRow(~0x800, false)]
        public void YellowShotTest(int data, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new BestShotHeader.BonusFields(data);
            Assert.AreEqual(expected, fields.YellowShot);
        });

        [DataTestMethod]
        [DataRow(0x1000, true)]
        [DataRow(~0x1000, false)]
        public void OrangeShotTest(int data, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new BestShotHeader.BonusFields(data);
            Assert.AreEqual(expected, fields.OrangeShot);
        });

        [DataTestMethod]
        [DataRow(0x2000, true)]
        [DataRow(~0x2000, false)]
        public void ColorfulShotTest(int data, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new BestShotHeader.BonusFields(data);
            Assert.AreEqual(expected, fields.ColorfulShot);
        });

        [DataTestMethod]
        [DataRow(0x4000, true)]
        [DataRow(~0x4000, false)]
        public void RainbowShotTest(int data, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new BestShotHeader.BonusFields(data);
            Assert.AreEqual(expected, fields.RainbowShot);
        });

        [DataTestMethod]
        [DataRow(0x1_0000, true)]
        [DataRow(~0x1_0000, false)]
        public void SoloShotTest(int data, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new BestShotHeader.BonusFields(data);
            Assert.AreEqual(expected, fields.SoloShot);
        });

        [DataTestMethod]
        [DataRow(0x40_0000, true)]
        [DataRow(~0x40_0000, false)]
        public void MacroBonusTest(int data, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new BestShotHeader.BonusFields(data);
            Assert.AreEqual(expected, fields.MacroBonus);
        });

        [DataTestMethod]
        [DataRow(0x100_0000, true)]
        [DataRow(~0x100_0000, false)]
        public void FrontShotTest(int data, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new BestShotHeader.BonusFields(data);
            Assert.AreEqual(expected, fields.FrontShot);
        });

        [DataTestMethod]
        [DataRow(0x200_0000, true)]
        [DataRow(~0x200_0000, false)]
        public void BackShotTest(int data, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new BestShotHeader.BonusFields(data);
            Assert.AreEqual(expected, fields.BackShot);
        });

        [DataTestMethod]
        [DataRow(0x400_0000, true)]
        [DataRow(~0x400_0000, false)]
        public void SideShotTest(int data, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new BestShotHeader.BonusFields(data);
            Assert.AreEqual(expected, fields.SideShot);
        });

        [DataTestMethod]
        [DataRow(0x800_0000, true)]
        [DataRow(~0x800_0000, false)]
        public void ClearShotTest(int data, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new BestShotHeader.BonusFields(data);
            Assert.AreEqual(expected, fields.ClearShot);
        });

        [DataTestMethod]
        [DataRow(0x1000_0000, true)]
        [DataRow(~0x1000_0000, false)]
        public void CatBonusTest(int data, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new BestShotHeader.BonusFields(data);
            Assert.AreEqual(expected, fields.CatBonus);
        });
    }
}
