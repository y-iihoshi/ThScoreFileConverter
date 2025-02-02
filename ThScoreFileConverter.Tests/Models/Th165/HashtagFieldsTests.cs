using ThScoreFileConverter.Models.Th165;

namespace ThScoreFileConverter.Tests.Models.Th165;

[TestClass]
public class HashtagFieldsTests
{
    [DataTestMethod]
    [DataRow(0, 12345678, int.MinValue)]
    [DataRow(12345678, int.MinValue, int.MaxValue)]
    [DataRow(int.MinValue, int.MaxValue, 0)]
    [DataRow(int.MaxValue, 0, 12345678)]
    public void DataTest(int data1, int data2, int data3)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.Data.ElementAt(0).ShouldBe(data1);
        fields.Data.ElementAt(1).ShouldBe(data2);
        fields.Data.ElementAt(2).ShouldBe(data3);
    }

    [DataTestMethod]
    [DataRow(1 << 0, 0, 0, true)]
    [DataRow(~(1 << 0), ~0, ~0, false)]
    public void EnemyIsInFrameTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.EnemyIsInFrame.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(1 << 1, 0, 0, true)]
    [DataRow(~(1 << 1), ~0, ~0, false)]
    public void EnemyIsPartlyInFrameTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.EnemyIsPartlyInFrame.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(1 << 2, 0, 0, true)]
    [DataRow(~(1 << 2), ~0, ~0, false)]
    public void WholeEnemyIsInFrameTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.WholeEnemyIsInFrame.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(1 << 3, 0, 0, true)]
    [DataRow(~(1 << 3), ~0, ~0, false)]
    public void EnemyIsInMiddleTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.EnemyIsInMiddle.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(1 << 4, 0, 0, true)]
    [DataRow(~(1 << 4), ~0, ~0, false)]
    public void IsSelfieTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.IsSelfie.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(1 << 5, 0, 0, true)]
    [DataRow(~(1 << 5), ~0, ~0, false)]
    public void IsTwoShotTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.IsTwoShot.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(1 << 7, 0, 0, true)]
    [DataRow(~(1 << 7), ~0, ~0, false)]
    public void BitDangerousTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.BitDangerous.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(1 << 8, 0, 0, true)]
    [DataRow(~(1 << 8), ~0, ~0, false)]
    public void SeriouslyDangerousTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.SeriouslyDangerous.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(1 << 9, 0, 0, true)]
    [DataRow(~(1 << 9), ~0, ~0, false)]
    public void ThoughtGonnaDieTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.ThoughtGonnaDie.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(1 << 10, 0, 0, true)]
    [DataRow(~(1 << 10), ~0, ~0, false)]
    public void ManyRedsTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.ManyReds.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(1 << 11, 0, 0, true)]
    [DataRow(~(1 << 11), ~0, ~0, false)]
    public void ManyPurplesTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.ManyPurples.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(1 << 12, 0, 0, true)]
    [DataRow(~(1 << 12), ~0, ~0, false)]
    public void ManyBluesTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.ManyBlues.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(1 << 13, 0, 0, true)]
    [DataRow(~(1 << 13), ~0, ~0, false)]
    public void ManyCyansTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.ManyCyans.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(1 << 14, 0, 0, true)]
    [DataRow(~(1 << 14), ~0, ~0, false)]
    public void ManyGreensTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.ManyGreens.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(1 << 15, 0, 0, true)]
    [DataRow(~(1 << 15), ~0, ~0, false)]
    public void ManyYellowsTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.ManyYellows.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(1 << 16, 0, 0, true)]
    [DataRow(~(1 << 16), ~0, ~0, false)]
    public void ManyOrangesTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.ManyOranges.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(1 << 17, 0, 0, true)]
    [DataRow(~(1 << 17), ~0, ~0, false)]
    public void TooColorfulTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.TooColorful.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(1 << 18, 0, 0, true)]
    [DataRow(~(1 << 18), ~0, ~0, false)]
    public void SevenColorsTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.SevenColors.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(1 << 19, 0, 0, true)]
    [DataRow(~(1 << 19), ~0, ~0, false)]
    public void NoBulletTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.NoBullet.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(1 << 21, 0, 0, true)]
    [DataRow(~(1 << 21), ~0, ~0, false)]
    public void IsLandscapePhotoTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.IsLandscapePhoto.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(1 << 26, 0, 0, true)]
    [DataRow(~(1 << 26), ~0, ~0, false)]
    public void CloseupTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.Closeup.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(1 << 27, 0, 0, true)]
    [DataRow(~(1 << 27), ~0, ~0, false)]
    public void QuiteCloseupTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.QuiteCloseup.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(1 << 28, 0, 0, true)]
    [DataRow(~(1 << 28), ~0, ~0, false)]
    public void TooCloseTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.TooClose.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 1, 0, true)]
    [DataRow(~0, ~(1 << 1), ~0, false)]
    public void EnemyIsInFullViewTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.EnemyIsInFullView.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 4, 0, true)]
    [DataRow(~0, ~(1 << 4), ~0, false)]
    public void TooManyBulletsTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.TooManyBullets.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 5, 0, true)]
    [DataRow(~0, ~(1 << 5), ~0, false)]
    public void TooPlayfulBarrageTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.TooPlayfulBarrage.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 6, 0, true)]
    [DataRow(~0, ~(1 << 6), ~0, false)]
    public void TooDenseTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.TooDense.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 7, 0, true)]
    [DataRow(~0, ~(1 << 7), ~0, false)]
    public void ChasedTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.Chased.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 8, 0, true)]
    [DataRow(~0, ~(1 << 8), ~0, false)]
    public void IsSuppositoryTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.IsSuppository.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 9, 0, true)]
    [DataRow(~0, ~(1 << 9), ~0, false)]
    public void IsButterflyLikeMothTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.IsButterflyLikeMoth.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 10, 0, true)]
    [DataRow(~0, ~(1 << 10), ~0, false)]
    public void EnemyIsUndamagedTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.EnemyIsUndamaged.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 11, 0, true)]
    [DataRow(~0, ~(1 << 11), ~0, false)]
    public void EnemyCanAffordTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.EnemyCanAfford.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 12, 0, true)]
    [DataRow(~0, ~(1 << 12), ~0, false)]
    public void EnemyIsWeakenedTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.EnemyIsWeakened.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 13, 0, true)]
    [DataRow(~0, ~(1 << 13), ~0, false)]
    public void EnemyIsDyingTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.EnemyIsDying.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 14, 0, true)]
    [DataRow(~0, ~(1 << 14), ~0, false)]
    public void FinishedTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.Finished.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 15, 0, true)]
    [DataRow(~0, ~(1 << 15), ~0, false)]
    public void IsThreeShotTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.IsThreeShot.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 16, 0, true)]
    [DataRow(~0, ~(1 << 16), ~0, false)]
    public void TwoEnemiesTogetherTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.TwoEnemiesTogether.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 17, 0, true)]
    [DataRow(~0, ~(1 << 17), ~0, false)]
    public void EnemiesAreOverlappingTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.EnemiesAreOverlapping.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 18, 0, true)]
    [DataRow(~0, ~(1 << 18), ~0, false)]
    public void PeaceSignAlongsideTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.PeaceSignAlongside.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 19, 0, true)]
    [DataRow(~0, ~(1 << 19), ~0, false)]
    public void EnemiesAreTooCloseTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.EnemiesAreTooClose.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 20, 0, true)]
    [DataRow(~0, ~(1 << 20), ~0, false)]
    public void ScorchingTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.Scorching.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 21, 0, true)]
    [DataRow(~0, ~(1 << 21), ~0, false)]
    public void TooBigBulletTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.TooBigBullet.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 22, 0, true)]
    [DataRow(~0, ~(1 << 22), ~0, false)]
    public void ThrowingEdgedToolsTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.ThrowingEdgedTools.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 23, 0, true)]
    [DataRow(~0, ~(1 << 23), ~0, false)]
    public void SnakyTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.Snaky.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 24, 0, true)]
    [DataRow(~0, ~(1 << 24), ~0, false)]
    public void LightLooksStoppedTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.LightLooksStopped.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 25, 0, true)]
    [DataRow(~0, ~(1 << 25), ~0, false)]
    public void IsSuperMoonTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.IsSuperMoon.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 26, 0, true)]
    [DataRow(~0, ~(1 << 26), ~0, false)]
    public void DazzlingTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.Dazzling.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 27, 0, true)]
    [DataRow(~0, ~(1 << 27), ~0, false)]
    public void MoreDazzlingTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.MoreDazzling.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 28, 0, true)]
    [DataRow(~0, ~(1 << 28), ~0, false)]
    public void MostDazzlingTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.MostDazzling.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 29, 0, true)]
    [DataRow(~0, ~(1 << 29), ~0, false)]
    public void FinishedTogetherTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.FinishedTogether.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 30, 0, true)]
    [DataRow(~0, ~(1 << 30), ~0, false)]
    public void WasDreamTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.WasDream.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 31, 0, true)]
    [DataRow(~0, ~(1 << 31), ~0, false)]
    public void IsRockyBarrageTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.IsRockyBarrage.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 0, 1 << 0, true)]
    [DataRow(~0, ~0, ~(1 << 0), false)]
    public void IsStickDestroyingBarrageTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.IsStickDestroyingBarrage.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 0, 1 << 1, true)]
    [DataRow(~0, ~0, ~(1 << 1), false)]
    public void FluffyTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.Fluffy.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 0, 1 << 2, true)]
    [DataRow(~0, ~0, ~(1 << 2), false)]
    public void IsDoggiePhotoTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.IsDoggiePhoto.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 0, 1 << 3, true)]
    [DataRow(~0, ~0, ~(1 << 3), false)]
    public void IsAnimalPhotoTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.IsAnimalPhoto.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 0, 1 << 4, true)]
    [DataRow(~0, ~0, ~(1 << 4), false)]
    public void IsZooTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.IsZoo.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 0, 1 << 5, true)]
    [DataRow(~0, ~0, ~(1 << 5), false)]
    public void IsLovelyHeartTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.IsLovelyHeart.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 0, 1 << 6, true)]
    [DataRow(~0, ~0, ~(1 << 6), false)]
    public void IsThunderTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.IsThunder.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 0, 1 << 7, true)]
    [DataRow(~0, ~0, ~(1 << 7), false)]
    public void IsDrumTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.IsDrum.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 0, 1 << 8, true)]
    [DataRow(~0, ~0, ~(1 << 8), false)]
    public void IsMistyTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.IsMisty.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 0, 1 << 9, true)]
    [DataRow(~0, ~0, ~(1 << 9), false)]
    public void IsBoringPhotoTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.IsBoringPhoto.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 0, 1 << 10, true)]
    [DataRow(~0, ~0, ~(1 << 10), false)]
    public void WasScoldedTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.WasScolded.ShouldBe(expected);
    }

    [DataTestMethod]
    [DataRow(0, 0, 1 << 11, true)]
    [DataRow(~0, ~0, ~(1 << 11), false)]
    public void IsSumirekoTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        fields.IsSumireko.ShouldBe(expected);
    }

    [TestMethod]
    public void DefaultConstructorTest()
    {
        var fields = (new HashtagFields[1])[0];

        fields.Data.ShouldBeEmpty();
        foreach (var prop in fields.GetType().GetProperties().Where(prop => prop.PropertyType == typeof(bool)))
        {
            var value = prop.GetValue(fields);
            value.ShouldBeOfType<bool>().ShouldBeFalse();
        }
    }
}
