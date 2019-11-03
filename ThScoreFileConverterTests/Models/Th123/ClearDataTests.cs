using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Th123
{
    [TestClass]
    public class ClearDataTests
    {
        [TestMethod]
        public void ClearDataTest()
            => Th105.ClearDataTests.ClearDataTestHelper<Th123Converter.Chara, Th123Converter.Level>();

        [TestMethod]
        public void ReadFromTest()
            => Th105.ClearDataTests.ReadFromTestHelper<Th123Converter.Chara, Th123Converter.Level>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull()
            => Th105.ClearDataTests.ReadFromTestNullHelper<Th123Converter.Chara, Th123Converter.Level>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortened()
            => Th105.ClearDataTests.ReadFromTestShortenedHelper<Th123Converter.Chara, Th123Converter.Level>();

        [TestMethod]
        public void ReadFromTestExceeded()
            => Th105.ClearDataTests.ReadFromTestExceededHelper<Th123Converter.Chara, Th123Converter.Level>();

        [TestMethod]
        public void ReadFromTestDuplicated()
            => Th105.ClearDataTests.ReadFromTestDuplicatedHelper<Th123Converter.Chara, Th123Converter.Level>();
    }
}
