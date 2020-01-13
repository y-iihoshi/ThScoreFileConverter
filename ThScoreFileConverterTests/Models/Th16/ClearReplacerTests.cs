using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th16;
using ThScoreFileConverterTests.Models.Th16.Stubs;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverterTests.Models.Th16
{
    [TestClass]
    public class ClearReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.Aya,
                    Rankings = Utils.GetEnumerable<LevelWithTotal>().ToDictionary(
                        level => level,
                        level => Enumerable.Range(0, 10).Select(
                            index => new ScoreDataStub()
                            {
                                StageProgress = (level == LevelWithTotal.Extra)
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
            Assert.AreEqual("Stage 5", replacer.Replace("%T16CLEARHAY"));
        }

        [TestMethod]
        public void ReplaceTestExtra()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("Not Clear", replacer.Replace("%T16CLEARXAY"));
        }

        [TestMethod]
        public void ReplaceTestExtraClear()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.Aya,
                    Rankings = Utils.GetEnumerable<LevelWithTotal>().ToDictionary(
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
            Assert.AreEqual("All Clear", replacer.Replace("%T16CLEARXAY"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("-------", replacer.Replace("%T16CLEARHAY"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRankings()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.Aya,
                    Rankings = new Dictionary<LevelWithTotal, IReadOnlyList<IScoreData>>(),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("-------", replacer.Replace("%T16CLEARHAY"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRanking()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.Aya,
                    Rankings = Utils.GetEnumerable<LevelWithTotal>().ToDictionary(
                        level => level,
                        level => new List<IScoreData>() as IReadOnlyList<IScoreData>),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("-------", replacer.Replace("%T16CLEARHAY"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T16XXXXXHAY", replacer.Replace("%T16XXXXXHAY"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T16CLEARYAY", replacer.Replace("%T16CLEARYAY"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T16CLEARHXX", replacer.Replace("%T16CLEARHXX"));
        }
    }
}
