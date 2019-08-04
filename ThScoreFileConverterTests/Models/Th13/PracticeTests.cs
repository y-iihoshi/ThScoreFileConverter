using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThScoreFileConverterTests.Models.Wrappers;
using ThScoreFileConverter.Models.Th13;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th13PracticeTests
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

        internal static void Validate(in Th13PracticeWrapper practice, in Properties properties)
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
                var practice = new Th13PracticeWrapper();

                Validate(practice, properties);
            });

        [TestMethod]
        public void Th13PracticeReadFromTest()
            => TestUtils.Wrap(() =>
            {
                var practice = Th13PracticeWrapper.Create(MakeByteArray(ValidProperties));

                Validate(practice, ValidProperties);
            });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th13PracticeReadFromTestNull()
            => TestUtils.Wrap(() =>
            {
                var practice = new Th13PracticeWrapper();

                practice.ReadFrom(null);

                Assert.Fail(TestUtils.Unreachable);
            });
    }
}
