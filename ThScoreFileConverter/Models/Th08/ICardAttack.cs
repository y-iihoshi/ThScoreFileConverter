//-----------------------------------------------------------------------
// <copyright file="ICardAttack.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models.Th08;

namespace ThScoreFileConverter.Models.Th08;

internal interface ICardAttack : Th06.IChapter
{
    short CardId { get; }

    IEnumerable<byte> CardName { get; }

    IEnumerable<byte> Comment { get; }

    IEnumerable<byte> EnemyName { get; }

    bool HasTried { get; }

    LevelPracticeWithTotal Level { get; }

    ICardAttackCareer PracticeCareer { get; }

    ICardAttackCareer StoryCareer { get; }
}
