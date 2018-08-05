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
    public class Th13SpellCardTests
    {
        internal struct Properties<TLevel>
            where TLevel : struct, Enum
        {
            public byte[] name;
            public int clearCount;
            public int practiceClearCount;
            public int trialCount;
            public int practiceTrialCount;
            public int id;
            public TLevel level;
            public int practiceScore;
        };

        internal static Properties<TLevel> GetValidProperties<TLevel>()
            where TLevel : struct, Enum
            => new Properties<TLevel>()
            {
                name = TestUtils.MakeRandomArray<byte>(0x80),
                clearCount = 1,
                practiceClearCount = 2,
                trialCount = 3,
                practiceTrialCount = 4,
                id = 5,
                level = TestUtils.Cast<TLevel>(1),
                practiceScore = 6789
            };

        internal static byte[] MakeByteArray<TLevel>(in Properties<TLevel> properties)
            where TLevel : struct, Enum
            => TestUtils.MakeByteArray(
                properties.name,
                properties.clearCount,
                properties.practiceClearCount,
                properties.trialCount,
                properties.practiceTrialCount,
                properties.id - 1,
                TestUtils.Cast<int>(properties.level),
                properties.practiceScore);

        internal static void Validate<TParent, TLevel>(
            in Th13SpellCardWrapper<TParent, TLevel> spellCard, in Properties<TLevel> properties)
            where TParent : ThConverter
            where TLevel : struct, Enum
        {
            CollectionAssert.AreEqual(properties.name, spellCard.Name?.ToArray());
            Assert.AreEqual(properties.clearCount, spellCard.ClearCount);
            Assert.AreEqual(properties.practiceClearCount, spellCard.PracticeClearCount);
            Assert.AreEqual(properties.trialCount, spellCard.TrialCount);
            Assert.AreEqual(properties.practiceTrialCount, spellCard.PracticeTrialCount);
            Assert.AreEqual(properties.id, spellCard.Id);
            Assert.AreEqual(properties.level, spellCard.Level);
            Assert.AreEqual(properties.practiceScore, spellCard.PracticeScore);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th13SpellCardTestHelper<TParent, TLevel>()
            where TParent : ThConverter
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = new Properties<TLevel>();
                var spellCard = new Th13SpellCardWrapper<TParent, TLevel>();

                Validate(spellCard, properties);
                Assert.IsFalse(spellCard.HasTried().Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th13SpellCardReadFromTestHelper<TParent, TLevel>()
            where TParent : ThConverter
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties<TLevel>();

                var spellCard = Th13SpellCardWrapper<TParent, TLevel>.Create(MakeByteArray(properties));

                Validate(spellCard, properties);
                Assert.IsTrue(spellCard.HasTried().Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th13SpellCardReadFromTestNullHelper<TParent, TLevel>()
            where TParent : ThConverter
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var spellCard = new Th13SpellCardWrapper<TParent, TLevel>();

                spellCard.ReadFrom(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "spellCard")]
        internal static void Th13SpellCardReadFromTestShortenedNameHelper<TParent, TLevel>()
            where TParent : ThConverter
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties<TLevel>();
                properties.name = properties.name.Take(properties.name.Length - 1).ToArray();

                var spellCard = Th13SpellCardWrapper<TParent, TLevel>.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th13SpellCardReadFromTestExceededNameHelper<TParent, TLevel>()
            where TParent : ThConverter
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties<TLevel>();
                properties.name = properties.name.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray();

                var spellCard = Th13SpellCardWrapper<TParent, TLevel>.Create(MakeByteArray(properties));

                CollectionAssert.AreNotEqual(properties.name, spellCard.Name.ToArray());
                CollectionAssert.AreEqual(properties.name.Take(0x80).ToArray(), spellCard.Name.ToArray());
                Assert.AreNotEqual(properties.clearCount, spellCard.ClearCount);
                Assert.AreNotEqual(properties.practiceClearCount, spellCard.PracticeClearCount);
                Assert.AreNotEqual(properties.trialCount, spellCard.TrialCount);
                Assert.AreNotEqual(properties.practiceTrialCount, spellCard.PracticeTrialCount);
                Assert.AreNotEqual(properties.id, spellCard.Id);
                Assert.AreNotEqual(properties.level, spellCard.Level.Value);
                Assert.AreNotEqual(properties.practiceScore, spellCard.PracticeScore);
                Assert.IsTrue(spellCard.HasTried().Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "spellCard")]
        internal static void Th13SpellCardReadFromTestInvalidLevelHelper<TParent, TLevel>(int level)
            where TParent : ThConverter
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties<TLevel>();
                properties.level = TestUtils.Cast<TLevel>(level);

                var spellCard = Th13SpellCardWrapper<TParent, TLevel>.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th13SpellCardTestHelper<TParent>()
            where TParent : ThConverter
            => Th13SpellCardTestHelper<TParent, ThConverter.Level>();

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th13SpellCardReadFromTestHelper<TParent>()
            where TParent : ThConverter
            => Th13SpellCardReadFromTestHelper<TParent, ThConverter.Level>();

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th13SpellCardReadFromTestNullHelper<TParent>()
            where TParent : ThConverter
            => Th13SpellCardReadFromTestNullHelper<TParent, ThConverter.Level>();

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th13SpellCardReadFromTestShortenedNameHelper<TParent>()
            where TParent : ThConverter
            => Th13SpellCardReadFromTestShortenedNameHelper<TParent, ThConverter.Level>();

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th13SpellCardReadFromTestExceededNameHelper<TParent>()
            where TParent : ThConverter
            => Th13SpellCardReadFromTestExceededNameHelper<TParent, ThConverter.Level>();

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th13SpellCardReadFromTestInvalidLevelHelper<TParent>(int level)
            where TParent : ThConverter
            => Th13SpellCardReadFromTestInvalidLevelHelper<TParent, ThConverter.Level>(level);

        public static IEnumerable<object[]> InvalidTh13LevelPractices
            => TestUtils.GetInvalidEnumerators(typeof(Th13Converter.LevelPractice));

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(ThConverter.Level));

        #region Th13

        [TestMethod()]
        public void Th13SpellCardTest()
            => Th13SpellCardTestHelper<Th13Converter, Th13Converter.LevelPractice>();

        [TestMethod()]
        public void Th13SpellCardReadFromTest()
            => Th13SpellCardReadFromTestHelper<Th13Converter, Th13Converter.LevelPractice>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th13SpellCardReadFromTestNull()
            => Th13SpellCardReadFromTestNullHelper<Th13Converter, Th13Converter.LevelPractice>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th13SpellCardReadFromTestShortenedName()
            => Th13SpellCardReadFromTestShortenedNameHelper<Th13Converter, Th13Converter.LevelPractice>();

        [TestMethod()]
        public void Th13SpellCardReadFromTestExceededName()
            => Th13SpellCardReadFromTestExceededNameHelper<Th13Converter, Th13Converter.LevelPractice>();

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidTh13LevelPractices))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th13SpellCardReadFromTestInvalidLevel(int level)
            => Th13SpellCardReadFromTestInvalidLevelHelper<Th13Converter, Th13Converter.LevelPractice>(level);

        #endregion

        #region Th14

        [TestMethod()]
        public void Th14SpellCardTest()
            => Th13SpellCardTestHelper<Th14Converter>();

        [TestMethod()]
        public void Th14SpellCardReadFromTest()
            => Th13SpellCardReadFromTestHelper<Th14Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th14SpellCardReadFromTestNull()
            => Th13SpellCardReadFromTestNullHelper<Th14Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th14SpellCardReadFromTestShortenedName()
            => Th13SpellCardReadFromTestShortenedNameHelper<Th14Converter>();

        [TestMethod()]
        public void Th14SpellCardReadFromTestExceededName()
            => Th13SpellCardReadFromTestExceededNameHelper<Th14Converter>();

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th14SpellCardReadFromTestInvalidLevel(int level)
            => Th13SpellCardReadFromTestInvalidLevelHelper<Th14Converter>(level);

        #endregion

        #region Th15

        [TestMethod()]
        public void Th15SpellCardTest()
            => Th13SpellCardTestHelper<Th15Converter>();

        [TestMethod()]
        public void Th15SpellCardReadFromTest()
            => Th13SpellCardReadFromTestHelper<Th15Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th15SpellCardReadFromTestNull()
            => Th13SpellCardReadFromTestNullHelper<Th15Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th15SpellCardReadFromTestShortenedName()
            => Th13SpellCardReadFromTestShortenedNameHelper<Th15Converter>();

        [TestMethod()]
        public void Th15SpellCardReadFromTestExceededName()
            => Th13SpellCardReadFromTestExceededNameHelper<Th15Converter>();

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th15SpellCardReadFromTestInvalidLevel(int level)
            => Th13SpellCardReadFromTestInvalidLevelHelper<Th15Converter>(level);

        #endregion

        #region Th16

        [TestMethod()]
        public void Th16SpellCardTest()
            => Th13SpellCardTestHelper<Th16Converter>();

        [TestMethod()]
        public void Th16SpellCardReadFromTest()
            => Th13SpellCardReadFromTestHelper<Th16Converter>();

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th16SpellCardReadFromTestNull()
            => Th13SpellCardReadFromTestNullHelper<Th16Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th16SpellCardReadFromTestShortenedName()
            => Th13SpellCardReadFromTestShortenedNameHelper<Th16Converter>();

        [TestMethod()]
        public void Th16SpellCardReadFromTestExceededName()
            => Th13SpellCardReadFromTestExceededNameHelper<Th16Converter>();

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th16SpellCardReadFromTestInvalidLevel(int level)
            => Th13SpellCardReadFromTestInvalidLevelHelper<Th16Converter>(level);

        #endregion
    }
}
