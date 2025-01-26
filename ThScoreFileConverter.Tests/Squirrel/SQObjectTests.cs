using ThScoreFileConverter.Squirrel;
using SQOT = ThScoreFileConverter.Squirrel.SQObjectType;

namespace ThScoreFileConverter.Tests.Squirrel;

[TestClass]
public class SQObjectTests
{
    internal static SQObject CreateTestHelper(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        using var reader = new BinaryReader(stream);

        return SQObject.Create(reader);
    }

    [TestMethod]
    public void CreateTestSQNull()
    {
        var sqnull = CreateTestHelper(TestUtils.MakeByteArray((int)SQOT.Null)) as SQNull;

        var instance = sqnull.ShouldNotBeNull();
        instance.Type.ShouldBe(SQOT.Null);
    }

    [TestMethod]
    public void CreateTestSQBool()
    {
        var sqbool = CreateTestHelper(TestUtils.MakeByteArray((int)SQOT.Bool, (byte)0x01)) as SQBool;

        var instance = sqbool.ShouldNotBeNull();
        instance.Type.ShouldBe(SQOT.Bool);
        instance.Value.ShouldBeTrue();
        ((bool)instance).ShouldBeTrue();
    }

    [TestMethod]
    public void CreateTestSQInteger()
    {
        var expected = 123;

        var sqinteger = CreateTestHelper(TestUtils.MakeByteArray((int)SQOT.Integer, expected)) as SQInteger;

        var instance = sqinteger.ShouldNotBeNull();
        instance.Type.ShouldBe(SQOT.Integer);
        instance.Value.ShouldBe(expected);
        ((int)instance).ShouldBe(expected);
    }

    [TestMethod]
    public void CreateTestSQFloat()
    {
        var expected = 0.25f;

        var sqfloat = CreateTestHelper(TestUtils.MakeByteArray((int)SQOT.Float, expected)) as SQFloat;

        var instance = sqfloat.ShouldNotBeNull();
        instance.Type.ShouldBe(SQOT.Float);
        instance.Value.ShouldBe(expected);
        ((float)instance).ShouldBe(expected);
    }

    [TestMethod]
    public void CreateTestSQString()
    {
        var expected = "博麗 霊夢";
        var bytes = TestUtils.CP932Encoding.GetBytes(expected);

        var sqstring = CreateTestHelper(TestUtils.MakeByteArray((int)SQOT.String, bytes.Length, bytes)) as SQString;

        var instance = sqstring.ShouldNotBeNull();
        instance.Type.ShouldBe(SQOT.String);
        instance.Value.ShouldBe(expected);
        ((string)instance).ShouldBe(expected);
    }

    [TestMethod]
    public void CreateTestSQArray()
    {
        var expected = 123;
        var array = new[]
        {
            (int)SQOT.Array, 1,
            (int)SQOT.Integer, 0, (int)SQOT.Integer, expected,
            (int)SQOT.Null,
        };

        var sqarray = CreateTestHelper(TestUtils.MakeByteArray(array)) as SQArray;

        var instance = sqarray.ShouldNotBeNull();
        instance.Type.ShouldBe(SQOT.Array);
        instance.Value.Count().ShouldBe(1);
        instance.Value.First().ShouldBeOfType<SQInteger>().Value.ShouldBe(expected);
    }

    [TestMethod]
    public void CreateTestSQTable()
    {
        var expectedKey = 123;
        var expectedValue = 456;
        var array = new[]
        {
            (int)SQOT.Table,
            (int)SQOT.Integer, expectedKey, (int)SQOT.Integer, expectedValue,
            (int)SQOT.Null,
        };

        var sqtable = CreateTestHelper(TestUtils.MakeByteArray(array)) as SQTable;

        var instance = sqtable.ShouldNotBeNull();
        instance.Type.ShouldBe(SQOT.Table);
        instance.Value.Count.ShouldBe(1);
        instance.Value.Keys.First().ShouldBeOfType<SQInteger>().Value.ShouldBe(expectedKey);
        instance.Value[new SQInteger(expectedKey)].ShouldBeOfType<SQInteger>().Value.ShouldBe(expectedValue);
    }

    [TestMethod]
    public void CreateTestSQClosure()
    {
        var sqclosure = CreateTestHelper(TestUtils.MakeByteArray((int)SQOT.Closure)) as SQClosure;

        var instance = sqclosure.ShouldNotBeNull();
        instance.Type.ShouldBe(SQOT.Closure);
    }

    [TestMethod]
    public void CreateTestSQInstance()
    {
        var sqinstance = CreateTestHelper(TestUtils.MakeByteArray((int)SQOT.Instance)) as SQInstance;

        var instance = sqinstance.ShouldNotBeNull();
        instance.Type.ShouldBe(SQOT.Instance);
    }

    [TestMethod]
    public void CreateTestShorteneed()
    {
        _ = Should.Throw<EndOfStreamException>(() => CreateTestHelper(new byte[3]));
    }

    [DataTestMethod]
    [DataRow(SQOT.Class)]
    [DataRow(SQOT.FuncProto)]
    [DataRow(SQOT.Generator)]
    [DataRow(SQOT.NativeClosure)]
    [DataRow(SQOT.Outer)]
    [DataRow(SQOT.Thread)]
    [DataRow(SQOT.UserData)]
    [DataRow(SQOT.UserPointer)]
    [DataRow(SQOT.WeakRef)]
    public void CreateTestInvalid(SQOT type)
    {
        _ = Should.Throw<InvalidDataException>(() => CreateTestHelper(TestUtils.MakeByteArray((int)type)));
    }
}
