using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th10ScoreDataTests
    {
        internal struct Properties<TStageProgress>
            where TStageProgress : struct, Enum
        {
            public uint score;
            public TStageProgress stageProgress;
            public byte continueCount;
            public byte[] name;
            public uint dateTime;
            public float slowRate;
        };

        internal static Properties<TStageProgress> GetValidProperties<TStageProgress>()
            where TStageProgress : struct, Enum
            => new Properties<TStageProgress>()
            {
                score = 12u,
                stageProgress = TestUtils.Cast<TStageProgress>(3),
                continueCount = 4,
                name = TestUtils.MakeRandomArray<byte>(10),
                dateTime = 567u,
                slowRate = 8.9f
            };

        internal static byte[] MakeByteArray<TParent, TStageProgress>(in Properties<TStageProgress> properties)
            where TParent : ThConverter
            where TStageProgress : struct, Enum
        {
            var unknownSize = 0;

            var type = typeof(TParent);
            if (type == typeof(Th10Converter))
            {
                unknownSize = 0;
            }
            else if (type == typeof(Th128Converter))
            {
                unknownSize = 8;
            }
            else
            {
                unknownSize = 4;
            }

            return TestUtils.MakeByteArray(
                properties.score,
                TestUtils.Cast<byte>(properties.stageProgress),
                properties.continueCount,
                properties.name,
                properties.dateTime,
                properties.slowRate,
                new byte[unknownSize]);
        }

        internal static void Validate<TParent, TStageProgress>(
            in Th10ScoreDataWrapper<TParent, TStageProgress> scoreData, in Properties<TStageProgress> properties)
            where TParent : ThConverter
            where TStageProgress : struct, Enum
        {
            Assert.AreEqual(properties.score, scoreData.Score);
            Assert.AreEqual(properties.stageProgress, scoreData.StageProgress);
            Assert.AreEqual(properties.continueCount, scoreData.ContinueCount);
            CollectionAssert.AreEqual(properties.name, scoreData.Name?.ToArray());
            Assert.AreEqual(properties.dateTime, scoreData.DateTime);
            Assert.AreEqual(properties.slowRate, scoreData.SlowRate);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th10ScoreDataTestHelper<TParent, TStageProgress>()
            where TParent : ThConverter
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = new Properties<TStageProgress>();
                var scoreData = new Th10ScoreDataWrapper<TParent, TStageProgress>();

                Validate(scoreData, properties);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th10ScoreDataReadFromTestHelper<TParent, TStageProgress>()
            where TParent : ThConverter
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties<TStageProgress>();

                var scoreData = Th10ScoreDataWrapper<TParent, TStageProgress>.Create(
                    MakeByteArray<TParent, TStageProgress>(properties));

                Validate(scoreData, properties);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th10ScoreDataReadFromTestNullHelper<TParent, TStageProgress>()
            where TParent : ThConverter
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var scoreData = new Th10ScoreDataWrapper<TParent, TStageProgress>();
                scoreData.ReadFrom(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "scoreData")]
        internal static void Th10ScoreDataReadFromTestShortenedNameHelper<TParent, TStageProgress>()
            where TParent : ThConverter
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties<TStageProgress>();
                properties.name = properties.name.Take(properties.name.Length - 1).ToArray();

                var scoreData = Th10ScoreDataWrapper<TParent, TStageProgress>.Create(
                    MakeByteArray<TParent, TStageProgress>(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th10ScoreDataReadFromTestExceededNameHelper<TParent, TStageProgress>()
            where TParent : ThConverter
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties<TStageProgress>();
                properties.name = properties.name.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray();

                var scoreData = Th10ScoreDataWrapper<TParent, TStageProgress>.Create(
                    MakeByteArray<TParent, TStageProgress>(properties));

                Assert.AreEqual(properties.score, scoreData.Score);
                Assert.AreEqual(properties.stageProgress, scoreData.StageProgress);
                Assert.AreEqual(properties.continueCount, scoreData.ContinueCount);
                CollectionAssert.AreNotEqual(properties.name, scoreData.Name.ToArray());
                CollectionAssert.AreEqual(
                    properties.name.Take(properties.name.Length - 1).ToArray(), scoreData.Name.ToArray());
                Assert.AreNotEqual(properties.dateTime, scoreData.DateTime);
                Assert.AreNotEqual(properties.slowRate, scoreData.SlowRate);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "scoreData")]
        internal static void Th10ScoreDataReadFromTestInvalidStageProgressHelper<TParent, TStageProgress>(
            int stageProgress)
            where TParent : ThConverter
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties<TStageProgress>();
                properties.stageProgress = TestUtils.Cast<TStageProgress>(stageProgress);

                var scoreData = Th10ScoreDataWrapper<TParent, TStageProgress>.Create(
                    MakeByteArray<TParent, TStageProgress>(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        public static IEnumerable<object[]> Th10InvalidStageProgresses
            => TestUtils.GetInvalidEnumerators(typeof(Th10Converter.StageProgress));

        public static IEnumerable<object[]> Th11InvalidStageProgresses
            => TestUtils.GetInvalidEnumerators(typeof(Th11Converter.StageProgress));

        public static IEnumerable<object[]> Th12InvalidStageProgresses
            => TestUtils.GetInvalidEnumerators(typeof(Th12Converter.StageProgress));

        public static IEnumerable<object[]> Th128InvalidStageProgresses
            => TestUtils.GetInvalidEnumerators(typeof(Th128Converter.StageProgress));

        public static IEnumerable<object[]> Th13InvalidStageProgresses
            => TestUtils.GetInvalidEnumerators(typeof(Th13Converter.StageProgress));

        public static IEnumerable<object[]> Th14InvalidStageProgresses
            => TestUtils.GetInvalidEnumerators(typeof(Th14Converter.StageProgress));

        #region Th10

        [TestMethod()]
        public void Th10ScoreDataTest()
            => Th10ScoreDataTestHelper<Th10Converter, Th10Converter.StageProgress>();

        [TestMethod()]
        public void Th10ScoreDataReadFromTest()
            => Th10ScoreDataReadFromTestHelper<Th10Converter, Th10Converter.StageProgress>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th10ScoreDataReadFromTestNull()
            => Th10ScoreDataReadFromTestNullHelper<Th10Converter, Th10Converter.StageProgress>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th10ScoreDataReadFromTestShortenedName()
            => Th10ScoreDataReadFromTestShortenedNameHelper<Th10Converter, Th10Converter.StageProgress>();

        [TestMethod()]
        public void Th10ScoreDataReadFromTestExceededName()
            => Th10ScoreDataReadFromTestExceededNameHelper<Th10Converter, Th10Converter.StageProgress>();

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(Th10InvalidStageProgresses))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th10ScoreDataReadFromTestInvalidStageProgress(int stageProgress)
            => Th10ScoreDataReadFromTestInvalidStageProgressHelper<Th10Converter, Th10Converter.StageProgress>(
                stageProgress);

        #endregion

        #region Th11

        [TestMethod()]
        public void Th11ScoreDataTest()
            => Th10ScoreDataTestHelper<Th11Converter, Th11Converter.StageProgress>();

        [TestMethod()]
        public void Th11ScoreDataReadFromTest()
            => Th10ScoreDataReadFromTestHelper<Th11Converter, Th11Converter.StageProgress>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th11ScoreDataReadFromTestNull()
            => Th10ScoreDataReadFromTestNullHelper<Th11Converter, Th11Converter.StageProgress>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th11ScoreDataReadFromTestShortenedName()
            => Th10ScoreDataReadFromTestShortenedNameHelper<Th11Converter, Th11Converter.StageProgress>();

        [TestMethod()]
        public void Th11ScoreDataReadFromTestExceededName()
            => Th10ScoreDataReadFromTestExceededNameHelper<Th11Converter, Th11Converter.StageProgress>();

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(Th10InvalidStageProgresses))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th11ScoreDataReadFromTestInvalidStageProgress(int stageProgress)
            => Th10ScoreDataReadFromTestInvalidStageProgressHelper<Th11Converter, Th11Converter.StageProgress>(
                stageProgress);

        #endregion

        #region Th12

        [TestMethod()]
        public void Th12ScoreDataTest()
            => Th10ScoreDataTestHelper<Th12Converter, Th12Converter.StageProgress>();

        [TestMethod()]
        public void Th12ScoreDataReadFromTest()
            => Th10ScoreDataReadFromTestHelper<Th12Converter, Th12Converter.StageProgress>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th12ScoreDataReadFromTestNull()
            => Th10ScoreDataReadFromTestNullHelper<Th12Converter, Th12Converter.StageProgress>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th12ScoreDataReadFromTestShortenedName()
            => Th10ScoreDataReadFromTestShortenedNameHelper<Th12Converter, Th12Converter.StageProgress>();

        [TestMethod()]
        public void Th12ScoreDataReadFromTestExceededName()
            => Th10ScoreDataReadFromTestExceededNameHelper<Th12Converter, Th12Converter.StageProgress>();

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(Th12InvalidStageProgresses))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th12ScoreDataReadFromTestInvalidStageProgress(int stageProgress)
            => Th10ScoreDataReadFromTestInvalidStageProgressHelper<Th12Converter, Th12Converter.StageProgress>(
                stageProgress);

        #endregion

        #region Th128

        [TestMethod()]
        public void Th128ScoreDataTest()
            => Th10ScoreDataTestHelper<Th128Converter, Th128Converter.StageProgress>();

        [TestMethod()]
        public void Th128ScoreDataReadFromTest()
            => Th10ScoreDataReadFromTestHelper<Th128Converter, Th128Converter.StageProgress>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th128ScoreDataReadFromTestNull()
            => Th10ScoreDataReadFromTestNullHelper<Th128Converter, Th128Converter.StageProgress>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th128ScoreDataReadFromTestShortenedName()
            => Th10ScoreDataReadFromTestShortenedNameHelper<Th128Converter, Th128Converter.StageProgress>();

        [TestMethod()]
        public void Th128ScoreDataReadFromTestExceededName()
            => Th10ScoreDataReadFromTestExceededNameHelper<Th128Converter, Th128Converter.StageProgress>();

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(Th128InvalidStageProgresses))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th128ScoreDataReadFromTestInvalidStageProgress(int stageProgress)
            => Th10ScoreDataReadFromTestInvalidStageProgressHelper<Th128Converter, Th128Converter.StageProgress>(
                stageProgress);

        #endregion

        #region Th13

        [TestMethod()]
        public void Th13ScoreDataTest()
            => Th10ScoreDataTestHelper<Th13Converter, Th13Converter.StageProgress>();

        [TestMethod()]
        public void Th13ScoreDataReadFromTest()
            => Th10ScoreDataReadFromTestHelper<Th13Converter, Th13Converter.StageProgress>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th13ScoreDataReadFromTestNull()
            => Th10ScoreDataReadFromTestNullHelper<Th13Converter, Th13Converter.StageProgress>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th13ScoreDataReadFromTestShortenedName()
            => Th10ScoreDataReadFromTestShortenedNameHelper<Th13Converter, Th13Converter.StageProgress>();

        [TestMethod()]
        public void Th13ScoreDataReadFromTestExceededName()
            => Th10ScoreDataReadFromTestExceededNameHelper<Th13Converter, Th13Converter.StageProgress>();

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(Th13InvalidStageProgresses))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th13ScoreDataReadFromTestInvalidStageProgress(int stageProgress)
            => Th10ScoreDataReadFromTestInvalidStageProgressHelper<Th13Converter, Th13Converter.StageProgress>(
                stageProgress);

        #endregion

        #region Th14

        [TestMethod()]
        public void Th14ScoreDataTest()
            => Th10ScoreDataTestHelper<Th14Converter, Th14Converter.StageProgress>();

        [TestMethod()]
        public void Th14ScoreDataReadFromTest()
            => Th10ScoreDataReadFromTestHelper<Th14Converter, Th14Converter.StageProgress>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th14ScoreDataReadFromTestNull()
            => Th10ScoreDataReadFromTestNullHelper<Th14Converter, Th14Converter.StageProgress>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th14ScoreDataReadFromTestShortenedName()
            => Th10ScoreDataReadFromTestShortenedNameHelper<Th14Converter, Th14Converter.StageProgress>();

        [TestMethod()]
        public void Th14ScoreDataReadFromTestExceededName()
            => Th10ScoreDataReadFromTestExceededNameHelper<Th14Converter, Th14Converter.StageProgress>();

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(Th14InvalidStageProgresses))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th14ScoreDataReadFromTestInvalidStageProgress(int stageProgress)
            => Th10ScoreDataReadFromTestInvalidStageProgressHelper<Th14Converter, Th14Converter.StageProgress>(
                stageProgress);

        #endregion
    }
}
