using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThScoreFileConverterTests.Models.Wrappers;
using Practice = ThScoreFileConverter.Models.Th10.Practice;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
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

        internal static void Validate(in Th10PracticeWrapper practice, in Properties properties)
            => Validate(practice.Target as Practice, properties);

        internal static void Validate(in Practice practice, in Properties properties)
        {
            Assert.AreEqual(properties.score, practice.Score);
            Assert.AreEqual(properties.stageFlag, practice.StageFlag);
        }

        [TestMethod]
        public void Th10PracticeTest()
            => TestUtils.Wrap(() =>
            {
                var properties = new Properties();
                var practice = new Th10PracticeWrapper();

                Validate(practice, properties);
            });

        [TestMethod]
        public void Th10PracticeReadFromTest()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;

                var practice = Th10PracticeWrapper.Create(MakeByteArray(properties));

                Validate(practice, properties);
            });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th10PracticeReadFromTestNull()
            => TestUtils.Wrap(() =>
            {
                var practice = new Th10PracticeWrapper();
                practice.ReadFrom(null);

                Assert.Fail(TestUtils.Unreachable);
            });
    }
}
