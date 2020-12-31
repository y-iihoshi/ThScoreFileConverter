using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
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

        private static Mock<INumberFormatter> MockNumberFormatter()
        {
            var mock = new Mock<INumberFormatter>();
            _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
                .Returns((object value) => "invoked: " + value.ToString());
            return mock;
        }

        [TestMethod]
        public void ShotExReplacerTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotExReplacerTestNullBestShots()
        {
            var formatterMock = MockNumberFormatter();
            _ = Assert.ThrowsException<ArgumentNullException>(
                () => _ = new ShotExReplacer(null!, formatterMock.Object, @"C:\path\to\output\"));
        }

        [TestMethod]
        public void ShotExReplacerTestEmptyBestShots()
        {
            var bestshots = new Dictionary<(Day, int), (string, IBestShotHeader)>();
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(bestshots, formatterMock.Object, @"C:\path\to\output\");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotExReplacerTestInvalidBestShotPath()
        {
            var bestshots = new List<(string, IBestShotHeader header)>
            {
                ("abcde", BestShotHeaderTests.MockBestShotHeader().Object),
            }.ToDictionary(element => (element.header.Weekday, (int)element.header.Dream));
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(bestshots, formatterMock.Object, @"C:\path\to\output\");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotExReplacerTestNullOutputFilePath()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, formatterMock.Object, null!);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotExReplacerTestEmptyOutputFilePath()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, formatterMock.Object, string.Empty);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotExReplacerTestInvalidOutputFilePath()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, formatterMock.Object, "abcde");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestPath()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual(@"bestshots/bs02_03.png", replacer.Replace("%T165SHOTEX0231"));
        }

        [TestMethod]
        public void ReplaceTestWidth()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual("4", replacer.Replace("%T165SHOTEX0232"));
        }

        [TestMethod]
        public void ReplaceTestHeight()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual("5", replacer.Replace("%T165SHOTEX0233"));
        }

        [TestMethod]
        public void ReplaceTestDateTime()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
            var expected = DateTimeHelper.GetString(11);
            Assert.AreEqual(expected, replacer.Replace("%T165SHOTEX0234"));
        }

        [TestMethod]
        public void ReplaceTestHashtags()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
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
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual("invoked: 19", replacer.Replace("%T165SHOTEX0236"));
        }

        [TestMethod]
        public void ReplaceTestNumLikes()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual("invoked: 20", replacer.Replace("%T165SHOTEX0237"));
        }

        [TestMethod]
        public void ReplaceTestNumFavs()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual("invoked: 21", replacer.Replace("%T165SHOTEX0238"));
        }

        [TestMethod]
        public void ReplaceTestScore()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual("invoked: 13", replacer.Replace("%T165SHOTEX0239"));
        }

        [TestMethod]
        public void ReplaceTestEmptyBestShots()
        {
            var bestshots = new Dictionary<(Day, int), (string, IBestShotHeader)>();
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(bestshots, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOTEX0231"));
            Assert.AreEqual("0", replacer.Replace("%T165SHOTEX0232"));
            Assert.AreEqual("0", replacer.Replace("%T165SHOTEX0233"));
            Assert.AreEqual(DateTimeHelper.GetString(null), replacer.Replace("%T165SHOTEX0234"));
            Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOTEX0235"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T165SHOTEX0236"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T165SHOTEX0237"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T165SHOTEX0238"));
            Assert.AreEqual("invoked: 0", replacer.Replace("%T165SHOTEX0239"));
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
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(bestshots, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOTEX0235"));
        }

        [TestMethod]
        public void ReplaceTestInvalidBestShotPaths()
        {
            var bestshots = new List<(string, IBestShotHeader header)>
            {
                ("abcde", BestShotHeaderTests.MockBestShotHeader().Object),
            }.ToDictionary(element => (element.header.Weekday, (int)element.header.Dream));
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(bestshots, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOTEX0231"));
        }

        [TestMethod]
        public void ReplaceTestNullOutputFilePath()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, formatterMock.Object, null!);
            Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOTEX0231"));
        }

        [TestMethod]
        public void ReplaceTestEmptyOutputFilePath()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, formatterMock.Object, string.Empty);
            Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOTEX0231"));
        }

        [TestMethod]
        public void ReplaceTestInvalidOutputFilePath()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, formatterMock.Object, "abcde");
            Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOTEX0231"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentDay()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOTEX0331"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentScene()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T165SHOTEX0221"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentSpellCard()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual("%T165SHOTEX0131", replacer.Replace("%T165SHOTEX0131"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual("%T165XXXXXX0231", replacer.Replace("%T165XXXXXX0231"));
        }

        [TestMethod]
        public void ReplaceTestInvalidDay()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual("%T165SHOTEXXX31", replacer.Replace("%T165SHOTEXXX31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidScene()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual("%T165SHOTEX02X1", replacer.Replace("%T165SHOTEX02X1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new ShotExReplacer(BestShots, formatterMock.Object, @"C:\path\to\output\");
            Assert.AreEqual("%T165SHOTEX023X", replacer.Replace("%T165SHOTEX023X"));
        }
    }
}
