//-----------------------------------------------------------------------
// <copyright file="Th15Converter.cs" company="None">
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
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th15;
using CardInfo = ThScoreFileConverter.Models.SpellCardInfo<
    ThScoreFileConverter.Models.Stage, ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverter.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th15Converter : ThConverter
    {
        // Thanks to thwiki.info
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly", Justification = "Reviewed.")]
        private static readonly Dictionary<int, CardInfo> CardTable =
            new List<CardInfo>()
            {
                new CardInfo(  1, "凶弾「スピードストライク」",           Stage.One,   Level.Hard),
                new CardInfo(  2, "凶弾「スピードストライク」",           Stage.One,   Level.Lunatic),
                new CardInfo(  3, "弾符「イーグルシューティング」",       Stage.One,   Level.Easy),
                new CardInfo(  4, "弾符「イーグルシューティング」",       Stage.One,   Level.Normal),
                new CardInfo(  5, "弾符「イーグルシューティング」",       Stage.One,   Level.Hard),
                new CardInfo(  6, "弾符「鷹は撃ち抜いた」",               Stage.One,   Level.Lunatic),
                new CardInfo(  7, "銃符「ルナティックガン」",             Stage.One,   Level.Easy),
                new CardInfo(  8, "銃符「ルナティックガン」",             Stage.One,   Level.Normal),
                new CardInfo(  9, "銃符「ルナティックガン」",             Stage.One,   Level.Hard),
                new CardInfo( 10, "銃符「ルナティックガン」",             Stage.One,   Level.Lunatic),
                new CardInfo( 11, "兎符「ストロベリーダンゴ」",           Stage.Two,   Level.Easy),
                new CardInfo( 12, "兎符「ストロベリーダンゴ」",           Stage.Two,   Level.Normal),
                new CardInfo( 13, "兎符「ベリーベリーダンゴ」",           Stage.Two,   Level.Hard),
                new CardInfo( 14, "兎符「ベリーベリーダンゴ」",           Stage.Two,   Level.Lunatic),
                new CardInfo( 15, "兎符「ダンゴインフリューエンス」",     Stage.Two,   Level.Easy),
                new CardInfo( 16, "兎符「ダンゴインフリューエンス」",     Stage.Two,   Level.Normal),
                new CardInfo( 17, "兎符「ダンゴインフリューエンス」",     Stage.Two,   Level.Hard),
                new CardInfo( 18, "兎符「ダンゴインフリューエンス」",     Stage.Two,   Level.Lunatic),
                new CardInfo( 19, "月見「セプテンバーフルムーン」",       Stage.Two,   Level.Easy),
                new CardInfo( 20, "月見「セプテンバーフルムーン」",       Stage.Two,   Level.Normal),
                new CardInfo( 21, "月見「セプテンバーフルムーン」",       Stage.Two,   Level.Hard),
                new CardInfo( 22, "月見酒「ルナティックセプテンバー」",   Stage.Two,   Level.Lunatic),
                new CardInfo( 23, "夢符「緋色の悪夢」",                   Stage.Three, Level.Easy),
                new CardInfo( 24, "夢符「緋色の悪夢」",                   Stage.Three, Level.Normal),
                new CardInfo( 25, "夢符「緋色の圧迫悪夢」",               Stage.Three, Level.Hard),
                new CardInfo( 26, "夢符「緋色の圧迫悪夢」",               Stage.Three, Level.Lunatic),
                new CardInfo( 27, "夢符「藍色の愁夢」",                   Stage.Three, Level.Easy),
                new CardInfo( 28, "夢符「藍色の愁夢」",                   Stage.Three, Level.Normal),
                new CardInfo( 29, "夢符「藍色の愁三重夢」",               Stage.Three, Level.Hard),
                new CardInfo( 30, "夢符「愁永遠の夢」",                   Stage.Three, Level.Lunatic),
                new CardInfo( 31, "夢符「刈安色の迷夢」",                 Stage.Three, Level.Easy),
                new CardInfo( 32, "夢符「刈安色の迷夢」",                 Stage.Three, Level.Normal),
                new CardInfo( 33, "夢符「刈安色の錯綜迷夢」",             Stage.Three, Level.Hard),
                new CardInfo( 34, "夢符「刈安色の錯綜迷夢」",             Stage.Three, Level.Lunatic),
                new CardInfo( 35, "夢符「ドリームキャッチャー」",         Stage.Three, Level.Easy),
                new CardInfo( 36, "夢符「ドリームキャッチャー」",         Stage.Three, Level.Normal),
                new CardInfo( 37, "夢符「蒼色のドリームキャッチャー」",   Stage.Three, Level.Hard),
                new CardInfo( 38, "夢符「夢我夢中」",                     Stage.Three, Level.Lunatic),
                new CardInfo( 39, "月符「紺色の狂夢」",                   Stage.Three, Level.Easy),
                new CardInfo( 40, "月符「紺色の狂夢」",                   Stage.Three, Level.Normal),
                new CardInfo( 41, "月符「紺色の狂夢」",                   Stage.Three, Level.Hard),
                new CardInfo( 42, "月符「紺色の狂夢」",                   Stage.Three, Level.Lunatic),
                new CardInfo( 43, "玉符「烏合の呪」",                     Stage.Four,  Level.Easy),
                new CardInfo( 44, "玉符「烏合の呪」",                     Stage.Four,  Level.Normal),
                new CardInfo( 45, "玉符「烏合の逆呪」",                   Stage.Four,  Level.Hard),
                new CardInfo( 46, "玉符「烏合の二重呪」",                 Stage.Four,  Level.Lunatic),
                new CardInfo( 47, "玉符「穢身探知型機雷」",               Stage.Four,  Level.Easy),
                new CardInfo( 48, "玉符「穢身探知型機雷」",               Stage.Four,  Level.Normal),
                new CardInfo( 49, "玉符「穢身探知型機雷 改」",            Stage.Four,  Level.Hard),
                new CardInfo( 50, "玉符「穢身探知型機雷 改」",            Stage.Four,  Level.Lunatic),
                new CardInfo( 51, "玉符「神々の弾冠」",                   Stage.Four,  Level.Easy),
                new CardInfo( 52, "玉符「神々の弾冠」",                   Stage.Four,  Level.Normal),
                new CardInfo( 53, "玉符「神々の光り輝く弾冠」",           Stage.Four,  Level.Hard),
                new CardInfo( 54, "玉符「神々の光り輝く弾冠」",           Stage.Four,  Level.Lunatic),
                new CardInfo( 55, "「片翼の白鷺」",                       Stage.Four,  Level.Easy),
                new CardInfo( 56, "「片翼の白鷺」",                       Stage.Four,  Level.Normal),
                new CardInfo( 57, "「片翼の白鷺」",                       Stage.Four,  Level.Hard),
                new CardInfo( 58, "「片翼の白鷺」",                       Stage.Four,  Level.Lunatic),
                new CardInfo( 59, "獄符「ヘルエクリプス」",               Stage.Five,  Level.Easy),
                new CardInfo( 60, "獄符「ヘルエクリプス」",               Stage.Five,  Level.Normal),
                new CardInfo( 61, "獄符「地獄の蝕」",                     Stage.Five,  Level.Hard),
                new CardInfo( 62, "獄符「地獄の蝕」",                     Stage.Five,  Level.Lunatic),
                new CardInfo( 63, "獄符「フラッシュアンドストライプ」",   Stage.Five,  Level.Easy),
                new CardInfo( 64, "獄符「フラッシュアンドストライプ」",   Stage.Five,  Level.Normal),
                new CardInfo( 65, "獄符「スターアンドストライプ」",       Stage.Five,  Level.Hard),
                new CardInfo( 66, "獄符「スターアンドストライプ」",       Stage.Five,  Level.Lunatic),
                new CardInfo( 67, "獄炎「グレイズインフェルノ」",         Stage.Five,  Level.Easy),
                new CardInfo( 68, "獄炎「グレイズインフェルノ」",         Stage.Five,  Level.Normal),
                new CardInfo( 69, "獄炎「グレイズインフェルノ」",         Stage.Five,  Level.Hard),
                new CardInfo( 70, "獄炎「かすりの獄意」",                 Stage.Five,  Level.Lunatic),
                new CardInfo( 71, "地獄「ストライプドアビス」",           Stage.Five,  Level.Easy),
                new CardInfo( 72, "地獄「ストライプドアビス」",           Stage.Five,  Level.Normal),
                new CardInfo( 73, "地獄「ストライプドアビス」",           Stage.Five,  Level.Hard),
                new CardInfo( 74, "地獄「ストライプドアビス」",           Stage.Five,  Level.Lunatic),
                new CardInfo( 75, "「フェイクアポロ」",                   Stage.Five,  Level.Easy),
                new CardInfo( 76, "「フェイクアポロ」",                   Stage.Five,  Level.Normal),
                new CardInfo( 77, "「アポロ捏造説」",                     Stage.Five,  Level.Hard),
                new CardInfo( 78, "「アポロ捏造説」",                     Stage.Five,  Level.Lunatic),
                new CardInfo( 79, "「掌の純光」",                         Stage.Six,   Level.Easy),
                new CardInfo( 80, "「掌の純光」",                         Stage.Six,   Level.Normal),
                new CardInfo( 81, "「掌の純光」",                         Stage.Six,   Level.Hard),
                new CardInfo( 82, "「掌の純光」",                         Stage.Six,   Level.Lunatic),
                new CardInfo( 83, "「殺意の百合」",                       Stage.Six,   Level.Easy),
                new CardInfo( 84, "「殺意の百合」",                       Stage.Six,   Level.Normal),
                new CardInfo( 85, "「殺意の百合」",                       Stage.Six,   Level.Hard),
                new CardInfo( 86, "「殺意の百合」",                       Stage.Six,   Level.Lunatic),
                new CardInfo( 87, "「原始の神霊界」",                     Stage.Six,   Level.Easy),
                new CardInfo( 88, "「原始の神霊界」",                     Stage.Six,   Level.Normal),
                new CardInfo( 89, "「現代の神霊界」",                     Stage.Six,   Level.Hard),
                new CardInfo( 90, "「現代の神霊界」",                     Stage.Six,   Level.Lunatic),
                new CardInfo( 91, "「震え凍える星」",                     Stage.Six,   Level.Easy),
                new CardInfo( 92, "「震え凍える星」",                     Stage.Six,   Level.Normal),
                new CardInfo( 93, "「震え凍える星」",                     Stage.Six,   Level.Hard),
                new CardInfo( 94, "「震え凍える星」",                     Stage.Six,   Level.Lunatic),
                new CardInfo( 95, "「純粋なる狂気」",                     Stage.Six,   Level.Easy),
                new CardInfo( 96, "「純粋なる狂気」",                     Stage.Six,   Level.Normal),
                new CardInfo( 97, "「純粋なる狂気」",                     Stage.Six,   Level.Hard),
                new CardInfo( 98, "「純粋なる狂気」",                     Stage.Six,   Level.Lunatic),
                new CardInfo( 99, "「溢れ出る瑕穢」",                     Stage.Six,   Level.Easy),
                new CardInfo(100, "「溢れ出る瑕穢」",                     Stage.Six,   Level.Normal),
                new CardInfo(101, "「溢れ出る瑕穢」",                     Stage.Six,   Level.Hard),
                new CardInfo(102, "「地上穢の純化」",                     Stage.Six,   Level.Lunatic),
                new CardInfo(103, "純符「ピュアリーバレットヘル」",       Stage.Six,   Level.Easy),
                new CardInfo(104, "純符「ピュアリーバレットヘル」",       Stage.Six,   Level.Normal),
                new CardInfo(105, "純符「純粋な弾幕地獄」",               Stage.Six,   Level.Hard),
                new CardInfo(106, "純符「純粋な弾幕地獄」",               Stage.Six,   Level.Lunatic),
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
                new CardInfo(119, "「最初で最後の無名の弾幕」",           Stage.Extra, Level.Extra),
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
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("P")] Pointdevice,
            [EnumAltName("L")] Legacy,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum Chara
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("RM")] Reimu,
            [EnumAltName("MR")] Marisa,
            [EnumAltName("SN")] Sanae,
            [EnumAltName("RS")] Reisen,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum CharaWithTotal
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("RM")] Reimu,
            [EnumAltName("MR")] Marisa,
            [EnumAltName("SN")] Sanae,
            [EnumAltName("RS")] Reisen,
            [EnumAltName("TL")] Total,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum StagePractice
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("1")] St1,
            [EnumAltName("2")] St2,
            [EnumAltName("3")] St3,
            [EnumAltName("4")] St4,
            [EnumAltName("5")] St5,
            [EnumAltName("6")] St6,
            [EnumAltName("X")] Extra,
            [EnumAltName("-")] NotUsed,
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
            [EnumAltName("All Clear")]   Clear,
            [EnumAltName("Extra Clear")] ExtraClear,
