using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th145;

namespace ThScoreFileConverterTests.Models.Th145
{
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
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ClearTimeReplacerTestNull()
        {
            _ = new ClearTimeReplacer(null);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ClearTimeReplacerTestEmpty()
        {
            var clearTimes = new Dictionary<Level, IReadOnlyDictionary<Chara, int>>();
            var replacer = new ClearTimeReplacer(clearTimes);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ClearTimeReplacerTestEmptyCharacters()
        {
            var clearTimes = new Dictionary<Level, IReadOnlyDictionary<Chara, int>>
            {
                { Level.Hard, new Dictionary<Chara, int>() },
            };

            var replacer = new ClearTimeReplacer(clearTimes);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTest()
        {
            var replacer = new ClearTimeReplacer(ClearTimes);
            Assert.AreEqual("0:00:21", replacer.Replace("%T145TIMECLRHMR"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotal()
        {
            var replacer = new ClearTimeReplacer(ClearTimes);
            Assert.AreEqual("0:01:26", replacer.Replace("%T145TIMECLRTMR"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotal()
        {
            var replacer = new ClearTimeReplacer(ClearTimes);
            Assert.AreEqual("0:01:04", replacer.Replace("%T145TIMECLRHTL"));
        }

        [TestMethod]
        public void ReplaceTestTotal()
        {
            var replacer = new ClearTimeReplacer(ClearTimes);
            Assert.AreEqual("0:03:36", replacer.Replace("%T145TIMECLRTTL"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentLevel()
        {
            var replacer = new ClearTimeReplacer(ClearTimes);
            Assert.AreEqual("0:00:00", replacer.Replace("%T145TIMECLRLMR"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentChara()
        {
            var replacer = new ClearTimeReplacer(ClearTimes);
            Assert.AreEqual("0:00:00", replacer.Replace("%T145TIMECLRHIU"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var clearTimes = new Dictionary<Level, IReadOnlyDictionary<Chara, int>>();
            var replacer = new ClearTimeReplacer(clearTimes);
            Assert.AreEqual("0:00:00", replacer.Replace("%T145TIMECLRHMR"));
        }

        [TestMethod]
        public void ReplaceTestEmptyCharacters()
        {
            var clearTimes = new Dictionary<Level, IReadOnlyDictionary<Chara, int>>
            {
                { Level.Hard, new Dictionary<Chara, int>() },
            };

            var replacer = new ClearTimeReplacer(clearTimes);
            Assert.AreEqual("0:00:00", replacer.Replace("%T145TIMECLRHMR"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ClearTimeReplacer(ClearTimes);
            Assert.AreEqual("%T145XXXXXXXHMR", replacer.Replace("%T145XXXXXXXHMR"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ClearTimeReplacer(ClearTimes);
            Assert.AreEqual("%T145TIMECLRXMR", replacer.Replace("%T145TIMECLRXMR"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new ClearTimeReplacer(ClearTimes);
            Assert.AreEqual("%T145TIMECLRHXX", replacer.Replace("%T145TIMECLRHXX"));
        }
    }
}
