using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th10ScoreDataTests
    {
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th10ScoreDataTestHelper<TParent, TStageProgress>()
            where TParent : ThConverter
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var scoreData = new Th10ScoreDataWrapper<TParent, TStageProgress>();

                Assert.AreEqual(default, scoreData.Score.Value);
                Assert.AreEqual(default, scoreData.StageProgress.Value);
                Assert.AreEqual(default, scoreData.ContinueCount.Value);
                Assert.IsNull(scoreData.Name);
                Assert.AreEqual(default, scoreData.DateTime.Value);
                Assert.AreEqual(default, scoreData.SlowRate.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th10ScoreDataReadFromTestHelper<TParent, TStageProgress>()
            where TParent : ThConverter
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var score = 12u;
                var stageProgress = TestUtils.Cast<TStageProgress>(3);
                var continueCount = (byte)4;
                var name = TestUtils.MakeRandomArray<byte>(10);
                var dateTime = 567u;
                var slowRate = 8.9f;

                byte[] data = null;
                var type = typeof(TParent);
                if (type == typeof(Th10Converter))
                {
                    data = TestUtils.MakeByteArray(
                        score, TestUtils.Cast<byte>(stageProgress), continueCount, name, dateTime, slowRate);
                }
                else if (type == typeof(Th128Converter))
                {
                    var unknown = TestUtils.MakeRandomArray<byte>(8);
                    data = TestUtils.MakeByteArray(
                        score, TestUtils.Cast<byte>(stageProgress), continueCount, name, dateTime, slowRate, unknown);
                }
                else
                {
                    var unknown = default(uint);
                    data = TestUtils.MakeByteArray(
                        score, TestUtils.Cast<byte>(stageProgress), continueCount, name, dateTime, slowRate, unknown);
                }

                var scoreData = Th10ScoreDataWrapper<TParent, TStageProgress>.Create(data);

                Assert.AreEqual(score, scoreData.Score);
                Assert.AreEqual(stageProgress, scoreData.StageProgress);
                Assert.AreEqual(continueCount, scoreData.ContinueCount);
                CollectionAssert.AreEqual(name, scoreData.Name.ToArray());
                Assert.AreEqual(dateTime, scoreData.DateTime);
                Assert.AreEqual(slowRate, scoreData.SlowRate);
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
                var score = 12u;
                var stageProgress = TestUtils.Cast<TStageProgress>(3);
                var continueCount = (byte)4;
                var name = TestUtils.MakeRandomArray<byte>(9);
                var dateTime = 567u;
                var slowRate = 8.9f;

                byte[] data = null;
                var type = typeof(TParent);
                if (type == typeof(Th10Converter))
                {
                    data = TestUtils.MakeByteArray(
                        score, TestUtils.Cast<byte>(stageProgress), continueCount, name, dateTime, slowRate);
                }
                else if (type == typeof(Th128Converter))
                {
                    var unknown = TestUtils.MakeRandomArray<byte>(8);
                    data = TestUtils.MakeByteArray(
                        score, TestUtils.Cast<byte>(stageProgress), continueCount, name, dateTime, slowRate, unknown);
                }
                else
                {
                    var unknown = default(uint);
                    data = TestUtils.MakeByteArray(
                        score, TestUtils.Cast<byte>(stageProgress), continueCount, name, dateTime, slowRate, unknown);
                }

                var scoreData = Th10ScoreDataWrapper<TParent, TStageProgress>.Create(data);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th10ScoreDataReadFromTestExceededNameHelper<TParent, TStageProgress>()
            where TParent : ThConverter
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var score = 12u;
                var stageProgress = TestUtils.Cast<TStageProgress>(3);
                var continueCount = (byte)4;
                var name = TestUtils.MakeRandomArray<byte>(11);
                var dateTime = 567u;
                var slowRate = 8.9f;

                byte[] data = null;
                var type = typeof(TParent);
                if (type == typeof(Th10Converter))
                {
                    data = TestUtils.MakeByteArray(
                        score, TestUtils.Cast<byte>(stageProgress), continueCount, name, dateTime, slowRate);
                }
                else if (type == typeof(Th128Converter))
                {
                    var unknown = TestUtils.MakeRandomArray<byte>(8);
                    data = TestUtils.MakeByteArray(
                        score, TestUtils.Cast<byte>(stageProgress), continueCount, name, dateTime, slowRate, unknown);
                }
                else
                {
                    var unknown = default(uint);
                    data = TestUtils.MakeByteArray(
                        score, TestUtils.Cast<byte>(stageProgress), continueCount, name, dateTime, slowRate, unknown);
                }

                var scoreData = Th10ScoreDataWrapper<TParent, TStageProgress>.Create(data);

                Assert.AreEqual(score, scoreData.Score);
                Assert.AreEqual(stageProgress, scoreData.StageProgress);
                Assert.AreEqual(continueCount, scoreData.ContinueCount);
                CollectionAssert.AreNotEqual(name, scoreData.Name.ToArray());
                CollectionAssert.AreEqual(name.Take(10).ToArray(), scoreData.Name.ToArray());
                Assert.AreNotEqual(dateTime, scoreData.DateTime);
                Assert.AreNotEqual(slowRate, scoreData.SlowRate);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "scoreData")]
        internal static void Th10ScoreDataReadFromTestInvalidStageProgressHelper<TParent, TStageProgress>()
            where TParent : ThConverter
            where TStageProgress : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var score = 12u;
                var stageProgress = TestUtils.Cast<TStageProgress>(byte.MaxValue);
                var continueCount = (byte)4;
                var name = TestUtils.MakeRandomArray<byte>(10);
                var dateTime = 567u;
                var slowRate = 8.9f;

                byte[] data = null;
                var type = typeof(TParent);
                if (type == typeof(Th10Converter))
                {
                    data = TestUtils.MakeByteArray(
                        score, TestUtils.Cast<byte>(stageProgress), continueCount, name, dateTime, slowRate);
                }
                else if (type == typeof(Th128Converter))
                {
                    var unknown = TestUtils.MakeRandomArray<byte>(8);
                    data = TestUtils.MakeByteArray(
                        score, TestUtils.Cast<byte>(stageProgress), continueCount, name, dateTime, slowRate, unknown);
                }
                else
                {
                    var unknown = default(uint);
                    data = TestUtils.MakeByteArray(
                        score, TestUtils.Cast<byte>(stageProgress), continueCount, name, dateTime, slowRate, unknown);
                }

                var scoreData = Th10ScoreDataWrapper<TParent, TStageProgress>.Create(data);

                Assert.Fail(TestUtils.Unreachable);
            });

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

        [TestMethod()]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th10ScoreDataReadFromTestInvalidStageProgress()
            => Th10ScoreDataReadFromTestInvalidStageProgressHelper<Th10Converter, Th10Converter.StageProgress>();

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

        [TestMethod()]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th11ScoreDataReadFromTestInvalidStageProgress()
            => Th10ScoreDataReadFromTestInvalidStageProgressHelper<Th11Converter, Th11Converter.StageProgress>();

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

        [TestMethod()]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th12ScoreDataReadFromTestInvalidStageProgress()
            => Th10ScoreDataReadFromTestInvalidStageProgressHelper<Th12Converter, Th12Converter.StageProgress>();

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

        [TestMethod()]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th128ScoreDataReadFromTestInvalidStageProgress()
            => Th10ScoreDataReadFromTestInvalidStageProgressHelper<Th128Converter, Th128Converter.StageProgress>();

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

        [TestMethod()]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th13ScoreDataReadFromTestInvalidStageProgress()
            => Th10ScoreDataReadFromTestInvalidStageProgressHelper<Th13Converter, Th13Converter.StageProgress>();

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

        [TestMethod()]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th14ScoreDataReadFromTestInvalidStageProgress()
            => Th10ScoreDataReadFromTestInvalidStageProgressHelper<Th14Converter, Th14Converter.StageProgress>();

        #endregion
    }
}
