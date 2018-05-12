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
    public class Th07CardAttackTests
    {
        [TestMethod()]
        public void Th07CardAttackTestChapter()
        {
            try
            {
                var signature = "CATK";
                short size1 = 0x78;
                short size2 = 0x78;
                var unknown1 = 1u;
                var maxBonuses = TestUtils.MakeRandomArray<uint>(7);
                short cardId = 123;
                byte unknown2 = 2;
                var cardName = TestUtils.MakeRandomArray<byte>(0x30);
                byte unknown3 = 2;
                var trialCounts = TestUtils.MakeRandomArray<ushort>(6).Concat(new ushort[] { 1 }).ToArray();
                var clearCounts = TestUtils.MakeRandomArray<ushort>(7);
                var data = TestUtils.MakeByteArray(
                    unknown1,
                    maxBonuses,
                    (short)(cardId - 1),
                    unknown2,
                    cardName,
                    unknown3,
                    trialCounts,
                    clearCounts);

                var chapter = Th06ChapterWrapper<Th07Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var cardAttack = new Th07CardAttackWrapper(chapter);

                Assert.AreEqual(signature, cardAttack.Signature);
                Assert.AreEqual(size1, cardAttack.Size1);
                Assert.AreEqual(size2, cardAttack.Size2);
                CollectionAssert.AreEqual(data, cardAttack.Data.ToArray());
                Assert.AreEqual(data[0], cardAttack.FirstByteOfData);
                CollectionAssert.AreEqual(maxBonuses, cardAttack.MaxBonuses.Values.ToArray());
                Assert.AreEqual(cardId, cardAttack.CardId);
                CollectionAssert.AreEqual(cardName, cardAttack.CardName.ToArray());
                CollectionAssert.AreEqual(trialCounts, cardAttack.TrialCounts.Values.ToArray());
                CollectionAssert.AreEqual(clearCounts, cardAttack.ClearCounts.Values.ToArray());
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
        public void Th07CardAttackTestNullChapter()
        {
            try
            {
                var cardAttack = new Th07CardAttackWrapper(null);

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
        public void Th07CardAttackTestInvalidSignature()
        {
            try
            {
                var signature = "catk";
                short size1 = 0x78;
                short size2 = 0x78;
                var unknown1 = 1u;
                var maxBonuses = TestUtils.MakeRandomArray<uint>(7);
                short cardId = 123;
                byte unknown2 = 2;
                var cardName = TestUtils.MakeRandomArray<byte>(0x30);
                byte unknown3 = 2;
                var trialCounts = TestUtils.MakeRandomArray<ushort>(7);
                var clearCounts = TestUtils.MakeRandomArray<ushort>(7);
                var data = TestUtils.MakeByteArray(
                    unknown1,
                    maxBonuses,
                    (short)(cardId - 1),
                    unknown2,
                    cardName,
                    unknown3,
                    trialCounts,
                    clearCounts);

                var chapter = Th06ChapterWrapper<Th07Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var cardAttack = new Th07CardAttackWrapper(chapter);

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
        public void Th07CardAttackTestInvalidSize1()
        {
            try
            {
                var signature = "CATK";
                short size1 = 0x79;
                short size2 = 0x78;
                var unknown1 = 1u;
                var maxBonuses = TestUtils.MakeRandomArray<uint>(7);
                short cardId = 123;
                byte unknown2 = 2;
                var cardName = TestUtils.MakeRandomArray<byte>(0x30);
                byte unknown3 = 2;
                var trialCounts = TestUtils.MakeRandomArray<ushort>(7);
                var clearCounts = TestUtils.MakeRandomArray<ushort>(7);
                var data = TestUtils.MakeByteArray(
                    unknown1,
                    maxBonuses,
                    (short)(cardId - 1),
                    unknown2,
                    cardName,
                    unknown3,
                    trialCounts,
                    clearCounts);

                var chapter = Th06ChapterWrapper<Th07Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var cardAttack = new Th07CardAttackWrapper(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        public void Th07CardAttackTestNotTried()
        {
            try
            {
                var signature = "CATK";
                short size1 = 0x78;
                short size2 = 0x78;
                var unknown1 = 1u;
                var maxBonuses = TestUtils.MakeRandomArray<uint>(7);
                short cardId = 123;
                byte unknown2 = 2;
                var cardName = TestUtils.MakeRandomArray<byte>(0x30);
                byte unknown3 = 2;
                var trialCounts = TestUtils.MakeRandomArray<ushort>(6).Concat(new ushort[] { 0 }).ToArray();
                var clearCounts = TestUtils.MakeRandomArray<ushort>(7);
                var data = TestUtils.MakeByteArray(
                    unknown1,
                    maxBonuses,
                    (short)(cardId - 1),
                    unknown2,
                    cardName,
                    unknown3,
                    trialCounts,
                    clearCounts);

                var chapter = Th06ChapterWrapper<Th07Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var cardAttack = new Th07CardAttackWrapper(chapter);

                Assert.AreEqual(signature, cardAttack.Signature);
                Assert.AreEqual(size1, cardAttack.Size1);
                Assert.AreEqual(size2, cardAttack.Size2);
                CollectionAssert.AreEqual(data, cardAttack.Data.ToArray());
                Assert.AreEqual(data[0], cardAttack.FirstByteOfData);
                CollectionAssert.AreEqual(maxBonuses, cardAttack.MaxBonuses.Values.ToArray());
                Assert.AreEqual(cardId, cardAttack.CardId);
                CollectionAssert.AreEqual(cardName, cardAttack.CardName.ToArray());
                CollectionAssert.AreEqual(trialCounts, cardAttack.TrialCounts.Values.ToArray());
                CollectionAssert.AreEqual(clearCounts, cardAttack.ClearCounts.Values.ToArray());
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
