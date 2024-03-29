﻿//-----------------------------------------------------------------------
// <copyright file="AchievementReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th17;

// %T17ACHV[xx]
internal sealed class AchievementReplacer(IAchievementHolder holder)
    : AchievementReplacerBase(Definitions.FormatPrefix, Definitions.Achievements, holder)
{
}
