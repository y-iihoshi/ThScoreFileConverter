using CommunityToolkit.Diagnostics;
using ThScoreFileConverter.Squirrel;
using SQOT = ThScoreFileConverter.Squirrel.SQObjectType;

namespace ThScoreFileConverter.Tests.Squirrel;

[TestClass]
public class SQArrayTests
{
    [TestMethod]
    public void SQArrayTest()
    {
        var sqarray = new SQArray();

        sqarray.Type.ShouldBe(SQOT.Array);
        sqarray.Value.ShouldBeEmpty();
    }

    internal static SQArray CreateTestHelper(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        using var reader = new BinaryReader(stream);

        return SQArray.Create(reader);
    }

    [TestMethod]
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
        Guard.IsNotNull(array);
        Guard.IsNotNull(expected);

        var sqarray = CreateTestHelper(TestUtils.MakeByteArray(array));

        sqarray.Type.ShouldBe(SQOT.Array);
        for (var index = 0; index < expected.Length; ++index)
        {
            var element = sqarray.Value.ElementAt(index);
            var value = element.ShouldBeOfType<SQInteger>();
            ((int)value).ShouldBe(expected[index]);
        }
    }

    [TestMethod]
    [DataRow(new[] { (int)SQOT.Array, 0, (int)SQOT.Null },
        DisplayName = "empty")]
    public void CreateTestEmpty(int[] array)
    {
        var sqarray = CreateTestHelper(TestUtils.MakeByteArray(array));

        sqarray.Type.ShouldBe(SQOT.Array);
        sqarray.Value.ShouldBeEmpty();
    }

    [TestMethod]
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
        _ = Should.Throw<EndOfStreamException>(() => CreateTestHelper(TestUtils.MakeByteArray(array)));
    }

    [TestMethod]
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
        _ = Should.Throw<InvalidDataException>(() => CreateTestHelper(TestUtils.MakeByteArray(array)));
    }

    [TestMethod]
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

        sqarray.ToString().ShouldBe(expected);
    }

    [TestMethod]
    public void ToStringTestEmpty()
    {
        new SQArray().ToString().ShouldBe("[  ]");
    }
}
