using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    public enum Protagonist
    {
        [EnumAltName("RM", LongName = "博麗 霊夢")] Reimu,
        [EnumAltName("MR", LongName = "霧雨 魔理沙")] Marisa
    }

    public enum UnnamedCharacter
    {
        [EnumAltName("Dai")] 大妖精,
        [EnumAltName("Koa")] 小悪魔,
        [EnumAltName("Tokiko")] 名無しの本読み妖怪
    }

    [TestClass]
    public class EnumExtensionsTests
    {
        [TestMethod]
        public void ToShortNameTest()
        {
            Assert.AreEqual(string.Empty, DayOfWeek.Sunday.ToShortName());
            Assert.AreEqual("RM", Protagonist.Reimu.ToShortName());
            Assert.AreEqual("MR", Protagonist.Marisa.ToShortName());
            Assert.AreEqual("Dai", UnnamedCharacter.大妖精.ToShortName());
            Assert.AreEqual("Koa", UnnamedCharacter.小悪魔.ToShortName());
            Assert.AreEqual("Tokiko", UnnamedCharacter.名無しの本読み妖怪.ToShortName());
        }

        [TestMethod]
        public void ToLongNameTest()
        {
            Assert.AreEqual(string.Empty, DayOfWeek.Sunday.ToLongName());
            Assert.AreEqual("博麗 霊夢", Protagonist.Reimu.ToLongName());
            Assert.AreEqual("霧雨 魔理沙", Protagonist.Marisa.ToLongName());
            Assert.AreEqual(string.Empty, UnnamedCharacter.大妖精.ToLongName());
            Assert.AreEqual(string.Empty, UnnamedCharacter.小悪魔.ToLongName());
            Assert.AreEqual(string.Empty, UnnamedCharacter.名無しの本読み妖怪.ToLongName());
        }
    }
}
