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
using ThScoreFileConverter.Core.Models.Th165;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th165;

// %T165SHOTEX[xx][y][z]
internal class ShotExReplacer : IStringReplaceable
{
    private static readonly string Pattern = StringHelper.Create(
        $"{Definitions.FormatPrefix}SHOTEX({Parsers.DayParser.Pattern})([1-7])([1-9])");

    private static readonly Func<IBestShotHeader, IReadOnlyList<Hashtag>> HashtagList =
        header => new List<Hashtag>
        {
            new(header.Fields.IsSelfie, "＃自撮り！"),
            new(header.Fields.IsTwoShot, "＃ツーショット！"),
            new(header.Fields.IsThreeShot, "＃スリーショット！"),
            new(header.Fields.TwoEnemiesTogether, "＃二人まとめて撮影した！"),
            new(header.Fields.EnemyIsPartlyInFrame, "＃敵が見切れてる"),
            new(header.Fields.WholeEnemyIsInFrame, "＃敵を収めたよ"),
            new(header.Fields.EnemyIsInMiddle, "＃敵がど真ん中"),
            new(header.Fields.PeaceSignAlongside, "＃並んでピース"),
            new(header.Fields.EnemiesAreTooClose, "＃二人が近すぎるｗ"),
            new(header.Fields.EnemiesAreOverlapping, "＃二人が重なってるｗｗ"),
            new(header.Fields.Closeup, "＃接写！"),
            new(header.Fields.QuiteCloseup, "＃かなりの接写！"),
            new(header.Fields.TooClose, "＃近すぎてぶつかるー！"),
            new(header.Fields.TooManyBullets, "＃弾多すぎｗ"),
            new(header.Fields.TooPlayfulBarrage, "＃弾幕ふざけすぎｗｗ"),
            new(header.Fields.TooDense, "＃ちょっ、密度濃すぎｗｗｗ"),
            new(header.Fields.BitDangerous, "＃ちょっと危なかった"),
            new(header.Fields.SeriouslyDangerous, "＃マジで危なかった"),
            new(header.Fields.ThoughtGonnaDie, "＃死ぬかと思った"),
            new(header.Fields.EnemyIsInFullView, "＃敵が丸見えｗ"),
            new(header.Fields.ManyReds, "＃赤色多いな"),
            new(header.Fields.ManyPurples, "＃紫色多いね"),
            new(header.Fields.ManyBlues, "＃青色多いよ"),
            new(header.Fields.ManyCyans, "＃水色多いし"),
            new(header.Fields.ManyGreens, "＃緑色多いねぇ"),
            new(header.Fields.ManyYellows, "＃黄色多いなぁ"),
            new(header.Fields.ManyOranges, "＃橙色多いお"),
            new(header.Fields.TooColorful, "＃カラフル過ぎｗ"),
            new(header.Fields.SevenColors, "＃七色全部揃った！"),
            new(header.Fields.Dazzling, "＃うおっ、まぶし！"),
            new(header.Fields.MoreDazzling, "＃ぐあ、眩しすぎるー！"),
            new(header.Fields.MostDazzling, "＃うあー、目が、目がー！"),
            new(header.Fields.EnemyIsUndamaged, "＃敵は無傷だ"),
            new(header.Fields.EnemyCanAfford, "＃敵はまだ余裕がある"),
            new(header.Fields.EnemyIsWeakened, "＃敵がだいぶ弱ってる"),
            new(header.Fields.EnemyIsDying, "＃敵が瀕死だ"),
            new(header.Fields.Finished, "＃トドメをさしたよ！"),
            new(header.Fields.FinishedTogether, "＃二人まとめてトドメ！"),
            new(header.Fields.Chased, "＃追い打ちしたよ！"),
            new(header.Fields.IsSuppository, "＃座薬ｗｗｗ"),
            new(header.Fields.IsButterflyLikeMoth, "＃蛾みたいな蝶だ！"),
            new(header.Fields.Scorching, "＃アチチ、焦げちゃうよ"),
            new(header.Fields.TooBigBullet, "＃弾、大きすぎでしょｗ"),
            new(header.Fields.ThrowingEdgedTools, "＃刃物投げんな (و｀ω´)6"),
            new(header.Fields.IsThunder, "＃ぎゃー、雷はスマホがー"),
            new(header.Fields.Snaky, "＃うねうねだー！"),
            new(header.Fields.LightLooksStopped, "＃光が止まって見える！"),
            new(header.Fields.IsSuperMoon, "＃スーパームーン！"),
            new(header.Fields.IsRockyBarrage, "＃岩の弾幕とかｗｗ"),
            new(header.Fields.IsStickDestroyingBarrage, "＃弾幕を破壊する棒……？"),
            new(header.Fields.IsLovelyHeart, "＃ラブリーハート！"),
            new(header.Fields.IsDrum, "＃ドンドコドンドコ"),
            new(header.Fields.Fluffy, "＃もふもふもふー"),
            new(header.Fields.IsDoggiePhoto, "＃わんわん写真"),
            new(header.Fields.IsAnimalPhoto, "＃アニマルフォト"),
            new(header.Fields.IsZoo, "＃動物園！"),
            new(header.Fields.IsMisty, "＃身体が霧状に！？"),
            new(header.Fields.WasScolded, "＃怒られちゃった……"),
            new(header.Fields.IsLandscapePhoto, "＃風景写真"),
            new(header.Fields.IsBoringPhoto, "＃何ともつまらない写真"),
            new(header.Fields.IsSumireko, "＃私こそが宇佐見菫子だ！"),
        };

