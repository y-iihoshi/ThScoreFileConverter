using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models.Th143;

namespace ThScoreFileConverter.Core.Tests.Models.Th143;

[TestClass]
public class SceneParserTests
{
    [TestMethod]
    public void ParseTest()
    {
        var parser = new SceneParser();

        var pattern = $@"var (\w+) = ({parser.Pattern});";
        var evaluator = new MatchEvaluator(match =>
        {
            var name = match.Groups[1].Value;
            var value = parser.Parse(match.Groups[2]);
            return $"{name}^2 == {value * value}";
        });

        var pairs = new[]
        {
            ("var a0 = 0;", "a0^2 == 100"),
            ("var a1 = 1;", "a1^2 == 1"),
            ("var a2 = 2;", "a2^2 == 4"),
            ("var a3 = 3;", "a3^2 == 9"),
            ("var a4 = 4;", "a4^2 == 16"),
            ("var a5 = 5;", "a5^2 == 25"),
            ("var a6 = 6;", "a6^2 == 36"),
            ("var a7 = 7;", "a7^2 == 49"),
            ("var a8 = 8;", "a8^2 == 64"),
            ("var a9 = 9;", "a9^2 == 81"),
        };

        foreach (var pair in pairs)
        {
            var replaced = Regex.Replace(pair.Item1, pattern, evaluator);
            Assert.AreEqual(pair.Item2, replaced);
        }
    }
}
