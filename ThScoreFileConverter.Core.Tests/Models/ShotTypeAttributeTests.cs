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

        _ = attribute.ShouldNotBeNull();
        attribute.Name.ShouldBe("Th06.ReimuA");
        attribute.FullName.ShouldBe("Th06.ReimuAFullName");
        attribute.ResourceType.ShouldBe(typeof(ShotTypeNames));
        attribute.Value.ShouldBe(Chara.ReimuA);
    }

    public static IEnumerable<object[]> InvalidCharacters => TestHelper.GetInvalidEnumerators<Chara>();

    [TestMethod]
    [DynamicData(nameof(InvalidCharacters))]
    public void ShotTypeAttributeTestInvalid(int chara)
    {
        _ = Should.Throw<ArgumentException>(() => new ShotTypeAttribute<Chara>((Chara)chara));
    }

    [TestMethod]
    public void GetLocalizedNameTest()
    {
        using var backup = TestHelper.BackupCultureInfo();

        var attribute = new ShotTypeAttribute<Chara>(Chara.MarisaA);

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        attribute.GetLocalizedName().ShouldBe("魔");

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        attribute.GetLocalizedName().ShouldBe("Magic");
    }

    [TestMethod]
    public void GetLocalizedNameTestUnregistered()
    {
        using var backup = TestHelper.BackupCultureInfo();

        var attribute = new ShotTypeAttribute<DayOfWeek>(DayOfWeek.Sunday);

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        attribute.GetLocalizedName().ShouldBe("System.Sunday");

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        attribute.GetLocalizedName().ShouldBe("System.Sunday");
    }

    [TestMethod]
    public void GetLocalizedFullNameTest()
    {
        using var backup = TestHelper.BackupCultureInfo();

        var attribute = new ShotTypeAttribute<Chara>(Chara.MarisaA);

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        attribute.GetLocalizedFullName().ShouldBe("魔符");

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        attribute.GetLocalizedFullName().ShouldBe("Magic Sign");
    }

    [TestMethod]
    public void GetLocalizedFullNameTestUnregistered()
    {
        using var backup = TestHelper.BackupCultureInfo();

        var attribute = new ShotTypeAttribute<DayOfWeek>(DayOfWeek.Sunday);

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        attribute.GetLocalizedFullName().ShouldBe("System.SundayFullName");

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        attribute.GetLocalizedFullName().ShouldBe("System.SundayFullName");
    }
}
