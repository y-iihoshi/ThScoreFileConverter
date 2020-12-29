using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models.Th165;

namespace ThScoreFileConverterTests.Models.Th165
{
    [TestClass]
    public class ShotExReplacerTests
    {
        internal static IReadOnlyDictionary<(Day, int), (string, IBestShotHeader)> BestShots { get; } =
            new List<(string, IBestShotHeader header)>
            {
                (@"C:\path\to\output\bestshots\bs02_03.png", BestShotHeaderTests.MockBestShotHeader().Object),
            }.ToDictionary(element => (element.header.Weekday, (int)element.header.Dream));

        [TestMethod]
        public void ShotExReplacerTest()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotExReplacerTestNullBestShots()
        {
            _ = Assert.ThrowsException<ArgumentNullException>(
                () => _ = new ShotExReplacer(null!, @"C:\path\to\output\"));
        }

        [TestMethod]
        public void ShotExReplacerTestEmptyBestShots()
        {
            var bestshots = new Dictionary<(Day, int), (string, IBestShotHeader)>();
            var replacer = new ShotExReplacer(bestshots, @"C:\path\to\output\");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotExReplacerTestInvalidBestShotPath()
        {
            var bestshots = new List<(string, IBestShotHeader header)>
            {
                ("abcde", BestShotHeaderTests.MockBestShotHeader().Object),
            }.ToDictionary(element => (element.header.Weekday, (int)element.header.Dream));
            var replacer = new ShotExReplacer(bestshots, @"C:\path\to\output\");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotExReplacerTestNullOutputFilePath()
        {
            var replacer = new ShotExReplacer(BestShots, null!);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotExReplacerTestEmptyOutputFilePath()
        {
            var replacer = new ShotExReplacer(BestShots, string.Empty);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotExReplacerTestInvalidOutputFilePath()
        {
            var replacer = new ShotExReplacer(BestShots, "abcde");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestPath()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual(@"bestshots/bs02_03.png", replacer.Replace("%T165SHOTEX0231"));
        }

        [TestMethod]
        public void ReplaceTestWidth()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual("4", replacer.Replace("%T165SHOTEX0232"));
        }

        [TestMethod]
        public void ReplaceTestHeight()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual("5", replacer.Replace("%T165SHOTEX0233"));
        }

        [TestMethod]
        public void ReplaceTestDateTime()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            var expected = DateTimeHelper.GetString(11);
            Assert.AreEqual(expected, replacer.Replace("%T165SHOTEX0234"));
        }

        [TestMethod]
        public void ReplaceTestHashtags()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual(string.Join(Environment.NewLine, new[]
            {
                "＃敵が見切れてる",
                "＃敵を収めたよ",
                "＃敵がど真ん中",
                "＃敵が丸見えｗ",
                "＃動物園！",
            }), replacer.Replace("%T165SHOTEX0235"));
        }

        [TestMethod]
        public void ReplaceTestNumViews()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual("19", replacer.Replace("%T165SHOTEX0236"));
        }

        [TestMethod]
        public void ReplaceTestNumLikes()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual("20", replacer.Replace("%T165SHOTEX0237"));
        }

        [TestMethod]
        public void ReplaceTestNumFavs()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual("21", replacer.Replace("%T165SHOTEX0238"));
        }

        [TestMethod]
        public void ReplaceTestScore()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual("13", replacer.Replace("%T165SHOTEX0239"));
        }

        [TestMethod]
        public void ReplaceTestEmptyBestShots()
        {
            var bestshots = new Dictionary<(Day, int), (string, IBestShotHeader)>();
            var replacer = new ShotExReplacer(bestshots, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOTEX0231"));
            Assert.AreEqual("0", replacer.Replace("%T165SHOTEX0232"));
            Assert.AreEqual("0", replacer.Replace("%T165SHOTEX0233"));
            Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T165SHOTEX0234"));
            Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOTEX0235"));
            Assert.AreEqual("0", replacer.Replace("%T165SHOTEX0236"));
            Assert.AreEqual("0", replacer.Replace("%T165SHOTEX0237"));
            Assert.AreEqual("0", replacer.Replace("%T165SHOTEX0238"));
            Assert.AreEqual("0", replacer.Replace("%T165SHOTEX0239"));
        }

        [TestMethod]
        public void ReplaceTestEmptyHashtags()
        {
            var mock = BestShotHeaderTests.MockBestShotHeader();
            _ = mock.SetupGet(m => m.Fields).Returns(new HashtagFields(0, 0, 0));
            var bestshots = new List<(string, IBestShotHeader header)>
            {
                (@"C:\path\to\output\bestshots\bs02_03.png", mock.Object),
            }.ToDictionary(element => (element.header.Weekday, (int)element.header.Dream));
            var replacer = new ShotExReplacer(bestshots, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOTEX0235"));
        }

        [TestMethod]
        public void ReplaceTestInvalidBestShotPaths()
        {
            var bestshots = new List<(string, IBestShotHeader header)>
            {
                ("abcde", BestShotHeaderTests.MockBestShotHeader().Object),
            }.ToDictionary(element => (element.header.Weekday, (int)element.header.Dream));
            var replacer = new ShotExReplacer(bestshots, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOTEX0231"));
        }

        [TestMethod]
        public void ReplaceTestNullOutputFilePath()
        {
            var replacer = new ShotExReplacer(BestShots, null!);
            Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOTEX0231"));
        }

        [TestMethod]
        public void ReplaceTestEmptyOutputFilePath()
        {
            var replacer = new ShotExReplacer(BestShots, string.Empty);
            Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOTEX0231"));
        }

        [TestMethod]
        public void ReplaceTestInvalidOutputFilePath()
        {
            var replacer = new ShotExReplacer(BestShots, "abcde");
            Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOTEX0231"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentDay()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOTEX0331"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentScene()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOTEX0221"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentSpellCard()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual("%T165SHOTEX0131", replacer.Replace("%T165SHOTEX0131"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual("%T165XXXXXX0231", replacer.Replace("%T165XXXXXX0231"));
        }

        [TestMethod]
        public void ReplaceTestInvalidDay()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual("%T165SHOTEXXX31", replacer.Replace("%T165SHOTEXXX31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidScene()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual("%T165SHOTEX02X1", replacer.Replace("%T165SHOTEX02X1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual("%T165SHOTEX023X", replacer.Replace("%T165SHOTEX023X"));
        }
    }
}
