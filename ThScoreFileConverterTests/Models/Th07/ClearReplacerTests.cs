using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverterTests.Models.Th06.Wrappers;
using static ThScoreFileConverter.Models.EnumExtensions;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class ClearReplacerTests
    {
        internal static IReadOnlyDictionary<(Chara, Level), List<HighScore>> Rankings { get; } =
            new Dictionary<(Chara, Level), List<HighScore>>
            {
                {
                    (HighScoreTests.ValidProperties.chara, HighScoreTests.ValidProperties.level),
                    new List<HighScore>
                    {
                        new HighScore(ChapterWrapper.Create(HighScoreTests.MakeByteArray(
                            HighScoreTests.ValidProperties)).Target),
                        new HighScore(ChapterWrapper.Create(HighScoreTests.MakeByteArray(
                            new HighScoreTests.Properties
                            {
                                signature = HighScoreTests.ValidProperties.signature,
                                size1 = HighScoreTests.ValidProperties.size1,
                                size2 = HighScoreTests.ValidProperties.size2,
                                score = HighScoreTests.ValidProperties.score,
                                slowRate = HighScoreTests.ValidProperties.slowRate,
                                chara = HighScoreTests.ValidProperties.chara,
                                level = HighScoreTests.ValidProperties.level,
                                stageProgress = HighScoreTests.ValidProperties.stageProgress + 1,
                                name = HighScoreTests.ValidProperties.name,
                                date = HighScoreTests.ValidProperties.date,
                                continueCount = HighScoreTests.ValidProperties.continueCount,
                            })).Target),
                    }
                },
            };

        [TestMethod]
        public void ClearReplacerTest()
        {
            var replacer = new ClearReplacer(Rankings);
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
        public void ClearReplacerTestEmptyRankings()
        {
            var rankings = new Dictionary<(Chara, Level), List<HighScore>>();
            var replacer = new ClearReplacer(rankings);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ClearReplacerTestEmptyScores()
        {
            var rankings = new Dictionary<(Chara, Level), List<HighScore>>
            {
                {
                    (HighScoreTests.ValidProperties.chara, HighScoreTests.ValidProperties.level),
                    new List<HighScore>()
                },
            };
            var replacer = new ClearReplacer(rankings);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTest()
        {
            var replacer = new ClearReplacer(Rankings);
            Assert.AreEqual(StageProgress.Four.ToShortName(), replacer.Replace("%T07CLEARHRB"));
        }

        [TestMethod]
        public void ReplaceTestExtra()
        {
            var rankings = new Dictionary<(Chara, Level), List<HighScore>>
            {
                {
                    (HighScoreTests.ValidProperties.chara, Level.Extra),
                    new List<HighScore>
                    {
                        new HighScore(ChapterWrapper.Create(HighScoreTests.MakeByteArray(
                            new HighScoreTests.Properties
                            {
                                signature = HighScoreTests.ValidProperties.signature,
                                size1 = HighScoreTests.ValidProperties.size1,
                                size2 = HighScoreTests.ValidProperties.size2,
                                score = HighScoreTests.ValidProperties.score,
                                slowRate = HighScoreTests.ValidProperties.slowRate,
                                chara = HighScoreTests.ValidProperties.chara,
                                level = Level.Extra,
                                stageProgress = StageProgress.Extra,
                                name = HighScoreTests.ValidProperties.name,
                                date = HighScoreTests.ValidProperties.date,
                                continueCount = HighScoreTests.ValidProperties.continueCount,
                            })).Target),
                    }
                },
            };
            var replacer = new ClearReplacer(rankings);
            Assert.AreEqual("Not Clear", replacer.Replace("%T07CLEARXRB"));
        }

        [TestMethod]
        public void ReplaceTestPhantasm()
        {
            var rankings = new Dictionary<(Chara, Level), List<HighScore>>
            {
                {
                    (HighScoreTests.ValidProperties.chara, Level.Phantasm),
                    new List<HighScore>
                    {
                        new HighScore(ChapterWrapper.Create(HighScoreTests.MakeByteArray(
                            new HighScoreTests.Properties
                            {
                                signature = HighScoreTests.ValidProperties.signature,
                                size1 = HighScoreTests.ValidProperties.size1,
                                size2 = HighScoreTests.ValidProperties.size2,
                                score = HighScoreTests.ValidProperties.score,
                                slowRate = HighScoreTests.ValidProperties.slowRate,
                                chara = HighScoreTests.ValidProperties.chara,
                                level = Level.Phantasm,
                                stageProgress = StageProgress.Phantasm,
                                name = HighScoreTests.ValidProperties.name,
                                date = HighScoreTests.ValidProperties.date,
                                continueCount = HighScoreTests.ValidProperties.continueCount,
                            })).Target),
                    }
                },
            };
            var replacer = new ClearReplacer(rankings);
            Assert.AreEqual("Not Clear", replacer.Replace("%T07CLEARPRB"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRankings()
        {
            var rankings = new Dictionary<(Chara, Level), List<HighScore>>();
            var replacer = new ClearReplacer(rankings);
            Assert.AreEqual(StageProgress.None.ToShortName(), replacer.Replace("%T07CLEARHRB"));
        }

        [TestMethod]
        public void ReplaceTestEmptyScores()
        {
            var rankings = new Dictionary<(Chara, Level), List<HighScore>>
            {
                {
                    (HighScoreTests.ValidProperties.chara, HighScoreTests.ValidProperties.level),
                    new List<HighScore>()
                },
            };
            var replacer = new ClearReplacer(rankings);
            Assert.AreEqual(StageProgress.None.ToShortName(), replacer.Replace("%T07CLEARHRB"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentLevel()
        {
            var replacer = new ClearReplacer(Rankings);
            Assert.AreEqual(StageProgress.None.ToShortName(), replacer.Replace("%T07CLEARNRB"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentChara()
        {
            var replacer = new ClearReplacer(Rankings);
            Assert.AreEqual(StageProgress.None.ToShortName(), replacer.Replace("%T07CLEARHRA"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ClearReplacer(Rankings);
            Assert.AreEqual("%T07XXXXXHRB", replacer.Replace("%T07XXXXXHRB"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ClearReplacer(Rankings);
            Assert.AreEqual("%T07CLEARYRB", replacer.Replace("%T07CLEARYRB"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new ClearReplacer(Rankings);
            Assert.AreEqual("%T07CLEARHXX", replacer.Replace("%T07CLEARHXX"));
        }
    }
}
