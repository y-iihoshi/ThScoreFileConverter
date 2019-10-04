using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th075;
using ThScoreFileConverterTests.Extensions;

namespace ThScoreFileConverterTests.Models.Th075
{
    [TestClass]
    public class StatusTests
    {
        internal struct Properties
        {
            public byte[] encodedLastName;
            public string decodedLastName;
            public Dictionary<Chara, IReadOnlyDictionary<Chara, int>> arcadeScores;

            public Properties(in Properties properties)
            {
                this.encodedLastName = properties.encodedLastName.ToArray();
                this.decodedLastName = properties.decodedLastName;
                this.arcadeScores = new Dictionary<Chara, IReadOnlyDictionary<Chara, int>>(properties.arcadeScores);
            }
        };

        internal static Properties ValidProperties => new Properties()
        {
            encodedLastName = new byte[] { 15, 37, 26, 50, 30, 43, 53, 103 },
            decodedLastName = "Player1 ",
            arcadeScores = Utils.GetEnumerator<Chara>().ToDictionary(
                chara => chara,
                chara => Utils.GetEnumerator<Chara>().ToDictionary(
                    enemy => enemy,
                    enemy => (int)chara * 100 + (int)enemy) as IReadOnlyDictionary<Chara, int>)
        };

        internal static byte[] MakeByteArray(in Properties properties)
        {
            var numCharas = Enum.GetValues(typeof(Chara)).Length;
            var numUnknownCharas = 4;
            int plus10(int value) => value + 10;

            return TestUtils.MakeByteArray(
                properties.encodedLastName,
                properties.arcadeScores[Chara.Reimu].Values.Select(plus10).ToArray(),
                new int[numUnknownCharas],
                properties.arcadeScores[Chara.Marisa].Values.Select(plus10).ToArray(),
                new int[numUnknownCharas],
                properties.arcadeScores[Chara.Sakuya].Values.Select(plus10).ToArray(),
                new int[numUnknownCharas],
                properties.arcadeScores[Chara.Alice].Values.Select(plus10).ToArray(),
                new int[numUnknownCharas],
                properties.arcadeScores[Chara.Patchouli].Values.Select(plus10).ToArray(),
                new int[numUnknownCharas],
                properties.arcadeScores[Chara.Youmu].Values.Select(plus10).ToArray(),
                new int[numUnknownCharas],
                properties.arcadeScores[Chara.Remilia].Values.Select(plus10).ToArray(),
                new int[numUnknownCharas],
                properties.arcadeScores[Chara.Yuyuko].Values.Select(plus10).ToArray(),
                new int[numUnknownCharas],
                properties.arcadeScores[Chara.Yukari].Values.Select(plus10).ToArray(),
                new int[numUnknownCharas],
                properties.arcadeScores[Chara.Suika].Values.Select(plus10).ToArray(),
                new int[numUnknownCharas],
                properties.arcadeScores[Chara.Meiling].Values.Select(plus10).ToArray(),
                new int[numUnknownCharas],
                new int[(numCharas + numUnknownCharas) * numUnknownCharas],
                new byte[0x128]);
        }

        internal static void Validate(in Properties properties, in Status status)
        {
            Assert.AreEqual(properties.decodedLastName, status.LastName);

            foreach (var chara in Utils.GetEnumerator<Chara>())
            {
                CollectionAssert.That.AreEqual(
                    properties.arcadeScores[chara].Values, status.ArcadeScores[chara].Values);
            }
        }

        [TestMethod]
        public void StatusTest() => TestUtils.Wrap(() =>
        {
            var status = new Status();

            Assert.IsNull(status.LastName);
            Assert.IsNull(status.ArcadeScores);
        });

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var status = TestUtils.Create<Status>(MakeByteArray(properties));

            Validate(properties, status);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var status = new Status();
            status.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortenedName() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.encodedLastName =
                properties.encodedLastName.Take(properties.encodedLastName.Length - 1).ToArray();

            _ = TestUtils.Create<Status>(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestExceededName() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.encodedLastName = properties.encodedLastName.Concat(new byte[1] { 1 }).ToArray();

            var status = TestUtils.Create<Status>(MakeByteArray(properties));

            Assert.AreEqual("Player1 ", status.LastName);
            CollectionAssert.That.AreNotEqual(
                properties.arcadeScores[Chara.Reimu].Values, status.ArcadeScores[Chara.Reimu].Values);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortenedArcadeScores() => TestUtils.Wrap(() =>
        {
            var properties = new Properties(ValidProperties);
            var scores = properties.arcadeScores[Chara.Meiling]
                .Where(pair => pair.Key != Chara.Meiling)
                .ToDictionary(pair => pair.Key, pair => pair.Value);
            properties.arcadeScores[Chara.Meiling] = scores;

            _ = TestUtils.Create<Status>(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestExceededArcadeScores() => TestUtils.Wrap(() =>
        {
            var properties = new Properties(ValidProperties);
            var scores = properties.arcadeScores[Chara.Meiling]
                .ToDictionary(pair => pair.Key, pair => pair.Value);
            scores.Add(TestUtils.Cast<Chara>(99), 99);
            properties.arcadeScores[Chara.Meiling] = scores;

            var status = TestUtils.Create<Status>(MakeByteArray(properties));

            Validate(ValidProperties, status);
        });
    }
}
