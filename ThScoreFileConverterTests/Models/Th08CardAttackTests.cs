using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace ThScoreFileConverter.Models.Tests
{
    [TestClass()]
    public class Th08CardAttackTests
    {
        [TestMethod()]
        public void Th08CardAttackTestChapter()
        {
            try
            {
                var signature = "CATK";
                short size1 = 0x22C;
                short size2 = 0x22C;
                var unknown1 = 1u;
                short cardId = 123;
                byte unknown2 = 2;
                var level = Th08Converter.LevelPracticeWithTotal.Normal;
                var cardName = TestUtils.MakeRandomArray<byte>(0x30);
                var enemyName = TestUtils.MakeRandomArray<byte>(0x30);
                var comment = TestUtils.MakeRandomArray<byte>(0x80);
                var storyMaxBonuses = TestUtils.MakeRandomArray<uint>(13);
                var storyTrialCounts = TestUtils.MakeRandomArray<int>(12).Concat(new int[] { 1 }).ToArray();
                var storyClearCounts = TestUtils.MakeRandomArray<int>(13);
                var practiceMaxBonuses = TestUtils.MakeRandomArray<uint>(13);
                var practiceTrialCounts = TestUtils.MakeRandomArray<int>(12).Concat(new int[] { 1 }).ToArray();
                var practiceClearCounts = TestUtils.MakeRandomArray<int>(13);
                var unknown3 = 3u;
                var data = TestUtils.MakeByteArray(
                    unknown1,
                    (short)(cardId - 1),
                    unknown2,
                    (byte)level,
                    cardName,
                    enemyName,
                    comment,
                    storyMaxBonuses,
                    storyTrialCounts,
                    storyClearCounts,
                    practiceMaxBonuses,
                    practiceTrialCounts,
                    practiceClearCounts,
                    unknown3);

                var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var cardAttack = new Th08CardAttackWrapper(chapter);

                Assert.AreEqual(signature, cardAttack.Signature);
                Assert.AreEqual(size1, cardAttack.Size1);
                Assert.AreEqual(size2, cardAttack.Size2);
                CollectionAssert.AreEqual(data, cardAttack.Data.ToArray());
                Assert.AreEqual(data[0], cardAttack.FirstByteOfData);
                Assert.AreEqual(cardId, cardAttack.CardId);
                Assert.AreEqual(level, cardAttack.Level.Value);
                CollectionAssert.AreEqual(cardName, cardAttack.CardName.ToArray());
                CollectionAssert.AreEqual(enemyName, cardAttack.EnemyName.ToArray());
                CollectionAssert.AreEqual(comment, cardAttack.Comment.ToArray());
                CollectionAssert.AreEqual(storyMaxBonuses, cardAttack.StoryCareer.MaxBonuses.Values.ToArray());
                CollectionAssert.AreEqual(storyTrialCounts, cardAttack.StoryCareer.TrialCounts.Values.ToArray());
                CollectionAssert.AreEqual(storyClearCounts, cardAttack.StoryCareer.ClearCounts.Values.ToArray());
                CollectionAssert.AreEqual(practiceMaxBonuses, cardAttack.PracticeCareer.MaxBonuses.Values.ToArray());
                CollectionAssert.AreEqual(practiceTrialCounts, cardAttack.PracticeCareer.TrialCounts.Values.ToArray());
                CollectionAssert.AreEqual(practiceClearCounts, cardAttack.PracticeCareer.ClearCounts.Values.ToArray());
                Assert.IsTrue(cardAttack.HasTried.Value);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "cardAttack")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08CardAttackTestNullChapter()
        {
            try
            {
                var cardAttack = new Th08CardAttackWrapper(null);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "cardAttack")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08CardAttackTestInvalidSignature()
        {
            try
            {
                var signature = "catk";
                short size1 = 0x22C;
                short size2 = 0x22C;
                var unknown1 = 1u;
                short cardId = 123;
                byte unknown2 = 2;
                var level = Th08Converter.LevelPracticeWithTotal.Normal;
                var cardName = TestUtils.MakeRandomArray<byte>(0x30);
                var enemyName = TestUtils.MakeRandomArray<byte>(0x30);
                var comment = TestUtils.MakeRandomArray<byte>(0x80);
                var storyMaxBonuses = TestUtils.MakeRandomArray<uint>(13);
                var storyTrialCounts = TestUtils.MakeRandomArray<int>(13);
                var storyClearCounts = TestUtils.MakeRandomArray<int>(13);
                var practiceMaxBonuses = TestUtils.MakeRandomArray<uint>(13);
                var practiceTrialCounts = TestUtils.MakeRandomArray<int>(13);
                var practiceClearCounts = TestUtils.MakeRandomArray<int>(13);
                var unknown3 = 3u;
                var data = TestUtils.MakeByteArray(
                    unknown1,
                    (short)(cardId - 1),
                    unknown2,
                    (byte)level,
                    cardName,
                    enemyName,
                    comment,
                    storyMaxBonuses,
                    storyTrialCounts,
                    storyClearCounts,
                    practiceMaxBonuses,
                    practiceTrialCounts,
                    practiceClearCounts,
                    unknown3);

                var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var cardAttack = new Th08CardAttackWrapper(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "cardAttack")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08CardAttackTestInvalidSize1()
        {
            try
            {
                var signature = "CATK";
                short size1 = 0x22D;
                short size2 = 0x22C;
                var unknown1 = 1u;
                short cardId = 123;
                byte unknown2 = 2;
                var level = Th08Converter.LevelPracticeWithTotal.Normal;
                var cardName = TestUtils.MakeRandomArray<byte>(0x30);
                var enemyName = TestUtils.MakeRandomArray<byte>(0x30);
                var comment = TestUtils.MakeRandomArray<byte>(0x80);
                var storyMaxBonuses = TestUtils.MakeRandomArray<uint>(13);
                var storyTrialCounts = TestUtils.MakeRandomArray<int>(13);
                var storyClearCounts = TestUtils.MakeRandomArray<int>(13);
                var practiceMaxBonuses = TestUtils.MakeRandomArray<uint>(13);
                var practiceTrialCounts = TestUtils.MakeRandomArray<int>(13);
                var practiceClearCounts = TestUtils.MakeRandomArray<int>(13);
                var unknown3 = 3u;
                var data = TestUtils.MakeByteArray(
                    unknown1,
                    (short)(cardId - 1),
                    unknown2,
                    (byte)level,
                    cardName,
                    enemyName,
                    comment,
                    storyMaxBonuses,
                    storyTrialCounts,
                    storyClearCounts,
                    practiceMaxBonuses,
                    practiceTrialCounts,
                    practiceClearCounts,
                    unknown3);

                var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var cardAttack = new Th08CardAttackWrapper(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08CardAttackTestInvalidLevel()
        {
            try
            {
                var signature = "CATK";
                short size1 = 0x22C;
                short size2 = 0x22C;
                var unknown1 = 1u;
                short cardId = 123;
                byte unknown2 = 2;
                var level = (Th08Converter.LevelPracticeWithTotal)(-1);
                var cardName = TestUtils.MakeRandomArray<byte>(0x30);
                var enemyName = TestUtils.MakeRandomArray<byte>(0x30);
                var comment = TestUtils.MakeRandomArray<byte>(0x80);
                var storyMaxBonuses = TestUtils.MakeRandomArray<uint>(13);
                var storyTrialCounts = TestUtils.MakeRandomArray<int>(12).Concat(new int[] { 1 }).ToArray();
                var storyClearCounts = TestUtils.MakeRandomArray<int>(13);
                var practiceMaxBonuses = TestUtils.MakeRandomArray<uint>(13);
                var practiceTrialCounts = TestUtils.MakeRandomArray<int>(12).Concat(new int[] { 1 }).ToArray();
                var practiceClearCounts = TestUtils.MakeRandomArray<int>(13);
                var unknown3 = 3u;
                var data = TestUtils.MakeByteArray(
                    unknown1,
                    (short)(cardId - 1),
                    unknown2,
                    (byte)level,
                    cardName,
                    enemyName,
                    comment,
                    storyMaxBonuses,
                    storyTrialCounts,
                    storyClearCounts,
                    practiceMaxBonuses,
                    practiceTrialCounts,
                    practiceClearCounts,
                    unknown3);

                var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var cardAttack = new Th08CardAttackWrapper(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        public void Th08CardAttackTestNotTried()
        {
            try
            {
                var signature = "CATK";
                short size1 = 0x22C;
                short size2 = 0x22C;
                var unknown1 = 1u;
                short cardId = 123;
                byte unknown2 = 2;
                var level = Th08Converter.LevelPracticeWithTotal.Normal;
                var cardName = TestUtils.MakeRandomArray<byte>(0x30);
                var enemyName = TestUtils.MakeRandomArray<byte>(0x30);
                var comment = TestUtils.MakeRandomArray<byte>(0x80);
                var storyMaxBonuses = TestUtils.MakeRandomArray<uint>(13);
                var storyTrialCounts = TestUtils.MakeRandomArray<int>(12).Concat(new int[] { 0 }).ToArray();
                var storyClearCounts = TestUtils.MakeRandomArray<int>(13);
                var practiceMaxBonuses = TestUtils.MakeRandomArray<uint>(13);
                var practiceTrialCounts = TestUtils.MakeRandomArray<int>(12).Concat(new int[] { 0 }).ToArray();
                var practiceClearCounts = TestUtils.MakeRandomArray<int>(13);
                var unknown3 = 3u;
                var data = TestUtils.MakeByteArray(
                    unknown1,
                    (short)(cardId - 1),
                    unknown2,
                    (byte)level,
                    cardName,
                    enemyName,
                    comment,
                    storyMaxBonuses,
                    storyTrialCounts,
                    storyClearCounts,
                    practiceMaxBonuses,
                    practiceTrialCounts,
                    practiceClearCounts,
                    unknown3);

                var chapter = Th06ChapterWrapper<Th08Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var cardAttack = new Th08CardAttackWrapper(chapter);

                Assert.AreEqual(signature, cardAttack.Signature);
                Assert.AreEqual(size1, cardAttack.Size1);
                Assert.AreEqual(size2, cardAttack.Size2);
                CollectionAssert.AreEqual(data, cardAttack.Data.ToArray());
                Assert.AreEqual(data[0], cardAttack.FirstByteOfData);
                Assert.AreEqual(cardId, cardAttack.CardId);
                Assert.AreEqual(level, cardAttack.Level.Value);
                CollectionAssert.AreEqual(cardName, cardAttack.CardName.ToArray());
                CollectionAssert.AreEqual(enemyName, cardAttack.EnemyName.ToArray());
                CollectionAssert.AreEqual(comment, cardAttack.Comment.ToArray());
                CollectionAssert.AreEqual(storyMaxBonuses, cardAttack.StoryCareer.MaxBonuses.Values.ToArray());
                CollectionAssert.AreEqual(storyTrialCounts, cardAttack.StoryCareer.TrialCounts.Values.ToArray());
                CollectionAssert.AreEqual(storyClearCounts, cardAttack.StoryCareer.ClearCounts.Values.ToArray());
                CollectionAssert.AreEqual(practiceMaxBonuses, cardAttack.PracticeCareer.MaxBonuses.Values.ToArray());
                CollectionAssert.AreEqual(practiceTrialCounts, cardAttack.PracticeCareer.TrialCounts.Values.ToArray());
                CollectionAssert.AreEqual(practiceClearCounts, cardAttack.PracticeCareer.ClearCounts.Values.ToArray());
                Assert.IsFalse(cardAttack.HasTried.Value);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }
    }
}
