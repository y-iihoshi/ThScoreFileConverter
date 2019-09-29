using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th165BestShotHeaderTests
    {
        internal struct Properties
        {
            public string signature;
            public Th165Converter.Day weekday;
            public short dream;
            public short width;
            public short height;
            public short width2;
            public short height2;
            public short halfWidth;
            public short halfHeight;
            public float slowRate;
            public uint dateTime;
            public float angle;
            public int score;
            public int[] fields;
            public int score2;
            public int basePoint;
            public int numViewed;
            public int numLikes;
            public int numFavs;
            public int numBullets;
            public int numBulletsNearby;
            public int riskBonus;
            public float bossShot;
            public float angleBonus;
            public int macroBonus;
            public float likesPerView;
            public float favsPerView;
            public int numHashtags;
            public int numRedBullets;
            public int numPurpleBullets;
            public int numBlueBullets;
            public int numCyanBullets;
            public int numGreenBullets;
            public int numYellowBullets;
            public int numOrangeBullets;
            public int numLightBullets;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "BST4",
            weekday = Th165Converter.Day.Monday,
            dream = 3,
            width = 4,
            height = 5,
            width2 = 6,
            height2 = 7,
            halfWidth = 8,
            halfHeight = 9,
            slowRate = 10f,
            dateTime = 11u,
            angle = 12f,
            score = 13,
            fields = new int[] { 14, 15, 16 },
            score2 = 17,
            basePoint = 18,
            numViewed = 19,
            numLikes = 20,
            numFavs = 21,
            numBullets = 22,
            numBulletsNearby = 23,
            riskBonus = 24,
            bossShot = 25f,
            angleBonus = 26f,
            macroBonus = 27,
            likesPerView = 28f,
            favsPerView = 29f,
            numHashtags = 30,
            numRedBullets = 31,
            numPurpleBullets = 32,
            numBlueBullets = 33,
            numCyanBullets = 34,
            numGreenBullets = 35,
            numYellowBullets = 36,
            numOrangeBullets = 37,
            numLightBullets = 38,
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(),
                (ushort)0,
                TestUtils.Cast<short>(properties.weekday),
                (short)(properties.dream - 1),
                (ushort)0,
                properties.width,
                properties.height,
                0u,
                properties.width2,
                properties.height2,
                properties.halfWidth,
                properties.halfHeight,
                0u,
                properties.slowRate,
                properties.dateTime,
                0u,
                properties.angle,
                properties.score,
                0u,
                properties.fields,
                TestUtils.MakeRandomArray<byte>(0x28),
                properties.score2,
                properties.basePoint,
                properties.numViewed,
                properties.numLikes,
                properties.numFavs,
                properties.numBullets,
                properties.numBulletsNearby,
                properties.riskBonus,
                properties.bossShot,
                0u,
                properties.angleBonus,
                properties.macroBonus,
                0u,
                0u,
                properties.likesPerView,
                properties.favsPerView,
                properties.numHashtags,
                properties.numRedBullets,
                properties.numPurpleBullets,
                properties.numBlueBullets,
                properties.numCyanBullets,
                properties.numGreenBullets,
                properties.numYellowBullets,
                properties.numOrangeBullets,
                properties.numLightBullets,
                TestUtils.MakeRandomArray<byte>(0x78));

        internal static void Validate(in Th165BestShotHeaderWrapper header, in Properties properties)
        {
            if (header == null)
                throw new ArgumentNullException(nameof(header));

            Assert.AreEqual(properties.signature, header.Signature);
            Assert.AreEqual(properties.weekday, header.Weekday);
            Assert.AreEqual(properties.dream, header.Dream);
            Assert.AreEqual(properties.width, header.Width);
            Assert.AreEqual(properties.height, header.Height);
            Assert.AreEqual(properties.width2, header.Width2);
            Assert.AreEqual(properties.height2, header.Height2);
            Assert.AreEqual(properties.halfWidth, header.HalfWidth);
            Assert.AreEqual(properties.halfHeight, header.HalfHeight);
            Assert.AreEqual(properties.slowRate, header.SlowRate);
            Assert.AreEqual(properties.dateTime, header.DateTime);
            Assert.AreEqual(properties.angle, header.Angle);
            Assert.AreEqual(properties.score, header.Score);
            CollectionAssert.That.AreEqual(properties.fields, header.Fields.Data);
            Assert.AreEqual(properties.score2, header.Score2);
            Assert.AreEqual(properties.basePoint, header.BasePoint);
            Assert.AreEqual(properties.numViewed, header.NumViewed);
            Assert.AreEqual(properties.numLikes, header.NumLikes);
            Assert.AreEqual(properties.numFavs, header.NumFavs);
            Assert.AreEqual(properties.numBullets, header.NumBullets);
            Assert.AreEqual(properties.numBulletsNearby, header.NumBulletsNearby);
            Assert.AreEqual(properties.riskBonus, header.RiskBonus);
            Assert.AreEqual(properties.bossShot, header.BossShot);
            Assert.AreEqual(properties.angleBonus, header.AngleBonus);
            Assert.AreEqual(properties.macroBonus, header.MacroBonus);
            Assert.AreEqual(properties.likesPerView, header.LikesPerView);
            Assert.AreEqual(properties.favsPerView, header.FavsPerView);
            Assert.AreEqual(properties.numHashtags, header.NumHashtags);
            Assert.AreEqual(properties.numRedBullets, header.NumRedBullets);
            Assert.AreEqual(properties.numPurpleBullets, header.NumPurpleBullets);
            Assert.AreEqual(properties.numBlueBullets, header.NumBlueBullets);
            Assert.AreEqual(properties.numCyanBullets, header.NumCyanBullets);
            Assert.AreEqual(properties.numGreenBullets, header.NumGreenBullets);
            Assert.AreEqual(properties.numYellowBullets, header.NumYellowBullets);
            Assert.AreEqual(properties.numOrangeBullets, header.NumOrangeBullets);
            Assert.AreEqual(properties.numLightBullets, header.NumLightBullets);
        }

        [TestMethod]
        public void Th165BestShotHeaderTest() => TestUtils.Wrap(() =>
        {
            var properties = new Properties();
            var header = new Th165BestShotHeaderWrapper();

            Validate(header, properties);
        });

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var header = Th165BestShotHeaderWrapper.Create(MakeByteArray(properties));

            Validate(header, properties);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var header = new Th165BestShotHeaderWrapper();

            header.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ReadFromTestEmptySignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = string.Empty;

            _ = Th165BestShotHeaderWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "header")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ReadFromTestShortenedSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.Substring(0, properties.signature.Length - 1);

            var header = Th165BestShotHeaderWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "header")]
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestExceededSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature += "E";

            var header = Th165BestShotHeaderWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidDays
            => TestUtils.GetInvalidEnumerators(typeof(Th165Converter.Day));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "header")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidDays))]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestInvalidDay(int day) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.weekday = TestUtils.Cast<Th165Converter.Day>(day);

            var header = Th165BestShotHeaderWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
