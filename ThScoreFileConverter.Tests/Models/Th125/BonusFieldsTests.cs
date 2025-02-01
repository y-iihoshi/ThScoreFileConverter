using ThScoreFileConverter.Models.Th125;

namespace ThScoreFileConverter.Tests.Models.Th125;

[TestClass]
public class BonusFieldsTests
{
    [DataTestMethod]
    [DataRow(0)]
    [DataRow(12345678)]
    [DataRow(int.MinValue)]
    [DataRow(int.MaxValue)]
    public void DataTest(int data)
    {
        var fields = new BonusFields(data);
        fields.Data.ShouldBe(data);
    }

    [DataTestMethod]
    [DataRow(0x4, true)]
    [DataRow(~0x4, false)]
    public void TwoShotTest(int data, bool expected)
    {
        var fields = new BonusFields(data);
        fields.TwoShot.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0x8, true)]
    [DataRow(~0x8, false)]
    public void NiceShotTest(int data, bool expected)
    {
        var fields = new BonusFields(data);
        fields.NiceShot.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0x10, true)]
    [DataRow(~0x10, false)]
    public void RiskBonusTest(int data, bool expected)
    {
        var fields = new BonusFields(data);
        fields.RiskBonus.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0x40, true)]
    [DataRow(~0x40, false)]
    public void RedShotTest(int data, bool expected)
    {
        var fields = new BonusFields(data);
        fields.RedShot.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0x80, true)]
    [DataRow(~0x80, false)]
    public void PurpleShotTest(int data, bool expected)
    {
        var fields = new BonusFields(data);
        fields.PurpleShot.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0x100, true)]
    [DataRow(~0x100, false)]
    public void BlueShotTest(int data, bool expected)
    {
        var fields = new BonusFields(data);
        fields.BlueShot.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0x200, true)]
    [DataRow(~0x200, false)]
    public void CyanShotTest(int data, bool expected)
    {
        var fields = new BonusFields(data);
        fields.CyanShot.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0x400, true)]
    [DataRow(~0x400, false)]
    public void GreenShotTest(int data, bool expected)
    {
        var fields = new BonusFields(data);
        fields.GreenShot.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0x800, true)]
    [DataRow(~0x800, false)]
    public void YellowShotTest(int data, bool expected)
    {
        var fields = new BonusFields(data);
        fields.YellowShot.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0x1000, true)]
    [DataRow(~0x1000, false)]
    public void OrangeShotTest(int data, bool expected)
    {
        var fields = new BonusFields(data);
        fields.OrangeShot.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0x2000, true)]
    [DataRow(~0x2000, false)]
    public void ColorfulShotTest(int data, bool expected)
    {
        var fields = new BonusFields(data);
        fields.ColorfulShot.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0x4000, true)]
    [DataRow(~0x4000, false)]
    public void RainbowShotTest(int data, bool expected)
    {
        var fields = new BonusFields(data);
        fields.RainbowShot.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0x1_0000, true)]
    [DataRow(~0x1_0000, false)]
    public void SoloShotTest(int data, bool expected)
    {
        var fields = new BonusFields(data);
        fields.SoloShot.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0x40_0000, true)]
    [DataRow(~0x40_0000, false)]
    public void MacroBonusTest(int data, bool expected)
    {
        var fields = new BonusFields(data);
        fields.MacroBonus.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0x100_0000, true)]
    [DataRow(~0x100_0000, false)]
    public void FrontShotTest(int data, bool expected)
    {
        var fields = new BonusFields(data);
        fields.FrontShot.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0x200_0000, true)]
    [DataRow(~0x200_0000, false)]
    public void BackShotTest(int data, bool expected)
    {
        var fields = new BonusFields(data);
        fields.BackShot.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0x400_0000, true)]
    [DataRow(~0x400_0000, false)]
    public void SideShotTest(int data, bool expected)
    {
        var fields = new BonusFields(data);
        fields.SideShot.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0x800_0000, true)]
    [DataRow(~0x800_0000, false)]
    public void ClearShotTest(int data, bool expected)
    {
        var fields = new BonusFields(data);
        fields.ClearShot.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0x1000_0000, true)]
    [DataRow(~0x1000_0000, false)]
    public void CatBonusTest(int data, bool expected)
    {
        var fields = new BonusFields(data);
        fields.CatBonus.ShouldBe(expected);
    }
}
