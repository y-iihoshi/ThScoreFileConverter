using System.Globalization;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Resources;

namespace ThScoreFileConverter.Core.Tests.Models;

[TestClass]
public class CharacterAttributeTests
{
    [TestMethod]
    public void CharacterAttributeTestName()
    {
        var name = "Reimu";
        var attribute = new CharacterAttribute(name);

        _ = attribute.ShouldNotBeNull();
        attribute.Name.ShouldBe(name);
        attribute.FullName.ShouldBe($"{name}FullName");
        attribute.ResourceType.ShouldBe(typeof(CharacterNames));
        attribute.Index.ShouldBe(0);
    }

    [TestMethod]
    public void CharacterAttributeTestIndex()
    {
        var name = "Reimu";
        var index = 1;
        var attribute = new CharacterAttribute(name, index);

        _ = attribute.ShouldNotBeNull();
        attribute.Name.ShouldBe(name);
        attribute.FullName.ShouldBe($"{name}FullName");
        attribute.ResourceType.ShouldBe(typeof(CharacterNames));
        attribute.Index.ShouldBe(1);
    }

    [TestMethod]
    public void CharacterAttributeTestNull()
    {
        _ = Should.Throw<ArgumentNullException>(() => new CharacterAttribute(null!));
    }

    [TestMethod]
    public void CharacterAttributeTestEmpty()
    {
        _ = Should.Throw<ArgumentException>(() => new CharacterAttribute(string.Empty));
    }

    [TestMethod]
    public void GetLocalizedNameTest()
    {
        using var backup = TestHelper.BackupCultureInfo();

        var name = "Marisa";
        var attribute = new CharacterAttribute(name);

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        attribute.GetLocalizedName().ShouldBe("魔理沙");

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        attribute.GetLocalizedName().ShouldBe("Marisa");
    }

    [TestMethod]
    public void GetLocalizedNameTestUnregistered()
    {
        using var backup = TestHelper.BackupCultureInfo();

        var name = "Rinnosuke";
        var attribute = new CharacterAttribute(name);

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        attribute.GetLocalizedName().ShouldBe("Rinnosuke");

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        attribute.GetLocalizedName().ShouldBe("Rinnosuke");
    }

    [TestMethod]
    public void GetLocalizedFullNameTest()
    {
        using var backup = TestHelper.BackupCultureInfo();

        var name = "Marisa";
        var attribute = new CharacterAttribute(name);

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        attribute.GetLocalizedFullName().ShouldBe("霧雨 魔理沙");

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        attribute.GetLocalizedFullName().ShouldBe("Kirisame Marisa");
    }

    [TestMethod]
    public void GetLocalizedFullNameTestUnregistered()
    {
        using var backup = TestHelper.BackupCultureInfo();

        var name = "Rinnosuke";
        var attribute = new CharacterAttribute(name);

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        attribute.GetLocalizedFullName().ShouldBe("RinnosukeFullName");

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        attribute.GetLocalizedFullName().ShouldBe("RinnosukeFullName");
    }
}
