using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th08;
using ThScoreFileConverter.Models.Th08;

namespace ThScoreFileConverter.Tests.Models.Th08;

internal static class PlayCountExtensions
{
    internal static void ShouldBe(this IPlayCount actual, IPlayCount expected)
    {
        actual.TotalTrial.ShouldBe(expected.TotalTrial);
        actual.Trials.Values.ShouldBe(expected.Trials.Values);
        actual.TotalClear.ShouldBe(expected.TotalClear);
        actual.TotalContinue.ShouldBe(expected.TotalContinue);
        actual.TotalPractice.ShouldBe(expected.TotalPractice);
    }
}

[TestClass]
public class PlayCountTests
{
    internal static IPlayCount MockInitialPlayCount()
    {
        var mock = Substitute.For<IPlayCount>();
        _ = mock.Trials.Returns(ImmutableDictionary<Chara, int>.Empty);
        return mock;
    }

    internal static IPlayCount MockPlayCount()
    {
        var mock = Substitute.For<IPlayCount>();
        _ = mock.TotalTrial.Returns(1);
        _ = mock.Trials.Returns(EnumHelper<Chara>.Enumerable.Select((chara, index) => (chara, index)).ToDictionary());
        _ = mock.TotalClear.Returns(3);
        _ = mock.TotalContinue.Returns(4);
        _ = mock.TotalPractice.Returns(5);
        return mock;
    }

    internal static byte[] MakeByteArray(IPlayCount playCount)
    {
        return TestUtils.MakeByteArray(
            playCount.TotalTrial,
            playCount.Trials.Values,
            0u,
            playCount.TotalClear,
            playCount.TotalContinue,
            playCount.TotalPractice);
    }

    [TestMethod]
    public void PlayCountTest()
    {
        var mock = MockInitialPlayCount();

        var playCount = new PlayCount();

        playCount.ShouldBe(mock);
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var mock = MockPlayCount();

        var playCount = TestUtils.Create<PlayCount>(MakeByteArray(mock));

        playCount.ShouldBe(mock);
    }

    [TestMethod]
    public void ReadFromTestShortenedTrials()
    {
        var mock = MockPlayCount();
        var trials = mock.Trials;
        _ = mock.Trials.Returns(trials.Where(pair => pair.Key != Chara.Yuyuko).ToDictionary());

        _ = Should.Throw<EndOfStreamException>(
            () => TestUtils.Create<PlayCount>(MakeByteArray(mock)));
    }

    [TestMethod]
    public void ReadFromTestExceededTrials()
    {
        var mock = MockPlayCount();
        var trials = mock.Trials;
        _ = mock.Trials.Returns(trials.Concat(new[] { ((Chara)99, 99) }.ToDictionary()).ToDictionary());

        var playCount = TestUtils.Create<PlayCount>(MakeByteArray(mock));

        playCount.TotalTrial.ShouldBe(mock.TotalTrial);
        playCount.Trials.Values.ShouldNotBe(mock.Trials.Values);
        playCount.Trials.Values.ShouldBe(mock.Trials.Values.SkipLast(1));
        playCount.TotalClear.ShouldNotBe(mock.TotalClear);
        playCount.TotalContinue.ShouldNotBe(mock.TotalContinue);
        playCount.TotalPractice.ShouldNotBe(mock.TotalPractice);
    }
}
