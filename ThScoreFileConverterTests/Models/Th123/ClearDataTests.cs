using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th123;
using Level = ThScoreFileConverter.Models.Th105.Level;

namespace ThScoreFileConverterTests.Models.Th123
{
    [TestClass]
    public class ClearDataTests
    {
        [TestMethod]
        public void ClearDataTest() => Th105.ClearDataTests.ClearDataTestHelper<Chara, Level>();

        [TestMethod]
        public void ReadFromTest() => Th105.ClearDataTests.ReadFromTestHelper<Chara, Level>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => Th105.ClearDataTests.ReadFromTestNullHelper<Chara, Level>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortened() => Th105.ClearDataTests.ReadFromTestShortenedHelper<Chara, Level>();

        [TestMethod]
        public void ReadFromTestExceeded() => Th105.ClearDataTests.ReadFromTestExceededHelper<Chara, Level>();

        [TestMethod]
        public void ReadFromTestDuplicated() => Th105.ClearDataTests.ReadFromTestDuplicatedHelper<Chara, Level>();
    }
}
