using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace ThScoreFileConverter.Models.Tests
{
    [TestClass()]
    public class Th10PracticeTests
    {
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void Th10PracticeTestHelper<TParent>()
        {
            try
            {
                var practice = new Th10PracticeWrapper<TParent>();

                Assert.AreEqual(default, practice.Score.Value);
                Assert.AreEqual(default, practice.StageFlag.Value);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void Th10PracticeReadFromTestHelper<TParent>()
        {
            try
            {
                var score = 123456u;
                var stageFlag = 789u;

                var practice = Th10PracticeWrapper<TParent>.Create(TestUtils.MakeByteArray(score, stageFlag));

                Assert.AreEqual(score, practice.Score);
                Assert.AreEqual(stageFlag, practice.StageFlag);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void Th10PracticeReadFromTestNullHelper<TParent>()
        {
            try
            {
                var practice = new Th10PracticeWrapper<TParent>();
                practice.ReadFrom(null);
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
        public void Th10PracticeTest()
            => Th10PracticeTestHelper<Th10Converter>();

        [TestMethod()]
        public void Th10PracticeReadFromTest()
            => Th10PracticeReadFromTestHelper<Th10Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th10PracticeReadFromTestNull()
            => Th10PracticeReadFromTestNullHelper<Th10Converter>();

        #endregion

        #region Th11

        [TestMethod()]
        public void Th11PracticeTest()
            => Th10PracticeTestHelper<Th11Converter>();

        [TestMethod()]
        public void Th11PracticeReadFromTest()
            => Th10PracticeReadFromTestHelper<Th11Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th11PracticeReadFromTestNull()
            => Th10PracticeReadFromTestNullHelper<Th11Converter>();

        #endregion

        #region Th12

        [TestMethod()]
        public void Th12PracticeTest()
            => Th10PracticeTestHelper<Th12Converter>();

        [TestMethod()]
        public void Th12PracticeReadFromTest()
            => Th10PracticeReadFromTestHelper<Th12Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th12PracticeReadFromTestNull()
            => Th10PracticeReadFromTestNullHelper<Th12Converter>();

        #endregion
    }
}
