using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThScoreFileConverter.Models.Th13;
using ThScoreFileConverterTests.Models.Th13.Wrappers;

namespace ThScoreFileConverterTests.Models.Th13
{
    [TestClass]
    public class PracticeTests
    {
        internal struct Properties
        {
            public uint score;
            public byte clearFlag;
            public byte enableFlag;
        };

        internal static Properties ValidProperties => new Properties()
        {
            score = 123456u,
            clearFlag = 7,
            enableFlag = 8
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(properties.score, properties.clearFlag, properties.enableFlag, (ushort)0);

        internal static void Validate(in PracticeWrapper practice, in Properties properties)
            => Validate(practice.Target as Practice, properties);

        internal static void Validate(in Practice practice, in Properties properties)
        {
            Assert.AreEqual(properties.score, practice.Score);
            Assert.AreEqual(properties.clearFlag, practice.ClearFlag);
            Assert.AreEqual(properties.enableFlag, practice.EnableFlag);
        }

        [TestMethod]
        public void Th13PracticeTest()
            => TestUtils.Wrap(() =>
            {
                var properties = new Properties();
                var practice = new PracticeWrapper();

                Validate(practice, properties);
            });

        [TestMethod]
        public void Th13PracticeReadFromTest()
            => TestUtils.Wrap(() =>
            {
                var practice = PracticeWrapper.Create(MakeByteArray(ValidProperties));

                Validate(practice, ValidProperties);
            });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th13PracticeReadFromTestNull()
            => TestUtils.Wrap(() =>
            {
                var practice = new PracticeWrapper();

                practice.ReadFrom(null);

                Assert.Fail(TestUtils.Unreachable);
            });
    }
}
