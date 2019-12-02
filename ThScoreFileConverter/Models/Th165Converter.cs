//-----------------------------------------------------------------------
// <copyright file="Th165Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591
#pragma warning disable SA1600 // ElementsMustBeDocumented

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th165;

namespace ThScoreFileConverter.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th165Converter : ThConverter
    {
        private AllScoreData allScoreData = null;

        private Dictionary<(Day Day, int Scene), (string Path, IBestShotHeader Header)> bestshots = null;

        public override string SupportedVersions => "1.00a";

        public override bool HasBestShotConverter => true;

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th165decoded.dat", FileMode.Create, FileAccess.ReadWrite))
#else
            using (var decoded = new MemoryStream())
#endif
            {
                if (!Decrypt(input, decrypted))
                    return false;

                decrypted.Seek(0, SeekOrigin.Begin);
                if (!Extract(decrypted, decoded))
                    return false;

                decoded.Seek(0, SeekOrigin.Begin);
                if (!Validate(decoded))
                    return false;

                decoded.Seek(0, SeekOrigin.Begin);
                this.allScoreData = Read(decoded);

                return this.allScoreData != null;
            }
        }

        protected override IEnumerable<IStringReplaceable> CreateReplacers(
            bool hideUntriedCards, string outputFilePath)
        {
            return new List<IStringReplaceable>
            {
                new ScoreReplacer(this),
                new ScoreTotalReplacer(this),
                new CardReplacer(this, hideUntriedCards),
                new NicknameReplacer(this),
                new TimeReplacer(this),
                new ShotReplacer(this, outputFilePath),
                new ShotExReplacer(this, outputFilePath),
            };
        }

        protected override string[] FilterBestShotFiles(string[] files)
        {
            var pattern = Utils.Format(@"bs({0})_\d{{2}}.dat", Parsers.DayLongPattern);

            return files.Where(file => Regex.IsMatch(
                Path.GetFileName(file), pattern, RegexOptions.IgnoreCase)).ToArray();
        }

        protected override void ConvertBestShot(Stream input, Stream output)
        {
            using (var decoded = new MemoryStream())
            {
                var outputFile = output as FileStream;

                using (var reader = new BinaryReader(input, Encoding.UTF8, true))
                {
                    var header = new BestShotHeader();
                    header.ReadFrom(reader);

                    if (this.bestshots == null)
                    {
                        this.bestshots =
                            new Dictionary<(Day, int), (string, IBestShotHeader)>(Definitions.SpellCards.Count);
                    }

                    var key = (header.Weekday, header.Dream);
                    if (!this.bestshots.ContainsKey(key))
                        this.bestshots.Add(key, (outputFile.Name, header));

                    Lzss.Extract(input, decoded);

                    decoded.Seek(0, SeekOrigin.Begin);
                    using (var bitmap = new Bitmap(header.Width, header.Height, PixelFormat.Format32bppArgb))
                    {
                        try
                        {
                            var permission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
                            permission.Demand();

                            var bitmapData = bitmap.LockBits(
                                new Rectangle(0, 0, header.Width, header.Height),
                                ImageLockMode.WriteOnly,
                                bitmap.PixelFormat);
                            var source = decoded.ToArray();
                            var destination = bitmapData.Scan0;
                            Marshal.Copy(source, 0, destination, source.Length);
                            bitmap.UnlockBits(bitmapData);
                        }
                        catch (SecurityException e)
                        {
                            Console.WriteLine(e.ToString());
                        }

                        bitmap.Save(output, ImageFormat.Png);
                        output.Flush();
                        output.SetLength(output.Position);
                    }
                }
            }
        }

        private static bool Decrypt(Stream input, Stream output)
        {
            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            using (var writer = new BinaryWriter(output, Encoding.UTF8, true))
            {
                var header = new Header();
                header.ReadFrom(reader);
                if (!header.IsValid)
                    return false;
                if (header.EncodedAllSize != reader.BaseStream.Length)
                    return false;

                header.WriteTo(writer);
                ThCrypt.Decrypt(input, output, header.EncodedBodySize, 0xAC, 0x35, 0x10, header.EncodedBodySize);

                return true;
            }
        }

        private static bool Extract(Stream input, Stream output)
        {
            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            using (var writer = new BinaryWriter(output, Encoding.UTF8, true))
            {
                var header = new Header();
                header.ReadFrom(reader);
                header.WriteTo(writer);

                var bodyBeginPos = output.Position;
                Lzss.Extract(input, output);
                output.Flush();
                output.SetLength(output.Position);

                return header.DecodedBodySize == (output.Position - bodyBeginPos);
            }
        }

        private static bool Validate(Stream input)
        {
            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            {
                var header = new Header();
                header.ReadFrom(reader);
                var remainSize = header.DecodedBodySize;
                var chapter = new Th10.Chapter();

                try
                {
                    while (remainSize > 0)
                    {
                        chapter.ReadFrom(reader);
                        if (!chapter.IsValid)
                            return false;
                        if (!Score.CanInitialize(chapter) &&
                            !Status.CanInitialize(chapter))
                            return false;

                        remainSize -= chapter.Size;
                    }
                }
                catch (EndOfStreamException)
                {
                    // It's OK, do nothing.
                }

                return remainSize == 0;
            }
        }

        private static AllScoreData Read(Stream input)
        {
            var dictionary = new Dictionary<string, Action<AllScoreData, Th10.Chapter>>
            {
                { Score.ValidSignature,  (data, ch) => data.Set(new Score(ch))  },
                { Status.ValidSignature, (data, ch) => data.Set(new Status(ch)) },
            };

            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            {
                var allScoreData = new AllScoreData();
                var chapter = new Th10.Chapter();

                var header = new Header();
                header.ReadFrom(reader);
                allScoreData.Set(header);

                try
                {
                    while (true)
                    {
                        chapter.ReadFrom(reader);
                        if (dictionary.TryGetValue(chapter.Signature, out var setChapter))
                            setChapter(allScoreData, chapter);
                    }
                }
                catch (EndOfStreamException)
                {
                    // It's OK, do nothing.
                }

                if ((allScoreData.Header != null) &&
                    //// (allScoreData.scores.Count >= 0) &&
                    (allScoreData.Status != null))
                    return allScoreData;
                else
                    return null;
            }
        }

        // %T165SCR[xx][y][z]
        private class ScoreReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T165SCR({0})([1-7])([1-4])", Parsers.DayParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ScoreReplacer(Th165Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var day = Parsers.DayParser.Parse(match.Groups[1].Value);
                    var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    var key = (day, scene);
                    if (!Definitions.SpellCards.ContainsKey(key))
                        return match.ToString();

                    var score = parent.allScoreData.Scores.FirstOrDefault(elem =>
                        (elem != null) && ((elem.Number >= 0) && (elem.Number < Definitions.SpellCards.Count)) &&
                        Definitions.SpellCards.ElementAt(elem.Number).Key.Equals(key));

                    switch (type)
                    {
                        case 1:     // high score
                            return (score != null) ? Utils.ToNumberString(score.HighScore) : "0";
                        case 2:     // challenge count
                            return (score != null) ? Utils.ToNumberString(score.ChallengeCount) : "0";
                        case 3:     // cleared count
                            return (score != null) ? Utils.ToNumberString(score.ClearCount) : "0";
                        case 4:     // num of photos
                            return (score != null) ? Utils.ToNumberString(score.NumPhotos) : "0";
                        default:    // unreachable
                            return match.ToString();
                    }
                });
            }

            public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }

        // %T165SCRTL[x]
        private class ScoreTotalReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(@"%T165SCRTL([1-6])");

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public ScoreTotalReplacer(Th165Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var type = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);

                    switch (type)
                    {
                        case 1:     // total score
                            return Utils.ToNumberString(parent.allScoreData.Scores.Sum(score => score.HighScore));
                        case 2:     // total of challenge counts
                            return Utils.ToNumberString(parent.allScoreData.Scores.Sum(score => score.ChallengeCount));
                        case 3:     // total of cleared counts
                            return Utils.ToNumberString(parent.allScoreData.Scores.Sum(score => score.ClearCount));
                        case 4:     // num of cleared scenes
                            return Utils.ToNumberString(
                                parent.allScoreData.Scores.Count(score => score.ClearCount > 0));
                        case 5:     // num of photos
                            return Utils.ToNumberString(parent.allScoreData.Scores.Sum(score => score.NumPhotos));
                        case 6:     // num of nicknames
                            return Utils.ToNumberString(
                                parent.allScoreData.Status.NicknameFlags.Count(flag => flag > 0));
                        default:    // unreachable
                            return match.ToString();
                    }
                });
            }

            public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }

        // %T165CARD[xx][y][z]
        private class CardReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T165CARD({0})([1-7])([12])", Parsers.DayParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public CardReplacer(Th165Converter parent, bool hideUntriedCards)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var day = Parsers.DayParser.Parse(match.Groups[1].Value);
                    var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    var key = (day, scene);
                    if (!Definitions.SpellCards.ContainsKey(key))
                        return match.ToString();

                    if (hideUntriedCards)
                    {
                        var score = parent.allScoreData.Scores.FirstOrDefault(elem =>
                            (elem != null) && ((elem.Number >= 0) && (elem.Number < Definitions.SpellCards.Count)) &&
                            Definitions.SpellCards.ElementAt(elem.Number).Key.Equals(key));
                        if ((score == null) || (score.ChallengeCount <= 0))
                            return "??????????";
                    }

                    if (type == 1)
                    {
                        return string.Join(
                            " &amp; ",
                            Definitions.SpellCards[key].Enemies.Select(enemy => enemy.ToLongName()).ToArray());
                    }
                    else
                    {
                        return Definitions.SpellCards[key].Card;
                    }
                });
            }

            public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }

        // %T165NICK[xx]
        private class NicknameReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T165NICK(\d{2})";

            private readonly MatchEvaluator evaluator;

            public NicknameReplacer(Th165Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);

                    if ((number > 0) && (number <= Definitions.Nicknames.Count))
                    {
                        return (parent.allScoreData.Status.NicknameFlags.ElementAt(number) > 0)
                            ? Definitions.Nicknames[number - 1] : "??????????";
                    }
                    else
                    {
                        return match.ToString();
                    }
                });
            }

            public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }

        // %T165TIMEPLY
        private class TimeReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T165TIMEPLY";

            private readonly MatchEvaluator evaluator;

            public TimeReplacer(Th165Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    return new Time(parent.allScoreData.Status.TotalPlayTime * 10, false).ToLongString();
                });
            }

            public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }

        // %T165SHOT[xx][y]
        private class ShotReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T165SHOT({0})([1-7])", Parsers.DayParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ShotReplacer(Th165Converter parent, string outputFilePath)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var day = Parsers.DayParser.Parse(match.Groups[1].Value);
                    var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                    var key = (day, scene);
                    if (!Definitions.SpellCards.ContainsKey(key))
                        return match.ToString();

                    if (!string.IsNullOrEmpty(outputFilePath) &&
                        parent.bestshots.TryGetValue(key, out var bestshot))
                    {
                        var relativePath = new Uri(outputFilePath)
                            .MakeRelativeUri(new Uri(bestshot.Path)).OriginalString;
                        var alternativeString = Utils.Format("SpellName: {0}", Definitions.SpellCards[key].Card);
                        return Utils.Format(
                            "<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" border=0>",
                            relativePath,
                            alternativeString);
                    }
                    else
                    {
                        return string.Empty;
                    }
                });
            }

            public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
        }

        // %T165SHOTEX[xx][y][z]
        private class ShotExReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T165SHOTEX({0})([1-7])([1-9])", Parsers.DayParser.Pattern);

            private static readonly Func<IBestShotHeader, List<Hashtag>> HashtagList =
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

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public ShotExReplacer(Th165Converter parent, string outputFilePath)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var day = Parsers.DayParser.Parse(match.Groups[1].Value);
                    var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    var key = (day, scene);
                    if (!Definitions.SpellCards.ContainsKey(key))
                        return match.ToString();

                    if (!string.IsNullOrEmpty(outputFilePath) &&
                        parent.bestshots.TryGetValue(key, out var bestshot))
                    {
                        switch (type)
                        {
                            case 1:     // relative path to the bestshot file
                                return new Uri(outputFilePath).MakeRelativeUri(new Uri(bestshot.Path)).OriginalString;
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
                        switch (type)
                        {
                            case 1: return string.Empty;
                            case 2: return "0";
                            case 3: return "0";
                            case 4: return "----/--/-- --:--:--";
                            case 5: return string.Empty;
                            case 6: return "0";
                            case 7: return "0";
                            case 8: return "0";
                            case 9: return "0";
                            default: return match.ToString();
                        }
                    }
                });
            }

            public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
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
