using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th10.Stubs;
using ThScoreFileConverterTests.Models.Th10.Wrappers;

namespace ThScoreFileConverterTests.Models.Th10
{
    [TestClass]
    public class ScoreDataTests
    {
        internal static ScoreDataStub<TStageProgress> GetValidStub<TStageProgress>()
            where TStageProgress : struct, Enum
            => new ScoreDataStub<TStageProgress>()
            {
                Score = 12u,
                StageProgress = TestUtils.Cast<TStageProgress>(3),
                ContinueCount = 4,
                Name = TestUtils.MakeRandomArray<byte>(10),
                DateTime = 567u,
                SlowRate = 8.9f
            };

        internal static byte[] MakeByteArray<TParent, TStageProgress>(IScoreData<TStageProgress> scoreData)
            where TParent : ThConverter
            where TStageProgress : struct, Enum
        {
            var unknownSize = 0;

            var type = typeof(TParent);
            if (type == typeof(Th10Converter))
            {
                // Do nothing
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
                scoreData.Score,
                (byte)TestUtils.Cast<int>(scoreData.StageProgress),
                scoreData.ContinueCount,
                scoreData.Name.ToArray(),
                scoreData.DateTime,
                scoreData.SlowRate,
                new byte[unknownSize]);
        }

        internal static void Validate<TParent, TStageProgress>(
            IScoreData<TStageProgress> expected, in ScoreDataWrapper<TParent, TStageProgress> actual)
            where TParent : ThConverter
            where TStageProgress : struct, Enum
        {
            Assert.AreEqual(expected.Score, actual.Score);
            Assert.AreEqual(expected.StageProgress, actual.StageProgress);
            Assert.AreEqual(expected.ContinueCount, actual.ContinueCount);
            CollectionAssert.That.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.DateTime, actual.DateTime);
            Assert.AreEqual(expected.SlowRate, actual.SlowRate);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th10ScoreDataTestHelper<TParent, TStageProgress>()
            where TParent : ThConverter
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = new ScoreDataStub<TStageProgress>();
                var scoreData = new ScoreDataWrapper<TParent, TStageProgress>();

                Validate(stub, scoreData);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th10ScoreDataReadFromTestHelper<TParent, TStageProgress>()
            where TParent : ThConverter
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = GetValidStub<TStageProgress>();

                var scoreData = ScoreDataWrapper<TParent, TStageProgress>.Create(
                    MakeByteArray<TParent, TStageProgress>(stub));

                Validate(stub, scoreData);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th10ScoreDataReadFromTestNullHelper<TParent, TStageProgress>()
            where TParent : ThConverter
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var scoreData = new ScoreDataWrapper<TParent, TStageProgress>();
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
                var stub = GetValidStub<TStageProgress>();
                stub.Name = stub.Name.SkipLast(1).ToArray();

                var scoreData = ScoreDataWrapper<TParent, TStageProgress>.Create(
                    MakeByteArray<TParent, TStageProgress>(stub));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th10ScoreDataReadFromTestExceededNameHelper<TParent, TStageProgress>()
            where TParent : ThConverter
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = GetValidStub<TStageProgress>();
                var validNameLength = stub.Name.Count();
                stub.Name = stub.Name.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray();

                var scoreData = ScoreDataWrapper<TParent, TStageProgress>.Create(
                    MakeByteArray<TParent, TStageProgress>(stub));

                Assert.AreEqual(stub.Score, scoreData.Score);
                Assert.AreEqual(stub.StageProgress, scoreData.StageProgress);
                Assert.AreEqual(stub.ContinueCount, scoreData.ContinueCount);
                CollectionAssert.That.AreNotEqual(stub.Name, scoreData.Name);
                CollectionAssert.That.AreEqual(stub.Name.Take(validNameLength), scoreData.Name);
                Assert.AreNotEqual(stub.DateTime, scoreData.DateTime);
                Assert.AreNotEqual(stub.SlowRate, scoreData.SlowRate);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th10ScoreDataReadFromTestInvalidStageProgressHelper<TParent, TStageProgress>(
            int stageProgress)
            where TParent : ThConverter
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = GetValidStub<TStageProgress>();
                stub.StageProgress = TestUtils.Cast<TStageProgress>(stageProgress);

