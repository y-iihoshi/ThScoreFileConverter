using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Th095;
using ThScoreFileConverterTests.Models.Th095.Stubs;
using ThScoreFileConverterTests.Models.Th095.Wrappers;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th095AllScoreDataTests
    {
        [TestMethod]
        public void Th095AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new Th095AllScoreDataWrapper();

            Assert.IsNull(allScoreData.Header);
            Assert.AreEqual(0, allScoreData.Scores.Count);
            Assert.IsNull(allScoreData.Status);
        });

        [TestMethod]
        public void Th095AllScoreDataSetHeaderTest() => TestUtils.Wrap(() =>
        {
            var array = HeaderTests.MakeByteArray(HeaderTests.GetValidProperties("TH95"));
            var header = HeaderWrapper<Th095Converter>.Create(array);

            var allScoreData = new Th095AllScoreDataWrapper();
            allScoreData.Set(header);

            Assert.AreSame(header.Target, allScoreData.Header.Target);
        });

        [TestMethod]
        public void Th095AllScoreDataSetHeaderTestTwice() => TestUtils.Wrap(() =>
        {
            var array = HeaderTests.MakeByteArray(HeaderTests.GetValidProperties("TH95"));
            var header1 = HeaderWrapper<Th095Converter>.Create(array);
            var header2 = HeaderWrapper<Th095Converter>.Create(array);

            var allScoreData = new Th095AllScoreDataWrapper();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1.Target, allScoreData.Header.Target);
            Assert.AreSame(header2.Target, allScoreData.Header.Target);
        });

        [TestMethod]
        public void Th095AllScoreDataSetScoreTest() => TestUtils.Wrap(() =>
        {
            var score = new ScoreStub(ScoreTests.ValidStub);

            var allScoreData = new Th095AllScoreDataWrapper();
            allScoreData.Set(score);

            Assert.AreSame(score, allScoreData.Scores[0]);
        });

        [TestMethod]
        public void Th095AllScoreDataSetScoreTestTwice() => TestUtils.Wrap(() =>
        {
            var score1 = new ScoreStub(ScoreTests.ValidStub);
            var score2 = new ScoreStub(score1);

            var allScoreData = new Th095AllScoreDataWrapper();
            allScoreData.Set(score1);
            allScoreData.Set(score2);

            Assert.AreSame(score1, allScoreData.Scores[0]);
            Assert.AreSame(score2, allScoreData.Scores[1]);
        });

        [TestMethod]
        public void Th095AllScoreDataSetStatusTest() => TestUtils.Wrap(() =>
        {
            var status = new StatusStub(StatusTests.ValidStub);

            var allScoreData = new Th095AllScoreDataWrapper();
            allScoreData.Set(status);

            Assert.AreSame(status, allScoreData.Status);
        });

        [TestMethod]
        public void Th095AllScoreDataSetStatusTestTwice() => TestUtils.Wrap(() =>
        {
            var status1 = new StatusStub(StatusTests.ValidStub);
            var status2 = new StatusStub(status1);

            var allScoreData = new Th095AllScoreDataWrapper();
            allScoreData.Set(status1);
            allScoreData.Set(status2);

            Assert.AreNotSame(status1, allScoreData.Status);
            Assert.AreSame(status2, allScoreData.Status);
        });
    }
}
