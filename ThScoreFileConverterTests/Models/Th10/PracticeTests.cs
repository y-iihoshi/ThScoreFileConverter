using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThScoreFileConverter.Models.Th10;

namespace ThScoreFileConverterTests.Models.Th10
{
    [TestClass]
    public class PracticeTests
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

        internal static void Validate(in Practice practice, in Properties properties)
        {
            Assert.AreEqual(properties.score, practice.Score);
            Assert.AreEqual(properties.stageFlag, practice.StageFlag);
        }

        [TestMethod]
        public void PracticeTest()
            => TestUtils.Wrap(() =>
            {
                var properties = new Properties();
                var practice = new Practice();

                Validate(practice, properties);
            });

        [TestMethod]
        public void ReadFromTest()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;

                var practice = TestUtils.Create<Practice>(MakeByteArray(properties));

                Validate(practice, properties);
            });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull()
            => TestUtils.Wrap(() =>
            {
                var practice = new Practice();
                practice.ReadFrom(null);

                Assert.Fail(TestUtils.Unreachable);
            });
    }
}
