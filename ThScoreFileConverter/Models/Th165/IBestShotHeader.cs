//-----------------------------------------------------------------------
// <copyright file="IBestShotHeader.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models.Th165;

namespace ThScoreFileConverter.Models.Th165;

internal interface IBestShotHeader : Models.IBestShotHeader
{
    float Angle { get; }

    float AngleBonus { get; }

    int BasePoint { get; }

    float BossShot { get; }

    uint DateTime { get; }

    short Dream { get; }

    float FavsPerView { get; }

    HashtagFields Fields { get; }

    short HalfHeight { get; }

    short HalfWidth { get; }

    short Height2 { get; }

    float LikesPerView { get; }

    int MacroBonus { get; }

    int NumBlueBullets { get; }

    int NumBullets { get; }

    int NumBulletsNearby { get; }

    int NumCyanBullets { get; }

    int NumFavs { get; }

    int NumGreenBullets { get; }

    int NumHashtags { get; }

    int NumLightBullets { get; }

    int NumLikes { get; }

    int NumOrangeBullets { get; }

    int NumPurpleBullets { get; }

    int NumRedBullets { get; }

    int NumViewed { get; }

    int NumYellowBullets { get; }

    int RiskBonus { get; }

    int Score { get; }

    int Score2 { get; }

    Day Weekday { get; }

    short Width2 { get; }
}
