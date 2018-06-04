using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th075StatusTests
    {
        [TestMethod()]
        public void Th075StatusTest()
        {
            try
            {
                var status = new Th075StatusWrapper();

                Assert.IsNull(status.LastName);
                Assert.IsNull(status.ArcadeScores);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        internal static void ReadFromTestHelper(Th075StatusWrapper status, byte[] array)
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    status.ReadFrom(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }

        internal static bool IsValidRange(int index, int offset)
            => (offset <= index) && (index < offset + 11);

        internal static int[] GetExpectedScores(IEnumerable<int> scores, int offset)
            => scores.Where((_, index) => IsValidRange(index, offset)).Select(score => score - 10).ToArray();

        [TestMethod()]
        public void ReadFromTest()
        {
            try
            {
                var status = new Th075StatusWrapper();
                var lastName = new byte[] { 15, 37, 26, 50, 30, 43, 53, 103 };
                var arcadeScores = TestUtils.MakeRandomArray<int>(15 * 15);
                var unknown = TestUtils.MakeRandomArray<byte>(0x128);

                ReadFromTestHelper(status, TestUtils.MakeByteArray(lastName, arcadeScores, unknown));

                Assert.AreEqual("Player1 ", status.LastName);
                CollectionAssert.AreEqual(
                    GetExpectedScores(arcadeScores, 0), status.ArcadeScores[Th075Converter.Chara.Reimu].Values);
                CollectionAssert.AreEqual(
                    GetExpectedScores(arcadeScores, 15), status.ArcadeScores[Th075Converter.Chara.Marisa].Values);
                CollectionAssert.AreEqual(
                    GetExpectedScores(arcadeScores, 30), status.ArcadeScores[Th075Converter.Chara.Sakuya].Values);
                CollectionAssert.AreEqual(
                    GetExpectedScores(arcadeScores, 45), status.ArcadeScores[Th075Converter.Chara.Alice].Values);
                CollectionAssert.AreEqual(
                    GetExpectedScores(arcadeScores, 60), status.ArcadeScores[Th075Converter.Chara.Patchouli].Values);
                CollectionAssert.AreEqual(
                    GetExpectedScores(arcadeScores, 75), status.ArcadeScores[Th075Converter.Chara.Youmu].Values);
                CollectionAssert.AreEqual(
                    GetExpectedScores(arcadeScores, 90), status.ArcadeScores[Th075Converter.Chara.Remilia].Values);
                CollectionAssert.AreEqual(
                    GetExpectedScores(arcadeScores, 105), status.ArcadeScores[Th075Converter.Chara.Yuyuko].Values);
                CollectionAssert.AreEqual(
                    GetExpectedScores(arcadeScores, 120), status.ArcadeScores[Th075Converter.Chara.Yukari].Values);
                CollectionAssert.AreEqual(
                    GetExpectedScores(arcadeScores, 135), status.ArcadeScores[Th075Converter.Chara.Suika].Values);
                CollectionAssert.AreEqual(
                    GetExpectedScores(arcadeScores, 150), status.ArcadeScores[Th075Converter.Chara.Meiling].Values);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull()
        {
            try
            {
                var status = new Th075StatusWrapper();
                status.ReadFrom(null);
                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortenedName()
        {
            try
            {
                var status = new Th075StatusWrapper();
                var lastName = new byte[] { 15, 37, 26, 50, 30, 43, 53 };
                var arcadeScores = TestUtils.MakeRandomArray<int>(15 * 15);
                var unknown = TestUtils.MakeRandomArray<byte>(0x128);

                ReadFromTestHelper(status, TestUtils.MakeByteArray(lastName, arcadeScores, unknown));

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        public void ReadFromTestExceededName()
        {
            try
            {
                var status = new Th075StatusWrapper();
                var lastName = new byte[] { 15, 37, 26, 50, 30, 43, 53, 103, 1 };
                var arcadeScores = TestUtils.MakeRandomArray<int>(15 * 15);
                var unknown = TestUtils.MakeRandomArray<byte>(0x128);

                ReadFromTestHelper(status, TestUtils.MakeByteArray(lastName, arcadeScores, unknown));

                Assert.AreEqual("Player1 ", status.LastName);
                CollectionAssert.AreNotEqual(
                    arcadeScores.Take(11).Select(score => score - 10).ToArray(),
                    status.ArcadeScores[Th075Converter.Chara.Reimu].Values);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortenedArcadeScores()
        {
            try
            {
                var status = new Th075StatusWrapper();
                var lastName = new byte[] { 15, 37, 26, 50, 30, 43, 53, 103 };
                var arcadeScores = TestUtils.MakeRandomArray<int>(15 * 15 - 1);
                var unknown = TestUtils.MakeRandomArray<byte>(0x128);

                ReadFromTestHelper(status, TestUtils.MakeByteArray(lastName, arcadeScores, unknown));

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [TestMethod()]
        public void ReadFromTestExceededArcadeScores()
        {
            try
            {
                var status = new Th075StatusWrapper();
                var lastName = new byte[] { 15, 37, 26, 50, 30, 43, 53, 103 };
                var arcadeScores = TestUtils.MakeRandomArray<int>(15 * 15 + 1);
                var unknown = TestUtils.MakeRandomArray<byte>(0x128);

                ReadFromTestHelper(status, TestUtils.MakeByteArray(lastName, arcadeScores, unknown));

                Assert.AreEqual("Player1 ", status.LastName);
                CollectionAssert.AreEqual(
                    GetExpectedScores(arcadeScores, 0), status.ArcadeScores[Th075Converter.Chara.Reimu].Values);
                CollectionAssert.AreEqual(
                    GetExpectedScores(arcadeScores, 15), status.ArcadeScores[Th075Converter.Chara.Marisa].Values);
                CollectionAssert.AreEqual(
                    GetExpectedScores(arcadeScores, 30), status.ArcadeScores[Th075Converter.Chara.Sakuya].Values);
                CollectionAssert.AreEqual(
                    GetExpectedScores(arcadeScores, 45), status.ArcadeScores[Th075Converter.Chara.Alice].Values);
                CollectionAssert.AreEqual(
                    GetExpectedScores(arcadeScores, 60), status.ArcadeScores[Th075Converter.Chara.Patchouli].Values);
                CollectionAssert.AreEqual(
                    GetExpectedScores(arcadeScores, 75), status.ArcadeScores[Th075Converter.Chara.Youmu].Values);
                CollectionAssert.AreEqual(
                    GetExpectedScores(arcadeScores, 90), status.ArcadeScores[Th075Converter.Chara.Remilia].Values);
                CollectionAssert.AreEqual(
                    GetExpectedScores(arcadeScores, 105), status.ArcadeScores[Th075Converter.Chara.Yuyuko].Values);
                CollectionAssert.AreEqual(
                    GetExpectedScores(arcadeScores, 120), status.ArcadeScores[Th075Converter.Chara.Yukari].Values);
                CollectionAssert.AreEqual(
                    GetExpectedScores(arcadeScores, 135), status.ArcadeScores[Th075Converter.Chara.Suika].Values);
                CollectionAssert.AreEqual(
                    GetExpectedScores(arcadeScores, 150), status.ArcadeScores[Th075Converter.Chara.Meiling].Values);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }
    }
}
