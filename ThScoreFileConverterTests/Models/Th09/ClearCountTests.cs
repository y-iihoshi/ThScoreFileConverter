using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th09;
using ThScoreFileConverterTests.Extensions;

namespace ThScoreFileConverterTests.Models.Th09
{
    [TestClass]
    public class ClearCountTests
    {
        internal static Mock<IClearCount> MockInitialClearCount()
        {
            var mock = new Mock<IClearCount>();
            _ = mock.SetupGet(m => m.Counts).Returns(ImmutableDictionary<Level, int>.Empty);
            return mock;
        }

        internal static Mock<IClearCount> MockClearCount()
        {
            var mock = new Mock<IClearCount>();
            _ = mock.SetupGet(m => m.Counts).Returns(
                EnumHelper.GetEnumerable<Level>().Select((level, index) => (level, index)).ToDictionary());
            return mock;
        }

        internal static byte[] MakeByteArray(IClearCount clearCount)
            => TestUtils.MakeByteArray(clearCount.Counts.Values.ToArray(), 0u);

        internal static void Validate(IClearCount expected, IClearCount actual)
            => CollectionAssert.That.AreEqual(expected.Counts.Values, actual.Counts.Values);

        [TestMethod]
        public void ClearCountTest()
        {
            var mock = MockInitialClearCount();

            var clearCount = new ClearCount();

            Validate(mock.Object, clearCount);
        }

        [TestMethod]
        public void ReadFromTest()
        {
            var mock = MockClearCount();

            var clearCount = TestUtils.Create<ClearCount>(MakeByteArray(mock.Object));

            Validate(mock.Object, clearCount);
        }

        [TestMethod]
        public void ReadFromTestNull()
        {
            var clearCount = new ClearCount();
            _ = Assert.ThrowsException<ArgumentNullException>(() => clearCount.ReadFrom(null!));
        }

        [TestMethod]
        public void ReadFromTestShortenedTrials()
        {
            var mock = MockClearCount();
            var counts = mock.Object.Counts;
            _ = mock.SetupGet(m => m.Counts).Returns(counts.Where(pair => pair.Key == Level.Extra).ToDictionary());

            _ = Assert.ThrowsException<EndOfStreamException>(
                () => _ = TestUtils.Create<ClearCount>(MakeByteArray(mock.Object)));
        }

        [TestMethod]
        public void ReadFromTestExceededTrials()
        {
            var mock = MockClearCount();
            var counts = mock.Object.Counts;
            _ = mock.SetupGet(m => m.Counts).Returns(
                counts.Concat(new[] { (TestUtils.Cast<Level>(99), 99) }.ToDictionary()).ToDictionary());

            var clearCount = TestUtils.Create<ClearCount>(MakeByteArray(mock.Object));

            CollectionAssert.That.AreNotEqual(mock.Object.Counts.Values, clearCount.Counts.Values);
            CollectionAssert.That.AreEqual(mock.Object.Counts.Values.SkipLast(1), clearCount.Counts.Values);
        }
    }
}
