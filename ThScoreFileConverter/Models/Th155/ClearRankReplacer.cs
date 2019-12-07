//-----------------------------------------------------------------------
// <copyright file="ClearRankReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ThScoreFileConverter.Models.Th155
{
    // %T155CLEAR[x][yy]
    internal class ClearRankReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T155CLEAR({0})({1})", Parsers.LevelParser.Pattern, Parsers.StoryCharaParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public ClearRankReplacer(IReadOnlyDictionary<StoryChara, AllScoreData.Story> storyDictionary)
        {
            if (storyDictionary is null)
                throw new ArgumentNullException(nameof(storyDictionary));

            this.evaluator = new MatchEvaluator(match =>
            {
                var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
                var chara = Parsers.StoryCharaParser.Parse(match.Groups[2].Value);

                static LevelFlags ToLevelFlag(Level lv)
                {
                    switch (lv)
                    {
                        case Level.Easy:
                            return LevelFlags.Easy;
                        case Level.Normal:
                            return LevelFlags.Normal;
                        case Level.Hard:
                            return LevelFlags.Hard;
                        case Level.Lunatic:
                            return LevelFlags.Lunatic;
                        case Level.OverDrive:
                            return LevelFlags.OverDrive;
                        default:
                            return LevelFlags.None;
                    }
                }

                if (storyDictionary.TryGetValue(chara, out var story)
                    && story.Available
                    && ((story.Ed & ToLevelFlag(level)) != LevelFlags.None))
                    return "Clear";
                else
                    return "Not Clear";
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
