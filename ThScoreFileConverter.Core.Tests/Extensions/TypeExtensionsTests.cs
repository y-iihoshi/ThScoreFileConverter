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
            typeof(Math).GetLeafNamespace().ShouldBe("System");
            typeof(Enumerable).GetLeafNamespace().ShouldBe("Linq");
            typeof(TypeExtensionsTests).GetLeafNamespace().ShouldBe("Extensions");
        }

        [TestMethod]
        public void GetLeafNamespaceTestNull()
        {
            Type type = null!;
            _ = Should.Throw<ArgumentNullException>(type.GetLeafNamespace);
        }

        [TestMethod]
        public void GetLeafNamespaceTestTwice()
        {
            typeof(Math).GetLeafNamespace().ShouldBe("System");
            typeof(Math).GetLeafNamespace().ShouldBe("System");  // cached
        }

        [TestMethod]
        public void GetLeafNamespaceTestGlobal()
        {
            typeof(Global).GetLeafNamespace().ShouldBeEmpty();
        }
    }
}
