using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th155;

namespace ThScoreFileConverterTests.Models.Th155
{
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
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ClearRankReplacerTestEmpty()
        {
            var storyDictionary = new Dictionary<StoryChara, AllScoreData.Story>();
            var replacer = new ClearRankReplacer(storyDictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTest()
        {
            var replacer = new ClearRankReplacer(StoryDictionary);
            Assert.AreEqual("Not Clear", replacer.Replace("%T155CLEARHRK"));
            Assert.AreEqual("Not Clear", replacer.Replace("%T155CLEARHMK"));
            Assert.AreEqual("Clear", replacer.Replace("%T155CLEARHNK"));
            Assert.AreEqual("Not Clear", replacer.Replace("%T155CLEARHMM"));
            Assert.AreEqual("Not Clear", replacer.Replace("%T155CLEARHMB"));
            Assert.AreEqual("Not Clear", replacer.Replace("%T155CLEARHFI"));
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
            Assert.AreEqual("Not Clear", replacer.Replace("%T155CLEARHMK"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentChara()
        {
            var replacer = new ClearRankReplacer(StoryDictionary);
            Assert.AreEqual("Not Clear", replacer.Replace("%T155CLEARHRD"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var storyDictionary = new Dictionary<StoryChara, AllScoreData.Story>();
            var replacer = new ClearRankReplacer(storyDictionary);
            Assert.AreEqual("Not Clear", replacer.Replace("%T155CLEARHMK"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ClearRankReplacer(StoryDictionary);
            Assert.AreEqual("%T155XXXXXHMK", replacer.Replace("%T155XXXXXHMK"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ClearRankReplacer(StoryDictionary);
            Assert.AreEqual("%T155CLEARXMK", replacer.Replace("%T155CLEARXMK"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new ClearRankReplacer(StoryDictionary);
            Assert.AreEqual("%T155CLEARHXX", replacer.Replace("%T155CLEARHXX"));
        }
    }
}
