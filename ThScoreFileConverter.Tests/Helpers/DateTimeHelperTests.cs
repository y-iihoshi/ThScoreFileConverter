using System.Globalization;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Tests.Helpers;

[TestClass]
public class DateTimeHelperTests
{
    [TestMethod]
    public void FormatTest()
    {
        DateTimeHelper.InvalidFormat.Length.ShouldBe(DateTimeHelper.ValidFormat.Length);
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(0)]
    [DataRow(1234567)]
    public void GetStringTest(int unixTime)
    {
        var expected = DateTime.UnixEpoch.AddSeconds(unixTime).ToLocalTime()
            .ToString(DateTimeHelper.ValidFormat, CultureInfo.CurrentCulture);
        DateTimeHelper.GetString(unixTime).ShouldBe(expected);
    }

    [TestMethod]
    public void GetStringTestNull()
    {
        DateTimeHelper.GetString(null).ShouldBe(DateTimeHelper.InvalidFormat);
    }
}
