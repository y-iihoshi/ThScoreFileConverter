using NSubstitute;
using ThScoreFileConverter.Models.Th10;

namespace ThScoreFileConverter.Tests.Models.Th10;

[TestClass]
public class PracticeTests
{
    internal static IPractice MockPractice()
    {
        var mock = Substitute.For<IPractice>();
        _ = mock.Score.Returns(123456u);
        _ = mock.Cleared.Returns((byte)7);
        _ = mock.Unlocked.Returns((byte)8);
        return mock;
    }

    internal static byte[] MakeByteArray(IPractice practice)
    {
        return TestUtils.MakeByteArray(practice.Score, practice.Cleared, practice.Unlocked, (ushort)0);
    }

    internal static void Validate(IPractice expected, IPractice actual)
    {
        Assert.AreEqual(expected.Score, actual.Score);
        Assert.AreEqual(expected.Cleared, actual.Cleared);
        Assert.AreEqual(expected.Unlocked, actual.Unlocked);
    }

    [TestMethod]
    public void PracticeTest()
    {
        var mock = Substitute.For<IPractice>();

        var practice = new Practice();

        Validate(mock, practice);
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var mock = MockPractice();

        var practice = TestUtils.Create<Practice>(MakeByteArray(mock));

        Validate(mock, practice);
    }
}
