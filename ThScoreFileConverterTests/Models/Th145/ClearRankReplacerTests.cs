using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th145;

namespace ThScoreFileConverterTests.Models.Th145
{
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
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ClearRankReplacerTestNull()
        {
            _ = new ClearRankReplacer(null);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ClearRankReplacerTestEmpty()
        {
            var clearRanks = new Dictionary<Level, IReadOnlyDictionary<Chara, int>>();
            var replacer = new ClearRankReplacer(clearRanks);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ClearRankReplacerTestEmptyCharacters()
        {
            var clearRanks = new Dictionary<Level, IReadOnlyDictionary<Chara, int>>
            {
                { Level.Hard, new Dictionary<Chara, int>() },
            };

            var replacer = new ClearRankReplacer(clearRanks);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTest()
        {
            var replacer = new ClearRankReplacer(ClearRanks);
            Assert.AreEqual("Gold", replacer.Replace("%T145CLEARHRA"));
            Assert.AreEqual("Silver", replacer.Replace("%T145CLEARHMR"));
            Assert.AreEqual("Bronze", replacer.Replace("%T145CLEARHIU"));
            Assert.AreEqual("Not Clear", replacer.Replace("%T145CLEARHBY"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentLevel()
        {
            var replacer = new ClearRankReplacer(ClearRanks);
            Assert.AreEqual("Not Clear", replacer.Replace("%T145CLEARLMR"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentChara()
        {
            var replacer = new ClearRankReplacer(ClearRanks);
            Assert.AreEqual("Not Clear", replacer.Replace("%T145CLEARHFT"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ClearRankReplacer(ClearRanks);
            Assert.AreEqual("%T145XXXXXHMR", replacer.Replace("%T145XXXXXHMR"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ClearRankReplacer(ClearRanks);
            Assert.AreEqual("%T145CLEARXMR", replacer.Replace("%T145CLEARXMR"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new ClearRankReplacer(ClearRanks);
            Assert.AreEqual("%T145CLEARHXX", replacer.Replace("%T145CLEARHXX"));
        }
    }
}
