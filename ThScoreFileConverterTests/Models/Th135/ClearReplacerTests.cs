using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th135;

namespace ThScoreFileConverterTests.Models.Th135
{
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
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ClearReplacerTestEmpty()
        {
            var dictionary = ImmutableDictionary<Chara, Levels>.Empty;
            var replacer = new ClearReplacer(dictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTest()
        {
            var replacer = new ClearReplacer(StoryClearFlags);
            Assert.AreEqual("Clear", replacer.Replace("%T135CLEAREMR"));
            Assert.AreEqual("Not Clear", replacer.Replace("%T135CLEARNMR"));
            Assert.AreEqual("Clear", replacer.Replace("%T135CLEARHMR"));
            Assert.AreEqual("Not Clear", replacer.Replace("%T135CLEARLMR"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = ImmutableDictionary<Chara, Levels>.Empty;
            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("Not Clear", replacer.Replace("%T135CLEAREMR"));
            Assert.AreEqual("Not Clear", replacer.Replace("%T135CLEARNMR"));
            Assert.AreEqual("Not Clear", replacer.Replace("%T135CLEARHMR"));
            Assert.AreEqual("Not Clear", replacer.Replace("%T135CLEARLMR"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ClearReplacer(StoryClearFlags);
            Assert.AreEqual("%T135XXXXXHMR", replacer.Replace("%T135XXXXXHMR"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ClearReplacer(StoryClearFlags);
            Assert.AreEqual("%T135CLEARXMR", replacer.Replace("%T135CLEARXMR"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new ClearReplacer(StoryClearFlags);
            Assert.AreEqual("%T135CLEARHXX", replacer.Replace("%T135CLEARHXX"));
        }
    }
}
