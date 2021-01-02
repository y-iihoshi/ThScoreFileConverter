using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th17;

namespace ThScoreFileConverterTests.Models.Th17
{
    [TestClass]
    public class AchievementReplacerTests
    {
        internal static IStatus Status { get; } = StatusTests.MockStatus().Object;

        [TestMethod]
        public void AchievementReplacerTest()
        {
            var replacer = new AchievementReplacer(Status);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTest()
        {
            var replacer = new AchievementReplacer(Status);
            Assert.AreEqual("ノーマルクリア", replacer.Replace("%T17ACHV23"));
        }

        [TestMethod]
        public void ReplaceTestNotCleared()
        {
            var replacer = new AchievementReplacer(Status);
            Assert.AreEqual("??????????", replacer.Replace("%T17ACHV22"));
        }

        [TestMethod]
        public void ReplaceTestZeroNumber()
        {
            var replacer = new AchievementReplacer(Status);
            Assert.AreEqual("%T17ACHV00", replacer.Replace("%T17ACHV00"));
        }

        [TestMethod]
        public void ReplaceTestExceededNumber()
        {
            var replacer = new AchievementReplacer(Status);
            Assert.AreEqual("%T17ACHV41", replacer.Replace("%T17ACHV41"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new AchievementReplacer(Status);
            Assert.AreEqual("%T17XXXX22", replacer.Replace("%T17XXXX22"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new AchievementReplacer(Status);
            Assert.AreEqual("%T17ACHVXX", replacer.Replace("%T17ACHVXX"));
        }
    }
}
