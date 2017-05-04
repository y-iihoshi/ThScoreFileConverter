//-----------------------------------------------------------------------
// <copyright file="Th15Converter.cs" company="None">
//     (c) 2015 IIHOSHI Yoshinori
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

    internal class Th15Converter : ThConverter
    {
        // Thanks to thwiki.info
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly", Justification = "Reviewed.")]
        private static readonly Dictionary<int, CardInfo> CardTable =
            new List<CardInfo>()
            {
                new CardInfo(  1, "凶弾「スピードストライク」",           Stage.St1,   Level.Hard),
                new CardInfo(  2, "凶弾「スピードストライク」",           Stage.St1,   Level.Lunatic),
                new CardInfo(  3, "弾符「イーグルシューティング」",       Stage.St1,   Level.Easy),
                new CardInfo(  4, "弾符「イーグルシューティング」",       Stage.St1,   Level.Normal),
                new CardInfo(  5, "弾符「イーグルシューティング」",       Stage.St1,   Level.Hard),
                new CardInfo(  6, "弾符「鷹は撃ち抜いた」",               Stage.St1,   Level.Lunatic),
                new CardInfo(  7, "銃符「ルナティックガン」",             Stage.St1,   Level.Easy),
                new CardInfo(  8, "銃符「ルナティックガン」",             Stage.St1,   Level.Normal),
                new CardInfo(  9, "銃符「ルナティックガン」",             Stage.St1,   Level.Hard),
                new CardInfo( 10, "銃符「ルナティックガン」",             Stage.St1,   Level.Lunatic),
                new CardInfo( 11, "兎符「ストロベリーダンゴ」",           Stage.St2,   Level.Easy),
                new CardInfo( 12, "兎符「ストロベリーダンゴ」",           Stage.St2,   Level.Normal),
                new CardInfo( 13, "兎符「ベリーベリーダンゴ」",           Stage.St2,   Level.Hard),
                new CardInfo( 14, "兎符「ベリーベリーダンゴ」",           Stage.St2,   Level.Lunatic),
                new CardInfo( 15, "兎符「ダンゴインフリューエンス」",     Stage.St2,   Level.Easy),
                new CardInfo( 16, "兎符「ダンゴインフリューエンス」",     Stage.St2,   Level.Normal),
                new CardInfo( 17, "兎符「ダンゴインフリューエンス」",     Stage.St2,   Level.Hard),
                new CardInfo( 18, "兎符「ダンゴインフリューエンス」",     Stage.St2,   Level.Lunatic),
                new CardInfo( 19, "月見「セプテンバーフルムーン」",       Stage.St2,   Level.Easy),
                new CardInfo( 20, "月見「セプテンバーフルムーン」",       Stage.St2,   Level.Normal),
                new CardInfo( 21, "月見「セプテンバーフルムーン」",       Stage.St2,   Level.Hard),
                new CardInfo( 22, "月見酒「ルナティックセプテンバー」",   Stage.St2,   Level.Lunatic),
                new CardInfo( 23, "夢符「緋色の悪夢」",                   Stage.St3,   Level.Easy),
                new CardInfo( 24, "夢符「緋色の悪夢」",                   Stage.St3,   Level.Normal),
                new CardInfo( 25, "夢符「緋色の圧迫悪夢」",               Stage.St3,   Level.Hard),
                new CardInfo( 26, "夢符「緋色の圧迫悪夢」",               Stage.St3,   Level.Lunatic),
                new CardInfo( 27, "夢符「藍色の愁夢」",                   Stage.St3,   Level.Easy),
                new CardInfo( 28, "夢符「藍色の愁夢」",                   Stage.St3,   Level.Normal),
                new CardInfo( 29, "夢符「藍色の愁三重夢」",               Stage.St3,   Level.Hard),
                new CardInfo( 30, "夢符「愁永遠の夢」",                   Stage.St3,   Level.Lunatic),
                new CardInfo( 31, "夢符「刈安色の迷夢」",                 Stage.St3,   Level.Easy),
                new CardInfo( 32, "夢符「刈安色の迷夢」",                 Stage.St3,   Level.Normal),
                new CardInfo( 33, "夢符「刈安色の錯綜迷夢」",             Stage.St3,   Level.Hard),
                new CardInfo( 34, "夢符「刈安色の錯綜迷夢」",             Stage.St3,   Level.Lunatic),
                new CardInfo( 35, "夢符「ドリームキャッチャー」",         Stage.St3,   Level.Easy),
                new CardInfo( 36, "夢符「ドリームキャッチャー」",         Stage.St3,   Level.Normal),
                new CardInfo( 37, "夢符「蒼色のドリームキャッチャー」",   Stage.St3,   Level.Hard),
                new CardInfo( 38, "夢符「夢我夢中」",                     Stage.St3,   Level.Lunatic),
                new CardInfo( 39, "月符「紺色の狂夢」",                   Stage.St3,   Level.Easy),
                new CardInfo( 40, "月符「紺色の狂夢」",                   Stage.St3,   Level.Normal),
                new CardInfo( 41, "月符「紺色の狂夢」",                   Stage.St3,   Level.Hard),
                new CardInfo( 42, "月符「紺色の狂夢」",                   Stage.St3,   Level.Lunatic),
                new CardInfo( 43, "玉符「烏合の呪」",                     Stage.St4,   Level.Easy),
                new CardInfo( 44, "玉符「烏合の呪」",                     Stage.St4,   Level.Normal),
                new CardInfo( 45, "玉符「烏合の逆呪」",                   Stage.St4,   Level.Hard),
                new CardInfo( 46, "玉符「烏合の二重呪」",                 Stage.St4,   Level.Lunatic),
                new CardInfo( 47, "玉符「穢身探知型機雷」",               Stage.St4,   Level.Easy),
                new CardInfo( 48, "玉符「穢身探知型機雷」",               Stage.St4,   Level.Normal),
                new CardInfo( 49, "玉符「穢身探知型機雷 改」",            Stage.St4,   Level.Hard),
                new CardInfo( 50, "玉符「穢身探知型機雷 改」",            Stage.St4,   Level.Lunatic),
                new CardInfo( 51, "玉符「神々の弾冠」",                   Stage.St4,   Level.Easy),
                new CardInfo( 52, "玉符「神々の弾冠」",                   Stage.St4,   Level.Normal),
                new CardInfo( 53, "玉符「神々の光り輝く弾冠」",           Stage.St4,   Level.Hard),
                new CardInfo( 54, "玉符「神々の光り輝く弾冠」",           Stage.St4,   Level.Lunatic),
                new CardInfo( 55, "「片翼の白鷺」",                       Stage.St4,   Level.Easy),
                new CardInfo( 56, "「片翼の白鷺」",                       Stage.St4,   Level.Normal),
                new CardInfo( 57, "「片翼の白鷺」",                       Stage.St4,   Level.Hard),
                new CardInfo( 58, "「片翼の白鷺」",                       Stage.St4,   Level.Lunatic),
                new CardInfo( 59, "獄符「ヘルエクリプス」",               Stage.St5,   Level.Easy),
                new CardInfo( 60, "獄符「ヘルエクリプス」",               Stage.St5,   Level.Normal),
                new CardInfo( 61, "獄符「地獄の蝕」",                     Stage.St5,   Level.Hard),
                new CardInfo( 62, "獄符「地獄の蝕」",                     Stage.St5,   Level.Lunatic),
                new CardInfo( 63, "獄符「フラッシュアンドストライプ」",   Stage.St5,   Level.Easy),
                new CardInfo( 64, "獄符「フラッシュアンドストライプ」",   Stage.St5,   Level.Normal),
                new CardInfo( 65, "獄符「スターアンドストライプ」",       Stage.St5,   Level.Hard),
                new CardInfo( 66, "獄符「スターアンドストライプ」",       Stage.St5,   Level.Lunatic),
                new CardInfo( 67, "獄炎「グレイズインフェルノ」",         Stage.St5,   Level.Easy),
                new CardInfo( 68, "獄炎「グレイズインフェルノ」",         Stage.St5,   Level.Normal),
                new CardInfo( 69, "獄炎「グレイズインフェルノ」",         Stage.St5,   Level.Hard),
                new CardInfo( 70, "獄炎「かすりの獄意」",                 Stage.St5,   Level.Lunatic),
                new CardInfo( 71, "地獄「ストライプドアビス」",           Stage.St5,   Level.Easy),
                new CardInfo( 72, "地獄「ストライプドアビス」",           Stage.St5,   Level.Normal),
                new CardInfo( 73, "地獄「ストライプドアビス」",           Stage.St5,   Level.Hard),
                new CardInfo( 74, "地獄「ストライプドアビス」",           Stage.St5,   Level.Lunatic),
                new CardInfo( 75, "「フェイクアポロ」",                   Stage.St5,   Level.Easy),
                new CardInfo( 76, "「フェイクアポロ」",                   Stage.St5,   Level.Normal),
                new CardInfo( 77, "「アポロ捏造説」",                     Stage.St5,   Level.Hard),
                new CardInfo( 78, "「アポロ捏造説」",                     Stage.St5,   Level.Lunatic),
                new CardInfo( 79, "「掌の純光」",                         Stage.St6,   Level.Easy),
                new CardInfo( 80, "「掌の純光」",                         Stage.St6,   Level.Normal),
                new CardInfo( 81, "「掌の純光」",                         Stage.St6,   Level.Hard),
                new CardInfo( 82, "「掌の純光」",                         Stage.St6,   Level.Lunatic),
                new CardInfo( 83, "「殺意の百合」",                       Stage.St6,   Level.Easy),
                new CardInfo( 84, "「殺意の百合」",                       Stage.St6,   Level.Normal),
                new CardInfo( 85, "「殺意の百合」",                       Stage.St6,   Level.Hard),
                new CardInfo( 86, "「殺意の百合」",                       Stage.St6,   Level.Lunatic),
                new CardInfo( 87, "「原始の神霊界」",                     Stage.St6,   Level.Easy),
                new CardInfo( 88, "「原始の神霊界」",                     Stage.St6,   Level.Normal),
                new CardInfo( 89, "「現代の神霊界」",                     Stage.St6,   Level.Hard),
                new CardInfo( 90, "「現代の神霊界」",                     Stage.St6,   Level.Lunatic),
                new CardInfo( 91, "「震え凍える星」",                     Stage.St6,   Level.Easy),
                new CardInfo( 92, "「震え凍える星」",                     Stage.St6,   Level.Normal),
                new CardInfo( 93, "「震え凍える星」",                     Stage.St6,   Level.Hard),
                new CardInfo( 94, "「震え凍える星」",                     Stage.St6,   Level.Lunatic),
                new CardInfo( 95, "「純粋なる狂気」",                     Stage.St6,   Level.Easy),
                new CardInfo( 96, "「純粋なる狂気」",                     Stage.St6,   Level.Normal),
                new CardInfo( 97, "「純粋なる狂気」",                     Stage.St6,   Level.Hard),
                new CardInfo( 98, "「純粋なる狂気」",                     Stage.St6,   Level.Lunatic),
                new CardInfo( 99, "「溢れ出る瑕穢」",                     Stage.St6,   Level.Easy),
                new CardInfo(100, "「溢れ出る瑕穢」",                     Stage.St6,   Level.Normal),
                new CardInfo(101, "「溢れ出る瑕穢」",                     Stage.St6,   Level.Hard),
                new CardInfo(102, "「地上穢の純化」",                     Stage.St6,   Level.Lunatic),
                new CardInfo(103, "純符「ピュアリーバレットヘル」",       Stage.St6,   Level.Easy),
                new CardInfo(104, "純符「ピュアリーバレットヘル」",       Stage.St6,   Level.Normal),
                new CardInfo(105, "純符「純粋な弾幕地獄」",               Stage.St6,   Level.Hard),
                new CardInfo(106, "純符「純粋な弾幕地獄」",               Stage.St6,   Level.Lunatic),
                new CardInfo(107, "胡蝶「バタフライサプランテーション」", Stage.Extra, Level.Extra),
                new CardInfo(108, "超特急「ドリームエクスプレス」",       Stage.Extra, Level.Extra),
                new CardInfo(109, "這夢「クリーピングバレット」",         Stage.Extra, Level.Extra),
                new CardInfo(110, "異界「逢魔ガ刻」",                     Stage.Extra, Level.Extra),
                new CardInfo(111, "地球「邪穢在身」",                     Stage.Extra, Level.Extra),
                new CardInfo(112, "月「アポロ反射鏡」",                   Stage.Extra, Level.Extra),
                new CardInfo(113, "「袋の鼠を追い詰める為の単純な弾幕」", Stage.Extra, Level.Extra),
                new CardInfo(114, "異界「地獄のノンイデアル弾幕」",       Stage.Extra, Level.Extra),
                new CardInfo(115, "地球「地獄に降る雨」",                 Stage.Extra, Level.Extra),
                new CardInfo(116, "月「ルナティックインパクト」",         Stage.Extra, Level.Extra),
                new CardInfo(117, "「人を殺める為の純粋な弾幕」",         Stage.Extra, Level.Extra),
                new CardInfo(118, "「トリニタリアンラプソディ」",         Stage.Extra, Level.Extra),
                new CardInfo(119, "「最初で最後の無名の弾幕」",           Stage.Extra, Level.Extra)
            }.ToDictionary(card => card.Id);

        private static readonly EnumShortNameParser<GameMode> GameModeParser =
            new EnumShortNameParser<GameMode>();

        private static readonly EnumShortNameParser<Chara> CharaParser =
            new EnumShortNameParser<Chara>();

        private static readonly EnumShortNameParser<CharaWithTotal> CharaWithTotalParser =
            new EnumShortNameParser<CharaWithTotal>();

        private AllScoreData allScoreData = null;

        public enum GameMode
        {
            [EnumAltName("P")] Pointdevice,
            [EnumAltName("L")] Legacy
        }

        public enum Chara
        {
            [EnumAltName("RM")] Reimu,
            [EnumAltName("MR")] Marisa,
            [EnumAltName("SN")] Sanae,
            [EnumAltName("RS")] Reisen
        }

        public enum CharaWithTotal
        {
            [EnumAltName("RM")] Reimu,
            [EnumAltName("MR")] Marisa,
            [EnumAltName("SN")] Sanae,
            [EnumAltName("RS")] Reisen,
            [EnumAltName("TL")] Total
        }

        public enum StagePractice
        {
            [EnumAltName("1")] St1,
            [EnumAltName("2")] St2,
            [EnumAltName("3")] St3,
            [EnumAltName("4")] St4,
            [EnumAltName("5")] St5,
            [EnumAltName("6")] St6,
            [EnumAltName("X")] Extra,
            [EnumAltName("-")] NotUsed
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
            [EnumAltName("All Clear")]   Clear,
            [EnumAltName("Extra Clear")] ExtraClear
        }

        public override string SupportedVersions
        {
            get { return "1.00b"; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th15decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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

        // %T15SCR[v][w][xx][y][z]
        private class ScoreReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T15SCR({0})({1})({2})(\d)([1-6])",
                GameModeParser.Pattern,
                LevelParser.Pattern,
                CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ScoreReplacer(Th15Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var mode = GameModeParser.Parse(match.Groups[1].Value);
                    var level = (LevelWithTotal)LevelParser.Parse(match.Groups[2].Value);
                    var chara = (CharaWithTotal)CharaParser.Parse(match.Groups[3].Value);
                    var rank = Utils.ToZeroBased(
                        int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture));
                    var type = int.Parse(match.Groups[5].Value, CultureInfo.InvariantCulture);

#if false   // FIXME
                    if (level == LevelWithTotal.Extra)
                        mode = GameMode.Pointdevice;
#endif

                    var ranking = parent.allScoreData.ClearData[chara].Data1[mode].Rankings[level][rank];
                    switch (type)
                    {
                        case 1:     // name
                            return Encoding.Default.GetString(ranking.Name).Split('\0')[0];
                        case 2:     // score
                            return Utils.ToNumberString((ranking.Score * 10) + ranking.ContinueCount);
                        case 3:     // stage
                            if (ranking.DateTime > 0)
                            {
                                if (ranking.StageProgress == StageProgress.Extra)
                                    return "Not Clear";
                                else if (ranking.StageProgress == StageProgress.ExtraClear)
                                    return StageProgress.Clear.ToShortName();
                                else
                                    return ranking.StageProgress.ToShortName();
                            }
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
                        case 6:     // retry
                            if (ranking.DateTime > 0)
                                return Utils.ToNumberString(ranking.RetryCount);
                            else
                                return "-----";
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

        // %T15C[w][xxx][yy][z]
        private class CareerReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T15C({0})(\d{{3}})({1})([12])", GameModeParser.Pattern, CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CareerReplacer(Th15Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var mode = GameModeParser.Parse(match.Groups[1].Value);
                    var number = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    var chara = CharaWithTotalParser.Parse(match.Groups[3].Value);
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    Func<SpellCard, int> getCount;
                    if (type == 1)
                        getCount = (card => card.ClearCount);
                    else
                        getCount = (card => card.TrialCount);

                    var cards = parent.allScoreData.ClearData[chara].Data1[mode].Cards;
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

        // %T15CARD[xxx][y]
        private class CardReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T15CARD(\d{3})([NR])";

            private readonly MatchEvaluator evaluator;

            public CardReplacer(Th15Converter parent, bool hideUntriedCards)
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
                                var tried = parent.allScoreData.ClearData[CharaWithTotal.Total].Data1.Any(
                                    data =>
                                    {
                                        SpellCard card;
                                        return data.Value.Cards.TryGetValue(number, out card)
                                            && card.HasTried();
                                    });
                                if (!tried)
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

        // %T15CRG[v][w][xx][y][z]
        private class CollectRateReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T15CRG({0})({1})({2})({3})([12])",
                GameModeParser.Pattern,
                LevelWithTotalParser.Pattern,
                CharaWithTotalParser.Pattern,
                StageWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CollectRateReplacer(Th15Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var mode = GameModeParser.Parse(match.Groups[1].Value);
                    var level = LevelWithTotalParser.Parse(match.Groups[2].Value);
                    var chara = CharaWithTotalParser.Parse(match.Groups[3].Value);
                    var stage = StageWithTotalParser.Parse(match.Groups[4].Value);
                    var type = int.Parse(match.Groups[5].Value, CultureInfo.InvariantCulture);

                    if (stage == StageWithTotal.Extra)
                        return match.ToString();

#if false   // FIXME
                    if (level == LevelWithTotal.Extra)
                        mode = GameMode.Pointdevice;
#endif

                    Func<SpellCard, bool> findByType;
                    if (type == 1)
                        findByType = (card => card.ClearCount > 0);
                    else
                        findByType = (card => card.TrialCount > 0);

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

                    return parent.allScoreData.ClearData[chara].Data1[mode].Cards.Values
                        .Count(Utils.MakeAndPredicate(findByType, findByLevel, findByStage))
                        .ToString(CultureInfo.CurrentCulture);
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T15CLEAR[x][yy]
        private class ClearReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T15CLEAR({0})({1})({2})", GameModeParser.Pattern, LevelParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ClearReplacer(Th15Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var mode = GameModeParser.Parse(match.Groups[1].Value);
                    var level = (LevelWithTotal)LevelParser.Parse(match.Groups[2].Value);
                    var chara = (CharaWithTotal)CharaParser.Parse(match.Groups[3].Value);

#if false   // FIXME
                    if (level == LevelWithTotal.Extra)
                        mode = GameMode.Pointdevice;
#endif

                    var rankings = parent.allScoreData.ClearData[chara].Data1[mode].Rankings[level]
                        .Where(ranking => ranking.DateTime > 0);
                    var stageProgress = (rankings.Count() > 0)
                        ? rankings.Max(ranking => ranking.StageProgress) : StageProgress.None;

                    if (stageProgress == StageProgress.Extra)
                        return "Not Clear";
                    else if (stageProgress == StageProgress.ExtraClear)
                        return StageProgress.Clear.ToShortName();
                    else
                        return stageProgress.ToShortName();
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T15CHARA[x][yy][z]
        private class CharaReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T15CHARA({0})({1})([1-3])", GameModeParser.Pattern, CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CharaReplacer(Th15Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var mode = GameModeParser.Parse(match.Groups[1].Value);
                    var chara = CharaWithTotalParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    Func<ClearData, long> getValueByType;
                    Func<long, string> toString;
                    if (type == 1)
                    {
                        getValueByType = (data => data.Data1[mode].TotalPlayCount);
                        toString = Utils.ToNumberString;
                    }
                    else if (type == 2)
                    {
                        getValueByType = (data => data.Data1[mode].PlayTime);
                        toString = (value => new Time(value * 10, false).ToString());
                    }
                    else
                    {
                        getValueByType = (data => data.Data1[mode].ClearCounts.Values.Sum());
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

        // %T15CHARAEX[w][x][yy][z]
        private class CharaExReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T15CHARAEX({0})({1})({2})([1-3])",
                GameModeParser.Pattern,
                LevelWithTotalParser.Pattern,
                CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CharaExReplacer(Th15Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var mode = GameModeParser.Parse(match.Groups[1].Value);
                    var level = LevelWithTotalParser.Parse(match.Groups[2].Value);
                    var chara = CharaWithTotalParser.Parse(match.Groups[3].Value);
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    Func<ClearData, long> getValueByType;
                    Func<long, string> toString;
                    if (type == 1)
                    {
                        getValueByType = (data => data.Data1[mode].TotalPlayCount);
                        toString = Utils.ToNumberString;
                    }
                    else if (type == 2)
                    {
                        getValueByType = (data => data.Data1[mode].PlayTime);
                        toString = (value => new Time(value * 10, false).ToString());
                    }
                    else
                    {
                        if (level == LevelWithTotal.Total)
                            getValueByType = (data => data.Data1[mode].ClearCounts.Values.Sum());
                        else
                            getValueByType = (data => data.Data1[mode].ClearCounts[level]);
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

        // %T15PRAC[x][yy][z]
        private class PracticeReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T15PRAC({0})({1})({2})", LevelParser.Pattern, CharaParser.Pattern, StageParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public PracticeReplacer(Th15Converter parent)
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

        private class LevelStagePair : Pair<Level, StagePractice>
        {
            public LevelStagePair(Level level, StagePractice stage)
                : base(level, stage)
            {
            }

            public LevelStagePair(Level level, Stage stage)
                : base(level, (StagePractice)stage)
            {
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Level Level
            {
                get { return this.First; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public StagePractice Stage
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
            public const string ValidSignature = "TH51";
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
            public const ushort ValidVersion = 0x0001;
            public const int ValidSize = 0x0000A4A0;

            public ClearData(Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException("Signature");
                if (this.Version != ValidVersion)
                    throw new InvalidDataException("Version");
                if (this.Size != ValidSize)
                    throw new InvalidDataException("Size");

                var modes = Utils.GetEnumerator<GameMode>();
                var levels = Utils.GetEnumerator<Level>();
                var stages = Utils.GetEnumerator<StagePractice>();

                this.Data1 = new Dictionary<GameMode, ClearDataPerGameMode>(modes.Count());
                this.Practices = new Dictionary<LevelStagePair, Practice>(levels.Count() * stages.Count());

                using (var stream = new MemoryStream(this.Data, false))
                {
                    var reader = new BinaryReader(stream);

                    this.Chara = (CharaWithTotal)reader.ReadInt32();

                    foreach (var mode in modes)
                    {
                        var data = new ClearDataPerGameMode();
                        data.ReadFrom(reader);
                        if (!this.Data1.ContainsKey(mode))
                            this.Data1.Add(mode, data);
                    }

                    foreach (var level in levels)
                        foreach (var stage in stages)
                        {
                            var practice = new Practice();
                            practice.ReadFrom(reader);
                            var key = new LevelStagePair(level, stage);
                            if (!this.Practices.ContainsKey(key))
                                this.Practices.Add(key, practice);
                        }
                }
            }

            public CharaWithTotal Chara { get; private set; }   // size: 4Bytes

            public Dictionary<GameMode, ClearDataPerGameMode> Data1 { get; private set; }

            public Dictionary<LevelStagePair, Practice> Practices { get; private set; }

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
            public const ushort ValidVersion = 0x0001;
            public const int ValidSize = 0x0000042C;

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
                    reader.ReadBytes(0x11);
                    this.TotalPlayTime = reader.ReadInt32();
                    reader.ReadBytes(0x03E0);
                }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] LastName { get; private set; }    // .Length = 10 (The last 2 bytes are always 0x00 ?)

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] BgmFlags { get; private set; }    // .Length = 17

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int TotalPlayTime { get; private set; }  // unit: 10ms

            public static bool CanInitialize(Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
            }
        }

        private class ClearDataPerGameMode : IBinaryReadable
        {
            public Dictionary<LevelWithTotal, ScoreData[]> Rankings { get; private set; }

            public int TotalPlayCount { get; private set; }

            public int PlayTime { get; private set; }           // unit: 10ms

            public Dictionary<LevelWithTotal, int> ClearCounts { get; private set; }

            public Dictionary<LevelWithTotal, int> ClearFlags { get; private set; }     // Really...?

            public Dictionary<int, SpellCard> Cards { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException("reader");

                var levelsWithTotal = Utils.GetEnumerator<LevelWithTotal>();
                var numLevelsWithTotal = levelsWithTotal.Count();

                this.Rankings = new Dictionary<LevelWithTotal, ScoreData[]>(numLevelsWithTotal);
                this.ClearCounts = new Dictionary<LevelWithTotal, int>(numLevelsWithTotal);
                this.ClearFlags = new Dictionary<LevelWithTotal, int>(numLevelsWithTotal);
                this.Cards = new Dictionary<int, SpellCard>(CardTable.Count);

                foreach (var level in levelsWithTotal)
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

                reader.ReadBytes(0x140);

                for (var number = 0; number < CardTable.Count; number++)
                {
                    var card = new SpellCard();
                    card.ReadFrom(reader);
                    if (!this.Cards.ContainsKey(card.Id))
                        this.Cards.Add(card.Id, card);
                }

                this.TotalPlayCount = reader.ReadInt32();
                this.PlayTime = reader.ReadInt32();

                reader.ReadUInt32();
                foreach (var level in levelsWithTotal)
                {
                    var clearCount = reader.ReadInt32();
                    if (!this.ClearCounts.ContainsKey(level))
                        this.ClearCounts.Add(level, clearCount);
                }

                reader.ReadUInt32();
                foreach (var level in levelsWithTotal)
                {
                    var clearFlag = reader.ReadInt32();
                    if (!this.ClearFlags.ContainsKey(level))
                        this.ClearFlags.Add(level, clearFlag);
                }

                reader.ReadUInt32();
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

            public uint RetryCount { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException("reader");

                this.Score = reader.ReadUInt32();
                this.StageProgress = (StageProgress)reader.ReadByte();
                this.ContinueCount = reader.ReadByte();
                this.Name = reader.ReadBytes(10);
                this.DateTime = reader.ReadUInt32();
                reader.ReadUInt32();
                this.SlowRate = reader.ReadSingle();
                this.RetryCount = reader.ReadUInt32();
            }
        }

        private class SpellCard : IBinaryReadable
        {
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] Name { get; private set; }            // .Length = 0x80

            public int ClearCount { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int PracticeClearCount { get; private set; }

            public int TrialCount { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int PracticeTrialCount { get; private set; }

            public int Id { get; private set; }                 // 1-based

            public Level Level { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int PracticeScore { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException("reader");

                this.Name = reader.ReadBytes(0x80);
                this.ClearCount = reader.ReadInt32();
                this.PracticeClearCount = reader.ReadInt32();
                this.TrialCount = reader.ReadInt32();
                this.PracticeTrialCount = reader.ReadInt32();
                this.Id = reader.ReadInt32() + 1;
                this.Level = (Level)reader.ReadInt32();
                this.PracticeScore = reader.ReadInt32();
            }

            public bool HasTried()
            {
                return this.TrialCount > 0;
            }
        }

        private class Practice : IBinaryReadable
        {
            public uint Score { get; private set; }         // * 10

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte ClearFlag { get; private set; }     // 0x00: Not clear, 0x01: Cleared

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte EnableFlag { get; private set; }    // 0x00: Disable, 0x01: Enable

            public void ReadFrom(BinaryReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException("reader");

                this.Score = reader.ReadUInt32();
                this.ClearFlag = reader.ReadByte();
                this.EnableFlag = reader.ReadByte();
                reader.ReadUInt16();    // always 0x0000?
            }
        }
    }
}
