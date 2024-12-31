using System;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverter.Tests.Models;

[TestClass]
public class EnumShortNameParserTests
{
    public enum Difficulty
    {
        [EnumAltName("E")] Easy,
        [EnumAltName("N")] Normal,
        [EnumAltName("H")] Hard,
        [EnumAltName("L")] Lunatic,
        [EnumAltName("X")] Extra,
    }

    [TestMethod]
    public void PatternTest()
    {
        var parser = new EnumShortNameParser<Difficulty>();
        Assert.AreEqual("E|N|H|L|X", parser.Pattern);
    }

    [TestMethod]
    public void PatternTestNoShortName()
    {
        var parser = new EnumShortNameParser<DayOfWeek>();
        Assert.AreEqual(string.Empty, parser.Pattern);
    }

    [TestMethod]
    public void ParseTest()
    {
        var parser = new EnumShortNameParser<Difficulty>();
        Assert.AreEqual(Difficulty.Easy, parser.Parse("E"));
        Assert.AreEqual(Difficulty.Normal, parser.Parse("N"));
        Assert.AreEqual(Difficulty.Hard, parser.Parse("H"));
        Assert.AreEqual(Difficulty.Lunatic, parser.Parse("L"));
        Assert.AreEqual(Difficulty.Extra, parser.Parse("X"));
    }

    [TestMethod]
    public void ParseTestMismatchedCase()
    {
        var parser = new EnumShortNameParser<Difficulty>();
        Assert.AreEqual(Difficulty.Easy, parser.Parse("e"));
        Assert.AreEqual(Difficulty.Normal, parser.Parse("n"));
        Assert.AreEqual(Difficulty.Hard, parser.Parse("h"));
        Assert.AreEqual(Difficulty.Lunatic, parser.Parse("l"));
        Assert.AreEqual(Difficulty.Extra, parser.Parse("x"));
    }

    [TestMethod]
    public void ParseTestEmpty()
    {
        var parser = new EnumShortNameParser<Difficulty>();
        _ = Assert.ThrowsException<InvalidOperationException>(() => parser.Parse(string.Empty));
    }

    [TestMethod]
    public void ParseTestUnknown()
    {
        var parser = new EnumShortNameParser<Difficulty>();
        _ = Assert.ThrowsException<InvalidOperationException>(() => parser.Parse("A"));
    }

    [TestMethod]
    public void ParseRegexTest()
    {
        var parser = new EnumShortNameParser<Difficulty>();

        var pattern = $"Level: ({parser.Pattern}), ({parser.Pattern})";
        var evaluator = new MatchEvaluator(match =>
        {
            var level1 = parser.Parse(match.Groups[1]);
            var level2 = parser.Parse(match.Groups[2]);
            return $"Lv1: {level1}, Lv2: {level2}";
        });

        var replaced = Regex.Replace("Level: E, X", pattern, evaluator);

        Assert.AreEqual("Lv1: Easy, Lv2: Extra", replaced);
    }

    [TestMethod]
    public void ParseRegexTestNull()
    {
        var parser = new EnumShortNameParser<Difficulty>();

        var pattern = $"Level: ({parser.Pattern})";
        var evaluator = new MatchEvaluator(match =>
        {
            var level = parser.Parse((Group)null!);
            return $"Lv: {level}";
        });

        _ = Assert.ThrowsException<ArgumentNullException>(() => Regex.Replace("Level: E", pattern, evaluator));
    }
}
