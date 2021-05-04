using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th128;
using ThScoreFileConverterTests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th128.StageProgress>;

namespace ThScoreFileConverterTests.Models.Th128
{
    [TestClass]
    public class ClearDataTests
    {
        internal static Mock<IClearData> MockClearData()
        {
            static IScoreData CreateScoreData(int index)
            {
                var mock = new Mock<IScoreData>();
                _ = mock.SetupGet(s => s.Score).Returns(12345670u - ((uint)index * 1000u));
                _ = mock.SetupGet(s => s.StageProgress).Returns(StageProgress.A2Clear);
                _ = mock.SetupGet(s => s.ContinueCount).Returns((byte)index);
                _ = mock.SetupGet(s => s.Name).Returns(TestUtils.CP932Encoding.GetBytes($"Player{index}\0\0\0"));
                _ = mock.SetupGet(s => s.DateTime).Returns(34567890u);
                _ = mock.SetupGet(s => s.SlowRate).Returns(1.2f);
                return mock.Object;
            }

            var levels = EnumHelper<Level>.Enumerable;

            var mock = new Mock<IClearData>();
            _ = mock.SetupGet(m => m.Signature).Returns("CR");
            _ = mock.SetupGet(m => m.Version).Returns(3);
            _ = mock.SetupGet(m => m.Checksum).Returns(0u);
            _ = mock.SetupGet(m => m.Size).Returns(0x66C);
            _ = mock.SetupGet(m => m.Route).Returns(RouteWithTotal.A2);
            _ = mock.SetupGet(m => m.Rankings).Returns(
                levels.ToDictionary(
                    level => level,
                    level => Enumerable.Range(0, 10).Select(index => CreateScoreData(index)).ToList()
                        as IReadOnlyList<IScoreData>));
            _ = mock.SetupGet(m => m.TotalPlayCount).Returns(23);
            _ = mock.SetupGet(m => m.PlayTime).Returns(4567890);
            _ = mock.SetupGet(m => m.ClearCounts).Returns(
                levels.ToDictionary(level => level, level => 100 - (int)level));
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
                clearData.Rankings.Values.SelectMany(
                    ranking => ranking.Select(scoreData => ScoreDataTests.MakeByteArray(scoreData))),
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

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            var clearData = new ClearData(chapter);

            Validate(mock.Object, clearData);
            Assert.IsFalse(clearData.IsValid);
        }

        [TestMethod]
        public void ClearDataTestInvalidSignature()
        {
            var mock = MockClearData();
            var signature = mock.Object.Signature;
            _ = mock.SetupGet(m => m.Signature).Returns(signature.ToLowerInvariant());

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => new ClearData(chapter));
        }

        [TestMethod]
        public void ClearDataTestInvalidVersion()
        {
            var mock = MockClearData();
            var version = mock.Object.Version;
            _ = mock.SetupGet(m => m.Version).Returns(++version);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => new ClearData(chapter));
        }

        [TestMethod]
        public void ClearDataTestInvalidSize()
        {
            var mock = MockClearData();
            var size = mock.Object.Size;
            _ = mock.SetupGet(m => m.Size).Returns(--size);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
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
}
