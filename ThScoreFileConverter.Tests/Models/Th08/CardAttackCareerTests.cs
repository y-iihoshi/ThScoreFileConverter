using System.Collections.Immutable;
using System.IO;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th08;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th08;

[TestClass]
public class CardAttackCareerTests
{
    internal static ICardAttackCareer MockInitialCardAttackCareer()
    {
        var mock = Substitute.For<ICardAttackCareer>();
        _ = mock.MaxBonuses.Returns(ImmutableDictionary<CharaWithTotal, uint>.Empty);
        _ = mock.TrialCounts.Returns(ImmutableDictionary<CharaWithTotal, int>.Empty);
        _ = mock.ClearCounts.Returns(ImmutableDictionary<CharaWithTotal, int>.Empty);
        return mock;
    }

    internal static ICardAttackCareer MockCardAttackCareer()
    {
        var pairs = EnumHelper<CharaWithTotal>.Enumerable.Select((chara, index) => (chara, index)).ToArray();
        var mock = Substitute.For<ICardAttackCareer>();
        _ = mock.MaxBonuses.Returns(pairs.ToDictionary(pair => pair.chara, pair => (uint)pair.index));
        _ = mock.TrialCounts.Returns(pairs.ToDictionary(pair => pair.chara, pair => 20 + pair.index));
        _ = mock.ClearCounts.Returns(pairs.ToDictionary(pair => pair.chara, pair => 20 - pair.index));
        return mock;
    }

    internal static byte[] MakeByteArray(ICardAttackCareer career)
    {
        return TestUtils.MakeByteArray(
            career.MaxBonuses.Values, career.TrialCounts.Values, career.ClearCounts.Values);
    }

    internal static void Validate(ICardAttackCareer expected, ICardAttackCareer actual)
    {
        CollectionAssert.That.AreEqual(expected.MaxBonuses.Values, actual.MaxBonuses.Values);
        CollectionAssert.That.AreEqual(expected.TrialCounts.Values, actual.TrialCounts.Values);
        CollectionAssert.That.AreEqual(expected.ClearCounts.Values, actual.ClearCounts.Values);
    }

    [TestMethod]
    public void CardAttackCareerTest()
    {
        var mock = MockInitialCardAttackCareer();

        var career = new CardAttackCareer();

        Validate(mock, career);
    }

    [TestMethod]
    public void ReadFromTest()
    {
        var mock = MockCardAttackCareer();

        var career = TestUtils.Create<CardAttackCareer>(MakeByteArray(mock));

        Validate(mock, career);
    }

    [TestMethod]
    public void ReadFromTestShortenedMaxBonuses()
    {
        var mock = MockCardAttackCareer();
        var maxBonuses = mock.MaxBonuses;
        _ = mock.MaxBonuses.Returns(maxBonuses.Where(pair => pair.Key != CharaWithTotal.Total).ToDictionary());

        _ = Assert.ThrowsException<EndOfStreamException>(
            () => TestUtils.Create<CardAttackCareer>(MakeByteArray(mock)));
    }

    [TestMethod]
    public void ReadFromTestExceededMaxBonuses()
    {
        var mock = MockCardAttackCareer();
        var maxBonuses = mock.MaxBonuses;
        _ = mock.MaxBonuses.Returns(maxBonuses.Concat(new[] { ((CharaWithTotal)999, 999u) }.ToDictionary()).ToDictionary());

        var career = TestUtils.Create<CardAttackCareer>(MakeByteArray(mock));

        CollectionAssert.That.AreNotEqual(mock.MaxBonuses.Values, career.MaxBonuses.Values);
        CollectionAssert.That.AreEqual(mock.MaxBonuses.Values.SkipLast(1), career.MaxBonuses.Values);
        CollectionAssert.That.AreNotEqual(mock.TrialCounts.Values, career.TrialCounts.Values);
        CollectionAssert.That.AreNotEqual(mock.ClearCounts.Values, career.ClearCounts.Values);
    }

    [TestMethod]
    public void ReadFromTestShortenedTrialCounts()
    {
        var mock = MockCardAttackCareer();
        var trialCounts = mock.TrialCounts;
        _ = mock.TrialCounts.Returns(trialCounts.Where(pair => pair.Key != CharaWithTotal.Total).ToDictionary());

        _ = Assert.ThrowsException<EndOfStreamException>(
            () => TestUtils.Create<CardAttackCareer>(MakeByteArray(mock)));
    }

    [TestMethod]
    public void ReadFromTestExceededTrialCounts()
    {
        var mock = MockCardAttackCareer();
        var trialCounts = mock.TrialCounts;
        _ = mock.TrialCounts.Returns(trialCounts.Concat(new[] { ((CharaWithTotal)999, 999) }.ToDictionary()).ToDictionary());

        var career = TestUtils.Create<CardAttackCareer>(MakeByteArray(mock));

        CollectionAssert.That.AreEqual(mock.MaxBonuses.Values, career.MaxBonuses.Values);
        CollectionAssert.That.AreNotEqual(mock.TrialCounts.Values, career.TrialCounts.Values);
        CollectionAssert.That.AreEqual(mock.TrialCounts.Values.SkipLast(1), career.TrialCounts.Values);
        CollectionAssert.That.AreNotEqual(mock.ClearCounts.Values, career.ClearCounts.Values);
    }

    [TestMethod]
    public void ReadFromTestShortenedClearCounts()
    {
        var mock = MockCardAttackCareer();
        var clearCounts = mock.ClearCounts;
        _ = mock.ClearCounts.Returns(clearCounts.Where(pair => pair.Key != CharaWithTotal.Total).ToDictionary());

        _ = Assert.ThrowsException<EndOfStreamException>(
            () => TestUtils.Create<CardAttackCareer>(MakeByteArray(mock)));
    }

    [TestMethod]
    public void ReadFromTestExceededClearCounts()
    {
        var mock = MockCardAttackCareer();
        var clearCounts = mock.ClearCounts;
        _ = mock.ClearCounts.Returns(clearCounts.Concat(new[] { ((CharaWithTotal)999, 999) }.ToDictionary()).ToDictionary());

        var career = TestUtils.Create<CardAttackCareer>(MakeByteArray(mock));

        CollectionAssert.That.AreEqual(mock.MaxBonuses.Values, career.MaxBonuses.Values);
        CollectionAssert.That.AreEqual(mock.TrialCounts.Values, career.TrialCounts.Values);
        CollectionAssert.That.AreNotEqual(mock.ClearCounts.Values, career.ClearCounts.Values);
        CollectionAssert.That.AreEqual(mock.ClearCounts.Values.SkipLast(1), career.ClearCounts.Values);
    }
}
