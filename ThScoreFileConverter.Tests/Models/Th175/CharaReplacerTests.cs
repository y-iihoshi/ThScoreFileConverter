using System.Collections.Generic;
using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th175;
using ThScoreFileConverter.Models.Th175;
using INumberFormatter = ThScoreFileConverter.Models.INumberFormatter;

namespace ThScoreFileConverter.Tests.Models.Th175;

[TestClass]
public class CharaReplacerTests
{
    internal static IReadOnlyDictionary<Chara, int> UseCounts { get; } =
        new Dictionary<Chara, int>
        {
            { Chara.Reimu, 98 },
            { Chara.Marisa, 76 },
        };

    internal static IReadOnlyDictionary<Chara, int> RetireCounts { get; } =
        new Dictionary<Chara, int>
        {
            { Chara.Reimu, 76 },
            { Chara.Marisa, 54 },
        };

    internal static IReadOnlyDictionary<Chara, int> ClearCounts { get; } =
        new Dictionary<Chara, int>
        {
            { Chara.Reimu, 54 },
            { Chara.Marisa, 32 },
        };

    internal static IReadOnlyDictionary<Chara, int> PerfectClearCounts { get; } =
        new Dictionary<Chara, int>
        {
            { Chara.Reimu, 32 },
            { Chara.Marisa, 10 },
        };

    private static INumberFormatter MockNumberFormatter()
    {
        // NOTE: NSubstitute v5.0.0 has no substitute for Moq's It.IsAny<It.IsValueType>.
        var mock = Substitute.For<INumberFormatter>();
        _ = mock.FormatNumber(Arg.Any<int>()).Returns(callInfo => $"invoked: {(int)callInfo[0]}");
        return mock;
    }

    [TestMethod]
    public void CharaReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CharaReplacerTestEmptyUseCounts()
    {
        var counts = ImmutableDictionary<Chara, int>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(counts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CharaReplacerTestEmptyRetireCounts()
    {
        var counts = ImmutableDictionary<Chara, int>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(UseCounts, counts, ClearCounts, PerfectClearCounts, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CharaReplacerTestEmptyClearCounts()
    {
        var counts = ImmutableDictionary<Chara, int>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(UseCounts, RetireCounts, counts, PerfectClearCounts, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CharaReplacerTestEmptyPerfectClearCounts()
    {
        var counts = ImmutableDictionary<Chara, int>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, counts, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestUseCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);

        Assert.AreEqual("invoked: 98", replacer.Replace("%T175CHRRM1"));
    }

    [TestMethod]
    public void ReplaceTestRetireCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);

        Assert.AreEqual("invoked: 76", replacer.Replace("%T175CHRRM2"));
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);

        Assert.AreEqual("invoked: 54", replacer.Replace("%T175CHRRM3"));
    }

    [TestMethod]
    public void ReplaceTestPerfectClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);

        Assert.AreEqual("invoked: 32", replacer.Replace("%T175CHRRM4"));
    }

    [TestMethod]
    public void ReplaceTestTotalUseCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);

        Assert.AreEqual("invoked: 174", replacer.Replace("%T175CHRTL1"));
    }

    [TestMethod]
    public void ReplaceTestTotalRetireCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);

        Assert.AreEqual("invoked: 130", replacer.Replace("%T175CHRTL2"));
    }

    [TestMethod]
    public void ReplaceTestTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);

        Assert.AreEqual("invoked: 86", replacer.Replace("%T175CHRTL3"));
    }

    [TestMethod]
    public void ReplaceTestTotalPerfectClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);

        Assert.AreEqual("invoked: 42", replacer.Replace("%T175CHRTL4"));
    }

    [TestMethod]
    public void ReplaceTestEmptyUseCounts()
    {
        var counts = ImmutableDictionary<Chara, int>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(counts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T175CHRRM1"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRetireCounts()
    {
        var counts = ImmutableDictionary<Chara, int>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(UseCounts, counts, ClearCounts, PerfectClearCounts, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T175CHRRM2"));
    }

    [TestMethod]
    public void ReplaceTestEmptyClearCounts()
    {
        var counts = ImmutableDictionary<Chara, int>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(UseCounts, RetireCounts, counts, PerfectClearCounts, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T175CHRRM3"));
    }

    [TestMethod]
    public void ReplaceTestEmptyPerfectClearCounts()
    {
        var counts = ImmutableDictionary<Chara, int>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, counts, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T175CHRRM4"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentUseCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T175CHRKN1"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentRetireCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T175CHRKN2"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T175CHRKN3"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentPerfectClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T175CHRKN4"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);
        Assert.AreEqual("%T175XXXRM1", replacer.Replace("%T175XXXRM1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);
        Assert.AreEqual("%T175CHRXX1", replacer.Replace("%T175CHRXX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);
        Assert.AreEqual("%T175CHRRMX", replacer.Replace("%T175CHRRMX"));
    }
}
