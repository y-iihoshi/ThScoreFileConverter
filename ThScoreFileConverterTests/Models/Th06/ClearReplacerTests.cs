using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverterTests.Models.Th06.Stubs;

namespace ThScoreFileConverterTests.Models.Th06
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
                            StageProgress = HighScoreTests.ValidStub.StageProgress + 1,
                        },
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
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>();
            var replacer = new ClearReplacer(rankings);
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
            var replacer = new ClearReplacer(rankings);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTest()
        {
            var replacer = new ClearReplacer(Rankings);
            Assert.AreEqual(StageProgress.Four.ToShortName(), replacer.Replace("%T06CLEARHRB"));
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
            var replacer = new ClearReplacer(rankings);
            Assert.AreEqual("Not Clear", replacer.Replace("%T06CLEARXRB"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRankings()
        {
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>();
            var replacer = new ClearReplacer(rankings);
            Assert.AreEqual(StageProgress.None.ToShortName(), replacer.Replace("%T06CLEARHRB"));
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
            var replacer = new ClearReplacer(rankings);
            Assert.AreEqual(StageProgress.None.ToShortName(), replacer.Replace("%T06CLEARHRB"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentLevel()
        {
            var replacer = new ClearReplacer(Rankings);
            Assert.AreEqual(StageProgress.None.ToShortName(), replacer.Replace("%T06CLEARNRB"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentChara()
        {
            var replacer = new ClearReplacer(Rankings);
            Assert.AreEqual(StageProgress.None.ToShortName(), replacer.Replace("%T06CLEARHRA"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ClearReplacer(Rankings);
            Assert.AreEqual("%T06XXXXXHRB", replacer.Replace("%T06XXXXXHRB"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ClearReplacer(Rankings);
            Assert.AreEqual("%T06CLEARYRB", replacer.Replace("%T06CLEARYRB"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new ClearReplacer(Rankings);
            Assert.AreEqual("%T06CLEARHXX", replacer.Replace("%T06CLEARHXX"));
        }
    }
}
