using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Wrappers;
using SpellCard = ThScoreFileConverter.Models.Th10.SpellCard;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th10SpellCardTests
    {
        internal struct Properties
        {
            public byte[] name;
            public int clearCount;
            public int trialCount;
            public int id;
            public ThConverter.Level level;
        };

        internal static Properties ValidProperties => new Properties()
        {
            name = TestUtils.MakeRandomArray<byte>(0x80),
            clearCount = 123,
            trialCount = 456,
            id = 789,
            level = ThConverter.Level.Normal
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.name,
                properties.clearCount,
                properties.trialCount,
                properties.id - 1,
                (int)properties.level);

        internal static void Validate(in Th10SpellCardWrapper spellCard, in Properties properties)
            => Validate(spellCard.Target as SpellCard, properties);

        internal static void Validate(in SpellCard spellCard, in Properties properties)
        {
            CollectionAssert.AreEqual(properties.name, spellCard.Name?.ToArray());
            Assert.AreEqual(properties.clearCount, spellCard.ClearCount);
            Assert.AreEqual(properties.trialCount, spellCard.TrialCount);
            Assert.AreEqual(properties.id, spellCard.Id);
            Assert.AreEqual(properties.level, spellCard.Level);
        }

        [TestMethod]
        public void Th10SpellCardTest()
            => TestUtils.Wrap(() =>
            {
                var properties = new Properties();
                var spellCard = new Th10SpellCardWrapper();

                Validate(spellCard, properties);
                Assert.IsFalse(spellCard.HasTried.Value);
            });

        [TestMethod]
        public void Th10SpellCardReadFromTest()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;

                var spellCard = Th10SpellCardWrapper.Create(MakeByteArray(properties));

                Validate(spellCard, properties);
                Assert.IsTrue(spellCard.HasTried.Value);
            });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th10SpellCardReadFromTestNull()
            => TestUtils.Wrap(() =>
            {
                var spellCard = new Th10SpellCardWrapper();
                spellCard.ReadFrom(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "spellCard")]
        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th10SpellCardReadFromTestShortenedName()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.name = properties.name.Take(properties.name.Length - 1).ToArray();

                var spellCard = Th10SpellCardWrapper.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "spellCard")]
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th10SpellCardReadFromTestExceededName()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.name = properties.name.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray();

                var spellCard = Th10SpellCardWrapper.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(ThConverter.Level));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "spellCard")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th10SpellCardReadFromTestInvalidLevel(int level)
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.level = TestUtils.Cast<ThConverter.Level>(level);

                var spellCard = Th10SpellCardWrapper.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });
    }
}
