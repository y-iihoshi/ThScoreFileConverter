//-----------------------------------------------------------------------
// <copyright file="Th14Converter.cs" company="None">
//     (c) 2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:CurlyBracketsMustNotBeOmitted", Justification = "Reviewed.")]

namespace ThScoreFileConverter
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using CardInfo = SpellCardInfo<Th14Converter.Stage, Th14Converter.Level>;

    internal class Th14Converter : ThConverter
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

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly", Justification = "Reviewed.")]
        static Th14Converter()
        {
            var cardList = new List<CardInfo>()
            {
                new CardInfo(  1, "氷符「アルティメットブリザード」",   Stage.St1,   Level.Hard),
                new CardInfo(  2, "氷符「アルティメットブリザード」",   Stage.St1,   Level.Lunatic),
                new CardInfo(  3, "水符「テイルフィンスラップ」",       Stage.St1,   Level.Easy),
                new CardInfo(  4, "水符「テイルフィンスラップ」",       Stage.St1,   Level.Normal),
                new CardInfo(  5, "水符「テイルフィンスラップ」",       Stage.St1,   Level.Hard),
                new CardInfo(  6, "水符「テイルフィンスラップ」",       Stage.St1,   Level.Lunatic),
                new CardInfo(  7, "鱗符「スケールウェイブ」",           Stage.St1,   Level.Easy),
                new CardInfo(  8, "鱗符「スケールウェイブ」",           Stage.St1,   Level.Normal),
                new CardInfo(  9, "鱗符「逆鱗の荒波」",                 Stage.St1,   Level.Hard),
                new CardInfo( 10, "鱗符「逆鱗の大荒波」",               Stage.St1,   Level.Lunatic),
                new CardInfo( 11, "飛符「フライングヘッド」",           Stage.St2,   Level.Easy),
                new CardInfo( 12, "飛符「フライングヘッド」",           Stage.St2,   Level.Normal),
                new CardInfo( 13, "飛符「フライングヘッド」",           Stage.St2,   Level.Hard),
                new CardInfo( 14, "飛符「フライングヘッド」",           Stage.St2,   Level.Lunatic),
                new CardInfo( 15, "首符「クローズアイショット」",       Stage.St2,   Level.Easy),
                new CardInfo( 16, "首符「クローズアイショット」",       Stage.St2,   Level.Normal),
                new CardInfo( 17, "首符「ろくろ首飛来」",               Stage.St2,   Level.Hard),
                new CardInfo( 18, "首符「ろくろ首飛来」",               Stage.St2,   Level.Lunatic),
                new CardInfo( 19, "飛頭「マルチプリケイティブヘッド」", Stage.St2,   Level.Easy),
                new CardInfo( 20, "飛頭「マルチプリケイティブヘッド」", Stage.St2,   Level.Normal),
                new CardInfo( 21, "飛頭「セブンズヘッド」",             Stage.St2,   Level.Hard),
                new CardInfo( 22, "飛頭「ナインズヘッド」",             Stage.St2,   Level.Lunatic),
                new CardInfo( 23, "飛頭「デュラハンナイト」",           Stage.St2,   Level.Easy),
                new CardInfo( 24, "飛頭「デュラハンナイト」",           Stage.St2,   Level.Normal),
                new CardInfo( 25, "飛頭「デュラハンナイト」",           Stage.St2,   Level.Hard),
                new CardInfo( 26, "飛頭「デュラハンナイト」",           Stage.St2,   Level.Lunatic),
                new CardInfo( 27, "牙符「月下の犬歯」",                 Stage.St3,   Level.Hard),
                new CardInfo( 28, "牙符「月下の犬歯」",                 Stage.St3,   Level.Lunatic),
                new CardInfo( 29, "変身「トライアングルファング」",     Stage.St3,   Level.Easy),
                new CardInfo( 30, "変身「トライアングルファング」",     Stage.St3,   Level.Normal),
                new CardInfo( 31, "変身「スターファング」",             Stage.St3,   Level.Hard),
                new CardInfo( 32, "変身「スターファング」",             Stage.St3,   Level.Lunatic),
                new CardInfo( 33, "咆哮「ストレンジロア」",             Stage.St3,   Level.Easy),
                new CardInfo( 34, "咆哮「ストレンジロア」",             Stage.St3,   Level.Normal),
                new CardInfo( 35, "咆哮「満月の遠吠え」",               Stage.St3,   Level.Hard),
                new CardInfo( 36, "咆哮「満月の遠吠え」",               Stage.St3,   Level.Lunatic),
                new CardInfo( 37, "狼符「スターリングパウンス」",       Stage.St3,   Level.Easy),
                new CardInfo( 38, "狼符「スターリングパウンス」",       Stage.St3,   Level.Normal),
                new CardInfo( 39, "天狼「ハイスピードパウンス」",       Stage.St3,   Level.Hard),
                new CardInfo( 40, "天狼「ハイスピードパウンス」",       Stage.St3,   Level.Lunatic),
                new CardInfo( 41, "平曲「祇園精舎の鐘の音」",           Stage.St4,   Level.Easy),
                new CardInfo( 42, "平曲「祇園精舎の鐘の音」",           Stage.St4,   Level.Normal),
                new CardInfo( 43, "平曲「祇園精舎の鐘の音」",           Stage.St4,   Level.Hard),
                new CardInfo( 44, "平曲「祇園精舎の鐘の音」",           Stage.St4,   Level.Lunatic),
                new CardInfo( 45, "怨霊「耳無し芳一」",                 Stage.St4,   Level.Easy),
                new CardInfo( 46, "怨霊「耳無し芳一」",                 Stage.St4,   Level.Normal),
                new CardInfo( 47, "怨霊「平家の大怨霊」",               Stage.St4,   Level.Hard),
                new CardInfo( 48, "怨霊「平家の大怨霊」",               Stage.St4,   Level.Lunatic),
                new CardInfo( 49, "楽符「邪悪な五線譜」",               Stage.St4,   Level.Easy),
                new CardInfo( 50, "楽符「邪悪な五線譜」",               Stage.St4,   Level.Normal),
                new CardInfo( 51, "楽符「凶悪な五線譜」",               Stage.St4,   Level.Hard),
                new CardInfo( 52, "楽符「ダブルスコア」",               Stage.St4,   Level.Lunatic),
                new CardInfo( 53, "琴符「諸行無常の琴の音」",           Stage.St4,   Level.Easy),
                new CardInfo( 54, "琴符「諸行無常の琴の音」",           Stage.St4,   Level.Normal),
                new CardInfo( 55, "琴符「諸行無常の琴の音」",           Stage.St4,   Level.Hard),
                new CardInfo( 56, "琴符「諸行無常の琴の音」",           Stage.St4,   Level.Lunatic),
                new CardInfo( 57, "響符「平安の残響」",                 Stage.St4,   Level.Easy),
                new CardInfo( 58, "響符「平安の残響」",                 Stage.St4,   Level.Normal),
                new CardInfo( 59, "響符「エコーチェンバー」",           Stage.St4,   Level.Hard),
                new CardInfo( 60, "響符「エコーチェンバー」",           Stage.St4,   Level.Lunatic),
                new CardInfo( 61, "箏曲「下克上送箏曲」",               Stage.St4,   Level.Easy),
                new CardInfo( 62, "箏曲「下克上送箏曲」",               Stage.St4,   Level.Normal),
                new CardInfo( 63, "筝曲「下克上レクイエム」",           Stage.St4,   Level.Hard),
                new CardInfo( 64, "筝曲「下克上レクイエム」",           Stage.St4,   Level.Lunatic),
                new CardInfo( 65, "欺符「逆針撃」",                     Stage.St5,   Level.Easy),
                new CardInfo( 66, "欺符「逆針撃」",                     Stage.St5,   Level.Normal),
                new CardInfo( 67, "欺符「逆針撃」",                     Stage.St5,   Level.Hard),
                new CardInfo( 68, "欺符「逆針撃」",                     Stage.St5,   Level.Lunatic),
                new CardInfo( 69, "逆符「鏡の国の弾幕」",               Stage.St5,   Level.Easy),
                new CardInfo( 70, "逆符「鏡の国の弾幕」",               Stage.St5,   Level.Normal),
                new CardInfo( 71, "逆符「イビルインザミラー」",         Stage.St5,   Level.Hard),
                new CardInfo( 72, "逆符「イビルインザミラー」",         Stage.St5,   Level.Lunatic),
                new CardInfo( 73, "逆符「天地有用」",                   Stage.St5,   Level.Easy),
                new CardInfo( 74, "逆符「天地有用」",                   Stage.St5,   Level.Normal),
                new CardInfo( 75, "逆符「天下転覆」",                   Stage.St5,   Level.Hard),
                new CardInfo( 76, "逆符「天下転覆」",                   Stage.St5,   Level.Lunatic),
                new CardInfo( 77, "逆弓「天壌夢弓」",                   Stage.St5,   Level.Easy),
                new CardInfo( 78, "逆弓「天壌夢弓」",                   Stage.St5,   Level.Normal),
                new CardInfo( 79, "逆弓「天壌夢弓の詔勅」",             Stage.St5,   Level.Hard),
                new CardInfo( 80, "逆弓「天壌夢弓の詔勅」",             Stage.St5,   Level.Lunatic),
                new CardInfo( 81, "逆転「リバースヒエラルキー」",       Stage.St5,   Level.Easy),
                new CardInfo( 82, "逆転「リバースヒエラルキー」",       Stage.St5,   Level.Normal),
                new CardInfo( 83, "逆転「チェンジエアブレイブ」",       Stage.St5,   Level.Hard),
                new CardInfo( 84, "逆転「チェンジエアブレイブ」",       Stage.St5,   Level.Lunatic),
                new CardInfo( 85, "小弾「小人の道」",                   Stage.St6,   Level.Easy),
                new CardInfo( 86, "小弾「小人の道」",                   Stage.St6,   Level.Normal),
                new CardInfo( 87, "小弾「小人の茨道」",                 Stage.St6,   Level.Hard),
                new CardInfo( 88, "小弾「小人の茨道」",                 Stage.St6,   Level.Lunatic),
                new CardInfo( 89, "小槌「大きくなあれ」",               Stage.St6,   Level.Easy),
                new CardInfo( 90, "小槌「大きくなあれ」",               Stage.St6,   Level.Normal),
                new CardInfo( 91, "小槌「もっと大きくなあれ」",         Stage.St6,   Level.Hard),
                new CardInfo( 92, "小槌「もっと大きくなあれ」",         Stage.St6,   Level.Lunatic),
                new CardInfo( 93, "妖剣「輝針剣」",                     Stage.St6,   Level.Easy),
                new CardInfo( 94, "妖剣「輝針剣」",                     Stage.St6,   Level.Normal),
                new CardInfo( 95, "妖剣「輝針剣」",                     Stage.St6,   Level.Hard),
                new CardInfo( 96, "妖剣「輝針剣」",                     Stage.St6,   Level.Lunatic),
                new CardInfo( 97, "小槌「お前が大きくなあれ」",         Stage.St6,   Level.Easy),
                new CardInfo( 98, "小槌「お前が大きくなあれ」",         Stage.St6,   Level.Normal),
                new CardInfo( 99, "小槌「お前が大きくなあれ」",         Stage.St6,   Level.Hard),
                new CardInfo(100, "小槌「お前が大きくなあれ」",         Stage.St6,   Level.Lunatic),
                new CardInfo(101, "「進撃の小人」",                     Stage.St6,   Level.Easy),
                new CardInfo(102, "「進撃の小人」",                     Stage.St6,   Level.Normal),
                new CardInfo(103, "「ウォールオブイッスン」",           Stage.St6,   Level.Hard),
                new CardInfo(104, "「ウォールオブイッスン」",           Stage.St6,   Level.Lunatic),
                new CardInfo(105, "「ホップオマイサムセブン」",         Stage.St6,   Level.Easy),
                new CardInfo(106, "「ホップオマイサムセブン」",         Stage.St6,   Level.Normal),
                new CardInfo(107, "「七人の一寸法師」",                 Stage.St6,   Level.Hard),
                new CardInfo(108, "「七人の一寸法師」",                 Stage.St6,   Level.Lunatic),
                new CardInfo(109, "弦楽「嵐のアンサンブル」",           Stage.Extra, Level.Extra),
                new CardInfo(110, "弦楽「浄瑠璃世界」",                 Stage.Extra, Level.Extra),
                new CardInfo(111, "一鼓「暴れ宮太鼓」",                 Stage.Extra, Level.Extra),
                new CardInfo(112, "二鼓「怨霊アヤノツヅミ」",           Stage.Extra, Level.Extra),
                new CardInfo(113, "三鼓「午前零時のスリーストライク」", Stage.Extra, Level.Extra),
                new CardInfo(114, "死鼓「ランドパーカス」",             Stage.Extra, Level.Extra),
                new CardInfo(115, "五鼓「デンデン太鼓」",               Stage.Extra, Level.Extra),
                new CardInfo(116, "六鼓「オルタネイトスティッキング」", Stage.Extra, Level.Extra),
                new CardInfo(117, "七鼓「高速和太鼓ロケット」",         Stage.Extra, Level.Extra),
                new CardInfo(118, "八鼓「雷神の怒り」",                 Stage.Extra, Level.Extra),
                new CardInfo(119, "「ブルーレディショー」",             Stage.Extra, Level.Extra),
                new CardInfo(120, "「プリスティンビート」",             Stage.Extra, Level.Extra)
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

        public Th14Converter()
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

        public enum LevelPractice
        {
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("X")] Extra,
            [EnumAltName("-")] NotUsed
        }

        public enum LevelPracticeWithTotal
        {
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("X")] Extra,
            [EnumAltName("-")] NotUsed,
            [EnumAltName("T")] Total
        }

        public enum Chara
        {
            [EnumAltName("RA")] ReimuA,
            [EnumAltName("RB")] ReimuB,
            [EnumAltName("MA")] MarisaA,
            [EnumAltName("MB")] MarisaB,
            [EnumAltName("SA")] SakuyaA,
            [EnumAltName("SB")] SakuyaB
        }

        public enum CharaWithTotal
        {
            [EnumAltName("RA")] ReimuA,
            [EnumAltName("RB")] ReimuB,
            [EnumAltName("MA")] MarisaA,
            [EnumAltName("MB")] MarisaB,
            [EnumAltName("SA")] SakuyaA,
            [EnumAltName("SB")] SakuyaB,
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
            using (var decoded = new FileStream("th14decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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
            if (header.Signature != "TH41")
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

                    if (!((chapter.Signature == "CR") && (chapter.Version == 0x0001)) &&
                        !((chapter.Signature == "ST") && (chapter.Version == 0x0001)))
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

        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1513:ClosingCurlyBracketMustBeFollowedByBlankLine", Justification = "Reviewed.")]
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

        // %T14SCR[w][xx][y][z]
        private string ReplaceScore(string input)
        {
            var pattern = Utils.Format(@"%T14SCR([{0}])({1})(\d)([1-5])", LevelPattern, CharaPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = (LevelPracticeWithTotal)ToLevel(match.Groups[1].Value);
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

        // %T14C[w][xxx][yy][z]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
        private string ReplaceCareer(string input)
        {
            var pattern = Utils.Format(@"%T14C([SP])(\d{{3}})({0})([12])", CharaWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var kind = match.Groups[1].Value.ToUpperInvariant();
                var number = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                var chara = ToCharaWithTotal(match.Groups[3].Value);
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                Func<SpellCard, int> getCount = (card => 0);
                if (kind == "S")
                {
                    if (type == 1)
                        getCount = (card => card.ClearCount);
                    else
                        getCount = (card => card.TrialCount);
                }
                else
                {
                    if (type == 1)
                        getCount = (card => card.PracticeClearCount);
                    else
                        getCount = (card => card.PracticeTrialCount);
                }

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

        // %T14CARD[xxx][y]
        private string ReplaceCard(string input, bool hideUntriedCards)
        {
            var pattern = @"%T14CARD(\d{3})([NR])";
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

        // %T14CRG[v][w][xx][y][z]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
        private string ReplaceCollectRate(string input)
        {
            var pattern = Utils.Format(
                @"%T14CRG([SP])([{0}])({1})([{2}])([12])",
                LevelWithTotalPattern,
                CharaWithTotalPattern,
                StageWithTotalPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var kind = match.Groups[1].Value.ToUpperInvariant();
                var level = ToLevelWithTotal(match.Groups[2].Value);
                var chara = ToCharaWithTotal(match.Groups[3].Value);
                var stage = ToStageWithTotal(match.Groups[4].Value);
                var type = int.Parse(match.Groups[5].Value, CultureInfo.InvariantCulture);

                if (stage == StageWithTotal.Extra)
                    return match.ToString();

                Func<SpellCard, bool> findByKindType = (card => true);
                Func<SpellCard, bool> findByLevel = (card => true);
                Func<SpellCard, bool> findByStage = (card => true);

                if (kind == "S")
                {
                    if (type == 1)
                        findByKindType = (card => card.ClearCount > 0);
                    else
                        findByKindType = (card => card.TrialCount > 0);
                }
                else
                {
                    if (type == 1)
                        findByKindType = (card => card.PracticeClearCount > 0);
                    else
                        findByKindType = (card => card.PracticeTrialCount > 0);
                }

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

                var and = Utils.MakeAndPredicate(findByKindType, findByLevel, findByStage);
                return this.allScoreData.ClearData[chara].Cards.Values.Count(and)
                    .ToString(CultureInfo.CurrentCulture);
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T14CLEAR[x][yy]
        private string ReplaceClear(string input)
        {
            var pattern = Utils.Format(@"%T14CLEAR([{0}])({1})", LevelPattern, CharaPattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = (LevelPracticeWithTotal)ToLevel(match.Groups[1].Value);
                var chara = (CharaWithTotal)ToChara(match.Groups[2].Value);

                var rankings = this.allScoreData.ClearData[chara].Rankings[level]
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
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T14CHARA[xx][y]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
        private string ReplaceChara(string input)
        {
            var pattern = Utils.Format(@"%T14CHARA({0})([1-3])", CharaWithTotalPattern);
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

        // %T14CHARAEX[x][yy][z]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
        private string ReplaceCharaEx(string input)
        {
            var pattern = Utils.Format(
                @"%T14CHARAEX([{0}])({1})([1-3])", LevelWithTotalPattern, CharaWithTotalPattern);
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
                        getValueByType = (data => data.ClearCounts[(LevelPracticeWithTotal)level]);
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

        // %T14PRAC[x][yy][z]
        private string ReplacePractice(string input)
        {
            var pattern = Utils.Format(
                @"%T14PRAC([{0}])({1})([{2}])", LevelPattern, CharaPattern, StagePattern);
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

        private class LevelStagePair : Pair<LevelPractice, StagePractice>
        {
            public LevelStagePair(LevelPractice level, StagePractice stage)
                : base(level, stage)
            {
            }

            public LevelStagePair(Level level, Stage stage)
                : base((LevelPractice)level, (StagePractice)stage)
            {
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public LevelPractice Level
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
                if (this.Version != 0x0001)
                    throw new InvalidDataException("Version");
                if (this.Size != 0x00005298)
                    throw new InvalidDataException("Size");

                var numLevels = Enum.GetValues(typeof(LevelPracticeWithTotal)).Length;
                var numPairs = Enum.GetValues(typeof(LevelPractice)).Length *
                    Enum.GetValues(typeof(StagePractice)).Length;
                this.Rankings = new Dictionary<LevelPracticeWithTotal, ScoreData[]>(numLevels);
                this.ClearCounts = new Dictionary<LevelPracticeWithTotal, int>(numLevels);
                this.ClearFlags = new Dictionary<LevelPracticeWithTotal, int>(numLevels);
                this.Practices = new Dictionary<LevelStagePair, Practice>(numPairs);
                this.Cards = new Dictionary<int, SpellCard>(CardTable.Count);
            }

            public CharaWithTotal Chara { get; private set; }   // size: 4Bytes

            public Dictionary<LevelPracticeWithTotal, ScoreData[]> Rankings { get; private set; }

            public int TotalPlayCount { get; private set; }

            public int PlayTime { get; private set; }           // = seconds * 60fps

            public Dictionary<LevelPracticeWithTotal, int> ClearCounts { get; private set; }

            public Dictionary<LevelPracticeWithTotal, int> ClearFlags { get; private set; }     // Really...?

            public Dictionary<LevelStagePair, Practice> Practices { get; private set; }

            public Dictionary<int, SpellCard> Cards { get; private set; }

            public override void ReadFrom(BinaryReader reader)
            {
                var levelsPracticeWithTotal = Utils.GetEnumerator<LevelPracticeWithTotal>();
                var stages = Utils.GetEnumerator<StagePractice>();

                this.Chara = (CharaWithTotal)reader.ReadInt32();

                foreach (var level in levelsPracticeWithTotal)
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

                foreach (var level in levelsPracticeWithTotal)
                {
                    var clearCount = reader.ReadInt32();
                    if (!this.ClearCounts.ContainsKey(level))
                        this.ClearCounts.Add(level, clearCount);
                }

                foreach (var level in levelsPracticeWithTotal)
                {
                    var clearFlag = reader.ReadInt32();
                    if (!this.ClearFlags.ContainsKey(level))
                        this.ClearFlags.Add(level, clearFlag);
                }

                foreach (var level in Utils.GetEnumerator<LevelPractice>())
                    foreach (var stage in stages)
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
                if (this.Version != 0x0001)
                    throw new InvalidDataException("Version");
                if (this.Size != 0x0000042C)
                    throw new InvalidDataException("Size");
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] LastName { get; private set; }    // .Length = 10 (The last 2 bytes are always 0x00 ?)

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] BgmFlags { get; private set; }    // .Length = 17

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int TotalPlayTime { get; private set; }  // unit: 10ms

            public override void ReadFrom(BinaryReader reader)
            {
                this.LastName = reader.ReadBytes(10);
                reader.ReadBytes(0x10);
                this.BgmFlags = reader.ReadBytes(17);
                reader.ReadBytes(0x11);
                this.TotalPlayTime = reader.ReadInt32();
                reader.ReadBytes(0x03E0);
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
            public byte[] Name { get; private set; }            // .Length = 0x80

            public int ClearCount { get; private set; }

            public int PracticeClearCount { get; private set; }

            public int TrialCount { get; private set; }

            public int PracticeTrialCount { get; private set; }

            public int Number { get; private set; }             // 1-based

            public Level Level { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public int PracticeScore { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                this.Name = reader.ReadBytes(0x80);
                this.ClearCount = reader.ReadInt32();
                this.PracticeClearCount = reader.ReadInt32();
                this.TrialCount = reader.ReadInt32();
                this.PracticeTrialCount = reader.ReadInt32();
                this.Number = reader.ReadInt32() + 1;
                this.Level = (Level)reader.ReadInt32();
                this.PracticeScore = reader.ReadInt32();
            }

            public bool HasTried()
            {
                return (this.TrialCount > 0) || (this.PracticeTrialCount > 0);
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
                this.Score = reader.ReadUInt32();
                this.ClearFlag = reader.ReadByte();
                this.EnableFlag = reader.ReadByte();
                reader.ReadUInt16();    // always 0x0000?
            }
        }
    }
}
