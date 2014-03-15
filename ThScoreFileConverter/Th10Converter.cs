//-----------------------------------------------------------------------
// <copyright file="Th10Converter.cs" company="None">
//     (c) 2013-2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.LayoutRules",
    "SA1503:CurlyBracketsMustNotBeOmitted",
    Justification = "Reviewed.")]

namespace ThScoreFileConverter
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using CardInfo = SpellCardInfo<Th10Converter.Stage, Th10Converter.Level>;

    internal class Th10Converter : ThConverter
    {
        private static readonly Dictionary<int, CardInfo> CardTable;

        private static readonly string LevelPattern;
        private static readonly string LevelWithTotalPattern;
        private static readonly string CharaPattern;
        private static readonly string CharaWithTotalPattern;
        private static readonly string StagePattern;
        private static readonly string StageWithTotalPattern;

        private static readonly Func<string, Level> ToLevel;
        private static readonly Func<string, LevelWithTotal> ToLevelWithTotal;
        private static readonly Func<string, Chara> ToChara;
        private static readonly Func<string, CharaWithTotal> ToCharaWithTotal;
        private static readonly Func<string, Stage> ToStage;
        private static readonly Func<string, StageWithTotal> ToStageWithTotal;

        private AllScoreData allScoreData = null;

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Performance",
            "CA1810:InitializeReferenceTypeStaticFieldsInline",
            Justification = "Reviewed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.SpacingRules",
            "SA1025:CodeMustNotContainMultipleWhitespaceInARow",
            Justification = "Reviewed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.SpacingRules",
            "SA1008:OpeningParenthesisMustBeSpacedCorrectly",
            Justification = "Reviewed.")]
        static Th10Converter()
        {
            var cardList = new List<CardInfo>()
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
            };
            CardTable = cardList.ToDictionary(card => card.Number);

            var levels = Utils.GetEnumerator<Level>();
            var levelsWithTotal = Utils.GetEnumerator<LevelWithTotal>();
            var charas = Utils.GetEnumerator<Chara>();
            var charasWithTotal = Utils.GetEnumerator<CharaWithTotal>();
            var stages = Utils.GetEnumerator<Stage>();
            var stagesWithTotal = Utils.GetEnumerator<StageWithTotal>();

            LevelPattern = string.Join(
                string.Empty, levels.Select(lv => lv.ToShortName()).ToArray());
            LevelWithTotalPattern = string.Join(
                string.Empty, levelsWithTotal.Select(lv => lv.ToShortName()).ToArray());
            CharaPattern = string.Join(
                "|", charas.Select(ch => ch.ToShortName()).ToArray());
            CharaWithTotalPattern = string.Join(
                "|", charasWithTotal.Select(ch => ch.ToShortName()).ToArray());
            StagePattern = string.Join(
                string.Empty, stages.Select(st => st.ToShortName()).ToArray());
            StageWithTotalPattern = string.Join(
                string.Empty, stagesWithTotal.Select(st => st.ToShortName()).ToArray());

            var comparisonType = StringComparison.OrdinalIgnoreCase;

            ToLevel = (shortName =>
                levels.First(lv => lv.ToShortName().Equals(shortName, comparisonType)));
            ToLevelWithTotal = (shortName =>
                levelsWithTotal.First(lv => lv.ToShortName().Equals(shortName, comparisonType)));
            ToChara = (shortName =>
                charas.First(ch => ch.ToShortName().Equals(shortName, comparisonType)));
            ToCharaWithTotal = (shortName =>
                charasWithTotal.First(ch => ch.ToShortName().Equals(shortName, comparisonType)));
            ToStage = (shortName =>
                stages.First(st => st.ToShortName().Equals(shortName, comparisonType)));
            ToStageWithTotal = (shortName =>
                stagesWithTotal.First(st => st.ToShortName().Equals(shortName, comparisonType)));
        }

        public Th10Converter()
        {
        }

        public enum Level
        {
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("X")] Extra
        }

        public enum LevelWithTotal
        {
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("X")] Extra,
            [EnumAltName("T")] Total
        }

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

        public enum Stage
        {
            [EnumAltName("1")] St1,
            [EnumAltName("2")] St2,
            [EnumAltName("3")] St3,
            [EnumAltName("4")] St4,
            [EnumAltName("5")] St5,
            [EnumAltName("6")] St6,
            [EnumAltName("X")] Extra
        }

        public enum StageWithTotal
        {
            [EnumAltName("1")] St1,
            [EnumAltName("2")] St2,
            [EnumAltName("3")] St3,
            [EnumAltName("4")] St4,
            [EnumAltName("5")] St5,
            [EnumAltName("6")] St6,
            [EnumAltName("X")] Extra,
            [EnumAltName("0")] Total
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.SpacingRules",
            "SA1025:CodeMustNotContainMultipleWhitespaceInARow",
            Justification = "Reviewed.")]
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

        protected override void Convert(Stream input, Stream output, bool hideUntriedCards)
        {
            var reader = new StreamReader(input, Encoding.GetEncoding("shift_jis"));
            var writer = new StreamWriter(output, Encoding.GetEncoding("shift_jis"));

            var allLines = reader.ReadToEnd();
            allLines = this.ReplaceScore(allLines);
            allLines = this.ReplaceCareer(allLines);
            allLines = this.ReplaceCard(allLines, hideUntriedCards);
            allLines = this.ReplaceCollectRate(allLines);
            allLines = this.ReplaceClear(allLines);
            allLines = this.ReplaceChara(allLines);
            allLines = this.ReplaceCharaEx(allLines);
            allLines = this.ReplacePractice(allLines);

            writer.Write(allLines);
            writer.Flush();
            writer.BaseStream.SetLength(writer.BaseStream.Position);
        }

        private static bool Decrypt(Stream input, Stream output)
        {
            var reader = new BinaryReader(input);
            var writer = new BinaryWriter(output);

            var header = new Header();
            header.ReadFrom(reader);
            if (header.Signature != "TH10")
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
                while (true)
                {
                    chapter.ReadFrom(reader);

                    if (!((chapter.Signature == "CR") && (chapter.Version == 0x0000)) &&
                        !((chapter.Signature == "ST") && (chapter.Version == 0x0000)))
                        return false;

                    //// -4 means the size of Size.
                    reader.BaseStream.Seek(-4, SeekOrigin.Current);
                    //// 8 means the total size of Signature, Version, and Checksum.
                    var body = reader.ReadBytes(chapter.Size - 8);
                    var sum = body.Sum(elem => (int)elem);
                    if (sum != chapter.Checksum)
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.LayoutRules",
            "SA1513:ClosingCurlyBracketMustBeFollowedByBlankLine",
            Justification = "Reviewed.")]
        private static AllScoreData Read(Stream input)
        {
            var reader = new BinaryReader(input);
            var allScoreData = new AllScoreData();
            var chapter = new Chapter();

            var header = new Header();
            header.ReadFrom(reader);
            allScoreData.Header = header;

            try
            {
                while (true)
                {
                    chapter.ReadFrom(reader);
                    switch (chapter.Signature)
                    {
                        case "CR":
                            {
                                var clearData = new ClearData(chapter);
                                clearData.ReadFrom(reader);
                                if (!allScoreData.ClearData.ContainsKey(clearData.Chara))
                                    allScoreData.ClearData.Add(clearData.Chara, clearData);
                            }
                            break;
                        case "ST":
                            {
                                var status = new Status(chapter);
                                status.ReadFrom(reader);
                                allScoreData.Status = status;
                            }
                            break;
                        default:
                            // 12 means the total size of Signature, Version, Checksum, and Size.
                            reader.ReadBytes(chapter.Size - 12);
                            break;
                    }
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
        private string ReplaceScore(string input)
        {
            var pattern = Utils.Format(@"%T10SCR([{0}])({1})(\d)([1-5])", LevelPattern, CharaPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevel(match.Groups[1].Value);
                var chara = (CharaWithTotal)ToChara(match.Groups[2].Value);
                var rank = Utils.ToZeroBased(int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                var ranking = this.allScoreData.ClearData[chara].Rankings[level][rank];
                switch (type)
                {
                    case 1:     // name
                        return Encoding.Default.GetString(ranking.Name).Split('\0')[0];
                    case 2:     // score
                        return this.ToNumberString((ranking.Score * 10) + ranking.ContinueCount);
                    case 3:     // stage
                        if (ranking.DateTime > 0)
                            return (ranking.StageProgress == StageProgress.Extra)
                                ? "Not Clear" : ranking.StageProgress.ToShortName();
                        else
                            return StageProgress.None.ToShortName();
                    case 4:     // date & time
                        if (ranking.DateTime > 0)
                            return new DateTime(1970, 1, 1).AddSeconds(ranking.DateTime)
                                .ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture);
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
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T10C[xxx][yy][z]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceCareer(string input)
        {
            var pattern = Utils.Format(@"%T10C(\d{{3}})({0})([12])", CharaWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var chara = ToCharaWithTotal(match.Groups[2].Value);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                Func<SpellCard, int> getCount = (card => 0);
                if (type == 1)
                    getCount = (card => card.ClearCount);
                else
                    getCount = (card => card.TrialCount);

                var cards = this.allScoreData.ClearData[chara].Cards;
                if (number == 0)
                    return this.ToNumberString(cards.Values.Sum(getCount));
                else if (CardTable.ContainsKey(number))
                {
                    SpellCard card;
                    if (cards.TryGetValue(number, out card))
                        return this.ToNumberString(getCount(card));
                    else
                        return "0";
                }
                else
                    return match.ToString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T10CARD[xxx][y]
        private string ReplaceCard(string input, bool hideUntriedCards)
        {
            var pattern = @"%T10CARD(\d{3})([NR])";
            var evaluator = new MatchEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var type = match.Groups[2].Value.ToUpperInvariant();

                if (CardTable.ContainsKey(number))
                {
                    if (type == "N")
                    {
                        if (hideUntriedCards)
                        {
                            var cards = this.allScoreData.ClearData[CharaWithTotal.Total].Cards;
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
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T10CRG[w][xx][y][z]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceCollectRate(string input)
        {
            var pattern = Utils.Format(
                @"%T10CRG([{0}])({1})([{2}])([12])",
                LevelWithTotalPattern,
                CharaWithTotalPattern,
                StageWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevelWithTotal(match.Groups[1].Value);
                var chara = ToCharaWithTotal(match.Groups[2].Value);
                var stage = ToStageWithTotal(match.Groups[3].Value);
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                if (stage == StageWithTotal.Extra)
                    return match.ToString();

                Func<SpellCard, bool> findByLevel = (card => true);
                Func<SpellCard, bool> findByStage = (card => true);
                Func<SpellCard, bool> findByType = (card => true);

                if (stage == StageWithTotal.Total)
                {
                    // Do nothing
                }
                else
                    findByStage = (card => CardTable[card.Number].Stage == (Stage)stage);

                switch (level)
                {
                    case LevelWithTotal.Total:
                        // Do nothing
                        break;
                    case LevelWithTotal.Extra:
                        findByStage = (card => CardTable[card.Number].Stage == Stage.Extra);
                        break;
                    default:
                        findByLevel = (card => card.Level == (Level)level);
                        break;
                }

                if (type == 1)
                    findByType = (card => card.ClearCount > 0);
                else
                    findByType = (card => card.TrialCount > 0);

                var and = Utils.MakeAndPredicate(findByLevel, findByStage, findByType);
                return this.allScoreData.ClearData[chara].Cards.Values.Count(and)
                    .ToString(CultureInfo.CurrentCulture);
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T10CLEAR[x][yy]
        private string ReplaceClear(string input)
        {
            var pattern = Utils.Format(@"%T10CLEAR([{0}])({1})", LevelPattern, CharaPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevel(match.Groups[1].Value);
                var chara = (CharaWithTotal)ToChara(match.Groups[2].Value);

                var rankings = this.allScoreData.ClearData[chara].Rankings[level]
                    .Where(ranking => ranking.DateTime > 0);
                var stageProgress = (rankings.Count() > 0)
                    ? rankings.Max(ranking => ranking.StageProgress) : StageProgress.None;

                return (stageProgress == StageProgress.Extra) ? "Not Clear" : stageProgress.ToShortName();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T10CHARA[xx][y]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceChara(string input)
        {
            var pattern = Utils.Format(@"%T10CHARA({0})([1-3])", CharaWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var chara = ToCharaWithTotal(match.Groups[1].Value);
                var type = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                Func<ClearData, long> getValueByType = (data => 0L);
                Func<long, string> toString = (value => string.Empty);
                if (type == 1)
                {
                    getValueByType = (data => data.TotalPlayCount);
                    toString = (value => this.ToNumberString(value));
                }
                else if (type == 2)
                {
                    getValueByType = (data => data.PlayTime);
                    toString = (value => new Time(value).ToString());
                }
                else
                {
                    getValueByType = (data => data.ClearCounts.Values.Sum());
                    toString = (value => this.ToNumberString(value));
                }

                Func<AllScoreData, long> getValueByChara = (allData => 0L);
                if (chara == CharaWithTotal.Total)
                    getValueByChara = (allData => allData.ClearData.Values.Sum(
                        data => (data.Chara != chara) ? getValueByType(data) : 0L));
                else
                    getValueByChara = (allData => getValueByType(allData.ClearData[chara]));

                return toString(getValueByChara(this.allScoreData));
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T10CHARAEX[x][yy][z]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1119:StatementMustNotUseUnnecessaryParenthesis",
            Justification = "Reviewed.")]
        private string ReplaceCharaEx(string input)
        {
            var pattern = Utils.Format(
                @"%T10CHARAEX([{0}])({1})([1-3])", LevelWithTotalPattern, CharaWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevelWithTotal(match.Groups[1].Value);
                var chara = ToCharaWithTotal(match.Groups[2].Value);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                Func<ClearData, long> getValueByType = (data => 0L);
                Func<long, string> toString = (value => string.Empty);
                if (type == 1)
                {
                    getValueByType = (data => data.TotalPlayCount);
                    toString = (value => this.ToNumberString(value));
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
                    toString = (value => this.ToNumberString(value));
                }

                Func<AllScoreData, long> getValueByChara = (allData => 0L);
                if (chara == CharaWithTotal.Total)
                    getValueByChara = (allData => allData.ClearData.Values.Sum(
                        data => (data.Chara != chara) ? getValueByType(data) : 0L));
                else
                    getValueByChara = (allData => getValueByType(allData.ClearData[chara]));

                return toString(getValueByChara(this.allScoreData));
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T10PRAC[x][yy][z]
        private string ReplacePractice(string input)
        {
            var pattern = Utils.Format(
                @"%T10PRAC([{0}])({1})([{2}])", LevelPattern, CharaPattern, StagePattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = ToLevel(match.Groups[1].Value);
                var chara = (CharaWithTotal)ToChara(match.Groups[2].Value);
                var stage = ToStage(match.Groups[3].Value);

                if (level == Level.Extra)
                    return match.ToString();
                if (stage == Stage.Extra)
                    return match.ToString();

                if (this.allScoreData.ClearData.ContainsKey(chara))
                {
                    var key = new LevelStagePair(level, stage);
                    var practices = this.allScoreData.ClearData[chara].Practices;
                    return practices.ContainsKey(key)
                        ? this.ToNumberString(practices[key].Score * 10) : "0";
                }
                else
                    return "0";
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        private class LevelStagePair : Pair<Level, Stage>
        {
            public LevelStagePair(Level level, Stage stage)
                : base(level, stage)
            {
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public Level Level
            {
                get { return this.First; }
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
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

            public Header Header { get; set; }

            public Dictionary<CharaWithTotal, ClearData> ClearData { get; set; }

            public Status Status { get; set; }
        }

        private class Header : IBinaryReadable
        {
            private uint unknown1;
            private uint unknown2;

            public string Signature { get; private set; }

            public int EncodedAllSize { get; private set; }

            public int EncodedBodySize { get; private set; }

            public int DecodedBodySize { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                this.Signature = new string(reader.ReadChars(4));
                this.EncodedAllSize = reader.ReadInt32();
                this.unknown1 = reader.ReadUInt32();
                this.unknown2 = reader.ReadUInt32();
                this.EncodedBodySize = reader.ReadInt32();
                this.DecodedBodySize = reader.ReadInt32();
            }

            public void WriteTo(BinaryWriter writer)
            {
                writer.Write(this.Signature.ToCharArray());
                writer.Write(this.EncodedAllSize);
                writer.Write(this.unknown1);
                writer.Write(this.unknown2);
                writer.Write(this.EncodedBodySize);
                writer.Write(this.DecodedBodySize);
            }
        }

        private class Chapter : IBinaryReadable
        {
            public Chapter()
            {
            }

            public Chapter(Chapter ch)
            {
                this.Signature = ch.Signature;
                this.Version = ch.Version;
                this.Checksum = ch.Checksum;
                this.Size = ch.Size;
            }

            public string Signature { get; private set; }

            public ushort Version { get; private set; }

            public uint Checksum { get; private set; }

            public int Size { get; private set; }

            public virtual void ReadFrom(BinaryReader reader)
            {
                this.Signature = Encoding.Default.GetString(reader.ReadBytes(2));
                this.Version = reader.ReadUInt16();
                this.Checksum = reader.ReadUInt32();
                this.Size = reader.ReadInt32();
            }
        }

        private class ClearData : Chapter   // per character
        {
            public ClearData(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "CR")
                    throw new InvalidDataException("Signature");
                if (this.Version != 0x0000)
                    throw new InvalidDataException("Version");
                if (this.Size != 0x0000437C)
                    throw new InvalidDataException("Size");

                var numLevels = Enum.GetValues(typeof(Level)).Length;
                var numPairs = Utils.GetEnumerator<Level>().Where(lv => lv != Level.Extra).Count() *
                    Utils.GetEnumerator<Stage>().Where(st => st != Stage.Extra).Count();
                this.Rankings = new Dictionary<Level, ScoreData[]>(numLevels);
                this.ClearCounts = new Dictionary<Level, int>(numLevels);
                this.Practices = new Dictionary<LevelStagePair, Practice>(numPairs);
                this.Cards = new Dictionary<int, SpellCard>(CardTable.Count);
            }

            public CharaWithTotal Chara { get; private set; }   // size: 4Bytes

            public Dictionary<Level, ScoreData[]> Rankings { get; private set; }

            public int TotalPlayCount { get; private set; }

            public int PlayTime { get; private set; }           // = seconds * 60fps

            public Dictionary<Level, int> ClearCounts { get; private set; }

            public Dictionary<LevelStagePair, Practice> Practices { get; private set; }

            public Dictionary<int, SpellCard> Cards { get; private set; }

            public override void ReadFrom(BinaryReader reader)
            {
                var levels = Utils.GetEnumerator<Level>();
                var stages = Utils.GetEnumerator<Stage>();

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

                foreach (var level in levels.Where(lv => lv != Level.Extra))
                    foreach (var stage in stages.Where(st => st != Stage.Extra))
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
                    if (!this.Cards.ContainsKey(card.Number))
                        this.Cards.Add(card.Number, card);
                }
            }
        }

        private class Status : Chapter
        {
            public Status(Chapter ch)
                : base(ch)
            {
                if (this.Signature != "ST")
                    throw new InvalidDataException("Signature");
                if (this.Version != 0x0000)
                    throw new InvalidDataException("Version");
                if (this.Size != 0x00000448)
                    throw new InvalidDataException("Size");
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public byte[] LastName { get; private set; }    // .Length = 10 (The last 2 bytes are always 0x00 ?)

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public byte[] BgmFlags { get; private set; }    // .Length = 18

            public override void ReadFrom(BinaryReader reader)
            {
                this.LastName = reader.ReadBytes(10);
                reader.ReadBytes(0x10);
                this.BgmFlags = reader.ReadBytes(18);
                reader.ReadBytes(0x0410);
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
                this.Score = reader.ReadUInt32();
                this.StageProgress = (StageProgress)reader.ReadByte();
                this.ContinueCount = reader.ReadByte();
                this.Name = reader.ReadBytes(10);
                this.DateTime = reader.ReadUInt32();
                this.SlowRate = reader.ReadSingle();
            }
        }

        private class SpellCard : IBinaryReadable
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public byte[] Name { get; private set; }    // .Length = 0x80

            public int ClearCount { get; private set; }

            public int TrialCount { get; private set; }

            public int Number { get; private set; }     // 1-based

            public Level Level { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                this.Name = reader.ReadBytes(0x80);
                this.ClearCount = reader.ReadInt32();
                this.TrialCount = reader.ReadInt32();
                this.Number = reader.ReadInt32() + 1;
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

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "For future use.")]
            public uint StageFlag { get; private set; } // 0x00000000: disable, 0x00000101: enable ?

            public void ReadFrom(BinaryReader reader)
            {
                this.Score = reader.ReadUInt32();
                this.StageFlag = reader.ReadUInt32();
            }
        }
    }
}
