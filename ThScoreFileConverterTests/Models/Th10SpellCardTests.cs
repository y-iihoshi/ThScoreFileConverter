using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th10SpellCardTests
    {
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th10SpellCardTestHelper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var spellCard = new Th10SpellCardWrapper<TParent>();

                Assert.IsNull(spellCard.Name);
                Assert.AreEqual(default, spellCard.ClearCount.Value);
                Assert.AreEqual(default, spellCard.TrialCount.Value);
                Assert.AreEqual(default, spellCard.Id.Value);
                Assert.AreEqual(default, spellCard.Level.Value);
                Assert.IsFalse(spellCard.HasTried().Value);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th10SpellCardReadFromTestHelper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var name = TestUtils.MakeRandomArray<byte>(0x80);
                var clearCount = 123;
                var trialCount = 456;
                var id = 789;
                var level = ThConverter.Level.Normal;

                var spellCard = Th10SpellCardWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(name, clearCount, trialCount, id - 1, (int)level));

                CollectionAssert.AreEqual(name, spellCard.Name.ToArray());
                Assert.AreEqual(clearCount, spellCard.ClearCount);
                Assert.AreEqual(trialCount, spellCard.TrialCount);
                Assert.AreEqual(id, spellCard.Id);
                Assert.AreEqual(level, spellCard.Level.Value);
                Assert.IsTrue(spellCard.HasTried().Value);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th10SpellCardReadFromTestNullHelper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var spellCard = new Th10SpellCardWrapper<TParent>();
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
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "spellCard")]
        internal static void Th10SpellCardReadFromTestShortenedNameHelper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var name = TestUtils.MakeRandomArray<byte>(0x7F);
                var clearCount = 123;
                var trialCount = 456;
                var id = 789;
                var level = ThConverter.Level.Normal;

                var spellCard = Th10SpellCardWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(name, clearCount, trialCount, id - 1, (int)level));

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th10SpellCardReadFromTestExceededNameHelper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var name = TestUtils.MakeRandomArray<byte>(0x81);
                var clearCount = 123;
                var trialCount = 456;
                var id = 789;
                var level = ThConverter.Level.Normal;

                var spellCard = Th10SpellCardWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(name, clearCount, trialCount, id - 1, (int)level));

                CollectionAssert.AreNotEqual(name, spellCard.Name.ToArray());
                CollectionAssert.AreEqual(name.Take(0x80).ToArray(), spellCard.Name.ToArray());
                Assert.AreNotEqual(clearCount, spellCard.ClearCount);
                Assert.AreNotEqual(trialCount, spellCard.TrialCount);
                Assert.AreNotEqual(id, spellCard.Id);
                Assert.AreNotEqual(level, spellCard.Level.Value);
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
        internal static void Th10SpellCardReadFromTestInvalidLevelHelper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var name = TestUtils.MakeRandomArray<byte>(0x80);
                var clearCount = 123;
                var trialCount = 456;
                var id = 789;
                var level = (ThConverter.Level)(-1);

                var spellCard = Th10SpellCardWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(name, clearCount, trialCount, id - 1, (int)level));

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        #region Th10

        [TestMethod()]
        public void Th10SpellCardTest()
            => Th10SpellCardTestHelper<Th10Converter>();

        [TestMethod()]
        public void Th10SpellCardReadFromTest()
            => Th10SpellCardReadFromTestHelper<Th10Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th10SpellCardReadFromTestNull()
            => Th10SpellCardReadFromTestNullHelper<Th10Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th10SpellCardReadFromTestShortenedName()
            => Th10SpellCardReadFromTestShortenedNameHelper<Th10Converter>();

        [TestMethod()]
        public void Th10SpellCardReadFromTestExceededName()
            => Th10SpellCardReadFromTestExceededNameHelper<Th10Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th10SpellCardReadFromTestInvalidLevel()
            => Th10SpellCardReadFromTestInvalidLevelHelper<Th10Converter>();

        #endregion

        #region Th11

        [TestMethod()]
        public void Th11SpellCardTest()
            => Th10SpellCardTestHelper<Th11Converter>();

        [TestMethod()]
        public void Th11SpellCardReadFromTest()
            => Th10SpellCardReadFromTestHelper<Th11Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th11SpellCardReadFromTestNull()
            => Th10SpellCardReadFromTestNullHelper<Th11Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th11SpellCardReadFromTestShortenedName()
            => Th10SpellCardReadFromTestShortenedNameHelper<Th11Converter>();

        [TestMethod()]
        public void Th11SpellCardReadFromTestExceededName()
            => Th10SpellCardReadFromTestExceededNameHelper<Th11Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th11SpellCardReadFromTestInvalidLevel()
            => Th10SpellCardReadFromTestInvalidLevelHelper<Th11Converter>();

        #endregion

        #region Th12

        [TestMethod()]
        public void Th12SpellCardTest()
            => Th10SpellCardTestHelper<Th12Converter>();

        [TestMethod()]
        public void Th12SpellCardReadFromTest()
            => Th10SpellCardReadFromTestHelper<Th12Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th12SpellCardReadFromTestNull()
            => Th10SpellCardReadFromTestNullHelper<Th12Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th12SpellCardReadFromTestShortenedName()
            => Th10SpellCardReadFromTestShortenedNameHelper<Th12Converter>();

        [TestMethod()]
        public void Th12SpellCardReadFromTestExceededName()
            => Th10SpellCardReadFromTestExceededNameHelper<Th12Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th12SpellCardReadFromTestInvalidLevel()
            => Th10SpellCardReadFromTestInvalidLevelHelper<Th12Converter>();

        #endregion
    }
}
