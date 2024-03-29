﻿//-----------------------------------------------------------------------
// <copyright file="AchievementReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th18;

// %T18ACHV[xx]
internal sealed class AchievementReplacer(Th17.IAchievementHolder holder)
    : Th17.AchievementReplacerBase(Definitions.FormatPrefix, Definitions.Achievements, holder)
{
}
