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

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th09ClearCountTests
    {
        internal static ClearCountStub ValidStub => new ClearCountStub()
        {
            Counts = Utils.GetEnumerator<Level>()
                .Select((level, index) => new { level, index })
                .ToDictionary(pair => pair.level, pair => pair.index)
        };

        internal static byte[] MakeByteArray(IClearCount clearCount)
            => TestUtils.MakeByteArray(clearCount.Counts.Values.ToArray(), 0u);

        internal static void Validate(IClearCount expected, in Th09ClearCountWrapper actual)
            => CollectionAssert.That.AreEqual(expected.Counts.Values, actual.Counts.Values);

        [TestMethod]
        public void Th09ClearCountReadFromTest() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var clearCount = Th09ClearCountWrapper.Create(MakeByteArray(stub));

            Validate(stub, clearCount);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th09ClearCountReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var clearCount = new Th09ClearCountWrapper();
            clearCount.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th09ClearCountReadFromTestShortenedTrials() => TestUtils.Wrap(() =>
        {
            var stub = new ClearCountStub(ValidStub);
            stub.Counts = stub.Counts.Where(pair => pair.Key == Level.Extra)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            _ = Th09ClearCountWrapper.Create(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void Th09ClearCountReadFromTestExceededTrials() => TestUtils.Wrap(() =>
        {
            var stub = new ClearCountStub(ValidStub);
            stub.Counts = stub.Counts.Concat(new Dictionary<Level, int>
            {
                { TestUtils.Cast<Level>(99), 99 },
            }).ToDictionary(pair => pair.Key, pair => pair.Value);

            var clearCount = Th09ClearCountWrapper.Create(MakeByteArray(stub));

            CollectionAssert.That.AreNotEqual(stub.Counts.Values, clearCount.Counts.Values);
            CollectionAssert.That.AreEqual(stub.Counts.Values.SkipLast(1), clearCount.Counts.Values);
        });
    }
}
