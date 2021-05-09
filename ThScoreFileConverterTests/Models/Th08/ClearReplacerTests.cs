using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;
using IHighScore = ThScoreFileConverter.Models.Th08.IHighScore;

namespace ThScoreFileConverterTests.Models.Th08
{
    [TestClass]
    public class ClearReplacerTests
    {
        private static IEnumerable<IReadOnlyList<IHighScore>> CreateRankings()
        {
            var mock1 = HighScoreTests.MockHighScore();
            var mock2 = HighScoreTests.MockHighScore();
            _ = mock2.SetupGet(m => m.StageProgress).Returns(mock1.Object.StageProgress - 1);
            return new[] { new[] { mock1.Object, mock2.Object } };
        }

        internal static IReadOnlyDictionary<(Chara, Level), IReadOnlyList<IHighScore>> Rankings { get; } =
            CreateRankings().ToDictionary(ranking => (ranking[0].Chara, ranking[0].Level));

        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearData { get; } =
            new[] { ClearDataTests.MockClearData().Object }.ToDictionary(entry => entry.Chara);

        [TestMethod]
        public void ClearReplacerTest()
        {
            var replacer = new ClearReplacer(Rankings, ClearData);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ClearReplacerTestEmptyRankings()
        {
            var rankings = ImmutableDictionary<(Chara, Level), IReadOnlyList<IHighScore>>.Empty;
            var replacer = new ClearReplacer(rankings, ClearData);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ClearReplacerTestEmptyScores()
        {
            var mock = HighScoreTests.MockHighScore();
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
            {
                { (mock.Object.Chara, mock.Object.Level), ImmutableList<IHighScore>.Empty },
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
            var mock = HighScoreTests.MockHighScore();
            _ = mock.SetupGet(m => m.StageProgress).Returns(StageProgress.FourUncanny);
            var rankings = new[] { new[] { mock.Object } }.ToDictionary(
                ranking => (ranking[0].Chara, ranking[0].Level), ranking => ranking as IReadOnlyList<IHighScore>);
            var replacer = new ClearReplacer(rankings, ClearData);
            Assert.AreEqual("Stage 4", replacer.Replace("%T08CLEARHMA"));
        }

        [TestMethod]
        public void ReplaceTestPowerful()
        {
            var mock = HighScoreTests.MockHighScore();
            _ = mock.SetupGet(m => m.StageProgress).Returns(StageProgress.FourPowerful);
            var rankings = new[] { new[] { mock.Object } }.ToDictionary(
                ranking => (ranking[0].Chara, ranking[0].Level), ranking => ranking as IReadOnlyList<IHighScore>);
            var replacer = new ClearReplacer(rankings, ClearData);
            Assert.AreEqual("Stage 4", replacer.Replace("%T08CLEARHMA"));
        }

        [TestMethod]
        public void ReplaceTestFinalAClear()
        {
            var mock = HighScoreTests.MockHighScore();
            _ = mock.SetupGet(m => m.StageProgress).Returns(StageProgress.Clear);
            var rankings = new[] { new[] { mock.Object } }.ToDictionary(
                ranking => (ranking[0].Chara, ranking[0].Level), ranking => ranking as IReadOnlyList<IHighScore>);
            var replacer = new ClearReplacer(rankings, ClearData);
            Assert.AreEqual("FinalA Clear", replacer.Replace("%T08CLEARHMA"));
        }

        [TestMethod]
        public void ReplaceTestAllClear()
        {
            var highScoreMock = HighScoreTests.MockHighScore();
            _ = highScoreMock.SetupGet(m => m.StageProgress).Returns(StageProgress.Clear);
            var rankings = new[] { new[] { highScoreMock.Object } }.ToDictionary(
                ranking => (ranking[0].Chara, ranking[0].Level), ranking => ranking as IReadOnlyList<IHighScore>);

            var clearDataMock = ClearDataTests.MockClearData();
            _ = clearDataMock.SetupGet(m => m.StoryFlags).Returns(
                EnumHelper<Level>.Enumerable.ToDictionary(level => level, _ => PlayableStages.Stage6B));
            var clearData = new[] { clearDataMock.Object }.ToDictionary(entry => entry.Chara);

            var replacer = new ClearReplacer(rankings, clearData);
            Assert.AreEqual(StageProgress.Clear.ToShortName(), replacer.Replace("%T08CLEARHMA"));
        }

        [TestMethod]
        public void ReplaceTestExtra()
        {
            var mock = HighScoreTests.MockHighScore();
            _ = mock.SetupGet(m => m.Level).Returns(Level.Extra);
            _ = mock.SetupGet(m => m.StageProgress).Returns(StageProgress.Extra);
            var rankings = new[] { new[] { mock.Object } }.ToDictionary(
                ranking => (ranking[0].Chara, ranking[0].Level), ranking => ranking as IReadOnlyList<IHighScore>);
            var replacer = new ClearReplacer(rankings, ClearData);
            Assert.AreEqual("Not Clear", replacer.Replace("%T08CLEARXMA"));
        }

        [TestMethod]
        public void ReplaceTestExtraClear()
        {
            var mock = HighScoreTests.MockHighScore();
            _ = mock.SetupGet(m => m.Level).Returns(Level.Extra);
            _ = mock.SetupGet(m => m.StageProgress).Returns(StageProgress.Clear);
            var rankings = new[] { new[] { mock.Object } }.ToDictionary(
                ranking => (ranking[0].Chara, ranking[0].Level), ranking => ranking as IReadOnlyList<IHighScore>);
            var replacer = new ClearReplacer(rankings, ClearData);
            Assert.AreEqual(StageProgress.Clear.ToShortName(), replacer.Replace("%T08CLEARXMA"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRankings()
        {
            var rankings = ImmutableDictionary<(Chara, Level), IReadOnlyList<IHighScore>>.Empty;
            var replacer = new ClearReplacer(rankings, ClearData);
            Assert.AreEqual("-------", replacer.Replace("%T08CLEARHMA"));
        }

        [TestMethod]
        public void ReplaceTestEmptyScores()
        {
            var mock = HighScoreTests.MockHighScore();
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
            {
                { (mock.Object.Chara, mock.Object.Level), ImmutableList<IHighScore>.Empty },
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
