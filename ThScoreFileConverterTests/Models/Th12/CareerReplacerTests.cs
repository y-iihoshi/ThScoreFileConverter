﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th12;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<ThScoreFileConverter.Models.Th12.CharaWithTotal>;
using ISpellCard = ThScoreFileConverter.Models.Th10.ISpellCard<ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverterTests.Models.Th12
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
            var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(dictionary, formatterMock.Object);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 126", replacer.Replace("%T12C003RB1"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 459", replacer.Replace("%T12C003RB2"));
        }

        [TestMethod]
        public void ReplaceTestTotalClearCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 20340", replacer.Replace("%T12C000RB1"));
        }

        [TestMethod]
        public void ReplaceTestTotalTrialCount()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 57969", replacer.Replace("%T12C000RB2"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T12C003RB1"));
        }

        [TestMethod]
        public void ReplaceTestEmptyCards()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.ReimuB) && (m.Cards == ImmutableDictionary<int, ISpellCard>.Empty))
            }.ToDictionary(clearData => clearData.Chara);
            var formatterMock = MockNumberFormatter();

            var replacer = new CareerReplacer(dictionary, formatterMock.Object);
            Assert.AreEqual("invoked: 0", replacer.Replace("%T12C003RB1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T12X003RB1", replacer.Replace("%T12X003RB1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T12C114RB1", replacer.Replace("%T12C114RB1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T12C003XX1", replacer.Replace("%T12C003XX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var formatterMock = MockNumberFormatter();
            var replacer = new CareerReplacer(ClearDataDictionary, formatterMock.Object);
            Assert.AreEqual("%T12C003RBX", replacer.Replace("%T12C003RBX"));
        }
    }
}
