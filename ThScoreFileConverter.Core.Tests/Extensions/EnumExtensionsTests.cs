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
        Assert.AreEqual(nameof(DayOfWeek.Sunday), DayOfWeek.Sunday.ToName());
        Assert.AreEqual(nameof(Protagonist.Reimu), Protagonist.Reimu.ToName());
        Assert.AreEqual(nameof(Protagonist.Marisa), Protagonist.Marisa.ToName());
    }

    [TestMethod]
    public void ToPatternTest()
    {
        Assert.AreEqual(string.Empty, DayOfWeek.Sunday.ToPattern());
        Assert.AreEqual("RM", Protagonist.Reimu.ToPattern());
        Assert.AreEqual("MR", Protagonist.Marisa.ToPattern());
    }

    [TestMethod]
    public void ToDisplayNameTest()
    {
        Assert.AreEqual(nameof(DayOfWeek.Sunday), DayOfWeek.Sunday.ToDisplayName());
        Assert.AreEqual("博麗 霊夢", Protagonist.Reimu.ToDisplayName());
        Assert.AreEqual("霧雨 魔理沙", Protagonist.Marisa.ToDisplayName());
    }

    [TestMethod]
    public void ToDisplayShortNameTest()
    {
        Assert.AreEqual(nameof(DayOfWeek.Sunday), DayOfWeek.Sunday.ToDisplayShortName());
        Assert.AreEqual("霊夢", Protagonist.Reimu.ToDisplayShortName());
        Assert.AreEqual("魔理沙", Protagonist.Marisa.ToDisplayShortName());
    }

    [TestMethod]
    public void ToCharaNameTest()
    {
        using var backup = TestHelper.BackupCultureInfo();

        Assert.AreEqual(string.Empty, DayOfWeek.Sunday.ToCharaName());

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        Assert.AreEqual("霊夢", Protagonist.Reimu.ToCharaName());
        Assert.AreEqual("魔理沙", Protagonist.Marisa.ToCharaName());

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        Assert.AreEqual("Reimu", Protagonist.Reimu.ToCharaName());
        Assert.AreEqual("Marisa", Protagonist.Marisa.ToCharaName());
    }

    [TestMethod]
    public void ToCharaFullNameTest()
    {
        using var backup = TestHelper.BackupCultureInfo();

        Assert.AreEqual(string.Empty, DayOfWeek.Sunday.ToCharaFullName());

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        Assert.AreEqual("博麗 霊夢", Protagonist.Reimu.ToCharaFullName());
        Assert.AreEqual("霧雨 魔理沙", Protagonist.Marisa.ToCharaFullName());

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        Assert.AreEqual("Hakurei Reimu", Protagonist.Reimu.ToCharaFullName());
        Assert.AreEqual("Kirisame Marisa", Protagonist.Marisa.ToCharaFullName());
    }

    [TestMethod]
    public void ToCharaNameTestMultiple()
    {
        using var backup = TestHelper.BackupCultureInfo();

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        Assert.AreEqual("レミリア", Sisters.Scarlet.ToCharaName());
        Assert.AreEqual("フランドール", Sisters.Scarlet.ToCharaName(1));
        Assert.AreEqual("ルナサ", Sisters.Prismriver.ToCharaName());
        Assert.AreEqual("メルラン", Sisters.Prismriver.ToCharaName(1));
        Assert.AreEqual("リリカ", Sisters.Prismriver.ToCharaName(2));

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        Assert.AreEqual("Remilia", Sisters.Scarlet.ToCharaName());
        Assert.AreEqual("Flandre", Sisters.Scarlet.ToCharaName(1));
        Assert.AreEqual("Lunasa", Sisters.Prismriver.ToCharaName());
        Assert.AreEqual("Merlin", Sisters.Prismriver.ToCharaName(1));
        Assert.AreEqual("Lyrica", Sisters.Prismriver.ToCharaName(2));
    }

    [TestMethod]
    public void ToCharaFullNameTestMultiple()
    {
        using var backup = TestHelper.BackupCultureInfo();

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        Assert.AreEqual("レミリア・スカーレット", Sisters.Scarlet.ToCharaFullName());
        Assert.AreEqual("フランドール・スカーレット", Sisters.Scarlet.ToCharaFullName(1));
        Assert.AreEqual("ルナサ・プリズムリバー", Sisters.Prismriver.ToCharaFullName());
        Assert.AreEqual("メルラン・プリズムリバー", Sisters.Prismriver.ToCharaFullName(1));
        Assert.AreEqual("リリカ・プリズムリバー", Sisters.Prismriver.ToCharaFullName(2));

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        Assert.AreEqual("Remilia Scarlet", Sisters.Scarlet.ToCharaFullName());
        Assert.AreEqual("Flandre Scarlet", Sisters.Scarlet.ToCharaFullName(1));
        Assert.AreEqual("Lunasa Prismriver", Sisters.Prismriver.ToCharaFullName());
        Assert.AreEqual("Merlin Prismriver", Sisters.Prismriver.ToCharaFullName(1));
        Assert.AreEqual("Lyrica Prismriver", Sisters.Prismriver.ToCharaFullName(2));
    }

    [TestMethod]
    public void ToShotTypeNameTest()
    {
        using var backup = TestHelper.BackupCultureInfo();

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        Assert.AreEqual(string.Empty, DayOfWeek.Sunday.ToShotTypeName());
        Assert.AreEqual("霊", Chara.ReimuA.ToShotTypeName());
        Assert.AreEqual("魔", Chara.MarisaA.ToShotTypeName());

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        Assert.AreEqual(string.Empty, DayOfWeek.Sunday.ToShotTypeName());
        Assert.AreEqual("Spirit", Chara.ReimuA.ToShotTypeName());
        Assert.AreEqual("Magic", Chara.MarisaA.ToShotTypeName());
    }

    [TestMethod]
    public void ToShotTypeFullNameTest()
    {
        using var backup = TestHelper.BackupCultureInfo();

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
        Assert.AreEqual(string.Empty, DayOfWeek.Sunday.ToShotTypeFullName());
        Assert.AreEqual("霊符", Chara.ReimuA.ToShotTypeFullName());
        Assert.AreEqual("魔符", Chara.MarisaA.ToShotTypeFullName());

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        Assert.AreEqual(string.Empty, DayOfWeek.Sunday.ToShotTypeFullName());
        Assert.AreEqual("Spirit Sign", Chara.ReimuA.ToShotTypeFullName());
        Assert.AreEqual("Magic Sign", Chara.MarisaA.ToShotTypeFullName());
    }
}
