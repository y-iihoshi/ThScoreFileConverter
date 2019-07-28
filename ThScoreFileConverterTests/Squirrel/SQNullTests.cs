using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Squirrel;
using ThScoreFileConverterTests.Models;

namespace ThScoreFileConverterTests.Squirrel
{
    [TestClass]
    public class SQNullTests
    {
        [TestMethod]
        public void SQNullTest()
        {
            var sqnull = new SQNull();

            Assert.AreEqual(SQObjectType.Null, sqnull.Type);
            Assert.IsNull(sqnull.Value);
        }

        internal static SQNull CreateTestHelper(byte[] bytes)
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(bytes);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;

                    return SQNull.Create(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }

        [TestMethod]
        public void CreateTest()
        {
            var sqnull = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Null));

            Assert.AreEqual(SQObjectType.Null, sqnull.Type);
            Assert.IsNull(sqnull.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateTestNull()
        {
            _ = SQNull.Create(null);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void CreateTestInvalid()
        {
            _ = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Bool));

            Assert.Fail(TestUtils.Unreachable);
        }
    }
}