#pragma warning restore SA1134 // Attributes should not share line
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
                new PracticeReplacer(this),
            };
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
        }

        private static AllScoreData Read(Stream input)
        {
            var dictionary = new Dictionary<string, Action<AllScoreData, Th10.Chapter>>
            {
                { ClearData.ValidSignature, (data, ch) => data.Set(new ClearData(ch)) },
                { Status.ValidSignature,    (data, ch) => data.Set(new Status(ch))    },
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
                    (allScoreData.ClearData.Count == Enum.GetValues(typeof(CharaWithTotal)).Length) &&
                    (allScoreData.Status != null))
                    return allScoreData;
                else
                    return null;
            }
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

                    var ranking = parent.allScoreData.ClearData[chara].GameModeData[mode].Rankings[level][rank];
                    switch (type)
                    {
                        case 1:     // name
                            return Encoding.Default.GetString(ranking.Name.ToArray()).Split('\0')[0];
                        case 2:     // score
                            return Utils.ToNumberString((ranking.Score * 10) + ranking.ContinueCount);
                        case 3:     // stage
                            if (ranking.DateTime == 0)
                                return StageProgress.None.ToShortName();
                            if (ranking.StageProgress == StageProgress.Extra)
                                return "Not Clear";
                            if (ranking.StageProgress == StageProgress.ExtraClear)
                                return StageProgress.Clear.ToShortName();
                            return ranking.StageProgress.ToShortName();
                        case 4:     // date & time
                            if (ranking.DateTime == 0)
                                return "----/--/-- --:--:--";
                            return new DateTime(1970, 1, 1).AddSeconds(ranking.DateTime).ToLocalTime()
                                .ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture);
                        case 5:     // slow
                            if (ranking.DateTime == 0)
                                return "-----%";
                            return Utils.Format("{0:F3}%", ranking.SlowRate);
                        case 6:     // retry
                            if (ranking.DateTime == 0)
                                return "-----";
                            return Utils.ToNumberString(ranking.RetryCount);
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

                    Func<Th13.ISpellCard<Level>, int> getCount;
                    if (type == 1)
                        getCount = (card => card.ClearCount);
                    else
                        getCount = (card => card.TrialCount);

                    var cards = parent.allScoreData.ClearData[chara].GameModeData[mode].Cards;
                    if (number == 0)
                    {
                        return Utils.ToNumberString(cards.Values.Sum(getCount));
                    }
                    else if (CardTable.ContainsKey(number))
                    {
                        if (cards.TryGetValue(number, out var card))
                            return Utils.ToNumberString(getCount(card));
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
                                var tried = parent.allScoreData.ClearData[CharaWithTotal.Total].GameModeData.Any(
                                    data => data.Value.Cards.TryGetValue(number, out var card) && card.HasTried);
                                if (!tried)
                                    return "??????????";
                            }

                            return CardTable[number].Name;
                        }
                        else
                        {
                            return CardTable[number].Level.ToString();
                        }
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

                    Func<Th13.ISpellCard<Level>, bool> findByType;
                    if (type == 1)
                        findByType = (card => card.ClearCount > 0);
                    else
                        findByType = (card => card.TrialCount > 0);

                    Func<Th13.ISpellCard<Level>, bool> findByStage;
                    if (stage == StageWithTotal.Total)
                        findByStage = (card => true);
                    else
                        findByStage = (card => CardTable[card.Id].Stage == (Stage)stage);

                    Func<Th13.ISpellCard<Level>, bool> findByLevel = (card => true);
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

                    return parent.allScoreData.ClearData[chara].GameModeData[mode].Cards.Values
                        .Count(Utils.MakeAndPredicate(findByType, findByLevel, findByStage))
                        .ToString(CultureInfo.CurrentCulture);
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T15CLEAR[x][y][zz]
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

                    var rankings = parent.allScoreData.ClearData[chara].GameModeData[mode].Rankings[level]
                        .Where(ranking => ranking.DateTime > 0);
                    var stageProgress = rankings.Any()
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

                    Func<IClearData, long> getValueByType;
                    Func<long, string> toString;
                    if (type == 1)
                    {
                        getValueByType = (data => data.GameModeData[mode].TotalPlayCount);
                        toString = Utils.ToNumberString;
                    }
                    else if (type == 2)
                    {
                        getValueByType = (data => data.GameModeData[mode].PlayTime);
                        toString = (value => new Time(value * 10, false).ToString());
                    }
                    else
                    {
                        getValueByType = (data => data.GameModeData[mode].ClearCounts.Values.Sum());
                        toString = Utils.ToNumberString;
                    }

                    Func<AllScoreData, long> getValueByChara;
                    if (chara == CharaWithTotal.Total)
                    {
                        getValueByChara = (allData => allData.ClearData.Values
                            .Where(data => data.Chara != chara).Sum(getValueByType));
                    }
                    else
                    {
                        getValueByChara = (allData => getValueByType(allData.ClearData[chara]));
                    }

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

                    Func<IClearData, long> getValueByType;
                    Func<long, string> toString;
                    if (type == 1)
                    {
                        getValueByType = (data => data.GameModeData[mode].TotalPlayCount);
                        toString = Utils.ToNumberString;
                    }
                    else if (type == 2)
                    {
                        getValueByType = (data => data.GameModeData[mode].PlayTime);
                        toString = (value => new Time(value * 10, false).ToString());
                    }
                    else
                    {
                        if (level == LevelWithTotal.Total)
                            getValueByType = (data => data.GameModeData[mode].ClearCounts.Values.Sum());
                        else
                            getValueByType = (data => data.GameModeData[mode].ClearCounts[level]);
                        toString = Utils.ToNumberString;
                    }

                    Func<AllScoreData, long> getValueByChara;
                    if (chara == CharaWithTotal.Total)
                    {
                        getValueByChara = (allData => allData.ClearData.Values
                            .Where(data => data.Chara != chara).Sum(getValueByType));
                    }
                    else
                    {
                        getValueByChara = (allData => getValueByType(allData.ClearData[chara]));
                    }

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
                        var key = (level, (StagePractice)stage);
                        var practices = parent.allScoreData.ClearData[chara].Practices;
                        return practices.ContainsKey(key)
                            ? Utils.ToNumberString(practices[key].Score * 10) : "0";
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
            private readonly Dictionary<CharaWithTotal, IClearData> clearData;

            public AllScoreData()
            {
                this.clearData =
                    new Dictionary<CharaWithTotal, IClearData>(Enum.GetValues(typeof(CharaWithTotal)).Length);
            }

            public Th095.HeaderBase Header { get; private set; }

            public IReadOnlyDictionary<CharaWithTotal, IClearData> ClearData => this.clearData;

            public Th125.IStatus Status { get; private set; }

            public void Set(Th095.HeaderBase header) => this.Header = header;

            public void Set(IClearData data)
            {
                if (!this.clearData.ContainsKey(data.Chara))
                    this.clearData.Add(data.Chara, data);
            }

            public void Set(Th125.IStatus status) => this.Status = status;
        }

        private class ClearData : Th10.Chapter, IClearData  // per character
        {
            public const string ValidSignature = "CR";
            public const ushort ValidVersion = 0x0001;
            public const int ValidSize = 0x0000A4A0;

            public ClearData(Th10.Chapter chapter)
                : base(chapter, ValidSignature, ValidVersion, ValidSize)
            {
                var modes = Utils.GetEnumerator<GameMode>();
                var levels = Utils.GetEnumerator<Level>();
                var stages = Utils.GetEnumerator<StagePractice>();

                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    this.Chara = (CharaWithTotal)reader.ReadInt32();

                    this.GameModeData = modes.ToDictionary(mode => mode, _ =>
                    {
                        var data = new ClearDataPerGameMode();
                        data.ReadFrom(reader);
                        return data as IClearDataPerGameMode;
                    });

                    this.Practices = levels
                        .SelectMany(level => stages.Select(stage => (level, stage)))
                        .ToDictionary(pair => pair, _ =>
                        {
                            var practice = new Th13.Practice();
                            practice.ReadFrom(reader);
                            return practice as Th13.IPractice;
                        });
                }
            }

            public CharaWithTotal Chara { get; }

            public IReadOnlyDictionary<GameMode, IClearDataPerGameMode> GameModeData { get; }

            public IReadOnlyDictionary<(Level, StagePractice), Th13.IPractice> Practices { get; }

            public static bool CanInitialize(Th10.Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
            }
        }

        private class Status : Th10.Chapter, Th125.IStatus
        {
            public const string ValidSignature = "ST";
            public const ushort ValidVersion = 0x0001;
            public const int ValidSize = 0x0000042C;

            public Status(Th10.Chapter chapter)
                : base(chapter, ValidSignature, ValidVersion, ValidSize)
            {
                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    this.LastName = reader.ReadExactBytes(10);
                    reader.ReadExactBytes(0x10);
                    this.BgmFlags = reader.ReadExactBytes(17);
                    reader.ReadExactBytes(0x11);
                    this.TotalPlayTime = reader.ReadInt32();
                    reader.ReadExactBytes(0x03E0);
                }
            }

            public IEnumerable<byte> LastName { get; }  // The last 2 bytes are always 0x00 ?

            public IEnumerable<byte> BgmFlags { get; }

            public int TotalPlayTime { get; }   // unit: 10ms

            public static bool CanInitialize(Th10.Chapter chapter)
            {
                return chapter.Signature.Equals(ValidSignature, StringComparison.Ordinal)
                    && (chapter.Version == ValidVersion)
                    && (chapter.Size == ValidSize);
            }
        }

        private class ClearDataPerGameMode : IBinaryReadable, IClearDataPerGameMode
        {
            public IReadOnlyDictionary<LevelWithTotal, IReadOnlyList<IScoreData>> Rankings { get; private set; }

            public int TotalPlayCount { get; private set; }

            public int PlayTime { get; private set; }   // unit: 10ms

            public IReadOnlyDictionary<LevelWithTotal, int> ClearCounts { get; private set; }

            public IReadOnlyDictionary<LevelWithTotal, int> ClearFlags { get; private set; } // Really...?

            public IReadOnlyDictionary<int, Th13.ISpellCard<Level>> Cards { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException(nameof(reader));

                var levelsWithTotal = Utils.GetEnumerator<LevelWithTotal>();

                this.Rankings = levelsWithTotal.ToDictionary(
                    level => level,
                    _ => Enumerable.Range(0, 10).Select(rank =>
                    {
                        var score = new ScoreData();
                        score.ReadFrom(reader);
                        return score;
                    }).ToList() as IReadOnlyList<IScoreData>);

                reader.ReadBytes(0x140);

                this.Cards = Enumerable.Range(0, CardTable.Count).Select(_ =>
                {
                    var card = new SpellCard();
                    card.ReadFrom(reader);
                    return card as Th13.ISpellCard<Level>;
                }).ToDictionary(card => card.Id);

                this.TotalPlayCount = reader.ReadInt32();
                this.PlayTime = reader.ReadInt32();
                reader.ReadUInt32();
                this.ClearCounts = levelsWithTotal.ToDictionary(level => level, _ => reader.ReadInt32());
                reader.ReadUInt32();
                this.ClearFlags = levelsWithTotal.ToDictionary(level => level, _ => reader.ReadInt32());
                reader.ReadUInt32();
            }
        }

        private class SpellCard : Th13.SpellCard<Level>
        {
            public override bool HasTried => this.TrialCount > 0;
        }
    }
}
