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

namespace ThScoreFileConverter.Models.Th125
{
    // %T125SHOTEX[w][x][y][z]
    internal class ShotExReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T125SHOTEX({0})({1})([1-9])([1-7])", Parsers.CharaParser.Pattern, Parsers.LevelParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public ShotExReplacer(
            IReadOnlyDictionary<(Chara, Level, int), (string Path, IBestShotHeader Header)> bestshots,
            IReadOnlyList<IScore> scores,
            string outputFilePath)
        {
            if (bestshots is null)
                throw new ArgumentNullException(nameof(bestshots));
            if (scores is null)
                throw new ArgumentNullException(nameof(scores));

            this.evaluator = new MatchEvaluator(match =>
            {
                var chara = Parsers.CharaParser.Parse(match.Groups[1].Value);
                var level = Parsers.LevelParser.Parse(match.Groups[2].Value);
                var scene = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                var key = (level, scene);
                if (!Definitions.SpellCards.ContainsKey(key))
                    return match.ToString();

                if (bestshots.TryGetValue((chara, level, scene), out var bestshot))
                {
                    IScore score;
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
                            return Utils.ToNumberString(bestshot.Header.ResultScore);
                        case 5:     // slow rate
                            return Utils.Format("{0:F6}%", bestshot.Header.SlowRate);
                        case 6:     // date & time
                            score = scores.FirstOrDefault(elem =>
                                (elem != null) && (elem.Chara == chara) && elem.LevelScene.Equals(key));
                            if (score == null)
                                return "----/--/-- --:--:--";
                            return new DateTime(1970, 1, 1)
                                .AddSeconds(score.DateTime).ToLocalTime()
                                .ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture);
                        case 7:     // detail info
                            detailStrings = MakeDetailList(bestshot.Header)
                                .Where(detail => detail.Outputs)
                                .Select(detail => Utils.Format(detail.Format, detail.Value));
                            return string.Join(Environment.NewLine, detailStrings.ToArray());
                        default:    // unreachable
                            return match.ToString();
                    }
                }
                else
                {
                    switch (type)
                    {
                        case 1: return string.Empty;
                        case 2: return "0";
                        case 3: return "0";
                        case 4: return "--------";
                        case 5: return "-----%";
                        case 6: return "----/--/-- --:--:--";
                        case 7: return string.Empty;
                        default: return match.ToString();
                    }
                }
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);

        private static IEnumerable<Detail> MakeDetailList(IBestShotHeader header)
        {
            string Str(int value) => Utils.ToNumberString(value);
            string Fmt(string format, object value) => Utils.Format(format, value);

            return new[]
            {
                new Detail(true,                       "Base Point    {0,9}", Str(header.BasePoint)),
                new Detail(header.Fields.ClearShot,    "Clear Shot!   {0,9}", Fmt("+ {0}", header.ClearShot)),
                new Detail(header.Fields.SoloShot,     "Solo Shot     {0,9}", "+ 100"),
                new Detail(header.Fields.RedShot,      "Red Shot      {0,9}", "+ 300"),
                new Detail(header.Fields.PurpleShot,   "Purple Shot   {0,9}", "+ 300"),
                new Detail(header.Fields.BlueShot,     "Blue Shot     {0,9}", "+ 300"),
                new Detail(header.Fields.CyanShot,     "Cyan Shot     {0,9}", "+ 300"),
                new Detail(header.Fields.GreenShot,    "Green Shot    {0,9}", "+ 300"),
                new Detail(header.Fields.YellowShot,   "Yellow Shot   {0,9}", "+ 300"),
                new Detail(header.Fields.OrangeShot,   "Orange Shot   {0,9}", "+ 300"),
                new Detail(header.Fields.ColorfulShot, "Colorful Shot {0,9}", "+ 900"),
                new Detail(header.Fields.RainbowShot,  "Rainbow Shot  {0,9}", Fmt("+ {0}", Str(2100))),
                new Detail(header.Fields.RiskBonus,    "Risk Bonus    {0,9}", Fmt("+ {0}", Str(header.RiskBonus))),
                new Detail(header.Fields.MacroBonus,   "Macro Bonus   {0,9}", Fmt("+ {0}", Str(header.MacroBonus))),
                new Detail(header.Fields.FrontShot,    "Front Shot    {0,9}", Fmt("+ {0}", header.FrontSideBackShot)),
                new Detail(header.Fields.SideShot,     "Side Shot     {0,9}", Fmt("+ {0}", header.FrontSideBackShot)),
                new Detail(header.Fields.BackShot,     "Back Shot     {0,9}", Fmt("+ {0}", header.FrontSideBackShot)),
                new Detail(header.Fields.CatBonus,     "Cat Bonus     {0,9}", "+ 666"),
                new Detail(true,                       string.Empty,          string.Empty),
                new Detail(true,                       "Boss Shot!    {0,9}", Fmt("* {0:F2}", header.BossShot)),
                new Detail(header.Fields.TwoShot,      "Two Shot!     {0,9}", "* 1.50"),
                new Detail(header.Fields.NiceShot,     "Nice Shot!    {0,9}", Fmt("* {0:F2}", header.NiceShot)),
                new Detail(true,                       "Angle Bonus   {0,9}", Fmt("* {0:F2}", header.AngleBonus)),
                new Detail(true,                       string.Empty,          string.Empty),
                new Detail(true,                       "Result Score  {0,9}", Str(header.ResultScore)),
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
}
