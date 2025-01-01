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

        var power_a_2 = Regex.Replace("var a = 1;", pattern, evaluator);
        Assert.AreEqual("a^2 == 1", power_a_2);
        var power_b_2 = Regex.Replace("var b = 23;", pattern, evaluator);
        Assert.AreEqual("b^2 == 529", power_b_2);
        var power_c_2 = Regex.Replace("var c = 456;", pattern, evaluator);
        Assert.AreEqual("c^2 == 207936", power_c_2);
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

        var power_a_2 = Regex.Replace("var a = 1;", pattern, evaluator);
        Assert.AreEqual("var a = 1;", power_a_2);
        var power_b_2 = Regex.Replace("var b = 2;", pattern, evaluator);
        Assert.AreEqual("b^2 == 4", power_b_2);
        var power_c_2 = Regex.Replace("var c = 3;", pattern, evaluator);
        Assert.AreEqual("c^2 == 9", power_c_2);
        var power_d_2 = Regex.Replace("var d = 4;", pattern, evaluator);
        Assert.AreEqual("d^2 == 16", power_d_2);
        var power_e_2 = Regex.Replace("var e = 5;", pattern, evaluator);
        Assert.AreEqual("var e = 5;", power_e_2);
    }
}
