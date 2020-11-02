using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th09;
using ThScoreFileConverterTests.Models.Th09.Stubs;

namespace ThScoreFileConverterTests.Models.Th09
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
                    }
                },
            };

        internal static IReadOnlyDictionary<Chara, IClearCount> ClearCounts { get; } =
            new Dictionary<Chara, IClearCount>
            {
                { HighScoreTests.ValidStub.Chara, new ClearCountStub(ClearCountTests.ValidStub) },
            };

        internal static IReadOnlyDictionary<Chara, IClearCount> ZeroClearCounts { get; } =
            new Dictionary<Chara, IClearCount>
            {
                {
                    HighScoreTests.ValidStub.Chara,
                    new ClearCountStub()
                    {
                        Counts = Utils.GetEnumerable<Level>().ToDictionary(level => level, _ => 0),
                    }
                },
            };

        [TestMethod]
        public void ClearReplacerTest()
        {
            var replacer = new ClearReplacer(Rankings, ClearCounts);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ClearReplacerTestNullRankings()
        {
            _ = new ClearReplacer(null!, ClearCounts);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ClearReplacerTestNullClearCounts()
        {
            _ = new ClearReplacer(Rankings, null!);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ClearReplacerTestEmptyRankings()
        {
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>();
            var replacer = new ClearReplacer(rankings, ClearCounts);
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
            var replacer = new ClearReplacer(rankings, ClearCounts);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var replacer = new ClearReplacer(Rankings, ClearCounts);
            Assert.AreEqual("2", replacer.Replace("%T09CLEARHMR1"));
        }

        [TestMethod]
        public void ReplaceTestCleared()
        {
            var replacer = new ClearReplacer(Rankings, ClearCounts);
            Assert.AreEqual("Cleared", replacer.Replace("%T09CLEARHMR2"));
        }

        [TestMethod]
        public void ReplaceTestNotCleared()
        {
            var replacer = new ClearReplacer(Rankings, ZeroClearCounts);
            Assert.AreEqual("Not Cleared", replacer.Replace("%T09CLEARHMR2"));
        }

        [TestMethod]
        public void ReplaceTestNotTried()
        {
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
            {
                {
                    (HighScoreTests.ValidStub.Chara, HighScoreTests.ValidStub.Level),
                    new List<IHighScore>
                    {
                        new HighScoreStub(HighScoreTests.ValidStub)
                        {
                            Date = TestUtils.CP932Encoding.GetBytes("--/--\0"),
                        },
                    }
                },
            };
            var replacer = new ClearReplacer(rankings, ZeroClearCounts);
            Assert.AreEqual("-------", replacer.Replace("%T09CLEARHMR2"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRankings()
        {
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>();
            var replacer = new ClearReplacer(rankings, ZeroClearCounts);
            Assert.AreEqual("-------", replacer.Replace("%T09CLEARHMR2"));
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
            var replacer = new ClearReplacer(rankings, ZeroClearCounts);
            Assert.AreEqual("-------", replacer.Replace("%T09CLEARHMR2"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentLevel()
        {
            var clearCounts = new Dictionary<Chara, IClearCount>
            {
                {
                    HighScoreTests.ValidStub.Chara,
                    new ClearCountStub()
                    {
                        Counts = ClearCountTests.ValidStub.Counts.Where(pair => pair.Key != Level.Normal)
                            .ToDictionary(),
                    }
                },
            };
            var replacer = new ClearReplacer(Rankings, clearCounts);
            Assert.AreEqual("0", replacer.Replace("%T09CLEARNMR1"));
            Assert.AreEqual("-------", replacer.Replace("%T09CLEARNMR2"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentChara()
        {
            var replacer = new ClearReplacer(Rankings, ClearCounts);
            Assert.AreEqual("0", replacer.Replace("%T09CLEARHRM1"));
            Assert.AreEqual("-------", replacer.Replace("%T09CLEARHRM2"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ClearReplacer(Rankings, ClearCounts);
            Assert.AreEqual("%T09XXXXXHMR1", replacer.Replace("%T09XXXXXHMR1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ClearReplacer(Rankings, ClearCounts);
            Assert.AreEqual("%T09CLEARYMR1", replacer.Replace("%T09CLEARYMR1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new ClearReplacer(Rankings, ClearCounts);
            Assert.AreEqual("%T09CLEARHXX1", replacer.Replace("%T09CLEARHXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new ClearReplacer(Rankings, ClearCounts);
            Assert.AreEqual("%T09CLEARHMRX", replacer.Replace("%T09CLEARHMRX"));
        }
    }
}
