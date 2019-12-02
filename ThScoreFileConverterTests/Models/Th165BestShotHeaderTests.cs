using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th165;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th165.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th165BestShotHeaderTests
    {
        internal static BestShotHeaderStub ValidStub { get; } = new BestShotHeaderStub
        {
            Signature = "BST4",
            Weekday = Day.Monday,
            Dream = 3,
            Width = 4,
            Height = 5,
            Width2 = 6,
            Height2 = 7,
            HalfWidth = 8,
            HalfHeight = 9,
            SlowRate = 10f,
            DateTime = 11u,
            Angle = 12f,
            Score = 13,
            Fields = new HashtagFields(14, 15, 16),
            Score2 = 17,
            BasePoint = 18,
            NumViewed = 19,
            NumLikes = 20,
            NumFavs = 21,
            NumBullets = 22,
            NumBulletsNearby = 23,
            RiskBonus = 24,
            BossShot = 25f,
            AngleBonus = 26f,
            MacroBonus = 27,
            LikesPerView = 28f,
            FavsPerView = 29f,
            NumHashtags = 30,
            NumRedBullets = 31,
            NumPurpleBullets = 32,
            NumBlueBullets = 33,
            NumCyanBullets = 34,
            NumGreenBullets = 35,
            NumYellowBullets = 36,
            NumOrangeBullets = 37,
            NumLightBullets = 38,
        };

        internal static byte[] MakeByteArray(IBestShotHeader header)
            => TestUtils.MakeByteArray(
                header.Signature.ToCharArray(),
                (ushort)0,
                TestUtils.Cast<short>(header.Weekday),
                (short)(header.Dream - 1),
                (ushort)0,
                header.Width,
                header.Height,
                0u,
                header.Width2,
                header.Height2,
                header.HalfWidth,
                header.HalfHeight,
                0u,
                header.SlowRate,
                header.DateTime,
                0u,
                header.Angle,
                header.Score,
                0u,
                header.Fields.Data.ToArray(),
                TestUtils.MakeRandomArray<byte>(0x28),
                header.Score2,
                header.BasePoint,
                header.NumViewed,
                header.NumLikes,
                header.NumFavs,
                header.NumBullets,
                header.NumBulletsNearby,
                header.RiskBonus,
                header.BossShot,
                0u,
                header.AngleBonus,
                header.MacroBonus,
                0u,
                0u,
                header.LikesPerView,
                header.FavsPerView,
                header.NumHashtags,
                header.NumRedBullets,
                header.NumPurpleBullets,
                header.NumBlueBullets,
                header.NumCyanBullets,
                header.NumGreenBullets,
                header.NumYellowBullets,
                header.NumOrangeBullets,
                header.NumLightBullets,
                TestUtils.MakeRandomArray<byte>(0x78));

        internal static void Validate(IBestShotHeader expected, in Th165BestShotHeaderWrapper actual)
        {
            if (actual == null)
                throw new ArgumentNullException(nameof(actual));

            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Weekday, actual.Weekday);
            Assert.AreEqual(expected.Dream, actual.Dream);
            Assert.AreEqual(expected.Width, actual.Width);
            Assert.AreEqual(expected.Height, actual.Height);
            Assert.AreEqual(expected.Width2, actual.Width2);
            Assert.AreEqual(expected.Height2, actual.Height2);
            Assert.AreEqual(expected.HalfWidth, actual.HalfWidth);
            Assert.AreEqual(expected.HalfHeight, actual.HalfHeight);
            Assert.AreEqual(expected.SlowRate, actual.SlowRate);
            Assert.AreEqual(expected.DateTime, actual.DateTime);
            Assert.AreEqual(expected.Angle, actual.Angle);
            Assert.AreEqual(expected.Score, actual.Score);
            CollectionAssert.That.AreEqual(expected.Fields.Data, actual.Fields.Value.Data);
            Assert.AreEqual(expected.Score2, actual.Score2);
            Assert.AreEqual(expected.BasePoint, actual.BasePoint);
            Assert.AreEqual(expected.NumViewed, actual.NumViewed);
            Assert.AreEqual(expected.NumLikes, actual.NumLikes);
            Assert.AreEqual(expected.NumFavs, actual.NumFavs);
            Assert.AreEqual(expected.NumBullets, actual.NumBullets);
            Assert.AreEqual(expected.NumBulletsNearby, actual.NumBulletsNearby);
            Assert.AreEqual(expected.RiskBonus, actual.RiskBonus);
            Assert.AreEqual(expected.BossShot, actual.BossShot);
            Assert.AreEqual(expected.AngleBonus, actual.AngleBonus);
            Assert.AreEqual(expected.MacroBonus, actual.MacroBonus);
            Assert.AreEqual(expected.LikesPerView, actual.LikesPerView);
            Assert.AreEqual(expected.FavsPerView, actual.FavsPerView);
            Assert.AreEqual(expected.NumHashtags, actual.NumHashtags);
            Assert.AreEqual(expected.NumRedBullets, actual.NumRedBullets);
            Assert.AreEqual(expected.NumPurpleBullets, actual.NumPurpleBullets);
            Assert.AreEqual(expected.NumBlueBullets, actual.NumBlueBullets);
            Assert.AreEqual(expected.NumCyanBullets, actual.NumCyanBullets);
            Assert.AreEqual(expected.NumGreenBullets, actual.NumGreenBullets);
            Assert.AreEqual(expected.NumYellowBullets, actual.NumYellowBullets);
            Assert.AreEqual(expected.NumOrangeBullets, actual.NumOrangeBullets);
            Assert.AreEqual(expected.NumLightBullets, actual.NumLightBullets);
        }

        [TestMethod]
        public void Th165BestShotHeaderTest() => TestUtils.Wrap(() =>
        {
            var stub = new BestShotHeaderStub();
            var header = new Th165BestShotHeaderWrapper();

            Validate(stub, header);
        });

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var header = Th165BestShotHeaderWrapper.Create(MakeByteArray(stub));

            Validate(stub, header);
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
            var stub = new BestShotHeaderStub(ValidStub)
            {
                Signature = string.Empty,
            };

            _ = Th165BestShotHeaderWrapper.Create(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ReadFromTestShortenedSignature() => TestUtils.Wrap(() =>
        {
            var stub = new BestShotHeaderStub(ValidStub);
            stub.Signature = stub.Signature.Substring(0, stub.Signature.Length - 1);

            _ = Th165BestShotHeaderWrapper.Create(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestExceededSignature() => TestUtils.Wrap(() =>
        {
            var stub = new BestShotHeaderStub(ValidStub);
            stub.Signature += "E";

            _ = Th165BestShotHeaderWrapper.Create(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidDays
            => TestUtils.GetInvalidEnumerators(typeof(Day));

        [DataTestMethod]
        [DynamicData(nameof(InvalidDays))]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestInvalidDay(int day) => TestUtils.Wrap(() =>
        {
            var stub = new BestShotHeaderStub(ValidStub)
            {
                Weekday = TestUtils.Cast<Day>(day),
            };

            _ = Th165BestShotHeaderWrapper.Create(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
