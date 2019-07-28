using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Squirrel;
using ThScoreFileConverterTests.Models;

namespace ThScoreFileConverterTests.Squirrel
{
    [TestClass]
    public class SQBoolTests
    {
        [TestMethod]
        public void SQBoolTestTrue()
        {
            var sqbool = SQBool.True;

            Assert.AreEqual(SQObjectType.Bool, sqbool.Type);
            Assert.IsTrue(sqbool.Value);
            Assert.IsTrue(sqbool);
        }

        [TestMethod]
        public void SQBoolTestFalse()
        {
            var sqbool = SQBool.False;

            Assert.AreEqual(SQObjectType.Bool, sqbool.Type);
            Assert.IsFalse(sqbool.Value);
            Assert.IsFalse(sqbool);
        }

        internal static SQBool CreateTestHelper(byte[] bytes)
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(bytes);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;

                    return SQBool.Create(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow((byte)0x00, false)]
        [DataRow((byte)0x01, true)]
        [DataRow((byte)0x02, true)]
        [DataRow((byte)0xFF, true)]
        public void CreateTest(byte serialized, bool expected)
        {
            var sqbool = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Bool, serialized));

            Assert.AreEqual(SQObjectType.Bool, sqbool.Type);
            Assert.AreEqual(expected, sqbool.Value);
            Assert.AreEqual(expected, sqbool);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateTestNull()
        {
            _ = SQBool.Create(null);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void CreateTestShortened()
        {
            _ = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Bool));

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void CreateTestInvalid()
        {
            _ = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Null, (byte)0x00));

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void EqualsTest()
        {
            var sqtrue = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Bool, (byte)0x01));
            var sqfalse = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Bool, (byte)0x00));

            Assert.AreEqual(SQBool.True, sqtrue);
            Assert.AreEqual(SQBool.False, sqfalse);
            Assert.AreNotEqual(SQBool.True, SQBool.False);
        }
    }
}
