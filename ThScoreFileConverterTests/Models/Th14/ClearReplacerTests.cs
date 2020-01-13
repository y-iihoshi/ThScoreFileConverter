using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;
using ScoreDataStub = ThScoreFileConverterTests.Models.Th10.Stubs.ScoreDataStub<
    ThScoreFileConverter.Models.Th13.StageProgress>;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverterTests.Models.Th14
{
    [TestClass]
    public class ClearReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.ReimuB,
                    Rankings = Utils.GetEnumerable<LevelPracticeWithTotal>().ToDictionary(
                        level => level,
                        level => Enumerable.Range(0, 10).Select(
                            index => new ScoreDataStub()
                            {
                                StageProgress = (level == LevelPracticeWithTotal.Extra)
                                    ? StageProgress.Extra : (StageProgress)(5 - (index % 5)),
                                DateTime = (uint)index % 2,
                            }).ToList() as IReadOnlyList<IScoreData>),
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
            Assert.AreEqual("Stage 5", replacer.Replace("%T14CLEARHRB"));
        }

        [TestMethod]
        public void ReplaceTestExtra()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("Not Clear", replacer.Replace("%T14CLEARXRB"));
        }

        [TestMethod]
        public void ReplaceTestExtraClear()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.ReimuB,
                    Rankings = Utils.GetEnumerable<LevelPracticeWithTotal>().ToDictionary(
                        level => level,
                        level => new List<IScoreData>
                        {
                            new ScoreDataStub
                            {
                                StageProgress = StageProgress.ExtraClear,
                                DateTime = 1u,
                            },
                        } as IReadOnlyList<IScoreData>),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("All Clear", replacer.Replace("%T14CLEARXRB"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("-------", replacer.Replace("%T14CLEARHRB"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRankings()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.ReimuB,
                    Rankings = new Dictionary<LevelPracticeWithTotal, IReadOnlyList<IScoreData>>(),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("-------", replacer.Replace("%T14CLEARHRB"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRanking()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.ReimuB,
                    Rankings = Utils.GetEnumerable<LevelPracticeWithTotal>().ToDictionary(
                        level => level,
                        level => new List<IScoreData>() as IReadOnlyList<IScoreData>),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("-------", replacer.Replace("%T14CLEARHRB"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T14XXXXXHRB", replacer.Replace("%T14XXXXXHRB"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T14CLEARYRB", replacer.Replace("%T14CLEARYRB"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T14CLEARHXX", replacer.Replace("%T14CLEARHXX"));
        }
    }
}
