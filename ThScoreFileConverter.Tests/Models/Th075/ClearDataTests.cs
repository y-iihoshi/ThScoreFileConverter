﻿using NSubstitute;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th075;
using ThScoreFileConverter.Tests.Models.Th075.Stubs;

namespace ThScoreFileConverter.Tests.Models.Th075;

internal static class ClearDataExtensions
{
    internal static void ShouldBe(this IClearData actual, IClearData expected)
    {
        actual.UseCount.ShouldBe(expected.UseCount);
        actual.ClearCount.ShouldBe(expected.ClearCount);
        actual.MaxCombo.ShouldBe(expected.MaxCombo);
        actual.MaxDamage.ShouldBe(expected.MaxDamage);
        actual.MaxBonuses.ShouldBe(expected.MaxBonuses);
        actual.CardGotCount.ShouldBe(expected.CardGotCount);
        actual.CardTrialCount.ShouldBe(expected.CardTrialCount);
        actual.CardTrulyGot.ShouldBe(expected.CardTrulyGot);
        actual.Ranking.Count.ShouldBe(expected.Ranking.Count);
        foreach (var index in Enumerable.Range(0, expected.Ranking.Count))
        {
            var expectedHighScore = expected.Ranking[index];
            var actualHighScore = actual.Ranking[index];
            actualHighScore.Name.ShouldBe(expectedHighScore.Name);
            actualHighScore.Month.ShouldBe(expectedHighScore.Month);
            actualHighScore.Day.ShouldBe(expectedHighScore.Day);
            actualHighScore.Score.ShouldBe(expectedHighScore.Score);
        }
    }
}

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
        _ = mock.MaxBonuses.Returns([.. Enumerable.Range(9, 100)]);
        _ = mock.CardGotCount.Returns([.. Enumerable.Range(8, 100).Select(count => (short)count)]);
        _ = mock.CardTrialCount.Returns([.. Enumerable.Range(7, 100).Select(count => (short)count)]);
        _ = mock.CardTrulyGot.Returns([.. Enumerable.Range(6, 100).Select(got => (byte)got)]);
        _ = mock.Ranking.Returns(
            [.. Enumerable.Range(0, 10)
                .Select(index => new HighScoreStub()
                {
                    EncodedName = [15, 37, 26, 50, 30, 43, (byte)(52 + index), 103],
                    Name = StringHelper.Create($"Player{index} "),
                    Month = (byte)(1 + index),
                    Day = (byte)(10 + index),
                    Score = 1234567 + index,
                } as IHighScore)]);
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

    [TestMethod]
    public void ClearDataTest()
    {
        var mock = MockInitialClearData();
        var clearData = new ClearData();

        clearData.ShouldBe(mock);
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var mock = MockClearData();
        var clearData = TestUtils.Create<ClearData>(MakeByteArray(mock));

        clearData.ShouldBe(mock);
    }
}
