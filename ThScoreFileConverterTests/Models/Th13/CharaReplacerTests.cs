using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th13;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th13.CharaWithTotal,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th13.StagePractice>;

namespace ThScoreFileConverterTests.Models.Th13
{
    [TestClass]
    public class CharaReplacerTests
    {
        private static IEnumerable<IClearData> CreateClearDataList()
        {
            var levels = Utils.GetEnumerable<LevelPracticeWithTotal>();
            return new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.Marisa)
                         && (m.TotalPlayCount == 23)
                         && (m.PlayTime == 4567890)
                         && (m.ClearCounts == levels.ToDictionary(level => level, level => 100 - (int)level))),
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.Sanae)
                         && (m.TotalPlayCount == 12)
                         && (m.PlayTime == 3456789)
                         && (m.ClearCounts == levels.ToDictionary(level => level, level => 50 - (int)level))),
            };
        }

        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            CreateClearDataList().ToDictionary(element => element.Chara);

        [TestMethod]
        public void CharaReplacerTest()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void CharaReplacerTestNull()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new CharaReplacer(null!));

        [TestMethod]
        public void CharaReplacerTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CharaReplacer(dictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestTotalPlayCount()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("23", replacer.Replace("%T13CHARAMR1"));
        }

        [TestMethod]
        public void ReplaceTestPlayTime()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("21:08:51", replacer.Replace("%T13CHARAMR2"));
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("679", replacer.Replace("%T13CHARAMR3"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalTotalPlayCount()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("35", replacer.Replace("%T13CHARATL1"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalPlayTime()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("37:09:04", replacer.Replace("%T13CHARATL2"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalClearCount()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new CharaReplacer(ClearDataDictionary);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("1,008", replacer.Replace("%T13CHARATL3"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("1008", replacer.Replace("%T13CHARATL3"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CharaReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T13CHARAMR1"));
            Assert.AreEqual("0:00:00", replacer.Replace("%T13CHARAMR2"));
            Assert.AreEqual("0", replacer.Replace("%T13CHARAMR3"));
        }

        [TestMethod]
        public void ReplaceTestEmptyClearCounts()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.Marisa)
                         && (m.ClearCounts == new Dictionary<LevelPracticeWithTotal, int>()))
            }.ToDictionary(clearData => clearData.Chara);

            var replacer = new CharaReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T13CHARAMR3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13XXXXXMR1", replacer.Replace("%T13XXXXXMR1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13CHARAXX1", replacer.Replace("%T13CHARAXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13CHARAMRX", replacer.Replace("%T13CHARAMRX"));
        }
    }
}
