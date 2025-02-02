using ThScoreFileConverter.Core.Extensions;

namespace ThScoreFileConverter.Core.Tests.Extensions;

[TestClass]
public class EnumerableExtensionsTests
{
    [TestMethod]
    public void CartesianTest()
    {
        var first = new[] { 1, 2 };
        var second = new[] { 3, 4, 5 };
        var expected = new[] { (1, 3), (1, 4), (1, 5), (2, 3), (2, 4), (2, 5) };

        first.Cartesian(second).ShouldBe(expected);
    }

    [TestMethod]
    public void CartesianTestFirstNull()
    {
        int[] first = null!;
        var second = new[] { 3, 4, 5 };

        _ = Should.Throw<ArgumentNullException>(() => first.Cartesian(second));
    }

    [TestMethod]
    public void CartesianTestSecondNull()
    {
        var first = new[] { 1, 2 };
        int[] second = null!;

        _ = Should.Throw<ArgumentNullException>(() => first.Cartesian(second));
    }
}
