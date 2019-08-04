using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    public sealed class Th165HashtagFieldsWrapper
    {
        private static readonly Type ConverterType = typeof(Th165Converter);
        private static readonly string AssemblyNameToTest = ConverterType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ConverterType.FullName + "+BestShotHeader+HashtagFields";

        private readonly PrivateObject pobj = null;

        public Th165HashtagFieldsWrapper(int data1, int data2, int data3)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { data1, data2, data3 });
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "obj")]
        public Th165HashtagFieldsWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;

        public IEnumerable<int> Data
            => this.pobj.GetProperty(nameof(this.Data)) as IEnumerable<int>;
        public bool? EnemyIsInFrame
            => this.pobj.GetProperty(nameof(this.EnemyIsInFrame)) as bool?;
        public bool? EnemyIsPartlyInFrame
            => this.pobj.GetProperty(nameof(this.EnemyIsPartlyInFrame)) as bool?;
        public bool? WholeEnemyIsInFrame
            => this.pobj.GetProperty(nameof(this.WholeEnemyIsInFrame)) as bool?;
        public bool? EnemyIsInMiddle
            => this.pobj.GetProperty(nameof(this.EnemyIsInMiddle)) as bool?;
        public bool? IsSelfie
            => this.pobj.GetProperty(nameof(this.IsSelfie)) as bool?;
        public bool? IsTwoShot
            => this.pobj.GetProperty(nameof(this.IsTwoShot)) as bool?;
        public bool? BitDangerous
            => this.pobj.GetProperty(nameof(this.BitDangerous)) as bool?;
        public bool? SeriouslyDangerous
            => this.pobj.GetProperty(nameof(this.SeriouslyDangerous)) as bool?;
        public bool? ThoughtGonnaDie
            => this.pobj.GetProperty(nameof(this.ThoughtGonnaDie)) as bool?;
        public bool? ManyReds
            => this.pobj.GetProperty(nameof(this.ManyReds)) as bool?;
        public bool? ManyPurples
            => this.pobj.GetProperty(nameof(this.ManyPurples)) as bool?;
        public bool? ManyBlues
            => this.pobj.GetProperty(nameof(this.ManyBlues)) as bool?;
        public bool? ManyCyans
            => this.pobj.GetProperty(nameof(this.ManyCyans)) as bool?;
        public bool? ManyGreens
            => this.pobj.GetProperty(nameof(this.ManyGreens)) as bool?;
        public bool? ManyYellows
            => this.pobj.GetProperty(nameof(this.ManyYellows)) as bool?;
        public bool? ManyOranges
            => this.pobj.GetProperty(nameof(this.ManyOranges)) as bool?;
        public bool? TooColorful
            => this.pobj.GetProperty(nameof(this.TooColorful)) as bool?;
        public bool? SevenColors
            => this.pobj.GetProperty(nameof(this.SevenColors)) as bool?;
        public bool? NoBullet
            => this.pobj.GetProperty(nameof(this.NoBullet)) as bool?;
        public bool? IsLandscapePhoto
            => this.pobj.GetProperty(nameof(this.IsLandscapePhoto)) as bool?;
        public bool? Closeup
            => this.pobj.GetProperty(nameof(this.Closeup)) as bool?;
        public bool? QuiteCloseup
            => this.pobj.GetProperty(nameof(this.QuiteCloseup)) as bool?;
        public bool? TooClose
            => this.pobj.GetProperty(nameof(this.TooClose)) as bool?;
        public bool? EnemyIsInFullView
            => this.pobj.GetProperty(nameof(this.EnemyIsInFullView)) as bool?;
        public bool? TooManyBullets
            => this.pobj.GetProperty(nameof(this.TooManyBullets)) as bool?;
        public bool? TooPlayfulBarrage
            => this.pobj.GetProperty(nameof(this.TooPlayfulBarrage)) as bool?;
        public bool? TooDense
            => this.pobj.GetProperty(nameof(this.TooDense)) as bool?;
        public bool? Chased
            => this.pobj.GetProperty(nameof(this.Chased)) as bool?;
        public bool? IsSuppository
            => this.pobj.GetProperty(nameof(this.IsSuppository)) as bool?;
        public bool? IsButterflyLikeMoth
            => this.pobj.GetProperty(nameof(this.IsButterflyLikeMoth)) as bool?;
        public bool? EnemyIsUndamaged
            => this.pobj.GetProperty(nameof(this.EnemyIsUndamaged)) as bool?;
        public bool? EnemyCanAfford
            => this.pobj.GetProperty(nameof(this.EnemyCanAfford)) as bool?;
        public bool? EnemyIsWeakened
            => this.pobj.GetProperty(nameof(this.EnemyIsWeakened)) as bool?;
        public bool? EnemyIsDying
            => this.pobj.GetProperty(nameof(this.EnemyIsDying)) as bool?;
        public bool? Finished
            => this.pobj.GetProperty(nameof(this.Finished)) as bool?;
        public bool? IsThreeShot
            => this.pobj.GetProperty(nameof(this.IsThreeShot)) as bool?;
        public bool? TwoEnemiesTogether
            => this.pobj.GetProperty(nameof(this.TwoEnemiesTogether)) as bool?;
        public bool? EnemiesAreOverlapping
            => this.pobj.GetProperty(nameof(this.EnemiesAreOverlapping)) as bool?;
        public bool? PeaceSignAlongside
            => this.pobj.GetProperty(nameof(this.PeaceSignAlongside)) as bool?;
        public bool? EnemiesAreTooClose
            => this.pobj.GetProperty(nameof(this.EnemiesAreTooClose)) as bool?;
        public bool? Scorching
            => this.pobj.GetProperty(nameof(this.Scorching)) as bool?;
        public bool? TooBigBullet
            => this.pobj.GetProperty(nameof(this.TooBigBullet)) as bool?;
        public bool? ThrowingEdgedTools
            => this.pobj.GetProperty(nameof(this.ThrowingEdgedTools)) as bool?;
        public bool? Snaky
            => this.pobj.GetProperty(nameof(this.Snaky)) as bool?;
        public bool? LightLooksStopped
            => this.pobj.GetProperty(nameof(this.LightLooksStopped)) as bool?;
        public bool? IsSuperMoon
            => this.pobj.GetProperty(nameof(this.IsSuperMoon)) as bool?;
        public bool? Dazzling
            => this.pobj.GetProperty(nameof(this.Dazzling)) as bool?;
        public bool? MoreDazzling
            => this.pobj.GetProperty(nameof(this.MoreDazzling)) as bool?;
        public bool? MostDazzling
            => this.pobj.GetProperty(nameof(this.MostDazzling)) as bool?;
        public bool? FinishedTogether
            => this.pobj.GetProperty(nameof(this.FinishedTogether)) as bool?;
        public bool? WasDream
            => this.pobj.GetProperty(nameof(this.WasDream)) as bool?;
        public bool? IsRockyBarrage
            => this.pobj.GetProperty(nameof(this.IsRockyBarrage)) as bool?;
        public bool? IsStickDestroyingBarrage
            => this.pobj.GetProperty(nameof(this.IsStickDestroyingBarrage)) as bool?;
        public bool? Fluffy
            => this.pobj.GetProperty(nameof(this.Fluffy)) as bool?;
        public bool? IsDoggiePhoto
            => this.pobj.GetProperty(nameof(this.IsDoggiePhoto)) as bool?;
        public bool? IsAnimalPhoto
            => this.pobj.GetProperty(nameof(this.IsAnimalPhoto)) as bool?;
        public bool? IsZoo
            => this.pobj.GetProperty(nameof(this.IsZoo)) as bool?;
        public bool? IsLovelyHeart
            => this.pobj.GetProperty(nameof(this.IsLovelyHeart)) as bool?;
        public bool? IsThunder
            => this.pobj.GetProperty(nameof(this.IsThunder)) as bool?;
        public bool? IsDrum
            => this.pobj.GetProperty(nameof(this.IsDrum)) as bool?;
        public bool? IsMisty
            => this.pobj.GetProperty(nameof(this.IsMisty)) as bool?;
        public bool? IsBoringPhoto
            => this.pobj.GetProperty(nameof(this.IsBoringPhoto)) as bool?;
        public bool? WasScolded
            => this.pobj.GetProperty(nameof(this.WasScolded)) as bool?;
        public bool? IsSumireko
            => this.pobj.GetProperty(nameof(this.IsSumireko)) as bool?;
    }
}
