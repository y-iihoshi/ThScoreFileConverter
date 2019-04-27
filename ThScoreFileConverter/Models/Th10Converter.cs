//-----------------------------------------------------------------------
// <copyright file="Th10Converter.cs" company="None">
//     (c) 2013-2019 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed.")]

namespace ThScoreFileConverter.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using CardInfo = SpellCardInfo<ThConverter.Stage, ThConverter.Level>;

    internal class Th10Converter : ThConverter
    {
        // Thanks to thwiki.info
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly", Justification = "Reviewed.")]
        private static readonly Dictionary<int, CardInfo> CardTable =
            new List<CardInfo>()
            {
                new CardInfo(  1, "葉符「狂いの落葉」",                       Stage.St1,   Level.Hard),
                new CardInfo(  2, "葉符「狂いの落葉」",                       Stage.St1,   Level.Lunatic),
                new CardInfo(  3, "秋符「オータムスカイ」",                   Stage.St1,   Level.Easy),
                new CardInfo(  4, "秋符「オータムスカイ」",                   Stage.St1,   Level.Normal),
                new CardInfo(  5, "秋符「秋の空と乙女の心」",                 Stage.St1,   Level.Hard),
                new CardInfo(  6, "秋符「秋の空と乙女の心」",                 Stage.St1,   Level.Lunatic),
                new CardInfo(  7, "豊符「オヲトシハーベスター」",             Stage.St1,   Level.Easy),
                new CardInfo(  8, "豊符「オヲトシハーベスター」",             Stage.St1,   Level.Normal),
                new CardInfo(  9, "豊作「穀物神の約束」",                     Stage.St1,   Level.Hard),
                new CardInfo( 10, "豊作「穀物神の約束」",                     Stage.St1,   Level.Lunatic),
                new CardInfo( 11, "厄符「バッドフォーチュン」",               Stage.St2,   Level.Easy),
                new CardInfo( 12, "厄符「バッドフォーチュン」",               Stage.St2,   Level.Normal),
                new CardInfo( 13, "厄符「厄神様のバイオリズム」",             Stage.St2,   Level.Hard),
                new CardInfo( 14, "厄符「厄神様のバイオリズム」",             Stage.St2,   Level.Lunatic),
                new CardInfo( 15, "疵符「ブロークンアミュレット」",           Stage.St2,   Level.Easy),
                new CardInfo( 16, "疵符「ブロークンアミュレット」",           Stage.St2,   Level.Normal),
                new CardInfo( 17, "疵痕「壊されたお守り」",                   Stage.St2,   Level.Hard),
                new CardInfo( 18, "疵痕「壊されたお守り」",                   Stage.St2,   Level.Lunatic),
                new CardInfo( 19, "悪霊「ミスフォーチュンズホイール」",       Stage.St2,   Level.Easy),
                new CardInfo( 20, "悪霊「ミスフォーチュンズホイール」",       Stage.St2,   Level.Normal),
                new CardInfo( 21, "悲運「大鐘婆の火」",                       Stage.St2,   Level.Hard),
                new CardInfo( 22, "悲運「大鐘婆の火」",                       Stage.St2,   Level.Lunatic),
                new CardInfo( 23, "創符「ペインフロー」",                     Stage.St2,   Level.Easy),
                new CardInfo( 24, "創符「ペインフロー」",                     Stage.St2,   Level.Normal),
                new CardInfo( 25, "創符「流刑人形」",                         Stage.St2,   Level.Hard),
                new CardInfo( 26, "創符「流刑人形」",                         Stage.St2,   Level.Lunatic),
                new CardInfo( 27, "光学「オプティカルカモフラージュ」",       Stage.St3,   Level.Easy),
                new CardInfo( 28, "光学「オプティカルカモフラージュ」",       Stage.St3,   Level.Normal),
                new CardInfo( 29, "光学「ハイドロカモフラージュ」",           Stage.St3,   Level.Hard),
                new CardInfo( 30, "光学「ハイドロカモフラージュ」",           Stage.St3,   Level.Lunatic),
                new CardInfo( 31, "洪水「ウーズフラッディング」",             Stage.St3,   Level.Easy),
                new CardInfo( 32, "洪水「ウーズフラッディング」",             Stage.St3,   Level.Normal),
                new CardInfo( 33, "洪水「デリューヴィアルメア」",             Stage.St3,   Level.Hard),
                new CardInfo( 34, "漂溺「光り輝く水底のトラウマ」",           Stage.St3,   Level.Lunatic),
                new CardInfo( 35, "水符「河童のポロロッカ」",                 Stage.St3,   Level.Easy),
                new CardInfo( 36, "水符「河童のポロロッカ」",                 Stage.St3,   Level.Normal),
                new CardInfo( 37, "水符「河童のフラッシュフラッド」",         Stage.St3,   Level.Hard),
                new CardInfo( 38, "水符「河童の幻想大瀑布」",                 Stage.St3,   Level.Lunatic),
                new CardInfo( 39, "河童「お化けキューカンバー」",             Stage.St3,   Level.Easy),
                new CardInfo( 40, "河童「お化けキューカンバー」",             Stage.St3,   Level.Normal),
                new CardInfo( 41, "河童「のびーるアーム」",                   Stage.St3,   Level.Hard),
                new CardInfo( 42, "河童「スピン・ザ・セファリックプレート」", Stage.St3,   Level.Lunatic),
                new CardInfo( 43, "岐符「天の八衢」",                         Stage.St4,   Level.Easy),
                new CardInfo( 44, "岐符「天の八衢」",                         Stage.St4,   Level.Normal),
                new CardInfo( 45, "岐符「サルタクロス」",                     Stage.St4,   Level.Hard),
                new CardInfo( 46, "岐符「サルタクロス」",                     Stage.St4,   Level.Lunatic),
                new CardInfo( 47, "風神「風神木の葉隠れ」",                   Stage.St4,   Level.Easy),
                new CardInfo( 48, "風神「風神木の葉隠れ」",                   Stage.St4,   Level.Normal),
                new CardInfo( 49, "風神「天狗颪」",                           Stage.St4,   Level.Hard),
                new CardInfo( 50, "風神「二百十日」",                         Stage.St4,   Level.Lunatic),
                new CardInfo( 51, "「幻想風靡」",                             Stage.St4,   Level.Normal),
                new CardInfo( 52, "「幻想風靡」",                             Stage.St4,   Level.Hard),
                new CardInfo( 53, "「無双風神」",                             Stage.St4,   Level.Lunatic),
                new CardInfo( 54, "塞符「山神渡御」",                         Stage.St4,   Level.Easy),
                new CardInfo( 55, "塞符「山神渡御」",                         Stage.St4,   Level.Normal),
                new CardInfo( 56, "塞符「天孫降臨」",                         Stage.St4,   Level.Hard),
                new CardInfo( 57, "塞符「天上天下の照國」",                   Stage.St4,   Level.Lunatic),
                new CardInfo( 58, "秘術「グレイソーマタージ」",               Stage.St5,   Level.Easy),
                new CardInfo( 59, "秘術「グレイソーマタージ」",               Stage.St5,   Level.Normal),
                new CardInfo( 60, "秘術「忘却の祭儀」",                       Stage.St5,   Level.Hard),
                new CardInfo( 61, "秘術「一子相伝の弾幕」",                   Stage.St5,   Level.Lunatic),
                new CardInfo( 62, "奇跡「白昼の客星」",                       Stage.St5,   Level.Easy),
                new CardInfo( 63, "奇跡「白昼の客星」",                       Stage.St5,   Level.Normal),
                new CardInfo( 64, "奇跡「客星の明るい夜」",                   Stage.St5,   Level.Hard),
                new CardInfo( 65, "奇跡「客星の明るすぎる夜」",               Stage.St5,   Level.Lunatic),
                new CardInfo( 66, "開海「海が割れる日」",                     Stage.St5,   Level.Easy),
                new CardInfo( 67, "開海「海が割れる日」",                     Stage.St5,   Level.Normal),
                new CardInfo( 68, "開海「モーゼの奇跡」",                     Stage.St5,   Level.Hard),
                new CardInfo( 69, "開海「モーゼの奇跡」",                     Stage.St5,   Level.Lunatic),
                new CardInfo( 70, "準備「神風を喚ぶ星の儀式」",               Stage.St5,   Level.Easy),
                new CardInfo( 71, "準備「神風を喚ぶ星の儀式」",               Stage.St5,   Level.Normal),
                new CardInfo( 72, "準備「サモンタケミナカタ」",               Stage.St5,   Level.Hard),
                new CardInfo( 73, "準備「サモンタケミナカタ」",               Stage.St5,   Level.Lunatic),
                new CardInfo( 74, "奇跡「神の風」",                           Stage.St5,   Level.Easy),
                new CardInfo( 75, "奇跡「神の風」",                           Stage.St5,   Level.Normal),
                new CardInfo( 76, "大奇跡「八坂の神風」",                     Stage.St5,   Level.Hard),
                new CardInfo( 77, "大奇跡「八坂の神風」",                     Stage.St5,   Level.Lunatic),
                new CardInfo( 78, "神祭「エクスパンデッド・オンバシラ」",     Stage.St6,   Level.Easy),
                new CardInfo( 79, "神祭「エクスパンデッド・オンバシラ」",     Stage.St6,   Level.Normal),
                new CardInfo( 80, "奇祭「目処梃子乱舞」",                     Stage.St6,   Level.Hard),
                new CardInfo( 81, "奇祭「目処梃子乱舞」",                     Stage.St6,   Level.Lunatic),
                new CardInfo( 82, "筒粥「神の粥」",                           Stage.St6,   Level.Easy),
                new CardInfo( 83, "筒粥「神の粥」",                           Stage.St6,   Level.Normal),
                new CardInfo( 84, "忘穀「アンリメンバードクロップ」",         Stage.St6,   Level.Hard),
                new CardInfo( 85, "神穀「ディバイニングクロップ」",           Stage.St6,   Level.Lunatic),
                new CardInfo( 86, "贄符「御射山御狩神事」",                   Stage.St6,   Level.Easy),
                new CardInfo( 87, "贄符「御射山御狩神事」",                   Stage.St6,   Level.Normal),
                new CardInfo( 88, "神秘「葛井の清水」",                       Stage.St6,   Level.Hard),
                new CardInfo( 89, "神秘「ヤマトトーラス」",                   Stage.St6,   Level.Lunatic),
                new CardInfo( 90, "天流「お天水の奇跡」",                     Stage.St6,   Level.Easy),
                new CardInfo( 91, "天流「お天水の奇跡」",                     Stage.St6,   Level.Normal),
                new CardInfo( 92, "天竜「雨の源泉」",                         Stage.St6,   Level.Hard),
                new CardInfo( 93, "天竜「雨の源泉」",                         Stage.St6,   Level.Lunatic),
                new CardInfo( 94, "「マウンテン・オブ・フェイス」",           Stage.St6,   Level.Easy),
                new CardInfo( 95, "「マウンテン・オブ・フェイス」",           Stage.St6,   Level.Normal),
                new CardInfo( 96, "「風神様の神徳」",                         Stage.St6,   Level.Hard),
                new CardInfo( 97, "「風神様の神徳」",                         Stage.St6,   Level.Lunatic),
                new CardInfo( 98, "神符「水眼の如き美しき源泉」",             Stage.Extra, Level.Extra),
                new CardInfo( 99, "神符「杉で結ぶ古き縁」",                   Stage.Extra, Level.Extra),
                new CardInfo(100, "神符「神が歩かれた御神渡り」",             Stage.Extra, Level.Extra),
                new CardInfo(101, "開宴「二拝二拍一拝」",                     Stage.Extra, Level.Extra),
                new CardInfo(102, "土着神「手長足長さま」",                   Stage.Extra, Level.Extra),
                new CardInfo(103, "神具「洩矢の鉄の輪」",                     Stage.Extra, Level.Extra),
                new CardInfo(104, "源符「厭い川の翡翠」",                     Stage.Extra, Level.Extra),
                new CardInfo(105, "蛙狩「蛙は口ゆえ蛇に呑まるる」",           Stage.Extra, Level.Extra),
                new CardInfo(106, "土着神「七つの石と七つの木」",             Stage.Extra, Level.Extra),
                new CardInfo(107, "土着神「ケロちゃん風雨に負けず」",         Stage.Extra, Level.Extra),
                new CardInfo(108, "土着神「宝永四年の赤蛙」",                 Stage.Extra, Level.Extra),
                new CardInfo(109, "「諏訪大戦　～ 土着神話 vs 中央神話」",    Stage.Extra, Level.Extra),
                new CardInfo(110, "祟符「ミシャグジさま」",                   Stage.Extra, Level.Extra)
            }.ToDictionary(card => card.Id);

        private static readonly EnumShortNameParser<Chara> CharaParser =
            new EnumShortNameParser<Chara>();

        private static readonly EnumShortNameParser<CharaWithTotal> CharaWithTotalParser =
            new EnumShortNameParser<CharaWithTotal>();

        private AllScoreData allScoreData = null;

        public enum Chara
        {
            [EnumAltName("RA")] ReimuA,
            [EnumAltName("RB")] ReimuB,
            [EnumAltName("RC")] ReimuC,
            [EnumAltName("MA")] MarisaA,
            [EnumAltName("MB")] MarisaB,
            [EnumAltName("MC")] MarisaC
        }

        public enum CharaWithTotal
        {
            [EnumAltName("RA")] ReimuA,
            [EnumAltName("RB")] ReimuB,
            [EnumAltName("RC")] ReimuC,
            [EnumAltName("MA")] MarisaA,
            [EnumAltName("MB")] MarisaB,
            [EnumAltName("MC")] MarisaC,
            [EnumAltName("TL")] Total
        }

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        public enum StageProgress
        {
            [EnumAltName("-------")]     None,
            [EnumAltName("Stage 1")]     St1,
            [EnumAltName("Stage 2")]     St2,
            [EnumAltName("Stage 3")]     St3,
            [EnumAltName("Stage 4")]     St4,
            [EnumAltName("Stage 5")]     St5,
            [EnumAltName("Stage 6")]     St6,
            [EnumAltName("Extra Stage")] Extra,
            [EnumAltName("All Clear")]   Clear
        }

        public override string SupportedVersions
        {
            get { return "1.00a"; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th10decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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
                new CharaReplacer(this),
                new CharaExReplacer(this),
                new PracticeReplacer(this)
            };
        }

        private static bool Decrypt(Stream input, Stream output)
        {
            var reader = new BinaryReader(input);
            var writer = new BinaryWriter(output);

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

        private static bool Extract(Stream input, Stream output)
        {
            var reader = new BinaryReader(input);
            var writer = new BinaryWriter(output);

            var header = new Header();
            header.ReadFrom(reader);
            header.WriteTo(writer);

            var bodyBeginPos = output.Position;
            Lzss.Extract(input, output);
            output.Flush();
            output.SetLength(output.Position);

            return header.DecodedBodySize == (output.Position - bodyBeginPos);
        }

        private static bool Validate(Stream input)
        {
            var reader = new BinaryReader(input);

            var header = new Header();
            header.ReadFrom(reader);
            var remainSize = header.DecodedBodySize;
            var chapter = new Chapter();

            try
            {
                while (remainSize > 0)
                {
                    chapter.ReadFrom(reader);
                    if (!chapter.IsValid)
                        return false;
                    if (!ClearData.CanInitialize(chapter) && !Status.CanInitialize(chapter))
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

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        private static AllScoreData Read(Stream input)
        {
            var dictionary = new Dictionary<string, Action<AllScoreData, Chapter>>
            {
                { ClearData.ValidSignature, (data, ch) => data.Set(new ClearData(ch)) },
                { Status.ValidSignature,    (data, ch) => data.Set(new Status(ch))    }
            };

            var reader = new BinaryReader(input);
            var allScoreData = new AllScoreData();
            var chapter = new Chapter();

            var header = new Header();
            header.ReadFrom(reader);
            allScoreData.Set(header);

            try
            {
                while (true)
                {
                    chapter.ReadFrom(reader);
                    if (dictionary.TryGetValue(chapter.Signature, out Action<AllScoreData, Chapter> setChapter))
                        setChapter(allScoreData, chapter);
                }
            }
            catch (EndOfStreamException)
            {
                // It's OK, do nothing.
            }

            if ((allScoreData.Header != null) &&
                (allScoreData.ClearData.Count == Enum.GetValues(typeof(CharaWithTotal)).Length) &&
                (allScoreData.Status != null))
                return allScoreData;
            else
                return null;
        }

        // %T10SCR[w][xx][y][z]
        private class ScoreReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T10SCR({0})({1})(\d)([1-5])", LevelParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ScoreReplacer(Th10Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var chara = (CharaWithTotal)CharaParser.Parse(match.Groups[2].Value);
                    var rank = Utils.ToZeroBased(
                        int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    var ranking = parent.allScoreData.ClearData[chara].Rankings[level][rank];
                    switch (type)
                    {
                        case 1:     // name
                            return Encoding.Default.GetString(ranking.Name).Split('\0')[0];
                        case 2:     // score
                            return Utils.ToNumberString((ranking.Score * 10) + ranking.ContinueCount);
                        case 3:     // stage
                            if (ranking.DateTime > 0)
                                return (ranking.StageProgress == StageProgress.Extra)
                                    ? "Not Clear" : ranking.StageProgress.ToShortName();
                            else
                                return StageProgress.None.ToShortName();
                        case 4:     // date & time
                            if (ranking.DateTime > 0)
                                return new DateTime(1970, 1, 1).AddSeconds(ranking.DateTime).ToLocalTime()
                                    .ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture);
                            else
                                return "----/--/-- --:--:--";
                        case 5:     // slow
                            if (ranking.DateTime > 0)
                                return Utils.Format("{0:F3}%", ranking.SlowRate);
                            else
                                return "-----%";
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

        // %T10C[xxx][yy][z]
        private class CareerReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T10C(\d{{3}})({0})([12])", CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CareerReplacer(Th10Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var chara = CharaWithTotalParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    Func<SpellCard, int> getCount;
                    if (type == 1)
                        getCount = (card => card.ClearCount);
                    else
                        getCount = (card => card.TrialCount);

                    var cards = parent.allScoreData.ClearData[chara].Cards;
                    if (number == 0)
                        return Utils.ToNumberString(cards.Values.Sum(getCount));
                    else if (CardTable.ContainsKey(number))
                    {
                        if (cards.TryGetValue(number, out SpellCard card))
                            return Utils.ToNumberString(getCount(card));
                        else
                            return "0";
                    }
                    else
                        return match.ToString();
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T10CARD[xxx][y]
        private class CardReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T10CARD(\d{3})([NR])";

            private readonly MatchEvaluator evaluator;

            public CardReplacer(Th10Converter parent, bool hideUntriedCards)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var type = match.Groups[2].Value.ToUpperInvariant();

                    if (CardTable.ContainsKey(number))
                    {
                        if (type == "N")
                        {
                            if (hideUntriedCards)
                            {
                                var cards = parent.allScoreData.ClearData[CharaWithTotal.Total].Cards;
                                if (!cards.TryGetValue(number, out SpellCard card) || !card.HasTried())
                                    return "??????????";
                            }

                            return CardTable[number].Name;
                        }
                        else
                            return CardTable[number].Level.ToString();
                    }
                    else
                        return match.ToString();
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T10CRG[w][xx][y][z]
        private class CollectRateReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T10CRG({0})({1})({2})([12])",
                LevelWithTotalParser.Pattern,
                CharaWithTotalParser.Pattern,
                StageWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CollectRateReplacer(Th10Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelWithTotalParser.Parse(match.Groups[1].Value);
                    var chara = CharaWithTotalParser.Parse(match.Groups[2].Value);
                    var stage = StageWithTotalParser.Parse(match.Groups[3].Value);
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    if (stage == StageWithTotal.Extra)
                        return match.ToString();

                    Func<SpellCard, bool> findByStage;
                    if (stage == StageWithTotal.Total)
                        findByStage = (card => true);
                    else
                        findByStage = (card => CardTable[card.Id].Stage == (Stage)stage);

                    Func<SpellCard, bool> findByLevel = (card => true);
                    switch (level)
                    {
                        case LevelWithTotal.Total:
                            // Do nothing
                            break;
                        case LevelWithTotal.Extra:
                            findByStage = (card => CardTable[card.Id].Stage == Stage.Extra);
                            break;
                        default:
                            findByLevel = (card => card.Level == (Level)level);
                            break;
                    }

                    Func<SpellCard, bool> findByType;
                    if (type == 1)
                        findByType = (card => card.ClearCount > 0);
                    else
                        findByType = (card => card.TrialCount > 0);

                    return parent.allScoreData.ClearData[chara].Cards.Values
                        .Count(Utils.MakeAndPredicate(findByLevel, findByStage, findByType))
                        .ToString(CultureInfo.CurrentCulture);
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T10CLEAR[x][yy]
        private class ClearReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T10CLEAR({0})({1})", LevelParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ClearReplacer(Th10Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var chara = (CharaWithTotal)CharaParser.Parse(match.Groups[2].Value);

                    var rankings = parent.allScoreData.ClearData[chara].Rankings[level]
                        .Where(ranking => ranking.DateTime > 0);
                    var stageProgress = (rankings.Count() > 0)
                        ? rankings.Max(ranking => ranking.StageProgress) : StageProgress.None;

                    return (stageProgress == StageProgress.Extra)
                        ? "Not Clear" : stageProgress.ToShortName();
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T10CHARA[xx][y]
        private class CharaReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T10CHARA({0})([1-3])", CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CharaReplacer(Th10Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var chara = CharaWithTotalParser.Parse(match.Groups[1].Value);
                    var type = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                    Func<ClearData, long> getValueByType;
                    Func<long, string> toString;
                    if (type == 1)
                    {
                        getValueByType = (data => data.TotalPlayCount);
                        toString = Utils.ToNumberString;
                    }
                    else if (type == 2)
                    {
                        getValueByType = (data => data.PlayTime);
                        toString = (value => new Time(value).ToString());
                    }
                    else
                    {
                        getValueByType = (data => data.ClearCounts.Values.Sum());
                        toString = Utils.ToNumberString;
                    }

                    Func<AllScoreData, long> getValueByChara;
                    if (chara == CharaWithTotal.Total)
                        getValueByChara = (allData => allData.ClearData.Values
                            .Where(data => data.Chara != chara).Sum(getValueByType));
                    else
                        getValueByChara = (allData => getValueByType(allData.ClearData[chara]));

                    return toString(getValueByChara(parent.allScoreData));
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T10CHARAEX[x][yy][z]
        private class CharaExReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T10CHARAEX({0})({1})([1-3])",
                LevelWithTotalParser.Pattern,
                CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CharaExReplacer(Th10Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelWithTotalParser.Parse(match.Groups[1].Value);
                    var chara = CharaWithTotalParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    Func<ClearData, long> getValueByType;
                    Func<long, string> toString;
                    if (type == 1)
                    {
                        getValueByType = (data => data.TotalPlayCount);
                        toString = Utils.ToNumberString;
                    }
                    else if (type == 2)
                    {
                        getValueByType = (data => data.PlayTime);
                        toString = (value => new Time(value).ToString());
                    }
                    else
                    {
                        if (level == LevelWithTotal.Total)
                            getValueByType = (data => data.ClearCounts.Values.Sum());
                        else
                            getValueByType = (data => data.ClearCounts[(Level)level]);
                        toString = Utils.ToNumberString;
                    }

                    Func<AllScoreData, long> getValueByChara;
                    if (chara == CharaWithTotal.Total)
                        getValueByChara = (allData => allData.ClearData.Values
                            .Where(data => data.Chara != chara).Sum(getValueByType));
                    else
                        getValueByChara = (allData => getValueByType(allData.ClearData[chara]));

                    return toString(getValueByChara(parent.allScoreData));
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T10PRAC[x][yy][z]
        private class PracticeReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T10PRAC({0})({1})({2})", LevelParser.Pattern, CharaParser.Pattern, StageParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public PracticeReplacer(Th10Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var chara = (CharaWithTotal)CharaParser.Parse(match.Groups[2].Value);
                    var stage = StageParser.Parse(match.Groups[3].Value);

                    if (level == Level.Extra)
                        return match.ToString();
                    if (stage == Stage.Extra)
                        return match.ToString();

                    if (parent.allScoreData.ClearData.ContainsKey(chara))
                    {
                        var key = new LevelStagePair(level, stage);
                        var practices = parent.allScoreData.ClearData[chara].Practices;
                        return practices.ContainsKey(key)
                            ? Utils.ToNumberString(practices[key].Score * 10) : "0";
                    }
                    else
                        return "0";
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        private class LevelStagePair : Pair<Level, Stage>
        {
            public LevelStagePair(Level level, Stage stage)
                : base(level, stage)
            {
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Level Level
            {
                get { return this.First; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Stage Stage
            {
                get { return this.Second; }
            }
        }

        private class AllScoreData
        {
            public AllScoreData()
            {
                this.ClearData =
                    new Dictionary<CharaWithTotal, ClearData>(Enum.GetValues(typeof(CharaWithTotal)).Length);
            }

            public Header Header { get; private set; }

            public Dictionary<CharaWithTotal, ClearData> ClearData { get; private set; }

            public Status Status { get; private set; }

            public void Set(Header header)
            {
                this.Header = header;
            }

            public void Set(ClearData data)
            {
                if (!this.ClearData.ContainsKey(data.Chara))
                    this.ClearData.Add(data.Chara, data);
            }

            public void Set(Status status)
            {
                this.Status = status;
            }
        }

        private class Header : IBinaryReadable, IBinaryWritable
        {
            public const string ValidSignature = "TH10";
            public const int SignatureSize = 4;
            public const int Size = SignatureSize + (sizeof(int) * 3) + (sizeof(uint) * 2);

            private uint unknown1;
            private uint unknown2;

            public Header()
            {
                this.Signature = string.Empty;
                this.EncodedAllSize = 0;
                this.EncodedBodySize = 0;
                this.DecodedBodySize = 0;
            }

            public string Signature { get; private set; }

            public int EncodedAllSize { get; private set; }

            public int EncodedBodySize { get; private set; }

            public int DecodedBodySize { get; private set; }

            public bool IsValid
            {
                get
                {
                    return this.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                        && (this.EncodedAllSize - this.EncodedBodySize == Size);
                }
            }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader is null)
                    throw new ArgumentNullException(nameof(reader));

                this.Signature = Encoding.Default.GetString(reader.ReadExactBytes(SignatureSize));
                this.EncodedAllSize = reader.ReadInt32();
                if (this.EncodedAllSize < 0)
                    throw new InvalidDataException(nameof(this.EncodedAllSize));
                this.unknown1 = reader.ReadUInt32();
                this.unknown2 = reader.ReadUInt32();
                this.EncodedBodySize = reader.ReadInt32();
                if (this.EncodedBodySize < 0)
                    throw new InvalidDataException(nameof(this.EncodedBodySize));
                this.DecodedBodySize = reader.ReadInt32();
                if (this.DecodedBodySize < 0)
                    throw new InvalidDataException(nameof(this.DecodedBodySize));
            }

            public void WriteTo(BinaryWriter writer)
            {
                if (writer is null)
                    throw new ArgumentNullException(nameof(writer));

                writer.Write(Encoding.Default.GetBytes(this.Signature));
                writer.Write(this.EncodedAllSize);
                writer.Write(this.unknown1);
                writer.Write(this.unknown2);
                writer.Write(this.EncodedBodySize);
                writer.Write(this.DecodedBodySize);
            }
        }

        private class Chapter : IBinaryReadable
        {
            public const int SignatureSize = 2;

            public Chapter()
            {
                this.Signature = string.Empty;
                this.Version = 0;
                this.Checksum = 0;
                this.Size = 0;
                this.Data = new byte[] { };
            }

            protected Chapter(Chapter chapter)
            {
                if (chapter is null)
                    throw new ArgumentNullException(nameof(chapter));

                this.Signature = chapter.Signature;
                this.Version = chapter.Version;
                this.Checksum = chapter.Checksum;
                this.Size = chapter.Size;
                this.Data = new byte[chapter.Data.Length];
                chapter.Data.CopyTo(this.Data, 0);
            }

            public string Signature { get; private set; }

            public ushort Version { get; private set; }

            public uint Checksum { get; private set; }

            public int Size { get; private set; }

            public bool IsValid
            {
                get
                {
                    var sum = BitConverter.GetBytes(this.Size).Concat(this.Data).Sum(elem => (uint)elem);
                    return sum == this.Checksum;
                }
            }

            protected byte[] Data { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader is null)
                    throw new ArgumentNullException(nameof(reader));

                this.Signature = Encoding.Default.GetString(reader.ReadExactBytes(SignatureSize));
                this.Version = reader.ReadUInt16();
                this.Checksum = reader.ReadUInt32();
                this.Size = reader.ReadInt32();
                this.Data = reader.ReadExactBytes(
                    this.Size - SignatureSize - sizeof(ushort) - sizeof(uint) - sizeof(int));
            }
        }

        private class ClearData : Chapter   // per character
        {
            public const string ValidSignature = "CR";
            public const ushort ValidVersion = 0x0000;
            public const int ValidSize = 0x0000437C;

            public ClearData(Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException("Signature");
                if (this.Version != ValidVersion)
                    throw new InvalidDataException("Version");
                if (this.Size != ValidSize)
                    throw new InvalidDataException("Size");

                var levels = Utils.GetEnumerator<Level>();
                var levelsExceptExtra = levels.Where(lv => lv != Level.Extra);
                var stages = Utils.GetEnumerator<Stage>();
                var stagesExceptExtra = stages.Where(st => st != Stage.Extra);
                var numLevels = levels.Count();
                var numPairs = levelsExceptExtra.Count() * stagesExceptExtra.Count();

                this.Rankings = new Dictionary<Level, ScoreData[]>(numLevels);
                this.ClearCounts = new Dictionary<Level, int>(numLevels);
                this.Practices = new Dictionary<LevelStagePair, Practice>(numPairs);
                this.Cards = new Dictionary<int, SpellCard>(CardTable.Count);

                using (var stream = new MemoryStream(this.Data, false))
                {
                    var reader = new BinaryReader(stream);

                    this.Chara = (CharaWithTotal)reader.ReadInt32();

                    foreach (var level in levels)
                    {
                        if (!this.Rankings.ContainsKey(level))
                            this.Rankings.Add(level, new ScoreData[10]);
                        for (var rank = 0; rank < 10; rank++)
                        {
                            var score = new ScoreData();
                            score.ReadFrom(reader);
                            this.Rankings[level][rank] = score;
                        }
                    }

                    this.TotalPlayCount = reader.ReadInt32();
                    this.PlayTime = reader.ReadInt32();

                    foreach (var level in levels)
                    {
                        var clearCount = reader.ReadInt32();
                        if (!this.ClearCounts.ContainsKey(level))
                            this.ClearCounts.Add(level, clearCount);
                    }

                    foreach (var level in levelsExceptExtra)
                        foreach (var stage in stagesExceptExtra)
                        {
                            var practice = new Practice();
                            practice.ReadFrom(reader);
                            var key = new LevelStagePair(level, stage);
                            if (!this.Practices.ContainsKey(key))
                                this.Practices.Add(key, practice);
                        }

                    for (var number = 0; number < CardTable.Count; number++)
                    {
                        var card = new SpellCard();
                        card.ReadFrom(reader);
                        if (!this.Cards.ContainsKey(card.Id))
                            this.Cards.Add(card.Id, card);
                    }
                }
            }

            public CharaWithTotal Chara { get; private set; }   // size: 4Bytes

            public Dictionary<Level, ScoreData[]> Rankings { get; private set; }

            public int TotalPlayCount { get; private set; }

            public int PlayTime { get; private set; }           // = seconds * 60fps

            public Dictionary<Level, int> ClearCounts { get; private set; }

            public Dictionary<LevelStagePair, Practice> Practices { get; private set; }

            public Dictionary<int, SpellCard> Cards { get; private set; }

            public static bool CanInitialize(Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
            }
        }

        private class Status : Chapter
        {
            public const string ValidSignature = "ST";
            public const ushort ValidVersion = 0x0000;
            public const int ValidSize = 0x00000448;

            public Status(Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException("Signature");
                if (this.Version != ValidVersion)
                    throw new InvalidDataException("Version");
                if (this.Size != ValidSize)
                    throw new InvalidDataException("Size");

                using (var stream = new MemoryStream(this.Data, false))
                {
                    var reader = new BinaryReader(stream);

                    this.LastName = reader.ReadExactBytes(10);
                    reader.ReadExactBytes(0x10);
                    this.BgmFlags = reader.ReadExactBytes(18);
                    reader.ReadExactBytes(0x0410);
                }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] LastName { get; private set; }    // .Length = 10 (The last 2 bytes are always 0x00 ?)

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] BgmFlags { get; private set; }    // .Length = 18

            public static bool CanInitialize(Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
            }
        }

        private class ScoreData : IBinaryReadable
        {
            public uint Score { get; private set; }     // * 10

            public StageProgress StageProgress { get; private set; }    // size: 1Byte

            public byte ContinueCount { get; private set; }

            public byte[] Name { get; private set; }    // .Length = 10 (The last 2 bytes are always 0x00 ?)

            public uint DateTime { get; private set; }  // UNIX time (unit: [s])

            public float SlowRate { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader is null)
                    throw new ArgumentNullException(nameof(reader));

                this.Score = reader.ReadUInt32();
                this.StageProgress = Utils.ToEnum<StageProgress>(reader.ReadByte());
                this.ContinueCount = reader.ReadByte();
                this.Name = reader.ReadExactBytes(10);
                this.DateTime = reader.ReadUInt32();
                this.SlowRate = reader.ReadSingle();
            }
        }

        private class SpellCard : IBinaryReadable
        {
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] Name { get; private set; }    // .Length = 0x80

            public int ClearCount { get; private set; }

            public int TrialCount { get; private set; }

            public int Id { get; private set; }         // 1-based

            public Level Level { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader is null)
                    throw new ArgumentNullException(nameof(reader));

                this.Name = reader.ReadExactBytes(0x80);
                this.ClearCount = reader.ReadInt32();
                this.TrialCount = reader.ReadInt32();
                this.Id = reader.ReadInt32() + 1;
                this.Level = Utils.ToEnum<Level>(reader.ReadInt32());
            }

            public bool HasTried()
            {
                return this.TrialCount > 0;
            }
        }

        private class Practice : IBinaryReadable
        {
            public uint Score { get; private set; }     // * 10

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public uint StageFlag { get; private set; } // 0x00000000: disable, 0x00000101: enable ?

            public void ReadFrom(BinaryReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException("reader");

                this.Score = reader.ReadUInt32();
                this.StageFlag = reader.ReadUInt32();
            }
        }
    }
}
