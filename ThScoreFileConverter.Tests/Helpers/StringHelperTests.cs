using System.Globalization;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Tests.Helpers;

[TestClass]
public class StringHelperTests
{
    [TestMethod]
    public void CreateTest()
    {
        var now = DateTime.Now;
        var expected = string.Create(CultureInfo.CurrentCulture, $"{now:F}");

        StringHelper.Create($"{now:F}").ShouldBe(expected);
    }

    [TestMethod]
    public void FormatTest()
    {
        var now = DateTime.Now;
        var expected = string.Format(CultureInfo.CurrentCulture, "{0:F}", now);

        StringHelper.Format("{0:F}", now).ShouldBe(expected);
    }
}
