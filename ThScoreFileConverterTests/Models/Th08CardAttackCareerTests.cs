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
    public class Th08CardAttackCareerTests
    {
        internal static CardAttackCareerStub ValidStub => new CardAttackCareerStub()
        {
            MaxBonuses = Utils.GetEnumerator<Th08Converter.CharaWithTotal>()
                .Select((chara, index) => (chara, index))
                .ToDictionary(pair => pair.chara, pair => (uint)pair.index),
            TrialCounts = Utils.GetEnumerator<Th08Converter.CharaWithTotal>()
                .Select((chara, index) => (chara, index))
                .ToDictionary(pair => pair.chara, pair => 20 + pair.index),
            ClearCounts = Utils.GetEnumerator<Th08Converter.CharaWithTotal>()
                .Select((chara, index) => (chara, index))
                .ToDictionary(pair => pair.chara, pair => 20 - pair.index)
        };

        internal static byte[] MakeByteArray(ICardAttackCareer career)
            => TestUtils.MakeByteArray(
                career.MaxBonuses.Values.ToArray(),
                career.TrialCounts.Values.ToArray(),
                career.ClearCounts.Values.ToArray());

        internal static void Validate(ICardAttackCareer expected, in Th08CardAttackCareerWrapper actual)
        {
            CollectionAssert.That.AreEqual(expected.MaxBonuses.Values, actual.MaxBonuses.Values);
            CollectionAssert.That.AreEqual(expected.TrialCounts.Values, actual.TrialCounts.Values);
            CollectionAssert.That.AreEqual(expected.ClearCounts.Values, actual.ClearCounts.Values);
        }

        [TestMethod]
        public void Th08CardAttackCareerReadFromTest() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var career = Th08CardAttackCareerWrapper.Create(MakeByteArray(stub));

            Validate(stub, career);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08CardAttackCareerReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var career = new Th08CardAttackCareerWrapper();
            career.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th08CardAttackCareerReadFromTestShortenedMaxBonuses() => TestUtils.Wrap(() =>
        {
            var stub = new CardAttackCareerStub(ValidStub);
            stub.MaxBonuses = stub.MaxBonuses.Where(pair => pair.Key != Th08Converter.CharaWithTotal.Total)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            _ = Th08CardAttackCareerWrapper.Create(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void Th08CardAttackCareerReadFromTestExceededMaxBonuses() => TestUtils.Wrap(() =>
        {
            var stub = new CardAttackCareerStub(ValidStub);
            stub.MaxBonuses = stub.MaxBonuses.Concat(new Dictionary<Th08Converter.CharaWithTotal, uint>
            {
                { TestUtils.Cast<Th08Converter.CharaWithTotal>(999), 999u },
            }).ToDictionary(pair => pair.Key, pair => pair.Value);

            var career = Th08CardAttackCareerWrapper.Create(MakeByteArray(stub));

            CollectionAssert.That.AreNotEqual(stub.MaxBonuses.Values, career.MaxBonuses.Values);
            CollectionAssert.That.AreEqual(stub.MaxBonuses.Values.SkipLast(1), career.MaxBonuses.Values);
            CollectionAssert.That.AreNotEqual(stub.TrialCounts.Values, career.TrialCounts.Values);
            CollectionAssert.That.AreNotEqual(stub.ClearCounts.Values, career.ClearCounts.Values);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th08CardAttackCareerReadFromTestShortenedTrialCounts() => TestUtils.Wrap(() =>
        {
            var stub = new CardAttackCareerStub(ValidStub);
            stub.TrialCounts = stub.TrialCounts.Where(pair => pair.Key != Th08Converter.CharaWithTotal.Total)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            _ = Th08CardAttackCareerWrapper.Create(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void Th08CardAttackCareerReadFromTestExceededTrialCounts() => TestUtils.Wrap(() =>
        {
            var stub = new CardAttackCareerStub(ValidStub);
            stub.TrialCounts = stub.TrialCounts.Concat(new Dictionary<Th08Converter.CharaWithTotal, int>
            {
                { TestUtils.Cast<Th08Converter.CharaWithTotal>(999), 999 },
            }).ToDictionary(pair => pair.Key, pair => pair.Value);

            var career = Th08CardAttackCareerWrapper.Create(MakeByteArray(stub));

            CollectionAssert.That.AreEqual(stub.MaxBonuses.Values, career.MaxBonuses.Values);
            CollectionAssert.That.AreNotEqual(stub.TrialCounts.Values, career.TrialCounts.Values);
            CollectionAssert.That.AreEqual(stub.TrialCounts.Values.SkipLast(1), career.TrialCounts.Values);
            CollectionAssert.That.AreNotEqual(stub.ClearCounts.Values, career.ClearCounts.Values);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th08CardAttackCareerReadFromTestShortenedClearCounts() => TestUtils.Wrap(() =>
        {
            var stub = new CardAttackCareerStub(ValidStub);
            stub.ClearCounts = stub.ClearCounts.Where(pair => pair.Key != Th08Converter.CharaWithTotal.Total)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            _ = Th08CardAttackCareerWrapper.Create(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void Th08CardAttackCareerReadFromTestExceededClearCounts() => TestUtils.Wrap(() =>
        {
            var stub = new CardAttackCareerStub(ValidStub);
            stub.ClearCounts = stub.ClearCounts.Concat(new Dictionary<Th08Converter.CharaWithTotal, int>
            {
                { TestUtils.Cast<Th08Converter.CharaWithTotal>(999), 999 },
            }).ToDictionary(pair => pair.Key, pair => pair.Value);

            var career = Th08CardAttackCareerWrapper.Create(MakeByteArray(stub));

            CollectionAssert.That.AreEqual(stub.MaxBonuses.Values, career.MaxBonuses.Values);
            CollectionAssert.That.AreEqual(stub.TrialCounts.Values, career.TrialCounts.Values);
            CollectionAssert.That.AreNotEqual(stub.ClearCounts.Values, career.ClearCounts.Values);
            CollectionAssert.That.AreEqual(stub.ClearCounts.Values.SkipLast(1), career.ClearCounts.Values);
        });
    }
}
