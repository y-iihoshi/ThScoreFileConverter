using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverterTests.Extensions;

namespace ThScoreFileConverterTests.Models.Th08
{
    [TestClass]
    public class PlayCountTests
    {
        internal static Mock<IPlayCount> MockInitialPlayCount()
        {
            var mock = new Mock<IPlayCount>();
            _ = mock.SetupGet(m => m.Trials).Returns(ImmutableDictionary<Chara, int>.Empty);
            return mock;
        }

        internal static Mock<IPlayCount> MockPlayCount()
        {
            var mock = new Mock<IPlayCount>();
            _ = mock.SetupGet(m => m.TotalTrial).Returns(1);
            _ = mock.SetupGet(m => m.Trials).Returns(
                Utils.GetEnumerable<Chara>().Select((chara, index) => (chara, index)).ToDictionary());
            _ = mock.SetupGet(m => m.TotalClear).Returns(3);
            _ = mock.SetupGet(m => m.TotalContinue).Returns(4);
            _ = mock.SetupGet(m => m.TotalPractice).Returns(5);
            return mock;
        }

        internal static byte[] MakeByteArray(IPlayCount playCount)
            => TestUtils.MakeByteArray(
                playCount.TotalTrial,
                playCount.Trials.Values.ToArray(),
                0u,
                playCount.TotalClear,
                playCount.TotalContinue,
                playCount.TotalPractice);

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

            Validate(mock.Object, playCount);
        }

        [TestMethod]
        public void ReadFromTest()
        {
            var mock = MockPlayCount();

            var playCount = TestUtils.Create<PlayCount>(MakeByteArray(mock.Object));

            Validate(mock.Object, playCount);
        }

        [TestMethod]
        public void ReadFromTestNull()
        {
            var playCount = new PlayCount();
            _ = Assert.ThrowsException<ArgumentNullException>(() => playCount.ReadFrom(null!));
        }

        [TestMethod]
        public void ReadFromTestShortenedTrials()
        {
            var mock = MockPlayCount();
            var trials = mock.Object.Trials;
            _ = mock.SetupGet(m => m.Trials).Returns(trials.Where(pair => pair.Key != Chara.Yuyuko).ToDictionary());

            _ = Assert.ThrowsException<EndOfStreamException>(
                () => _ = TestUtils.Create<PlayCount>(MakeByteArray(mock.Object)));
        }

        [TestMethod]
        public void ReadFromTestExceededTrials()
        {
            var mock = MockPlayCount();
            var trials = mock.Object.Trials;
            _ = mock.SetupGet(m => m.Trials).Returns(
                trials.Concat(new[] { (TestUtils.Cast<Chara>(99), 99) }.ToDictionary()).ToDictionary());

            var playCount = TestUtils.Create<PlayCount>(MakeByteArray(mock.Object));

            Assert.AreEqual(mock.Object.TotalTrial, playCount.TotalTrial);
            CollectionAssert.That.AreNotEqual(mock.Object.Trials.Values, playCount.Trials.Values);
            CollectionAssert.That.AreEqual(mock.Object.Trials.Values.SkipLast(1), playCount.Trials.Values);
            Assert.AreNotEqual(mock.Object.TotalClear, playCount.TotalClear);
            Assert.AreNotEqual(mock.Object.TotalContinue, playCount.TotalContinue);
            Assert.AreNotEqual(mock.Object.TotalPractice, playCount.TotalPractice);
        }
    }
}
