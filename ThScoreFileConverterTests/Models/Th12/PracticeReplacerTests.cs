﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th12;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<
    ThScoreFileConverter.Models.Th12.CharaWithTotal, ThScoreFileConverter.Models.Th10.StageProgress>;
using IPractice = ThScoreFileConverter.Models.Th10.IPractice;

namespace ThScoreFileConverterTests.Models.Th12
{
    [TestClass]
    public class PracticeReplacerTests
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
        public void PracticeReplacerTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void PracticeReplacerTestNull()
        {
            var formatterMock = MockNumberFormatter();
            _ = Assert.ThrowsException<ArgumentNullException>(
                () => _ = new PracticeReplacer(null!, formatterMock.Object));
        }

        [TestMethod]
        public void PracticeReplacerTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(dictionary, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 1234360", replacer.Replace("%T12PRACHRB3"));
        }

        [TestMethod]
        public void ReplaceTestLevelExtra()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T12PRACXRB3", replacer.Replace("%T12PRACXRB3"));
        }

        [TestMethod]
        public void ReplaceTestStageExtra()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T12PRACHRBX", replacer.Replace("%T12PRACHRBX"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T12PRACHRB3"));
        }

        [TestMethod]
        public void ReplaceTestEmptyPractices()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.ReimuB)
                         && (m.Practices == new Dictionary<(Level, Stage), IPractice>()))
            }.ToDictionary(clearData => clearData.Chara);
            var formatterMock = MockNumberFormatter();

            var replacer = new PracticeReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T12PRACHRB3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T12XXXXHRB3", replacer.Replace("%T12XXXXHRB3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T12PRACYRB3", replacer.Replace("%T12PRACYRB3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T12PRACHXX3", replacer.Replace("%T12PRACHXX3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidStage()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T12PRACHRBY", replacer.Replace("%T12PRACHRBY"));
        }
    }
}
