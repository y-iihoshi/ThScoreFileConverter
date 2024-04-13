using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th075;
using ThScoreFileConverter.Tests.Models.Th075.Stubs;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th075;

[TestClass]
public class ClearDataTests
{
    internal static IClearData MockInitialClearData()
    {
        var mock = Substitute.For<IClearData>();
        _ = mock.MaxBonuses.Returns([]);
        _ = mock.CardGotCount.Returns([]);
        _ = mock.CardTrialCount.Returns([]);
        _ = mock.CardTrulyGot.Returns([]);
        _ = mock.Ranking.Returns([]);
        return mock;
    }

    internal static IClearData MockClearData()
    {
        var mock = Substitute.For<IClearData>();
        _ = mock.UseCount.Returns(1234);
        _ = mock.ClearCount.Returns(2345);
        _ = mock.MaxCombo.Returns(3456);
        _ = mock.MaxDamage.Returns(4567);
        _ = mock.MaxBonuses.Returns(Enumerable.Range(9, 100).ToList());
        _ = mock.CardGotCount.Returns(Enumerable.Range(8, 100).Select(count => (short)count).ToList());
        _ = mock.CardTrialCount.Returns(Enumerable.Range(7, 100).Select(count => (short)count).ToList());
        _ = mock.CardTrulyGot.Returns(Enumerable.Range(6, 100).Select(got => (byte)got).ToList());
        _ = mock.Ranking.Returns(
            Enumerable.Range(0, 10)
                .Select(index => new HighScoreStub()
                {
                    EncodedName = [15, 37, 26, 50, 30, 43, (byte)(52 + index), 103],
                    Name = StringHelper.Create($"Player{index} "),
                    Month = (byte)(1 + index),
                    Day = (byte)(10 + index),
                    Score = 1234567 + index,
                } as IHighScore).ToList());
        return mock;
    }

    internal static byte[] MakeByteArray(IClearData clearData)
    {
        return TestUtils.MakeByteArray(
            clearData.UseCount,
            clearData.ClearCount,
            clearData.MaxCombo,
            clearData.MaxDamage,
            clearData.MaxBonuses,
            new byte[0xC8],
            clearData.CardGotCount,
            new byte[0x64],
            clearData.CardTrialCount,
            new byte[0x64],
            clearData.CardTrulyGot,
            new byte[0x38],
            clearData.Ranking.Select(element => HighScoreTests.MakeByteArray((HighScoreStub)element)));
    }

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

        Validate(mock, clearData);
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var mock = MockClearData();
        var clearData = TestUtils.Create<ClearData>(MakeByteArray(mock));

        Validate(mock, clearData);
    }
}
