using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Th123
{
    [TestClass]
    public class SpellCardResultTests
    {
        [TestMethod]
        public void SpellCardResultTest()
            => Th105.SpellCardResultTests.SpellCardResultTestHelper<Th123Converter.Chara, Th123Converter.Level>();

        [TestMethod]
        public void ReadFromTest()
            => Th105.SpellCardResultTests.ReadFromTestHelper<Th123Converter.Chara, Th123Converter.Level>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull()
            => Th105.SpellCardResultTests.ReadFromTestNullHelper<Th123Converter.Chara, Th123Converter.Level>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortened()
            => Th105.SpellCardResultTests.ReadFromTestShortenedHelper<Th123Converter.Chara, Th123Converter.Level>();

        [TestMethod]
        public void ReadFromTestExceeded()
            => Th105.SpellCardResultTests.ReadFromTestExceededHelper<Th123Converter.Chara, Th123Converter.Level>();
    }
}
