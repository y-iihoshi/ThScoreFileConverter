using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverterTests.Models.Th08.Stubs;
using Stage = ThScoreFileConverter.Models.Th08.Stage;

namespace ThScoreFileConverterTests.Models.Th08
{
    [TestClass]
    public class PracticeReplacerTests
    {
        internal static IReadOnlyDictionary<Chara, IPracticeScore> PracticeScores { get; } =
            new List<IPracticeScore>
            {
                new PracticeScoreStub(PracticeScoreTests.ValidStub),
            }.ToDictionary(element => element.Chara);

        [TestMethod]
        public void PracticeReplacerTest()
        {
            var replacer = new PracticeReplacer(PracticeScores);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PracticeReplacerTestNull()
        {
            _ = new PracticeReplacer(null!);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void PracticeReplacerTestEmpty()
        {
            var practiceScores = new Dictionary<Chara, IPracticeScore>();
            var replacer = new PracticeReplacer(practiceScores);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestScore()
        {
            var replacer = new PracticeReplacer(PracticeScores);
            Assert.AreEqual("260", replacer.Replace("%T08PRACHMA6A1"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var replacer = new PracticeReplacer(PracticeScores);
            Assert.AreEqual("62", replacer.Replace("%T08PRACHMA6A2"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var practiceScores = new Dictionary<Chara, IPracticeScore>();
            var replacer = new PracticeReplacer(practiceScores);
            Assert.AreEqual("0", replacer.Replace("%T08PRACHMA6A1"));
        }

        [TestMethod]
        public void ReplaceTestLevelExtra()
        {
            var replacer = new PracticeReplacer(PracticeScores);
            Assert.AreEqual("%T08PRACXMA6A1", replacer.Replace("%T08PRACXMA6A1"));
        }

        [TestMethod]
        public void ReplaceTestStageExtra()
        {
            var replacer = new PracticeReplacer(PracticeScores);
            Assert.AreEqual("%T08PRACHMAEX1", replacer.Replace("%T08PRACHMAEX1"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentLevel()
        {
            var practiceScores = new List<IPracticeScore>
            {
                new PracticeScoreStub(PracticeScoreTests.ValidStub)
                {
                    HighScores = PracticeScoreTests.ValidStub.HighScores
                        .Where(pair => pair.Key.Item2 != Level.Normal)
                        .ToDictionary(pair => pair.Key, pair => pair.Value),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new PracticeReplacer(practiceScores);
            Assert.AreEqual("0", replacer.Replace("%T08PRACNMA6A1"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentChara()
        {
            var replacer = new PracticeReplacer(PracticeScores);
            Assert.AreEqual("0", replacer.Replace("%T08PRACHRY6A1"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentStage()
        {
            var practiceScores = new List<IPracticeScore>
            {
                new PracticeScoreStub(PracticeScoreTests.ValidStub)
                {
                    HighScores = PracticeScoreTests.ValidStub.HighScores
                        .Where(pair => pair.Key.Item1 != Stage.Five)
                        .ToDictionary(pair => pair.Key, pair => pair.Value),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new PracticeReplacer(practiceScores);
            Assert.AreEqual("0", replacer.Replace("%T08PRACHMA5A1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new PracticeReplacer(PracticeScores);
            Assert.AreEqual("%T08XXXXHMA6A1", replacer.Replace("%T08XXXXHMA6A1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new PracticeReplacer(PracticeScores);
            Assert.AreEqual("%T08PRACYMA6A1", replacer.Replace("%T08PRACYMA6A1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new PracticeReplacer(PracticeScores);
            Assert.AreEqual("%T08PRACHXX6A1", replacer.Replace("%T08PRACHXX6A1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidStage()
        {
            var replacer = new PracticeReplacer(PracticeScores);
            Assert.AreEqual("%T08PRACHMAXX1", replacer.Replace("%T08PRACHMAXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new PracticeReplacer(PracticeScores);
            Assert.AreEqual("%T08PRACHMA6AX", replacer.Replace("%T08PRACHMA6AX"));
        }
    }
}
