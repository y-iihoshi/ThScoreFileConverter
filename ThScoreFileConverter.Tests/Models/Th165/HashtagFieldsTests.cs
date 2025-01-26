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
        Assert.AreEqual(data1, fields.Data.ElementAt(0));
        Assert.AreEqual(data2, fields.Data.ElementAt(1));
        Assert.AreEqual(data3, fields.Data.ElementAt(2));
    }

    [DataTestMethod]
    [DataRow(1 << 0, 0, 0, true)]
    [DataRow(~(1 << 0), ~0, ~0, false)]
    public void EnemyIsInFrameTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.EnemyIsInFrame);
    }

    [DataTestMethod]
    [DataRow(1 << 1, 0, 0, true)]
    [DataRow(~(1 << 1), ~0, ~0, false)]
    public void EnemyIsPartlyInFrameTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.EnemyIsPartlyInFrame);
    }

    [DataTestMethod]
    [DataRow(1 << 2, 0, 0, true)]
    [DataRow(~(1 << 2), ~0, ~0, false)]
    public void WholeEnemyIsInFrameTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.WholeEnemyIsInFrame);
    }

    [DataTestMethod]
    [DataRow(1 << 3, 0, 0, true)]
    [DataRow(~(1 << 3), ~0, ~0, false)]
    public void EnemyIsInMiddleTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.EnemyIsInMiddle);
    }

    [DataTestMethod]
    [DataRow(1 << 4, 0, 0, true)]
    [DataRow(~(1 << 4), ~0, ~0, false)]
    public void IsSelfieTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.IsSelfie);
    }

    [DataTestMethod]
    [DataRow(1 << 5, 0, 0, true)]
    [DataRow(~(1 << 5), ~0, ~0, false)]
    public void IsTwoShotTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.IsTwoShot);
    }

    [DataTestMethod]
    [DataRow(1 << 7, 0, 0, true)]
    [DataRow(~(1 << 7), ~0, ~0, false)]
    public void BitDangerousTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.BitDangerous);
    }

    [DataTestMethod]
    [DataRow(1 << 8, 0, 0, true)]
    [DataRow(~(1 << 8), ~0, ~0, false)]
    public void SeriouslyDangerousTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.SeriouslyDangerous);
    }

    [DataTestMethod]
    [DataRow(1 << 9, 0, 0, true)]
    [DataRow(~(1 << 9), ~0, ~0, false)]
    public void ThoughtGonnaDieTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.ThoughtGonnaDie);
    }

    [DataTestMethod]
    [DataRow(1 << 10, 0, 0, true)]
    [DataRow(~(1 << 10), ~0, ~0, false)]
    public void ManyRedsTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.ManyReds);
    }

    [DataTestMethod]
    [DataRow(1 << 11, 0, 0, true)]
    [DataRow(~(1 << 11), ~0, ~0, false)]
    public void ManyPurplesTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.ManyPurples);
    }

    [DataTestMethod]
    [DataRow(1 << 12, 0, 0, true)]
    [DataRow(~(1 << 12), ~0, ~0, false)]
    public void ManyBluesTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.ManyBlues);
    }

    [DataTestMethod]
    [DataRow(1 << 13, 0, 0, true)]
    [DataRow(~(1 << 13), ~0, ~0, false)]
    public void ManyCyansTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.ManyCyans);
    }

    [DataTestMethod]
    [DataRow(1 << 14, 0, 0, true)]
    [DataRow(~(1 << 14), ~0, ~0, false)]
    public void ManyGreensTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.ManyGreens);
    }

    [DataTestMethod]
    [DataRow(1 << 15, 0, 0, true)]
    [DataRow(~(1 << 15), ~0, ~0, false)]
    public void ManyYellowsTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.ManyYellows);
    }

    [DataTestMethod]
    [DataRow(1 << 16, 0, 0, true)]
    [DataRow(~(1 << 16), ~0, ~0, false)]
    public void ManyOrangesTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.ManyOranges);
    }

    [DataTestMethod]
    [DataRow(1 << 17, 0, 0, true)]
    [DataRow(~(1 << 17), ~0, ~0, false)]
    public void TooColorfulTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.TooColorful);
    }

    [DataTestMethod]
    [DataRow(1 << 18, 0, 0, true)]
    [DataRow(~(1 << 18), ~0, ~0, false)]
    public void SevenColorsTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.SevenColors);
    }

    [DataTestMethod]
    [DataRow(1 << 19, 0, 0, true)]
    [DataRow(~(1 << 19), ~0, ~0, false)]
    public void NoBulletTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.NoBullet);
    }

    [DataTestMethod]
    [DataRow(1 << 21, 0, 0, true)]
    [DataRow(~(1 << 21), ~0, ~0, false)]
    public void IsLandscapePhotoTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.IsLandscapePhoto);
    }

    [DataTestMethod]
    [DataRow(1 << 26, 0, 0, true)]
    [DataRow(~(1 << 26), ~0, ~0, false)]
    public void CloseupTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.Closeup);
    }

    [DataTestMethod]
    [DataRow(1 << 27, 0, 0, true)]
    [DataRow(~(1 << 27), ~0, ~0, false)]
    public void QuiteCloseupTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.QuiteCloseup);
    }

    [DataTestMethod]
    [DataRow(1 << 28, 0, 0, true)]
    [DataRow(~(1 << 28), ~0, ~0, false)]
    public void TooCloseTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.TooClose);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 1, 0, true)]
    [DataRow(~0, ~(1 << 1), ~0, false)]
    public void EnemyIsInFullViewTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.EnemyIsInFullView);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 4, 0, true)]
    [DataRow(~0, ~(1 << 4), ~0, false)]
    public void TooManyBulletsTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.TooManyBullets);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 5, 0, true)]
    [DataRow(~0, ~(1 << 5), ~0, false)]
    public void TooPlayfulBarrageTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.TooPlayfulBarrage);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 6, 0, true)]
    [DataRow(~0, ~(1 << 6), ~0, false)]
    public void TooDenseTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.TooDense);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 7, 0, true)]
    [DataRow(~0, ~(1 << 7), ~0, false)]
    public void ChasedTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.Chased);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 8, 0, true)]
    [DataRow(~0, ~(1 << 8), ~0, false)]
    public void IsSuppositoryTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.IsSuppository);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 9, 0, true)]
    [DataRow(~0, ~(1 << 9), ~0, false)]
    public void IsButterflyLikeMothTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.IsButterflyLikeMoth);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 10, 0, true)]
    [DataRow(~0, ~(1 << 10), ~0, false)]
    public void EnemyIsUndamagedTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.EnemyIsUndamaged);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 11, 0, true)]
    [DataRow(~0, ~(1 << 11), ~0, false)]
    public void EnemyCanAffordTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.EnemyCanAfford);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 12, 0, true)]
    [DataRow(~0, ~(1 << 12), ~0, false)]
    public void EnemyIsWeakenedTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.EnemyIsWeakened);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 13, 0, true)]
    [DataRow(~0, ~(1 << 13), ~0, false)]
    public void EnemyIsDyingTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.EnemyIsDying);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 14, 0, true)]
    [DataRow(~0, ~(1 << 14), ~0, false)]
    public void FinishedTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.Finished);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 15, 0, true)]
    [DataRow(~0, ~(1 << 15), ~0, false)]
    public void IsThreeShotTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.IsThreeShot);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 16, 0, true)]
    [DataRow(~0, ~(1 << 16), ~0, false)]
    public void TwoEnemiesTogetherTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.TwoEnemiesTogether);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 17, 0, true)]
    [DataRow(~0, ~(1 << 17), ~0, false)]
    public void EnemiesAreOverlappingTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.EnemiesAreOverlapping);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 18, 0, true)]
    [DataRow(~0, ~(1 << 18), ~0, false)]
    public void PeaceSignAlongsideTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.PeaceSignAlongside);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 19, 0, true)]
    [DataRow(~0, ~(1 << 19), ~0, false)]
    public void EnemiesAreTooCloseTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.EnemiesAreTooClose);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 20, 0, true)]
    [DataRow(~0, ~(1 << 20), ~0, false)]
    public void ScorchingTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.Scorching);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 21, 0, true)]
    [DataRow(~0, ~(1 << 21), ~0, false)]
    public void TooBigBulletTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.TooBigBullet);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 22, 0, true)]
    [DataRow(~0, ~(1 << 22), ~0, false)]
    public void ThrowingEdgedToolsTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.ThrowingEdgedTools);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 23, 0, true)]
    [DataRow(~0, ~(1 << 23), ~0, false)]
    public void SnakyTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.Snaky);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 24, 0, true)]
    [DataRow(~0, ~(1 << 24), ~0, false)]
    public void LightLooksStoppedTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.LightLooksStopped);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 25, 0, true)]
    [DataRow(~0, ~(1 << 25), ~0, false)]
    public void IsSuperMoonTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.IsSuperMoon);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 26, 0, true)]
    [DataRow(~0, ~(1 << 26), ~0, false)]
    public void DazzlingTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.Dazzling);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 27, 0, true)]
    [DataRow(~0, ~(1 << 27), ~0, false)]
    public void MoreDazzlingTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.MoreDazzling);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 28, 0, true)]
    [DataRow(~0, ~(1 << 28), ~0, false)]
    public void MostDazzlingTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.MostDazzling);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 29, 0, true)]
    [DataRow(~0, ~(1 << 29), ~0, false)]
    public void FinishedTogetherTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.FinishedTogether);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 30, 0, true)]
    [DataRow(~0, ~(1 << 30), ~0, false)]
    public void WasDreamTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.WasDream);
    }

    [DataTestMethod]
    [DataRow(0, 1 << 31, 0, true)]
    [DataRow(~0, ~(1 << 31), ~0, false)]
    public void IsRockyBarrageTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.IsRockyBarrage);
    }

    [DataTestMethod]
    [DataRow(0, 0, 1 << 0, true)]
    [DataRow(~0, ~0, ~(1 << 0), false)]
    public void IsStickDestroyingBarrageTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.IsStickDestroyingBarrage);
    }

    [DataTestMethod]
    [DataRow(0, 0, 1 << 1, true)]
    [DataRow(~0, ~0, ~(1 << 1), false)]
    public void FluffyTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.Fluffy);
    }

    [DataTestMethod]
    [DataRow(0, 0, 1 << 2, true)]
    [DataRow(~0, ~0, ~(1 << 2), false)]
    public void IsDoggiePhotoTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.IsDoggiePhoto);
    }

    [DataTestMethod]
    [DataRow(0, 0, 1 << 3, true)]
    [DataRow(~0, ~0, ~(1 << 3), false)]
    public void IsAnimalPhotoTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.IsAnimalPhoto);
    }

    [DataTestMethod]
    [DataRow(0, 0, 1 << 4, true)]
    [DataRow(~0, ~0, ~(1 << 4), false)]
    public void IsZooTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.IsZoo);
    }

    [DataTestMethod]
    [DataRow(0, 0, 1 << 5, true)]
    [DataRow(~0, ~0, ~(1 << 5), false)]
    public void IsLovelyHeartTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.IsLovelyHeart);
    }

    [DataTestMethod]
    [DataRow(0, 0, 1 << 6, true)]
    [DataRow(~0, ~0, ~(1 << 6), false)]
    public void IsThunderTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.IsThunder);
    }

    [DataTestMethod]
    [DataRow(0, 0, 1 << 7, true)]
    [DataRow(~0, ~0, ~(1 << 7), false)]
    public void IsDrumTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.IsDrum);
    }

    [DataTestMethod]
    [DataRow(0, 0, 1 << 8, true)]
    [DataRow(~0, ~0, ~(1 << 8), false)]
    public void IsMistyTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.IsMisty);
    }

    [DataTestMethod]
    [DataRow(0, 0, 1 << 9, true)]
    [DataRow(~0, ~0, ~(1 << 9), false)]
    public void IsBoringPhotoTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.IsBoringPhoto);
    }

    [DataTestMethod]
    [DataRow(0, 0, 1 << 10, true)]
    [DataRow(~0, ~0, ~(1 << 10), false)]
    public void WasScoldedTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.WasScolded);
    }

    [DataTestMethod]
    [DataRow(0, 0, 1 << 11, true)]
    [DataRow(~0, ~0, ~(1 << 11), false)]
    public void IsSumirekoTest(int data1, int data2, int data3, bool expected)
    {
        var fields = new HashtagFields(data1, data2, data3);
        Assert.AreEqual(expected, fields.IsSumireko);
    }

    [TestMethod]
    public void DefaultConstructorTest()
    {
        var fields = (new HashtagFields[1])[0];

        Assert.AreEqual(0, fields.Data.Count());
        foreach (var prop in fields.GetType().GetProperties().Where(prop => prop.PropertyType == typeof(bool)))
        {
            var value = prop.GetValue(fields);
            Assert.IsTrue((value is bool boolean) && !boolean);
        }
    }
}
