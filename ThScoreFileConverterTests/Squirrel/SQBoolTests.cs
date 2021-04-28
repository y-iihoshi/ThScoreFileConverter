using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Squirrel;
using ThScoreFileConverterTests.UnitTesting;

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
            using var stream = new MemoryStream(bytes);
            using var reader = new BinaryReader(stream);

            return SQBool.Create(reader);
        }

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
        public void CreateTestShortened()
        {
            _ = Assert.ThrowsException<EndOfStreamException>(
                () => _ = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Bool)));
        }

        [TestMethod]
        public void CreateTestInvalid()
        {
            _ = Assert.ThrowsException<InvalidDataException>(
                () => _ = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Null, (byte)0x00)));
        }

        [TestMethod]
        public void EqualsTestNull()
        {
            Assert.IsFalse(SQBool.True.Equals(null!));
        }

        [TestMethod]
        public void EqualsTestNullObject()
        {
            Assert.IsFalse(SQBool.True.Equals((object)null!));
        }

        [TestMethod]
        public void EqualsTestInvalidType()
        {
            Assert.IsFalse(SQBool.True.Equals(SQNull.Instance));
        }

        [TestMethod]
        public void EqualsTestSelf()
        {
            Assert.IsTrue(SQBool.True.Equals(SQBool.True));
        }

        [TestMethod]
        public void EqualsTestSelfObject()
        {
            Assert.IsTrue(SQBool.True.Equals(SQBool.True as object));
        }

        [TestMethod]
        public void EqualsTestEqual()
        {
            var sqtrue = CreateTestHelper(SquirrelHelper.MakeByteArray(true).ToArray());

            Assert.IsTrue(SQBool.True.Equals(sqtrue));
        }

        [TestMethod]
        public void EqualsTestNotEqual()
        {
            Assert.IsFalse(SQBool.True.Equals(SQBool.False));
        }

        [TestMethod]
        public void GetHashCodeTestEqual()
        {
            var sqtrue = CreateTestHelper(SquirrelHelper.MakeByteArray(true).ToArray());

            Assert.AreEqual(SQBool.True.GetHashCode(), sqtrue.GetHashCode());
        }

        [TestMethod]
        public void GetHashCodeTestNotEqual()
        {
            Assert.AreNotEqual(SQBool.True.GetHashCode(), SQBool.False.GetHashCode());
        }
    }
}
