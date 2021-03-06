﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th123;
using IClearData = ThScoreFileConverter.Models.Th105.IClearData<ThScoreFileConverter.Models.Th123.Chara>;
using ISpellCardResult = ThScoreFileConverter.Models.Th105.ISpellCardResult<ThScoreFileConverter.Models.Th123.Chara>;
using Level = ThScoreFileConverter.Models.Th105.Level;

namespace ThScoreFileConverterTests.Models.Th123
{
    [TestClass]
    public class CollectRateReplacerTests
    {
        private static IClearData CreateClearData()
        {
#if false
            return Mock.Of<IClearData>(
                c => c.SpellCardResults == new[]
                {
                    Mock.Of<ISpellCardResult>(
                        s => (s.Enemy == Chara.Reimu)
                             && (s.Id == 5)
                             && (s.Level == Level.Normal)
                             && (s.GotCount == 12)
                             && (s.TrialCount == 34)),
                    Mock.Of<ISpellCardResult>(
                        s => (s.Enemy == Chara.Reimu)
                             && (s.Id == 6)
                             && (s.Level == Level.Hard)
                             && (s.GotCount == 56)
                             && (s.TrialCount == 78)),
                    Mock.Of<ISpellCardResult>(
                        s => (s.Enemy == Chara.Iku)
                             && (s.Id == 10)
                             && (s.Level == Level.Hard)
                             && (s.GotCount == 0)
                             && (s.TrialCount == 90)),
                    Mock.Of<ISpellCardResult>(
                        s => (s.Enemy == Chara.Tenshi)
                             && (s.Id == 18)
                             && (s.Level == Level.Hard)
                             && (s.GotCount == 0)
                             && (s.TrialCount == 0)),
                }.ToDictionary(result => (result.Enemy, result.Id)));   // causes CS8143
#else
            var mock = new Mock<IClearData>();
            _ = mock.SetupGet(m => m.SpellCardResults).Returns(
                new[]
                {
                    Mock.Of<ISpellCardResult>(
                        s => (s.Enemy == Chara.Reimu)
                             && (s.Id == 5)
                             && (s.Level == Level.Normal)
                             && (s.GotCount == 12)
                             && (s.TrialCount == 34)),
                    Mock.Of<ISpellCardResult>(
                        s => (s.Enemy == Chara.Reimu)
                             && (s.Id == 6)
                             && (s.Level == Level.Hard)
                             && (s.GotCount == 56)
                             && (s.TrialCount == 78)),
                    Mock.Of<ISpellCardResult>(
                        s => (s.Enemy == Chara.Iku)
                             && (s.Id == 10)
                             && (s.Level == Level.Hard)
                             && (s.GotCount == 0)
                             && (s.TrialCount == 90)),
                    Mock.Of<ISpellCardResult>(
                        s => (s.Enemy == Chara.Tenshi)
                             && (s.Id == 18)
                             && (s.Level == Level.Hard)
                             && (s.GotCount == 0)
                             && (s.TrialCount == 0)),
                }.ToDictionary(result => (result.Enemy, result.Id)));
            return mock.Object;
#endif
        }

        internal static IReadOnlyDictionary<Chara, IClearData> ClearDataDictionary { get; } =
            new[] { (Chara.Cirno, CreateClearData()) }.ToDictionary();

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
            var dictionary = ImmutableDictionary<Chara, IClearData>.Empty;
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(dictionary, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestGotCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 1", replacer.Replace("%T123CRGHCI1"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 2", replacer.Replace("%T123CRGHCI2"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalGotCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 2", replacer.Replace("%T123CRGTCI1"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 3", replacer.Replace("%T123CRGTCI2"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = ImmutableDictionary<Chara, IClearData>.Empty;
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T123CRGHCI1"));
        }

        [TestMethod]
        public void ReplaceTestEmptySpellCardResults()
        {
            var dictionary = new Dictionary<Chara, IClearData>
            {
                {
                    Chara.Marisa,
                    Mock.Of<IClearData>(
                        m => m.SpellCardResults == ImmutableDictionary<(Chara, int), ISpellCardResult>.Empty)
                },
            };
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T123CRGHCI1"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T123CRGHMR1", replacer.Replace("%T123CRGHMR1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T123XXXHCI1", replacer.Replace("%T123XXXHCI1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T123CRGXCI1", replacer.Replace("%T123CRGXCI1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T123CRGHXX1", replacer.Replace("%T123CRGHXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CollectRateReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T123CRGHCIX", replacer.Replace("%T123CRGHCIX"));
        }
    }
}
