using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThScoreFileConverter.Models.Tests
{
    [TestClass()]
    public class PairTests
    {
        [TestMethod()]
        public void PairTest()
        {
            var pobj = new PrivateObject(typeof(Pair<int, uint>), 1, 2u);

            Assert.AreEqual(1, pobj.GetProperty("First"));
            Assert.AreEqual(2u, pobj.GetProperty("Second"));
        }

        [TestMethod()]
        public void EqualsTestNull()
        {
            var pobj = new PrivateObject(typeof(Pair<int, uint>), 1, 2u);
            var pair = pobj.Target as Pair<int, uint>;

            Assert.IsFalse(pair.Equals(null));
        }

        [TestMethod()]
        public void EqualsTestInvalidType()
        {
            var pobj = new PrivateObject(typeof(Pair<int, uint>), 1, 2u);
            var pair = pobj.Target as Pair<int, uint>;

            Assert.IsFalse(pair.Equals(1));
        }

        [TestMethod()]
        public void EqualsTestSelf()
        {
            var pobj = new PrivateObject(typeof(Pair<int, uint>), 1, 2u);
            var pair = pobj.Target as Pair<int, uint>;

            Assert.IsTrue(pair.Equals(pair));
        }

        [TestMethod()]
        public void EqualsTestEqualBoth()
        {
            var pobj1 = new PrivateObject(typeof(Pair<int, uint>), 1, 2u);
            var pair1 = pobj1.Target as Pair<int, uint>;

            var pobj2 = new PrivateObject(typeof(Pair<int, uint>), 1, 2u);
            var pair2 = pobj2.Target as Pair<int, uint>;

            Assert.IsTrue(pair1.Equals(pair2));
        }

        [TestMethod()]
        public void EqualsTestEqualFirst()
        {
            var pobj1 = new PrivateObject(typeof(Pair<int, uint>), 1, 2u);
            var pair1 = pobj1.Target as Pair<int, uint>;

            var pobj2 = new PrivateObject(typeof(Pair<int, uint>), 1, 3u);
            var pair2 = pobj2.Target as Pair<int, uint>;

            Assert.IsFalse(pair1.Equals(pair2));
        }

        [TestMethod()]
        public void EqualsTestEqualSecond()
        {
            var pobj1 = new PrivateObject(typeof(Pair<int, uint>), 1, 2u);
            var pair1 = pobj1.Target as Pair<int, uint>;

            var pobj2 = new PrivateObject(typeof(Pair<int, uint>), 3, 2u);
            var pair2 = pobj2.Target as Pair<int, uint>;

            Assert.IsFalse(pair1.Equals(pair2));
        }

        [TestMethod()]
        public void EqualsTestNotEqualBoth()
        {
            var pobj1 = new PrivateObject(typeof(Pair<int, uint>), 1, 2u);
            var pair1 = pobj1.Target as Pair<int, uint>;

            var pobj2 = new PrivateObject(typeof(Pair<int, uint>), 3, 4u);
            var pair2 = pobj2.Target as Pair<int, uint>;

            Assert.IsFalse(pair1.Equals(pair2));
        }

        [TestMethod()]
        public void GetHashCodeTestEqual()
        {
            var obj1 = new PrivateObject(typeof(Pair<int, uint>), 1, 2u);
            var pair1 = obj1.Target as Pair<int, uint>;

            var obj2 = new PrivateObject(typeof(Pair<int, uint>), 1, 2u);
            var pair2 = obj2.Target as Pair<int, uint>;

            Assert.IsTrue(pair1.GetHashCode() == pair2.GetHashCode());
        }

        [TestMethod()]
        public void GetHashCodeTestNotEqual()
        {
            var obj1 = new PrivateObject(typeof(Pair<int, uint>), 1, 2u);
            var pair1 = obj1.Target as Pair<int, uint>;

            var obj2 = new PrivateObject(typeof(Pair<int, uint>), 3, 4u);
            var pair2 = obj2.Target as Pair<int, uint>;

            Assert.IsTrue(pair1.GetHashCode() != pair2.GetHashCode());
        }
    }
}
