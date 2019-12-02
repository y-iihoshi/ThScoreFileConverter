//-----------------------------------------------------------------------
// <copyright file="HashtagFields.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace ThScoreFileConverter.Models.Th165
{
    internal struct HashtagFields
    {
        private static readonly int[] Masks;

        private readonly BitVector32[] data;

#pragma warning disable CA2207 // Initialize value type static fields inline
        static HashtagFields()
        {
            Masks = new int[32];
            Masks[0] = BitVector32.CreateMask();
            for (var i = 1; i < Masks.Length; i++)
            {
                Masks[i] = BitVector32.CreateMask(Masks[i - 1]);
            }
        }
#pragma warning restore CA2207 // Initialize value type static fields inline

        public HashtagFields(int data1, int data2, int data3)
        {
            this.data = new BitVector32[3];
            this.data[0] = new BitVector32(data1);
            this.data[1] = new BitVector32(data2);
            this.data[2] = new BitVector32(data3);
        }

        public IEnumerable<int> Data => this.data?.Select(vector => vector.Data);

        public bool EnemyIsInFrame => this.data[0][Masks[0]]; // Not used

        public bool EnemyIsPartlyInFrame => this.data[0][Masks[1]];

        public bool WholeEnemyIsInFrame => this.data[0][Masks[2]];

        public bool EnemyIsInMiddle => this.data[0][Masks[3]];

        public bool IsSelfie => this.data[0][Masks[4]];

        public bool IsTwoShot => this.data[0][Masks[5]];

        public bool BitDangerous => this.data[0][Masks[7]];

        public bool SeriouslyDangerous => this.data[0][Masks[8]];

        public bool ThoughtGonnaDie => this.data[0][Masks[9]];

        public bool ManyReds => this.data[0][Masks[10]];

        public bool ManyPurples => this.data[0][Masks[11]];

        public bool ManyBlues => this.data[0][Masks[12]];

        public bool ManyCyans => this.data[0][Masks[13]];

        public bool ManyGreens => this.data[0][Masks[14]];

        public bool ManyYellows => this.data[0][Masks[15]];

        public bool ManyOranges => this.data[0][Masks[16]];

        public bool TooColorful => this.data[0][Masks[17]];

        public bool SevenColors => this.data[0][Masks[18]];

        public bool NoBullet => this.data[0][Masks[19]]; // Not used

        public bool IsLandscapePhoto => this.data[0][Masks[21]];

        public bool Closeup => this.data[0][Masks[26]];

        public bool QuiteCloseup => this.data[0][Masks[27]];

        public bool TooClose => this.data[0][Masks[28]];

        public bool EnemyIsInFullView => this.data[1][Masks[1]];

        public bool TooManyBullets => this.data[1][Masks[4]];

        public bool TooPlayfulBarrage => this.data[1][Masks[5]];

        public bool TooDense => this.data[1][Masks[6]]; // FIXME

        public bool Chased => this.data[1][Masks[7]];

        public bool IsSuppository => this.data[1][Masks[8]];

        public bool IsButterflyLikeMoth => this.data[1][Masks[9]];

        public bool EnemyIsUndamaged => this.data[1][Masks[10]];

        public bool EnemyCanAfford => this.data[1][Masks[11]];

        public bool EnemyIsWeakened => this.data[1][Masks[12]];

        public bool EnemyIsDying => this.data[1][Masks[13]];

        public bool Finished => this.data[1][Masks[14]];

        public bool IsThreeShot => this.data[1][Masks[15]];

        public bool TwoEnemiesTogether => this.data[1][Masks[16]];

        public bool EnemiesAreOverlapping => this.data[1][Masks[17]];

        public bool PeaceSignAlongside => this.data[1][Masks[18]];

        public bool EnemiesAreTooClose => this.data[1][Masks[19]]; // FIXME

        public bool Scorching => this.data[1][Masks[20]];

        public bool TooBigBullet => this.data[1][Masks[21]];

        public bool ThrowingEdgedTools => this.data[1][Masks[22]];

        public bool Snaky => this.data[1][Masks[23]];

        public bool LightLooksStopped => this.data[1][Masks[24]];

        public bool IsSuperMoon => this.data[1][Masks[25]];

        public bool Dazzling => this.data[1][Masks[26]];

        public bool MoreDazzling => this.data[1][Masks[27]];

        public bool MostDazzling => this.data[1][Masks[28]];

        public bool FinishedTogether => this.data[1][Masks[29]];

        public bool WasDream => this.data[1][Masks[30]]; // FIXME; Not used

        public bool IsRockyBarrage => this.data[1][Masks[31]];

        public bool IsStickDestroyingBarrage => this.data[2][Masks[0]];

        public bool Fluffy => this.data[2][Masks[1]];

        public bool IsDoggiePhoto => this.data[2][Masks[2]];

        public bool IsAnimalPhoto => this.data[2][Masks[3]];

        public bool IsZoo => this.data[2][Masks[4]];

        public bool IsLovelyHeart => this.data[2][Masks[5]]; // FIXME

        public bool IsThunder => this.data[2][Masks[6]];

        public bool IsDrum => this.data[2][Masks[7]];

        public bool IsMisty => this.data[2][Masks[8]]; // FIXME

        public bool IsBoringPhoto => this.data[2][Masks[9]];

        public bool WasScolded => this.data[2][Masks[10]]; // FIXME

        public bool IsSumireko => this.data[2][Masks[11]];
    }
}
