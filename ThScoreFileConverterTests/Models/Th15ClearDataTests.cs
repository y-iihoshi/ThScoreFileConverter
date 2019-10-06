using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th13;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th10.Wrappers;
using ThScoreFileConverterTests.Models.Th13;
using ThScoreFileConverterTests.Models.Th13.Stubs;
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
            public Dictionary<(Level, Th15Converter.StagePractice), IPractice> practices;
        };

        internal static Properties GetValidProperties()
        {
            var modes = Utils.GetEnumerator<Th15Converter.GameMode>();
            var levels = Utils.GetEnumerator<Level>();
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
                    .SelectMany(level => stages.Select(stage => (level, stage)))
                    .ToDictionary(
                        pair => pair,
                        pair => new PracticeStub()
                        {
                            Score = 123456u - TestUtils.Cast<uint>(pair.level) * 10u,
                            ClearFlag = (byte)(TestUtils.Cast<int>(pair.stage) % 2),
                            EnableFlag = (byte)(TestUtils.Cast<int>(pair.level) % 2)
                        } as IPractice)
            };
        }

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(
                (int)properties.chara,
                properties.data1.Values.SelectMany(
                    data => Th15ClearDataPerGameModeTests.MakeByteArray(data)).ToArray(),
                properties.practices.Values.SelectMany(
                    practice => PracticeTests.MakeByteArray(practice)).ToArray(),
                new byte[0x40]);

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
            CollectionAssert.That.AreEqual(data, clearData.Data);
            Assert.AreEqual(properties.chara, clearData.Chara);

            foreach (var pair in properties.data1)
            {
                Th15ClearDataPerGameModeTests.Validate(clearData.Data1Item(pair.Key), pair.Value);
            }

            foreach (var pair in properties.practices)
            {
                PracticeTests.Validate(pair.Value, clearData.Practices[pair.Key]);
            }
        }


        [TestMethod]
        public void Th15ClearDataTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
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

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
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

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var clearData = new Th15ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th15ClearDataTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();
            --properties.size;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
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

                var chapter = ChapterWrapper.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(expected, Th15ClearDataWrapper.CanInitialize(chapter));
            });
    }
}
