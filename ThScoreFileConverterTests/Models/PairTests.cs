using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace ThScoreFileConverter.Models.Tests
{
    [TestClass()]
    public class PairTests
    {
        [TestMethod()]
        public void PairTest()
        {
            var obj = new PrivateObject(typeof(Pair<int, uint>), 1, 2u);

            Assert.AreEqual(1, obj.GetProperty("First"));
            Assert.AreEqual(2u, obj.GetProperty("Second"));
        }

        [TestMethod()]
        public void EqualsTest()
        {
            var IntUIntPairType = typeof(Pair<int, uint>);
            var UIntIntPairType = typeof(Pair<uint, int>);

            var obj1 = new PrivateObject(IntUIntPairType, 1, 2u);
            var obj2 = new PrivateObject(UIntIntPairType, 1u, 2);
            var obj3 = new PrivateObject(IntUIntPairType, 1, 2u);
            var obj4 = new PrivateObject(IntUIntPairType, 1, 3u);
            var obj5 = new PrivateObject(IntUIntPairType, 3, 2u);
            var obj6 = new PrivateObject(IntUIntPairType, 3, 4u);

            Func<PrivateObject, object, bool> equals =
                (pobj, arg) => (bool)pobj.Invoke("Equals", new object[] { arg }, CultureInfo.InvariantCulture);

            Assert.IsFalse(equals(obj1, null));
            Assert.IsTrue(equals(obj1, obj1.Target));
            Assert.IsFalse(equals(obj1, obj2.Target));
            Assert.IsTrue(equals(obj1, obj3.Target));
            Assert.IsFalse(equals(obj1, obj4.Target));
            Assert.IsFalse(equals(obj1, obj5.Target));
            Assert.IsFalse(equals(obj1, obj6.Target));
        }

        [TestMethod()]
        public void GetHashCodeTest()
        {
            var type = typeof(Pair<int, uint>);

            var obj1 = new PrivateObject(type, 1, 2u);
            var obj2 = new PrivateObject(type, 1, 2u);
            var obj3 = new PrivateObject(type, 3, 4u);

            Func<PrivateObject, int> getHashCode =
                pobj => (int)pobj.Invoke("GetHashCode", new object[] { }, CultureInfo.InvariantCulture);

            Assert.IsTrue(getHashCode(obj1) == getHashCode(obj2));
            Assert.IsTrue(getHashCode(obj1) != getHashCode(obj3));
        }
    }
}
