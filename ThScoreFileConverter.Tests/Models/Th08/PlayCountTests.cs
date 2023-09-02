using System.Collections.Immutable;
using System.IO;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th08;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th08;

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

    internal static void Validate(IPlayCount expected, IPlayCount actual)
    {
        Assert.AreEqual(expected.TotalTrial, actual.TotalTrial);
        CollectionAssert.That.AreEqual(expected.Trials.Values, actual.Trials.Values);
        Assert.AreEqual(expected.TotalClear, actual.TotalClear);
        Assert.AreEqual(expected.TotalContinue, actual.TotalContinue);
        Assert.AreEqual(expected.TotalPractice, actual.TotalPractice);
    }

    [TestMethod]
    public void PlayCountTest()
    {
        var mock = MockInitialPlayCount();

        var playCount = new PlayCount();

        Validate(mock, playCount);
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var mock = MockPlayCount();

        var playCount = TestUtils.Create<PlayCount>(MakeByteArray(mock));

        Validate(mock, playCount);
    }

    [TestMethod]
    public void ReadFromTestShortenedTrials()
    {
        var mock = MockPlayCount();
        var trials = mock.Trials;
        _ = mock.Trials.Returns(trials.Where(pair => pair.Key != Chara.Yuyuko).ToDictionary());

        _ = Assert.ThrowsException<EndOfStreamException>(
            () => TestUtils.Create<PlayCount>(MakeByteArray(mock)));
    }

    [TestMethod]
    public void ReadFromTestExceededTrials()
    {
        var mock = MockPlayCount();
        var trials = mock.Trials;
        _ = mock.Trials.Returns(trials.Concat(new[] { ((Chara)99, 99) }.ToDictionary()).ToDictionary());

        var playCount = TestUtils.Create<PlayCount>(MakeByteArray(mock));

        Assert.AreEqual(mock.TotalTrial, playCount.TotalTrial);
        CollectionAssert.That.AreNotEqual(mock.Trials.Values, playCount.Trials.Values);
        CollectionAssert.That.AreEqual(mock.Trials.Values.SkipLast(1), playCount.Trials.Values);
        Assert.AreNotEqual(mock.TotalClear, playCount.TotalClear);
        Assert.AreNotEqual(mock.TotalContinue, playCount.TotalContinue);
        Assert.AreNotEqual(mock.TotalPractice, playCount.TotalPractice);
    }
}
