using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th075;
using ThScoreFileConverter.Models.Th075;

namespace ThScoreFileConverter.Tests.Models.Th075;

internal static class AllScoreDataExtensions
{
    internal static void ShouldBe(this AllScoreData actual, AllScoreDataTests.Properties expected)
    {
        foreach (var pair in expected.clearData)
        {
            actual.ClearData[pair.Key].ShouldBe(pair.Value);
        }

        actual.Status.ShouldNotBeNull().ShouldBe(expected.status);
    }
}

[TestClass]
public class AllScoreDataTests
{
    internal struct Properties
    {
        public Dictionary<(CharaWithReserved, Level), IClearData> clearData;
        public StatusTests.Properties status;
    }

    internal static Properties ValidProperties { get; } = new Properties()
    {
        clearData = EnumHelper.Cartesian<CharaWithReserved, Level>()
            .ToDictionary(pair => pair, _ => ClearDataTests.MockClearData()),
        status = StatusTests.ValidProperties,
    };

    internal static byte[] MakeByteArray(in Properties properties)
    {
        return TestUtils.MakeByteArray(
            properties.clearData.Select(pair => ClearDataTests.MakeByteArray(pair.Value)),
            StatusTests.MakeByteArray(properties.status));
    }

    [TestMethod]
    public void AllScoreDataTest()
    {
        var allScoreData = new AllScoreData();

        allScoreData.ClearData.ShouldBeEmpty();
        allScoreData.Status.ShouldBeNull();
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var properties = ValidProperties;

        var allScoreData = TestUtils.Create<AllScoreData>(MakeByteArray(properties));

        allScoreData.ShouldBe(properties);
    }
}
