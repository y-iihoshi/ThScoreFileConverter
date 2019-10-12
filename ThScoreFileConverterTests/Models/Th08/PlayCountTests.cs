using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th08.Stubs;

namespace ThScoreFileConverterTests.Models.Th08
{
    [TestClass]
    public class PlayCountTests
    {
        internal static PlayCountStub ValidStub { get; } = new PlayCountStub()
        {
            TotalTrial = 1,
            Trials = Utils.GetEnumerator<Chara>()
                .Select((chara, index) => (chara, index))
                .ToDictionary(pair => pair.chara, pair => pair.index),
            TotalClear = 3,
            TotalContinue = 4,
            TotalPractice = 5,
        };

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
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var playCount = TestUtils.Create<PlayCount>(MakeByteArray(stub));

            Validate(stub, playCount);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var playCount = new PlayCount();
            playCount.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortenedTrials() => TestUtils.Wrap(() =>
        {
            var stub = new PlayCountStub(ValidStub);
            stub.Trials = stub.Trials.Where(pair => pair.Key != Chara.Yuyuko)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            _ = TestUtils.Create<PlayCount>(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestExceededTrials() => TestUtils.Wrap(() =>
        {
            var stub = new PlayCountStub(ValidStub);
            stub.Trials = stub.Trials.Concat(new Dictionary<Chara, int>
            {
                { TestUtils.Cast<Chara>(99), 99 },
            }).ToDictionary(pair => pair.Key, pair => pair.Value);

            var playCount = TestUtils.Create<PlayCount>(MakeByteArray(stub));

            Assert.AreEqual(stub.TotalTrial, playCount.TotalTrial);
            CollectionAssert.That.AreNotEqual(stub.Trials.Values, playCount.Trials.Values);
            CollectionAssert.That.AreEqual(stub.Trials.Values.SkipLast(1), playCount.Trials.Values);
            Assert.AreNotEqual(stub.TotalClear, playCount.TotalClear);
            Assert.AreNotEqual(stub.TotalContinue, playCount.TotalContinue);
            Assert.AreNotEqual(stub.TotalPractice, playCount.TotalPractice);
        });
    }
}
