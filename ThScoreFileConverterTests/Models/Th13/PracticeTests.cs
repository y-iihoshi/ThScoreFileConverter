using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using ThScoreFileConverter.Models.Th13;

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

        internal static Practice Create(byte[] array)
        {
            var practice = new Practice();

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    practice.ReadFrom(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return practice;
        }

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
                var practice = new Practice();

                Validate(practice, properties);
            });

        [TestMethod]
        public void Th13PracticeReadFromTest()
            => TestUtils.Wrap(() =>
            {
                var practice = Create(MakeByteArray(ValidProperties));

                Validate(practice, ValidProperties);
            });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th13PracticeReadFromTestNull()
            => TestUtils.Wrap(() =>
            {
                var practice = new Practice();

                practice.ReadFrom(null);

                Assert.Fail(TestUtils.Unreachable);
            });
    }
}
