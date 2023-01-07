using System;
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

#if NET6_0_OR_GREATER
        var expected = string.Create(CultureInfo.CurrentCulture, $"{now:F}");
#else
        var expected = string.Format(CultureInfo.CurrentCulture, "{0:F}", now);
#endif
        var actual = StringHelper.Create($"{now:F}");

        Assert.AreEqual(expected, actual);
    }
}
