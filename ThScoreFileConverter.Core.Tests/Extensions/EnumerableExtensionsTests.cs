using System;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Tests.UnitTesting;

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

        CollectionAssert.That.AreEqual(expected, first.Cartesian(second));
    }

    [TestMethod]
    public void CartesianTestFirstNull()
    {
        int[] first = null!;
        var second = new[] { 3, 4, 5 };

        _ = Assert.ThrowsException<ArgumentNullException>(() => first.Cartesian(second));
    }

    [TestMethod]
    public void CartesianTestSecondNull()
    {
        var first = new[] { 1, 2 };
        int[] second = null!;

        _ = Assert.ThrowsException<ArgumentNullException>(() => first.Cartesian(second));
    }
}
