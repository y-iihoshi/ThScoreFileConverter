using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th13;
using ThScoreFileConverterTests.Extensions;

namespace ThScoreFileConverterTests.Models.Th13
{
    [TestClass]
    public class SpellCardTests
    {
        internal static Mock<ISpellCard<TLevel>> MockSpellCard<TLevel>()
            where TLevel : struct, Enum
        {
            var mock = new Mock<ISpellCard<TLevel>>();
            _ = mock.SetupGet(m => m.Name).Returns(TestUtils.MakeRandomArray<byte>(0x80));
            _ = mock.SetupGet(m => m.ClearCount).Returns(1);
            _ = mock.SetupGet(m => m.PracticeClearCount).Returns(2);
            _ = mock.SetupGet(m => m.TrialCount).Returns(3);
            _ = mock.SetupGet(m => m.PracticeTrialCount).Returns(4);
            _ = mock.SetupGet(m => m.Id).Returns(5);
            _ = mock.SetupGet(m => m.Level).Returns(TestUtils.Cast<TLevel>(1));
            _ = mock.SetupGet(m => m.PracticeScore).Returns(6789);
            return mock;
        }

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
        {
            var mock = new Mock<ISpellCard<TLevel>>();
            var spellCard = new SpellCard<TLevel>();

            Validate(mock.Object, spellCard);
            Assert.IsFalse(spellCard.HasTried);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestHelper<TLevel>()
            where TLevel : struct, Enum
        {
            var mock = MockSpellCard<TLevel>();

            var spellCard = TestUtils.Create<SpellCard<TLevel>>(MakeByteArray(mock.Object));

            Validate(mock.Object, spellCard);
            Assert.IsTrue(spellCard.HasTried);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestNullHelper<TLevel>()
            where TLevel : struct, Enum
        {
            var spellCard = new SpellCard<TLevel>();

            _ = Assert.ThrowsException<ArgumentNullException>(() => spellCard.ReadFrom(null!));
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestShortenedNameHelper<TLevel>()
            where TLevel : struct, Enum
        {
            var mock = MockSpellCard<TLevel>();
            var name = mock.Object.Name;
            _ = mock.SetupGet(m => m.Name).Returns(name.SkipLast(1).ToArray());

            _ = Assert.ThrowsException<InvalidCastException>(
                () => _ = TestUtils.Create<SpellCard<TLevel>>(MakeByteArray(mock.Object)));
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestExceededNameHelper<TLevel>()
            where TLevel : struct, Enum
        {
            var mock = MockSpellCard<TLevel>();
            var name = mock.Object.Name;
            _ = mock.SetupGet(m => m.Name).Returns(name.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray());

            _ = Assert.ThrowsException<InvalidCastException>(
                () => _ = TestUtils.Create<SpellCard<TLevel>>(MakeByteArray(mock.Object)));
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestInvalidLevelHelper<TLevel>(int level)
            where TLevel : struct, Enum
        {
            var mock = MockSpellCard<TLevel>();
            _ = mock.SetupGet(m => m.Level).Returns(TestUtils.Cast<TLevel>(level));

            _ = Assert.ThrowsException<InvalidCastException>(
                () => _ = TestUtils.Create<SpellCard<TLevel>>(MakeByteArray(mock.Object)));
        }

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
        public void Th13ReadFromTestNull()
            => ReadFromTestNullHelper<LevelPractice>();

        [TestMethod]
        public void Th13ReadFromTestShortenedName()
            => ReadFromTestShortenedNameHelper<LevelPractice>();

        [TestMethod]
        public void Th13ReadFromTestExceededName()
            => ReadFromTestExceededNameHelper<LevelPractice>();

        [DataTestMethod]
        [DynamicData(nameof(InvalidTh13LevelPractices))]
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
        public void Th14ReadFromTestNull()
            => ReadFromTestNullHelper<Level>();

        [TestMethod]
        public void Th14ReadFromTestShortenedName()
            => ReadFromTestShortenedNameHelper<Level>();

        [TestMethod]
        public void Th14ReadFromTestExceededName()
            => ReadFromTestExceededNameHelper<Level>();

        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
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
        public void Th16ReadFromTestNull()
            => ReadFromTestNullHelper<Level>();

        [TestMethod]
        public void Th16ReadFromTestShortenedName()
            => ReadFromTestShortenedNameHelper<Level>();

        [TestMethod]
        public void Th16ReadFromTestExceededName()
            => ReadFromTestExceededNameHelper<Level>();

        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        public void Th16ReadFromTestInvalidLevel(int level)
            => ReadFromTestInvalidLevelHelper<Level>(level);

        #endregion
    }
}
