using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Squirrel;
using ThScoreFileConverterTests.UnitTesting;
using SQOT = ThScoreFileConverter.Squirrel.SQObjectType;

namespace ThScoreFileConverterTests.Squirrel
{
    [TestClass]
    public class SQTableTests
    {
        [TestMethod]
        public void SQTableTest()
        {
            var sqtable = new SQTable();

            Assert.AreEqual(SQOT.Table, sqtable.Type);
            Assert.AreEqual(0, sqtable.Value.Count);
        }

        internal static SQTable CreateTestHelper(byte[] bytes)
        {
            using var stream = new MemoryStream(bytes);
            using var reader = new BinaryReader(stream);

            return SQTable.Create(reader);
        }

        [DataTestMethod]
        [DataRow(
            new[] { (int)SQOT.Table, (int)SQOT.Integer, 123, (int)SQOT.Integer, 456, (int)SQOT.Null },
            new[] { 123 },
            new[] { 456 },
            DisplayName = "one pair")]
        [DataRow(
            new[] {
                (int)SQOT.Table,
                (int)SQOT.Integer, 123, (int)SQOT.Integer, 456,
                (int)SQOT.Integer, 78, (int)SQOT.Integer, 90,
                (int)SQOT.Null },
            new[] { 123, 78 },
            new[] { 456, 90 },
            DisplayName = "two pairs")]
        public void CreateTest(int[] array, int[] expectedKeys, int[] expectedValues)
        {
            var sqtable = CreateTestHelper(TestUtils.MakeByteArray(array));

            Assert.AreEqual(SQOT.Table, sqtable.Type);

            for (var index = 0; index < (expectedKeys?.Length ?? 0); ++index)
            {
                var key = sqtable.Value.Keys.ElementAt(index);
                Assert.IsTrue(key is SQInteger);
                Assert.AreEqual(expectedKeys?[index], (SQInteger)key);
            }

            for (var index = 0; index < (expectedValues?.Length ?? 0); ++index)
            {
                var value = sqtable.Value.Values.ElementAt(index);
                Assert.IsTrue(value is SQInteger);
                Assert.AreEqual(expectedValues?[index], (SQInteger)value);
            }
        }

        [DataTestMethod]
        [DataRow(new[] { (int)SQOT.Table, (int)SQOT.Null },
            DisplayName = "empty")]
        public void CreateTestEmpty(int[] array)
        {
            var sqtable = CreateTestHelper(TestUtils.MakeByteArray(array));

            Assert.AreEqual(SQOT.Table, sqtable.Type);
            Assert.AreEqual(0, sqtable.Value.Count);
        }

        [DataTestMethod]
        [DataRow(new[] { (int)SQOT.Table, (int)SQOT.Integer, 123, (int)SQOT.Integer, (int)SQOT.Null },
            DisplayName = "missing value data")]
        [DataRow(new[] { (int)SQOT.Table, (int)SQOT.Integer, 123, (int)SQOT.Integer, 456 },
            DisplayName = "missing sentinel")]
        [DataRow(new[] { (int)SQOT.Table, (int)SQOT.Integer, 123, (int)SQOT.Integer },
            DisplayName = "missing value data and sentinel")]
        [DataRow(new[] { (int)SQOT.Table, (int)SQOT.Integer, 123, (int)SQOT.Null },
            DisplayName = "missing key or value")]
        [DataRow(new[] { (int)SQOT.Table },
            DisplayName = "empty and missing sentinel")]
        public void CreateTestShortened(int[] array)
        {
            _ = Assert.ThrowsException<EndOfStreamException>(() => CreateTestHelper(TestUtils.MakeByteArray(array)));
        }

        [DataTestMethod]
        [DataRow(new[] { (int)SQOT.Null, (int)SQOT.Integer, 123, (int)SQOT.Integer, 456, (int)SQOT.Null },
            DisplayName = "invalid type")]
        [DataRow(new[] { (int)SQOT.Table, 999, 123, (int)SQOT.Integer, 456, (int)SQOT.Null },
            DisplayName = "invalid key type")]
        [DataRow(new[] { (int)SQOT.Table, (int)SQOT.Integer, 123, 999, 456, (int)SQOT.Null },
            DisplayName = "invalid value type")]
        [DataRow(new[] { (int)SQOT.Table, (int)SQOT.Integer, 123, (int)SQOT.Integer, 456, 999 },
            DisplayName = "invalid sentinel")]
        [DataRow(new[] { (int)SQOT.Table, 123, (int)SQOT.Integer, 456, (int)SQOT.Null },
            DisplayName = "missing key type")]
        [DataRow(new[] { (int)SQOT.Table, (int)SQOT.Integer, (int)SQOT.Integer, 456, (int)SQOT.Null },
            DisplayName = "missing key data")]
        [DataRow(new[] { (int)SQOT.Table, (int)SQOT.Integer, 123, 456, (int)SQOT.Null },
            DisplayName = "missing value type")]
        [DataRow(new[] { (int)SQOT.Table, 123, (int)SQOT.Integer, 456 },
            DisplayName = "missing key type and sentinel")]
        [DataRow(new[] { (int)SQOT.Table, 123, (int)SQOT.Integer, (int)SQOT.Null },
            DisplayName = "missing key type and value data")]
        [DataRow(new[] { (int)SQOT.Table, (int)SQOT.Integer, (int)SQOT.Integer, 456 },
            DisplayName = "missing key data and sentinel")]
        [DataRow(new[] { (int)SQOT.Table, (int)SQOT.Integer, 123, 456 },
            DisplayName = "missing value type and sentinel")]
        public void CreateTestInvalid(int[] array)
        {
            _ = Assert.ThrowsException<InvalidDataException>(() => CreateTestHelper(TestUtils.MakeByteArray(array)));
        }
    }
}
