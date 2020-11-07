using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th123;
using IClearData = ThScoreFileConverter.Models.Th105.IClearData<ThScoreFileConverter.Models.Th123.Chara>;
using ISpellCardResult = ThScoreFileConverter.Models.Th105.ISpellCardResult<ThScoreFileConverter.Models.Th123.Chara>;

namespace ThScoreFileConverterTests.Models.Th123
{
    [TestClass]
    public class CareerReplacerTests
    {
        private static IClearData CreateClearData()
        {
#if false
            return Mock.Of<IClearData>(
                c => c.SpellCardResults == new[]
                {
                    Mock.Of<ISpellCardResult>(
                        s => (s.Enemy == Chara.Meiling)
                             && (s.Id == 6)
                             && (s.GotCount == 12)
                             && (s.TrialCount == 34)
                             && (s.Frames == 5678)),
                    Mock.Of<ISpellCardResult>(
                        s => (s.Enemy == Chara.Marisa)
                             && (s.Id == 18)
                             && (s.GotCount == 1)
                             && (s.TrialCount == 90)
                             && (s.Frames == 23456)),
                }.ToDictionary(result => (result.Enemy, result.Id)));   // causes CS8143
#else
            var mock = new Mock<IClearData>();
            _ = mock.SetupGet(m => m.SpellCardResults).Returns(
                new[]
                {
                    Mock.Of<ISpellCardResult>(
                        m => (m.Enemy == Chara.Meiling)
                             && (m.Id == 6)
                             && (m.GotCount == 12)
                             && (m.TrialCount == 34)
                             && (m.Frames == 5678)),
                    Mock.Of<ISpellCardResult>(
                        m => (m.Enemy == Chara.Marisa)
                             && (m.Id == 18)
                             && (m.GotCount == 1)
                             && (m.TrialCount == 90)
                             && (m.Frames == 23456)),
                }.ToDictionary(result => (result.Enemy, result.Id)));
            return mock.Object;
#endif
        }

        internal static IReadOnlyDictionary<Chara, IClearData> ClearDataDictionary { get; } =
            new[] { (Chara.Cirno, CreateClearData()) }.ToDictionary();

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
            var dictionary = new Dictionary<Chara, IClearData>();
            var replacer = new CareerReplacer(dictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestGotCount()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("12", replacer.Replace("%T123C15CI1"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("34", replacer.Replace("%T123C15CI2"));
        }

        [TestMethod]
        public void ReplaceTestTime()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("01:34.633", replacer.Replace("%T123C15CI3"));
        }

        [TestMethod]
        public void ReplaceTestTotalGotCount()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("13", replacer.Replace("%T123C00CI1"));
        }

        [TestMethod]
        public void ReplaceTestTotalTrialCount()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("124", replacer.Replace("%T123C00CI2"));
        }

        [TestMethod]
        public void ReplaceTestTotalTime()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("08:05.566", replacer.Replace("%T123C00CI3"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<Chara, IClearData>();
            var replacer = new CareerReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T123C15CI1"));
            Assert.AreEqual("0", replacer.Replace("%T123C15CI2"));
            Assert.AreEqual("00:00.000", replacer.Replace("%T123C15CI3"));
        }

        [TestMethod]
        public void ReplaceTestEmptySpellCardResults()
        {
            var dictionary = new Dictionary<Chara, IClearData>
            {
                {
                    Chara.Marisa,
                    Mock.Of<IClearData>(m => m.SpellCardResults == new Dictionary<(Chara, int), ISpellCardResult>())
                },
            };
            var replacer = new CareerReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T123C15CI1"));
            Assert.AreEqual("0", replacer.Replace("%T123C15CI2"));
            Assert.AreEqual("00:00.000", replacer.Replace("%T123C15CI3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T123X15CI1", replacer.Replace("%T123X15CI1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T123C65CI1", replacer.Replace("%T123C65CI1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T123C15XX1", replacer.Replace("%T123C15XX1"));
            Assert.AreEqual("%T123C15NM1", replacer.Replace("%T123C15NM1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CareerReplacer(ClearDataDictionary);
            Assert.AreEqual("%T123C15CIX", replacer.Replace("%T123C15CIX"));
        }
    }
}
