using System.Collections.Immutable;
using ThScoreFileConverter.Core.Models.Th145;
using ThScoreFileConverter.Models.Th145;

namespace ThScoreFileConverter.Tests.Models.Th145;

[TestClass]
public class ClearTimeReplacerTests
{
    internal static IReadOnlyDictionary<Level, IReadOnlyDictionary<Chara, int>> ClearTimes { get; } =
        new Dictionary<Level, IReadOnlyDictionary<Chara, int>>
        {
            {
                Level.Normal,
                new Dictionary<Chara, int>
                {
                    { Chara.ReimuA, 87 * 60 },
                    { Chara.Marisa, 65 * 60 },
                }
            },
            {
                Level.Hard,
                new Dictionary<Chara, int>
                {
                    { Chara.ReimuA, 43 * 60 },
                    { Chara.Marisa, 21 * 60 },
                }
            },
        };

    [TestMethod]
    public void ClearTimeReplacerTest()
    {
        var replacer = new ClearTimeReplacer(ClearTimes);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ClearTimeReplacerTestEmpty()
    {
        var clearTimes = ImmutableDictionary<Level, IReadOnlyDictionary<Chara, int>>.Empty;
        var replacer = new ClearTimeReplacer(clearTimes);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ClearTimeReplacerTestEmptyCharacters()
    {
        var clearTimes = new Dictionary<Level, IReadOnlyDictionary<Chara, int>>
        {
            { Level.Hard, ImmutableDictionary<Chara, int>.Empty },
        };

        var replacer = new ClearTimeReplacer(clearTimes);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var replacer = new ClearTimeReplacer(ClearTimes);
        replacer.Replace("%T145TIMECLRHMR").ShouldBe("0:00:21");
    }

    [TestMethod]
    public void ReplaceTestLevelTotal()
    {
        var replacer = new ClearTimeReplacer(ClearTimes);
        replacer.Replace("%T145TIMECLRTMR").ShouldBe("0:01:26");
    }

    [TestMethod]
    public void ReplaceTestCharaTotal()
    {
        var replacer = new ClearTimeReplacer(ClearTimes);
        replacer.Replace("%T145TIMECLRHTL").ShouldBe("0:01:04");
    }

    [TestMethod]
    public void ReplaceTestTotal()
    {
        var replacer = new ClearTimeReplacer(ClearTimes);
        replacer.Replace("%T145TIMECLRTTL").ShouldBe("0:03:36");
    }

    [TestMethod]
    public void ReplaceTestNonexistentLevel()
    {
        var replacer = new ClearTimeReplacer(ClearTimes);
        replacer.Replace("%T145TIMECLRLMR").ShouldBe("0:00:00");
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var replacer = new ClearTimeReplacer(ClearTimes);
        replacer.Replace("%T145TIMECLRHIU").ShouldBe("0:00:00");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var clearTimes = ImmutableDictionary<Level, IReadOnlyDictionary<Chara, int>>.Empty;
        var replacer = new ClearTimeReplacer(clearTimes);
        replacer.Replace("%T145TIMECLRHMR").ShouldBe("0:00:00");
    }

    [TestMethod]
    public void ReplaceTestEmptyCharacters()
    {
        var clearTimes = new Dictionary<Level, IReadOnlyDictionary<Chara, int>>
        {
            { Level.Hard, ImmutableDictionary<Chara, int>.Empty },
        };

        var replacer = new ClearTimeReplacer(clearTimes);
        replacer.Replace("%T145TIMECLRHMR").ShouldBe("0:00:00");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new ClearTimeReplacer(ClearTimes);
        replacer.Replace("%T145XXXXXXXHMR").ShouldBe("%T145XXXXXXXHMR");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var replacer = new ClearTimeReplacer(ClearTimes);
        replacer.Replace("%T145TIMECLRXMR").ShouldBe("%T145TIMECLRXMR");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var replacer = new ClearTimeReplacer(ClearTimes);
        replacer.Replace("%T145TIMECLRHXX").ShouldBe("%T145TIMECLRHXX");
    }
}