    private readonly MatchEvaluator evaluator;

    public ShotExReplacer(
        IReadOnlyDictionary<(Day Day, int Scene), (string Path, IBestShotHeader Header)> bestshots,
        INumberFormatter formatter,
        string outputFilePath)
    {
        this.evaluator = new MatchEvaluator(match =>
        {
            var day = Parsers.DayParser.Parse(match.Groups[1].Value);
            var scene = IntegerHelper.Parse(match.Groups[2].Value);
            var type = IntegerHelper.Parse(match.Groups[3].Value);

            var key = (day, scene);
            if (!Definitions.SpellCards.ContainsKey(key))
                return match.ToString();

            if (bestshots.TryGetValue(key, out var bestshot))
            {
                return type switch
                {
                    1 => UriHelper.GetRelativePath(outputFilePath, bestshot.Path),
                    2 => bestshot.Header.Width.ToString(CultureInfo.InvariantCulture),
                    3 => bestshot.Header.Height.ToString(CultureInfo.InvariantCulture),
                    4 => DateTimeHelper.GetString(bestshot.Header.DateTime),
                    5 => string.Join(
                        Environment.NewLine,
                        HashtagList(bestshot.Header).Where(hashtag => hashtag.Outputs).Select(hashtag => hashtag.Name)),
                    6 => formatter.FormatNumber(bestshot.Header.NumViewed),
                    7 => formatter.FormatNumber(bestshot.Header.NumLikes),
                    8 => formatter.FormatNumber(bestshot.Header.NumFavs),
                    9 => formatter.FormatNumber(bestshot.Header.Score),
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
                    4 => DateTimeHelper.GetString(null),
                    5 => string.Empty,
                    6 => formatter.FormatNumber(0),
                    7 => formatter.FormatNumber(0),
                    8 => formatter.FormatNumber(0),
                    9 => formatter.FormatNumber(0),
                    _ => match.ToString(),
                };
            }
        });
    }

    public string Replace(string input)
    {
        return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }

    private class Hashtag
    {
        public Hashtag(bool outputs, string name)
        {
            this.Outputs = outputs;
            this.Name = name;
        }

        public bool Outputs { get; }

        public string Name { get; }
    }
}
