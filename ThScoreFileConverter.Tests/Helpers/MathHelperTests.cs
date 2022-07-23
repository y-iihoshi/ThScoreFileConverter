using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Tests.Helpers;

[TestClass]
public class MathHelperTests
{
    [DataTestMethod]
    [DataRow(6, 2, 3, 0)]
    [DataRow(5, 2, 2, 1)]
    [DataRow(-6, 2, -3, 0)]
    [DataRow(-5, 2, -2, -1)]
    [DataRow(6, -2, -3, 0)]
    [DataRow(5, -2, -2, 1)]
    [DataRow(-6, -2, 3, 0)]
    [DataRow(-5, -2, 2, -1)]
    public void DivRemTest(int left, int right, int div, int rem)
    {
        var (d, r) = MathHelper.DivRem(left, right);
        Assert.AreEqual(div, d);
        Assert.AreEqual(rem, r);
    }
}
