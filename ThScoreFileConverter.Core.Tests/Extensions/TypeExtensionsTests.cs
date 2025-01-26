using ThScoreFileConverter.Core.Extensions;

internal static class Global;

namespace ThScoreFileConverter.Core.Tests.Extensions
{
    [TestClass]
    public class TypeExtensionsTests
    {
        [TestMethod]
        public void GetLeafNamespaceTest()
        {
            Assert.AreEqual("System", typeof(Math).GetLeafNamespace());
            Assert.AreEqual("Linq", typeof(Enumerable).GetLeafNamespace());
            Assert.AreEqual("Extensions", typeof(TypeExtensionsTests).GetLeafNamespace());
        }

        [TestMethod]
        public void GetLeafNamespaceTestNull()
        {
            Type type = null!;
            _ = Assert.ThrowsException<ArgumentNullException>(type.GetLeafNamespace);
        }

        [TestMethod]
        public void GetLeafNamespaceTestTwice()
        {
            Assert.AreEqual("System", typeof(Math).GetLeafNamespace());
            Assert.AreEqual("System", typeof(Math).GetLeafNamespace());  // cached
        }

        [TestMethod]
        public void GetLeafNamespaceTestGlobal()
        {
            Assert.AreEqual(string.Empty, typeof(Global).GetLeafNamespace());
        }
    }
}
