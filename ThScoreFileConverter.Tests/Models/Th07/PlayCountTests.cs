using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th07;
using ThScoreFileConverter.Models.Th07;

namespace ThScoreFileConverter.Tests.Models.Th07;

internal static class PlayCountExtensions
{
    internal static void ShouldBe(this PlayCount actual, PlayCountTests.Properties expected)
    {
        actual.TotalTrial.ShouldBe(expected.totalTrial);
        actual.Trials.Values.ShouldBe(expected.trials.Values);
        actual.TotalRetry.ShouldBe(expected.totalRetry);
        actual.TotalClear.ShouldBe(expected.totalClear);
        actual.TotalContinue.ShouldBe(expected.totalContinue);
        actual.TotalPractice.ShouldBe(expected.totalPractice);
    }
}

[TestClass]
public class PlayCountTests
{
    internal struct Properties(in Properties properties)
    {
        public int totalTrial = properties.totalTrial;
        public Dictionary<Chara, int> trials = new(properties.trials);
        public int totalRetry = properties.totalRetry;
        public int totalClear = properties.totalClear;
        public int totalContinue = properties.totalContinue;
        public int totalPractice = properties.totalPractice;
    }

    internal static Properties ValidProperties { get; } = new Properties()
    {
        totalTrial = 1,
        trials = EnumHelper<Chara>.Enumerable.Select((chara, index) => (chara, index)).ToDictionary(),
        totalRetry = 2,
        totalClear = 3,
        totalContinue = 4,
        totalPractice = 5,
    };

    internal static byte[] MakeByteArray(in Properties properties)
    {
        return TestUtils.MakeByteArray(
            properties.totalTrial,
            properties.trials.Values,
            properties.totalRetry,
            properties.totalClear,
            properties.totalContinue,
            properties.totalPractice);
    }

    [TestMethod]
    public void PlayCountTest()
    {
        var playCount = new PlayCount();

        playCount.TotalTrial.ShouldBe(default);
        playCount.Trials.ShouldBeEmpty();
        playCount.TotalRetry.ShouldBe(default);
        playCount.TotalClear.ShouldBe(default);
        playCount.TotalContinue.ShouldBe(default);
        playCount.TotalPractice.ShouldBe(default);
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var properties = ValidProperties;

        var playCount = TestUtils.Create<PlayCount>(MakeByteArray(properties));

        playCount.ShouldBe(properties);
    }

    [TestMethod]
    public void ReadFromTestShortenedTrials()
    {
        var properties = new Properties(ValidProperties);
        _ = properties.trials.Remove(Chara.SakuyaB);

        _ = Should.Throw<EndOfStreamException>(
            () => TestUtils.Create<PlayCount>(MakeByteArray(properties)));
    }

    [TestMethod]
    public void ReadFromTestExceededTrials()
    {
        var properties = new Properties(ValidProperties);
        properties.trials.Add((Chara)99, 99);

        var playCount = TestUtils.Create<PlayCount>(MakeByteArray(properties));

        playCount.TotalTrial.ShouldBe(properties.totalTrial);
        playCount.Trials.Values.ShouldNotBe(properties.trials.Values);
        playCount.Trials.Values.ShouldBe(properties.trials.Values.SkipLast(1));
        playCount.TotalRetry.ShouldNotBe(properties.totalRetry);
        playCount.TotalClear.ShouldNotBe(properties.totalClear);
        playCount.TotalContinue.ShouldNotBe(properties.totalContinue);
        playCount.TotalPractice.ShouldNotBe(properties.totalPractice);
    }
}
