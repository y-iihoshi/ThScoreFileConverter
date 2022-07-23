//-----------------------------------------------------------------------
// <copyright file="PracticeReplacerBase.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Text.RegularExpressions;

namespace ThScoreFileConverter.Models.Th10;

internal class PracticeReplacerBase<TLevel, TChara, TStage> : IStringReplaceable
    where TLevel : struct, Enum
    where TChara : struct, Enum
    where TStage : struct, Enum
{
    private readonly string pattern;
    private readonly MatchEvaluator evaluator;

    protected PracticeReplacerBase(
        string formatPrefix,
        EnumShortNameParser<TLevel> levelParser,
        EnumShortNameParser<TChara> charaParser,
        EnumShortNameParser<TStage> stageParser,
        Func<TLevel, bool> levelCanPractice,
        Func<TStage, bool> stageCanPractice,
        Func<TLevel, TChara, TStage, IPractice?> getPractice,
        INumberFormatter formatter)
    {
        this.pattern = Utils.Format(
            @"{0}PRAC({1})({2})({3})", formatPrefix, levelParser.Pattern, charaParser.Pattern, stageParser.Pattern);
        this.evaluator = new MatchEvaluator(match =>
        {
            var level = levelParser.Parse(match.Groups[1].Value);
            var chara = charaParser.Parse(match.Groups[2].Value);
            var stage = stageParser.Parse(match.Groups[3].Value);

            return levelCanPractice(level) && stageCanPractice(stage)
                ? formatter.FormatNumber(
                    getPractice(level, chara, stage) is { } practice ? (practice.Score * 10) : default)
                : match.ToString();
        });
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, this.pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
