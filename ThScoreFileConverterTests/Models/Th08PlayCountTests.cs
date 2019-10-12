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
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th08PlayCountTests
    {
        internal static PlayCountStub ValidStub => new PlayCountStub()
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

        internal static void Validate(IPlayCount expected, in Th08PlayCountWrapper actual)
        {
            Assert.AreEqual(expected.TotalTrial, actual.TotalTrial.Value);
            CollectionAssert.That.AreEqual(expected.Trials.Values, actual.Trials.Values);
            Assert.AreEqual(expected.TotalClear, actual.TotalClear.Value);
            Assert.AreEqual(expected.TotalContinue, actual.TotalContinue.Value);
            Assert.AreEqual(expected.TotalPractice, actual.TotalPractice.Value);
        }

        [TestMethod]
        public void Th08PlayCountReadFromTest() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var playCount = Th08PlayCountWrapper.Create(MakeByteArray(stub));

            Validate(stub, playCount);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08PlayCountReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var playCount = new Th08PlayCountWrapper();
            playCount.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th08PlayCountReadFromTestShortenedTrials() => TestUtils.Wrap(() =>
        {
            var stub = new PlayCountStub(ValidStub);
            stub.Trials = stub.Trials.Where(pair => pair.Key != Chara.Yuyuko)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            _ = Th08PlayCountWrapper.Create(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void Th08PlayCountReadFromTestExceededTrials() => TestUtils.Wrap(() =>
        {
            var stub = new PlayCountStub(ValidStub);
            stub.Trials = stub.Trials.Concat(new Dictionary<Chara, int>
            {
                { TestUtils.Cast<Chara>(99), 99 },
            }).ToDictionary(pair => pair.Key, pair => pair.Value);

            var playCount = Th08PlayCountWrapper.Create(MakeByteArray(stub));

            Assert.AreEqual(stub.TotalTrial, playCount.TotalTrial.Value);
            CollectionAssert.That.AreNotEqual(stub.Trials.Values, playCount.Trials.Values);
            CollectionAssert.That.AreEqual(stub.Trials.Values.SkipLast(1), playCount.Trials.Values);
            Assert.AreNotEqual(stub.TotalClear, playCount.TotalClear.Value);
            Assert.AreNotEqual(stub.TotalContinue, playCount.TotalContinue.Value);
            Assert.AreNotEqual(stub.TotalPractice, playCount.TotalPractice.Value);
        });
    }
}
