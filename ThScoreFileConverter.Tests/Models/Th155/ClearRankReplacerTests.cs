using System.Collections.Immutable;
using ThScoreFileConverter.Core.Models.Th155;
using ThScoreFileConverter.Models.Th155;

namespace ThScoreFileConverter.Tests.Models.Th155;

[TestClass]
public class ClearRankReplacerTests
{
    internal static IReadOnlyDictionary<StoryChara, AllScoreData.Story> StoryDictionary { get; } =
        new Dictionary<StoryChara, AllScoreData.Story>
        {
            {
                StoryChara.ReimuKasen,
                new AllScoreData.Story
                {
                    Available = true,
                    Ed = Levels.OverDrive,
                }
            },
            {
                StoryChara.MarisaKoishi,
                new AllScoreData.Story
                {
                    Available = true,
                    Ed = Levels.Lunatic,
                }
            },
            {
                StoryChara.NitoriKokoro,
                new AllScoreData.Story
                {
                    Available = true,
                    Ed = Levels.Hard,
                }
            },
            {
                StoryChara.MamizouMokou,
                new AllScoreData.Story
                {
                    Available = true,
                    Ed = Levels.Normal,
                }
            },
            {
                StoryChara.MikoByakuren,
                new AllScoreData.Story
                {
                    Available = true,
                    Ed = Levels.Easy,
                }
            },
            {
                StoryChara.FutoIchirin,
                new AllScoreData.Story
                {
                    Available = true,
                    Ed = Levels.None,
                }
            },
        };

    [TestMethod]
    public void ClearRankReplacerTest()
    {
        var replacer = new ClearRankReplacer(StoryDictionary);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ClearRankReplacerTestEmpty()
    {
        var storyDictionary = ImmutableDictionary<StoryChara, AllScoreData.Story>.Empty;
        var replacer = new ClearRankReplacer(storyDictionary);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var replacer = new ClearRankReplacer(StoryDictionary);
        replacer.Replace("%T155CLEARHRK").ShouldBe("Not Clear");
        replacer.Replace("%T155CLEARHMK").ShouldBe("Not Clear");
        replacer.Replace("%T155CLEARHNK").ShouldBe("Clear");
        replacer.Replace("%T155CLEARHMM").ShouldBe("Not Clear");
        replacer.Replace("%T155CLEARHMB").ShouldBe("Not Clear");
        replacer.Replace("%T155CLEARHFI").ShouldBe("Not Clear");
    }

    [TestMethod]
    public void ReplaceTestNotAvailable()
    {
        var dictionary = new Dictionary<StoryChara, AllScoreData.Story>
        {
            {
                StoryChara.MarisaKoishi,
                new AllScoreData.Story
                {
                    Available = false,
                    Ed = Levels.Hard,
                }
            },
        };

        var replacer = new ClearRankReplacer(dictionary);
        replacer.Replace("%T155CLEARHMK").ShouldBe("Not Clear");
    }

    [TestMethod]
    public void ReplaceTestNonexistentChara()
    {
        var replacer = new ClearRankReplacer(StoryDictionary);
        replacer.Replace("%T155CLEARHRD").ShouldBe("Not Clear");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var storyDictionary = ImmutableDictionary<StoryChara, AllScoreData.Story>.Empty;
        var replacer = new ClearRankReplacer(storyDictionary);
        replacer.Replace("%T155CLEARHMK").ShouldBe("Not Clear");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new ClearRankReplacer(StoryDictionary);
        replacer.Replace("%T155XXXXXHMK").ShouldBe("%T155XXXXXHMK");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var replacer = new ClearRankReplacer(StoryDictionary);
        replacer.Replace("%T155CLEARXMK").ShouldBe("%T155CLEARXMK");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var replacer = new ClearRankReplacer(StoryDictionary);
        replacer.Replace("%T155CLEARHXX").ShouldBe("%T155CLEARHXX");
    }
}
