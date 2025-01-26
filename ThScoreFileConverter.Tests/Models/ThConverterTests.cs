using NSubstitute;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverter.Tests.Models;

[TestClass]
public class ThConverterTests
{
    [TestMethod]
    public void ThConverterTest()
    {
        var converter = Substitute.ForPartsOf<ThConverter>();

        Assert.AreEqual(string.Empty, converter.SupportedVersions);
        Assert.IsFalse(converter.HasBestShotConverter);
        Assert.IsTrue(converter.HasCardReplacer);
    }

    [TestMethod]
    public void ConvertTestNull()
    {
        var converter = Substitute.ForPartsOf<ThConverter>();

        converter.ConvertFinished +=
            (sender, e) => Assert.Fail($"{nameof(converter.ConvertFinished)}: {TestUtils.Unreachable}");
        converter.ConvertAllFinished +=
            (sender, e) => Assert.Fail($"{nameof(converter.ConvertAllFinished)}: {TestUtils.Unreachable}");
        converter.ExceptionOccurred +=
            (sender, e) => Console.WriteLine($"{nameof(converter.ExceptionOccurred)}: {e.Exception}");

        _ = Assert.ThrowsException<ArgumentNullException>(() => converter.Convert(null!));
    }

    [TestMethod]
    public void ConvertTestInvalidType()
    {
        var converter = Substitute.ForPartsOf<ThConverter>();

        converter.ConvertFinished +=
            (sender, e) => Assert.Fail($"{nameof(converter.ConvertFinished)}: {TestUtils.Unreachable}");
        converter.ConvertAllFinished +=
            (sender, e) => Assert.Fail($"{nameof(converter.ConvertAllFinished)}: {TestUtils.Unreachable}");
        converter.ExceptionOccurred +=
            (sender, e) => Console.WriteLine($"{nameof(converter.ExceptionOccurred)}: {e.Exception}");

        _ = Assert.ThrowsException<ArgumentException>(() => converter.Convert(1));
    }

    [TestMethod]
    public void ConvertTestNoSettings()
    {
        var settings = new SettingsPerTitle();
        var formatter = Substitute.For<INumberFormatter>();
        var converter = Substitute.ForPartsOf<ThConverter>();

        converter.ConvertFinished +=
            (sender, e) => Assert.Fail($"{nameof(converter.ConvertFinished)}: {TestUtils.Unreachable}");
        converter.ConvertAllFinished +=
            (sender, e) => Assert.Fail($"{nameof(converter.ConvertAllFinished)}: {TestUtils.Unreachable}");
        converter.ExceptionOccurred +=
            (sender, e) => Console.WriteLine($"{nameof(converter.ExceptionOccurred)}: {e.Exception}");

        _ = Assert.ThrowsException<ArgumentException>(() => converter.Convert((settings, formatter)));
    }
}
