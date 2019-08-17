//-----------------------------------------------------------------------
// <copyright file="Th06Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591
#pragma warning disable SA1600 // ElementsMustBeDocumented
#pragma warning disable SA1602 // EnumerationItemsMustBeDocumented

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Models.Th06;
using CardInfo = ThScoreFileConverter.Models.SpellCardInfo<
    ThScoreFileConverter.Models.ThConverter.Stage, ThScoreFileConverter.Models.ThConverter.Level>;

namespace ThScoreFileConverter.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th06Converter : ThConverter
    {
        // Thanks to thwiki.info
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly", Justification = "Reviewed.")]
        private static readonly Dictionary<int, CardInfo> CardTable =
            new List<CardInfo>()
            {
                new CardInfo( 1, "月符「ムーンライトレイ」",         Stage.St1,   Level.Hard, Level.Lunatic),
                new CardInfo( 2, "夜符「ナイトバード」",             Stage.St1,   Level.Easy, Level.Normal, Level.Hard, Level.Lunatic),
                new CardInfo( 3, "闇符「ディマーケイション」",       Stage.St1,   Level.Easy, Level.Normal, Level.Hard, Level.Lunatic),
                new CardInfo( 4, "氷符「アイシクルフォール」",       Stage.St2,   Level.Easy, Level.Normal),
                new CardInfo( 5, "雹符「ヘイルストーム」",           Stage.St2,   Level.Hard, Level.Lunatic),
                new CardInfo( 6, "凍符「パーフェクトフリーズ」",     Stage.St2,   Level.Easy, Level.Normal, Level.Hard, Level.Lunatic),
                new CardInfo( 7, "雪符「ダイアモンドブリザード」",   Stage.St2,   Level.Normal, Level.Hard, Level.Lunatic),
                new CardInfo( 8, "華符「芳華絢爛」",                 Stage.St3,   Level.Easy, Level.Normal),
                new CardInfo( 9, "華符「セラギネラ９」",             Stage.St3,   Level.Hard, Level.Lunatic),
                new CardInfo(10, "虹符「彩虹の風鈴」",               Stage.St3,   Level.Easy, Level.Normal, Level.Hard, Level.Lunatic),
                new CardInfo(11, "幻符「華想夢葛」",                 Stage.St3,   Level.Hard, Level.Lunatic),
                new CardInfo(12, "彩符「彩雨」",                     Stage.St3,   Level.Easy, Level.Normal),
                new CardInfo(13, "彩符「彩光乱舞」",                 Stage.St3,   Level.Hard, Level.Lunatic),
                new CardInfo(14, "彩符「極彩颱風」",                 Stage.St3,   Level.Normal, Level.Hard, Level.Lunatic),
                new CardInfo(15, "火符「アグニシャイン」",           Stage.St4,   Level.Easy, Level.Normal),
                new CardInfo(16, "水符「プリンセスウンディネ」",     Stage.St4,   Level.Easy, Level.Normal),
                new CardInfo(17, "木符「シルフィホルン」",           Stage.St4,   Level.Easy, Level.Normal),
                new CardInfo(18, "土符「レイジィトリリトン」",       Stage.St4,   Level.Easy, Level.Normal),
                new CardInfo(19, "金符「メタルファティーグ」",       Stage.St4,   Level.Normal),
                new CardInfo(20, "火符「アグニシャイン上級」",       Stage.St4,   Level.Normal, Level.Hard, Level.Lunatic),
                new CardInfo(21, "木符「シルフィホルン上級」",       Stage.St4,   Level.Normal, Level.Hard, Level.Lunatic),
                new CardInfo(22, "土符「レイジィトリリトン上級」",   Stage.St4,   Level.Normal, Level.Hard, Level.Lunatic),
                new CardInfo(23, "火符「アグニレイディアンス」",     Stage.St4,   Level.Hard, Level.Lunatic),
                new CardInfo(24, "水符「ベリーインレイク」",         Stage.St4,   Level.Hard, Level.Lunatic),
                new CardInfo(25, "木符「グリーンストーム」",         Stage.St4,   Level.Hard, Level.Lunatic),
                new CardInfo(26, "土符「トリリトンシェイク」",       Stage.St4,   Level.Hard, Level.Lunatic),
                new CardInfo(27, "金符「シルバードラゴン」",         Stage.St4,   Level.Hard, Level.Lunatic),
                new CardInfo(28, "火＆土符「ラーヴァクロムレク」",   Stage.St4,   Level.Easy, Level.Normal, Level.Hard, Level.Lunatic),
                new CardInfo(29, "木＆火符「フォレストブレイズ」",   Stage.St4,   Level.Easy, Level.Normal, Level.Hard, Level.Lunatic),
                new CardInfo(30, "水＆木符「ウォーターエルフ」",     Stage.St4,   Level.Easy, Level.Normal, Level.Hard, Level.Lunatic),
                new CardInfo(31, "金＆水符「マーキュリポイズン」",   Stage.St4,   Level.Normal, Level.Hard, Level.Lunatic),
                new CardInfo(32, "土＆金符「エメラルドメガリス」",   Stage.St4,   Level.Easy, Level.Normal, Level.Hard, Level.Lunatic),
                new CardInfo(33, "奇術「ミスディレクション」",       Stage.St5,   Level.Easy, Level.Normal),
                new CardInfo(34, "奇術「幻惑ミスディレクション」",   Stage.St5,   Level.Hard, Level.Lunatic),
                new CardInfo(35, "幻在「クロックコープス」",         Stage.St5,   Level.Easy, Level.Normal),
                new CardInfo(36, "幻象「ルナクロック」",             Stage.St5,   Level.Easy, Level.Normal),
                new CardInfo(37, "メイド秘技「操りドール」",         Stage.St5,   Level.Easy, Level.Normal),
                new CardInfo(38, "幻幽「ジャック・ザ・ルドビレ」",   Stage.St5,   Level.Hard, Level.Lunatic),
                new CardInfo(39, "幻世「ザ・ワールド」",             Stage.St5,   Level.Hard, Level.Lunatic),
                new CardInfo(40, "メイド秘技「殺人ドール」",         Stage.St5,   Level.Hard, Level.Lunatic),
                new CardInfo(41, "奇術「エターナルミーク」",         Stage.St6,   Level.Normal, Level.Hard, Level.Lunatic),
                new CardInfo(42, "天罰「スターオブダビデ」",         Stage.St6,   Level.Normal),
                new CardInfo(43, "冥符「紅色の冥界」",               Stage.St6,   Level.Normal),
                new CardInfo(44, "呪詛「ブラド・ツェペシュの呪い」", Stage.St6,   Level.Normal),
                new CardInfo(45, "紅符「スカーレットシュート」",     Stage.St6,   Level.Normal),
                new CardInfo(46, "「レッドマジック」",               Stage.St6,   Level.Normal),
                new CardInfo(47, "神罰「幼きデーモンロード」",       Stage.St6,   Level.Hard, Level.Lunatic),
                new CardInfo(48, "獄符「千本の針の山」",             Stage.St6,   Level.Hard, Level.Lunatic),
                new CardInfo(49, "神術「吸血鬼幻想」",               Stage.St6,   Level.Hard, Level.Lunatic),
                new CardInfo(50, "紅符「スカーレットマイスタ」",     Stage.St6,   Level.Hard, Level.Lunatic),
                new CardInfo(51, "「紅色の幻想郷」",                 Stage.St6,   Level.Hard, Level.Lunatic),
                new CardInfo(52, "月符「サイレントセレナ」",         Stage.Extra, Level.Extra),
                new CardInfo(53, "日符「ロイヤルフレア」",           Stage.Extra, Level.Extra),
                new CardInfo(54, "火水木金土符「賢者の石」",         Stage.Extra, Level.Extra),
                new CardInfo(55, "禁忌「クランベリートラップ」",     Stage.Extra, Level.Extra),
                new CardInfo(56, "禁忌「レーヴァテイン」",           Stage.Extra, Level.Extra),
                new CardInfo(57, "禁忌「フォーオブアカインド」",     Stage.Extra, Level.Extra),
                new CardInfo(58, "禁忌「カゴメカゴメ」",             Stage.Extra, Level.Extra),
                new CardInfo(59, "禁忌「恋の迷路」",                 Stage.Extra, Level.Extra),
                new CardInfo(60, "禁弾「スターボウブレイク」",       Stage.Extra, Level.Extra),
                new CardInfo(61, "禁弾「カタディオプトリック」",     Stage.Extra, Level.Extra),
                new CardInfo(62, "禁弾「過去を刻む時計」",           Stage.Extra, Level.Extra),
                new CardInfo(63, "秘弾「そして誰もいなくなるか？」", Stage.Extra, Level.Extra),
                new CardInfo(64, "ＱＥＤ「４９５年の波紋」",         Stage.Extra, Level.Extra),
            }.ToDictionary(card => card.Id);

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly", Justification = "Reviewed.")]
        private static readonly List<HighScore> InitialRanking =
            new List<HighScore>()
            {
                new HighScore(1000000),
                new HighScore( 900000),
                new HighScore( 800000),
                new HighScore( 700000),
                new HighScore( 600000),
                new HighScore( 500000),
                new HighScore( 400000),
                new HighScore( 300000),
                new HighScore( 200000),
                new HighScore( 100000),
            };

        private static readonly EnumShortNameParser<Chara> CharaParser = new EnumShortNameParser<Chara>();

        private AllScoreData allScoreData = null;

        public enum Chara
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("RA")] ReimuA,
            [EnumAltName("RB")] ReimuB,
            [EnumAltName("MA")] MarisaA,
            [EnumAltName("MB")] MarisaB,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum CharaWithTotal
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("RA")] ReimuA,
            [EnumAltName("RB")] ReimuB,
            [EnumAltName("MA")] MarisaA,
            [EnumAltName("MB")] MarisaB,
            [EnumAltName("TL")] Total,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum StageProgress
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("-------")]     None,
            [EnumAltName("Stage 1")]     St1,
            [EnumAltName("Stage 2")]     St2,
            [EnumAltName("Stage 3")]     St3,
            [EnumAltName("Stage 4")]     St4,
            [EnumAltName("Stage 5")]     St5,
            [EnumAltName("Stage 6")]     St6,
            [EnumAltName("Extra Stage")] Extra,
            [EnumAltName("All Clear")]   Clear = 99,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public override string SupportedVersions
        {
            get { return "1.02h"; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th06decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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
                new CareerReplacer(this),
                new CardReplacer(this, hideUntriedCards),
                new CollectRateReplacer(this),
                new ClearReplacer(this),
                new PracticeReplacer(this),
            };
        }

        private static bool Decrypt(Stream input, Stream output)
        {
            var size = (int)input.Length;
            var data = new byte[size];
            input.Read(data, 0, size);

            uint checksum = 0;
            byte temp = 0;
            for (var index = 2; index < size; index++)
            {
                temp += data[index - 1];
                temp = (byte)((temp >> 5) | (temp << 3));
                data[index] ^= temp;
                if (index > 3)
                    checksum += data[index];
            }

            output.Write(data, 0, size);

            return (ushort)checksum == BitConverter.ToUInt16(data, 2);
        }

        private static bool Extract(Stream input, Stream output)
        {
            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            using (var writer = new BinaryWriter(output, Encoding.UTF8, true))
            {
                var header = new FileHeader();

                header.ReadFrom(reader);
                if (!header.IsValid)
                    return false;
                if (header.DecodedAllSize != input.Length)
                    return false;

                header.WriteTo(writer);

#if false
                Lzss.Extract(input, output);
#else
                var body = new byte[header.DecodedAllSize - header.Size];
                input.Read(body, 0, body.Length);
                output.Write(body, 0, body.Length);
#endif
                output.Flush();
                output.SetLength(output.Position);

                return output.Position == header.DecodedAllSize;
            }
        }

        private static bool Validate(Stream input)
        {
            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            {
                var header = new FileHeader();
                var chapter = new Chapter();

                header.ReadFrom(reader);
                var remainSize = header.DecodedAllSize - header.Size;
                if (remainSize <= 0)
                    return false;

                try
                {
                    while (remainSize > 0)
                    {
                        chapter.ReadFrom(reader);
                        if (chapter.Size1 == 0)
                            return false;

                        switch (chapter.Signature)
                        {
                            case Header.ValidSignature:
                                if (chapter.FirstByteOfData != 0x10)
                                    return false;
                                break;
                            default:
                                break;
                        }

                        remainSize -= chapter.Size1;
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
            var dictionary = new Dictionary<string, Action<AllScoreData, Chapter>>
            {
                { Header.ValidSignature,        (data, ch) => data.Set(new Header(ch))        },
                { HighScore.ValidSignature,     (data, ch) => data.Set(new HighScore(ch))     },
                { ClearData.ValidSignature,     (data, ch) => data.Set(new ClearData(ch))     },
                { CardAttack.ValidSignature,    (data, ch) => data.Set(new CardAttack(ch))    },
                { PracticeScore.ValidSignature, (data, ch) => data.Set(new PracticeScore(ch)) },
            };

            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            {
                var allScoreData = new AllScoreData();
                var chapter = new Chapter();

                reader.ReadExactBytes(FileHeader.ValidSize);

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
                    //// (allScoreData.rankings.Count >= 0) &&
                    //// (allScoreData.cardAttacks.Length == NumCards) &&
                    //// (allScoreData.practiceScores.Count >= 0) &&
                    (allScoreData.ClearData.Count == Enum.GetValues(typeof(Chara)).Length))
                    return allScoreData;
                else
                    return null;
            }
        }

        // %T06SCR[w][xx][y][z]
        private class ScoreReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T06SCR({0})({1})(\d)([1-3])", LevelParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ScoreReplacer(Th06Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var chara = CharaParser.Parse(match.Groups[2].Value);
                    var rank = Utils.ToZeroBased(
                        int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    var key = (chara, level);
                    var score = parent.allScoreData.Rankings.ContainsKey(key)
                        ? parent.allScoreData.Rankings[key][rank] : InitialRanking[rank];

                    switch (type)
                    {
                        case 1:     // name
                            return Encoding.Default.GetString(score.Name).Split('\0')[0];
                        case 2:     // score
                            return Utils.ToNumberString(score.Score);
                        case 3:     // stage
                            return score.StageProgress.ToShortName();
                        default:    // unreachable
                            return match.ToString();
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T06C[xx][y]
        private class CareerReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T06C(\d{2})([12])";

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CareerReplacer(Th06Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var type = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                    Func<CardAttack, int> getCount;
                    if (type == 1)
                        getCount = (attack => attack.ClearCount);
                    else
                        getCount = (attack => attack.TrialCount);

                    if (number == 0)
                    {
                        return Utils.ToNumberString(parent.allScoreData.CardAttacks.Values.Sum(getCount));
                    }
                    else if (CardTable.ContainsKey(number))
                    {
                        if (parent.allScoreData.CardAttacks.TryGetValue(number, out CardAttack attack))
                            return Utils.ToNumberString(getCount(attack));
                        else
                            return "0";
                    }
                    else
                    {
                        return match.ToString();
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T06CARD[xx][y]
        private class CardReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T06CARD(\d{2})([NR])";

            private readonly MatchEvaluator evaluator;

            public CardReplacer(Th06Converter parent, bool hideUntriedCards)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var type = match.Groups[2].Value.ToUpperInvariant();

                    if (CardTable.ContainsKey(number))
                    {
                        if (hideUntriedCards)
                        {
                            if (!parent.allScoreData.CardAttacks.TryGetValue(number, out CardAttack attack) ||
                                !attack.HasTried())
                                return (type == "N") ? "??????????" : "?????";
                        }

                        return (type == "N")
                            ? CardTable[number].Name
                            : string.Join(
                                ", ", CardTable[number].Levels.Select(lv => lv.ToString()).ToArray());
                    }
                    else
                    {
                        return match.ToString();
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T06CRG[x][y]
        private class CollectRateReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T06CRG({0})([12])", StageWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CollectRateReplacer(Th06Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var stage = StageWithTotalParser.Parse(match.Groups[1].Value);
                    var type = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                    Func<CardAttack, bool> findByStage;
                    if (stage == StageWithTotal.Total)
                        findByStage = (attack => true);
                    else
                        findByStage = (attack => CardTable[attack.CardId].Stage == (Stage)stage);

                    Func<CardAttack, bool> findByType;
                    if (type == 1)
                        findByType = (attack => attack.ClearCount > 0);
                    else
                        findByType = (attack => attack.TrialCount > 0);

                    return parent.allScoreData.CardAttacks.Values
                        .Count(Utils.MakeAndPredicate(findByStage, findByType))
                        .ToString(CultureInfo.CurrentCulture);
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T06CLEAR[x][yy]
        private class ClearReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T06CLEAR({0})({1})", LevelParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ClearReplacer(Th06Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var chara = CharaParser.Parse(match.Groups[2].Value);

                    var key = (chara, level);
                    if (parent.allScoreData.Rankings.ContainsKey(key))
                    {
                        var stageProgress =
                            parent.allScoreData.Rankings[key].Max(rank => rank.StageProgress);
                        if (stageProgress == StageProgress.Extra)
                            return "Not Clear";
                        else
                            return stageProgress.ToShortName();
                    }
                    else
                    {
                        return StageProgress.None.ToShortName();
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T06PRAC[x][yy][z]
        private class PracticeReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T06PRAC({0})({1})({2})", LevelParser.Pattern, CharaParser.Pattern, StageParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public PracticeReplacer(Th06Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var chara = CharaParser.Parse(match.Groups[2].Value);
                    var stage = StageParser.Parse(match.Groups[3].Value);

                    if (level == Level.Extra)
                        return match.ToString();
                    if (stage == Stage.Extra)
                        return match.ToString();

                    var key = (chara, level);
                    if (parent.allScoreData.PracticeScores.ContainsKey(key))
                    {
                        var scores = parent.allScoreData.PracticeScores[key];
                        return scores.ContainsKey(stage)
                            ? Utils.ToNumberString(scores[stage].HighScore) : "0";
                    }
                    else
                    {
                        return "0";
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        private class AllScoreData
        {
            public AllScoreData()
            {
                var numCharas = Enum.GetValues(typeof(Chara)).Length;
                var numPairs = numCharas * Enum.GetValues(typeof(Level)).Length;
                this.Rankings = new Dictionary<(Chara, Level), List<HighScore>>(numPairs);
                this.ClearData = new Dictionary<Chara, ClearData>(numCharas);
                this.CardAttacks = new Dictionary<int, CardAttack>(CardTable.Count);
                this.PracticeScores = new Dictionary<(Chara, Level), Dictionary<Stage, PracticeScore>>(numPairs);
            }

            public Header Header { get; private set; }

            public Dictionary<(Chara, Level), List<HighScore>> Rankings { get; private set; }

            public Dictionary<Chara, ClearData> ClearData { get; private set; }

            public Dictionary<int, CardAttack> CardAttacks { get; private set; }

            public Dictionary<(Chara, Level), Dictionary<Stage, PracticeScore>> PracticeScores
            {
                get; private set;
            }

            public void Set(Header header) => this.Header = header;

            public void Set(HighScore score)
            {
                var key = (score.Chara, score.Level);
                if (!this.Rankings.ContainsKey(key))
                    this.Rankings.Add(key, new List<HighScore>(InitialRanking));
                var ranking = this.Rankings[key];
                ranking.Add(score);
                ranking.Sort((lhs, rhs) => rhs.Score.CompareTo(lhs.Score));
                ranking.RemoveAt(ranking.Count - 1);
            }

            public void Set(ClearData data)
            {
                if (!this.ClearData.ContainsKey(data.Chara))
                    this.ClearData.Add(data.Chara, data);
            }

            public void Set(CardAttack attack)
            {
                if (!this.CardAttacks.ContainsKey(attack.CardId))
                    this.CardAttacks.Add(attack.CardId, attack);
            }

            public void Set(PracticeScore score)
            {
                if ((score.Level != Level.Extra) && (score.Stage != Stage.Extra) &&
                    !((score.Level == Level.Easy) && (score.Stage == Stage.St6)))
                {
                    var key = (score.Chara, score.Level);
                    if (!this.PracticeScores.ContainsKey(key))
                    {
                        var numStages = Utils.GetEnumerator<Stage>()
                            .Where(st => st != Stage.Extra).Count();
                        this.PracticeScores.Add(key, new Dictionary<Stage, PracticeScore>(numStages));
                    }

                    var scores = this.PracticeScores[key];
                    if (!scores.ContainsKey(score.Stage))
                        scores.Add(score.Stage, score);
                }
            }
        }

        private class CardAttack : Chapter  // per card
        {
            public const string ValidSignature = "CATK";
            public const short ValidSize = 0x0040;

            public CardAttack(Chapter chapter)
                : base(chapter, ValidSignature, ValidSize)
            {
                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    reader.ReadExactBytes(8);
                    this.CardId = (short)(reader.ReadInt16() + 1);
                    reader.ReadExactBytes(6);
                    this.CardName = reader.ReadExactBytes(0x24);
                    this.TrialCount = reader.ReadUInt16();
                    this.ClearCount = reader.ReadUInt16();
                }
            }

            public short CardId { get; }    // 1-based

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] CardName { get; } // Null-terminated

            public ushort TrialCount { get; }

            public ushort ClearCount { get; }

            public bool HasTried() => this.TrialCount > 0;
        }

        private class PracticeScore : Chapter   // per character, level, stage
        {
            public const string ValidSignature = "PSCR";
            public const short ValidSize = 0x0014;

            public PracticeScore(Chapter chapter)
                : base(chapter, ValidSignature, ValidSize)
            {
                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    reader.ReadUInt32();    // always 0x00000010?
                    this.HighScore = reader.ReadInt32();
                    this.Chara = Utils.ToEnum<Chara>(reader.ReadByte());
                    this.Level = Utils.ToEnum<Level>(reader.ReadByte());
                    this.Stage = Utils.ToEnum<Stage>(reader.ReadByte());
                    reader.ReadByte();
                }
            }

            public int HighScore { get; }

            public Chara Chara { get; }

            public Level Level { get; }

            public Stage Stage { get; }
        }
    }
}
