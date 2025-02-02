using ThScoreFileConverter.Models.Th075;
using ThScoreFileConverter.Tests.Models.Th075.Stubs;

namespace ThScoreFileConverter.Tests.Models.Th075;

internal static class HighScoreExtensions
{
    internal static void ShouldBe(this IHighScore actual, IHighScore expected)
    {
        actual.Name.ShouldBe(expected.Name);
        actual.Month.ShouldBe(expected.Month);
        actual.Day.ShouldBe(expected.Day);
        actual.Score.ShouldBe(expected.Score);
    }
}

[TestClass]
public class HighScoreTests
{
    internal static HighScoreStub ValidStub { get; } = new HighScoreStub()
    {
        EncodedName = [15, 37, 26, 50, 30, 43, 53, 103],
        Name = "Player1 ",
        Month = 6,
        Day = 15,
        Score = 1234567,
    };

    internal static byte[] MakeByteArray(in HighScoreStub stub)
    {
        return TestUtils.MakeByteArray(
            stub.EncodedName,
            stub.Month,
            stub.Day,
            new byte[2],
            stub.Score);
    }

    [TestMethod]
    public void HighScoreTest()
    {
        var highScore = new HighScore();

        highScore.Name.ShouldBeEmpty();
        highScore.Month.ShouldBe(default);
        highScore.Day.ShouldBe(default);
        highScore.Score.ShouldBe(default);
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var highScore = TestUtils.Create<HighScore>(MakeByteArray(ValidStub));

        highScore.ShouldBe(ValidStub);
    }

    [TestMethod]
    public void ReadFromTestShortenedName()
    {
        var stub = new HighScoreStub(ValidStub);
        stub.EncodedName = stub.EncodedName.SkipLast(1).ToArray();

        _ = Should.Throw<InvalidDataException>(() => TestUtils.Create<HighScore>(MakeByteArray(stub)));
    }

    [TestMethod]
    public void ReadFromTestExceededName()
    {
        var stub = new HighScoreStub(ValidStub);
        stub.EncodedName = stub.EncodedName.Concat([default]).ToArray();

        _ = Should.Throw<InvalidDataException>(() => TestUtils.Create<HighScore>(MakeByteArray(stub)));
    }

    [DataTestMethod]
    [DataRow(0, 0)]
    [DataRow(1, 1)]
    [DataRow(1, 31)]
    [DataRow(2, 1)]
    [DataRow(2, 28)]
    [DataRow(2, 29)]
    [DataRow(3, 1)]
    [DataRow(3, 31)]
    [DataRow(4, 1)]
    [DataRow(4, 30)]
    [DataRow(5, 1)]
    [DataRow(5, 31)]
    [DataRow(6, 1)]
    [DataRow(6, 30)]
    [DataRow(7, 1)]
    [DataRow(7, 31)]
    [DataRow(8, 1)]
    [DataRow(8, 31)]
    [DataRow(9, 1)]
    [DataRow(9, 30)]
    [DataRow(10, 1)]
    [DataRow(10, 31)]
    [DataRow(11, 1)]
    [DataRow(11, 30)]
    [DataRow(12, 1)]
    [DataRow(12, 31)]
    public void ReadFromTestValidMonthDay(int month, int day)
    {
        var stub = new HighScoreStub(ValidStub)
        {
            Month = (byte)month,
            Day = (byte)day,
        };

        var highScore = TestUtils.Create<HighScore>(MakeByteArray(stub));

        highScore.ShouldBe(stub);
    }

    [DataTestMethod]
    [DataRow(0, -1)]
    [DataRow(0, 1)]
    [DataRow(1, 0)]
    [DataRow(1, 32)]
    [DataRow(2, 0)]
    [DataRow(2, 30)]
    [DataRow(3, 0)]
    [DataRow(3, 32)]
    [DataRow(4, 0)]
    [DataRow(4, 31)]
    [DataRow(5, 0)]
    [DataRow(5, 32)]
    [DataRow(6, 0)]
    [DataRow(6, 31)]
    [DataRow(7, 0)]
    [DataRow(7, 32)]
    [DataRow(8, 0)]
    [DataRow(8, 32)]
    [DataRow(9, 0)]
    [DataRow(9, 31)]
    [DataRow(10, 0)]
    [DataRow(10, 32)]
    [DataRow(11, 0)]
    [DataRow(11, 31)]
    [DataRow(12, 0)]
    [DataRow(12, 32)]
    public void ReadFromTestInvalidMonthDay(int month, int day)
    {
        var properties = new HighScoreStub(ValidStub)
        {
            Month = (byte)month,
            Day = (byte)day,
        };

        _ = Should.Throw<InvalidDataException>(
            () => TestUtils.Create<HighScore>(MakeByteArray(properties)));
    }
}
