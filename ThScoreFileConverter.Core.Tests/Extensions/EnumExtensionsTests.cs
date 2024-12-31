﻿using System;
using System.ComponentModel.DataAnnotations;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Models;

namespace ThScoreFileConverter.Core.Tests.Extensions;

public enum Protagonist
{
    [EnumAltName("RM", LongName = "博麗 霊夢"), Pattern("RM"), Display(Name = "Hakurei Reimu")] Reimu,
    [EnumAltName("MR", LongName = "霧雨 魔理沙"), Pattern("MR"), Display(Name = "Kirisame Marisa")] Marisa,
}

public enum UnnamedCharacter
{
    [EnumAltName("Dai")] 大妖精,
    [EnumAltName("Koa")] 小悪魔,
    [EnumAltName("Tokiko")] 名無しの本読み妖怪,
}

[TestClass]
public class EnumExtensionsTests
{
    [TestMethod]
    public void ToShortNameTest()
    {
        Assert.AreEqual(string.Empty, DayOfWeek.Sunday.ToShortName());
        Assert.AreEqual("RM", Protagonist.Reimu.ToShortName());
        Assert.AreEqual("MR", Protagonist.Marisa.ToShortName());
        Assert.AreEqual("Dai", UnnamedCharacter.大妖精.ToShortName());
        Assert.AreEqual("Koa", UnnamedCharacter.小悪魔.ToShortName());
        Assert.AreEqual("Tokiko", UnnamedCharacter.名無しの本読み妖怪.ToShortName());
    }

    [TestMethod]
    public void ToLongNameTest()
    {
        Assert.AreEqual(string.Empty, DayOfWeek.Sunday.ToLongName());
        Assert.AreEqual("博麗 霊夢", Protagonist.Reimu.ToLongName());
        Assert.AreEqual("霧雨 魔理沙", Protagonist.Marisa.ToLongName());
        Assert.AreEqual(string.Empty, UnnamedCharacter.大妖精.ToLongName());
        Assert.AreEqual(string.Empty, UnnamedCharacter.小悪魔.ToLongName());
        Assert.AreEqual(string.Empty, UnnamedCharacter.名無しの本読み妖怪.ToLongName());
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
        Assert.AreEqual("Hakurei Reimu", Protagonist.Reimu.ToDisplayName());
        Assert.AreEqual("Kirisame Marisa", Protagonist.Marisa.ToDisplayName());
    }
}
