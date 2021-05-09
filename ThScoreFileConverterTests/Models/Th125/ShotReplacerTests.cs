using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th125;
using INumberFormatter = ThScoreFileConverter.Models.INumberFormatter;

namespace ThScoreFileConverterTests.Models.Th125
{
    [TestClass]
    public class ShotReplacerTests
    {
        internal static IReadOnlyDictionary<(Chara, Level, int), (string, IBestShotHeader)> BestShots { get; } =
            new List<(string, IBestShotHeader header)>
            {
                (@"C:\path\to\output\bestshots\bs2_02_3.png", BestShotHeaderTests.MockBestShotHeader().Object),
            }.ToDictionary(element => (Chara.Hatate, element.header.Level, (int)element.header.Scene));

        private static Mock<INumberFormatter> MockNumberFormatter()
        {
            var mock = new Mock<INumberFormatter>();
            _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
                .Returns((object value) => "invoked: " + value.ToString());
            _ = mock.Setup(formatter => formatter.FormatPercent(It.IsAny<double>(), It.IsAny<int>()))
                .Returns((double value, int precision) => "invoked: " + value.ToString($"F{precision}") + "%");
            return mock;
        }

        [TestMethod]
        public void ShotReplacerTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotReplacerTestEmptyBestShots()
        {
            var bestshots = ImmutableDictionary<(Chara, Level, int), (string, IBestShotHeader)>.Empty;
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotReplacer(bestshots, formatterMock.Object, @"C:\path\to\output\");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotReplacerTestInvalidBestShotPath()
        {
            var bestshots = new List<(string, IBestShotHeader header)>
            {
                ("abcde", BestShotHeaderTests.MockBestShotHeader().Object),
            }.ToDictionary(element => (Chara.Hatate, element.header.Level, (int)element.header.Scene));
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotReplacer(bestshots, formatterMock.Object, @"C:\path\to\output\");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotReplacerTestEmptyOutputFilePath()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotReplacer(BestShots, formatterMock.Object, string.Empty);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotReplacerTestInvalidOutputFilePath()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotReplacer(BestShots, formatterMock.Object, "abcde");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
            var expected = string.Join(Environment.NewLine, new string[]
            {
                @"<img src=""bestshots/bs2_02_3.png"" alt=""ClearData: invoked: 13",
                @"Slow: invoked: 11.000000%",
                @"SpellName: abcde"" title=""ClearData: invoked: 13",
                @"Slow: invoked: 11.000000%",
                @"SpellName: abcde"" border=0>",
            });

            Assert.AreEqual(expected, replacer.Replace("%T125SHOTH23"));
        }

        [TestMethod]
        public void ReplaceTestEmptyBestShots()
        {
            var bestshots = ImmutableDictionary<(Chara, Level, int), (string, IBestShotHeader)>.Empty;
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotReplacer(bestshots, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTH23"));
        }

        [TestMethod]
        public void ReplaceTestInvalidBestShotPaths()
        {
            var bestshots = new List<(string, IBestShotHeader header)>
            {
                ("abcde", BestShotHeaderTests.MockBestShotHeader().Object),
            }.ToDictionary(element => (Chara.Hatate, element.header.Level, (int)element.header.Scene));
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotReplacer(bestshots, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTH23"));
        }

        [TestMethod]
        public void ReplaceTestEmptyOutputFilePath()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotReplacer(BestShots, formatterMock.Object, string.Empty);
            Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTH23"));
        }

        [TestMethod]
        public void ReplaceTestInvalidOutputFilePath()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotReplacer(BestShots, formatterMock.Object, "abcde");
            Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTH23"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTA23"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentLevel()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTH13"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentScene()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T125SHOTH22"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentSpellCard()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual("%T125SHOTH99", replacer.Replace("%T125SHOTH99"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual("%T125XXXXH23", replacer.Replace("%T125XXXXH23"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual("%T125SHOTX23", replacer.Replace("%T125SHOTX23"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual("%T125SHOTHY3", replacer.Replace("%T125SHOTHY3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidScene()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual("%T125SHOTH2X", replacer.Replace("%T125SHOTH2X"));
        }
    }
}
