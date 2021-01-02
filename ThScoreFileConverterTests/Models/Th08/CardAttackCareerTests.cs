using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverterTests.Extensions;

namespace ThScoreFileConverterTests.Models.Th08
{
    [TestClass]
    public class CardAttackCareerTests
    {
        internal static Mock<ICardAttackCareer> MockInitialCardAttackCareer()
        {
            var mock = new Mock<ICardAttackCareer>();
            _ = mock.SetupGet(m => m.MaxBonuses).Returns(ImmutableDictionary<CharaWithTotal, uint>.Empty);
            _ = mock.SetupGet(m => m.TrialCounts).Returns(ImmutableDictionary<CharaWithTotal, int>.Empty);
            _ = mock.SetupGet(m => m.ClearCounts).Returns(ImmutableDictionary<CharaWithTotal, int>.Empty);
            return mock;
        }

        internal static Mock<ICardAttackCareer> MockCardAttackCareer()
        {
            var pairs = EnumHelper<CharaWithTotal>.Enumerable.Select((chara, index) => (chara, index));
            var mock = new Mock<ICardAttackCareer>();
            _ = mock.SetupGet(m => m.MaxBonuses).Returns(
                pairs.ToDictionary(pair => pair.chara, pair => (uint)pair.index));
            _ = mock.SetupGet(m => m.TrialCounts).Returns(
                pairs.ToDictionary(pair => pair.chara, pair => 20 + pair.index));
            _ = mock.SetupGet(m => m.ClearCounts).Returns(
                pairs.ToDictionary(pair => pair.chara, pair => 20 - pair.index));
            return mock;
        }

        internal static byte[] MakeByteArray(ICardAttackCareer career)
            => TestUtils.MakeByteArray(
                career.MaxBonuses.Values.ToArray(),
                career.TrialCounts.Values.ToArray(),
                career.ClearCounts.Values.ToArray());

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

            Validate(mock.Object, career);
        }

        [TestMethod]
        public void ReadFromTest()
        {
            var mock = MockCardAttackCareer();

            var career = TestUtils.Create<CardAttackCareer>(MakeByteArray(mock.Object));

            Validate(mock.Object, career);
        }

        [TestMethod]
        public void ReadFromTestShortenedMaxBonuses()
        {
            var mock = MockCardAttackCareer();
            var maxBonuses = mock.Object.MaxBonuses;
            _ = mock.SetupGet(m => m.MaxBonuses).Returns(
                maxBonuses.Where(pair => pair.Key != CharaWithTotal.Total).ToDictionary());

            _ = Assert.ThrowsException<EndOfStreamException>(
                () => _ = TestUtils.Create<CardAttackCareer>(MakeByteArray(mock.Object)));
        }

        [TestMethod]
        public void ReadFromTestExceededMaxBonuses()
        {
            var mock = MockCardAttackCareer();
            var maxBonuses = mock.Object.MaxBonuses;
            _ = mock.SetupGet(m => m.MaxBonuses).Returns(
                maxBonuses.Concat(new[] { (TestUtils.Cast<CharaWithTotal>(999), 999u) }.ToDictionary()).ToDictionary());

            var career = TestUtils.Create<CardAttackCareer>(MakeByteArray(mock.Object));

            CollectionAssert.That.AreNotEqual(mock.Object.MaxBonuses.Values, career.MaxBonuses.Values);
            CollectionAssert.That.AreEqual(mock.Object.MaxBonuses.Values.SkipLast(1), career.MaxBonuses.Values);
            CollectionAssert.That.AreNotEqual(mock.Object.TrialCounts.Values, career.TrialCounts.Values);
            CollectionAssert.That.AreNotEqual(mock.Object.ClearCounts.Values, career.ClearCounts.Values);
        }

        [TestMethod]
        public void ReadFromTestShortenedTrialCounts()
        {
            var mock = MockCardAttackCareer();
            var trialCounts = mock.Object.TrialCounts;
            _ = mock.SetupGet(m => m.TrialCounts).Returns(
                trialCounts.Where(pair => pair.Key != CharaWithTotal.Total).ToDictionary());

            _ = Assert.ThrowsException<EndOfStreamException>(
                () => _ = TestUtils.Create<CardAttackCareer>(MakeByteArray(mock.Object)));
        }

        [TestMethod]
        public void ReadFromTestExceededTrialCounts()
        {
            var mock = MockCardAttackCareer();
            var trialCounts = mock.Object.TrialCounts;
            _ = mock.SetupGet(m => m.TrialCounts).Returns(
                trialCounts.Concat(new[] { (TestUtils.Cast<CharaWithTotal>(999), 999) }.ToDictionary()).ToDictionary());

            var career = TestUtils.Create<CardAttackCareer>(MakeByteArray(mock.Object));

            CollectionAssert.That.AreEqual(mock.Object.MaxBonuses.Values, career.MaxBonuses.Values);
            CollectionAssert.That.AreNotEqual(mock.Object.TrialCounts.Values, career.TrialCounts.Values);
            CollectionAssert.That.AreEqual(mock.Object.TrialCounts.Values.SkipLast(1), career.TrialCounts.Values);
            CollectionAssert.That.AreNotEqual(mock.Object.ClearCounts.Values, career.ClearCounts.Values);
        }

        [TestMethod]
        public void ReadFromTestShortenedClearCounts()
        {
            var mock = MockCardAttackCareer();
            var clearCounts = mock.Object.ClearCounts;
            _ = mock.SetupGet(m => m.ClearCounts).Returns(
                clearCounts.Where(pair => pair.Key != CharaWithTotal.Total).ToDictionary());

            _ = Assert.ThrowsException<EndOfStreamException>(
                () => _ = TestUtils.Create<CardAttackCareer>(MakeByteArray(mock.Object)));
        }

        [TestMethod]
        public void ReadFromTestExceededClearCounts()
        {
            var mock = MockCardAttackCareer();
            var clearCounts = mock.Object.ClearCounts;
            _ = mock.SetupGet(m => m.ClearCounts).Returns(
                clearCounts.Concat(new[] { (TestUtils.Cast<CharaWithTotal>(999), 999) }.ToDictionary()).ToDictionary());

            var career = TestUtils.Create<CardAttackCareer>(MakeByteArray(mock.Object));

            CollectionAssert.That.AreEqual(mock.Object.MaxBonuses.Values, career.MaxBonuses.Values);
            CollectionAssert.That.AreEqual(mock.Object.TrialCounts.Values, career.TrialCounts.Values);
            CollectionAssert.That.AreNotEqual(mock.Object.ClearCounts.Values, career.ClearCounts.Values);
            CollectionAssert.That.AreEqual(mock.Object.ClearCounts.Values.SkipLast(1), career.ClearCounts.Values);
        }
    }
}
