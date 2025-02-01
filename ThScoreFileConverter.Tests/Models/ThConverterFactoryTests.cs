using ThScoreFileConverter.Models;

namespace ThScoreFileConverter.Tests.Models;

[TestClass]
public class ThConverterFactoryTests
{
    [TestMethod]
    public void CanCreateTest()
    {
        ThConverterFactory.CanCreate("TH06").ShouldBeTrue();
    }

    [TestMethod]
    public void CanCreateTestEmptyKey()
    {
        ThConverterFactory.CanCreate(string.Empty).ShouldBeFalse();
    }

    [TestMethod]
    public void CanCreateTestInvalidKey()
    {
        ThConverterFactory.CanCreate("invalidKey").ShouldBeFalse();
    }

    [TestMethod]
    public void CreateTest()
    {
        var converter = ThConverterFactory.Create("TH06");
        var converterType = converter?.GetType();

        converterType.ShouldBe(typeof(ThScoreFileConverter.Models.Th06.Converter));
    }

    [TestMethod]
    public void CreateTestEmptyKey()
    {
        var converter = ThConverterFactory.Create(string.Empty);

        converter.ShouldBeNull();
    }

    [TestMethod]
    public void CreateTestInvalidKey()
    {
        var converter = ThConverterFactory.Create("invalidKey");

        converter.ShouldBeNull();
    }
}
