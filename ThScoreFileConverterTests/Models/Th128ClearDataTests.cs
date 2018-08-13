using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th128ClearDataTests
    {
        internal struct Properties
        {
            public string signature;
            public ushort version;
            public uint checksum;
            public int size;
            public Th128Converter.RouteWithTotal route;
            public Dictionary<
                ThConverter.Level, Th10ScoreDataTests.Properties<Th128Converter.StageProgress>[]> rankings;
            public int totalPlayCount;
            public int playTime;
            public Dictionary<ThConverter.Level, int> clearCounts;
        };

        internal static Properties GetValidProperties()
        {
            var levels = Utils.GetEnumerator<ThConverter.Level>();

            return new Properties()
            {
                signature = "CR",
                version = 3,
                checksum = 0u,
                size = 0x66C,
                route = Th128Converter.RouteWithTotal.A2,
                rankings = levels.ToDictionary(
                    level => level,
                    level => Enumerable.Range(0, 10).Select(
                        index => new Th10ScoreDataTests.Properties<Th128Converter.StageProgress>()
                        {
                            score = 12345670u - (uint)index * 1000u,
                            stageProgress = Th128Converter.StageProgress.A2Clear,
                            continueCount = (byte)index,
                            name = TestUtils.MakeRandomArray<byte>(10),
                            dateTime = 34567890u,
                            slowRate = 1.2f
                        }).ToArray()),
                totalPlayCount = 23,
                playTime = 4567890,
                clearCounts = levels.ToDictionary(level => level, level => 100 - (int)level)
            };
        }

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(
                (int)properties.route,
                properties.rankings.Values.SelectMany(
                    ranking => ranking.SelectMany(
                        scoreData => Th10ScoreDataTests.MakeByteArray<Th128Converter, Th128Converter.StageProgress>(
                            scoreData))).ToArray(),
                properties.totalPlayCount,
                properties.playTime,
                properties.clearCounts.Values.ToArray());

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(),
                properties.version,
                properties.checksum,
                properties.size,
                MakeData(properties));

        internal static void Validate(in Th128ClearDataWrapper clearData, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, clearData.Signature);
            Assert.AreEqual(properties.version, clearData.Version);
            Assert.AreEqual(properties.checksum, clearData.Checksum);
            Assert.AreEqual(properties.size, clearData.Size);
            CollectionAssert.AreEqual(data, clearData.Data.ToArray());
            Assert.AreEqual(properties.route, clearData.Route);

            foreach (var pair in properties.rankings)
            {
                for (var index = 0; index < pair.Value.Length; ++index)
                {
                    Th10ScoreDataTests.Validate(clearData.RankingItem(pair.Key, index), pair.Value[index]);
                }
            }

            Assert.AreEqual(properties.totalPlayCount, clearData.TotalPlayCount);
            Assert.AreEqual(properties.playTime, clearData.PlayTime);
            CollectionAssert.AreEqual(properties.clearCounts.Values, clearData.ClearCounts.Values.ToArray());
        }

        [TestMethod]
        public void Th128ClearDataTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();

            var chapter = Th10ChapterWrapper<Th128Converter>.Create(MakeByteArray(properties));
            var clearData = new Th128ClearDataWrapper(chapter);

            Validate(clearData, properties);
            Assert.IsFalse(clearData.IsValid.Value);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th128ClearDataTestNullChapter() => TestUtils.Wrap(() =>
        {
            var clearData = new Th128ClearDataWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th128ClearDataTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();
            properties.signature = properties.signature.ToLowerInvariant(); 

            var chapter = Th10ChapterWrapper<Th128Converter>.Create(MakeByteArray(properties));
            var clearData = new Th128ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th128ClearDataTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();
            ++properties.version;

            var chapter = Th10ChapterWrapper<Th128Converter>.Create(MakeByteArray(properties));
            var clearData = new Th128ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th128ClearDataTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();
            ++properties.size;

            var chapter = Th10ChapterWrapper<Th128Converter>.Create(MakeByteArray(properties));
            var clearData = new Th128ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("CR", (ushort)3, 0x66C, true)]
        [DataRow("cr", (ushort)3, 0x66C, false)]
        [DataRow("CR", (ushort)2, 0x66C, false)]
        [DataRow("CR", (ushort)3, 0x66D, false)]
        public void Th128ClearDataCanInitializeTest(string signature, ushort version, int size, bool expected)
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = Th10ChapterWrapper<Th128Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(expected, Th128ClearDataWrapper.CanInitialize(chapter));
            });
    }
}
