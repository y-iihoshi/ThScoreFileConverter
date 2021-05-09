using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th06;
using IHighScore = ThScoreFileConverter.Models.Th06.IHighScore<
    ThScoreFileConverter.Models.Th06.Chara,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th06.StageProgress>;

namespace ThScoreFileConverterTests.Models.Th06
{
    [TestClass]
    public class ClearReplacerTests
    {
        private static IEnumerable<IReadOnlyList<IHighScore>> CreateRankings()
        {
            var mock1 = HighScoreTests.MockHighScore();
            var mock2 = HighScoreTests.MockHighScore();
            _ = mock2.SetupGet(m => m.StageProgress).Returns(mock1.Object.StageProgress + 1);
            return new[] { new[] { mock1.Object, mock2.Object } };
        }

        internal static IReadOnlyDictionary<(Chara, Level), IReadOnlyList<IHighScore>> Rankings { get; } =
            CreateRankings().ToDictionary(ranking => (ranking[0].Chara, ranking[0].Level));

        [TestMethod]
        public void ClearReplacerTest()
        {
            var replacer = new ClearReplacer(Rankings);
            Assert.IsNotNull(replacer);
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
            var mock = HighScoreTests.MockHighScore();
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
            {
                { (mock.Object.Chara, mock.Object.Level), ImmutableList<IHighScore>.Empty },
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
            var mock = HighScoreTests.MockHighScore();
            _ = mock.SetupGet(m => m.Level).Returns(Level.Extra);
            _ = mock.SetupGet(m => m.StageProgress).Returns(StageProgress.Extra);
            var rankings = new[] { new[] { mock.Object } }.ToDictionary(
                ranking => (ranking[0].Chara, ranking[0].Level), ranking => ranking as IReadOnlyList<IHighScore>);
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
            var mock = HighScoreTests.MockHighScore();
            var rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>
            {
                { (mock.Object.Chara, mock.Object.Level), ImmutableList<IHighScore>.Empty },
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
