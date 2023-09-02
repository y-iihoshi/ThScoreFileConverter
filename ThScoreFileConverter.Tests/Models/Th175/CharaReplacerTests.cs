using System.Collections.Generic;
using System.Collections.Immutable;
using ThScoreFileConverter.Core.Models.Th175;
using ThScoreFileConverter.Models.Th175;

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

    [TestMethod]
    public void CharaReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CharaReplacerTestEmptyUseCounts()
    {
        var counts = ImmutableDictionary<Chara, int>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(counts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CharaReplacerTestEmptyRetireCounts()
    {
        var counts = ImmutableDictionary<Chara, int>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(UseCounts, counts, ClearCounts, PerfectClearCounts, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CharaReplacerTestEmptyClearCounts()
    {
        var counts = ImmutableDictionary<Chara, int>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(UseCounts, RetireCounts, counts, PerfectClearCounts, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CharaReplacerTestEmptyPerfectClearCounts()
    {
        var counts = ImmutableDictionary<Chara, int>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, counts, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestUseCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);

        Assert.AreEqual("invoked: 98", replacer.Replace("%T175CHRRM1"));
    }

    [TestMethod]
    public void ReplaceTestRetireCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);

        Assert.AreEqual("invoked: 76", replacer.Replace("%T175CHRRM2"));
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);

        Assert.AreEqual("invoked: 54", replacer.Replace("%T175CHRRM3"));
    }

    [TestMethod]
    public void ReplaceTestPerfectClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);

        Assert.AreEqual("invoked: 32", replacer.Replace("%T175CHRRM4"));
    }

    [TestMethod]
    public void ReplaceTestTotalUseCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);

        Assert.AreEqual("invoked: 174", replacer.Replace("%T175CHRTL1"));
    }

    [TestMethod]
    public void ReplaceTestTotalRetireCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);

        Assert.AreEqual("invoked: 130", replacer.Replace("%T175CHRTL2"));
    }

    [TestMethod]
    public void ReplaceTestTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);

        Assert.AreEqual("invoked: 86", replacer.Replace("%T175CHRTL3"));
    }

    [TestMethod]
    public void ReplaceTestTotalPerfectClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);

        Assert.AreEqual("invoked: 42", replacer.Replace("%T175CHRTL4"));
    }

    [TestMethod]
    public void ReplaceTestEmptyUseCounts()
    {
        var counts = ImmutableDictionary<Chara, int>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(counts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T175CHRRM1"));
    }

    [TestMethod]
    public void ReplaceTestEmptyRetireCounts()
    {
        var counts = ImmutableDictionary<Chara, int>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(UseCounts, counts, ClearCounts, PerfectClearCounts, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T175CHRRM2"));
    }

    [TestMethod]
    public void ReplaceTestEmptyClearCounts()
    {
        var counts = ImmutableDictionary<Chara, int>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(UseCounts, RetireCounts, counts, PerfectClearCounts, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T175CHRRM3"));
    }

    [TestMethod]
    public void ReplaceTestEmptyPerfectClearCounts()
    {
        var counts = ImmutableDictionary<Chara, int>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, counts, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T175CHRRM4"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentUseCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T175CHRKN1"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentRetireCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T175CHRKN2"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T175CHRKN3"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentPerfectClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T175CHRKN4"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);
        Assert.AreEqual("%T175XXXRM1", replacer.Replace("%T175XXXRM1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);
        Assert.AreEqual("%T175CHRXX1", replacer.Replace("%T175CHRXX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(UseCounts, RetireCounts, ClearCounts, PerfectClearCounts, formatterMock);
        Assert.AreEqual("%T175CHRRMX", replacer.Replace("%T175CHRRMX"));
    }
}
