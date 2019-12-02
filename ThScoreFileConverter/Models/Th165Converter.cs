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
using System.Collections.Specialized;
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

        private Dictionary<(Day Day, int Scene), (string Path, BestShotHeader Header)> bestshots = null;

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
                            new Dictionary<(Day, int), (string, BestShotHeader)>(Definitions.SpellCards.Count);
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

            private static readonly Func<BestShotHeader, List<Hashtag>> HashtagList =
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

        private class AllScoreData
        {
            private readonly List<IScore> scores;

            public AllScoreData() => this.scores = new List<IScore>(Definitions.SpellCards.Count);

            public Th095.HeaderBase Header { get; private set; }

            public IReadOnlyList<IScore> Scores => this.scores;

            public IStatus Status { get; private set; }

            public void Set(Th095.HeaderBase header) => this.Header = header;

            public void Set(IScore score) => this.scores.Add(score);

            public void Set(IStatus status) => this.Status = status;
        }

        private class Status : Th10.Chapter, IStatus
        {
            public const string ValidSignature = "ST";
            public const ushort ValidVersion = 0x0002;
            public const int ValidSize = 0x00000224;

            public Status(Th10.Chapter chapter)
                : base(chapter, ValidSignature, ValidVersion, ValidSize)
            {
                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    this.LastName = reader.ReadExactBytes(14);
                    reader.ReadExactBytes(0x12);
                    this.BgmFlags = reader.ReadExactBytes(8);
                    reader.ReadExactBytes(0x18);
                    this.TotalPlayTime = reader.ReadInt32();
                    reader.ReadInt32(); // always 0?
                    reader.ReadInt32(); // 0x15?
                    reader.ReadInt32(); // always 0?
                    reader.ReadExactBytes(0x40);    // story flags?
                    this.NicknameFlags = reader.ReadExactBytes(51);
                    reader.ReadExactBytes(0x155);
                }
            }

            public IEnumerable<byte> LastName { get; }  // The last 2 bytes are always 0x00 ?

            public IEnumerable<byte> BgmFlags { get; }

            public int TotalPlayTime { get; }   // unit: 10ms

            public IEnumerable<byte> NicknameFlags { get; } // The first byte should be ignored

            public static bool CanInitialize(Th10.Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
            }
        }

        private class BestShotHeader : IBinaryReadable
        {
            public const string ValidSignature = "BST4";
            public const int SignatureSize = 4;

            public string Signature { get; private set; }

            public Day Weekday { get; private set; }

            public short Dream { get; private set; } // 1-based

            public short Width { get; private set; }

            public short Height { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public short Width2 { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public short Height2 { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public short HalfWidth { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public short HalfHeight { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public float SlowRate { get; private set; }

            public uint DateTime { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public float Angle { get; private set; } // -PI .. +PI [rad]

            public int Score { get; private set; }

            public HashtagFields Fields { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int Score2 { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int BasePoint { get; private set; } // FIXME

            public int NumViewed { get; private set; }

            public int NumLikes { get; private set; }

            public int NumFavs { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int NumBullets { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int NumBulletsNearby { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int RiskBonus { get; private set; } // max(NumBulletsNearby, 2) * 40 .. min(NumBulletsNearby, 25) * 40

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public float BossShot { get; private set; } // 1.20? .. 2.00

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public float AngleBonus { get; private set; } // 1.00? .. 1.30

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int MacroBonus { get; private set; } // 0 .. 60?

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public float LikesPerView { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public float FavsPerView { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int NumHashtags { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int NumRedBullets { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int NumPurpleBullets { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int NumBlueBullets { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int NumCyanBullets { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int NumGreenBullets { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int NumYellowBullets { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int NumOrangeBullets { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int NumLightBullets { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader is null)
                    throw new ArgumentNullException(nameof(reader));

                this.Signature = Encoding.Default.GetString(reader.ReadExactBytes(SignatureSize));
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException();

                reader.ReadUInt16();    // always 0x0401?
                this.Weekday = Utils.ToEnum<Day>(reader.ReadInt16());
                this.Dream = (short)(reader.ReadInt16() + 1);
                reader.ReadUInt16();    // 0x0100 ... Version?
                this.Width = reader.ReadInt16();
                this.Height = reader.ReadInt16();
                reader.ReadInt32(); // always 0?
                this.Width2 = reader.ReadInt16();
                this.Height2 = reader.ReadInt16();
                this.HalfWidth = reader.ReadInt16();
                this.HalfHeight = reader.ReadInt16();
                reader.ReadInt32(); // always 0?
                this.SlowRate = reader.ReadSingle();
                this.DateTime = reader.ReadUInt32();
                reader.ReadInt32(); // always 0?
                this.Angle = reader.ReadSingle();
                this.Score = reader.ReadInt32();
                reader.ReadInt32(); // always 0?
                this.Fields = new HashtagFields(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
                reader.ReadBytes(0x28); // always all 0?
                this.Score2 = reader.ReadInt32();
                this.BasePoint = reader.ReadInt32();
                this.NumViewed = reader.ReadInt32();
                this.NumLikes = reader.ReadInt32();
                this.NumFavs = reader.ReadInt32();
                this.NumBullets = reader.ReadInt32();
                this.NumBulletsNearby = reader.ReadInt32();
                this.RiskBonus = reader.ReadInt32();
                this.BossShot = reader.ReadSingle();
                reader.ReadInt32(); // always 0? (Nice Shot?)
                this.AngleBonus = reader.ReadSingle();
                this.MacroBonus = reader.ReadInt32();
                reader.ReadInt32(); // always 0? (Front/Side/Back Shot?)
                reader.ReadInt32(); // always 0? (Clear Shot?)
                this.LikesPerView = reader.ReadSingle();
                this.FavsPerView = reader.ReadSingle();
                this.NumHashtags = reader.ReadInt32();
                this.NumRedBullets = reader.ReadInt32();
                this.NumPurpleBullets = reader.ReadInt32();
                this.NumBlueBullets = reader.ReadInt32();
                this.NumCyanBullets = reader.ReadInt32();
                this.NumGreenBullets = reader.ReadInt32();
                this.NumYellowBullets = reader.ReadInt32();
                this.NumOrangeBullets = reader.ReadInt32();
                this.NumLightBullets = reader.ReadInt32();
                reader.ReadBytes(0x44); // always all 0?
                reader.ReadBytes(0x34);
            }

            public struct HashtagFields
            {
                private static readonly int[] Masks;

                private readonly BitVector32[] data;

#pragma warning disable CA2207 // Initialize value type static fields inline
                static HashtagFields()
                {
                    Masks = new int[32];
                    Masks[0] = BitVector32.CreateMask();
                    for (var i = 1; i < Masks.Length; i++)
                    {
                        Masks[i] = BitVector32.CreateMask(Masks[i - 1]);
                    }
                }
#pragma warning restore CA2207 // Initialize value type static fields inline

                public HashtagFields(int data1, int data2, int data3)
                {
                    this.data = new BitVector32[3];
                    this.data[0] = new BitVector32(data1);
                    this.data[1] = new BitVector32(data2);
                    this.data[2] = new BitVector32(data3);
                }

                [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
                public IEnumerable<int> Data => this.data?.Select(vector => vector.Data);

                [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
                public bool EnemyIsInFrame => this.data[0][Masks[0]]; // Not used

                public bool EnemyIsPartlyInFrame => this.data[0][Masks[1]];

                public bool WholeEnemyIsInFrame => this.data[0][Masks[2]];

                public bool EnemyIsInMiddle => this.data[0][Masks[3]];

                public bool IsSelfie => this.data[0][Masks[4]];

                public bool IsTwoShot => this.data[0][Masks[5]];

                public bool BitDangerous => this.data[0][Masks[7]];

                public bool SeriouslyDangerous => this.data[0][Masks[8]];

                public bool ThoughtGonnaDie => this.data[0][Masks[9]];

                public bool ManyReds => this.data[0][Masks[10]];

                public bool ManyPurples => this.data[0][Masks[11]];

                public bool ManyBlues => this.data[0][Masks[12]];

                public bool ManyCyans => this.data[0][Masks[13]];

                public bool ManyGreens => this.data[0][Masks[14]];

                public bool ManyYellows => this.data[0][Masks[15]];

                public bool ManyOranges => this.data[0][Masks[16]];

                public bool TooColorful => this.data[0][Masks[17]];

                public bool SevenColors => this.data[0][Masks[18]];

                [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
                public bool NoBullet => this.data[0][Masks[19]]; // Not used

                public bool IsLandscapePhoto => this.data[0][Masks[21]];

                public bool Closeup => this.data[0][Masks[26]];

                public bool QuiteCloseup => this.data[0][Masks[27]];

                public bool TooClose => this.data[0][Masks[28]];

                public bool EnemyIsInFullView => this.data[1][Masks[1]];

                public bool TooManyBullets => this.data[1][Masks[4]];

                public bool TooPlayfulBarrage => this.data[1][Masks[5]];

                public bool TooDense => this.data[1][Masks[6]]; // FIXME

                public bool Chased => this.data[1][Masks[7]];

                public bool IsSuppository => this.data[1][Masks[8]];

                public bool IsButterflyLikeMoth => this.data[1][Masks[9]];

                public bool EnemyIsUndamaged => this.data[1][Masks[10]];

                public bool EnemyCanAfford => this.data[1][Masks[11]];

                public bool EnemyIsWeakened => this.data[1][Masks[12]];

                public bool EnemyIsDying => this.data[1][Masks[13]];

                public bool Finished => this.data[1][Masks[14]];

                public bool IsThreeShot => this.data[1][Masks[15]];

                public bool TwoEnemiesTogether => this.data[1][Masks[16]];

                public bool EnemiesAreOverlapping => this.data[1][Masks[17]];

                public bool PeaceSignAlongside => this.data[1][Masks[18]];

                public bool EnemiesAreTooClose => this.data[1][Masks[19]]; // FIXME

                public bool Scorching => this.data[1][Masks[20]];

                public bool TooBigBullet => this.data[1][Masks[21]];

                public bool ThrowingEdgedTools => this.data[1][Masks[22]];

                public bool Snaky => this.data[1][Masks[23]];

                public bool LightLooksStopped => this.data[1][Masks[24]];

                public bool IsSuperMoon => this.data[1][Masks[25]];

                public bool Dazzling => this.data[1][Masks[26]];

                public bool MoreDazzling => this.data[1][Masks[27]];

                public bool MostDazzling => this.data[1][Masks[28]];

                public bool FinishedTogether => this.data[1][Masks[29]];

                [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
                public bool WasDream => this.data[1][Masks[30]]; // FIXME; Not used

                public bool IsRockyBarrage => this.data[1][Masks[31]];

                public bool IsStickDestroyingBarrage => this.data[2][Masks[0]];

                public bool Fluffy => this.data[2][Masks[1]];

                public bool IsDoggiePhoto => this.data[2][Masks[2]];

                public bool IsAnimalPhoto => this.data[2][Masks[3]];

                public bool IsZoo => this.data[2][Masks[4]];

                public bool IsLovelyHeart => this.data[2][Masks[5]]; // FIXME

                public bool IsThunder => this.data[2][Masks[6]];

                public bool IsDrum => this.data[2][Masks[7]];

                public bool IsMisty => this.data[2][Masks[8]]; // FIXME

                public bool IsBoringPhoto => this.data[2][Masks[9]];

                public bool WasScolded => this.data[2][Masks[10]]; // FIXME

                public bool IsSumireko => this.data[2][Masks[11]];
            }
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
