//-----------------------------------------------------------------------
// <copyright file="Th12Converter.cs" company="None">
//     (c) 2013-2014 IIHOSHI Yoshinori
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

    internal class Th12Converter : ThConverter
    {
        // Thanks to thwiki.info
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly", Justification = "Reviewed.")]
        private static readonly Dictionary<int, CardInfo> CardTable =
            new List<CardInfo>()
            {
                new CardInfo(  1, "棒符「ビジーロッド」",                 Stage.St1,   Level.Hard),
                new CardInfo(  2, "棒符「ビジーロッド」",                 Stage.St1,   Level.Lunatic),
                new CardInfo(  3, "捜符「レアメタルディテクター」",       Stage.St1,   Level.Easy),
                new CardInfo(  4, "捜符「レアメタルディテクター」",       Stage.St1,   Level.Normal),
                new CardInfo(  5, "捜符「ゴールドディテクター」",         Stage.St1,   Level.Hard),
                new CardInfo(  6, "捜符「ゴールドディテクター」",         Stage.St1,   Level.Lunatic),
                new CardInfo(  7, "視符「ナズーリンペンデュラム」",       Stage.St1,   Level.Easy),
                new CardInfo(  8, "視符「ナズーリンペンデュラム」",       Stage.St1,   Level.Normal),
                new CardInfo(  9, "視符「高感度ナズーリンペンデュラム」", Stage.St1,   Level.Hard),
                new CardInfo( 10, "守符「ペンデュラムガード」",           Stage.St1,   Level.Lunatic),
                new CardInfo( 11, "大輪「からかさ後光」",                 Stage.St2,   Level.Easy),
                new CardInfo( 12, "大輪「からかさ後光」",                 Stage.St2,   Level.Normal),
                new CardInfo( 13, "大輪「ハロウフォゴットンワールド」",   Stage.St2,   Level.Hard),
                new CardInfo( 14, "大輪「ハロウフォゴットンワールド」",   Stage.St2,   Level.Lunatic),
                new CardInfo( 15, "傘符「パラソルスターシンフォニー」",   Stage.St2,   Level.Easy),
                new CardInfo( 16, "傘符「パラソルスターシンフォニー」",   Stage.St2,   Level.Normal),
                new CardInfo( 17, "傘符「パラソルスターメモリーズ」",     Stage.St2,   Level.Hard),
                new CardInfo( 18, "傘符「パラソルスターメモリーズ」",     Stage.St2,   Level.Lunatic),
                new CardInfo( 19, "雨符「雨夜の怪談」",                   Stage.St2,   Level.Easy),
                new CardInfo( 20, "雨符「雨夜の怪談」",                   Stage.St2,   Level.Normal),
                new CardInfo( 21, "雨傘「超撥水かさかさお化け」",         Stage.St2,   Level.Hard),
                new CardInfo( 22, "雨傘「超撥水かさかさお化け」",         Stage.St2,   Level.Lunatic),
                new CardInfo( 23, "化符「忘れ傘の夜行列車」",             Stage.St2,   Level.Easy),
                new CardInfo( 24, "化符「忘れ傘の夜行列車」",             Stage.St2,   Level.Normal),
                new CardInfo( 25, "化鉄「置き傘特急ナイトカーニバル」",   Stage.St2,   Level.Hard),
                new CardInfo( 26, "化鉄「置き傘特急ナイトカーニバル」",   Stage.St2,   Level.Lunatic),
                new CardInfo( 27, "鉄拳「問答無用の妖怪拳」",             Stage.St3,   Level.Easy),
                new CardInfo( 28, "鉄拳「問答無用の妖怪拳」",             Stage.St3,   Level.Normal),
                new CardInfo( 29, "神拳「雲上地獄突き」",                 Stage.St3,   Level.Hard),
                new CardInfo( 30, "神拳「天海地獄突き」",                 Stage.St3,   Level.Lunatic),
                new CardInfo( 31, "拳符「天網サンドバッグ」",             Stage.St3,   Level.Easy),
                new CardInfo( 32, "拳符「天網サンドバッグ」",             Stage.St3,   Level.Normal),
                new CardInfo( 33, "連打「雲界クラーケン殴り」",           Stage.St3,   Level.Hard),
                new CardInfo( 34, "連打「キングクラーケン殴り」",         Stage.St3,   Level.Lunatic),
                new CardInfo( 35, "拳打「げんこつスマッシュ」",           Stage.St3,   Level.Easy),
                new CardInfo( 36, "拳打「げんこつスマッシュ」",           Stage.St3,   Level.Normal),
                new CardInfo( 37, "潰滅「天上天下連続フック」",           Stage.St3,   Level.Hard),
                new CardInfo( 38, "潰滅「天上天下連続フック」",           Stage.St3,   Level.Lunatic),
                new CardInfo( 39, "大喝「時代親父大目玉」",               Stage.St3,   Level.Easy),
                new CardInfo( 40, "大喝「時代親父大目玉」",               Stage.St3,   Level.Normal),
                new CardInfo( 41, "忿怒「天変大目玉焼き」",               Stage.St3,   Level.Hard),
                new CardInfo( 42, "忿怒「空前絶後大目玉焼き」",           Stage.St3,   Level.Lunatic),
                new CardInfo( 43, "転覆「道連れアンカー」",               Stage.St4,   Level.Easy),
                new CardInfo( 44, "転覆「道連れアンカー」",               Stage.St4,   Level.Normal),
                new CardInfo( 45, "転覆「沈没アンカー」",                 Stage.St4,   Level.Hard),
                new CardInfo( 46, "転覆「撃沈アンカー」",                 Stage.St4,   Level.Lunatic),
                new CardInfo( 47, "溺符「ディープヴォーテックス」",       Stage.St4,   Level.Easy),
                new CardInfo( 48, "溺符「ディープヴォーテックス」",       Stage.St4,   Level.Normal),
                new CardInfo( 49, "溺符「シンカブルヴォーテックス」",     Stage.St4,   Level.Hard),
                new CardInfo( 50, "溺符「シンカブルヴォーテックス」",     Stage.St4,   Level.Lunatic),
                new CardInfo( 51, "湊符「ファントムシップハーバー」",     Stage.St4,   Level.Easy),
                new CardInfo( 52, "湊符「ファントムシップハーバー」",     Stage.St4,   Level.Normal),
                new CardInfo( 53, "湊符「幽霊船の港」",                   Stage.St4,   Level.Hard),
                new CardInfo( 54, "湊符「幽霊船永久停泊」",               Stage.St4,   Level.Lunatic),
                new CardInfo( 55, "幽霊「シンカーゴースト」",             Stage.St4,   Level.Normal),
                new CardInfo( 56, "幽霊「忍び寄る柄杓」",                 Stage.St4,   Level.Hard),
                new CardInfo( 57, "幽霊「忍び寄る柄杓」",                 Stage.St4,   Level.Lunatic),
                new CardInfo( 58, "宝塔「グレイテストトレジャー」",       Stage.St5,   Level.Easy),
                new CardInfo( 59, "宝塔「グレイテストトレジャー」",       Stage.St5,   Level.Normal),
                new CardInfo( 60, "宝塔「グレイテストトレジャー」",       Stage.St5,   Level.Hard),
                new CardInfo( 61, "宝塔「グレイテストトレジャー」",       Stage.St5,   Level.Lunatic),
                new CardInfo( 62, "宝塔「レイディアントトレジャー」",     Stage.St5,   Level.Easy),
                new CardInfo( 63, "宝塔「レイディアントトレジャー」",     Stage.St5,   Level.Normal),
                new CardInfo( 64, "宝塔「レイディアントトレジャーガン」", Stage.St5,   Level.Hard),
                new CardInfo( 65, "宝塔「レイディアントトレジャーガン」", Stage.St5,   Level.Lunatic),
                new CardInfo( 66, "光符「アブソリュートジャスティス」",   Stage.St5,   Level.Easy),
                new CardInfo( 67, "光符「アブソリュートジャスティス」",   Stage.St5,   Level.Normal),
                new CardInfo( 68, "光符「正義の威光」",                   Stage.St5,   Level.Hard),
                new CardInfo( 69, "光符「正義の威光」",                   Stage.St5,   Level.Lunatic),
                new CardInfo( 70, "法力「至宝の独鈷杵」",                 Stage.St5,   Level.Easy),
                new CardInfo( 71, "法力「至宝の独鈷杵」",                 Stage.St5,   Level.Normal),
                new CardInfo( 72, "法灯「隙間無い法の独鈷杵」",           Stage.St5,   Level.Hard),
                new CardInfo( 73, "法灯「隙間無い法の独鈷杵」",           Stage.St5,   Level.Lunatic),
                new CardInfo( 74, "光符「浄化の魔」",                     Stage.St5,   Level.Easy),
                new CardInfo( 75, "光符「浄化の魔」",                     Stage.St5,   Level.Normal),
                new CardInfo( 76, "光符「浄化の魔」",                     Stage.St5,   Level.Hard),
                new CardInfo( 77, "「コンプリートクラリフィケイション」", Stage.St5,   Level.Lunatic),
                new CardInfo( 78, "魔法「紫雲のオーメン」",               Stage.St6,   Level.Easy),
                new CardInfo( 79, "魔法「紫雲のオーメン」",               Stage.St6,   Level.Normal),
                new CardInfo( 80, "吉兆「紫の雲路」",                     Stage.St6,   Level.Hard),
                new CardInfo( 81, "吉兆「極楽の紫の雲路」",               Stage.St6,   Level.Lunatic),
                new CardInfo( 82, "魔法「魔界蝶の妖香」",                 Stage.St6,   Level.Easy),
                new CardInfo( 83, "魔法「魔界蝶の妖香」",                 Stage.St6,   Level.Normal),
                new CardInfo( 84, "魔法「マジックバタフライ」",           Stage.St6,   Level.Hard),
                new CardInfo( 85, "魔法「マジックバタフライ」",           Stage.St6,   Level.Lunatic),
                new CardInfo( 86, "光魔「スターメイルシュトロム」",       Stage.St6,   Level.Easy),
                new CardInfo( 87, "光魔「スターメイルシュトロム」",       Stage.St6,   Level.Normal),
                new CardInfo( 88, "光魔「魔法銀河系」",                   Stage.St6,   Level.Hard),
                new CardInfo( 89, "光魔「魔法銀河系」",                   Stage.St6,   Level.Lunatic),
                new CardInfo( 90, "大魔法「魔神復誦」",                   Stage.St6,   Level.Easy),
                new CardInfo( 91, "大魔法「魔神復誦」",                   Stage.St6,   Level.Normal),
                new CardInfo( 92, "大魔法「魔神復誦」",                   Stage.St6,   Level.Hard),
                new CardInfo( 93, "大魔法「魔神復誦」",                   Stage.St6,   Level.Lunatic),
                new CardInfo( 94, "「聖尼公のエア巻物」",                 Stage.St6,   Level.Normal),
                new CardInfo( 95, "「聖尼公のエア巻物」",                 Stage.St6,   Level.Hard),
                new CardInfo( 96, "超人「聖白蓮」",                       Stage.St6,   Level.Lunatic),
                new CardInfo( 97, "飛鉢「フライングファンタスティカ」",   Stage.St6,   Level.Easy),
                new CardInfo( 98, "飛鉢「フライングファンタスティカ」",   Stage.St6,   Level.Normal),
                new CardInfo( 99, "飛鉢「伝説の飛空円盤」",               Stage.St6,   Level.Hard),
                new CardInfo(100, "飛鉢「伝説の飛空円盤」",               Stage.St6,   Level.Lunatic),
                new CardInfo(101, "傘符「大粒の涙雨」",                   Stage.Extra, Level.Extra),
                new CardInfo(102, "驚雨「ゲリラ台風」",                   Stage.Extra, Level.Extra),
                new CardInfo(103, "後光「からかさ驚きフラッシュ」",       Stage.Extra, Level.Extra),
                new CardInfo(104, "妖雲「平安のダーククラウド」",         Stage.Extra, Level.Extra),
                new CardInfo(105, "正体不明「忿怒のレッドＵＦＯ襲来」",   Stage.Extra, Level.Extra),
                new CardInfo(106, "鵺符「鵺的スネークショー」",           Stage.Extra, Level.Extra),
                new CardInfo(107, "正体不明「哀愁のブルーＵＦＯ襲来」",   Stage.Extra, Level.Extra),
                new CardInfo(108, "鵺符「弾幕キメラ」",                   Stage.Extra, Level.Extra),
                new CardInfo(109, "正体不明「義心のグリーンＵＦＯ襲来」", Stage.Extra, Level.Extra),
                new CardInfo(110, "鵺符「アンディファインドダークネス」", Stage.Extra, Level.Extra),
                new CardInfo(111, "正体不明「恐怖の虹色ＵＦＯ襲来」",     Stage.Extra, Level.Extra),
                new CardInfo(112, "「平安京の悪夢」",                     Stage.Extra, Level.Extra),
                new CardInfo(113, "恨弓「源三位頼政の弓」",               Stage.Extra, Level.Extra)
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
            [EnumAltName("MA")] MarisaA,
            [EnumAltName("MB")] MarisaB,
            [EnumAltName("SA")] SanaeA,
            [EnumAltName("SB")] SanaeB
        }

        public enum CharaWithTotal
        {
            [EnumAltName("RA")] ReimuA,
            [EnumAltName("RB")] ReimuB,
            [EnumAltName("MA")] MarisaA,
            [EnumAltName("MB")] MarisaB,
            [EnumAltName("SA")] SanaeA,
            [EnumAltName("SB")] SanaeB,
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
            get { return "1.00b"; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th12decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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
                Action<AllScoreData, Chapter> setChapter;
                while (true)
                {
                    chapter.ReadFrom(reader);
                    if (dictionary.TryGetValue(chapter.Signature, out setChapter))
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

        // %T12SCR[w][xx][y][z]
        private class ScoreReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T12SCR({0})({1})(\d)([1-5])", LevelParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ScoreReplacer(Th12Converter parent)
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

        // %T12C[xxx][yy][z]
        private class CareerReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T12C(\d{{3}})({0})([12])", CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CareerReplacer(Th12Converter parent)
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
                        SpellCard card;
                        if (cards.TryGetValue(number, out card))
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

        // %T12CARD[xxx][y]
        private class CardReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T12CARD(\d{3})([NR])";

            private readonly MatchEvaluator evaluator;

            public CardReplacer(Th12Converter parent, bool hideUntriedCards)
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
                                SpellCard card;
                                if (!cards.TryGetValue(number, out card) || !card.HasTried())
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

        // %T12CRG[w][xx][y][z]
        private class CollectRateReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T12CRG({0})({1})({2})([12])",
                LevelWithTotalParser.Pattern,
                CharaWithTotalParser.Pattern,
                StageWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CollectRateReplacer(Th12Converter parent)
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

        // %T12CLEAR[x][yy]
        private class ClearReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T12CLEAR({0})({1})", LevelParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ClearReplacer(Th12Converter parent)
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

        // %T12CHARA[xx][y]
        private class CharaReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T12CHARA({0})([1-3])", CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CharaReplacer(Th12Converter parent)
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

        // %T12CHARAEX[x][yy][z]
        private class CharaExReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T12CHARAEX({0})({1})([1-3])", LevelWithTotalParser.Pattern, CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CharaExReplacer(Th12Converter parent)
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

        // %T12PRAC[x][yy][z]
        private class PracticeReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T12PRAC({0})({1})({2})", LevelParser.Pattern, CharaParser.Pattern, StageParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public PracticeReplacer(Th12Converter parent)
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
            public const string ValidSignature = "TH21";
            public const int SignatureSize = 4;
            public const int Size = SignatureSize + (sizeof(int) * 3) + (sizeof(uint) * 2);

            private uint unknown1;
            private uint unknown2;

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
                if (reader == null)
                    throw new ArgumentNullException("reader");

                this.Signature = Encoding.Default.GetString(reader.ReadBytes(SignatureSize));
                this.EncodedAllSize = reader.ReadInt32();
                this.unknown1 = reader.ReadUInt32();
                this.unknown2 = reader.ReadUInt32();
                this.EncodedBodySize = reader.ReadInt32();
                this.DecodedBodySize = reader.ReadInt32();
            }

            public void WriteTo(BinaryWriter writer)
            {
                if (writer == null)
                    throw new ArgumentNullException("writer");

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
                if (reader == null)
                    throw new ArgumentNullException("reader");

                this.Signature = Encoding.Default.GetString(reader.ReadBytes(SignatureSize));
                this.Version = reader.ReadUInt16();
                this.Checksum = reader.ReadUInt32();
                this.Size = reader.ReadInt32();
                this.Data = reader.ReadBytes(
                    this.Size - SignatureSize - sizeof(ushort) - sizeof(uint) - sizeof(int));
            }
        }

        private class ClearData : Chapter   // per character
        {
            public const string ValidSignature = "CR";
            public const ushort ValidVersion = 0x0002;
            public const int ValidSize = 0x000045F4;

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
            public const ushort ValidVersion = 0x0002;
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

                    this.LastName = reader.ReadBytes(10);
                    reader.ReadBytes(0x10);
                    this.BgmFlags = reader.ReadBytes(17);
                    reader.ReadBytes(0x0411);
                }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] LastName { get; private set; }    // .Length = 10 (The last 2 bytes are always 0x00 ?)

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] BgmFlags { get; private set; }    // .Length = 17

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

            public float SlowRate { get; private set; } // really...?

            public void ReadFrom(BinaryReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException("reader");

                this.Score = reader.ReadUInt32();
                this.StageProgress = (StageProgress)reader.ReadByte();
                this.ContinueCount = reader.ReadByte();
                this.Name = reader.ReadBytes(10);
                this.DateTime = reader.ReadUInt32();
                this.SlowRate = reader.ReadSingle();
                reader.ReadUInt32();
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
                if (reader == null)
                    throw new ArgumentNullException("reader");

                this.Name = reader.ReadBytes(0x80);
                this.ClearCount = reader.ReadInt32();
                this.TrialCount = reader.ReadInt32();
                this.Id = reader.ReadInt32() + 1;
                this.Level = (Level)reader.ReadInt32();
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
