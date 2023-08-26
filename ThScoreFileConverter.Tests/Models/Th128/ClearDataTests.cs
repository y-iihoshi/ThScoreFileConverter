using System.Collections.Generic;
using System.IO;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th128;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using ThScoreFileConverter.Models.Th128;
using ThScoreFileConverter.Tests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th128.StageProgress>;

namespace ThScoreFileConverter.Tests.Models.Th128;

[TestClass]
public class ClearDataTests
{
    internal static IClearData MockClearData()
    {
        static IScoreData MockScoreData(int index)
        {
            var mock = Substitute.For<IScoreData>();
            _ = mock.Score.Returns(12345670u - ((uint)index * 1000u));
            _ = mock.StageProgress.Returns(StageProgress.A2Clear);
            _ = mock.ContinueCount.Returns((byte)index);
            _ = mock.Name.Returns(TestUtils.CP932Encoding.GetBytes($"Player{index}\0\0\0"));
            _ = mock.DateTime.Returns(34567890u);
            _ = mock.SlowRate.Returns(1.2f);
            return mock;
        }

        var levels = EnumHelper<Level>.Enumerable;
        var rankings = levels.ToDictionary(
            level => level,
            level => Enumerable.Range(0, 10).Select(MockScoreData).ToList() as IReadOnlyList<IScoreData>);

        var mock = Substitute.For<IClearData>();
        _ = mock.Signature.Returns("CR");
        _ = mock.Version.Returns((ushort)3);
        _ = mock.Checksum.Returns(0u);
        _ = mock.Size.Returns(0x66C);
        _ = mock.Route.Returns(RouteWithTotal.A2);
        _ = mock.Rankings.Returns(rankings);
        _ = mock.TotalPlayCount.Returns(23);
        _ = mock.PlayTime.Returns(4567890);
        _ = mock.ClearCounts.Returns(levels.ToDictionary(level => level, level => 100 - (int)level));
        return mock;
    }

    internal static byte[] MakeByteArray(IClearData clearData)
    {
        return TestUtils.MakeByteArray(
            clearData.Signature.ToCharArray(),
            clearData.Version,
            clearData.Checksum,
            clearData.Size,
            (int)clearData.Route,
            clearData.Rankings.Values.SelectMany(ranking => ranking.Select(ScoreDataTests.MakeByteArray)),
            clearData.TotalPlayCount,
            clearData.PlayTime,
            clearData.ClearCounts.Values);
    }

    internal static void Validate(IClearData expected, IClearData actual)
    {
        Assert.AreEqual(expected.Signature, actual.Signature);
        Assert.AreEqual(expected.Version, actual.Version);
        Assert.AreEqual(expected.Checksum, actual.Checksum);
        Assert.AreEqual(expected.Size, actual.Size);
        Assert.AreEqual(expected.Route, actual.Route);

        foreach (var pair in expected.Rankings)
        {
            for (var index = 0; index < pair.Value.Count; ++index)
            {
                Th10.ScoreDataTests.Validate(pair.Value[index], actual.Rankings[pair.Key][index]);
            }
        }

        Assert.AreEqual(expected.TotalPlayCount, actual.TotalPlayCount);
        Assert.AreEqual(expected.PlayTime, actual.PlayTime);
        CollectionAssert.That.AreEqual(expected.ClearCounts.Values, actual.ClearCounts.Values);
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
    [DataRow("CR", (ushort)3, 0x66C, true)]
    [DataRow("cr", (ushort)3, 0x66C, false)]
    [DataRow("CR", (ushort)2, 0x66C, false)]
    [DataRow("CR", (ushort)3, 0x66D, false)]
    public void CanInitializeTest(string signature, ushort version, int size, bool expected)
    {
        var checksum = 0u;
        var data = new byte[size];

        var chapter = TestUtils.Create<Chapter>(
            TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

        Assert.AreEqual(expected, ClearData.CanInitialize(chapter));
    }
}
