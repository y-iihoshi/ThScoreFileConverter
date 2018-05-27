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
    public class Th06ClearDataTests
    {
        [TestMethod()]
        public void Th06ClearDataTestChapter()
        {
            try
            {
                var signature = "CLRD";
                short size1 = 0x18;
                short size2 = 0x18;
                var unknown = 0u;
                var storyFlags = TestUtils.MakeRandomArray<byte>(5);
                var practiceFlags = TestUtils.MakeRandomArray<byte>(5);
                var chara = Th06Converter.Chara.ReimuB;
                var data = TestUtils.MakeByteArray(
                    unknown,
                    storyFlags,
                    practiceFlags,
                    (short)chara);

                var chapter = Th06ChapterWrapper<Th06Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var clearData = new Th06ClearDataWrapper(chapter);

                Assert.AreEqual(signature, clearData.Signature);
                Assert.AreEqual(size1, clearData.Size1);
                Assert.AreEqual(size2, clearData.Size2);
                CollectionAssert.AreEqual(data, clearData.Data.ToArray());
                Assert.AreEqual(data[0], clearData.FirstByteOfData);
                CollectionAssert.AreEqual(storyFlags, clearData.StoryFlags.Values.ToArray());
                CollectionAssert.AreEqual(practiceFlags, clearData.PracticeFlags.Values.ToArray());
                Assert.AreEqual(chara, clearData.Chara);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th06ClearDataTestNullChapter()
        {
            try
            {
                var clearData = new Th06ClearDataWrapper(null);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th06ClearDataTestInvalidSignature()
        {
            try
            {
                var signature = "clrd";
                short size1 = 0x18;
                short size2 = 0x18;
                var unknown = 0u;
                var storyFlags = TestUtils.MakeRandomArray<byte>(5);
                var practiceFlags = TestUtils.MakeRandomArray<byte>(5);
                var chara = Th06Converter.Chara.ReimuB;
                var data = TestUtils.MakeByteArray(
                    unknown,
                    storyFlags,
                    practiceFlags,
                    (short)chara);

                var chapter = Th06ChapterWrapper<Th06Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var clearData = new Th06ClearDataWrapper(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th06ClearDataTestInvalidSize1()
        {
            try
            {
                var signature = "CLRD";
                short size1 = 0x19;
                short size2 = 0x18;
                var unknown = 0u;
                var storyFlags = TestUtils.MakeRandomArray<byte>(5);
                var practiceFlags = TestUtils.MakeRandomArray<byte>(5);
                var chara = Th06Converter.Chara.ReimuB;
                var data = TestUtils.MakeByteArray(
                    unknown,
                    storyFlags,
                    practiceFlags,
                    (short)chara);

                var chapter = Th06ChapterWrapper<Th06Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var clearData = new Th06ClearDataWrapper(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "clearData")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th06ClearDataTestInvalidChara()
        {
            try
            {
                var signature = "CLRD";
                short size1 = 0x18;
                short size2 = 0x18;
                var unknown = 0u;
                var storyFlags = TestUtils.MakeRandomArray<byte>(5);
                var practiceFlags = TestUtils.MakeRandomArray<byte>(5);
                var chara = (Th06Converter.Chara)(-1);
                var data = TestUtils.MakeByteArray(
                    unknown,
                    storyFlags,
                    practiceFlags,
                    (short)chara);

                var chapter = Th06ChapterWrapper<Th06Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), size1, size2, data));
                var clearData = new Th06ClearDataWrapper(chapter);

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }
    }
}
