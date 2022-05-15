using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverterTests.UnitTesting;

namespace ThScoreFileConverterTests.Models.Th10;

[TestClass]
public class PracticeTests
{
    internal static Mock<IPractice> MockPractice()
    {
        var mock = new Mock<IPractice>();
        _ = mock.SetupGet(m => m.Score).Returns(123456u);
        _ = mock.SetupGet(m => m.Cleared).Returns(7);
        _ = mock.SetupGet(m => m.Unlocked).Returns(8);
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
        var mock = new Mock<IPractice>();

        var practice = new Practice();

        Validate(mock.Object, practice);
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var mock = MockPractice();

        var practice = TestUtils.Create<Practice>(MakeByteArray(mock.Object));

        Validate(mock.Object, practice);
    }
}
