using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverter.Models.Th165;
using ThScoreFileConverterTests.Models.Th165.Stubs;

namespace ThScoreFileConverterTests.Models.Th165
{
    [TestClass]
    public class ScoreReplacerTests
    {
        internal static IReadOnlyList<IScore> Scores { get; } = new List<IScore>
        {
            new ScoreStub(ScoreTests.ValidStub),
        };

        [TestMethod]
        public void ScoreReplacerTest()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ScoreReplacerTestNull()
        {
            _ = new ScoreReplacer(null!);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ScoreReplacerTestEmpty()
        {
            var scores = new List<IScore>();
            var replacer = new ScoreReplacer(scores);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestHighScore()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new ScoreReplacer(Scores);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("1,234,567", replacer.Replace("%T165SCR0441"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("1234567", replacer.Replace("%T165SCR0441"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestChallengeCount()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.AreEqual("56", replacer.Replace("%T165SCR0442"));
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.AreEqual("34", replacer.Replace("%T165SCR0443"));
        }

        [TestMethod]
        public void ReplaceTestNumPhotos()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.AreEqual("78", replacer.Replace("%T165SCR0444"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var scores = new List<IScore>();
            var replacer = new ScoreReplacer(scores);
            Assert.AreEqual("0", replacer.Replace("%T165SCR0441"));
            Assert.AreEqual("0", replacer.Replace("%T165SCR0442"));
            Assert.AreEqual("0", replacer.Replace("%T165SCR0443"));
            Assert.AreEqual("0", replacer.Replace("%T165SCR0444"));
        }

        [TestMethod]
        public void ReplaceTestNullScore()
        {
            var scores = new List<IScore> { null! };
            var replacer = new ScoreReplacer(scores);
            Assert.AreEqual("0", replacer.Replace("%T165SCR0441"));
            Assert.AreEqual("0", replacer.Replace("%T165SCR0442"));
            Assert.AreEqual("0", replacer.Replace("%T165SCR0443"));
            Assert.AreEqual("0", replacer.Replace("%T165SCR0444"));
        }

        [TestMethod]
        public void ReplaceTestZeroNumber()
        {
            var scores = new List<IScore>
            {
                new ScoreStub(ScoreTests.ValidStub)
                {
                    Number = 0,
                },
            };

            var replacer = new ScoreReplacer(scores);
            Assert.AreEqual("0", replacer.Replace("%T165SCR0441"));
            Assert.AreEqual("0", replacer.Replace("%T165SCR0442"));
            Assert.AreEqual("0", replacer.Replace("%T165SCR0443"));
            Assert.AreEqual("0", replacer.Replace("%T165SCR0444"));
        }

        [TestMethod]
        public void ReplaceTestExceededNumber()
        {
            var scores = new List<IScore>
            {
                new ScoreStub(ScoreTests.ValidStub)
                {
                    Number = 104,
                },
            };

            var replacer = new ScoreReplacer(scores);
            Assert.AreEqual("0", replacer.Replace("%T165SCR0441"));
            Assert.AreEqual("0", replacer.Replace("%T165SCR0442"));
            Assert.AreEqual("0", replacer.Replace("%T165SCR0443"));
            Assert.AreEqual("0", replacer.Replace("%T165SCR0444"));
        }

        [TestMethod]
        public void ReplaceTestMismatchNumber()
        {
            var scores = new List<IScore>
            {
                new ScoreStub(ScoreTests.ValidStub)
                {
                    Number = 70,
                },
            };

            var replacer = new ScoreReplacer(scores);
            Assert.AreEqual("0", replacer.Replace("%T165SCR0441"));
            Assert.AreEqual("0", replacer.Replace("%T165SCR0442"));
            Assert.AreEqual("0", replacer.Replace("%T165SCR0443"));
            Assert.AreEqual("0", replacer.Replace("%T165SCR0444"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentDayScene()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.AreEqual("%T165SCR0131", replacer.Replace("%T165SCR0131"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.AreEqual("%T165XXX0441", replacer.Replace("%T165XXX0441"));
        }

        [TestMethod]
        public void ReplaceTestInvalidDay()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.AreEqual("%T165SCRXX41", replacer.Replace("%T165SCRXX41"));
        }

        [TestMethod]
        public void ReplaceTestInvalidScene()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.AreEqual("%T165SCR04X1", replacer.Replace("%T165SCR04X1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new ScoreReplacer(Scores);
            Assert.AreEqual("%T165SCR044X", replacer.Replace("%T165SCR044X"));
        }
    }
}
