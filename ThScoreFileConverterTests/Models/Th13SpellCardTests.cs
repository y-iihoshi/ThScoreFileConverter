using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace ThScoreFileConverter.Models.Tests
{
    [TestClass()]
    public class Th13SpellCardTests
    {
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void Th13SpellCardTestHelper<TParent, TLevel>()
            where TLevel : struct, Enum
        {
            try
            {
                var spellCard = new Th13SpellCardWrapper<TParent, TLevel>();

                Assert.IsNull(spellCard.Name);
                Assert.AreEqual(0, spellCard.ClearCount);
                Assert.AreEqual(0, spellCard.PracticeClearCount);
                Assert.AreEqual(0, spellCard.TrialCount);
                Assert.AreEqual(0, spellCard.PracticeTrialCount);
                Assert.AreEqual(0, spellCard.Id);
                Assert.AreEqual(default, spellCard.Level.Value);
                Assert.AreEqual(0, spellCard.PracticeScore);
                Assert.IsFalse(spellCard.HasTried().Value);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void Th13SpellCardReadFromTestHelper<TParent, TLevel>()
            where TLevel : struct, Enum
        {
            try
            {
                var name = TestUtils.MakeRandomArray<byte>(0x80);
                var clearCount = 123;
                var practiceClearCount = 234;
                var trialCount = 456;
                var practiceTrialCount = 567;
                var id = 789;
                var level = 1;
                var practiceScore = 890;

                var spellCard = Th13SpellCardWrapper<TParent, TLevel>.Create(
                    TestUtils.MakeByteArray(
                        name,
                        clearCount,
                        practiceClearCount,
                        trialCount,
                        practiceTrialCount,
                        id - 1,
                        level,
                        practiceScore));

                CollectionAssert.AreEqual(name, spellCard.Name.ToArray());
                Assert.AreEqual(clearCount, spellCard.ClearCount);
                Assert.AreEqual(practiceClearCount, spellCard.PracticeClearCount);
                Assert.AreEqual(trialCount, spellCard.TrialCount);
                Assert.AreEqual(practiceTrialCount, spellCard.PracticeTrialCount);
                Assert.AreEqual(id, spellCard.Id);
                Assert.AreEqual(Enum.ToObject(typeof(TLevel), level), spellCard.Level.Value);
                Assert.AreEqual(practiceScore, spellCard.PracticeScore);
                Assert.IsTrue(spellCard.HasTried().Value);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void Th13SpellCardReadFromTestNullHelper<TParent, TLevel>()
            where TLevel : struct, Enum
        {
            try
            {
                var spellCard = new Th13SpellCardWrapper<TParent, TLevel>();
                spellCard.ReadFrom(null);
                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void Th13SpellCardReadFromTestShortenedNameHelper<TParent, TLevel>()
            where TLevel : struct, Enum
        {
            try
            {
                var name = TestUtils.MakeRandomArray<byte>(0x7F);
                var clearCount = 123;
                var practiceClearCount = 234;
                var trialCount = 456;
                var practiceTrialCount = 567;
                var id = 789;
                var level = 1;
                var practiceScore = 890;

                var spellCard = Th13SpellCardWrapper<TParent, TLevel>.Create(
                    TestUtils.MakeByteArray(
                        name,
                        clearCount,
                        practiceClearCount,
                        trialCount,
                        practiceTrialCount,
                        id - 1,
                        level,
                        practiceScore));

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void Th13SpellCardReadFromTestExceededNameHelper<TParent, TLevel>()
            where TLevel : struct, Enum
        {
            try
            {
                var name = TestUtils.MakeRandomArray<byte>(0x81);
                var clearCount = 123;
                var practiceClearCount = 234;
                var trialCount = 456;
                var practiceTrialCount = 567;
                var id = 789;
                var level = 1;
                var practiceScore = 890;

                var spellCard = Th13SpellCardWrapper<TParent, TLevel>.Create(
                    TestUtils.MakeByteArray(
                        name,
                        clearCount,
                        practiceClearCount,
                        trialCount,
                        practiceTrialCount,
                        id - 1,
                        level,
                        practiceScore));

                CollectionAssert.AreNotEqual(name, spellCard.Name.ToArray());
                CollectionAssert.AreEqual(name.Take(0x80).ToArray(), spellCard.Name.ToArray());
                Assert.AreNotEqual(clearCount, spellCard.ClearCount);
                Assert.AreNotEqual(practiceClearCount, spellCard.PracticeClearCount);
                Assert.AreNotEqual(trialCount, spellCard.TrialCount);
                Assert.AreNotEqual(practiceTrialCount, spellCard.PracticeTrialCount);
                Assert.AreNotEqual(id, spellCard.Id);
                Assert.AreNotEqual(Enum.ToObject(typeof(TLevel), level), spellCard.Level.Value);
                Assert.AreNotEqual(practiceScore, spellCard.PracticeScore);
                Assert.IsTrue(spellCard.HasTried().Value);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "spellCard")]
        public static void Th13SpellCardReadFromTestInvalidLevelHelper<TParent, TLevel>()
            where TLevel : struct, Enum
        {
            try
            {
                var name = TestUtils.MakeRandomArray<byte>(0x80);
                var clearCount = 123;
                var practiceClearCount = 234;
                var trialCount = 456;
                var practiceTrialCount = 567;
                var id = 789;
                var level = -1;
                var practiceScore = 890;

                var spellCard = Th13SpellCardWrapper<TParent, TLevel>.Create(
                    TestUtils.MakeByteArray(
                        name,
                        clearCount,
                        practiceClearCount,
                        trialCount,
                        practiceTrialCount,
                        id - 1,
                        level,
                        practiceScore));

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void Th13SpellCardTestHelper<TParent>()
            => Th13SpellCardTestHelper<TParent, ThConverter.Level>();

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void Th13SpellCardReadFromTestHelper<TParent>()
            => Th13SpellCardReadFromTestHelper<TParent, ThConverter.Level>();

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void Th13SpellCardReadFromTestNullHelper<TParent>()
            => Th13SpellCardReadFromTestNullHelper<TParent, ThConverter.Level>();

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void Th13SpellCardReadFromTestShortenedNameHelper<TParent>()
            => Th13SpellCardReadFromTestShortenedNameHelper<TParent, ThConverter.Level>();

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void Th13SpellCardReadFromTestExceededNameHelper<TParent>()
            => Th13SpellCardReadFromTestExceededNameHelper<TParent, ThConverter.Level>();

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void Th13SpellCardReadFromTestInvalidLevelHelper<TParent>()
            => Th13SpellCardReadFromTestInvalidLevelHelper<TParent, ThConverter.Level>();

        #region Th13

        [TestMethod()]
        public void Th13SpellCardTest()
            => Th13SpellCardTestHelper<Th13Converter, Th13Converter.LevelPractice>();

        [TestMethod()]
        public void Th13SpellCardReadFromTest()
            => Th13SpellCardReadFromTestHelper<Th13Converter, Th13Converter.LevelPractice>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th13SpellCardReadFromTestNull()
            => Th13SpellCardReadFromTestNullHelper<Th13Converter, Th13Converter.LevelPractice>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th13SpellCardReadFromTestShortenedName()
            => Th13SpellCardReadFromTestShortenedNameHelper<Th13Converter, Th13Converter.LevelPractice>();

        [TestMethod()]
        public void Th13SpellCardReadFromTestExceededName()
            => Th13SpellCardReadFromTestExceededNameHelper<Th13Converter, Th13Converter.LevelPractice>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th13SpellCardReadFromTestInvalidLevel()
            => Th13SpellCardReadFromTestInvalidLevelHelper<Th13Converter, Th13Converter.LevelPractice>();

        #endregion

        #region Th14

        [TestMethod()]
        public void Th14SpellCardTest()
            => Th13SpellCardTestHelper<Th14Converter>();

        [TestMethod()]
        public void Th14SpellCardReadFromTest()
            => Th13SpellCardReadFromTestHelper<Th14Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th14SpellCardReadFromTestNull()
            => Th13SpellCardReadFromTestNullHelper<Th14Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th14SpellCardReadFromTestShortenedName()
            => Th13SpellCardReadFromTestShortenedNameHelper<Th14Converter>();

        [TestMethod()]
        public void Th14SpellCardReadFromTestExceededName()
            => Th13SpellCardReadFromTestExceededNameHelper<Th14Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th14SpellCardReadFromTestInvalidLevel()
            => Th13SpellCardReadFromTestInvalidLevelHelper<Th14Converter>();

        #endregion

        #region Th15

        [TestMethod()]
        public void Th15SpellCardTest()
            => Th13SpellCardTestHelper<Th15Converter>();

        [TestMethod()]
        public void Th15SpellCardReadFromTest()
            => Th13SpellCardReadFromTestHelper<Th15Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th15SpellCardReadFromTestNull()
            => Th13SpellCardReadFromTestNullHelper<Th15Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th15SpellCardReadFromTestShortenedName()
            => Th13SpellCardReadFromTestShortenedNameHelper<Th15Converter>();

        [TestMethod()]
        public void Th15SpellCardReadFromTestExceededName()
            => Th13SpellCardReadFromTestExceededNameHelper<Th15Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th15SpellCardReadFromTestInvalidLevel()
            => Th13SpellCardReadFromTestInvalidLevelHelper<Th15Converter>();

        #endregion

        #region Th16

        [TestMethod()]
        public void Th16SpellCardTest()
            => Th13SpellCardTestHelper<Th16Converter>();

        [TestMethod()]
        public void Th16SpellCardReadFromTest()
            => Th13SpellCardReadFromTestHelper<Th16Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th16SpellCardReadFromTestNull()
            => Th13SpellCardReadFromTestNullHelper<Th16Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th16SpellCardReadFromTestShortenedName()
            => Th13SpellCardReadFromTestShortenedNameHelper<Th16Converter>();

        [TestMethod()]
        public void Th16SpellCardReadFromTestExceededName()
            => Th13SpellCardReadFromTestExceededNameHelper<Th16Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th16SpellCardReadFromTestInvalidLevel()
            => Th13SpellCardReadFromTestInvalidLevelHelper<Th16Converter>();

        #endregion
    }
}
