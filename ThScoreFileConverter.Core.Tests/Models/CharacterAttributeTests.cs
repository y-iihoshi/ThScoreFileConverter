using System;
using System.Globalization;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Resources;
using ThScoreFileConverter.Core.Tests.UnitTesting;

namespace ThScoreFileConverter.Core.Tests.Models;

[TestClass]
public class CharacterAttributeTests
{
    [TestMethod]
    public void CharacterAttributeTestOneArg()
    {
        var name = "Reimu";
        var attribute = new CharacterAttribute(name);

        Assert.IsNotNull(attribute);
        Assert.AreEqual(name, attribute.Name);
        Assert.AreEqual($"{name}FullName", attribute.FullName);
        Assert.AreEqual(typeof(CharacterNames), attribute.ResourceType);
    }

    [TestMethod]
    public void CharacterAttributeTestTwoArgs()
    {
        var name = "Reimu";
        var fullName = "HakureiReimu";
        var attribute = new CharacterAttribute(name, fullName);

        Assert.IsNotNull(attribute);
        Assert.AreEqual(name, attribute.Name);
        Assert.AreEqual(fullName, attribute.FullName);
        Assert.AreEqual(typeof(CharacterNames), attribute.ResourceType);
    }

    [TestMethod]
    public void CharacterAttributeTestThreeArgs()
    {
        var name = "Reimu";
        var fullName = "HakureiReimu";
        var resourceType = typeof(ExceptionMessages);
        var attribute = new CharacterAttribute(name, fullName, resourceType);

        Assert.IsNotNull(attribute);
        Assert.AreEqual(name, attribute.Name);
        Assert.AreEqual(fullName, attribute.FullName);
        Assert.AreEqual(resourceType, attribute.ResourceType);
    }

    [TestMethod]
    public void CharacterAttributeTestNullArgs()
    {
        var name = "Reimu";
        var fullName = "HakureiReimu";

        _ = Assert.ThrowsException<ArgumentNullException>(() => new CharacterAttribute(null!));
        _ = Assert.ThrowsException<ArgumentNullException>(() => new CharacterAttribute(name, null!));
        _ = Assert.ThrowsException<ArgumentNullException>(() => new CharacterAttribute(name, fullName, null!));
    }

    [TestMethod]
    public void CharacterAttributeTestEmptyArgs()
    {
        var name = "Reimu";

        _ = Assert.ThrowsException<ArgumentException>(() => new CharacterAttribute(string.Empty));
        _ = Assert.ThrowsException<ArgumentException>(() => new CharacterAttribute(name, string.Empty));
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
