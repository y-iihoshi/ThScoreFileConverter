using CommunityToolkit.Diagnostics;
using ThScoreFileConverter.Squirrel;
using SQOT = ThScoreFileConverter.Squirrel.SQObjectType;

namespace ThScoreFileConverter.Tests.Squirrel;

[TestClass]
public class SQTableTests
{
    [TestMethod]
    public void SQTableTest()
    {
        var sqtable = new SQTable();

        sqtable.Type.ShouldBe(SQOT.Table);
        sqtable.Value.ShouldBeEmpty();
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
        Guard.IsNotNull(array);
        Guard.IsNotNull(expectedKeys);
        Guard.IsNotNull(expectedValues);

        var sqtable = CreateTestHelper(TestUtils.MakeByteArray(array));

        sqtable.Type.ShouldBe(SQOT.Table);

        for (var index = 0; index < expectedKeys.Length; ++index)
        {
            var key = sqtable.Value.Keys.ElementAt(index);
            key.ShouldBeOfType<SQInteger>().Value.ShouldBe(expectedKeys[index]);
        }

        for (var index = 0; index < expectedValues.Length; ++index)
        {
            var value = sqtable.Value.Values.ElementAt(index);
            value.ShouldBeOfType<SQInteger>().Value.ShouldBe(expectedValues[index]);
        }
    }

    [DataTestMethod]
    [DataRow(new[] { (int)SQOT.Table, (int)SQOT.Null },
        DisplayName = "empty")]
    public void CreateTestEmpty(int[] array)
    {
        var sqtable = CreateTestHelper(TestUtils.MakeByteArray(array));

        sqtable.Type.ShouldBe(SQOT.Table);
        sqtable.Value.ShouldBeEmpty();
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
        _ = Should.Throw<EndOfStreamException>(() => CreateTestHelper(TestUtils.MakeByteArray(array)));
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
        _ = Should.Throw<InvalidDataException>(() => CreateTestHelper(TestUtils.MakeByteArray(array)));
    }

#pragma warning disable JSON002 // Probable JSON string detected
    [DataTestMethod]
    [DataRow(
        new[] { (int)SQOT.Table, (int)SQOT.Integer, 123, (int)SQOT.Integer, 456, (int)SQOT.Null },
        "{ 123: 456 }",
        DisplayName = "one pair")]
    [DataRow(
        new[] {
            (int)SQOT.Table,
            (int)SQOT.Integer, 123, (int)SQOT.Integer, 456,
            (int)SQOT.Integer, 78, (int)SQOT.Integer, 90,
            (int)SQOT.Null },
        "{ 123: 456, 78: 90 }",
        DisplayName = "two pairs")]
#pragma warning restore JSON002 // Probable JSON string detected
    public void ToStringTest(int[] array, string expected)
    {
        var sqtable = CreateTestHelper(TestUtils.MakeByteArray(array));

        sqtable.ToString().ShouldBe(expected);
    }

    [TestMethod]
    public void ToStringTestEmpty()
    {
        new SQTable().ToString().ShouldBe("{  }");
    }

    [TestMethod]
    public void ToDictionaryTest()
    {
        var sqtable = new SQTable(new Dictionary<SQObject, SQObject>
        {
            { new SQInteger(12), new SQInteger(34) },
            { new SQString("intKey"), new SQInteger(56) },
            { new SQString("floatKey"), new SQFloat(78f) },
        });

        var table = sqtable.ToDictionary(
            key => key is SQString,
            value => value is SQInteger,
            key => (string)(SQString)key,
            value => (int)(SQInteger)value);

        table.Count.ShouldBe(1);
        table.Keys.First().ShouldBe("intKey");
        table["intKey"].ShouldBe(56);
    }

    [TestMethod]
    public void ToDictionaryTestEmpty()
    {
        var sqtable = new SQTable();

        var table = sqtable.ToDictionary(
            key => key is SQString,
            value => value is SQInteger,
            key => (string)(SQString)key,
            value => (int)(SQInteger)value);

        table.ShouldBeEmpty();
    }

    [TestMethod]
    public void GetValueOrDefaultTest()
    {
        var sqtable = new SQTable(new Dictionary<SQObject, SQObject>
        {
            { new SQString("boolKey"), SQBool.True },
            { new SQString("intKey"), new SQInteger(123) },
            { new SQString("floatKey"), new SQFloat(456f) },
        });

        sqtable.GetValueOrDefault<bool>("boolKey").ShouldBeTrue();
        sqtable.GetValueOrDefault<int>("intKey").ShouldBe(123);
        sqtable.GetValueOrDefault<float>("floatKey").ShouldBe(456f);
    }

    [TestMethod]
    public void GetValueOrDefaultTestEmpty()
    {
        var sqtable = new SQTable();

        sqtable.GetValueOrDefault<bool>("boolKey").ShouldBe(default);
        sqtable.GetValueOrDefault<int>("intKey").ShouldBe(default);
        sqtable.GetValueOrDefault<float>("floatKey").ShouldBe(default);
    }

    [TestMethod]
    public void GetValueOrDefaultTestWrongType()
    {
        var sqtable = new SQTable(new Dictionary<SQObject, SQObject>
        {
            { new SQString("boolKey"), SQBool.True },
            { new SQString("intKey"), new SQInteger(123) },
            { new SQString("floatKey"), new SQFloat(456f) },
        });

        sqtable.GetValueOrDefault<int>("boolKey").ShouldBe(default);
        sqtable.GetValueOrDefault<float>("intKey").ShouldBe(default);
        sqtable.GetValueOrDefault<bool>("floatKey").ShouldBe(default);
    }
}
