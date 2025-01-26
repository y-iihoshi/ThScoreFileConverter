//-----------------------------------------------------------------------
// <copyright file="ICardAttack.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models.Th07;

namespace ThScoreFileConverter.Models.Th07;

internal interface ICardAttack : Th06.IChapter
{
    short CardId { get; }

    IEnumerable<byte> CardName { get; }

    IReadOnlyDictionary<CharaWithTotal, ushort> ClearCounts { get; }

    bool HasTried { get; }

    IReadOnlyDictionary<CharaWithTotal, uint> MaxBonuses { get; }

    IReadOnlyDictionary<CharaWithTotal, ushort> TrialCounts { get; }
}
