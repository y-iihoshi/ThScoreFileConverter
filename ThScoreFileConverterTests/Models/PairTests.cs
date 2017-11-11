using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var obj1 = new PrivateObject(typeof(Pair<int, uint>), 1, 2u);
            var obj2 = new PrivateObject(typeof(Pair<uint, int>), 1u, 2);
            var obj3 = new PrivateObject(typeof(Pair<int, uint>), 1, 2u);
            var obj4 = new PrivateObject(typeof(Pair<int, uint>), 1, 3u);
            var obj5 = new PrivateObject(typeof(Pair<int, uint>), 3, 2u);
            var obj6 = new PrivateObject(typeof(Pair<int, uint>), 3, 4u);

            Assert.IsFalse((bool)obj1.Invoke("Equals", new object[] { null }));
            Assert.IsTrue((bool)obj1.Invoke("Equals", obj1.Target));
            Assert.IsFalse((bool)obj1.Invoke("Equals", obj2.Target));
            Assert.IsTrue((bool)obj1.Invoke("Equals", obj3.Target));
            Assert.IsFalse((bool)obj1.Invoke("Equals", obj4.Target));
            Assert.IsFalse((bool)obj1.Invoke("Equals", obj5.Target));
            Assert.IsFalse((bool)obj1.Invoke("Equals", obj6.Target));
        }

        [TestMethod()]
        public void GetHashCodeTest()
        {
            var obj1 = new PrivateObject(typeof(Pair<int, uint>), 1, 2u);
            var obj2 = new PrivateObject(typeof(Pair<int, uint>), 1, 2u);
            var obj3 = new PrivateObject(typeof(Pair<int, uint>), 3, 4u);

            Assert.IsTrue((int)obj1.Invoke("GetHashCode") == (int)obj2.Invoke("GetHashCode"));
            Assert.IsTrue((int)obj1.Invoke("GetHashCode") != (int)obj3.Invoke("GetHashCode"));
        }
    }
}