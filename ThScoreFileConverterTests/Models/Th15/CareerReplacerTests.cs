using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverter.Models.Th15;
using ThScoreFileConverterTests.Models.Th15.Stubs;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverterTests.Models.Th15
{
    [TestClass]
    public class CareerReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            new List<IClearData>
            {
                ClearDataTests.MakeValidStub(),
            }.ToDictionary(element => element.Chara);

        [TestMethod]
        public void CareerReplacerTest()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CareerReplacerTestNull()
        {
            _ = new CareerReplacer(null!);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void CareerReplacerTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CareerReplacer(dictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestPointdeviceClearCount()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("15", replacer.Replace("%T15CP003MR1"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceTrialCount()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("59", replacer.Replace("%T15CP003MR2"));
        }

        [TestMethod]
        public void ReplaceTestPointdeviceTotalClearCount()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new CareerReplacer(ClearDataDictionary);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("8,568", replacer.Replace("%T15CP000MR1"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("8568", replacer.Replace("%T15CP000MR1"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestPointdeviceTotalTrialCount()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new CareerReplacer(ClearDataDictionary);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("13,804", replacer.Replace("%T15CP000MR2"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("13804", replacer.Replace("%T15CP000MR2"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestLegacyClearCount()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("15", replacer.Replace("%T15CL003MR1"));
        }

        [TestMethod]
        public void ReplaceTestLegacyTrialCount()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("59", replacer.Replace("%T15CL003MR2"));
        }

        [TestMethod]
        public void ReplaceTestLegacyTotalClearCount()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new CareerReplacer(ClearDataDictionary);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("8,568", replacer.Replace("%T15CL000MR1"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("8568", replacer.Replace("%T15CL000MR1"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestLegacyTotalTrialCount()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new CareerReplacer(ClearDataDictionary);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("13,804", replacer.Replace("%T15CL000MR2"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("13804", replacer.Replace("%T15CL000MR2"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CareerReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T15CP003MR1"));
        }

        [TestMethod]
        public void ReplaceTestEmptyGameModes()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub()
                {
                    Chara = CharaWithTotal.Marisa,
                    GameModeData = new Dictionary<GameMode, IClearDataPerGameMode>(),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new CareerReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T15CP003MR1"));
        }

        [TestMethod]
        public void ReplaceTestEmptyCards()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub()
                {
                    Chara = CharaWithTotal.Marisa,
                    GameModeData = new Dictionary<GameMode, IClearDataPerGameMode>
                    {
                        {
                            GameMode.Pointdevice,
                            new ClearDataPerGameModeStub
                            {
                                Cards = new Dictionary<int, ISpellCard>(),
                            }
                        },
                    },
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new CareerReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T15CP003MR1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15XP003MR1", replacer.Replace("%T15XP003MR1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidGameMode()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15CX003MR1", replacer.Replace("%T15CX003MR1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15CP120MR1", replacer.Replace("%T15CP120MR1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15CP003XX1", replacer.Replace("%T15CP003XX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15CP003MRX", replacer.Replace("%T15CP003MRX"));
        }
    }
}
