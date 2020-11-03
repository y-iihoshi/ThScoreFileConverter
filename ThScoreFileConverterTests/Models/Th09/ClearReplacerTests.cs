using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th09;

namespace ThScoreFileConverterTests.Models.Th09
{
    [TestClass]
    public class ClearReplacerTests
    {
        internal static IReadOnlyDictionary<(Chara, Level), IReadOnlyList<IHighScore>> Rankings { get; } =
            new[] { new[] { HighScoreTests.MockHighScore().Object } }.ToDictionary(
                ranking => (ranking[0].Chara, ranking[0].Level), ranking => ranking as IReadOnlyList<IHighScore>);

        internal static IReadOnlyDictionary<Chara, IClearCount> ClearCounts { get; }

        internal static IReadOnlyDictionary<Chara, IClearCount> ZeroClearCounts { get; }

        static ClearReplacerTests()
        {
            var highScoreMock = HighScoreTests.MockHighScore();
            var clearCountMock = ClearCountTests.MockClearCount();
            _ = clearCountMock.SetupGet(m => m.Counts).Returns(
                Utils.GetEnumerable<Level>().ToDictionary(level => level, _ => 0));

            ClearCounts =
                new[] { (highScoreMock.Object.Chara, ClearCountTests.MockClearCount().Object) }.ToDictionary();
            ZeroClearCounts = new[] { (highScoreMock.Object.Chara, clearCountMock.Object) }.ToDictionary();
        }

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
            var mock = HighScoreTests.MockHighScore();
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
            {
                { (mock.Object.Chara, mock.Object.Level), new List<IHighScore>() },
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
            var mock = HighScoreTests.MockHighScore();
            _ = mock.SetupGet(m => m.Date).Returns(TestUtils.CP932Encoding.GetBytes("--/--\0"));
            var rankings = new[] { new[] { mock.Object } }.ToDictionary(
                ranking => (ranking[0].Chara, ranking[0].Level), ranking => ranking as IReadOnlyList<IHighScore>);
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
            var mock = HighScoreTests.MockHighScore();
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
            {
                { (mock.Object.Chara, mock.Object.Level), new List<IHighScore>() },
            };
            var replacer = new ClearReplacer(rankings, ZeroClearCounts);
            Assert.AreEqual("-------", replacer.Replace("%T09CLEARHMR2"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentLevel()
        {
            var highScoreMock = HighScoreTests.MockHighScore();
            var clearCountMock = ClearCountTests.MockClearCount();
            var counts = clearCountMock.Object.Counts;
            _ = clearCountMock.SetupGet(m => m.Counts).Returns(
                counts.Where(pair => pair.Key != Level.Normal).ToDictionary());
            var clearCounts = new[] { (highScoreMock.Object.Chara, clearCountMock.Object) }.ToDictionary();
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
