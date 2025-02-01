using System.Collections.Immutable;
using ThScoreFileConverter.Core.Models.Th135;
using ThScoreFileConverter.Models.Th135;

namespace ThScoreFileConverter.Tests.Models.Th135;

[TestClass]
public class ClearReplacerTests
{
    internal static IReadOnlyDictionary<Chara, Levels> StoryClearFlags { get; } =
        new Dictionary<Chara, Levels>
        {
            { Chara.Marisa, Levels.Easy | Levels.Hard },
        };

    [TestMethod]
    public void ClearReplacerTest()
    {
        var replacer = new ClearReplacer(StoryClearFlags);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ClearReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<Chara, Levels>.Empty;
        var replacer = new ClearReplacer(dictionary);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var replacer = new ClearReplacer(StoryClearFlags);
        replacer.Replace("%T135CLEAREMR").ShouldBe("Clear");
        replacer.Replace("%T135CLEARNMR").ShouldBe("Not Clear");
        replacer.Replace("%T135CLEARHMR").ShouldBe("Clear");
        replacer.Replace("%T135CLEARLMR").ShouldBe("Not Clear");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<Chara, Levels>.Empty;
        var replacer = new ClearReplacer(dictionary);
        replacer.Replace("%T135CLEAREMR").ShouldBe("Not Clear");
        replacer.Replace("%T135CLEARNMR").ShouldBe("Not Clear");
        replacer.Replace("%T135CLEARHMR").ShouldBe("Not Clear");
        replacer.Replace("%T135CLEARLMR").ShouldBe("Not Clear");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new ClearReplacer(StoryClearFlags);
        replacer.Replace("%T135XXXXXHMR").ShouldBe("%T135XXXXXHMR");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var replacer = new ClearReplacer(StoryClearFlags);
        replacer.Replace("%T135CLEARXMR").ShouldBe("%T135CLEARXMR");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var replacer = new ClearReplacer(StoryClearFlags);
        replacer.Replace("%T135CLEARHXX").ShouldBe("%T135CLEARHXX");
    }
}
