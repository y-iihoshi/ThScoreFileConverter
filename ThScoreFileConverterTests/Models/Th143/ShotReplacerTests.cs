using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th143;

namespace ThScoreFileConverterTests.Models.Th143
{
    [TestClass]
    public class ShotReplacerTests
    {
        internal static IReadOnlyDictionary<(Day, int), (string, IBestShotHeader)> BestShots { get; } =
            new List<(string, IBestShotHeader header)>
            {
                (@"C:\path\to\output\bestshots\sc02_03.png", BestShotHeaderTests.ValidStub),
            }.ToDictionary(element => (element.header.Day, (int)element.header.Scene));

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
            _ = new ShotReplacer(null!, @"C:\path\to\output\");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ShotReplacerTestEmptyBestShots()
        {
            var bestshots = new Dictionary<(Day, int), (string, IBestShotHeader)>();
            var replacer = new ShotReplacer(bestshots, @"C:\path\to\output\");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotReplacerTestInvalidBestShotPath()
        {
            var bestshots = new List<(string, IBestShotHeader header)>
            {
                ("abcde", BestShotHeaderTests.ValidStub),
            }.ToDictionary(element => (element.header.Day, (int)element.header.Scene));
            var replacer = new ShotReplacer(bestshots, @"C:\path\to\output\");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotReplacerTestNullOutputFilePath()
        {
            var replacer = new ShotReplacer(BestShots, null!);
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
            var expected = string.Join(" ", new string[]
            {
                @"<img src=""bestshots/sc02_03.png""",
                @"alt=""SpellName: 劈音「ピアッシングサークル」""",
                @"title=""SpellName: 劈音「ピアッシングサークル」"" border=0>",
            });

            Assert.AreEqual(expected, replacer.Replace("%T143SHOT23"));
        }

        [TestMethod]
        public void ReplaceTestScene10()
        {
            var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T143SHOTL0"));
        }

        [TestMethod]
        public void ReplaceTestEmptyBestShots()
        {
            var bestshots = new Dictionary<(Day, int), (string, IBestShotHeader)>();
            var replacer = new ShotReplacer(bestshots, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T143SHOT23"));
        }

        [TestMethod]
        public void ReplaceTestInvalidBestShotPaths()
        {
            var bestshots = new List<(string, IBestShotHeader header)>
            {
                ("abcde", BestShotHeaderTests.ValidStub),
            }.ToDictionary(element => (element.header.Day, (int)element.header.Scene));
            var replacer = new ShotReplacer(bestshots, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T143SHOT23"));
        }

        [TestMethod]
        public void ReplaceTestNullOutputFilePath()
        {
            var replacer = new ShotReplacer(BestShots, null!);
            Assert.AreEqual(string.Empty, replacer.Replace("%T143SHOT23"));
        }

        [TestMethod]
        public void ReplaceTestEmptyOutputFilePath()
        {
            var replacer = new ShotReplacer(BestShots, string.Empty);
            Assert.AreEqual(string.Empty, replacer.Replace("%T143SHOT23"));
        }

        [TestMethod]
        public void ReplaceTestInvalidOutputFilePath()
        {
            var replacer = new ShotReplacer(BestShots, "abcde");
            Assert.AreEqual(string.Empty, replacer.Replace("%T143SHOT23"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentDay()
        {
            var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T143SHOT13"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentScene()
        {
            var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T143SHOT22"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentSpellCard()
        {
            var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual("%T143SHOT17", replacer.Replace("%T143SHOT17"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual("%T143XXXX23", replacer.Replace("%T143XXXX23"));
        }

        [TestMethod]
        public void ReplaceTestInvalidDay()
        {
            var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual("%T143SHOTX3", replacer.Replace("%T143SHOTX3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidScene()
        {
            var replacer = new ShotReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual("%T143SHOT2X", replacer.Replace("%T143SHOT2X"));
        }
    }
}
