using System.Collections.Immutable;
using ThScoreFileConverter.Core.Models.Th145;
using ThScoreFileConverter.Models.Th145;

namespace ThScoreFileConverter.Tests.Models.Th145;

[TestClass]
public class ClearRankReplacerTests
{
    internal static IReadOnlyDictionary<Level, IReadOnlyDictionary<Chara, int>> ClearRanks { get; } =
        new Dictionary<Level, IReadOnlyDictionary<Chara, int>>
        {
            {
                Level.Hard,
                new Dictionary<Chara, int>
                {
                    { Chara.ReimuA, 3 },
                    { Chara.Marisa, 2 },
                    { Chara.IchirinUnzan, 1 },
                    { Chara.Byakuren, 0 },
                }
            },
        };

    [TestMethod]
    public void ClearRankReplacerTest()
    {
        var replacer = new ClearRankReplacer(ClearRanks);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ClearRankReplacerTestEmpty()
    {
        var clearRanks = ImmutableDictionary<Level, IReadOnlyDictionary<Chara, int>>.Empty;
        var replacer = new ClearRankReplacer(clearRanks);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ClearRankReplacerTestEmptyCharacters()
    {
        var clearRanks = new Dictionary<Level, IReadOnlyDictionary<Chara, int>>
        {
            { Level.Hard, ImmutableDictionary<Chara, int>.Empty },
        };

        var replacer = new ClearRankReplacer(clearRanks);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var replacer = new ClearRankReplacer(ClearRanks);
        replacer.Replace("%T145CLEARHRA").ShouldBe("Gold");
        replacer.Replace("%T145CLEARHMR").ShouldBe("Silver");
        replacer.Replace("%T145CLEARHIU").ShouldBe("Bronze");
        replacer.Replace("%T145CLEARHBY").ShouldBe("Not Clear");
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var replacer = new ClearRankReplacer(ClearRanks);
        replacer.Replace("%T145CLEARLMR").ShouldBe("Not Clear");
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var replacer = new ClearRankReplacer(ClearRanks);
        replacer.Replace("%T145CLEARHFT").ShouldBe("Not Clear");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var clearRanks = ImmutableDictionary<Level, IReadOnlyDictionary<Chara, int>>.Empty;
        var replacer = new ClearRankReplacer(clearRanks);
        replacer.Replace("%T145CLEARHMR").ShouldBe("Not Clear");
    }

    [TestMethod]
    public void ReplaceTestEmptyCharacters()
    {
        var clearRanks = new Dictionary<Level, IReadOnlyDictionary<Chara, int>>
        {
            { Level.Hard, ImmutableDictionary<Chara, int>.Empty },
        };

        var replacer = new ClearRankReplacer(clearRanks);
        replacer.Replace("%T145CLEARHMR").ShouldBe("Not Clear");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new ClearRankReplacer(ClearRanks);
        replacer.Replace("%T145XXXXXHMR").ShouldBe("%T145XXXXXHMR");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var replacer = new ClearRankReplacer(ClearRanks);
        replacer.Replace("%T145CLEARXMR").ShouldBe("%T145CLEARXMR");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var replacer = new ClearRankReplacer(ClearRanks);
        replacer.Replace("%T145CLEARHXX").ShouldBe("%T145CLEARHXX");
    }
}
