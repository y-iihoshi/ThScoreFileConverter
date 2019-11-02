//-----------------------------------------------------------------------
// <copyright file="IClearData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th105
{
    internal interface IClearData<TChara, TLevel>
        where TChara : struct, Enum
        where TLevel : struct, Enum
    {
        IReadOnlyDictionary<int, ICardForDeck> CardsForDeck { get; }

        IReadOnlyDictionary<(TChara Chara, int CardId), ISpellCardResult<TChara, TLevel>> SpellCardResults { get; }
    }
}
