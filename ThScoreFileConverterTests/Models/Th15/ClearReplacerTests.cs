using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th15;
using ThScoreFileConverterTests.Models.Th15.Stubs;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverterTests.Models.Th15
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
                    GameModeData = new Dictionary<GameMode, IClearDataPerGameMode>
                    {
                        {
                            GameMode.Pointdevice,
                            new ClearDataPerGameModeStub
                            {
                                Rankings = Utils.GetEnumerator<LevelWithTotal>().ToDictionary(
                                    level => level,
                                    level => Enumerable.Range(0, 10).Select(
                                        index => new ScoreDataStub()
                                        {
                                            StageProgress = (level == LevelWithTotal.Extra)
                                                ? StageProgress.Extra : (StageProgress)(5 - (index % 5)),
                                            DateTime = (uint)index % 2,
                                        }).ToList() as IReadOnlyList<IScoreData>),
                            }
                        },
                    },
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
            _ = new ClearReplacer(null);
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
            Assert.AreEqual("Stage 5", replacer.Replace("%T15CLEARPHMR"));
        }

        [TestMethod]
        public void ReplaceTestExtra()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("Not Clear", replacer.Replace("%T15CLEARPXMR"));
        }

        [TestMethod]
        public void ReplaceTestExtraClear()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.Marisa,
                    GameModeData = new Dictionary<GameMode, IClearDataPerGameMode>
                    {
                        {
                            GameMode.Pointdevice,
                            new ClearDataPerGameModeStub
                            {
                                Rankings = Utils.GetEnumerator<LevelWithTotal>().ToDictionary(
                                    level => level,
                                    level => new List<IScoreData>
                                    {
                                        new ScoreDataStub
                                        {
                                            StageProgress = StageProgress.ExtraClear,
                                            DateTime = 1u,
                                        },
                                    } as IReadOnlyList<IScoreData>),
                            }
                        },
                    },
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("All Clear", replacer.Replace("%T15CLEARPXMR"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("-------", replacer.Replace("%T15CLEARPHMR"));
        }

        [TestMethod]
        public void ReplaceTestEmptyGameModes()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.Marisa,
                    GameModeData = new Dictionary<GameMode, IClearDataPerGameMode>(),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("-------", replacer.Replace("%T15CLEARPHMR"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRankings()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.Marisa,
                    GameModeData = new Dictionary<GameMode, IClearDataPerGameMode>
                    {
                        {
                            GameMode.Pointdevice,
                            new ClearDataPerGameModeStub
                            {
                                Rankings = new Dictionary<LevelWithTotal, IReadOnlyList<IScoreData>>(),
                            }
                        },
                    },
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("-------", replacer.Replace("%T15CLEARPHMR"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRanking()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.Marisa,
                    GameModeData = new Dictionary<GameMode, IClearDataPerGameMode>
                    {
                        {
                            GameMode.Pointdevice,
                            new ClearDataPerGameModeStub
                            {
                                Rankings = Utils.GetEnumerator<LevelWithTotal>().ToDictionary(
                                    level => level,
                                    level => new List<IScoreData>() as IReadOnlyList<IScoreData>),
                            }
                        },
                    },
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("-------", replacer.Replace("%T15CLEARPHMR"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15XXXXXPHMR", replacer.Replace("%T15XXXXXPHMR"));
        }

        [TestMethod]
        public void ReplaceTestInvalidGameMode()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15CLEARXHMR", replacer.Replace("%T15CLEARXHMR"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15CLEARPYMR", replacer.Replace("%T15CLEARPYMR"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15CLEARPHXX", replacer.Replace("%T15CLEARPHXX"));
        }
    }
}
