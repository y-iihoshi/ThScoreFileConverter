using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th17;
using ThScoreFileConverterTests.Models.Th17.Stubs;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverterTests.Models.Th17
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
            Assert.AreEqual("Stage 5", replacer.Replace("%T17CLEARHRB"));
        }

        [TestMethod]
        public void ReplaceTestExtra()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("Not Clear", replacer.Replace("%T17CLEARXRB"));
        }

        [TestMethod]
        public void ReplaceTestExtraClear()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.ReimuB,
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
            Assert.AreEqual("All Clear", replacer.Replace("%T17CLEARXRB"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("-------", replacer.Replace("%T17CLEARHRB"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRankings()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.ReimuB,
                    Rankings = new Dictionary<LevelWithTotal, IReadOnlyList<IScoreData>>(),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("-------", replacer.Replace("%T17CLEARHRB"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRanking()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.ReimuB,
                    Rankings = Utils.GetEnumerable<LevelWithTotal>().ToDictionary(
                        level => level,
                        level => new List<IScoreData>() as IReadOnlyList<IScoreData>),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("-------", replacer.Replace("%T17CLEARHRB"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T17XXXXXHRB", replacer.Replace("%T17XXXXXHRB"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T17CLEARYRB", replacer.Replace("%T17CLEARYRB"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T17CLEARHXX", replacer.Replace("%T17CLEARHXX"));
        }
    }
}
