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

        converter.SupportedVersions.ShouldBeEmpty();
        converter.HasBestShotConverter.ShouldBeFalse();
        converter.HasCardReplacer.ShouldBeTrue();
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

        _ = Should.Throw<ArgumentNullException>(() => converter.Convert(null!));
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

        _ = Should.Throw<ArgumentException>(() => converter.Convert(1));
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

        _ = Should.Throw<ArgumentException>(() => converter.Convert((settings, formatter)));
    }
}
