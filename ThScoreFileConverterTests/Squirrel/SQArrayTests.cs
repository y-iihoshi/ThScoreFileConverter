using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Squirrel;
using ThScoreFileConverterTests.UnitTesting;
using SQOT = ThScoreFileConverter.Squirrel.SQObjectType;

namespace ThScoreFileConverterTests.Squirrel;

[TestClass]
public class SQArrayTests
{
    [TestMethod]
    public void SQArrayTest()
    {
        var sqarray = new SQArray();

        Assert.AreEqual(SQOT.Array, sqarray.Type);
        Assert.AreEqual(0, sqarray.Value.Count());
    }

    internal static SQArray CreateTestHelper(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        using var reader = new BinaryReader(stream);

        return SQArray.Create(reader);
    }

    [DataTestMethod]
    [DataRow(
        new[] { (int)SQOT.Array, 1, (int)SQOT.Integer, 0, (int)SQOT.Integer, 123, (int)SQOT.Null },
        new[] { 123 },
        DisplayName = "one element")]
    [DataRow(
        new[] {
            (int)SQOT.Array, 2,
            (int)SQOT.Integer, 0, (int)SQOT.Integer, 123,
            (int)SQOT.Integer, 1, (int)SQOT.Integer, 456,
            (int)SQOT.Null },
        new[] { 123, 456 },
        DisplayName = "two elements")]
    public void CreateTest(int[] array, int[] expected)
    {
        var sqarray = CreateTestHelper(TestUtils.MakeByteArray(array));

        Assert.AreEqual(SQOT.Array, sqarray.Type);
        for (var index = 0; index < (expected?.Length ?? 0); ++index)
        {
            var element = sqarray.Value.ElementAt(index);
            Assert.IsTrue(element is SQInteger);
            Assert.AreEqual(expected?[index], (SQInteger)element);
        }
    }

    [DataTestMethod]
    [DataRow(new[] { (int)SQOT.Array, 0, (int)SQOT.Null },
        DisplayName = "empty")]
    public void CreateTestEmpty(int[] array)
    {
        var sqarray = CreateTestHelper(TestUtils.MakeByteArray(array));

        Assert.AreEqual(SQOT.Array, sqarray.Type);
        Assert.AreEqual(0, sqarray.Value.Count());
    }

    [DataTestMethod]
    [DataRow(new[] { (int)SQOT.Array, 999, (int)SQOT.Integer, 0, (int)SQOT.Integer, 123, (int)SQOT.Null },
        DisplayName = "invalid size")]
    [DataRow(new[] { (int)SQOT.Array, 1, (int)SQOT.Integer, 0, (int)SQOT.Integer, (int)SQOT.Null },
        DisplayName = "missing value data")]
    [DataRow(new[] { (int)SQOT.Array, 1, (int)SQOT.Integer, 0, (int)SQOT.Integer, 123 },
        DisplayName = "missing sentinel")]
    [DataRow(new[] { (int)SQOT.Array, 1, (int)SQOT.Integer, 0, (int)SQOT.Null },
        DisplayName = "missing value")]
    [DataRow(new[] { (int)SQOT.Array, 1, (int)SQOT.Integer, 0, (int)SQOT.Integer },
        DisplayName = "missing value data and sentinel")]
    [DataRow(new[] { (int)SQOT.Array, 999, (int)SQOT.Null },
        DisplayName = "empty and invalid number of elements")]
    [DataRow(new[] { (int)SQOT.Array, (int)SQOT.Null },
        DisplayName = "empty and missing number of elements")]
    [DataRow(new[] { (int)SQOT.Array, 0 },
        DisplayName = "empty and missing sentinel")]
    [DataRow(new[] { (int)SQOT.Array },
        DisplayName = "empty and only array type")]
    public void CreateTestShortened(int[] array)
    {
        _ = Assert.ThrowsException<EndOfStreamException>(() => CreateTestHelper(TestUtils.MakeByteArray(array)));
    }

