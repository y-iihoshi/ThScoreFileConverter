using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th15;
using ThScoreFileConverter.Models.Th15;
using static ThScoreFileConverter.Tests.Models.Th10.PracticeExtensions;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;
using GameMode = ThScoreFileConverter.Core.Models.Th15.GameMode;
using IPractice = ThScoreFileConverter.Models.Th10.IPractice;
using StagePractice = ThScoreFileConverter.Core.Models.Th14.StagePractice;

namespace ThScoreFileConverter.Tests.Models.Th15;

internal static class ClearDataExtensions
{
    internal static void ShouldBe(this IClearData actual, IClearData expected)
    {
        actual.Signature.ShouldBe(expected.Signature);
        actual.Version.ShouldBe(expected.Version);
        actual.Checksum.ShouldBe(expected.Checksum);
        actual.Size.ShouldBe(expected.Size);
        actual.Chara.ShouldBe(expected.Chara);

        foreach (var pair in expected.GameModeData)
        {
            actual.GameModeData[pair.Key].ShouldBe(pair.Value);
        }

        foreach (var pair in expected.Practices)
        {
            actual.Practices[pair.Key].ShouldBe(pair.Value);
        }
    }
}

[TestClass]
public class ClearDataTests
{
    internal static IClearData MockClearData()
    {
        static IPractice MockPractice((Level, StagePractice) pair)
        {
            var mock = Substitute.For<IPractice>();
            _ = mock.Score.Returns(123456u - ((uint)pair.Item1 * 10u));
            _ = mock.Cleared.Returns((byte)((int)pair.Item2 % 2));
            _ = mock.Unlocked.Returns((byte)((int)pair.Item1 % 2));
            return mock;
        }

        var gameModeData = EnumHelper<GameMode>.Enumerable.ToDictionary(
            mode => mode,
            _ => ClearDataPerGameModeTests.MockClearDataPerGameMode());
        var practices = EnumHelper.Cartesian<Level, StagePractice>().ToDictionary(pair => pair, MockPractice);

        var mock = Substitute.For<IClearData>();
        _ = mock.Signature.Returns("CR");
        _ = mock.Version.Returns((ushort)1);
        _ = mock.Checksum.Returns(0u);
        _ = mock.Size.Returns(0xA4A0);
        _ = mock.Chara.Returns(CharaWithTotal.Marisa);
        _ = mock.GameModeData.Returns(gameModeData);
        _ = mock.Practices.Returns(practices);
        return mock;
    }

    internal static byte[] MakeByteArray(IClearData clearData)
    {
        return TestUtils.MakeByteArray(
            clearData.Signature.ToCharArray(),
            clearData.Version,
            clearData.Checksum,
            clearData.Size,
            (int)clearData.Chara,
            clearData.GameModeData.Values.Select(ClearDataPerGameModeTests.MakeByteArray),
            clearData.Practices.Values.Select(Th10.PracticeTests.MakeByteArray),
            new byte[0x40]);
    }

    [TestMethod]
    public void ClearDataTestChapter()
    {
        var mock = MockClearData();

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var clearData = new ClearData(chapter);

        clearData.ShouldBe(mock);
        clearData.IsValid.ShouldBeFalse();
    }

    [TestMethod]
    public void ClearDataTestInvalidSignature()
    {
        var mock = MockClearData();
        var signature = mock.Signature;
        _ = mock.Signature.Returns(signature.ToLowerInvariant());

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new ClearData(chapter));
    }

    [TestMethod]
    public void ClearDataTestInvalidVersion()
    {
        var mock = MockClearData();
        var version = mock.Version;
        _ = mock.Version.Returns(++version);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new ClearData(chapter));
    }

    [TestMethod]
    public void ClearDataTestInvalidSize()
    {
        var mock = MockClearData();
        var size = mock.Size;
        _ = mock.Size.Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new ClearData(chapter));
    }

    [TestMethod]
    [DataRow("CR", (ushort)1, 0xA4A0, true)]
    [DataRow("cr", (ushort)1, 0xA4A0, false)]
    [DataRow("CR", (ushort)0, 0xA4A0, false)]
    [DataRow("CR", (ushort)1, 0xA4A1, false)]
    public void CanInitializeTest(string signature, ushort version, int size, bool expected)
    {
        var checksum = 0u;
        var data = new byte[size];

        var chapter = TestUtils.Create<Chapter>(
            TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

        ClearData.CanInitialize(chapter).ShouldBe(expected);
    }
}
