using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Tests.Extensions;

namespace ThScoreFileConverter.Core.Tests.Models;

[TestClass]
public class EnumPatternParserTests
{
    [TestMethod]
    public void PatternTest()
    {
        var parser = new EnumPatternParser<Protagonist>();
        Assert.AreEqual("RM|MR", parser.Pattern);
    }

    [TestMethod]
    public void PatternTestNoPattern()
    {
        var parser = new EnumPatternParser<DayOfWeek>();
        Assert.AreEqual(string.Empty, parser.Pattern);
    }

    [TestMethod]
    public void ParseTest()
    {
        var parser = new EnumPatternParser<Protagonist>();
        Assert.AreEqual(Protagonist.Reimu, parser.Parse("RM"));
        Assert.AreEqual(Protagonist.Marisa, parser.Parse("MR"));
    }

    [TestMethod]
    public void ParseTestMismatchedCase()
    {
        var parser = new EnumPatternParser<Protagonist>();
        Assert.AreEqual(Protagonist.Reimu, parser.Parse("rm"));
        Assert.AreEqual(Protagonist.Marisa, parser.Parse("mr"));
    }

    [TestMethod]
    public void ParseTestEmpty()
    {
        var parser = new EnumPatternParser<Protagonist>();
        _ = Assert.ThrowsException<InvalidOperationException>(() => parser.Parse(string.Empty));
    }

    [TestMethod]
    public void ParseTestUnknown()
    {
        var parser = new EnumPatternParser<Protagonist>();
        _ = Assert.ThrowsException<InvalidOperationException>(() => parser.Parse("A"));
    }

    [TestMethod]
    public void ParseRegexTest()
    {
        var parser = new EnumPatternParser<Protagonist>();

        var pattern = $"Chara: ({parser.Pattern}), ({parser.Pattern})";
        var evaluator = new MatchEvaluator(match =>
        {
            var chara1 = parser.Parse(match.Groups[1]);
            var chara2 = parser.Parse(match.Groups[2]);
            return $"Ch1: {chara1}, Ch2: {chara2}";
        });

        var replaced = Regex.Replace("Chara: RM, MR", pattern, evaluator);

        Assert.AreEqual("Ch1: Reimu, Ch2: Marisa", replaced);
    }

    [TestMethod]
    public void ParseRegexTestNull()
    {
        var parser = new EnumPatternParser<Protagonist>();

        var pattern = $"Chara: ({parser.Pattern})";
        var evaluator = new MatchEvaluator(match =>
        {
            var chara = parser.Parse((Group)null!);
            return $"Ch: {chara}";
        });

        _ = Assert.ThrowsException<ArgumentNullException>(() => Regex.Replace("Chara: RM", pattern, evaluator));
    }
}
