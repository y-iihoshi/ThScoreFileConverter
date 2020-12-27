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
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th165
{
    // %T165SHOTEX[xx][y][z]
    internal class ShotExReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T165SHOTEX({0})([1-7])([1-9])", Parsers.DayParser.Pattern);

        private static readonly Func<IBestShotHeader, IReadOnlyList<Hashtag>> HashtagList =
            header => new List<Hashtag>
            {
                new Hashtag(header.Fields.IsSelfie, "＃自撮り！"),
                new Hashtag(header.Fields.IsTwoShot, "＃ツーショット！"),
                new Hashtag(header.Fields.IsThreeShot, "＃スリーショット！"),
                new Hashtag(header.Fields.TwoEnemiesTogether, "＃二人まとめて撮影した！"),
                new Hashtag(header.Fields.EnemyIsPartlyInFrame, "＃敵が見切れてる"),
                new Hashtag(header.Fields.WholeEnemyIsInFrame, "＃敵を収めたよ"),
                new Hashtag(header.Fields.EnemyIsInMiddle, "＃敵がど真ん中"),
                new Hashtag(header.Fields.PeaceSignAlongside, "＃並んでピース"),
                new Hashtag(header.Fields.EnemiesAreTooClose, "＃二人が近すぎるｗ"),
                new Hashtag(header.Fields.EnemiesAreOverlapping, "＃二人が重なってるｗｗ"),
                new Hashtag(header.Fields.Closeup, "＃接写！"),
                new Hashtag(header.Fields.QuiteCloseup, "＃かなりの接写！"),
                new Hashtag(header.Fields.TooClose, "＃近すぎてぶつかるー！"),
                new Hashtag(header.Fields.TooManyBullets, "＃弾多すぎｗ"),
                new Hashtag(header.Fields.TooPlayfulBarrage, "＃弾幕ふざけすぎｗｗ"),
                new Hashtag(header.Fields.TooDense, "＃ちょっ、密度濃すぎｗｗｗ"),
                new Hashtag(header.Fields.BitDangerous, "＃ちょっと危なかった"),
                new Hashtag(header.Fields.SeriouslyDangerous, "＃マジで危なかった"),
                new Hashtag(header.Fields.ThoughtGonnaDie, "＃死ぬかと思った"),
                new Hashtag(header.Fields.EnemyIsInFullView, "＃敵が丸見えｗ"),
                new Hashtag(header.Fields.ManyReds, "＃赤色多いな"),
                new Hashtag(header.Fields.ManyPurples, "＃紫色多いね"),
                new Hashtag(header.Fields.ManyBlues, "＃青色多いよ"),
                new Hashtag(header.Fields.ManyCyans, "＃水色多いし"),
                new Hashtag(header.Fields.ManyGreens, "＃緑色多いねぇ"),
                new Hashtag(header.Fields.ManyYellows, "＃黄色多いなぁ"),
                new Hashtag(header.Fields.ManyOranges, "＃橙色多いお"),
                new Hashtag(header.Fields.TooColorful, "＃カラフル過ぎｗ"),
                new Hashtag(header.Fields.SevenColors, "＃七色全部揃った！"),
                new Hashtag(header.Fields.Dazzling, "＃うおっ、まぶし！"),
                new Hashtag(header.Fields.MoreDazzling, "＃ぐあ、眩しすぎるー！"),
                new Hashtag(header.Fields.MostDazzling, "＃うあー、目が、目がー！"),
                new Hashtag(header.Fields.EnemyIsUndamaged, "＃敵は無傷だ"),
                new Hashtag(header.Fields.EnemyCanAfford, "＃敵はまだ余裕がある"),
                new Hashtag(header.Fields.EnemyIsWeakened, "＃敵がだいぶ弱ってる"),
                new Hashtag(header.Fields.EnemyIsDying, "＃敵が瀕死だ"),
                new Hashtag(header.Fields.Finished, "＃トドメをさしたよ！"),
                new Hashtag(header.Fields.FinishedTogether, "＃二人まとめてトドメ！"),
                new Hashtag(header.Fields.Chased, "＃追い打ちしたよ！"),
                new Hashtag(header.Fields.IsSuppository, "＃座薬ｗｗｗ"),
                new Hashtag(header.Fields.IsButterflyLikeMoth, "＃蛾みたいな蝶だ！"),
                new Hashtag(header.Fields.Scorching, "＃アチチ、焦げちゃうよ"),
                new Hashtag(header.Fields.TooBigBullet, "＃弾、大きすぎでしょｗ"),
                new Hashtag(header.Fields.ThrowingEdgedTools, "＃刃物投げんな (و｀ω´)6"),
                new Hashtag(header.Fields.IsThunder, "＃ぎゃー、雷はスマホがー"),
                new Hashtag(header.Fields.Snaky, "＃うねうねだー！"),
                new Hashtag(header.Fields.LightLooksStopped, "＃光が止まって見える！"),
                new Hashtag(header.Fields.IsSuperMoon, "＃スーパームーン！"),
                new Hashtag(header.Fields.IsRockyBarrage, "＃岩の弾幕とかｗｗ"),
                new Hashtag(header.Fields.IsStickDestroyingBarrage, "＃弾幕を破壊する棒……？"),
                new Hashtag(header.Fields.IsLovelyHeart, "＃ラブリーハート！"),
                new Hashtag(header.Fields.IsDrum, "＃ドンドコドンドコ"),
                new Hashtag(header.Fields.Fluffy, "＃もふもふもふー"),
                new Hashtag(header.Fields.IsDoggiePhoto, "＃わんわん写真"),
                new Hashtag(header.Fields.IsAnimalPhoto, "＃アニマルフォト"),
                new Hashtag(header.Fields.IsZoo, "＃動物園！"),
                new Hashtag(header.Fields.IsMisty, "＃身体が霧状に！？"),
                new Hashtag(header.Fields.WasScolded, "＃怒られちゃった……"),
                new Hashtag(header.Fields.IsLandscapePhoto, "＃風景写真"),
                new Hashtag(header.Fields.IsBoringPhoto, "＃何ともつまらない写真"),
                new Hashtag(header.Fields.IsSumireko, "＃私こそが宇佐見菫子だ！"),
            };

        private readonly MatchEvaluator evaluator;

        public ShotExReplacer(
            IReadOnlyDictionary<(Day, int), (string Path, IBestShotHeader Header)> bestshots, string outputFilePath)
        {
            if (bestshots is null)
                throw new ArgumentNullException(nameof(bestshots));

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
                    switch (type)
                    {
                        case 1:     // relative path to the bestshot file
                            if (Uri.TryCreate(outputFilePath, UriKind.Absolute, out var outputFileUri) &&
                                Uri.TryCreate(bestshot.Path, UriKind.Absolute, out var bestshotUri))
                                return outputFileUri.MakeRelativeUri(bestshotUri).OriginalString;
                            else
                                return string.Empty;
                        case 2:     // width
                            return bestshot.Header.Width.ToString(CultureInfo.InvariantCulture);
                        case 3:     // height
                            return bestshot.Header.Height.ToString(CultureInfo.InvariantCulture);
                        case 4:     // date & time
                            return new DateTime(1970, 1, 1)
                                .AddSeconds(bestshot.Header.DateTime).ToLocalTime()
                                .ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture);
                        case 5:     // hashtags
                            var hashtags = HashtagList(bestshot.Header)
                                .Where(hashtag => hashtag.Outputs)
                                .Select(hashtag => hashtag.Name);
                            return string.Join(Environment.NewLine, hashtags.ToArray());
                        case 6:     // number of views
                            return Utils.ToNumberString(bestshot.Header.NumViewed);
                        case 7:     // number of likes
                            return Utils.ToNumberString(bestshot.Header.NumLikes);
                        case 8:     // number of favs
                            return Utils.ToNumberString(bestshot.Header.NumFavs);
                        case 9:     // score
                            return Utils.ToNumberString(bestshot.Header.Score);
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
                        4 => "----/--/-- --:--:--",
                        5 => string.Empty,
                        6 => "0",
                        7 => "0",
                        8 => "0",
                        9 => "0",
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
}