    [DataTestMethod]
    [DataRow(new[] { (int)SQOT.Null, 2, (int)SQOT.Integer, 0, (int)SQOT.Integer, 123, (int)SQOT.Null },
        DisplayName = "invalid type")]
    [DataRow(new[] { (int)SQOT.Array, -1, (int)SQOT.Integer, 0, (int)SQOT.Integer, 123, (int)SQOT.Null },
        DisplayName = "negative size")]
    [DataRow(new[] { (int)SQOT.Array, 0, (int)SQOT.Integer, 0, (int)SQOT.Integer, 123, (int)SQOT.Null },
        DisplayName = "zero size and one element")]
    [DataRow(new[] { (int)SQOT.Array, 1, (int)SQOT.Float, 0, (int)SQOT.Integer, 123, (int)SQOT.Null },
        DisplayName = "invalid index type")]
    [DataRow(new[] { (int)SQOT.Array, 1, (int)SQOT.Integer, 999, (int)SQOT.Integer, 123, (int)SQOT.Null },
        DisplayName = "invalid index data")]
    [DataRow(new[] { (int)SQOT.Array, 1, (int)SQOT.Integer, 0, 999, 123, (int)SQOT.Null },
        DisplayName = "invalid value type")]
    [DataRow(new[] { (int)SQOT.Array, 1, (int)SQOT.Integer, 0, (int)SQOT.Integer, 123, 999 },
        DisplayName = "invalid sentinel")]
    [DataRow(new[] { (int)SQOT.Array, (int)SQOT.Integer, 0, (int)SQOT.Integer, 123, (int)SQOT.Null },
        DisplayName = "missing number of elements")]
    [DataRow(new[] { (int)SQOT.Array, 1, 0, (int)SQOT.Integer, 123, (int)SQOT.Null },
        DisplayName = "missing index type")]
    [DataRow(new[] { (int)SQOT.Array, 1, (int)SQOT.Integer, (int)SQOT.Integer, 123, (int)SQOT.Null },
        DisplayName = "missing index data")]
    [DataRow(new[] { (int)SQOT.Array, 1, (int)SQOT.Integer, 0, 123, (int)SQOT.Null },
        DisplayName = "missing value type")]
    [DataRow(new[] { (int)SQOT.Array, 0, (int)SQOT.Integer, 123, (int)SQOT.Null },
        DisplayName = "missing number of elements and index type")]
    [DataRow(new[] { (int)SQOT.Array, (int)SQOT.Integer, 0, (int)SQOT.Integer, 123 },
        DisplayName = "missing number of elements and sentinel")]
    [DataRow(new[] { (int)SQOT.Array, 1, (int)SQOT.Integer, 123, (int)SQOT.Null },
        DisplayName = "missing index")]
    [DataRow(new[] { (int)SQOT.Array, 1, 0, (int)SQOT.Integer, (int)SQOT.Null },
        DisplayName = "missing index type and value data")]
    [DataRow(new[] { (int)SQOT.Array, 1, 0, (int)SQOT.Integer, 123 },
        DisplayName = "missing index type and sentinel")]
    [DataRow(new[] { (int)SQOT.Array, 1, (int)SQOT.Integer, (int)SQOT.Integer, 123 },
        DisplayName = "missing index data and sentinel")]
    [DataRow(new[] { (int)SQOT.Array, 1, (int)SQOT.Integer, 0, 123 },
        DisplayName = "missing value type and sentinel")]
    [DataRow(new[] { (int)SQOT.Array, 0, 999 },
        DisplayName = "empty and invalid sentinel")]
    public void CreateTestInvalid(int[] array)
    {
        _ = Assert.ThrowsException<InvalidDataException>(() => CreateTestHelper(TestUtils.MakeByteArray(array)));
    }

    [DataTestMethod]
    [DataRow(
        new[] { (int)SQOT.Array, 1, (int)SQOT.Integer, 0, (int)SQOT.Integer, 123, (int)SQOT.Null },
        "[ 123 ]",
        DisplayName = "one element")]
    [DataRow(
        new[] {
            (int)SQOT.Array, 2,
            (int)SQOT.Integer, 0, (int)SQOT.Integer, 123,
            (int)SQOT.Integer, 1, (int)SQOT.Integer, 456,
            (int)SQOT.Null },
        "[ 123, 456 ]",
        DisplayName = "two elements")]
    public void ToStringTest(int[] array, string expected)
    {
        var sqarray = CreateTestHelper(TestUtils.MakeByteArray(array));

        Assert.AreEqual(expected, sqarray.ToString());
    }

    [TestMethod]
    public void ToStringTestEmpty()
    {
        Assert.AreEqual("[  ]", new SQArray().ToString());
    }
}
