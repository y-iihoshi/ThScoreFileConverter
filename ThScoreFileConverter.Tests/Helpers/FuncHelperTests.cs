using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Tests.Helpers;

[TestClass]
public class FuncHelperTests
{

    [TestMethod]
    public void TrueTest()
    {
        Assert.IsTrue(FuncHelper.True(false));
        Assert.IsTrue(FuncHelper.True(true));
        Assert.IsTrue(FuncHelper.True(0));
        Assert.IsTrue(FuncHelper.True(1));
        Assert.IsTrue(FuncHelper.True(new int?()));
        Assert.IsTrue(FuncHelper.True(new object()));
        Assert.IsTrue(FuncHelper.True<object?>(null));
    }

    [TestMethod]
    public void MakeAndPredicateTest()
    {
        var pred = FuncHelper.MakeAndPredicate<int>(x => (x > 3), x => (x < 5));
        Assert.IsFalse(pred(3));
        Assert.IsTrue(pred(4));
        Assert.IsFalse(pred(5));
    }
}
