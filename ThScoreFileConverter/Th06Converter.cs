//-----------------------------------------------------------------------
// <copyright file="Th06Converter.cs" company="None">
//     (c) 2013-2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed.")]

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
    using CardInfo = SpellCardInfo<ThConverter.Stage, ThConverter.Level>;

    internal class Th06Converter : ThConverter
    {
        private static readonly Dictionary<int, CardInfo> CardTable;
        private static readonly List<HighScore> InitialRanking;

        private static readonly EnumShortNameParser<Chara> CharaParser;

        private AllScoreData allScoreData = null;

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly", Justification = "Reviewed.")]
        static Th06Converter()
        {
            // Thanks to thwiki.info
            var cardList = new List<CardInfo>()
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
                new CardInfo(64, "ＱＥＤ「４９５年の波紋」",         Stage.Extra, Level.Extra)
            };
            CardTable = cardList.ToDictionary(card => card.Id);

            InitialRanking = new List<HighScore>()
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
                new HighScore( 100000)
            };

            CharaParser = new EnumShortNameParser<Chara>();
        }

        public Th06Converter()
        {
        }

        public enum Chara
        {
            [EnumAltName("RA")] ReimuA,
            [EnumAltName("RB")] ReimuB,
            [EnumAltName("MA")] MarisaA,
            [EnumAltName("MB")] MarisaB
        }

        public enum CharaWithTotal
        {
            [EnumAltName("RA")] ReimuA,
            [EnumAltName("RB")] ReimuB,
            [EnumAltName("MA")] MarisaA,
            [EnumAltName("MB")] MarisaB,
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
            [EnumAltName("All Clear")]   Clear = 99
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
            allLines = this.ReplacePractice(allLines);

            writer.Write(allLines);
            writer.Flush();
            writer.BaseStream.SetLength(writer.BaseStream.Position);
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
            var reader = new BinaryReader(input);
            var writer = new BinaryWriter(output);
            var header = new FileHeader();

            header.ReadFrom(reader);
            if (!header.IsValid)
                return false;
            if (header.DecodedAllSize != input.Length)
                return false;

            header.WriteTo(writer);

            // Lzss.Extract(input, output);
            var body = new byte[header.DecodedAllSize - header.Size];
            input.Read(body, 0, body.Length);
            output.Write(body, 0, body.Length);
            output.Flush();
            output.SetLength(output.Position);

            return output.Position == header.DecodedAllSize;
        }

        private static bool Validate(Stream input)
        {
            var reader = new BinaryReader(input);
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

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:CodeMustNotContainMultipleWhitespaceInARow", Justification = "Reviewed.")]
        private static AllScoreData Read(Stream input)
        {
            var dictionary = new Dictionary<string, Action<AllScoreData, Chapter>>
            {
                { Header.ValidSignature,        (data, ch) => data.Set(new Header(ch))        },
                { HighScore.ValidSignature,     (data, ch) => data.Set(new HighScore(ch))     },
                { ClearData.ValidSignature,     (data, ch) => data.Set(new ClearData(ch))     },
                { CardAttack.ValidSignature,    (data, ch) => data.Set(new CardAttack(ch))    },
                { PracticeScore.ValidSignature, (data, ch) => data.Set(new PracticeScore(ch)) }
            };

            var reader = new BinaryReader(input);
            var allScoreData = new AllScoreData();
            var chapter = new Chapter();

            reader.ReadBytes(FileHeader.ValidSize);

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
                //// (allScoreData.rankings.Count >= 0) &&
                //// (allScoreData.cardAttacks.Length == NumCards) &&
                //// (allScoreData.practiceScores.Count >= 0) &&
                (allScoreData.ClearData.Count == Enum.GetValues(typeof(Chara)).Length))
                return allScoreData;
            else
                return null;
        }

        // %T06SCR[w][xx][y][z]
        private string ReplaceScore(string input)
        {
            var pattern = Utils.Format(
                @"%T06SCR({0})({1})(\d)([1-3])", LevelParser.Pattern, CharaParser.Pattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = LevelParser.Parse(match.Groups[1].Value);
                var chara = CharaParser.Parse(match.Groups[2].Value);
                var rank = Utils.ToZeroBased(int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                var key = new CharaLevelPair(chara, level);
                var score = this.allScoreData.Rankings.ContainsKey(key)
                    ? this.allScoreData.Rankings[key][rank] : InitialRanking[rank];

                switch (type)
                {
                    case 1:     // name
                        return Encoding.Default.GetString(score.Name).Split('\0')[0];
                    case 2:     // score
                        return this.ToNumberString(score.Score);
                    case 3:     // stage
                        return score.StageProgress.ToShortName();
                    default:    // unreachable
                        return match.ToString();
                }
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T06C[xx][y]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
        private string ReplaceCareer(string input)
        {
            var pattern = @"%T06C(\d{2})([12])";
            var evaluator = new MatchEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var type = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                Func<CardAttack, int> getCount = (attack => 0);
                if (type == 1)
                    getCount = (attack => attack.ClearCount);
                else
                    getCount = (attack => attack.TrialCount);

                if (number == 0)
                    return this.ToNumberString(this.allScoreData.CardAttacks.Values.Sum(getCount));
                else if (CardTable.ContainsKey(number))
                {
                    CardAttack attack;
                    if (this.allScoreData.CardAttacks.TryGetValue(number, out attack))
                        return this.ToNumberString(getCount(attack));
                    else
                        return "0";
                }
                else
                    return match.ToString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T06CARD[xx][y]
        private string ReplaceCard(string input, bool hideUntriedCards)
        {
            var pattern = @"%T06CARD(\d{2})([NR])";
            var evaluator = new MatchEvaluator(match =>
            {
                var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var type = match.Groups[2].Value.ToUpperInvariant();

                if (CardTable.ContainsKey(number))
                {
                    if (hideUntriedCards)
                    {
                        CardAttack attack;
                        if (!this.allScoreData.CardAttacks.TryGetValue(number, out attack) ||
                            !attack.HasTried())
                            return (type == "N") ? "??????????" : "?????";
                    }

                    return (type == "N")
                        ? CardTable[number].Name
                        : string.Join(", ", CardTable[number].Levels.Select(lv => lv.ToString()).ToArray());
                }
                else
                    return match.ToString();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T06CRG[x][y]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
        private string ReplaceCollectRate(string input)
        {
            var pattern = Utils.Format(@"%T06CRG({0})([12])", StageWithTotalParser.Pattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var stage = StageWithTotalParser.Parse(match.Groups[1].Value);
                var type = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                Func<CardAttack, bool> findByStage = (attack => true);
                Func<CardAttack, bool> findByType = (attack => true);

                if (stage == StageWithTotal.Total)
                {
                    // Do nothing
                }
                else
                    findByStage = (attack => CardTable[attack.CardId].Stage == (Stage)stage);

                if (type == 1)
                    findByType = (attack => attack.ClearCount > 0);
                else
                    findByType = (attack => attack.TrialCount > 0);

                var and = Utils.MakeAndPredicate(findByStage, findByType);
                return this.allScoreData.CardAttacks.Values.Count(and).ToString(CultureInfo.CurrentCulture);
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T06CLEAR[x][yy]
        private string ReplaceClear(string input)
        {
            var pattern = Utils.Format(@"%T06CLEAR({0})({1})", LevelParser.Pattern, CharaParser.Pattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = LevelParser.Parse(match.Groups[1].Value);
                var chara = CharaParser.Parse(match.Groups[2].Value);

                var key = new CharaLevelPair(chara, level);
                if (this.allScoreData.Rankings.ContainsKey(key))
                {
                    var stageProgress = this.allScoreData.Rankings[key].Max(rank => rank.StageProgress);
                    if (stageProgress == StageProgress.Extra)
                        return "Not Clear";
                    else
                        return stageProgress.ToShortName();
                }
                else
                    return StageProgress.None.ToShortName();
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        // %T06PRAC[x][yy][z]
        private string ReplacePractice(string input)
        {
            var pattern = Utils.Format(
                @"%T06PRAC({0})({1})({2})", LevelParser.Pattern, CharaParser.Pattern, StageParser.Pattern);
            var evaluator = new MatchEvaluator(match =>
            {
                var level = LevelParser.Parse(match.Groups[1].Value);
                var chara = CharaParser.Parse(match.Groups[2].Value);
                var stage = StageParser.Parse(match.Groups[3].Value);

                if (level == Level.Extra)
                    return match.ToString();
                if (stage == Stage.Extra)
                    return match.ToString();

                var key = new CharaLevelPair(chara, level);
                if (this.allScoreData.PracticeScores.ContainsKey(key))
                {
                    var scores = this.allScoreData.PracticeScores[key];
                    return scores.ContainsKey(stage)
                        ? this.ToNumberString(scores[stage].HighScore) : "0";
                }
                else
                    return "0";
            });
            return new Regex(pattern, RegexOptions.IgnoreCase).Replace(input, evaluator);
        }

        private class CharaLevelPair : Pair<Chara, Level>
        {
            public CharaLevelPair(Chara chara, Level level)
                : base(chara, level)
            {
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Chara Chara
            {
                get { return this.First; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Level Level
            {
                get { return this.Second; }
            }
        }

        private class FileHeader : IBinaryReadable, IBinaryWritable
        {
            public const short ValidVersion = 0x0010;
            public const int ValidSize = 0x00000014;

            private ushort unknown1;
            private ushort unknown2;
            private uint unknown3;

            public FileHeader()
            {
            }

            public ushort Checksum { get; private set; }

            public short Version { get; private set; }

            public int Size { get; private set; }

            public int DecodedAllSize { get; private set; }

            public bool IsValid
            {
                get { return (this.Version == ValidVersion) && (this.Size == ValidSize); }
            }

            public void ReadFrom(BinaryReader reader)
            {
                this.unknown1 = reader.ReadUInt16();
                this.Checksum = reader.ReadUInt16();
                this.Version = reader.ReadInt16();
                this.unknown2 = reader.ReadUInt16();
                this.Size = reader.ReadInt32();
                this.unknown3 = reader.ReadUInt32();
                this.DecodedAllSize = reader.ReadInt32();
            }

            public void WriteTo(BinaryWriter writer)
            {
                writer.Write(this.unknown1);
                writer.Write(this.Checksum);
                writer.Write(this.Version);
                writer.Write(this.unknown2);
                writer.Write(this.Size);
                writer.Write(this.unknown3);
                writer.Write(this.DecodedAllSize);
            }
        }

        private class AllScoreData
        {
            public AllScoreData()
            {
                var numCharas = Enum.GetValues(typeof(Chara)).Length;
                var numPairs = numCharas * Enum.GetValues(typeof(Level)).Length;
                this.Rankings = new Dictionary<CharaLevelPair, List<HighScore>>(numPairs);
                this.ClearData = new Dictionary<Chara, ClearData>(numCharas);
                this.CardAttacks = new Dictionary<int, CardAttack>(CardTable.Count);
                this.PracticeScores =
                    new Dictionary<CharaLevelPair, Dictionary<Stage, PracticeScore>>(numPairs);
            }

            public Header Header { get; private set; }

            public Dictionary<CharaLevelPair, List<HighScore>> Rankings { get; private set; }

            public Dictionary<Chara, ClearData> ClearData { get; private set; }

            public Dictionary<int, CardAttack> CardAttacks { get; private set; }

            public Dictionary<CharaLevelPair, Dictionary<Stage, PracticeScore>> PracticeScores
            {
                get; private set;
            }

            public void Set(Header header)
            {
                this.Header = header;
            }

            public void Set(HighScore score)
            {
                var key = new CharaLevelPair(score.Chara, score.Level);
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
                    var key = new CharaLevelPair(score.Chara, score.Level);
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

        private class Chapter : IBinaryReadable
        {
            public Chapter()
            {
                this.Signature = string.Empty;
                this.Size1 = 0;
                this.Size2 = 0;
                this.Data = new byte[] { };
            }

            protected Chapter(Chapter chapter)
            {
                this.Signature = chapter.Signature;
                this.Size1 = chapter.Size1;
                this.Size2 = chapter.Size2;
                this.Data = new byte[chapter.Data.Length];
                chapter.Data.CopyTo(this.Data, 0);
            }

            public string Signature { get; private set; }   // .Length = 4

            public short Size1 { get; private set; }

            public short Size2 { get; private set; }        // always equal to size1?

            public byte FirstByteOfData
            {
                get { return this.Data[0]; }
            }

            protected byte[] Data { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                this.Signature = Encoding.Default.GetString(reader.ReadBytes(4));
                this.Size1 = reader.ReadInt16();
                this.Size2 = reader.ReadInt16();
                this.Data = reader.ReadBytes(this.Size1 - this.Signature.Length - (sizeof(short) * 2));
            }
        }

        private class Header : Chapter
        {
            public const string ValidSignature = "TH6K";
            public const short ValidSize = 0x000C;

            public Header(Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException("Signature");
                if (this.Size1 != ValidSize)
                    throw new InvalidDataException("Size1");

                using (var stream = new MemoryStream(this.Data, false))
                using (var reader = new BinaryReader(stream))
                {
                    reader.ReadUInt32();    // always 0x00000010?
                }
            }
        }

        private class HighScore : Chapter   // per character, level, rank
        {
            public const string ValidSignature = "HSCR";
            public const short ValidSize = 0x001C;

            public HighScore(Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException("Signature");
                if (this.Size1 != ValidSize)
                    throw new InvalidDataException("Size1");

                using (var stream = new MemoryStream(this.Data, false))
                using (var reader = new BinaryReader(stream))
                {
                    reader.ReadUInt32();    // always 0x00000001?
                    this.Score = reader.ReadUInt32();
                    this.Chara = (Chara)reader.ReadByte();
                    this.Level = (Level)reader.ReadByte();
                    this.StageProgress = (StageProgress)reader.ReadByte();
                    this.Name = reader.ReadBytes(9);
                }
            }

            public HighScore(uint score)    // for InitialRanking only
                : base()
            {
                this.Score = score;
                this.Name = Encoding.Default.GetBytes("Nanashi\0\0");
            }

            public uint Score { get; private set; }

            public Chara Chara { get; private set; }                    // size: 1Byte

            public Level Level { get; private set; }                    // size: 1Byte

            public StageProgress StageProgress { get; private set; }    // size: 1Byte

            public byte[] Name { get; private set; }                    // .Length = 9, null-terminated
        }

        private class ClearData : Chapter   // per character
        {
            public const string ValidSignature = "CLRD";
            public const short ValidSize = 0x0018;

            public ClearData(Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException("Signature");
                if (this.Size1 != ValidSize)
                    throw new InvalidDataException("Size1");

                var levels = Utils.GetEnumerator<Level>();
                var numLevels = levels.Count();
                this.StoryFlags = new Dictionary<Level, byte>(numLevels);
                this.PracticeFlags = new Dictionary<Level, byte>(numLevels);

                using (var stream = new MemoryStream(this.Data, false))
                using (var reader = new BinaryReader(stream))
                {
                    reader.ReadUInt32();    // always 0x00000010?
                    foreach (var level in levels)
                        this.StoryFlags.Add(level, reader.ReadByte());
                    foreach (var level in levels)
                        this.PracticeFlags.Add(level, reader.ReadByte());
                    this.Chara = (Chara)reader.ReadInt16();
                }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Dictionary<Level, byte> StoryFlags { get; private set; }     // really...?

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public Dictionary<Level, byte> PracticeFlags { get; private set; }  // really...?

            public Chara Chara { get; private set; }            // size: 2Bytes
        }

        private class CardAttack : Chapter      // per card
        {
            public const string ValidSignature = "CATK";
            public const short ValidSize = 0x0040;

            public CardAttack(Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException("Signature");
                if (this.Size1 != ValidSize)
                    throw new InvalidDataException("Size1");

                using (var stream = new MemoryStream(this.Data, false))
                using (var reader = new BinaryReader(stream))
                {
                    reader.ReadBytes(8);
                    this.CardId = (short)(reader.ReadInt16() + 1);
                    reader.ReadBytes(6);
                    this.CardName = reader.ReadBytes(0x24);
                    this.TrialCount = reader.ReadUInt16();
                    this.ClearCount = reader.ReadUInt16();
                }
            }

            public short CardId { get; private set; }       // 1-based

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For future use.")]
            public byte[] CardName { get; private set; }    // .Length = 0x24, null-terminated

            public ushort TrialCount { get; private set; }

            public ushort ClearCount { get; private set; }

            public bool HasTried()
            {
                return this.TrialCount > 0;
            }
        }

        private class PracticeScore : Chapter   // per character, level, stage
        {
            public const string ValidSignature = "PSCR";
            public const short ValidSize = 0x0014;

            public PracticeScore(Chapter chapter)
                : base(chapter)
            {
                if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                    throw new InvalidDataException("Signature");
                if (this.Size1 != ValidSize)
                    throw new InvalidDataException("Size1");

                using (var stream = new MemoryStream(this.Data, false))
                using (var reader = new BinaryReader(stream))
                {
                    reader.ReadUInt32();    // always 0x00000010?
                    this.HighScore = reader.ReadInt32();
                    this.Chara = (Chara)reader.ReadByte();
                    this.Level = (Level)reader.ReadByte();
                    this.Stage = (Stage)reader.ReadByte();
                    reader.ReadByte();
                }
            }

            public int HighScore { get; private set; }

            public Chara Chara { get; private set; }        // size: 1Byte

            public Level Level { get; private set; }        // size: 1Byte

            public Stage Stage { get; private set; }        // size: 1Byte
        }
    }
}
