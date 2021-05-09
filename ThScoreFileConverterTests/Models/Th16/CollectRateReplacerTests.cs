using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th16;
using Definitions = ThScoreFileConverter.Models.Th16.Definitions;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th16.CharaWithTotal,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th14.StagePractice,
    ThScoreFileConverter.Models.Th16.IScoreData>;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Models.Level>;
using Level = ThScoreFileConverter.Models.Level;

namespace ThScoreFileConverterTests.Models.Th16
{
    [TestClass]
    public class CollectRateReplacerTests
    {
        private static IEnumerable<IClearData> CreateClearDataList()
        {
            static ISpellCard CreateSpellCard(
                int clear, int practiceClear, int trial, int practiceTrial, int id, Level level)
            {
                var mock = new Mock<ISpellCard>();
                _ = mock.SetupGet(s => s.ClearCount).Returns(clear);
                _ = mock.SetupGet(s => s.PracticeClearCount).Returns(practiceClear);
                _ = mock.SetupGet(s => s.TrialCount).Returns(trial);
                _ = mock.SetupGet(s => s.PracticeTrialCount).Returns(practiceTrial);
                _ = mock.SetupGet(s => s.Id).Returns(id);
                _ = mock.SetupGet(s => s.Level).Returns(level);
                return mock.Object;
            }

            var mock1 = new Mock<IClearData>();
            _ = mock1.SetupGet(c => c.Chara).Returns(CharaWithTotal.Aya);
            _ = mock1.SetupGet(c => c.Cards).Returns(
                Definitions.CardTable.ToDictionary(
                    pair => pair.Key,
                    pair => CreateSpellCard(
                        pair.Key % 3, pair.Key % 7, pair.Key % 5, pair.Key % 11, pair.Value.Id, pair.Value.Level)));

            var mock2 = new Mock<IClearData>();
            _ = mock2.SetupGet(c => c.Chara).Returns(CharaWithTotal.Total);
            _ = mock2.SetupGet(c => c.Cards).Returns(
                Definitions.CardTable.ToDictionary(
                    pair => pair.Key,
                    pair => CreateSpellCard(
                        pair.Key % 7, pair.Key % 3, pair.Key % 11, pair.Key % 5, pair.Value.Id, pair.Value.Level)));

            return new[] { mock1.Object, mock2.Object };
        }

        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            CreateClearDataList().ToDictionary(clearData => clearData.Chara);

        private static Mock<INumberFormatter> MockNumberFormatter()
        {
            var mock = new Mock<INumberFormatter>();
            _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
                .Returns((object value) => "invoked: " + value.ToString());
            return mock;
        }

        [TestMethod]
        public void CollectRateReplacerTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void CollectRateReplacerTestEmpty()
        {
            var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(dictionary, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestStoryClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 2", replacer.Replace("%T16CRGSHAY31"));
        }

        [TestMethod]
        public void ReplaceTestStoryTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 3", replacer.Replace("%T16CRGSHAY32"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelExtraClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 9", replacer.Replace("%T16CRGSXAY31"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelExtraTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 11", replacer.Replace("%T16CRGSXAY32"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 9", replacer.Replace("%T16CRGSTAY31"));
        }

        [TestMethod]
        public void ReplaceTestStoryLevelTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 12", replacer.Replace("%T16CRGSTAY32"));
        }

        [TestMethod]
        public void ReplaceTestStoryCharaTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 3", replacer.Replace("%T16CRGSHTL31"));
        }

        [TestMethod]
        public void ReplaceTestStoryCharaTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 3", replacer.Replace("%T16CRGSHTL32"));
        }

        [TestMethod]
        public void ReplaceTestStoryStageTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 17", replacer.Replace("%T16CRGSHAY01"));
        }

        [TestMethod]
        public void ReplaceTestStoryStageTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 21", replacer.Replace("%T16CRGSHAY02"));
        }

        [TestMethod]
        public void ReplaceTestStoryTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 102", replacer.Replace("%T16CRGSTTL01"));
        }

        [TestMethod]
        public void ReplaceTestStoryTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 109", replacer.Replace("%T16CRGSTTL02"));
        }

        [TestMethod]
        public void ReplaceTestPracticeClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 3", replacer.Replace("%T16CRGPHAY31"));
        }

        [TestMethod]
        public void ReplaceTestPracticeTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 3", replacer.Replace("%T16CRGPHAY32"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelExtraClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 11", replacer.Replace("%T16CRGPXAY31"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelExtraTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 12", replacer.Replace("%T16CRGPXAY32"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 12", replacer.Replace("%T16CRGPTAY31"));
        }

        [TestMethod]
        public void ReplaceTestPracticeLevelTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 12", replacer.Replace("%T16CRGPTAY32"));
        }

        [TestMethod]
        public void ReplaceTestPracticeCharaTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 2", replacer.Replace("%T16CRGPHTL31"));
        }

        [TestMethod]
        public void ReplaceTestPracticeCharaTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 3", replacer.Replace("%T16CRGPHTL32"));
        }

        [TestMethod]
        public void ReplaceTestPracticeStageTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 22", replacer.Replace("%T16CRGPHAY01"));
        }

        [TestMethod]
        public void ReplaceTestPracticeStageTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 24", replacer.Replace("%T16CRGPHAY02"));
        }

        [TestMethod]
        public void ReplaceTestPracticeTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 80", replacer.Replace("%T16CRGPTTL01"));
        }

        [TestMethod]
        public void ReplaceTestPracticeTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 96", replacer.Replace("%T16CRGPTTL02"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T16CRGSHAY31"));
        }

        [TestMethod]
        public void ReplaceTestEmptyCards()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.Aya) && (m.Cards == ImmutableDictionary<int, ISpellCard>.Empty))
            }.ToDictionary(clearData => clearData.Chara);
            var formatterMock = MockNumberFormatter();

            var replacer = new CollectRateReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T16CRGSHAY31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T16XXXSHAY31", replacer.Replace("%T16XXXSHAY31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidGameMode()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T16CRGXHAY31", replacer.Replace("%T16CRGXHAY31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T16CRGSYAY31", replacer.Replace("%T16CRGSYAY31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T16CRGSHXX31", replacer.Replace("%T16CRGSHXX31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidStage()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T16CRGSHAYX1", replacer.Replace("%T16CRGSHAYX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T16CRGSHAY3X", replacer.Replace("%T16CRGSHAY3X"));
        }
    }
}
