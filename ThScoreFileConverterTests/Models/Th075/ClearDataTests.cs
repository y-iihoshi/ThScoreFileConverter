using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th075;
using ThScoreFileConverterTests.Models.Th075.Stubs;
using ThScoreFileConverterTests.UnitTesting;

namespace ThScoreFileConverterTests.Models.Th075
{
    [TestClass]
    public class ClearDataTests
    {
        internal static Mock<IClearData> MockInitialClearData()
        {
            var mock = new Mock<IClearData>();
            _ = mock.SetupGet(m => m.MaxBonuses).Returns(new List<int>());
            _ = mock.SetupGet(m => m.CardGotCount).Returns(new List<short>());
            _ = mock.SetupGet(m => m.CardTrialCount).Returns(new List<short>());
            _ = mock.SetupGet(m => m.CardTrulyGot).Returns(new List<byte>());
            _ = mock.SetupGet(m => m.Ranking).Returns(new List<IHighScore>());
            return mock;
        }

        internal static Mock<IClearData> MockClearData()
        {
            var mock = new Mock<IClearData>();
            _ = mock.SetupGet(m => m.UseCount).Returns(1234);
            _ = mock.SetupGet(m => m.ClearCount).Returns(2345);
            _ = mock.SetupGet(m => m.MaxCombo).Returns(3456);
            _ = mock.SetupGet(m => m.MaxDamage).Returns(4567);
            _ = mock.SetupGet(m => m.MaxBonuses).Returns(Enumerable.Range(9, 100).ToList());
            _ = mock.SetupGet(m => m.CardGotCount).Returns(
                Enumerable.Range(8, 100).Select(count => (short)count).ToList());
            _ = mock.SetupGet(m => m.CardTrialCount).Returns(
                Enumerable.Range(7, 100).Select(count => (short)count).ToList());
            _ = mock.SetupGet(m => m.CardTrulyGot).Returns(
                Enumerable.Range(6, 100).Select(got => (byte)got).ToList());
            _ = mock.SetupGet(m => m.Ranking).Returns(
                Enumerable.Range(0, 10)
                    .Select(index => new HighScoreStub()
                    {
                        EncodedName = new byte[] { 15, 37, 26, 50, 30, 43, (byte)(52 + index), 103 },
                        Name = Utils.Format("Player{0} ", index),
                        Month = (byte)(1 + index),
                        Day = (byte)(10 + index),
                        Score = 1234567 + index,
                    } as IHighScore).ToList());
            return mock;
        }

        internal static byte[] MakeByteArray(IClearData clearData)
            => TestUtils.MakeByteArray(
                clearData.UseCount,
                clearData.ClearCount,
                clearData.MaxCombo,
                clearData.MaxDamage,
                clearData.MaxBonuses.ToArray(),
                new byte[0xC8],
                clearData.CardGotCount.ToArray(),
                new byte[0x64],
                clearData.CardTrialCount.ToArray(),
                new byte[0x64],
                clearData.CardTrulyGot.ToArray(),
                new byte[0x38],
                clearData.Ranking.SelectMany(
                    element => HighScoreTests.MakeByteArray((HighScoreStub)element)).ToArray());

        internal static void Validate(IClearData expected, IClearData actual)
        {
            Assert.AreEqual(expected.UseCount, actual.UseCount);
            Assert.AreEqual(expected.ClearCount, actual.ClearCount);
            Assert.AreEqual(expected.MaxCombo, actual.MaxCombo);
            Assert.AreEqual(expected.MaxDamage, actual.MaxDamage);
            CollectionAssert.That.AreEqual(expected.MaxBonuses, actual.MaxBonuses);
            CollectionAssert.That.AreEqual(expected.CardGotCount, actual.CardGotCount);
            CollectionAssert.That.AreEqual(expected.CardTrialCount, actual.CardTrialCount);
            CollectionAssert.That.AreEqual(expected.CardTrulyGot, actual.CardTrulyGot);
            Assert.AreEqual(expected.Ranking.Count, actual.Ranking.Count);
            foreach (var index in Enumerable.Range(0, expected.Ranking.Count))
            {
                var highScoreStub = expected.Ranking[index];
                var highScore = actual.Ranking[index];
                Assert.AreEqual(highScoreStub.Name, highScore.Name);
                Assert.AreEqual(highScoreStub.Month, highScore.Month);
                Assert.AreEqual(highScoreStub.Day, highScore.Day);
                Assert.AreEqual(highScoreStub.Score, highScore.Score);
            }
        }

        [TestMethod]
        public void ClearDataTest()
        {
            var mock = MockInitialClearData();
            var clearData = new ClearData();

            Validate(mock.Object, clearData);
        }

        [TestMethod]
        public void ReadFromTest()
        {
            var mock = MockClearData();
            var clearData = TestUtils.Create<ClearData>(MakeByteArray(mock.Object));

            Validate(mock.Object, clearData);
        }
    }
}
