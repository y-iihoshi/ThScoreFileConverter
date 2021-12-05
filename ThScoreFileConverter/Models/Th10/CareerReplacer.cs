//-----------------------------------------------------------------------
// <copyright file="CareerReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th10
{
    // %T10C[xxx][yy][z]
    internal class CareerReplacer : CareerReplacerBase<CharaWithTotal, Stage, Level>
    {
        public CareerReplacer(
            IReadOnlyDictionary<CharaWithTotal, IClearData<CharaWithTotal>> clearDataDictionary,
            INumberFormatter formatter)
            : base(
                  Definitions.FormatPrefix,
                  Parsers.CharaWithTotalParser,
                  clearDataDictionary,
                  Definitions.CardTable,
                  formatter)
        {
        }
    }
}
