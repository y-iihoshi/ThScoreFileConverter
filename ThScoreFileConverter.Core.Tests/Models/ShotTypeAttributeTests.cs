using System.Globalization;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th06;
using ThScoreFileConverter.Core.Resources;

namespace ThScoreFileConverter.Core.Tests.Models;

[TestClass]
public class ShotTypeAttributeTests
{
    [TestMethod]
    public void ShotTypeAttributeTest()
    {
        var attribute = new ShotTypeAttribute<Chara>(Chara.ReimuA);

        Assert.IsNotNull(attribute);
        Assert.AreEqual("Th06.ReimuA", attribute.Name);
        Assert.AreEqual("Th06.ReimuAFullName", attribute.FullName);
        Assert.AreEqual(typeof(ShotTypeNames), attribute.ResourceType);
        Assert.AreEqual(Chara.ReimuA, attribute.Value);
    }

    public static IEnumerable<object[]> InvalidCharacters => TestHelper.GetInvalidEnumerators<Chara>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidCharacters))]
    public void ShotTypeAttributeTestInvalid(int chara)
    {
        _ = Assert.ThrowsException<ArgumentException>(() => new ShotTypeAttribute<Chara>((Chara)chara));
    }

    [TestMethod]
    public void GetLocalizedNameTest()
    {
        using var backup = TestHelper.BackupCultureInfo();

        var attribute = new ShotTypeAttribute<Chara>(Chara.MarisaA);

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        Assert.AreEqual("魔", attribute.GetLocalizedName());

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        Assert.AreEqual("Magic", attribute.GetLocalizedName());
    }

    [TestMethod]
    public void GetLocalizedNameTestUnregistered()
    {
        using var backup = TestHelper.BackupCultureInfo();

        var attribute = new ShotTypeAttribute<DayOfWeek>(DayOfWeek.Sunday);

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        Assert.AreEqual("System.Sunday", attribute.GetLocalizedName());

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        Assert.AreEqual("System.Sunday", attribute.GetLocalizedName());
    }

    [TestMethod]
    public void GetLocalizedFullNameTest()
    {
        using var backup = TestHelper.BackupCultureInfo();

        var attribute = new ShotTypeAttribute<Chara>(Chara.MarisaA);

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        Assert.AreEqual("魔符", attribute.GetLocalizedFullName());

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        Assert.AreEqual("Magic Sign", attribute.GetLocalizedFullName());
    }

    [TestMethod]
    public void GetLocalizedFullNameTestUnregistered()
    {
        using var backup = TestHelper.BackupCultureInfo();

        var attribute = new ShotTypeAttribute<DayOfWeek>(DayOfWeek.Sunday);

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        Assert.AreEqual("System.SundayFullName", attribute.GetLocalizedFullName());

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        Assert.AreEqual("System.SundayFullName", attribute.GetLocalizedFullName());
    }
}
