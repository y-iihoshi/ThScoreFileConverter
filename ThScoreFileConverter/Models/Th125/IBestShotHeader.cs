//-----------------------------------------------------------------------
// <copyright file="IBestShotHeader.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th125
{
    internal interface IBestShotHeader : Th095.IBestShotHeader<Level>
    {
        float Angle { get; }

        float AngleBonus { get; }

        int BasePoint { get; }

        float BossShot { get; }

        int ClearShot { get; }

        uint DateTime { get; }

        BonusFields Fields { get; }

        int FrontSideBackShot { get; }

        short HalfHeight { get; }

        short HalfWidth { get; }

        short Height2 { get; }

        int MacroBonus { get; }

        float NiceShot { get; }

        int ResultScore2 { get; }

        int RiskBonus { get; }

        short Width2 { get; }
    }
}
