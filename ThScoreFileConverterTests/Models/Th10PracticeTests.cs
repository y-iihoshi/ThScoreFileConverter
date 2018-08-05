using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th10PracticeTests
    {
        internal struct Properties
        {
            public uint score;
            public uint stageFlag;
        };

        internal static Properties ValidProperties => new Properties()
        {
            score = 123456u,
            stageFlag = 789u
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(properties.score, properties.stageFlag);

        internal static void Validate<TParent>(in Th10PracticeWrapper<TParent> practice, in Properties properties)
            where TParent : ThConverter
        {
            Assert.AreEqual(properties.score, practice.Score);
            Assert.AreEqual(properties.stageFlag, practice.StageFlag);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th10PracticeTestHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = new Properties();
                var practice = new Th10PracticeWrapper<TParent>();

                Validate(practice, properties);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th10PracticeReadFromTestHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;

                var practice = Th10PracticeWrapper<TParent>.Create(MakeByteArray(properties));

                Validate(practice, properties);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th10PracticeReadFromTestNullHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var practice = new Th10PracticeWrapper<TParent>();
                practice.ReadFrom(null);

                Assert.Fail(TestUtils.Unreachable);
            });

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
