using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th123;

namespace ThScoreFileConverterTests.Models.Th123
{
    [TestClass]
    public class ClearDataTests
    {
        [TestMethod]
        public void ClearDataTest() => Th105.ClearDataTests.ClearDataTestHelper<Chara>();

        [TestMethod]
        public void ReadFromTest() => Th105.ClearDataTests.ReadFromTestHelper<Chara>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => Th105.ClearDataTests.ReadFromTestNullHelper<Chara>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortened() => Th105.ClearDataTests.ReadFromTestShortenedHelper<Chara>();

        [TestMethod]
        public void ReadFromTestExceeded() => Th105.ClearDataTests.ReadFromTestExceededHelper<Chara>();

        [TestMethod]
        public void ReadFromTestDuplicated() => Th105.ClearDataTests.ReadFromTestDuplicatedHelper<Chara>();
    }
}
