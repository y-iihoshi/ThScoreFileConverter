//-----------------------------------------------------------------------
// <copyright file="ISpellCard.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;

namespace ThScoreFileConverter.Models.Th13
{
    internal interface ISpellCard<TLevel> : Th10.ISpellCard<TLevel>
        where TLevel : struct, Enum
    {
        int PracticeClearCount { get; }

        int PracticeScore { get; }

        int PracticeTrialCount { get; }
    }
}
