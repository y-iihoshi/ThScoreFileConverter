using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th075StatusTests
    {
        internal struct Properties
        {
            public byte[] encodedLastName;
            public string decodedLastName;
            public Dictionary<Th075Converter.Chara, Dictionary<Th075Converter.Chara, int>> arcadeScores;

            public Properties(in Properties properties)
            {
                this.encodedLastName = properties.encodedLastName.ToArray();
                this.decodedLastName = properties.decodedLastName;
                this.arcadeScores = new Dictionary<Th075Converter.Chara, Dictionary<Th075Converter.Chara, int>>(
                    properties.arcadeScores);
            }
        };

        internal static Properties ValidProperties => new Properties()
        {
            encodedLastName = new byte[] { 15, 37, 26, 50, 30, 43, 53, 103 },
            decodedLastName = "Player1 ",
            arcadeScores = Utils.GetEnumerator<Th075Converter.Chara>().ToDictionary(
                chara => chara,
                chara => Utils.GetEnumerator<Th075Converter.Chara>().ToDictionary(
                    enemy => enemy,
                    enemy => (int)chara * 100 + (int)enemy))
        };

        internal static byte[] MakeByteArray(in Properties properties)
        {
            var numCharas = Enum.GetValues(typeof(Th075Converter.Chara)).Length;
            var numUnknownCharas = 4;
            int plus10(int value) => value + 10;

            return TestUtils.MakeByteArray(
                properties.encodedLastName,
                properties.arcadeScores[Th075Converter.Chara.Reimu].Values.Select(plus10).ToArray(),
                new int[numUnknownCharas],
                properties.arcadeScores[Th075Converter.Chara.Marisa].Values.Select(plus10).ToArray(),
                new int[numUnknownCharas],
                properties.arcadeScores[Th075Converter.Chara.Sakuya].Values.Select(plus10).ToArray(),
                new int[numUnknownCharas],
                properties.arcadeScores[Th075Converter.Chara.Alice].Values.Select(plus10).ToArray(),
                new int[numUnknownCharas],
                properties.arcadeScores[Th075Converter.Chara.Patchouli].Values.Select(plus10).ToArray(),
                new int[numUnknownCharas],
                properties.arcadeScores[Th075Converter.Chara.Youmu].Values.Select(plus10).ToArray(),
                new int[numUnknownCharas],
                properties.arcadeScores[Th075Converter.Chara.Remilia].Values.Select(plus10).ToArray(),
                new int[numUnknownCharas],
                properties.arcadeScores[Th075Converter.Chara.Yuyuko].Values.Select(plus10).ToArray(),
                new int[numUnknownCharas],
                properties.arcadeScores[Th075Converter.Chara.Yukari].Values.Select(plus10).ToArray(),
                new int[numUnknownCharas],
                properties.arcadeScores[Th075Converter.Chara.Suika].Values.Select(plus10).ToArray(),
                new int[numUnknownCharas],
                properties.arcadeScores[Th075Converter.Chara.Meiling].Values.Select(plus10).ToArray(),
                new int[numUnknownCharas],
                new int[(numCharas + numUnknownCharas) * numUnknownCharas],
                new byte[0x128]);
        }

        internal static void Validate(in Th075StatusWrapper status, in Properties properties)
        {
            Assert.AreEqual(properties.decodedLastName, status.LastName);

            foreach (var chara in Utils.GetEnumerator<Th075Converter.Chara>())
            {
                CollectionAssert.AreEqual(properties.arcadeScores[chara].Values, status.ArcadeScores[chara].Values);
            }
        }

        [TestMethod]
        public void Th075StatusTest() => TestUtils.Wrap(() =>
        {
            var status = new Th075StatusWrapper();

            Assert.IsNull(status.LastName);
            Assert.IsNull(status.ArcadeScores);
        });

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var status = Th075StatusWrapper.Create(MakeByteArray(properties));

            Validate(status, properties);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var status = new Th075StatusWrapper();
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

            Th075StatusWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestExceededName() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.encodedLastName = properties.encodedLastName.Concat(new byte[1] { 1 }).ToArray();

            var status = Th075StatusWrapper.Create(MakeByteArray(properties));

            Assert.AreEqual("Player1 ", status.LastName);
            CollectionAssert.AreNotEqual(
                properties.arcadeScores[Th075Converter.Chara.Reimu].Values,
                status.ArcadeScores[Th075Converter.Chara.Reimu].Values);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortenedArcadeScores() => TestUtils.Wrap(() =>
        {
            var properties = new Properties(ValidProperties);
            properties.arcadeScores[Th075Converter.Chara.Meiling].Remove(Th075Converter.Chara.Meiling);

            Th075StatusWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestExceededArcadeScores() => TestUtils.Wrap(() =>
        {
            var properties = new Properties(ValidProperties);
            properties.arcadeScores[Th075Converter.Chara.Meiling].Add(TestUtils.Cast<Th075Converter.Chara>(99), 99);

            var status = Th075StatusWrapper.Create(MakeByteArray(properties));

            Validate(status, ValidProperties);
        });
    }
}
