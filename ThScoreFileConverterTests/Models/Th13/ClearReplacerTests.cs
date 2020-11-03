using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th13;
using ClearDataStub = ThScoreFileConverterTests.Models.Th13.Stubs.ClearDataStub<
    ThScoreFileConverter.Models.Th13.CharaWithTotal,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th13.StagePractice>;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th13.CharaWithTotal,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th13.StagePractice>;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;

namespace ThScoreFileConverterTests.Models.Th13
{
    [TestClass]
    public class ClearReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.Marisa,
                    Rankings = Utils.GetEnumerable<LevelPracticeWithTotal>().ToDictionary(
                        level => level,
                        level => Enumerable.Range(0, 10).Select(
                            index => Mock.Of<IScoreData>(
                                m => (m.StageProgress == (level == LevelPracticeWithTotal.Extra
                                        ? StageProgress.Extra : (StageProgress)(5 - (index % 5))))
                                     && (m.DateTime == (uint)index % 2))).ToList() as IReadOnlyList<IScoreData>),
                },
            }.ToDictionary(element => element.Chara);

        [TestMethod]
        public void ClearReplacerTest()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
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
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new ClearReplacer(dictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTest()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("Stage 5", replacer.Replace("%T13CLEARHMR"));
        }

        [TestMethod]
        public void ReplaceTestExtra()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("Not Clear", replacer.Replace("%T13CLEARXMR"));
        }

        [TestMethod]
        public void ReplaceTestExtraClear()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.Marisa,
                    Rankings = Utils.GetEnumerable<LevelPracticeWithTotal>().ToDictionary(
                        level => level,
                        level => new[]
                        {
                            Mock.Of<IScoreData>(
                                m => (m.StageProgress == StageProgress.ExtraClear) && (m.DateTime == 1u))
                        } as IReadOnlyList<IScoreData>),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("All Clear", replacer.Replace("%T13CLEARXMR"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("-------", replacer.Replace("%T13CLEARHMR"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRankings()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.Marisa,
                    Rankings = new Dictionary<LevelPracticeWithTotal, IReadOnlyList<IScoreData>>(),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("-------", replacer.Replace("%T13CLEARHMR"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRanking()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.Marisa,
                    Rankings = Utils.GetEnumerable<LevelPracticeWithTotal>().ToDictionary(
                        level => level,
                        level => new List<IScoreData>() as IReadOnlyList<IScoreData>),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("-------", replacer.Replace("%T13CLEARHMR"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13XXXXXHMR", replacer.Replace("%T13XXXXXHMR"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13CLEARYMR", replacer.Replace("%T13CLEARYMR"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13CLEARHXX", replacer.Replace("%T13CLEARHXX"));
        }
    }
}
