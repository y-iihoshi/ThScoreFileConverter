using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Th095;
using ThScoreFileConverterTests.Models.Th095.Wrappers;
using ThScoreFileConverterTests.Models.Wrappers;
using ChapterWrapper = ThScoreFileConverterTests.Models.Th10.Wrappers.ChapterWrapper;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th143AllScoreDataTests
    {
        [TestMethod]
        public void Th143AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new Th143AllScoreDataWrapper();

            Assert.IsNull(allScoreData.Header);
            Assert.AreEqual(0, allScoreData.ScoresCount);
            Assert.AreEqual(0, allScoreData.ItemStatusesCount);
            Assert.IsNull(allScoreData.Status);
        });

        [TestMethod]
        public void Th143AllScoreDataSetHeaderTest() => TestUtils.Wrap(() =>
        {
            var array = HeaderTests.MakeByteArray(HeaderTests.GetValidProperties("T341"));
            var header = HeaderWrapper<Th143Converter>.Create(array);

            var allScoreData = new Th143AllScoreDataWrapper();
            allScoreData.Set(header);

            Assert.AreSame(header.Target, allScoreData.Header.Target);
        });

        [TestMethod]
        public void Th143AllScoreDataSetHeaderTestTwice() => TestUtils.Wrap(() =>
        {
            var array = HeaderTests.MakeByteArray(HeaderTests.GetValidProperties("T341"));
            var header1 = HeaderWrapper<Th143Converter>.Create(array);
            var header2 = HeaderWrapper<Th143Converter>.Create(array);

            var allScoreData = new Th143AllScoreDataWrapper();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1.Target, allScoreData.Header.Target);
            Assert.AreSame(header2.Target, allScoreData.Header.Target);
        });

        [TestMethod]
        public void Th143AllScoreDataSetScoreTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th143ScoreTests.MakeByteArray(Th143ScoreTests.ValidStub));
            var score = new Th143ScoreWrapper(chapter);

            var allScoreData = new Th143AllScoreDataWrapper();
            allScoreData.Set(score);

            Assert.AreSame(score.Target, allScoreData.ScoresItem(0).Target);
        });

        [TestMethod]
        public void Th143AllScoreDataSetScoreTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th143ScoreTests.MakeByteArray(Th143ScoreTests.ValidStub));
            var score1 = new Th143ScoreWrapper(chapter);
            var score2 = new Th143ScoreWrapper(chapter);

            var allScoreData = new Th143AllScoreDataWrapper();
            allScoreData.Set(score1);
            allScoreData.Set(score2);

            Assert.AreSame(score1.Target, allScoreData.ScoresItem(0).Target);
            Assert.AreSame(score2.Target, allScoreData.ScoresItem(1).Target);
        });

        [TestMethod]
        public void Th143AllScoreDataSetItemStatusTest() => TestUtils.Wrap(() =>
        {
            var stub = Th143ItemStatusTests.ValidStub;
            var chapter = ChapterWrapper.Create(Th143ItemStatusTests.MakeByteArray(stub));
            var status = new Th143ItemStatusWrapper(chapter);

            var allScoreData = new Th143AllScoreDataWrapper();
            allScoreData.Set(status);

            Assert.AreSame(status.Target, allScoreData.ItemStatusesItem(stub.Item).Target);
        });

        [TestMethod]
        public void Th143AllScoreDataSetItemStatusTestTwice() => TestUtils.Wrap(() =>
        {
            var stub = Th143ItemStatusTests.ValidStub;
            var chapter = ChapterWrapper.Create(Th143ItemStatusTests.MakeByteArray(stub));
            var status1 = new Th143ItemStatusWrapper(chapter);
            var status2 = new Th143ItemStatusWrapper(chapter);

            var allScoreData = new Th143AllScoreDataWrapper();
            allScoreData.Set(status1);
            allScoreData.Set(status2);

            Assert.AreSame(status1.Target, allScoreData.ItemStatusesItem(stub.Item).Target);
            Assert.AreNotSame(status2.Target, allScoreData.ItemStatusesItem(stub.Item).Target);
        });

        [TestMethod]
        public void Th143AllScoreDataSetStatusTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th143StatusTests.MakeByteArray(Th143StatusTests.ValidStub));
            var status = new Th143StatusWrapper(chapter);

            var allScoreData = new Th143AllScoreDataWrapper();
            allScoreData.Set(status);

            Assert.AreSame(status.Target, allScoreData.Status.Target);
        });

        [TestMethod]
        public void Th143AllScoreDataSetStatusTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th143StatusTests.MakeByteArray(Th143StatusTests.ValidStub));
            var status1 = new Th143StatusWrapper(chapter);
            var status2 = new Th143StatusWrapper(chapter);

            var allScoreData = new Th143AllScoreDataWrapper();
            allScoreData.Set(status1);
            allScoreData.Set(status2);

            Assert.AreNotSame(status1.Target, allScoreData.Status.Target);
            Assert.AreSame(status2.Target, allScoreData.Status.Target);
        });
    }
}
