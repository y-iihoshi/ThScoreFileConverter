using System.IO;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th15;
using ThScoreFileConverter.Models.Th15;
using ThScoreFileConverter.Tests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;
using GameMode = ThScoreFileConverter.Core.Models.Th15.GameMode;
using IPractice = ThScoreFileConverter.Models.Th10.IPractice;
using StagePractice = ThScoreFileConverter.Core.Models.Th14.StagePractice;

namespace ThScoreFileConverter.Tests.Models.Th15;

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

    internal static void Validate(IClearData expected, IClearData actual)
    {
        Assert.AreEqual(expected.Signature, actual.Signature);
        Assert.AreEqual(expected.Version, actual.Version);
        Assert.AreEqual(expected.Checksum, actual.Checksum);
        Assert.AreEqual(expected.Size, actual.Size);
        Assert.AreEqual(expected.Chara, actual.Chara);

        foreach (var pair in expected.GameModeData)
        {
            ClearDataPerGameModeTests.Validate(pair.Value, actual.GameModeData[pair.Key]);
        }

        foreach (var pair in expected.Practices)
        {
            Th10.PracticeTests.Validate(pair.Value, actual.Practices[pair.Key]);
        }
    }

    [TestMethod]
    public void ClearDataTestChapter()
    {
        var mock = MockClearData();

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var clearData = new ClearData(chapter);

        Validate(mock, clearData);
        Assert.IsFalse(clearData.IsValid);
    }

    [TestMethod]
    public void ClearDataTestInvalidSignature()
    {
        var mock = MockClearData();
        var signature = mock.Signature;
        _ = mock.Signature.Returns(signature.ToLowerInvariant());

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidDataException>(() => new ClearData(chapter));
    }

    [TestMethod]
    public void ClearDataTestInvalidVersion()
    {
        var mock = MockClearData();
        var version = mock.Version;
        _ = mock.Version.Returns(++version);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidDataException>(() => new ClearData(chapter));
    }

    [TestMethod]
    public void ClearDataTestInvalidSize()
    {
        var mock = MockClearData();
        var size = mock.Size;
        _ = mock.Size.Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidDataException>(() => new ClearData(chapter));
    }

    [DataTestMethod]
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

        Assert.AreEqual(expected, ClearData.CanInitialize(chapter));
    }
}
