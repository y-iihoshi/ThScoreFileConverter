using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Squirrel;
using ThScoreFileConverterTests.Models;

namespace ThScoreFileConverterTests.Squirrel
{
    [TestClass]
    public class SQFloatTests
    {
        [TestMethod]
        public void SQFloatTest()
        {
            var sqfloat = new SQFloat();

            Assert.AreEqual(SQObjectType.Float, sqfloat.Type);
            Assert.AreEqual(0f, sqfloat.Value);
            Assert.AreEqual(0f, sqfloat);
        }

        internal static SQFloat CreateTestHelper(byte[] bytes)
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(bytes);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;

                    return SQFloat.Create(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0f)]
        [DataRow(1f)]
        [DataRow(-1f)]
        [DataRow(0.25f)]
        [DataRow(0.1f)]
        [DataRow(float.NaN)]
        [DataRow(float.PositiveInfinity)]
        [DataRow(float.NegativeInfinity)]
        public void CreateTest(float expected)
        {
            var sqfloat = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Float, expected));

            Assert.AreEqual(SQObjectType.Float, sqfloat.Type);
            Assert.AreEqual(expected, sqfloat.Value);
            Assert.AreEqual(expected, sqfloat);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateTestNull()
        {
            _ = SQFloat.Create(null);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void CreateTestShortened()
        {
            _ = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Float, new byte[3]));

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void CreateTestInvalid()
        {
            _ = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Null, 0f));

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void EqualsTestNull() => Assert.IsFalse(new SQFloat().Equals(null));

        [TestMethod]
        public void EqualsTestNullObject() => Assert.IsFalse(new SQFloat().Equals(null as object));

        [TestMethod]
        public void EqualsTestInvalidType() => Assert.IsFalse(new SQFloat().Equals(SQNull.Instance));

        [TestMethod]
        public void EqualsTestSelf()
        {
            var value = new SQFloat();

            Assert.IsTrue(value.Equals(value));
        }

        [TestMethod]
        public void EqualsTestSelfObject()
        {
            var value = new SQFloat();

            Assert.IsTrue(value.Equals(value as object));
        }

        [TestMethod]
        public void EqualsTestEqual() => Assert.IsTrue(new SQFloat().Equals(new SQFloat(0f)));

        [TestMethod]
        public void EqualsTestNotEqual() => Assert.IsFalse(new SQFloat().Equals(new SQFloat(1f)));

        [TestMethod]
        public void GetHashCodeEqual() => Assert.AreEqual(new SQFloat().GetHashCode(), new SQFloat(0f).GetHashCode());

        [TestMethod]
        public void GetHashCodeNotEqual()
            => Assert.AreNotEqual(new SQFloat().GetHashCode(), new SQFloat(1f).GetHashCode());
    }
}
