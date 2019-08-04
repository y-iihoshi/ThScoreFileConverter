using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th165HashtagFieldsTests
    {
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 12345678, int.MinValue)]
        [DataRow(12345678, int.MinValue, int.MaxValue)]
        [DataRow(int.MinValue, int.MaxValue, 0)]
        [DataRow(int.MaxValue, 0, 12345678)]
        public void DataTest(int data1, int data2, int data3) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(data1, fields.Data.ElementAt(0));
            Assert.AreEqual(data2, fields.Data.ElementAt(1));
            Assert.AreEqual(data3, fields.Data.ElementAt(2));
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(1 << 0, 0, 0, true)]
        [DataRow(~(1 << 0), ~0, ~0, false)]
        public void EnemyIsInFrameTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.EnemyIsInFrame);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(1 << 1, 0, 0, true)]
        [DataRow(~(1 << 1), ~0, ~0, false)]
        public void EnemyIsPartlyInFrameTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.EnemyIsPartlyInFrame);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(1 << 2, 0, 0, true)]
        [DataRow(~(1 << 2), ~0, ~0, false)]
        public void WholeEnemyIsInFrameTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.WholeEnemyIsInFrame);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(1 << 3, 0, 0, true)]
        [DataRow(~(1 << 3), ~0, ~0, false)]
        public void EnemyIsInMiddleTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.EnemyIsInMiddle);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(1 << 4, 0, 0, true)]
        [DataRow(~(1 << 4), ~0, ~0, false)]
        public void IsSelfieTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.IsSelfie);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(1 << 5, 0, 0, true)]
        [DataRow(~(1 << 5), ~0, ~0, false)]
        public void IsTwoShotTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.IsTwoShot);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(1 << 7, 0, 0, true)]
        [DataRow(~(1 << 7), ~0, ~0, false)]
        public void BitDangerousTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.BitDangerous);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(1 << 8, 0, 0, true)]
        [DataRow(~(1 << 8), ~0, ~0, false)]
        public void SeriouslyDangerousTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.SeriouslyDangerous);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(1 << 9, 0, 0, true)]
        [DataRow(~(1 << 9), ~0, ~0, false)]
        public void ThoughtGonnaDieTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.ThoughtGonnaDie);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(1 << 10, 0, 0, true)]
        [DataRow(~(1 << 10), ~0, ~0, false)]
        public void ManyRedsTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.ManyReds);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(1 << 11, 0, 0, true)]
        [DataRow(~(1 << 11), ~0, ~0, false)]
        public void ManyPurplesTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.ManyPurples);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(1 << 12, 0, 0, true)]
        [DataRow(~(1 << 12), ~0, ~0, false)]
        public void ManyBluesTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.ManyBlues);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(1 << 13, 0, 0, true)]
        [DataRow(~(1 << 13), ~0, ~0, false)]
        public void ManyCyansTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.ManyCyans);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(1 << 14, 0, 0, true)]
        [DataRow(~(1 << 14), ~0, ~0, false)]
        public void ManyGreensTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.ManyGreens);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(1 << 15, 0, 0, true)]
        [DataRow(~(1 << 15), ~0, ~0, false)]
        public void ManyYellowsTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.ManyYellows);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(1 << 16, 0, 0, true)]
        [DataRow(~(1 << 16), ~0, ~0, false)]
        public void ManyOrangesTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.ManyOranges);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(1 << 17, 0, 0, true)]
        [DataRow(~(1 << 17), ~0, ~0, false)]
        public void TooColorfulTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.TooColorful);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(1 << 18, 0, 0, true)]
        [DataRow(~(1 << 18), ~0, ~0, false)]
        public void SevenColorsTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.SevenColors);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(1 << 19, 0, 0, true)]
        [DataRow(~(1 << 19), ~0, ~0, false)]
        public void NoBulletTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.NoBullet);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(1 << 21, 0, 0, true)]
        [DataRow(~(1 << 21), ~0, ~0, false)]
        public void IsLandscapePhotoTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.IsLandscapePhoto);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(1 << 26, 0, 0, true)]
        [DataRow(~(1 << 26), ~0, ~0, false)]
        public void CloseupTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.Closeup);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(1 << 27, 0, 0, true)]
        [DataRow(~(1 << 27), ~0, ~0, false)]
        public void QuiteCloseupTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.QuiteCloseup);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(1 << 28, 0, 0, true)]
        [DataRow(~(1 << 28), ~0, ~0, false)]
        public void TooCloseTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.TooClose);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 1, 0, true)]
        [DataRow(~0, ~(1 << 1), ~0, false)]
        public void EnemyIsInFullViewTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.EnemyIsInFullView);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 4, 0, true)]
        [DataRow(~0, ~(1 << 4), ~0, false)]
        public void TooManyBulletsTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.TooManyBullets);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 5, 0, true)]
        [DataRow(~0, ~(1 << 5), ~0, false)]
        public void TooPlayfulBarrageTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.TooPlayfulBarrage);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 6, 0, true)]
        [DataRow(~0, ~(1 << 6), ~0, false)]
        public void TooDenseTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.TooDense);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 7, 0, true)]
        [DataRow(~0, ~(1 << 7), ~0, false)]
        public void ChasedTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.Chased);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 8, 0, true)]
        [DataRow(~0, ~(1 << 8), ~0, false)]
        public void IsSuppositoryTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.IsSuppository);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 9, 0, true)]
        [DataRow(~0, ~(1 << 9), ~0, false)]
        public void IsButterflyLikeMothTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.IsButterflyLikeMoth);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 10, 0, true)]
        [DataRow(~0, ~(1 << 10), ~0, false)]
        public void EnemyIsUndamagedTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.EnemyIsUndamaged);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 11, 0, true)]
        [DataRow(~0, ~(1 << 11), ~0, false)]
        public void EnemyCanAffordTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.EnemyCanAfford);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 12, 0, true)]
        [DataRow(~0, ~(1 << 12), ~0, false)]
        public void EnemyIsWeakenedTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.EnemyIsWeakened);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 13, 0, true)]
        [DataRow(~0, ~(1 << 13), ~0, false)]
        public void EnemyIsDyingTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.EnemyIsDying);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 14, 0, true)]
        [DataRow(~0, ~(1 << 14), ~0, false)]
        public void FinishedTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.Finished);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 15, 0, true)]
        [DataRow(~0, ~(1 << 15), ~0, false)]
        public void IsThreeShotTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.IsThreeShot);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 16, 0, true)]
        [DataRow(~0, ~(1 << 16), ~0, false)]
        public void TwoEnemiesTogetherTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.TwoEnemiesTogether);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 17, 0, true)]
        [DataRow(~0, ~(1 << 17), ~0, false)]
        public void EnemiesAreOverlappingTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.EnemiesAreOverlapping);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 18, 0, true)]
        [DataRow(~0, ~(1 << 18), ~0, false)]
        public void PeaceSignAlongsideTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.PeaceSignAlongside);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 19, 0, true)]
        [DataRow(~0, ~(1 << 19), ~0, false)]
        public void EnemiesAreTooCloseTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.EnemiesAreTooClose);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 20, 0, true)]
        [DataRow(~0, ~(1 << 20), ~0, false)]
        public void ScorchingTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.Scorching);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 21, 0, true)]
        [DataRow(~0, ~(1 << 21), ~0, false)]
        public void TooBigBulletTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.TooBigBullet);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 22, 0, true)]
        [DataRow(~0, ~(1 << 22), ~0, false)]
        public void ThrowingEdgedToolsTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.ThrowingEdgedTools);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 23, 0, true)]
        [DataRow(~0, ~(1 << 23), ~0, false)]
        public void SnakyTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.Snaky);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 24, 0, true)]
        [DataRow(~0, ~(1 << 24), ~0, false)]
        public void LightLooksStoppedTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.LightLooksStopped);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 25, 0, true)]
        [DataRow(~0, ~(1 << 25), ~0, false)]
        public void IsSuperMoonTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.IsSuperMoon);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 26, 0, true)]
        [DataRow(~0, ~(1 << 26), ~0, false)]
        public void DazzlingTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.Dazzling);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 27, 0, true)]
        [DataRow(~0, ~(1 << 27), ~0, false)]
        public void MoreDazzlingTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.MoreDazzling);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 28, 0, true)]
        [DataRow(~0, ~(1 << 28), ~0, false)]
        public void MostDazzlingTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.MostDazzling);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 29, 0, true)]
        [DataRow(~0, ~(1 << 29), ~0, false)]
        public void FinishedTogetherTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.FinishedTogether);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 30, 0, true)]
        [DataRow(~0, ~(1 << 30), ~0, false)]
        public void WasDreamTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.WasDream);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 1 << 31, 0, true)]
        [DataRow(~0, ~(1 << 31), ~0, false)]
        public void IsRockyBarrageTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.IsRockyBarrage);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 0, 1 << 0, true)]
        [DataRow(~0, ~0, ~(1 << 0), false)]
        public void IsStickDestroyingBarrageTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.IsStickDestroyingBarrage);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 0, 1 << 1, true)]
        [DataRow(~0, ~0, ~(1 << 1), false)]
        public void FluffyTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.Fluffy);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 0, 1 << 2, true)]
        [DataRow(~0, ~0, ~(1 << 2), false)]
        public void IsDoggiePhotoTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.IsDoggiePhoto);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 0, 1 << 3, true)]
        [DataRow(~0, ~0, ~(1 << 3), false)]
        public void IsAnimalPhotoTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.IsAnimalPhoto);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 0, 1 << 4, true)]
        [DataRow(~0, ~0, ~(1 << 4), false)]
        public void IsZooTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.IsZoo);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 0, 1 << 5, true)]
        [DataRow(~0, ~0, ~(1 << 5), false)]
        public void IsLovelyHeartTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.IsLovelyHeart);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 0, 1 << 6, true)]
        [DataRow(~0, ~0, ~(1 << 6), false)]
        public void IsThunderTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.IsThunder);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 0, 1 << 7, true)]
        [DataRow(~0, ~0, ~(1 << 7), false)]
        public void IsDrumTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.IsDrum);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 0, 1 << 8, true)]
        [DataRow(~0, ~0, ~(1 << 8), false)]
        public void IsMistyTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.IsMisty);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 0, 1 << 9, true)]
        [DataRow(~0, ~0, ~(1 << 9), false)]
        public void IsBoringPhotoTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.IsBoringPhoto);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 0, 1 << 10, true)]
        [DataRow(~0, ~0, ~(1 << 10), false)]
        public void WasScoldedTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.WasScolded);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0, 0, 1 << 11, true)]
        [DataRow(~0, ~0, ~(1 << 11), false)]
        public void IsSumirekoTest(int data1, int data2, int data3, bool expected) => TestUtils.Wrap(() =>
        {
            var fields = new Th165HashtagFieldsWrapper(data1, data2, data3);
            Assert.AreEqual(expected, fields.IsSumireko);
        });
    }
}
