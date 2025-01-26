using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models;

namespace ThScoreFileConverter.Core.Tests.Models;

[TestClass]
public class IntegerParserTests
{
    [TestMethod]
    public void ParseTestDefault()
    {
        var parser = new IntegerParser();

        var pattern = $@"var (\w+) = ({parser.Pattern});";
        var evaluator = new MatchEvaluator(match =>
        {
            var name = match.Groups[1].Value;
            var value = parser.Parse(match.Groups[2]);
            return $"{name}^2 == {value * value}";
        });

        var pairs = new[]
        {
            ("var a = 1;",   "a^2 == 1"),
            ("var b = 23;",  "b^2 == 529"),
            ("var c = 456;", "c^2 == 207936"),
        };

        foreach (var pair in pairs)
        {
            Regex.Replace(pair.Item1, pattern, evaluator).ShouldBe(pair.Item2);
        }
    }

    [TestMethod]
    public void ParseTest()
    {
        var parser = new IntegerParser(@"[2-4]");

        var pattern = $@"var (\w+) = ({parser.Pattern});";
        var evaluator = new MatchEvaluator(match =>
        {
            var name = match.Groups[1].Value;
            var value = parser.Parse(match.Groups[2]);
            return $"{name}^2 == {value * value}";
        });

        var pairs = new[]
        {
            ("var a = 1;", "var a = 1;"),
            ("var b = 2;", "b^2 == 4"),
            ("var c = 3;", "c^2 == 9"),
            ("var d = 4;", "d^2 == 16"),
            ("var e = 5;", "var e = 5;"),
        };

        foreach (var pair in pairs)
        {
            Regex.Replace(pair.Item1, pattern, evaluator).ShouldBe(pair.Item2);
        }
    }
}
