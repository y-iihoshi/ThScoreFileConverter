//-----------------------------------------------------------------------
// <copyright file="IScore.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models.Th143;

namespace ThScoreFileConverter.Models.Th143;

internal interface IScore : Th095.IChapter
{
    IReadOnlyDictionary<ItemWithTotal, int> ChallengeCounts { get; }

    IReadOnlyDictionary<ItemWithTotal, int> ClearCounts { get; }

    int HighScore { get; }

    int Number { get; }
}
