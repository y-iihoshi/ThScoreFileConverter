using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Models.Th09;

namespace ThScoreFileConverter.Tests.Models.Th09;

internal static class ClearCountExtensions
{
    internal static void ShouldBe(this IClearCount actual, IClearCount expected)
    {
        actual.Counts.Values.ShouldBe(expected.Counts.Values);
    }
}

[TestClass]
public class ClearCountTests
{
    internal static IClearCount MockInitialClearCount()
    {
        var mock = Substitute.For<IClearCount>();
        _ = mock.Counts.Returns(ImmutableDictionary<Level, int>.Empty);
        return mock;
    }

    internal static IClearCount MockClearCount()
    {
        var mock = Substitute.For<IClearCount>();
        _ = mock.Counts.Returns(EnumHelper<Level>.Enumerable.Select((level, index) => (level, index)).ToDictionary());
        return mock;
    }

    internal static byte[] MakeByteArray(IClearCount clearCount)
    {
        return TestUtils.MakeByteArray(clearCount.Counts.Values, 0u);
    }

    [TestMethod]
    public void ClearCountTest()
    {
        var mock = MockInitialClearCount();

        var clearCount = new ClearCount();

        clearCount.ShouldBe(mock);
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var mock = MockClearCount();

        var clearCount = TestUtils.Create<ClearCount>(MakeByteArray(mock));

        clearCount.ShouldBe(mock);
    }

    [TestMethod]
    public void ReadFromTestShortenedTrials()
    {
        var mock = MockClearCount();
        var counts = mock.Counts;
        _ = mock.Counts.Returns(counts.Where(pair => pair.Key == Level.Extra).ToDictionary());

        _ = Should.Throw<EndOfStreamException>(() => TestUtils.Create<ClearCount>(MakeByteArray(mock)));
    }

    [TestMethod]
    public void ReadFromTestExceededTrials()
    {
        var mock = MockClearCount();
        var counts = mock.Counts;
        _ = mock.Counts.Returns(counts.Concat(new[] { ((Level)99, 99) }.ToDictionary()).ToDictionary());

        var clearCount = TestUtils.Create<ClearCount>(MakeByteArray(mock));

        clearCount.Counts.Values.ShouldNotBe(mock.Counts.Values);
        clearCount.Counts.Values.ShouldBe(mock.Counts.Values.SkipLast(1));
    }
}
