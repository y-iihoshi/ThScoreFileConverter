using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th14;
using ClearDataStub = ThScoreFileConverterTests.Models.Th13.Stubs.ClearDataStub<
    ThScoreFileConverter.Models.Th14.CharaWithTotal,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th14.LevelPractice,
    ThScoreFileConverter.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th14.StagePractice>;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th14.CharaWithTotal,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th14.LevelPractice,
    ThScoreFileConverter.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th14.StagePractice>;

namespace ThScoreFileConverterTests.Models.Th14
{
    [TestClass]
    public class CharaReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.ReimuB,
                    TotalPlayCount = 23,
                    PlayTime = 4567890,
                    ClearCounts = Utils.GetEnumerable<LevelPracticeWithTotal>()
                        .ToDictionary(level => level, level => 100 - (int)level),
                },
                new ClearDataStub
                {
                    Chara = CharaWithTotal.MarisaA,
                    TotalPlayCount = 12,
                    PlayTime = 3456789,
                    ClearCounts = Utils.GetEnumerable<LevelPracticeWithTotal>()
                        .ToDictionary(level => level, level => 50 - (int)level),
                },
            }.ToDictionary(element => element.Chara);

        [TestMethod]
        public void CharaReplacerTest()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CharaReplacerTestNull()
        {
            _ = new CharaReplacer(null!);
            Assert.Fail(TestUtils.Unreachable);
        }

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
            Assert.AreEqual("23", replacer.Replace("%T14CHARARB1"));
        }

        [TestMethod]
        public void ReplaceTestPlayTime()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("21:08:51", replacer.Replace("%T14CHARARB2"));
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("679", replacer.Replace("%T14CHARARB3"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalTotalPlayCount()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("35", replacer.Replace("%T14CHARATL1"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalPlayTime()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("37:09:04", replacer.Replace("%T14CHARATL2"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalClearCount()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new CharaReplacer(ClearDataDictionary);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("1,008", replacer.Replace("%T14CHARATL3"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("1008", replacer.Replace("%T14CHARATL3"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CharaReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T14CHARARB1"));
            Assert.AreEqual("0:00:00", replacer.Replace("%T14CHARARB2"));
            Assert.AreEqual("0", replacer.Replace("%T14CHARARB3"));
        }

        [TestMethod]
        public void ReplaceTestEmptyClearCounts()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.ReimuB,
                    ClearCounts = new Dictionary<LevelPracticeWithTotal, int>(),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new CharaReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T14CHARARB3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("%T14XXXXXRB1", replacer.Replace("%T14XXXXXRB1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("%T14CHARAXX1", replacer.Replace("%T14CHARAXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("%T14CHARARBX", replacer.Replace("%T14CHARARBX"));
        }
    }
}
