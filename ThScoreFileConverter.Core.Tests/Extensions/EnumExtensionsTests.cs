using System.ComponentModel.DataAnnotations;
using System.Globalization;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th06;

namespace ThScoreFileConverter.Core.Tests.Extensions;

public enum Protagonist
{
    [Pattern("RM")]
    [Display(Name = "博麗 霊夢", ShortName = "霊夢")]
    [Character(nameof(Reimu))]
    Reimu,

    [Pattern("MR")]
    [Display(Name = "霧雨 魔理沙", ShortName = "魔理沙")]
    [Character(nameof(Marisa))]
    Marisa,
}

public enum UnnamedCharacter
{
    大妖精,
    小悪魔,
    名無しの本読み妖怪,
}

public enum Sisters
{
    [Character("Remilia")]
    [Character("Flandre", 1)]
    Scarlet,

    [Character("Lunasa")]
    [Character("Merlin", 1)]
    [Character("Lyrica", 2)]
    Prismriver,
}

[TestClass]
public class EnumExtensionsTests
{
    [TestMethod]
    public void ToNameTest()
    {
        DayOfWeek.Sunday.ToName().ShouldBe(nameof(DayOfWeek.Sunday));
        Protagonist.Reimu.ToName().ShouldBe(nameof(Protagonist.Reimu));
        Protagonist.Marisa.ToName().ShouldBe(nameof(Protagonist.Marisa));
    }

    [TestMethod]
    public void ToPatternTest()
    {
        DayOfWeek.Sunday.ToPattern().ShouldBeEmpty();
        Protagonist.Reimu.ToPattern().ShouldBe("RM");
        Protagonist.Marisa.ToPattern().ShouldBe("MR");
    }

    [TestMethod]
    public void ToDisplayNameTest()
    {
        DayOfWeek.Sunday.ToDisplayName().ShouldBe(nameof(DayOfWeek.Sunday));
        Protagonist.Reimu.ToDisplayName().ShouldBe("博麗 霊夢");
        Protagonist.Marisa.ToDisplayName().ShouldBe("霧雨 魔理沙");
    }

    [TestMethod]
    public void ToDisplayShortNameTest()
    {
        DayOfWeek.Sunday.ToDisplayShortName().ShouldBe(nameof(DayOfWeek.Sunday));
        Protagonist.Reimu.ToDisplayShortName().ShouldBe("霊夢");
        Protagonist.Marisa.ToDisplayShortName().ShouldBe("魔理沙");
    }

    [TestMethod]
    public void ToCharaNameTest()
    {
        using var backup = TestHelper.BackupCultureInfo();

        DayOfWeek.Sunday.ToCharaName().ShouldBeEmpty();

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        Protagonist.Reimu.ToCharaName().ShouldBe("霊夢");
        Protagonist.Marisa.ToCharaName().ShouldBe("魔理沙");

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        Protagonist.Reimu.ToCharaName().ShouldBe("Reimu");
        Protagonist.Marisa.ToCharaName().ShouldBe("Marisa");
    }

    [TestMethod]
    public void ToCharaFullNameTest()
    {
        using var backup = TestHelper.BackupCultureInfo();

        DayOfWeek.Sunday.ToCharaFullName().ShouldBeEmpty();

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        Protagonist.Reimu.ToCharaFullName().ShouldBe("博麗 霊夢");
        Protagonist.Marisa.ToCharaFullName().ShouldBe("霧雨 魔理沙");

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        Protagonist.Reimu.ToCharaFullName().ShouldBe("Hakurei Reimu");
        Protagonist.Marisa.ToCharaFullName().ShouldBe("Kirisame Marisa");
    }

    [TestMethod]
    public void ToCharaNameTestMultiple()
    {
        using var backup = TestHelper.BackupCultureInfo();

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        Sisters.Scarlet.ToCharaName().ShouldBe("レミリア");
        Sisters.Scarlet.ToCharaName(1).ShouldBe("フランドール");
        Sisters.Prismriver.ToCharaName().ShouldBe("ルナサ");
        Sisters.Prismriver.ToCharaName(1).ShouldBe("メルラン");
        Sisters.Prismriver.ToCharaName(2).ShouldBe("リリカ");

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        Sisters.Scarlet.ToCharaName().ShouldBe("Remilia");
        Sisters.Scarlet.ToCharaName(1).ShouldBe("Flandre");
        Sisters.Prismriver.ToCharaName().ShouldBe("Lunasa");
        Sisters.Prismriver.ToCharaName(1).ShouldBe("Merlin");
        Sisters.Prismriver.ToCharaName(2).ShouldBe("Lyrica");
    }

    [TestMethod]
    public void ToCharaFullNameTestMultiple()
    {
        using var backup = TestHelper.BackupCultureInfo();

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        Sisters.Scarlet.ToCharaFullName().ShouldBe("レミリア・スカーレット");
        Sisters.Scarlet.ToCharaFullName(1).ShouldBe("フランドール・スカーレット");
        Sisters.Prismriver.ToCharaFullName().ShouldBe("ルナサ・プリズムリバー");
        Sisters.Prismriver.ToCharaFullName(1).ShouldBe("メルラン・プリズムリバー");
        Sisters.Prismriver.ToCharaFullName(2).ShouldBe("リリカ・プリズムリバー");

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        Sisters.Scarlet.ToCharaFullName().ShouldBe("Remilia Scarlet");
        Sisters.Scarlet.ToCharaFullName(1).ShouldBe("Flandre Scarlet");
        Sisters.Prismriver.ToCharaFullName().ShouldBe("Lunasa Prismriver");
        Sisters.Prismriver.ToCharaFullName(1).ShouldBe("Merlin Prismriver");
        Sisters.Prismriver.ToCharaFullName(2).ShouldBe("Lyrica Prismriver");
    }

    [TestMethod]
    public void ToShotTypeNameTest()
    {
        using var backup = TestHelper.BackupCultureInfo();

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        DayOfWeek.Sunday.ToShotTypeName().ShouldBeEmpty();
        Chara.ReimuA.ToShotTypeName().ShouldBe("霊");
        Chara.MarisaA.ToShotTypeName().ShouldBe("魔");

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        DayOfWeek.Sunday.ToShotTypeName().ShouldBeEmpty();
        Chara.ReimuA.ToShotTypeName().ShouldBe("Spirit");
        Chara.MarisaA.ToShotTypeName().ShouldBe("Magic");
    }

    [TestMethod]
    public void ToShotTypeFullNameTest()
    {
        using var backup = TestHelper.BackupCultureInfo();

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        DayOfWeek.Sunday.ToShotTypeFullName().ShouldBeEmpty();
        Chara.ReimuA.ToShotTypeFullName().ShouldBe("霊符");
        Chara.MarisaA.ToShotTypeFullName().ShouldBe("魔符");

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        DayOfWeek.Sunday.ToShotTypeFullName().ShouldBeEmpty();
        Chara.ReimuA.ToShotTypeFullName().ShouldBe("Spirit Sign");
        Chara.MarisaA.ToShotTypeFullName().ShouldBe("Magic Sign");
    }
}
