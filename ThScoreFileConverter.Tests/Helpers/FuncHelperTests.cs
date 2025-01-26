using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Tests.Helpers;

[TestClass]
public class FuncHelperTests
{

    [TestMethod]
    public void TrueTest()
    {
        FuncHelper.True(false).ShouldBeTrue();
        FuncHelper.True(true).ShouldBeTrue();
        FuncHelper.True(0).ShouldBeTrue();
        FuncHelper.True(1).ShouldBeTrue();
        FuncHelper.True(new int?()).ShouldBeTrue();
        FuncHelper.True(new object()).ShouldBeTrue();
        FuncHelper.True<object?>(null).ShouldBeTrue();
    }

    [TestMethod]
    public void MakeAndPredicateTest()
    {
        var pred = FuncHelper.MakeAndPredicate<int>(x => x > 3, x => x < 5);
        pred(3).ShouldBeFalse();
        pred(4).ShouldBeTrue();
        pred(5).ShouldBeFalse();
    }
}
