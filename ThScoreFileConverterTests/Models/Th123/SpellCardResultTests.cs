using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th123;
using Level = ThScoreFileConverter.Models.Th105.Level;

namespace ThScoreFileConverterTests.Models.Th123
{
    [TestClass]
    public class SpellCardResultTests
    {
        [TestMethod]
        public void SpellCardResultTest() => Th105.SpellCardResultTests.SpellCardResultTestHelper<Chara, Level>();

        [TestMethod]
        public void ReadFromTest() => Th105.SpellCardResultTests.ReadFromTestHelper<Chara, Level>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => Th105.SpellCardResultTests.ReadFromTestNullHelper<Chara, Level>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortened() => Th105.SpellCardResultTests.ReadFromTestShortenedHelper<Chara, Level>();

        [TestMethod]
        public void ReadFromTestExceeded() => Th105.SpellCardResultTests.ReadFromTestExceededHelper<Chara, Level>();
    }
}
