//-----------------------------------------------------------------------
// <copyright file="ShotExReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models.Th125;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th125;

// %T125SHOTEX[w][x][y][z]
internal class ShotExReplacer : IStringReplaceable
{
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}SHOTEX({Parsers.CharaParser.Pattern})({Parsers.LevelParser.Pattern})([1-9])([1-7])");

    private readonly MatchEvaluator evaluator;

    public ShotExReplacer(
        IReadOnlyDictionary<(Chara Chara, Level Level, int Scene), (string Path, IBestShotHeader Header)> bestshots,
        IReadOnlyList<IScore> scores,
        INumberFormatter formatter,
        string outputFilePath)
    {
        this.evaluator = new MatchEvaluator(match =>
        {
            var chara = Parsers.CharaParser.Parse(match.Groups[1].Value);
            var level = Parsers.LevelParser.Parse(match.Groups[2].Value);
            var scene = IntegerHelper.Parse(match.Groups[3].Value);
            var type = IntegerHelper.Parse(match.Groups[4].Value);

            var key = (level, scene);
            if (!Definitions.SpellCards.ContainsKey(key))
                return match.ToString();

            if (bestshots.TryGetValue((chara, level, scene), out var bestshot))
            {
                IEnumerable<string> detailStrings;
                switch (type)
                {
                    case 1:     // relative path to the bestshot file
                        if (Uri.TryCreate(outputFilePath, UriKind.Absolute, out var outputFileUri) &&
                            Uri.TryCreate(bestshot.Path, UriKind.Absolute, out var bestshotUri))
                        {
                            return outputFileUri.MakeRelativeUri(bestshotUri).OriginalString;
                        }
                        else
                        {
                            return string.Empty;
                        }

                    case 2:     // width
                        return bestshot.Header.Width.ToString(CultureInfo.InvariantCulture);
                    case 3:     // height
                        return bestshot.Header.Height.ToString(CultureInfo.InvariantCulture);
                    case 4:     // score
                        return formatter.FormatNumber(bestshot.Header.ResultScore);
                    case 5:     // slow rate
                        return formatter.FormatPercent(bestshot.Header.SlowRate, 6);
                    case 6:     // date & time
                        return DateTimeHelper.GetString(
                            scores.FirstOrDefault(s => (s?.Chara == chara) && s.LevelScene.Equals(key))?.DateTime);
                    case 7:     // detail info
                        detailStrings = MakeDetailList(bestshot.Header, formatter)
                            .Where(detail => detail.Outputs)
                            .Select(detail => Utils.Format(detail.Format, detail.Value));
                        return string.Join(Environment.NewLine, detailStrings.ToArray());
                    default:    // unreachable
                        return match.ToString();
                }
            }
            else
            {
                return type switch
                {
                    1 => string.Empty,
                    2 => "0",
                    3 => "0",
                    4 => "--------",
                    5 => "-----%",
                    6 => DateTimeHelper.GetString(null),
                    7 => string.Empty,
                    _ => match.ToString(),
                };
            }
        });
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }

    private static IEnumerable<Detail> MakeDetailList(IBestShotHeader header, INumberFormatter formatter)
    {
        Func<int, string> str = formatter.FormatNumber;

        return new Detail[]
        {
            new(true,                       "Base Point    {0,9}", str(header.BasePoint)),
            new(header.Fields.ClearShot,    "Clear Shot!   {0,9}", StringHelper.Create($"+ {header.ClearShot}")),
            new(header.Fields.SoloShot,     "Solo Shot     {0,9}", "+ 100"),
            new(header.Fields.RedShot,      "Red Shot      {0,9}", "+ 300"),
            new(header.Fields.PurpleShot,   "Purple Shot   {0,9}", "+ 300"),
            new(header.Fields.BlueShot,     "Blue Shot     {0,9}", "+ 300"),
            new(header.Fields.CyanShot,     "Cyan Shot     {0,9}", "+ 300"),
            new(header.Fields.GreenShot,    "Green Shot    {0,9}", "+ 300"),
            new(header.Fields.YellowShot,   "Yellow Shot   {0,9}", "+ 300"),
            new(header.Fields.OrangeShot,   "Orange Shot   {0,9}", "+ 300"),
            new(header.Fields.ColorfulShot, "Colorful Shot {0,9}", "+ 900"),
            new(header.Fields.RainbowShot,  "Rainbow Shot  {0,9}", StringHelper.Create($"+ {str(2100)}")),
            new(header.Fields.RiskBonus,    "Risk Bonus    {0,9}", StringHelper.Create($"+ {str(header.RiskBonus)}")),
            new(header.Fields.MacroBonus,   "Macro Bonus   {0,9}", StringHelper.Create($"+ {str(header.MacroBonus)}")),
            new(header.Fields.FrontShot,    "Front Shot    {0,9}", StringHelper.Create($"+ {header.FrontSideBackShot}")),
            new(header.Fields.SideShot,     "Side Shot     {0,9}", StringHelper.Create($"+ {header.FrontSideBackShot}")),
            new(header.Fields.BackShot,     "Back Shot     {0,9}", StringHelper.Create($"+ {header.FrontSideBackShot}")),
            new(header.Fields.CatBonus,     "Cat Bonus     {0,9}", "+ 666"),
            new(true,                       string.Empty,          string.Empty),
            new(true,                       "Boss Shot!    {0,9}", StringHelper.Create($"* {header.BossShot:F2}")),
            new(header.Fields.TwoShot,      "Two Shot!     {0,9}", "* 1.50"),
            new(header.Fields.NiceShot,     "Nice Shot!    {0,9}", StringHelper.Create($"* {header.NiceShot:F2}")),
            new(true,                       "Angle Bonus   {0,9}", StringHelper.Create($"* {header.AngleBonus:F2}")),
            new(true,                       string.Empty,          string.Empty),
            new(true,                       "Result Score  {0,9}", str(header.ResultScore)),
        };
    }

    private class Detail
    {
        public Detail(bool outputs, string format, string value)
        {
            this.Outputs = outputs;
            this.Format = format;
            this.Value = value;
        }

        public bool Outputs { get; }

        public string Format { get; }

        public string Value { get; }
    }
}
