//-----------------------------------------------------------------------
// <copyright file="ShotExReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Globalization;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Core.Models.Th125;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th125;

// %T125SHOTEX[w][x][y][z]
internal sealed class ShotExReplacer(
    IReadOnlyDictionary<(Chara Chara, Level Level, int Scene), (string Path, IBestShotHeader Header)> bestshots,
    IReadOnlyList<IScore> scores,
    INumberFormatter formatter,
    string outputFilePath)
    : IStringReplaceable
{
    private static readonly Core.Models.IntegerParser TypeParser = new(@"[1-7]");
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}SHOTEX({Parsers.CharaParser.Pattern})({Parsers.LevelParser.Pattern})({Parsers.SceneParser.Pattern})({TypeParser.Pattern})");

    private readonly MatchEvaluator evaluator = new(match =>
    {
        var chara = Parsers.CharaParser.Parse(match.Groups[1]);
        var level = Parsers.LevelParser.Parse(match.Groups[2]);
        var scene = Parsers.SceneParser.Parse(match.Groups[3]);
        var type = TypeParser.Parse(match.Groups[4]);

        var key = (level, scene);
        if (!Definitions.SpellCards.ContainsKey(key))
            return match.ToString();

        if (bestshots.TryGetValue((chara, level, scene), out var bestshot))
        {
            return type switch
            {
                1 => UriHelper.GetRelativePath(outputFilePath, bestshot.Path),
                2 => bestshot.Header.Width.ToString(CultureInfo.InvariantCulture),
                3 => bestshot.Header.Height.ToString(CultureInfo.InvariantCulture),
                4 => formatter.FormatNumber(bestshot.Header.ResultScore),
                5 => formatter.FormatPercent(bestshot.Header.SlowRate, 6),
                6 => DateTimeHelper.GetString(scores.FirstOrDefault(s => (s?.Chara == chara) && s.LevelScene.Equals(key))?.DateTime),
                7 => string.Join(Environment.NewLine, MakeDetailList(bestshot.Header, formatter)
                    .Where(detail => detail.Outputs)
                    .Select(detail => StringHelper.Format(detail.Format, detail.Value))),
                _ => match.ToString(),
            };
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

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }

    private static Detail[] MakeDetailList(IBestShotHeader header, INumberFormatter formatter)
    {
        Func<int, string> str = formatter.FormatNumber;

        return
        [
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
        ];
    }

    private sealed class Detail(bool outputs, string format, string value)
    {
        public bool Outputs { get; } = outputs;

        public string Format { get; } = format;

        public string Value { get; } = value;
    }
}
