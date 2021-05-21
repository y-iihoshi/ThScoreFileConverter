//-----------------------------------------------------------------------
// <copyright file="CardReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Linq;

namespace ThScoreFileConverter.Models.Th095
{
    // %T95CARD[x][y][z]
    internal class CardReplacer : CardReplacerBase<Level, Enemy>
    {
        public CardReplacer(IReadOnlyList<IScore> scores, bool hideUntriedCards)
            : base(
                  Definitions.FormatPrefix,
                  Parsers.LevelParser,
                  Definitions.SpellCards,
                  hideUntriedCards,
                  (level, scene) => HasTried(scores, level, scene))
        {
        }

        private static bool HasTried(IReadOnlyList<IScore> scores, Level level, int scene)
        {
            return scores.FirstOrDefault(
                elem => (elem is not null) && elem.LevelScene.Equals((level, scene))) is not null;
        }
    }
}
