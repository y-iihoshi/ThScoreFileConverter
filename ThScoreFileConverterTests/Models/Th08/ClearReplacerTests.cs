using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverterTests.Models.Th08.Stubs;
using IHighScore = ThScoreFileConverter.Models.Th08.IHighScore<
    ThScoreFileConverter.Models.Th08.Chara,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th08.StageProgress>;

namespace ThScoreFileConverterTests.Models.Th08
{
    [TestClass]
    public class ClearReplacerTests
    {
        internal static IReadOnlyDictionary<(Chara, Level), IReadOnlyList<IHighScore>> Rankings { get; } =
            new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
            {
                {
                    (HighScoreTests.ValidStub.Chara, HighScoreTests.ValidStub.Level),
                    new List<IHighScore>
                    {
                        new HighScoreStub(HighScoreTests.ValidStub),
                        new HighScoreStub(HighScoreTests.ValidStub)
                        {
                            StageProgress = HighScoreTests.ValidStub.StageProgress - 1,
                        },
                    }
                },
            };

        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearData { get; } = new List<IClearData>
        {
            new ClearDataStub(ClearDataTests.ValidStub),
        }.ToDictionary(entry => entry.Chara);

        [TestMethod]
        public void ClearReplacerTest()
        {
            var replacer = new ClearReplacer(Rankings, ClearData);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ClearReplacerTestNullRankings()
        {
            _ = new ClearReplacer(null!, ClearData);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ClearReplacerTestNullClearData()
        {
            _ = new ClearReplacer(Rankings, null!);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ClearReplacerTestEmptyRankings()
        {
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>();
            var replacer = new ClearReplacer(rankings, ClearData);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ClearReplacerTestEmptyScores()
        {
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
            {
                {
                    (HighScoreTests.ValidStub.Chara, HighScoreTests.ValidStub.Level),
                    new List<IHighScore>()
                },
            };
            var replacer = new ClearReplacer(rankings, ClearData);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTest()
        {
            var replacer = new ClearReplacer(Rankings, ClearData);
            Assert.AreEqual(StageProgress.Three.ToShortName(), replacer.Replace("%T08CLEARHMA"));
        }

        [TestMethod]
        public void ReplaceTestUncanny()
        {
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
            {
                {
                    (HighScoreTests.ValidStub.Chara, HighScoreTests.ValidStub.Level),
                    new List<IHighScore>
                    {
                        new HighScoreStub(HighScoreTests.ValidStub)
                        {
                            StageProgress = StageProgress.FourUncanny,
                        },
                    }
                },
            };
            var replacer = new ClearReplacer(rankings, ClearData);
            Assert.AreEqual("Stage 4", replacer.Replace("%T08CLEARHMA"));
        }

        [TestMethod]
        public void ReplaceTestPowerful()
        {
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
            {
                {
                    (HighScoreTests.ValidStub.Chara, HighScoreTests.ValidStub.Level),
                    new List<IHighScore>
                    {
                        new HighScoreStub(HighScoreTests.ValidStub)
                        {
                            StageProgress = StageProgress.FourPowerful,
                        },
                    }
                },
            };
            var replacer = new ClearReplacer(rankings, ClearData);
            Assert.AreEqual("Stage 4", replacer.Replace("%T08CLEARHMA"));
        }

        [TestMethod]
        public void ReplaceTestFinalAClear()
        {
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
            {
                {
                    (HighScoreTests.ValidStub.Chara, HighScoreTests.ValidStub.Level),
                    new List<IHighScore>
                    {
                        new HighScoreStub(HighScoreTests.ValidStub)
                        {
                            StageProgress = StageProgress.Clear,
                        },
                    }
                },
            };
            var replacer = new ClearReplacer(rankings, ClearData);
            Assert.AreEqual("FinalA Clear", replacer.Replace("%T08CLEARHMA"));
        }

        [TestMethod]
        public void ReplaceTestAllClear()
        {
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
            {
                {
                    (HighScoreTests.ValidStub.Chara, HighScoreTests.ValidStub.Level),
                    new List<IHighScore>
                    {
                        new HighScoreStub(HighScoreTests.ValidStub)
                        {
                            StageProgress = StageProgress.Clear,
                        },
                    }
                },
            };
            var clearData = new List<IClearData>
            {
                new ClearDataStub(ClearDataTests.ValidStub)
                {
                    StoryFlags = Utils.GetEnumerable<Level>().ToDictionary(level => level, _ => PlayableStages.Stage6B),
                },
            }.ToDictionary(entry => entry.Chara);
            var replacer = new ClearReplacer(rankings, clearData);
            Assert.AreEqual(StageProgress.Clear.ToShortName(), replacer.Replace("%T08CLEARHMA"));
        }

        [TestMethod]
        public void ReplaceTestExtra()
        {
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
            {
                {
                    (HighScoreTests.ValidStub.Chara, Level.Extra),
                    new List<IHighScore>
                    {
                        new HighScoreStub(HighScoreTests.ValidStub)
                        {
                            Level = Level.Extra,
                            StageProgress = StageProgress.Extra,
                        },
                    }
                },
            };
            var replacer = new ClearReplacer(rankings, ClearData);
            Assert.AreEqual("Not Clear", replacer.Replace("%T08CLEARXMA"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRankings()
        {
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>();
            var replacer = new ClearReplacer(rankings, ClearData);
            Assert.AreEqual("-------", replacer.Replace("%T08CLEARHMA"));
        }

        [TestMethod]
        public void ReplaceTestEmptyScores()
        {
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
            {
                {
                    (HighScoreTests.ValidStub.Chara, HighScoreTests.ValidStub.Level),
                    new List<IHighScore>()
                },
            };
            var replacer = new ClearReplacer(rankings, ClearData);
            Assert.AreEqual("-------", replacer.Replace("%T08CLEARHMA"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentLevel()
        {
            var replacer = new ClearReplacer(Rankings, ClearData);
            Assert.AreEqual("-------", replacer.Replace("%T08CLEARNMA"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentChara()
        {
            var replacer = new ClearReplacer(Rankings, ClearData);
            Assert.AreEqual("-------", replacer.Replace("%T08CLEARHRY"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ClearReplacer(Rankings, ClearData);
            Assert.AreEqual("%T08XXXXXHMA", replacer.Replace("%T08XXXXXHMA"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ClearReplacer(Rankings, ClearData);
            Assert.AreEqual("%T08CLEARYMA", replacer.Replace("%T08CLEARYMA"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new ClearReplacer(Rankings, ClearData);
            Assert.AreEqual("%T08CLEARHXX", replacer.Replace("%T08CLEARHXX"));
        }
    }
}
