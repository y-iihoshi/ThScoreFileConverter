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

        Assert.IsNotNull(attribute);
        Assert.AreEqual(name, attribute.Name);
        Assert.AreEqual($"{name}FullName", attribute.FullName);
        Assert.AreEqual(typeof(CharacterNames), attribute.ResourceType);
        Assert.AreEqual(0, attribute.Index);
    }

    [TestMethod]
    public void CharacterAttributeTestIndex()
    {
        var name = "Reimu";
        var index = 1;
        var attribute = new CharacterAttribute(name, index);

        Assert.IsNotNull(attribute);
        Assert.AreEqual(name, attribute.Name);
        Assert.AreEqual($"{name}FullName", attribute.FullName);
        Assert.AreEqual(typeof(CharacterNames), attribute.ResourceType);
        Assert.AreEqual(1, attribute.Index);
    }

    [TestMethod]
    public void CharacterAttributeTestNull()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => new CharacterAttribute(null!));
    }

    [TestMethod]
    public void CharacterAttributeTestEmpty()
    {
        _ = Assert.ThrowsException<ArgumentException>(() => new CharacterAttribute(string.Empty));
    }

    [TestMethod]
    public void GetLocalizedNameTest()
    {
        using var backup = TestHelper.BackupCultureInfo();

        var name = "Marisa";
        var attribute = new CharacterAttribute(name);

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        Assert.AreEqual("魔理沙", attribute.GetLocalizedName());

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        Assert.AreEqual("Marisa", attribute.GetLocalizedName());
    }

    [TestMethod]
    public void GetLocalizedNameTestUnregistered()
    {
        using var backup = TestHelper.BackupCultureInfo();

        var name = "Rinnosuke";
        var attribute = new CharacterAttribute(name);

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        Assert.AreEqual("Rinnosuke", attribute.GetLocalizedName());

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        Assert.AreEqual("Rinnosuke", attribute.GetLocalizedName());
    }

    [TestMethod]
    public void GetLocalizedFullNameTest()
    {
        using var backup = TestHelper.BackupCultureInfo();

        var name = "Marisa";
        var attribute = new CharacterAttribute(name);

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        Assert.AreEqual("霧雨 魔理沙", attribute.GetLocalizedFullName());

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        Assert.AreEqual("Kirisame Marisa", attribute.GetLocalizedFullName());
    }

    [TestMethod]
    public void GetLocalizedFullNameTestUnregistered()
    {
        using var backup = TestHelper.BackupCultureInfo();

        var name = "Rinnosuke";
        var attribute = new CharacterAttribute(name);

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        Assert.AreEqual("RinnosukeFullName", attribute.GetLocalizedFullName());

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        Assert.AreEqual("RinnosukeFullName", attribute.GetLocalizedFullName());
    }
}
