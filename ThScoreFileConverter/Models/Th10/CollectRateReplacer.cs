﻿//-----------------------------------------------------------------------
// <copyright file="CollectRateReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th10
{
    // %T10CRG[w][xx][y][z]
    internal class CollectRateReplacer : CollectRateReplacerBase<CharaWithTotal>
    {
        public CollectRateReplacer(
            IReadOnlyDictionary<CharaWithTotal, IClearData<CharaWithTotal>> clearDataDictionary,
            INumberFormatter formatter)
            : base(
                  Definitions.FormatPrefix,
                  Parsers.LevelWithTotalParser,
                  Parsers.CharaWithTotalParser,
                  Parsers.StageWithTotalParser,
                  Definitions.CardTable,
                  clearDataDictionary,
                  formatter)
        {
        }
    }
}
