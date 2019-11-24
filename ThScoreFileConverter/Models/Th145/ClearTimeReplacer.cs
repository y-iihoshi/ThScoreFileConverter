//-----------------------------------------------------------------------
// <copyright file="ClearTimeReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ThScoreFileConverter.Models.Th145
{
    // %T145TIMECLR[x][yy]
    internal class ClearTimeReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T145TIMECLR({0})({1})", Parsers.LevelWithTotalParser.Pattern, Parsers.CharaWithTotalParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public ClearTimeReplacer(IReadOnlyDictionary<Level, IReadOnlyDictionary<Chara, int>> clearTimes)
        {
            if (clearTimes is null)
                throw new ArgumentNullException(nameof(clearTimes));

            this.evaluator = new MatchEvaluator(match =>
            {
                var level = Parsers.LevelWithTotalParser.Parse(match.Groups[1].Value);
                var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[2].Value);

                Func<IReadOnlyDictionary<Chara, int>, int> getValueByChara;
                if (chara == CharaWithTotal.Total)
                    getValueByChara = dictionary => dictionary.Values.Sum();
                else
                    getValueByChara = dictionary => dictionary.TryGetValue((Chara)chara, out var time) ? time : 0;

                Func<IReadOnlyDictionary<Level, IReadOnlyDictionary<Chara, int>>, int> getValueByLevel;
                if (level == LevelWithTotal.Total)
                {
                    getValueByLevel = dictionary => dictionary.Values.Sum(getValueByChara);
                }
                else
                {
                    getValueByLevel = dictionary => dictionary.TryGetValue((Level)level, out var times)
                        ? getValueByChara(times) : 0;
                }

                return new Time(getValueByLevel(clearTimes)).ToString();
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
