using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th13;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th13.Stubs;
using ThScoreFileConverterTests.Models.Th13.Wrappers;

namespace ThScoreFileConverterTests.Models.Th15
{
    [TestClass]
    public class SpellCardTests
    {
        internal static SpellCardStub<TLevel> GetValidStub<TLevel>()
            where TLevel : struct, Enum
            => new SpellCardStub<TLevel>()
            {
                Name = TestUtils.MakeRandomArray<byte>(0x80),
                ClearCount = 1,
                PracticeClearCount = 2,
                TrialCount = 3,
                PracticeTrialCount = 4,
                Id = 5,
                Level = TestUtils.Cast<TLevel>(1),
                PracticeScore = 6789
            };

        internal static byte[] MakeByteArray<TLevel>(ISpellCard<TLevel> spellCard)
            where TLevel : struct, Enum
            => TestUtils.MakeByteArray(
                spellCard.Name,
                spellCard.ClearCount,
                spellCard.PracticeClearCount,
                spellCard.TrialCount,
                spellCard.PracticeTrialCount,
                spellCard.Id - 1,
                TestUtils.Cast<int>(spellCard.Level),
                spellCard.PracticeScore);

        internal static void Validate<TParent, TLevel>(
            ISpellCard<TLevel> expected, in SpellCardWrapper<TParent, TLevel> actual)
            where TParent : ThConverter
            where TLevel : struct, Enum
        {
            CollectionAssert.That.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.ClearCount, actual.ClearCount);
            Assert.AreEqual(expected.PracticeClearCount, actual.PracticeClearCount);
            Assert.AreEqual(expected.TrialCount, actual.TrialCount);
            Assert.AreEqual(expected.PracticeTrialCount, actual.PracticeTrialCount);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Level, actual.Level);
            Assert.AreEqual(expected.PracticeScore, actual.PracticeScore);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th13SpellCardTestHelper<TParent, TLevel>()
            where TParent : ThConverter
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = new SpellCardStub<TLevel>();
                var spellCard = new SpellCardWrapper<TParent, TLevel>();

                Validate(stub, spellCard);
                Assert.IsFalse(spellCard.HasTried.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th13SpellCardReadFromTestHelper<TParent, TLevel>()
            where TParent : ThConverter
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = GetValidStub<TLevel>();

                var spellCard = SpellCardWrapper<TParent, TLevel>.Create(MakeByteArray(stub));

                Validate(stub, spellCard);
                Assert.IsTrue(spellCard.HasTried.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th13SpellCardReadFromTestNullHelper<TParent, TLevel>()
            where TParent : ThConverter
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var spellCard = new SpellCardWrapper<TParent, TLevel>();

                spellCard.ReadFrom(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th13SpellCardReadFromTestShortenedNameHelper<TParent, TLevel>()
            where TParent : ThConverter
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = GetValidStub<TLevel>();
                stub.Name = stub.Name.SkipLast(1).ToArray();

                _ = SpellCardWrapper<TParent, TLevel>.Create(MakeByteArray(stub));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th13SpellCardReadFromTestExceededNameHelper<TParent, TLevel>()
            where TParent : ThConverter
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = GetValidStub<TLevel>();
                stub.Name = stub.Name.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray();

                _ = SpellCardWrapper<TParent, TLevel>.Create(MakeByteArray(stub));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th13SpellCardReadFromTestInvalidLevelHelper<TParent, TLevel>(int level)
            where TParent : ThConverter
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = GetValidStub<TLevel>();
                stub.Level = TestUtils.Cast<TLevel>(level);

                _ = SpellCardWrapper<TParent, TLevel>.Create(MakeByteArray(stub));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th13SpellCardTestHelper<TParent>()
            where TParent : ThConverter
            => Th13SpellCardTestHelper<TParent, Level>();

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th13SpellCardReadFromTestHelper<TParent>()
            where TParent : ThConverter
            => Th13SpellCardReadFromTestHelper<TParent, Level>();

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th13SpellCardReadFromTestNullHelper<TParent>()
            where TParent : ThConverter
            => Th13SpellCardReadFromTestNullHelper<TParent, Level>();

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th13SpellCardReadFromTestShortenedNameHelper<TParent>()
            where TParent : ThConverter
            => Th13SpellCardReadFromTestShortenedNameHelper<TParent, Level>();

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th13SpellCardReadFromTestExceededNameHelper<TParent>()
            where TParent : ThConverter
            => Th13SpellCardReadFromTestExceededNameHelper<TParent, Level>();

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th13SpellCardReadFromTestInvalidLevelHelper<TParent>(int level)
            where TParent : ThConverter
            => Th13SpellCardReadFromTestInvalidLevelHelper<TParent, Level>(level);

        public static IEnumerable<object[]> InvalidTh13LevelPractices
            => TestUtils.GetInvalidEnumerators(typeof(LevelPractice));

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Level));

        [TestMethod]
        public void Th15SpellCardTest()
            => Th13SpellCardTestHelper<Th15Converter>();

        [TestMethod]
        public void Th15SpellCardReadFromTest()
            => Th13SpellCardReadFromTestHelper<Th15Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th15SpellCardReadFromTestNull()
            => Th13SpellCardReadFromTestNullHelper<Th15Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th15SpellCardReadFromTestShortenedName()
            => Th13SpellCardReadFromTestShortenedNameHelper<Th15Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th15SpellCardReadFromTestExceededName()
            => Th13SpellCardReadFromTestExceededNameHelper<Th15Converter>();

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th15SpellCardReadFromTestInvalidLevel(int level)
            => Th13SpellCardReadFromTestInvalidLevelHelper<Th15Converter>(level);
    }
}
