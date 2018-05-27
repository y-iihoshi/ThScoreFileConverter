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
    public class Th06CardAttackTests
    {
        [TestMethod()]
        public void Th06CardAttackTestChapter()
        {
            try
            {
                var signature = "CATK";
                short size1 = 0x40;
                short size2 = 0x40;
                var unknown1 = TestUtils.MakeRandomArray<byte>(8);
                short cardId = 123;
                var unknown2 = TestUtils.MakeRandomArray<byte>(6);
                var cardName = TestUtils.MakeRandomArray<byte>(0x24);
                ushort trialCount = 789;
                ushort clearCount = 456;
                var data = TestUtils.MakeByteArray(
                    unknown1,
                    (short)(cardId - 1),
                    unknown2,
                    cardName,
                    trialCount,
                    clearCount);

                var chapter = Th06ChapterWrapper<Th06Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var cardAttack = new Th06CardAttackWrapper(chapter);

                Assert.AreEqual(signature, cardAttack.Signature);
                Assert.AreEqual(size1, cardAttack.Size1);
                Assert.AreEqual(size2, cardAttack.Size2);
                CollectionAssert.AreEqual(data, cardAttack.Data.ToArray());
                Assert.AreEqual(data[0], cardAttack.FirstByteOfData);
                Assert.AreEqual(cardId, cardAttack.CardId);
                CollectionAssert.AreEqual(cardName, cardAttack.CardName.ToArray());
                Assert.AreEqual(trialCount, cardAttack.TrialCount);
                Assert.AreEqual(clearCount, cardAttack.ClearCount);
                Assert.IsTrue(cardAttack.HasTried().Value);
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
        public void Th06CardAttackTestNullChapter()
        {
            try
            {
                var cardAttack = new Th06CardAttackWrapper(null);

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
        public void Th06CardAttackTestInvalidSignature()
        {
            try
            {
                var signature = "catk";
                short size1 = 0x40;
                short size2 = 0x40;
                var unknown1 = TestUtils.MakeRandomArray<byte>(8);
                short cardId = 123;
                var unknown2 = TestUtils.MakeRandomArray<byte>(6);
                var cardName = TestUtils.MakeRandomArray<byte>(0x24);
                ushort trialCount = 789;
                ushort clearCount = 456;
                var data = TestUtils.MakeByteArray(
                    unknown1,
                    (short)(cardId - 1),
                    unknown2,
                    cardName,
                    trialCount,
                    clearCount);

                var chapter = Th06ChapterWrapper<Th06Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var cardAttack = new Th06CardAttackWrapper(chapter);

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
        public void Th06CardAttackTestInvalidSize1()
        {
            try
            {
                var signature = "CATK";
                short size1 = 0x41;
                short size2 = 0x40;
                var unknown1 = TestUtils.MakeRandomArray<byte>(8);
                short cardId = 123;
                var unknown2 = TestUtils.MakeRandomArray<byte>(6);
                var cardName = TestUtils.MakeRandomArray<byte>(0x24);
                ushort trialCount = 789;
                ushort clearCount = 456;
                var data = TestUtils.MakeByteArray(
                    unknown1,
                    (short)(cardId - 1),
                    unknown2,
                    cardName,
                    trialCount,
                    clearCount);

                var chapter = Th06ChapterWrapper<Th06Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var cardAttack = new Th06CardAttackWrapper(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        public void Th06CardAttackTestNotTried()
        {
            try
            {
                var signature = "CATK";
                short size1 = 0x40;
                short size2 = 0x40;
                var unknown1 = TestUtils.MakeRandomArray<byte>(8);
                short cardId = 123;
                var unknown2 = TestUtils.MakeRandomArray<byte>(6);
                var cardName = TestUtils.MakeRandomArray<byte>(0x24);
                ushort trialCount = 0;
                ushort clearCount = 456;
                var data = TestUtils.MakeByteArray(
                    unknown1,
                    (short)(cardId - 1),
                    unknown2,
                    cardName,
                    trialCount,
                    clearCount);

                var chapter = Th06ChapterWrapper<Th06Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var cardAttack = new Th06CardAttackWrapper(chapter);

                Assert.AreEqual(signature, cardAttack.Signature);
                Assert.AreEqual(size1, cardAttack.Size1);
                Assert.AreEqual(size2, cardAttack.Size2);
                CollectionAssert.AreEqual(data, cardAttack.Data.ToArray());
                Assert.AreEqual(data[0], cardAttack.FirstByteOfData);
                Assert.AreEqual(cardId, cardAttack.CardId);
                CollectionAssert.AreEqual(cardName, cardAttack.CardName.ToArray());
                Assert.AreEqual(trialCount, cardAttack.TrialCount);
                Assert.AreEqual(clearCount, cardAttack.ClearCount);
                Assert.IsFalse(cardAttack.HasTried().Value);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }
    }
}
