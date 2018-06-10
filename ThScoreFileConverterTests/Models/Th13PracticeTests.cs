using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th13PracticeTests
    {
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th13PracticeTestHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var practice = new Th13PracticeWrapper<TParent>();

                Assert.AreEqual(default, practice.Score.Value);
                Assert.AreEqual(default, practice.ClearFlag.Value);
                Assert.AreEqual(default, practice.EnableFlag.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th13PracticeReadFromTestHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var score = 123456u;
                byte clearFlag = 7;
                byte enableFlag = 8;
                ushort unknown = 9;

                var practice = Th13PracticeWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(score, clearFlag, enableFlag, unknown));

                Assert.AreEqual(score, practice.Score);
                Assert.AreEqual(clearFlag, practice.ClearFlag);
                Assert.AreEqual(enableFlag, practice.EnableFlag);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th13PracticeReadFromTestNullHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var practice = new Th13PracticeWrapper<TParent>();
                practice.ReadFrom(null);
                Assert.Fail(TestUtils.Unreachable);
            });

        #region Th13

        [TestMethod()]
        public void Th13PracticeTest()
            => Th13PracticeTestHelper<Th13Converter>();

        [TestMethod()]
        public void Th13PracticeReadFromTest()
            => Th13PracticeReadFromTestHelper<Th13Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th13PracticeReadFromTestNull()
            => Th13PracticeReadFromTestNullHelper<Th13Converter>();

        #endregion

        #region Th14

        [TestMethod()]
        public void Th14PracticeTest()
            => Th13PracticeTestHelper<Th14Converter>();

        [TestMethod()]
        public void Th14PracticeReadFromTest()
            => Th13PracticeReadFromTestHelper<Th14Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th14PracticeReadFromTestNull()
            => Th13PracticeReadFromTestNullHelper<Th14Converter>();

        #endregion

        #region Th15

        [TestMethod()]
        public void Th15PracticeTest()
            => Th13PracticeTestHelper<Th15Converter>();

        [TestMethod()]
        public void Th15PracticeReadFromTest()
            => Th13PracticeReadFromTestHelper<Th15Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th15PracticeReadFromTestNull()
            => Th13PracticeReadFromTestNullHelper<Th15Converter>();

        #endregion

        #region Th16

        [TestMethod()]
        public void Th16PracticeTest()
            => Th13PracticeTestHelper<Th16Converter>();

        [TestMethod()]
        public void Th16PracticeReadFromTest()
            => Th13PracticeReadFromTestHelper<Th16Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th16PracticeReadFromTestNull()
            => Th13PracticeReadFromTestNullHelper<Th16Converter>();

        #endregion
    }
}
