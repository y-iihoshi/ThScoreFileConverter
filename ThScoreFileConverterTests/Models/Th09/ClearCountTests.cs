using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th09;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th09.Stubs;

namespace ThScoreFileConverterTests.Models.Th09
{
    [TestClass]
    public class ClearCountTests
    {
        internal static ClearCountStub ValidStub { get; } = new ClearCountStub()
        {
            Counts = Utils.GetEnumerator<Level>()
                .Select((level, index) => new { level, index })
                .ToDictionary(pair => pair.level, pair => pair.index)
        };

        internal static byte[] MakeByteArray(IClearCount clearCount)
            => TestUtils.MakeByteArray(clearCount.Counts.Values.ToArray(), 0u);

        internal static void Validate(IClearCount expected, IClearCount actual)
            => CollectionAssert.That.AreEqual(expected.Counts.Values, actual.Counts.Values);

        [TestMethod]
        public void ClearCountTest()
        {
            var stub = new ClearCountStub();

            var clearCount = new ClearCount();

            Validate(stub, clearCount);
        }

        [TestMethod]
        public void ReadFromTest()
        {
            var stub = ValidStub;

            var clearCount = TestUtils.Create<ClearCount>(MakeByteArray(stub));

            Validate(stub, clearCount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull()
        {
            var clearCount = new ClearCount();
            clearCount.ReadFrom(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortenedTrials()
        {
            var stub = new ClearCountStub(ValidStub);
            stub.Counts = stub.Counts.Where(pair => pair.Key == Level.Extra)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            _ = TestUtils.Create<ClearCount>(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ReadFromTestExceededTrials()
        {
            var stub = new ClearCountStub(ValidStub);
            stub.Counts = stub.Counts.Concat(new Dictionary<Level, int>
            {
                { TestUtils.Cast<Level>(99), 99 },
            }).ToDictionary(pair => pair.Key, pair => pair.Value);

            var clearCount = TestUtils.Create<ClearCount>(MakeByteArray(stub));

            CollectionAssert.That.AreNotEqual(stub.Counts.Values, clearCount.Counts.Values);
            CollectionAssert.That.AreEqual(stub.Counts.Values.SkipLast(1), clearCount.Counts.Values);
        }
    }
}
