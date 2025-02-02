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
        parser.Pattern.ShouldBe("RM|MR");
    }

    [TestMethod]
    public void PatternTestNoPattern()
    {
        var parser = new EnumPatternParser<DayOfWeek>();
        parser.Pattern.ShouldBeEmpty();
    }

    [TestMethod]
    public void ParseTest()
    {
        var parser = new EnumPatternParser<Protagonist>();
        parser.Parse("RM").ShouldBe(Protagonist.Reimu);
        parser.Parse("MR").ShouldBe(Protagonist.Marisa);
    }

    [TestMethod]
    public void ParseTestMismatchedCase()
    {
        var parser = new EnumPatternParser<Protagonist>();
        parser.Parse("rm").ShouldBe(Protagonist.Reimu);
        parser.Parse("mr").ShouldBe(Protagonist.Marisa);
    }

    [TestMethod]
    public void ParseTestEmpty()
    {
        var parser = new EnumPatternParser<Protagonist>();
        _ = Should.Throw<InvalidOperationException>(() => parser.Parse(string.Empty));
    }

    [TestMethod]
    public void ParseTestUnknown()
    {
        var parser = new EnumPatternParser<Protagonist>();
        _ = Should.Throw<InvalidOperationException>(() => parser.Parse("A"));
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

        Regex.Replace("Chara: RM, MR", pattern, evaluator).ShouldBe("Ch1: Reimu, Ch2: Marisa");
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

        _ = Should.Throw<ArgumentNullException>(() => Regex.Replace("Chara: RM", pattern, evaluator));
    }
}
