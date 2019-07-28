using System;
using System.Diagnostics.CodeAnalysis;
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
            Assert.AreEqual(string.Empty, sqstring.Value);
            Assert.AreEqual(string.Empty, sqstring);
        }

        internal static SQString CreateTestHelper(byte[] bytes)
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(bytes);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;

                    return SQString.Create(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
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
            _ = SQString.Create(null);

            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, "")]
        [DataRow(0, "abc")]
        [DataRow(0, null)]
        [DataRow(-1, "")]
        [DataRow(-1, "abc")]
        [DataRow(-1, null)]
        public void CreateTestEmpty(int size, string value)
        {
            var bytes = (value is null) ? new byte[0] : TestUtils.CP932Encoding.GetBytes(value);
            var sqstring = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.String, size, bytes));

            Assert.AreEqual(SQObjectType.String, sqstring.Type);
            Assert.AreEqual(string.Empty, sqstring.Value, false, CultureInfo.InvariantCulture);
            Assert.AreEqual(string.Empty, sqstring, false, CultureInfo.InvariantCulture);
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
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
    }
}
