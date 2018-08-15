using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th128SpellCardTests
    {
        internal struct Properties
        {
            public byte[] name;
            public int noMissCount;
            public int noIceCount;
            public int trialCount;
            public int id;
            public ThConverter.Level level;
        };

        internal static Properties ValidProperties => new Properties()
        {
            name = TestUtils.MakeRandomArray<byte>(0x80),
            noMissCount = 12,
            noIceCount = 34,
            trialCount = 56,
            id = 78,
            level = ThConverter.Level.Normal
        };

        internal static byte[] MakeByteArray(in Properties properties)
        => TestUtils.MakeByteArray(
            properties.name,
            properties.noMissCount,
            properties.noIceCount,
            0u,
            properties.trialCount,
            properties.id - 1,
            (int)properties.level);

        internal static void Validate(in Th128SpellCardWrapper spellCard, in Properties properties)
        {
            CollectionAssert.AreEqual(properties.name, spellCard.Name?.ToArray());
            Assert.AreEqual(properties.noMissCount, spellCard.NoMissCount);
            Assert.AreEqual(properties.noIceCount, spellCard.NoIceCount);
            Assert.AreEqual(properties.trialCount, spellCard.TrialCount);
            Assert.AreEqual(properties.id, spellCard.Id);
            Assert.AreEqual(properties.level, spellCard.Level);
        }

        [TestMethod]
        public void Th128SpellCardTest() => TestUtils.Wrap(() =>
        {
            var properties = new Properties();
            var spellCard = new Th128SpellCardWrapper();

            Validate(spellCard, properties);
            Assert.IsFalse(spellCard.HasTried().Value);
        });

        [TestMethod]
        public void Th128SpellCardReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var spellCard = Th128SpellCardWrapper.Create(MakeByteArray(properties));

            Validate(spellCard, properties);
            Assert.IsTrue(spellCard.HasTried().Value);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th128SpellCardReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var spellCard = new Th128SpellCardWrapper();
            spellCard.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "spellCard")]
        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th128SpellCardReadFromTestShortenedName() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.name = properties.name.Take(properties.name.Length - 1).ToArray();

            var spellCard = Th128SpellCardWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void Th128SpellCardReadFromTestExceededName() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.name = properties.name.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray();

            var spellCard = Th128SpellCardWrapper.Create(MakeByteArray(properties));

            CollectionAssert.AreNotEqual(properties.name, spellCard.Name.ToArray());
            CollectionAssert.AreEqual(
                properties.name.Take(properties.name.Length - 1).ToArray(), spellCard.Name.ToArray());
            Assert.AreNotEqual(properties.noMissCount, spellCard.NoMissCount);
            Assert.AreNotEqual(properties.noIceCount, spellCard.NoIceCount);
            Assert.AreNotEqual(properties.trialCount, spellCard.TrialCount);
            Assert.AreNotEqual(properties.id, spellCard.Id);
            Assert.AreNotEqual(properties.level, spellCard.Level.Value);
            Assert.IsTrue(spellCard.HasTried().Value);
        });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(ThConverter.Level));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "spellCard")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th128SpellCardReadFromTestInvalidLevel(int level) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.level = TestUtils.Cast<ThConverter.Level>(level);

            var spellCard = Th128SpellCardWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
