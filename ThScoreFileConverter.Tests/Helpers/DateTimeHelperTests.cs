using System.Globalization;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Tests.Helpers;

[TestClass]
public class DateTimeHelperTests
{
    [TestMethod]
    public void FormatTest()
    {
        Assert.AreEqual(DateTimeHelper.ValidFormat.Length, DateTimeHelper.InvalidFormat.Length);
    }

    [DataTestMethod]
    [DataRow(-1)]
    [DataRow(0)]
    [DataRow(1234567)]
    public void GetStringTest(int unixTime)
    {
        var expected = DateTime.UnixEpoch.AddSeconds(unixTime).ToLocalTime()
            .ToString(DateTimeHelper.ValidFormat, CultureInfo.CurrentCulture);
        Assert.AreEqual(expected, DateTimeHelper.GetString(unixTime));
    }

    [TestMethod]
    public void GetStringTestNull()
    {
        Assert.AreEqual(DateTimeHelper.InvalidFormat, DateTimeHelper.GetString(null));
    }
}
