using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Squirrel;
using ThScoreFileConverterTests.Models;

namespace ThScoreFileConverterTests.Squirrel
{
    [TestClass]
    public class SQClosureTests
    {
        [TestMethod]
        public void SQClosureTest()
        {
            var closure = new SQClosure();

            Assert.AreEqual(SQObjectType.Closure, closure.Type);
        }

        internal static SQClosure CreateTestHelper(byte[] bytes)
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(bytes);
                using var reader = new BinaryReader(stream);
                stream = null;

                return SQClosure.Create(reader);
            }
            finally
            {
                stream?.Dispose();
            }
        }

        [TestMethod]
        public void CreateTest()
        {
            var sqclosure = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Closure));

            Assert.AreEqual(SQObjectType.Closure, sqclosure.Type);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateTestNull()
        {
            _ = SQClosure.Create(null);

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
