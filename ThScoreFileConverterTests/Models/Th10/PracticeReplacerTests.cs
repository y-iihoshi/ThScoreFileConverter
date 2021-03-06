﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th10;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<ThScoreFileConverter.Models.Th10.CharaWithTotal>;

namespace ThScoreFileConverterTests.Models.Th10
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
        public void PracticeReplacerTestEmpty()
        {
            var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(dictionary, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTest()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 1234360", replacer.Replace("%T10PRACHRB3"));
        }

        [TestMethod]
        public void ReplaceTestLevelExtra()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T10PRACXRB3", replacer.Replace("%T10PRACXRB3"));
        }

        [TestMethod]
        public void ReplaceTestStageExtra()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T10PRACHRBX", replacer.Replace("%T10PRACHRBX"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T10PRACHRB3"));
        }

        [TestMethod]
        public void ReplaceTestEmptyPractices()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.ReimuB)
                         && (m.Practices == ImmutableDictionary<(Level, Stage), IPractice>.Empty))
            }.ToDictionary(clearData => clearData.Chara);
            var formatterMock = MockNumberFormatter();

            var replacer = new PracticeReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T10PRACHRB3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T10XXXXHRB3", replacer.Replace("%T10XXXXHRB3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T10PRACYRB3", replacer.Replace("%T10PRACYRB3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T10PRACHXX3", replacer.Replace("%T10PRACHXX3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidStage()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new PracticeReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T10PRACHRBY", replacer.Replace("%T10PRACHRBY"));
        }
    }
}
