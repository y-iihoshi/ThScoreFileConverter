using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverter.Models.Th075;
using ThScoreFileConverterTests.Models.Th075.Stubs;

namespace ThScoreFileConverterTests.Models.Th075
{
    [TestClass]
    public class ScoreReplacerTests
    {
        internal static IReadOnlyDictionary<(CharaWithReserved, Level), IClearData> ClearData { get; } =
            new Dictionary<(CharaWithReserved, Level), IClearData>
            {
                {
                    (CharaWithReserved.Reimu, Level.Hard),
                    new ClearDataStub(ClearDataTests.ValidStub)
                },
            };

        [TestMethod]
        public void ScoreReplacerTest()
        {
            var replacer = new ScoreReplacer(ClearData);
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
            var clearData = new Dictionary<(CharaWithReserved, Level), IClearData>();
            var replacer = new ScoreReplacer(clearData);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ScoreReplacerTestEmptyRanking()
        {
            var clearData = new Dictionary<(CharaWithReserved, Level), IClearData>
            {
                {
                    (CharaWithReserved.Reimu, Level.Hard),
                    new ClearDataStub(ClearDataTests.ValidStub)
                    {
                        Ranking = new List<IHighScore>(),
                    }
                },
            };

            var replacer = new ScoreReplacer(clearData);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestName()
        {
            var replacer = new ScoreReplacer(ClearData);
            Assert.AreEqual("Player0 ", replacer.Replace("%T75SCRHRM11"));
        }

        [TestMethod]
        public void ReplaceTestScore()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new ScoreReplacer(ClearData);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("1,234,567", replacer.Replace("%T75SCRHRM12"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("1234567", replacer.Replace("%T75SCRHRM12"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestDate()
        {
            var replacer = new ScoreReplacer(ClearData);
            Assert.AreEqual("01/10", replacer.Replace("%T75SCRHRM13"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var clearData = new Dictionary<(CharaWithReserved, Level), IClearData>();
            var replacer = new ScoreReplacer(clearData);
            Assert.AreEqual(string.Empty, replacer.Replace("%T75SCRHRM11"));
            Assert.AreEqual("0", replacer.Replace("%T75SCRHRM12"));
            Assert.AreEqual("00/00", replacer.Replace("%T75SCRHRM13"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRanking()
        {
            var clearData = new Dictionary<(CharaWithReserved, Level), IClearData>
            {
                {
                    (CharaWithReserved.Reimu, Level.Hard),
                    new ClearDataStub(ClearDataTests.ValidStub)
                    {
                        Ranking = new List<IHighScore>(),
                    }
                },
            };

            var replacer = new ScoreReplacer(clearData);
            Assert.AreEqual(string.Empty, replacer.Replace("%T75SCRHRM11"));
            Assert.AreEqual("0", replacer.Replace("%T75SCRHRM12"));
            Assert.AreEqual("00/00", replacer.Replace("%T75SCRHRM13"));
        }

        [TestMethod]
        public void ReplaceTestMeiling()
        {
            var replacer = new ScoreReplacer(ClearData);
            Assert.AreEqual("%T75SCRHML11", replacer.Replace("%T75SCRHML11"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentChara()
        {
            var replacer = new ScoreReplacer(ClearData);
            Assert.AreEqual(string.Empty, replacer.Replace("%T75SCRHMR11"));
            Assert.AreEqual("0", replacer.Replace("%T75SCRHMR12"));
            Assert.AreEqual("00/00", replacer.Replace("%T75SCRHMR13"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentLevel()
        {
            var replacer = new ScoreReplacer(ClearData);
            Assert.AreEqual(string.Empty, replacer.Replace("%T75SCRNRM11"));
            Assert.AreEqual("0", replacer.Replace("%T75SCRNRM12"));
            Assert.AreEqual("00/00", replacer.Replace("%T75SCRNRM13"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentRank()
        {
            var clearData = new Dictionary<(CharaWithReserved, Level), IClearData>
            {
                {
                    (CharaWithReserved.Reimu, Level.Hard),
                    new ClearDataStub(ClearDataTests.ValidStub)
                    {
                        Ranking = ClearDataTests.ValidStub.Ranking.Take(1).ToList(),
                    }
                },
            };

            var replacer = new ScoreReplacer(clearData);
            Assert.AreEqual(string.Empty, replacer.Replace("%T75SCRHRM21"));
            Assert.AreEqual("0", replacer.Replace("%T75SCRHRM22"));
            Assert.AreEqual("00/00", replacer.Replace("%T75SCRHRM23"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ScoreReplacer(ClearData);
            Assert.AreEqual("%T75XXXHRM11", replacer.Replace("%T75XXXHRM11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ScoreReplacer(ClearData);
            Assert.AreEqual("%T75SCRXRM11", replacer.Replace("%T75SCRXRM11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new ScoreReplacer(ClearData);
            Assert.AreEqual("%T75SCRHXX11", replacer.Replace("%T75SCRHXX11"));
        }

        [TestMethod]
        public void ReplaceTestInvalidRank()
        {
            var replacer = new ScoreReplacer(ClearData);
            Assert.AreEqual("%T75SCRHRMX1", replacer.Replace("%T75SCRHRMX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new ScoreReplacer(ClearData);
            Assert.AreEqual("%T75SCRHRM1X", replacer.Replace("%T75SCRHRM1X"));
        }
    }
}