                _ = ScoreDataWrapper<TParent, TStageProgress>.Create(MakeByteArray<TParent, TStageProgress>(stub));

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

        [TestMethod]
        public void Th10ScoreDataTest()
            => Th10ScoreDataTestHelper<Th10Converter, Th10Converter.StageProgress>();

        [TestMethod]
        public void Th10ScoreDataReadFromTest()
            => Th10ScoreDataReadFromTestHelper<Th10Converter, Th10Converter.StageProgress>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th10ScoreDataReadFromTestNull()
            => Th10ScoreDataReadFromTestNullHelper<Th10Converter, Th10Converter.StageProgress>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th10ScoreDataReadFromTestShortenedName()
            => Th10ScoreDataReadFromTestShortenedNameHelper<Th10Converter, Th10Converter.StageProgress>();

        [TestMethod]
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

        [TestMethod]
        public void Th11ScoreDataTest()
            => Th10ScoreDataTestHelper<Th11Converter, Th11Converter.StageProgress>();

        [TestMethod]
        public void Th11ScoreDataReadFromTest()
            => Th10ScoreDataReadFromTestHelper<Th11Converter, Th11Converter.StageProgress>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th11ScoreDataReadFromTestNull()
            => Th10ScoreDataReadFromTestNullHelper<Th11Converter, Th11Converter.StageProgress>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th11ScoreDataReadFromTestShortenedName()
            => Th10ScoreDataReadFromTestShortenedNameHelper<Th11Converter, Th11Converter.StageProgress>();

        [TestMethod]
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

        [TestMethod]
        public void Th12ScoreDataTest()
            => Th10ScoreDataTestHelper<Th12Converter, Th12Converter.StageProgress>();

        [TestMethod]
        public void Th12ScoreDataReadFromTest()
            => Th10ScoreDataReadFromTestHelper<Th12Converter, Th12Converter.StageProgress>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th12ScoreDataReadFromTestNull()
            => Th10ScoreDataReadFromTestNullHelper<Th12Converter, Th12Converter.StageProgress>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th12ScoreDataReadFromTestShortenedName()
            => Th10ScoreDataReadFromTestShortenedNameHelper<Th12Converter, Th12Converter.StageProgress>();

        [TestMethod]
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

        [TestMethod]
        public void Th128ScoreDataTest()
            => Th10ScoreDataTestHelper<Th128Converter, Th128Converter.StageProgress>();

        [TestMethod]
        public void Th128ScoreDataReadFromTest()
            => Th10ScoreDataReadFromTestHelper<Th128Converter, Th128Converter.StageProgress>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th128ScoreDataReadFromTestNull()
            => Th10ScoreDataReadFromTestNullHelper<Th128Converter, Th128Converter.StageProgress>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th128ScoreDataReadFromTestShortenedName()
            => Th10ScoreDataReadFromTestShortenedNameHelper<Th128Converter, Th128Converter.StageProgress>();

        [TestMethod]
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

        [TestMethod]
        public void Th13ScoreDataTest()
            => Th10ScoreDataTestHelper<Th13Converter, Th13Converter.StageProgress>();

        [TestMethod]
        public void Th13ScoreDataReadFromTest()
            => Th10ScoreDataReadFromTestHelper<Th13Converter, Th13Converter.StageProgress>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th13ScoreDataReadFromTestNull()
            => Th10ScoreDataReadFromTestNullHelper<Th13Converter, Th13Converter.StageProgress>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th13ScoreDataReadFromTestShortenedName()
            => Th10ScoreDataReadFromTestShortenedNameHelper<Th13Converter, Th13Converter.StageProgress>();

        [TestMethod]
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

        [TestMethod]
        public void Th14ScoreDataTest()
            => Th10ScoreDataTestHelper<Th14Converter, Th14Converter.StageProgress>();

        [TestMethod]
        public void Th14ScoreDataReadFromTest()
            => Th10ScoreDataReadFromTestHelper<Th14Converter, Th14Converter.StageProgress>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th14ScoreDataReadFromTestNull()
            => Th10ScoreDataReadFromTestNullHelper<Th14Converter, Th14Converter.StageProgress>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th14ScoreDataReadFromTestShortenedName()
            => Th10ScoreDataReadFromTestShortenedNameHelper<Th14Converter, Th14Converter.StageProgress>();

        [TestMethod]
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
