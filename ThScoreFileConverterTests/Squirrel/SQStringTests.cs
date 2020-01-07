using System;
using System.Globalization;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Squirrel;
using ThScoreFileConverterTests.Models;

namespace ThScoreFileConverterTests.Squirrel
{
    [TestClass]
    public class SQStringTests
    {
        [TestMethod]
        public void SQStringTest()
        {
            var sqstring = new SQString();

            Assert.AreEqual(SQObjectType.String, sqstring.Type);
            Assert.AreEqual(string.Empty, sqstring.Value, false, CultureInfo.InvariantCulture);
            Assert.AreEqual(string.Empty, sqstring, false, CultureInfo.InvariantCulture);
        }

        internal static SQString CreateTestHelper(byte[] bytes)
        {
            using var stream = new MemoryStream(bytes);
            using var reader = new BinaryReader(stream);

            return SQString.Create(reader);
        }

        [DataTestMethod]
        [DataRow("abc")]
        [DataRow("博麗 霊夢")]
        [DataRow("")]
        [DataRow("\0")]
        public void CreateTest(string expected)
        {
            var bytes = TestUtils.CP932Encoding.GetBytes(expected);
            var sqstring = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.String, bytes.Length, bytes));

            Assert.AreEqual(SQObjectType.String, sqstring.Type);
            Assert.AreEqual(expected, sqstring.Value, false, CultureInfo.InvariantCulture);
            Assert.AreEqual(expected, sqstring, false, CultureInfo.InvariantCulture);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateTestNull()
        {
            _ = SQString.Create(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        [DataTestMethod]
        [DataRow(0, "")]
        [DataRow(0, "abc")]
        [DataRow(0, null)]
        [DataRow(-1, "")]
        [DataRow(-1, "abc")]
        [DataRow(-1, null)]
        public void CreateTestEmpty(int size, string value)
        {
            var bytes = (value is null) ? Array.Empty<byte>() : TestUtils.CP932Encoding.GetBytes(value);
            var sqstring = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.String, size, bytes));

            Assert.AreEqual(SQObjectType.String, sqstring.Type);
            Assert.AreEqual(string.Empty, sqstring.Value, false, CultureInfo.InvariantCulture);
            Assert.AreEqual(string.Empty, sqstring, false, CultureInfo.InvariantCulture);
        }

        [DataTestMethod]
        [DataRow("abc")]
        [DataRow("博麗 霊夢")]
        [ExpectedException(typeof(EndOfStreamException))]
        public void CreateTestShortened(string value)
        {
            var bytes = TestUtils.CP932Encoding.GetBytes(value);
            _ = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.String, bytes.Length + 1, bytes));

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void CreateTestInvalid()
        {
            _ = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Null, 3, "abc"));

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void EqualsTestNull() => Assert.IsFalse(new SQString().Equals(null!));

        [TestMethod]
        public void EqualsTestNullObject() => Assert.IsFalse(new SQString().Equals((object)null!));

        [TestMethod]
        public void EqualsTestInvalidType() => Assert.IsFalse(new SQString().Equals(SQNull.Instance));

        [TestMethod]
        public void EqualsTestSelf()
        {
            var value = new SQString();

            Assert.IsTrue(value.Equals(value));
        }

        [TestMethod]
        public void EqualsTestSelfObject()
        {
            var value = new SQString();

            Assert.IsTrue(value.Equals(value as object));
        }

        [TestMethod]
        public void EqualsTestEqual() => Assert.IsTrue(new SQString().Equals(new SQString(string.Empty)));

        [TestMethod]
        public void EqualsTestNotEqual() => Assert.IsFalse(new SQString().Equals(new SQString("博麗 霊夢")));

        [TestMethod]
        public void GetHashCodeTestEqual()
            => Assert.AreEqual(new SQString().GetHashCode(), new SQString(string.Empty).GetHashCode());

        [TestMethod]
        public void GetHashCodeTestNotEqual()
            => Assert.AreNotEqual(new SQString().GetHashCode(), new SQString("博麗 霊夢").GetHashCode());
    }
}
