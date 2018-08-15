using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th15ClearDataTests
    {
        internal struct Properties
        {
            public string signature;
            public ushort version;
            public uint checksum;
            public int size;
            public Th15Converter.CharaWithTotal chara;
            public Dictionary<Th15Converter.GameMode, Th15ClearDataPerGameModeTests.Properties> data1;
            public Dictionary<
                Th10LevelStagePairTests.Properties<ThConverter.Level, Th15Converter.StagePractice>,
                Th13PracticeTests.Properties> practices;
        };

        internal static Properties GetValidProperties()
        {
            var modes = Utils.GetEnumerator<Th15Converter.GameMode>();
            var levels = Utils.GetEnumerator<ThConverter.Level>();
            var stages = Utils.GetEnumerator<Th15Converter.StagePractice>();

            return new Properties()
            {
                signature = "CR",
                version = 1,
                checksum = 0u,
                size = 0xA4A0,
                chara = Th15Converter.CharaWithTotal.Reimu,
                data1 = modes.ToDictionary(mode => mode, mode => Th15ClearDataPerGameModeTests.GetValidProperties()),
                practices = levels
                    .SelectMany(level => stages.Select(stage => new { level, stage }))
                    .ToDictionary(
                        pair => new Th10LevelStagePairTests.Properties<ThConverter.Level, Th15Converter.StagePractice>()
                        {
                            level = pair.level,
                            stage = pair.stage
                        },
                        pair => new Th13PracticeTests.Properties()
                        {
                            score = 123456u - TestUtils.Cast<uint>(pair.level) * 10u,
                            clearFlag = (byte)(TestUtils.Cast<int>(pair.stage) % 2),
                            enableFlag = (byte)(TestUtils.Cast<int>(pair.level) % 2)
                        })
            };
        }

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(
                (int)properties.chara,
                properties.data1.Values.SelectMany(
                    data => Th15ClearDataPerGameModeTests.MakeByteArray(data)).ToArray(),
                properties.practices.Values.SelectMany(
                    practice => Th13PracticeTests.MakeByteArray(practice)).ToArray());

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(),
                properties.version,
                properties.checksum,
                properties.size,
                MakeData(properties));

        internal static void Validate(in Th15ClearDataWrapper clearData, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, clearData.Signature);
            Assert.AreEqual(properties.version, clearData.Version);
            Assert.AreEqual(properties.checksum, clearData.Checksum);
            Assert.AreEqual(properties.size, clearData.Size);
            CollectionAssert.AreEqual(data, clearData.Data.ToArray());
            Assert.AreEqual(properties.chara, clearData.Chara);

            foreach (var pair in properties.data1)
            {
                Th15ClearDataPerGameModeTests.Validate(clearData.Data1Item(pair.Key), pair.Value);
            }

            foreach (var pair in properties.practices)
            {
                var levelStagePair =
                    new Th10LevelStagePairWrapper<Th15Converter, ThConverter.Level, Th15Converter.StagePractice>(
                        pair.Key.level, pair.Key.stage);
                Th13PracticeTests.Validate(clearData.PracticesItem(levelStagePair), pair.Value);
            }
        }


        [TestMethod]
        public void Th15ClearDataTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();

            var chapter = Th10ChapterWrapper<Th15Converter>.Create(MakeByteArray(properties));
            var clearData = new Th15ClearDataWrapper(chapter);

            Validate(clearData, properties);
            Assert.IsFalse(clearData.IsValid.Value);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th15ClearDataTestNullChapter() => TestUtils.Wrap(() =>
        {
            var clearData = new Th15ClearDataWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th15ClearDataTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();
            properties.signature = properties.signature.ToLowerInvariant(); 

            var chapter = Th10ChapterWrapper<Th15Converter>.Create(MakeByteArray(properties));
            var clearData = new Th15ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th15ClearDataTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();
            ++properties.version;

            var chapter = Th10ChapterWrapper<Th15Converter>.Create(MakeByteArray(properties));
            var clearData = new Th15ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th15ClearDataTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();
            ++properties.size;

            var chapter = Th10ChapterWrapper<Th15Converter>.Create(MakeByteArray(properties));
            var clearData = new Th15ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("CR", (ushort)1, 0xA4A0, true)]
        [DataRow("cr", (ushort)1, 0xA4A0, false)]
        [DataRow("CR", (ushort)0, 0xA4A0, false)]
        [DataRow("CR", (ushort)1, 0xA4A1, false)]
        public void Th15ClearDataCanInitializeTest(string signature, ushort version, int size, bool expected)
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = Th10ChapterWrapper<Th15Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(expected, Th15ClearDataWrapper.CanInitialize(chapter));
            });
    }
}
