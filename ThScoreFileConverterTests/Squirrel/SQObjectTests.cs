using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Squirrel;
using ThScoreFileConverterTests.Models;
using SQOT = ThScoreFileConverter.Squirrel.SQObjectType;

namespace ThScoreFileConverterTests.Squirrel
{
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

            Assert.IsNotNull(sqnull);
            Assert.AreEqual(SQOT.Null, sqnull!.Type);
        }

        [TestMethod]
        public void CreateTestSQBool()
        {
            var sqbool = CreateTestHelper(TestUtils.MakeByteArray((int)SQOT.Bool, (byte)0x01)) as SQBool;

            Assert.IsNotNull(sqbool);
            Assert.AreEqual(SQOT.Bool, sqbool!.Type);
            Assert.IsTrue(sqbool.Value);
            Assert.IsTrue(sqbool);
        }

        [TestMethod]
        public void CreateTestSQInteger()
        {
            var expected = 123;

            var sqinteger = CreateTestHelper(TestUtils.MakeByteArray((int)SQOT.Integer, expected)) as SQInteger;

            Assert.IsNotNull(sqinteger);
            Assert.AreEqual(SQOT.Integer, sqinteger!.Type);
            Assert.AreEqual(expected, sqinteger.Value);
            Assert.AreEqual(expected, sqinteger);
        }

        [TestMethod]
        public void CreateTestSQFloat()
        {
            var expected = 0.25f;

            var sqfloat = CreateTestHelper(TestUtils.MakeByteArray((int)SQOT.Float, expected)) as SQFloat;

            Assert.IsNotNull(sqfloat);
            Assert.AreEqual(SQOT.Float, sqfloat!.Type);
            Assert.AreEqual(expected, sqfloat.Value);
            Assert.AreEqual(expected, sqfloat);
        }

        [TestMethod]
        public void CreateTestSQString()
        {
            var expected = "博麗 霊夢";
            var bytes = TestUtils.CP932Encoding.GetBytes(expected);

            var sqstring = CreateTestHelper(TestUtils.MakeByteArray((int)SQOT.String, bytes.Length, bytes)) as SQString;

            Assert.IsNotNull(sqstring);
            Assert.AreEqual(SQOT.String, sqstring!.Type);
            Assert.AreEqual(expected, sqstring.Value);
            Assert.AreEqual(expected, sqstring);
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

            Assert.IsNotNull(sqarray);
            Assert.AreEqual(SQOT.Array, sqarray!.Type);
            Assert.AreEqual(1, sqarray.Value.Count());
            Assert.IsTrue(sqarray.Value.First() is SQInteger);
            Assert.AreEqual(expected, (SQInteger)sqarray.Value.First());
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

            Assert.IsNotNull(sqtable);
            Assert.AreEqual(SQOT.Table, sqtable!.Type);
            Assert.AreEqual(1, sqtable.Value.Count());

            Assert.IsTrue(sqtable.Value.Keys.First() is SQInteger);
            Assert.AreEqual(expectedKey, (SQInteger)sqtable.Value.Keys.First());
            Assert.IsTrue(sqtable.Value[new SQInteger(expectedKey)] is SQInteger);
            Assert.AreEqual(expectedValue, (SQInteger)sqtable.Value[new SQInteger(expectedKey)]);
        }

        [TestMethod]
        public void CreateTestSQClosure()
        {
            var sqclosure = CreateTestHelper(TestUtils.MakeByteArray((int)SQOT.Closure)) as SQClosure;

            Assert.IsNotNull(sqclosure);
            Assert.AreEqual(SQOT.Closure, sqclosure!.Type);
        }

        [TestMethod]
        public void CreateTestSQInstance()
        {
            var sqinstance = CreateTestHelper(TestUtils.MakeByteArray((int)SQOT.Instance)) as SQInstance;

            Assert.IsNotNull(sqinstance);
            Assert.AreEqual(SQOT.Instance, sqinstance!.Type);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateTestNull()
        {
            _ = SQObject.Create(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void CreateTestShorteneed()
        {
            _ = CreateTestHelper(new byte[3]);

            Assert.Fail(TestUtils.Unreachable);
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
        [ExpectedException(typeof(InvalidDataException))]
        public void CreateTestInvalid(SQOT type)
        {
            _ = CreateTestHelper(TestUtils.MakeByteArray((int)type));

            Assert.Fail(TestUtils.Unreachable);
        }
    }
}
