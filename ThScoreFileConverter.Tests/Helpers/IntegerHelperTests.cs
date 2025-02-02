using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Tests.Helpers;

[TestClass]
public class IntegerHelperTests
{
    [TestMethod]
    public void ToZeroBasedTest()
    {
        IntegerHelper.ToZeroBased(1).ShouldBe(0);
        IntegerHelper.ToZeroBased(2).ShouldBe(1);
        IntegerHelper.ToZeroBased(9).ShouldBe(8);
        IntegerHelper.ToZeroBased(0).ShouldBe(9);   // Hmm...
    }

    [TestMethod]
    public void ToZeroBasedTestNegative()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => IntegerHelper.ToZeroBased(-1));
    }

    [TestMethod]
    public void ToZeroBasedTestExceeded()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => IntegerHelper.ToZeroBased(10));
    }

    [TestMethod]
    public void ToOneBasedTest()
    {
        IntegerHelper.ToOneBased(0).ShouldBe(1);
        IntegerHelper.ToOneBased(1).ShouldBe(2);
        IntegerHelper.ToOneBased(8).ShouldBe(9);
        IntegerHelper.ToOneBased(9).ShouldBe(0);    // Hmm...
    }

    [TestMethod]
    public void ToOneBasedTestNegative()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => IntegerHelper.ToOneBased(-1));
    }

    [TestMethod]
    public void ToOneBasedTestExceeded()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => IntegerHelper.ToOneBased(10));
    }

    [DataTestMethod]
    [DataRow(0, 1)]
    [DataRow(1, 1)]
    [DataRow(9, 1)]
    [DataRow(10, 2)]
    [DataRow(11, 2)]
    [DataRow(99, 2)]
    [DataRow(100, 3)]
    [DataRow(101, 3)]
    [DataRow(-1, 1)]
    [DataRow(-9, 1)]
    [DataRow(-10, 2)]
    [DataRow(-11, 2)]
    [DataRow(-99, 2)]
    [DataRow(-100, 3)]
    [DataRow(-101, 3)]
    public void GetNumDigitsTest(int value, int numDigits)
    {
        IntegerHelper.GetNumDigits(value).ShouldBe(numDigits);
    }
}
