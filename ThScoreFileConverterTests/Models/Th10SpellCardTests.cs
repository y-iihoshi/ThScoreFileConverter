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

        internal static void Validate<TParent>(
            in Th10SpellCardWrapper<TParent> spellCard, in Properties properties)
            where TParent : ThConverter
        {
            CollectionAssert.AreEqual(properties.name, spellCard.Name?.ToArray());
            Assert.AreEqual(properties.clearCount, spellCard.ClearCount);
            Assert.AreEqual(properties.trialCount, spellCard.TrialCount);
            Assert.AreEqual(properties.id, spellCard.Id);
            Assert.AreEqual(properties.level, spellCard.Level);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th10SpellCardTestHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = new Properties();
                var spellCard = new Th10SpellCardWrapper<TParent>();

                Validate(spellCard, properties);
                Assert.IsFalse(spellCard.HasTried().Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th10SpellCardReadFromTestHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;

                var spellCard = Th10SpellCardWrapper<TParent>.Create(MakeByteArray(properties));

                Validate(spellCard, properties);
                Assert.IsTrue(spellCard.HasTried().Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th10SpellCardReadFromTestNullHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var spellCard = new Th10SpellCardWrapper<TParent>();
                spellCard.ReadFrom(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "spellCard")]
        internal static void Th10SpellCardReadFromTestShortenedNameHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.name = properties.name.Take(properties.name.Length - 1).ToArray();

                var spellCard = Th10SpellCardWrapper<TParent>.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th10SpellCardReadFromTestExceededNameHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.name = properties.name.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray();

                var spellCard = Th10SpellCardWrapper<TParent>.Create(MakeByteArray(properties));

                CollectionAssert.AreNotEqual(properties.name, spellCard.Name.ToArray());
                CollectionAssert.AreEqual(
                    properties.name.Take(properties.name.Length - 1).ToArray(), spellCard.Name.ToArray());
                Assert.AreNotEqual(properties.clearCount, spellCard.ClearCount);
                Assert.AreNotEqual(properties.trialCount, spellCard.TrialCount);
                Assert.AreNotEqual(properties.id, spellCard.Id);
                Assert.AreNotEqual(properties.level, spellCard.Level.Value);
                Assert.IsTrue(spellCard.HasTried().Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "spellCard")]
        internal static void Th10SpellCardReadFromTestInvalidLevelHelper<TParent>(int level)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.level = TestUtils.Cast<ThConverter.Level>(level);

                var spellCard = Th10SpellCardWrapper<TParent>.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(ThConverter.Level));

        #region Th10

        [TestMethod]
        public void Th10SpellCardTest()
            => Th10SpellCardTestHelper<Th10Converter>();

        [TestMethod]
        public void Th10SpellCardReadFromTest()
            => Th10SpellCardReadFromTestHelper<Th10Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th10SpellCardReadFromTestNull()
            => Th10SpellCardReadFromTestNullHelper<Th10Converter>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th10SpellCardReadFromTestShortenedName()
            => Th10SpellCardReadFromTestShortenedNameHelper<Th10Converter>();

        [TestMethod]
        public void Th10SpellCardReadFromTestExceededName()
            => Th10SpellCardReadFromTestExceededNameHelper<Th10Converter>();

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th10SpellCardReadFromTestInvalidLevel(int level)
            => Th10SpellCardReadFromTestInvalidLevelHelper<Th10Converter>(level);

        #endregion

        #region Th11

        [TestMethod]
        public void Th11SpellCardTest()
            => Th10SpellCardTestHelper<Th11Converter>();

        [TestMethod]
        public void Th11SpellCardReadFromTest()
            => Th10SpellCardReadFromTestHelper<Th11Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th11SpellCardReadFromTestNull()
            => Th10SpellCardReadFromTestNullHelper<Th11Converter>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th11SpellCardReadFromTestShortenedName()
            => Th10SpellCardReadFromTestShortenedNameHelper<Th11Converter>();

        [TestMethod]
        public void Th11SpellCardReadFromTestExceededName()
            => Th10SpellCardReadFromTestExceededNameHelper<Th11Converter>();

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th11SpellCardReadFromTestInvalidLevel(int level)
            => Th10SpellCardReadFromTestInvalidLevelHelper<Th11Converter>(level);

        #endregion

        #region Th12

        [TestMethod]
        public void Th12SpellCardTest()
            => Th10SpellCardTestHelper<Th12Converter>();

        [TestMethod]
        public void Th12SpellCardReadFromTest()
            => Th10SpellCardReadFromTestHelper<Th12Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th12SpellCardReadFromTestNull()
            => Th10SpellCardReadFromTestNullHelper<Th12Converter>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th12SpellCardReadFromTestShortenedName()
            => Th10SpellCardReadFromTestShortenedNameHelper<Th12Converter>();

        [TestMethod]
        public void Th12SpellCardReadFromTestExceededName()
            => Th10SpellCardReadFromTestExceededNameHelper<Th12Converter>();

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th12SpellCardReadFromTestInvalidLevel(int level)
            => Th10SpellCardReadFromTestInvalidLevelHelper<Th12Converter>(level);

        #endregion
    }
}
