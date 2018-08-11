using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th08CardAttackCareerTests
    {
        internal struct Properties
        {
            public Dictionary<Th08Converter.CharaWithTotal, uint> maxBonuses;
            public Dictionary<Th08Converter.CharaWithTotal, int> trialCounts;
            public Dictionary<Th08Converter.CharaWithTotal, int> clearCounts;

            public Properties(in Properties properties)
            {
                this.maxBonuses = new Dictionary<Th08Converter.CharaWithTotal, uint>(properties.maxBonuses);
                this.trialCounts = new Dictionary<Th08Converter.CharaWithTotal, int>(properties.trialCounts);
                this.clearCounts = new Dictionary<Th08Converter.CharaWithTotal, int>(properties.clearCounts);
            }
        };

        internal static Properties ValidProperties = new Properties()
        {
            maxBonuses = Utils.GetEnumerator<Th08Converter.CharaWithTotal>()
                .Select((chara, index) => new { chara, index })
                .ToDictionary(pair => pair.chara, pair => (uint)pair.index),
            trialCounts = Utils.GetEnumerator<Th08Converter.CharaWithTotal>()
                .Select((chara, index) => new { chara, index })
                .ToDictionary(pair => pair.chara, pair => (20 + pair.index)),
            clearCounts = Utils.GetEnumerator<Th08Converter.CharaWithTotal>()
                .Select((chara, index) => new { chara, index })
                .ToDictionary(pair => pair.chara, pair => (20 - pair.index))
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.maxBonuses.Values.ToArray(),
                properties.trialCounts.Values.ToArray(),
                properties.clearCounts.Values.ToArray());

        internal static void Validate(in Th08CardAttackCareerWrapper career, in Properties properties)
        {
            CollectionAssert.AreEqual(properties.maxBonuses.Values, career.MaxBonuses.Values.ToArray());
            CollectionAssert.AreEqual(properties.trialCounts.Values, career.TrialCounts.Values.ToArray());
            CollectionAssert.AreEqual(properties.clearCounts.Values, career.ClearCounts.Values.ToArray());
        }

        [TestMethod()]
        public void Th08CardAttackCareerTest() => TestUtils.Wrap(() =>
        {
            var career = new Th08CardAttackCareerWrapper();

            Assert.AreEqual(0, career.MaxBonuses.Count);
            Assert.AreEqual(0, career.TrialCounts.Count);
            Assert.AreEqual(0, career.ClearCounts.Count);
        });

        [TestMethod()]
        public void Th08CardAttackCareerReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var career = Th08CardAttackCareerWrapper.Create(MakeByteArray(properties));

            Validate(career, properties);
        });

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08CardAttackCareerReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var career = new Th08CardAttackCareerWrapper();
            career.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "career")]
        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th08CardAttackCareerReadFromTestShortenedMaxBonuses() => TestUtils.Wrap(() =>
        {
            var properties = new Properties(ValidProperties);
            properties.maxBonuses.Remove(Th08Converter.CharaWithTotal.Total);

            var career = Th08CardAttackCareerWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod()]
        public void Th08CardAttackCareerReadFromTestExceededMaxBonuses() => TestUtils.Wrap(() =>
        {
            var properties = new Properties(ValidProperties);
            properties.maxBonuses.Add(TestUtils.Cast<Th08Converter.CharaWithTotal>(999), 999u);

            var career = Th08CardAttackCareerWrapper.Create(MakeByteArray(properties));

            CollectionAssert.AreNotEqual(properties.maxBonuses.Values, career.MaxBonuses.Values.ToArray());
            CollectionAssert.AreEqual(
                properties.maxBonuses.Values.Take(properties.maxBonuses.Count - 1).ToArray(),
                career.MaxBonuses.Values.ToArray());
            CollectionAssert.AreNotEqual(properties.trialCounts.Values, career.TrialCounts.Values.ToArray());
            CollectionAssert.AreNotEqual(properties.clearCounts.Values, career.ClearCounts.Values.ToArray());
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "career")]
        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th08CardAttackCareerReadFromTestShortenedTrialCounts() => TestUtils.Wrap(() =>
        {
            var properties = new Properties(ValidProperties);
            properties.trialCounts.Remove(Th08Converter.CharaWithTotal.Total);

            var career = Th08CardAttackCareerWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod()]
        public void Th08CardAttackCareerReadFromTestExceededTrialCounts() => TestUtils.Wrap(() =>
        {
            var properties = new Properties(ValidProperties);
            properties.trialCounts.Add(TestUtils.Cast<Th08Converter.CharaWithTotal>(999), 999);

            var career = Th08CardAttackCareerWrapper.Create(MakeByteArray(properties));

            CollectionAssert.AreEqual(properties.maxBonuses.Values, career.MaxBonuses.Values.ToArray());
            CollectionAssert.AreNotEqual(properties.trialCounts.Values, career.TrialCounts.Values.ToArray());
            CollectionAssert.AreEqual(
                properties.trialCounts.Values.Take(properties.trialCounts.Count - 1).ToArray(),
                career.TrialCounts.Values.ToArray());
            CollectionAssert.AreNotEqual(properties.clearCounts.Values, career.ClearCounts.Values.ToArray());
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "career")]
        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th08CardAttackCareerReadFromTestShortenedClearCounts() => TestUtils.Wrap(() =>
        {
            var properties = new Properties(ValidProperties);
            properties.clearCounts.Remove(Th08Converter.CharaWithTotal.Total);

            var career = Th08CardAttackCareerWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod()]
        public void Th08CardAttackCareerReadFromTestExceededClearCounts() => TestUtils.Wrap(() =>
        {
            var properties = new Properties(ValidProperties);
            properties.clearCounts.Add(TestUtils.Cast<Th08Converter.CharaWithTotal>(999), 999);

            var career = Th08CardAttackCareerWrapper.Create(MakeByteArray(properties));

            CollectionAssert.AreEqual(properties.maxBonuses.Values, career.MaxBonuses.Values.ToArray());
            CollectionAssert.AreEqual(properties.trialCounts.Values, career.TrialCounts.Values.ToArray());
            CollectionAssert.AreNotEqual(properties.clearCounts.Values, career.ClearCounts.Values.ToArray());
            CollectionAssert.AreEqual(
                properties.clearCounts.Values.Take(properties.clearCounts.Count - 1).ToArray(),
                career.ClearCounts.Values.ToArray());
        });
    }
}
