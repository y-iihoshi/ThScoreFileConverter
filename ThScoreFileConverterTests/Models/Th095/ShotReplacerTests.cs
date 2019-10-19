using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th095;

namespace ThScoreFileConverterTests.Models.Th095
{
    [TestClass]
    public class ShotReplacerTests
    {
        internal static IReadOnlyDictionary<(Level, int), (string, IBestShotHeader)> BestShots { get; } =
            new List<(string, IBestShotHeader header)>
            {
                (@"C:\path\to\output\bestshots\bs_02_3.png", BestShotHeaderTests.ValidStub),
            }.ToDictionary(element => (element.header.Level, (int)element.header.Scene));

        [TestMethod]
        public void ShotReplacerTest()
        {
            var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShotReplacerTestNullBestShots()
        {
            _ = new ShotReplacer(null, @"C:\path\to\output\");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ShotReplacerTestEmptyBestShots()
        {
            var bestshots = new Dictionary<(Level, int), (string, IBestShotHeader)>();
            var replacer = new ShotReplacer(bestshots, @"C:\path\to\output\");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotReplacerTestInvalidBestShotPath()
        {
            var bestshots = new List<(string, IBestShotHeader header)>
            {
                ("abcde", BestShotHeaderTests.ValidStub),
            }.ToDictionary(element => (element.header.Level, (int)element.header.Scene));
            var replacer = new ShotReplacer(bestshots, @"C:\path\to\output\");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotReplacerTestNullOutputFilePath()
        {
            var replacer = new ShotReplacer(BestShots, null);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotReplacerTestEmptyOutputFilePath()
        {
            var replacer = new ShotReplacer(BestShots, string.Empty);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotReplacerTestInvalidOutputFilePath()
        {
            var replacer = new ShotReplacer(BestShots, "abcde");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTest()
        {
            var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
            var expected = string.Join(Environment.NewLine, new string[]
            {
                @"<img src=""bestshots/bs_02_3.png"" alt=""ClearData: 6",
                @"Slow: 7.000000%",
                @"SpellName: abcde"" title=""ClearData: 6",
                @"Slow: 7.000000%",
                @"SpellName: abcde"" border=0>",
            });

            Assert.AreEqual(expected, replacer.Replace("%T95SHOT23"));
        }

        [TestMethod]
        public void ReplaceTestEmptyBestShots()
        {
            var bestshots = new Dictionary<(Level, int), (string, IBestShotHeader)>();
            var replacer = new ShotReplacer(bestshots, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOT23"));
        }

        [TestMethod]
        public void ReplaceTestInvalidBestShotPaths()
        {
            var bestshots = new List<(string, IBestShotHeader header)>
            {
                ("abcde", BestShotHeaderTests.ValidStub),
            }.ToDictionary(element => (element.header.Level, (int)element.header.Scene));
            var replacer = new ShotReplacer(bestshots, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOT23"));
        }

        [TestMethod]
        public void ReplaceTestNullOutputFilePath()
        {
            var replacer = new ShotReplacer(BestShots, null);
            Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOT23"));
        }

        [TestMethod]
        public void ReplaceTestEmptyOutputFilePath()
        {
            var replacer = new ShotReplacer(BestShots, string.Empty);
            Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOT23"));
        }

        [TestMethod]
        public void ReplaceTestInvalidOutputFilePath()
        {
            var replacer = new ShotReplacer(BestShots, "abcde");
            Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOT23"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentLevel()
        {
            var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOT13"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentScene()
        {
            var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T95SHOT22"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentSpellCard()
        {
            var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual("%T95SHOT99", replacer.Replace("%T95SHOT99"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual("%T95XXXX23", replacer.Replace("%T95XXXX23"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual("%T95SHOTY3", replacer.Replace("%T95SHOTY3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidScene()
        {
            var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual("%T95SHOT2X", replacer.Replace("%T95SHOT2X"));
        }
    }
}
