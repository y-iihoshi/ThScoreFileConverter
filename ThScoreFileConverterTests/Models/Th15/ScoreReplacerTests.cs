using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th15;
using ThScoreFileConverterTests.Models.Th15.Stubs;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverterTests.Models.Th15
{
    [TestClass]
    public class ScoreReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            new List<IClearData>
            {
                ClearDataTests.MakeValidStub(),
            }.ToDictionary(element => element.Chara);

        [TestMethod]
        public void ScoreReplacerTest()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ScoreReplacerTestNull()
        {
            _ = new ScoreReplacer(null!);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ScoreReplacerTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new ScoreReplacer(dictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestName()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("Player1", replacer.Replace("%T15SCRPHMR21"));
        }

        [TestMethod]
        public void ReplaceTestScore()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new ScoreReplacer(ClearDataDictionary);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("123,446,701", replacer.Replace("%T15SCRPHMR22"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("123446701", replacer.Replace("%T15SCRPHMR22"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestStage()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("Stage 5", replacer.Replace("%T15SCRPHMR23"));
        }

        [TestMethod]
        public void ReplaceTestDateTime()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            var expected = new DateTime(1970, 1, 1).AddSeconds(34567890).ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
            Assert.AreEqual(expected, replacer.Replace("%T15SCRPHMR24"));
        }

        [TestMethod]
        public void ReplaceTestSlowRate()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("1.200%", replacer.Replace("%T15SCRPHMR25"));  // really...?
        }

        [TestMethod]
        public void ReplaceTestRetryCount()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("1", replacer.Replace("%T15SCRPHMR26"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("--------", replacer.Replace("%T15SCRPHMR21"));
            Assert.AreEqual("0", replacer.Replace("%T15SCRPHMR22"));
            Assert.AreEqual("-------", replacer.Replace("%T15SCRPHMR23"));
            Assert.AreEqual("----/--/-- --:--:--", replacer.Replace("%T15SCRPHMR24"));
            Assert.AreEqual("-----%", replacer.Replace("%T15SCRPHMR25"));
            Assert.AreEqual("-----", replacer.Replace("%T15SCRPHMR26"));
        }

        [TestMethod]
        public void ReplaceTestEmptyGameModes()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub()
                {
                    Chara = CharaWithTotal.Marisa,
                    GameModeData = new Dictionary<GameMode, IClearDataPerGameMode>(),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("--------", replacer.Replace("%T15SCRPHMR21"));
            Assert.AreEqual("0", replacer.Replace("%T15SCRPHMR22"));
            Assert.AreEqual("-------", replacer.Replace("%T15SCRPHMR23"));
            Assert.AreEqual("----/--/-- --:--:--", replacer.Replace("%T15SCRPHMR24"));
            Assert.AreEqual("-----%", replacer.Replace("%T15SCRPHMR25"));
            Assert.AreEqual("-----", replacer.Replace("%T15SCRPHMR26"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRankings()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub()
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

            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("--------", replacer.Replace("%T15SCRPHMR21"));
            Assert.AreEqual("0", replacer.Replace("%T15SCRPHMR22"));
            Assert.AreEqual("-------", replacer.Replace("%T15SCRPHMR23"));
            Assert.AreEqual("----/--/-- --:--:--", replacer.Replace("%T15SCRPHMR24"));
            Assert.AreEqual("-----%", replacer.Replace("%T15SCRPHMR25"));
            Assert.AreEqual("-----", replacer.Replace("%T15SCRPHMR26"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRanking()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub()
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

            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("--------", replacer.Replace("%T15SCRPHMR21"));
            Assert.AreEqual("0", replacer.Replace("%T15SCRPHMR22"));
            Assert.AreEqual("-------", replacer.Replace("%T15SCRPHMR23"));
            Assert.AreEqual("----/--/-- --:--:--", replacer.Replace("%T15SCRPHMR24"));
            Assert.AreEqual("-----%", replacer.Replace("%T15SCRPHMR25"));
            Assert.AreEqual("-----", replacer.Replace("%T15SCRPHMR26"));
        }

        [TestMethod]
        public void ReplaceTestStageExtra()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub()
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
                                        index => new ScoreDataStub
                                        {
                                            DateTime = 34567890u,
                                            StageProgress = StageProgress.Extra,
                                        }).ToList() as IReadOnlyList<IScoreData>),
                            }
                        },
                    },
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("Not Clear", replacer.Replace("%T15SCRPHMR23"));
        }

        [TestMethod]
        public void ReplaceTestStageExtraClear()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub()
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
                                        index => new ScoreDataStub
                                        {
                                            DateTime = 34567890u,
                                            StageProgress = StageProgress.ExtraClear,
                                        }).ToList() as IReadOnlyList<IScoreData>),
                            }
                        },
                    },
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("All Clear", replacer.Replace("%T15SCRPHMR23"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15XXXPHMR21", replacer.Replace("%T15XXXPHMR21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidGameMode()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15SCRXHMR21", replacer.Replace("%T15SCRXHMR21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15SCRPYMR21", replacer.Replace("%T15SCRPYMR21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15SCRPHXX21", replacer.Replace("%T15SCRPHXX21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidRank()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15SCRPHMRX1", replacer.Replace("%T15SCRPHMRX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15SCRPHMR2X", replacer.Replace("%T15SCRPHMR2X"));
        }
    }
}
