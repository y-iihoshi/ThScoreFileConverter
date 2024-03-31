using System;
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
}
