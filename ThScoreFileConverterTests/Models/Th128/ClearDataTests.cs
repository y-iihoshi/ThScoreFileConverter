using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th128;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th128.Stubs;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th128.StageProgress>;

namespace ThScoreFileConverterTests.Models.Th128
{
    [TestClass]
    public class ClearDataTests
    {
        internal static ClearDataStub MakeValidStub()
        {
            var levels = Utils.GetEnumerable<Level>();

            return new ClearDataStub()
            {
                Signature = "CR",
                Version = 3,
                Checksum = 0u,
                Size = 0x66C,
                Route = RouteWithTotal.A2,
                Rankings = levels.ToDictionary(
                    level => level,
                    level => Enumerable.Range(0, 10).Select(
                        index => Mock.Of<IScoreData>(
                            m => (m.Score == 12345670u - ((uint)index * 1000u))
                                 && (m.StageProgress == StageProgress.A2Clear)
                                 && (m.ContinueCount == (byte)index)
                                 && (m.Name == TestUtils.CP932Encoding.GetBytes($"Player{index}\0\0\0"))
                                 && (m.DateTime == 34567890u)
                                 && (m.SlowRate == 1.2f))).ToList() as IReadOnlyList<IScoreData>),
                TotalPlayCount = 23,
                PlayTime = 4567890,
                ClearCounts = levels.ToDictionary(level => level, level => 100 - (int)level),
            };
        }

        internal static byte[] MakeData(IClearData clearData)
            => TestUtils.MakeByteArray(
                (int)clearData.Route,
                clearData.Rankings.Values.SelectMany(
                    ranking => ranking.SelectMany(
                        scoreData => ScoreDataTests.MakeByteArray(scoreData))).ToArray(),
                clearData.TotalPlayCount,
                clearData.PlayTime,
                clearData.ClearCounts.Values.ToArray());

        internal static byte[] MakeByteArray(IClearData clearData)
            => TestUtils.MakeByteArray(
                clearData.Signature.ToCharArray(),
                clearData.Version,
                clearData.Checksum,
                clearData.Size,
                MakeData(clearData));

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
            var stub = MakeValidStub();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            var clearData = new ClearData(chapter);

            Validate(stub, clearData);
            Assert.IsFalse(clearData.IsValid);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ClearDataTestNullChapter()
        {
            _ = new ClearData(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ClearDataTestInvalidSignature()
        {
            var stub = MakeValidStub();
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new ClearData(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ClearDataTestInvalidVersion()
        {
            var stub = MakeValidStub();
            ++stub.Version;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new ClearData(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ClearDataTestInvalidSize()
        {
            var stub = MakeValidStub();
            --stub.Size;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new ClearData(chapter);

            Assert.Fail(TestUtils.Unreachable);
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
