using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th135;

namespace ThScoreFileConverterTests.Models.Th135
{
    [TestClass]
    public class ClearReplacerTests
    {
        internal static IReadOnlyDictionary<Chara, LevelFlags> StoryClearFlags { get; } =
            new Dictionary<Chara, LevelFlags>
            {
                { Chara.Marisa, LevelFlags.Easy | LevelFlags.Hard },
            };

        [TestMethod]
        public void ClearReplacerTest()
        {
            var replacer = new ClearReplacer(StoryClearFlags);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ClearReplacerTestNull()
        {
            _ = new ClearReplacer(null!);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ClearReplacerTestEmpty()
        {
            var dictionary = new Dictionary<Chara, LevelFlags>();
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
            var dictionary = new Dictionary<Chara, LevelFlags>();
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
