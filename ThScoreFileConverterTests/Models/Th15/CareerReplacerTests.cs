using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th15;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverterTests.Models.Th15
{
    [TestClass]
    public class CareerReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            new[] { ClearDataTests.MockClearData().Object }.ToDictionary(clearData => clearData.Chara);

        private static Mock<INumberFormatter> MockNumberFormatter()
        {
            var mock = new Mock<INumberFormatter>();
            _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
                .Returns((object value) => "invoked: " + value.ToString());
            return mock;
        }

        [TestMethod]
        public void CareerReplacerTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void CareerReplacerTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(dictionary, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestPointdeviceClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 15", replacer.Replace("%T15CP003MR1"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 59", replacer.Replace("%T15CP003MR2"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 8568", replacer.Replace("%T15CP000MR1"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 13804", replacer.Replace("%T15CP000MR2"));
        }

        [TestMethod]
        public void ReplaceTestLegacyClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 15", replacer.Replace("%T15CL003MR1"));
        }

        [TestMethod]
        public void ReplaceTestLegacyTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 59", replacer.Replace("%T15CL003MR2"));
        }

        [TestMethod]
        public void ReplaceTestLegacyTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 8568", replacer.Replace("%T15CL000MR1"));
        }

        [TestMethod]
        public void ReplaceTestLegacyTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 13804", replacer.Replace("%T15CL000MR2"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T15CP003MR1"));
        }

        [TestMethod]
        public void ReplaceTestEmptyGameModes()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.Marisa)
                         && (m.GameModeData == new Dictionary<GameMode, IClearDataPerGameMode>()))
            }.ToDictionary(clearData => clearData.Chara);
            var formatterMock = MockNumberFormatter();

            var replacer = new CareerReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T15CP003MR1"));
        }

        [TestMethod]
        public void ReplaceTestEmptyCards()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.Marisa)
                         && (m.GameModeData == new Dictionary<GameMode, IClearDataPerGameMode>
                            {
                                {
                                    GameMode.Pointdevice,
                                    Mock.Of<IClearDataPerGameMode>(c => c.Cards == new Dictionary<int, ISpellCard>())
                                },
                            }))
            }.ToDictionary(clearData => clearData.Chara);
            var formatterMock = MockNumberFormatter();

            var replacer = new CareerReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T15CP003MR1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T15XP003MR1", replacer.Replace("%T15XP003MR1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidGameMode()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T15CX003MR1", replacer.Replace("%T15CX003MR1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T15CP120MR1", replacer.Replace("%T15CP120MR1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T15CP003XX1", replacer.Replace("%T15CP003XX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T15CP003MRX", replacer.Replace("%T15CP003MRX"));
        }
    }
}
