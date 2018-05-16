using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace ThScoreFileConverter.Models.Tests
{
    [TestClass()]
    public class Th10SpellCardTests
    {
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void Th10SpellCardTestHelper<TParent>()
        {
            try
            {
                var spellCard = new Th10SpellCardWrapper<TParent>();

                Assert.IsNull(spellCard.Name);
                Assert.AreEqual(0, spellCard.ClearCount);
                Assert.AreEqual(0, spellCard.TrialCount);
                Assert.AreEqual(0, spellCard.Id);
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
        public static void Th10SpellCardReadFromTestHelper<TParent>()
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
        public static void Th10SpellCardReadFromTestNullHelper<TParent>()
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
        public static void Th10SpellCardReadFromTestInvalidLevelHelper<TParent>()
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
        [ExpectedException(typeof(InvalidCastException))]
        public void Th12SpellCardReadFromTestInvalidLevel()
            => Th10SpellCardReadFromTestInvalidLevelHelper<Th12Converter>();

        #endregion
    }
}
