using ThScoreFileConverter.Models;

namespace ThScoreFileConverter.Tests.Models;

[TestClass]
public class ThConverterEventArgsTests
{
    [TestMethod]
    public void ThConverterEventArgsTest()
    {
        var args = new ThConverterEventArgs(@"path\to\file", 2, 5);

        args.Path.ShouldBe(@"path\to\file");
        args.Current.ShouldBe(2);
        args.Total.ShouldBe(5);

        var message = args.Message;

        message.ShouldContain("file", Case.Sensitive);
        message.ShouldContain("2");
        message.ShouldContain("5");
    }

    [TestMethod]
    public void ThConverterEventArgsTestDefault()
    {
        var args = new ThConverterEventArgs();

        args.Path.ShouldBeEmpty();
        args.Current.ShouldBe(default);
        args.Total.ShouldBe(default);
    }

    [TestMethod]
    public void ThConverterEventArgsTestEmptyPath()
    {
        _ = Should.Throw<ArgumentException>(() => new ThConverterEventArgs(string.Empty, 2, 5));
    }

    [TestMethod]
    public void ThConverterEventArgsTestNegativeCurrent()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(
            () => new ThConverterEventArgs(@"path\to\file", -1, 5));
    }

    [TestMethod]
    public void ThConverterEventArgsTestZeroCurrent()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(
            () => new ThConverterEventArgs(@"path\to\file", 0, 5));
    }

    [TestMethod]
    public void ThConverterEventArgsTestExtendedCurrent()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(
            () => new ThConverterEventArgs(@"path\to\file", 6, 5));
    }

    [TestMethod]
    public void ThConverterEventArgsTestNegativeTotal()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(
            () => new ThConverterEventArgs(@"path\to\file", 2, -1));
    }

    [TestMethod]
    public void ThConverterEventArgsTestZeroTotal()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(
            () => new ThConverterEventArgs(@"path\to\file", 2, 0));
    }
}
