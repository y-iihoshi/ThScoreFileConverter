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

namespace ThScoreFileConverterTests.Models.Th13
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

        internal static void Validate<TLevel>(ISpellCard<TLevel> expected, ISpellCard<TLevel> actual)
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
        internal static void SpellCardTestHelper<TLevel>()
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = new SpellCardStub<TLevel>();
                var spellCard = new SpellCard<TLevel>();

                Validate(stub, spellCard);
                Assert.IsFalse(spellCard.HasTried);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestHelper<TLevel>()
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = GetValidStub<TLevel>();

                var spellCard = TestUtils.Create<SpellCard<TLevel>>(MakeByteArray(stub));

                Validate(stub, spellCard);
                Assert.IsTrue(spellCard.HasTried);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestNullHelper<TLevel>()
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var spellCard = new SpellCard<TLevel>();

                spellCard.ReadFrom(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestShortenedNameHelper<TLevel>()
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = GetValidStub<TLevel>();
                stub.Name = stub.Name.SkipLast(1).ToArray();

                _ = TestUtils.Create<SpellCard<TLevel>>(MakeByteArray(stub));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestExceededNameHelper<TLevel>()
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = GetValidStub<TLevel>();
                stub.Name = stub.Name.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray();

                _ = TestUtils.Create<SpellCard<TLevel>>(MakeByteArray(stub));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestInvalidLevelHelper<TLevel>(int level)
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = GetValidStub<TLevel>();
                stub.Level = TestUtils.Cast<TLevel>(level);

                _ = TestUtils.Create<SpellCard<TLevel>>(MakeByteArray(stub));

                Assert.Fail(TestUtils.Unreachable);
            });

        public static IEnumerable<object[]> InvalidTh13LevelPractices
            => TestUtils.GetInvalidEnumerators(typeof(LevelPractice));

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Level));

        #region Th13

        [TestMethod]
        public void Th13SpellCardTest()
            => SpellCardTestHelper<LevelPractice>();

        [TestMethod]
        public void Th13ReadFromTest()
            => ReadFromTestHelper<LevelPractice>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th13ReadFromTestNull()
            => ReadFromTestNullHelper<LevelPractice>();

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th13ReadFromTestShortenedName()
            => ReadFromTestShortenedNameHelper<LevelPractice>();

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th13ReadFromTestExceededName()
            => ReadFromTestExceededNameHelper<LevelPractice>();

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidTh13LevelPractices))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th13ReadFromTestInvalidLevel(int level)
            => ReadFromTestInvalidLevelHelper<LevelPractice>(level);

        #endregion

        #region Th14

        [TestMethod]
        public void Th14SpellCardTest()
            => SpellCardTestHelper<Level>();

        [TestMethod]
        public void Th14ReadFromTest()
            => ReadFromTestHelper<Level>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th14ReadFromTestNull()
            => ReadFromTestNullHelper<Level>();

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th14ReadFromTestShortenedName()
            => ReadFromTestShortenedNameHelper<Level>();

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th14ReadFromTestExceededName()
            => ReadFromTestExceededNameHelper<Level>();

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th14ReadFromTestInvalidLevel(int level)
            => ReadFromTestInvalidLevelHelper<Level>(level);

        #endregion

        #region Th16

        [TestMethod]
        public void Th16SpellCardTest()
            => SpellCardTestHelper<Level>();

        [TestMethod]
        public void Th16ReadFromTest()
            => ReadFromTestHelper<Level>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th16ReadFromTestNull()
            => ReadFromTestNullHelper<Level>();

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th16ReadFromTestShortenedName()
            => ReadFromTestShortenedNameHelper<Level>();

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th16ReadFromTestExceededName()
            => ReadFromTestExceededNameHelper<Level>();

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th16ReadFromTestInvalidLevel(int level)
            => ReadFromTestInvalidLevelHelper<Level>(level);

        #endregion
    }
}
