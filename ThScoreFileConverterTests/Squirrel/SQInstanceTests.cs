using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Squirrel;
using ThScoreFileConverterTests.Models;

namespace ThScoreFileConverterTests.Squirrel
{
    [TestClass]
    public class SQInstanceTests
    {
        [TestMethod]
        public void SQInstanceTest()
        {
            var instance = new SQInstance();

            Assert.AreEqual(SQObjectType.Instance, instance.Type);
        }

        internal static SQInstance CreateTestHelper(byte[] bytes)
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(bytes);
                using var reader = new BinaryReader(stream);
                stream = null;

                return SQInstance.Create(reader);
            }
            finally
            {
                stream?.Dispose();
            }
        }

        [TestMethod]
        public void CreateTest()
        {
            var sqinstance = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Instance));

            Assert.AreEqual(SQObjectType.Instance, sqinstance.Type);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateTestNull()
        {
            _ = SQInstance.Create(null);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void CreateTestInvalid()
        {
            _ = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Null));

            Assert.Fail(TestUtils.Unreachable);
        }
    }
}
