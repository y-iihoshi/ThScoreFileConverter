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

namespace ThScoreFileConverter.Models.Th165;

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
        this.data = new[] { new BitVector32(data1), new BitVector32(data2), new BitVector32(data3) };
    }

    public IEnumerable<int> Data => this.data?.Select(vector => vector.Data) ?? Enumerable.Empty<int>();

    public bool EnemyIsInFrame => this.data?[0][Masks[0]] ?? false; // Not used

    public bool EnemyIsPartlyInFrame => this.data?[0][Masks[1]] ?? false;

    public bool WholeEnemyIsInFrame => this.data?[0][Masks[2]] ?? false;

    public bool EnemyIsInMiddle => this.data?[0][Masks[3]] ?? false;

    public bool IsSelfie => this.data?[0][Masks[4]] ?? false;

    public bool IsTwoShot => this.data?[0][Masks[5]] ?? false;

    public bool BitDangerous => this.data?[0][Masks[7]] ?? false;

    public bool SeriouslyDangerous => this.data?[0][Masks[8]] ?? false;

    public bool ThoughtGonnaDie => this.data?[0][Masks[9]] ?? false;

    public bool ManyReds => this.data?[0][Masks[10]] ?? false;

    public bool ManyPurples => this.data?[0][Masks[11]] ?? false;

    public bool ManyBlues => this.data?[0][Masks[12]] ?? false;

    public bool ManyCyans => this.data?[0][Masks[13]] ?? false;

    public bool ManyGreens => this.data?[0][Masks[14]] ?? false;

    public bool ManyYellows => this.data?[0][Masks[15]] ?? false;

    public bool ManyOranges => this.data?[0][Masks[16]] ?? false;

    public bool TooColorful => this.data?[0][Masks[17]] ?? false;

    public bool SevenColors => this.data?[0][Masks[18]] ?? false;

    public bool NoBullet => this.data?[0][Masks[19]] ?? false; // Not used

    public bool IsLandscapePhoto => this.data?[0][Masks[21]] ?? false;

    public bool Closeup => this.data?[0][Masks[26]] ?? false;

    public bool QuiteCloseup => this.data?[0][Masks[27]] ?? false;

    public bool TooClose => this.data?[0][Masks[28]] ?? false;

    public bool EnemyIsInFullView => this.data?[1][Masks[1]] ?? false;

    public bool TooManyBullets => this.data?[1][Masks[4]] ?? false;

    public bool TooPlayfulBarrage => this.data?[1][Masks[5]] ?? false;

    public bool TooDense => this.data?[1][Masks[6]] ?? false; // FIXME

    public bool Chased => this.data?[1][Masks[7]] ?? false;

    public bool IsSuppository => this.data?[1][Masks[8]] ?? false;

    public bool IsButterflyLikeMoth => this.data?[1][Masks[9]] ?? false;

    public bool EnemyIsUndamaged => this.data?[1][Masks[10]] ?? false;

    public bool EnemyCanAfford => this.data?[1][Masks[11]] ?? false;

    public bool EnemyIsWeakened => this.data?[1][Masks[12]] ?? false;

    public bool EnemyIsDying => this.data?[1][Masks[13]] ?? false;

    public bool Finished => this.data?[1][Masks[14]] ?? false;

    public bool IsThreeShot => this.data?[1][Masks[15]] ?? false;

    public bool TwoEnemiesTogether => this.data?[1][Masks[16]] ?? false;

    public bool EnemiesAreOverlapping => this.data?[1][Masks[17]] ?? false;

    public bool PeaceSignAlongside => this.data?[1][Masks[18]] ?? false;

    public bool EnemiesAreTooClose => this.data?[1][Masks[19]] ?? false; // FIXME

    public bool Scorching => this.data?[1][Masks[20]] ?? false;

    public bool TooBigBullet => this.data?[1][Masks[21]] ?? false;

    public bool ThrowingEdgedTools => this.data?[1][Masks[22]] ?? false;

    public bool Snaky => this.data?[1][Masks[23]] ?? false;

    public bool LightLooksStopped => this.data?[1][Masks[24]] ?? false;

    public bool IsSuperMoon => this.data?[1][Masks[25]] ?? false;

    public bool Dazzling => this.data?[1][Masks[26]] ?? false;

    public bool MoreDazzling => this.data?[1][Masks[27]] ?? false;

    public bool MostDazzling => this.data?[1][Masks[28]] ?? false;

    public bool FinishedTogether => this.data?[1][Masks[29]] ?? false;

    public bool WasDream => this.data?[1][Masks[30]] ?? false; // FIXME; Not used

    public bool IsRockyBarrage => this.data?[1][Masks[31]] ?? false;

    public bool IsStickDestroyingBarrage => this.data?[2][Masks[0]] ?? false;

    public bool Fluffy => this.data?[2][Masks[1]] ?? false;

    public bool IsDoggiePhoto => this.data?[2][Masks[2]] ?? false;

    public bool IsAnimalPhoto => this.data?[2][Masks[3]] ?? false;

    public bool IsZoo => this.data?[2][Masks[4]] ?? false;

    public bool IsLovelyHeart => this.data?[2][Masks[5]] ?? false; // FIXME

    public bool IsThunder => this.data?[2][Masks[6]] ?? false;

    public bool IsDrum => this.data?[2][Masks[7]] ?? false;

    public bool IsMisty => this.data?[2][Masks[8]] ?? false; // FIXME

    public bool IsBoringPhoto => this.data?[2][Masks[9]] ?? false;

    public bool WasScolded => this.data?[2][Masks[10]] ?? false; // FIXME

    public bool IsSumireko => this.data?[2][Masks[11]] ?? false;
}
