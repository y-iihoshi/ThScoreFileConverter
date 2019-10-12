//-----------------------------------------------------------------------
// <copyright file="Th08Converter.cs" company="None">
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
using ThScoreFileConverter.Models.Th08;
using static ThScoreFileConverter.Models.Th08.Parsers;
using CardInfo = ThScoreFileConverter.Models.SpellCardInfo<
    ThScoreFileConverter.Models.Th08.StagePractice, ThScoreFileConverter.Models.Th08.LevelPractice>;
using IHighScore = ThScoreFileConverter.Models.Th08.IHighScore<
    ThScoreFileConverter.Models.Th08.Chara,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th08.StageProgress>;

namespace ThScoreFileConverter.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th08Converter : ThConverter
    {
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008:OpeningParenthesisMustBeSpacedCorrectly", Justification = "Reviewed.")]
        private static readonly List<IHighScore> InitialRanking =
            new List<IHighScore>()
            {
                new HighScore(100000),
                new HighScore( 90000),
                new HighScore( 80000),
                new HighScore( 70000),
                new HighScore( 60000),
                new HighScore( 50000),
                new HighScore( 40000),
                new HighScore( 30000),
                new HighScore( 20000),
                new HighScore( 10000),
            };

        private AllScoreData allScoreData = null;

        public override string SupportedVersions
        {
            get { return "1.00d"; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th08decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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
                new PlayReplacer(this),
                new TimeReplacer(this),
                new PracticeReplacer(this),
            };
        }

        private static bool Decrypt(Stream input, Stream output)
        {
            var size = (int)input.Length;
            ThCrypt.Decrypt(input, output, size, 0x59, 0x79, 0x0100, 0x0C00);

            var data = new byte[size];
            output.Seek(0, SeekOrigin.Begin);
            output.Read(data, 0, size);

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

            output.Seek(0, SeekOrigin.Begin);
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
                if (header.Size + header.EncodedBodySize != input.Length)
                    return false;

                header.WriteTo(writer);

                Lzss.Extract(input, output);
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
                var chapter = new Th06.Chapter();

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
                                if (chapter.FirstByteOfData != 0x01)
                                    return false;
                                break;
                            case Th07.VersionInfo.ValidSignature:
                                if (chapter.FirstByteOfData != 0x01)
                                    return false;
                                //// th08.exe does something more things here...
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
            var dictionary = new Dictionary<string, Action<AllScoreData, Th06.Chapter>>
            {
                { Header.ValidSignature,           (data, ch) => data.Set(new Header(ch))           },
                { HighScore.ValidSignature,        (data, ch) => data.Set(new HighScore(ch))        },
                { ClearData.ValidSignature,        (data, ch) => data.Set(new ClearData(ch))        },
                { CardAttack.ValidSignature,       (data, ch) => data.Set(new CardAttack(ch))       },
                { PracticeScore.ValidSignature,    (data, ch) => data.Set(new PracticeScore(ch))    },
                { FLSP.ValidSignature,             (data, ch) => data.Set(new FLSP(ch))             },
                { PlayStatus.ValidSignature,       (data, ch) => data.Set(new PlayStatus(ch))       },
                { Th07.LastName.ValidSignature,    (data, ch) => data.Set(new Th07.LastName(ch))    },
                { Th07.VersionInfo.ValidSignature, (data, ch) => data.Set(new Th07.VersionInfo(ch)) },
            };

            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            {
                var allScoreData = new AllScoreData();
                var chapter = new Th06.Chapter();

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
                    (allScoreData.ClearData.Count == Enum.GetValues(typeof(CharaWithTotal)).Length) &&
                    //// (allScoreData.cardAttacks.Length == NumCards) &&
                    //// (allScoreData.practiceScores.Count >= 0) &&
                    (allScoreData.Flsp != null) &&
                    (allScoreData.PlayStatus != null) &&
                    (allScoreData.LastName != null) &&
                    (allScoreData.VersionInfo != null))
                    return allScoreData;
                else
                    return null;
            }
        }

        // %T08SCR[w][xx][y][z]
        private class ScoreReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T08SCR({0})({1})(\d)([\dA-G])", LevelParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ScoreReplacer(Th08Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var chara = CharaParser.Parse(match.Groups[2].Value);
                    var rank = Utils.ToZeroBased(
                        int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                    var type = match.Groups[4].Value.ToUpperInvariant();

                    var key = (chara, level);
                    var score = parent.allScoreData.Rankings.ContainsKey(key)
                        ? parent.allScoreData.Rankings[key][rank] : InitialRanking[rank];
                    IEnumerable<string> cardStrings;

                    switch (type)
                    {
                        case "1":   // name
                            return Encoding.Default.GetString(score.Name.ToArray()).Split('\0')[0];
                        case "2":   // score
                            return Utils.ToNumberString((score.Score * 10) + score.ContinueCount);
                        case "3":   // stage
                            if ((level == Level.Extra) &&
                                (Encoding.Default.GetString(score.Date.ToArray()).TrimEnd('\0') == "--/--"))
                                return StageProgress.Extra.ToShortName();
                            else
                                return score.StageProgress.ToShortName();
                        case "4":   // date
                            return Encoding.Default.GetString(score.Date.ToArray()).TrimEnd('\0');
                        case "5":   // slow rate
                            return Utils.Format("{0:F3}%", score.SlowRate);
                        case "6":   // play time
                            return new Time(score.PlayTime).ToString();
                        case "7":   // initial number of players
                            return (score.PlayerNum + 1).ToString(CultureInfo.CurrentCulture);
                        case "8":   // point items
                            return Utils.ToNumberString(score.PointItem);
                        case "9":   // time point
                            return Utils.ToNumberString(score.TimePoint);
                        case "0":   // miss count
                            return score.MissCount.ToString(CultureInfo.CurrentCulture);
                        case "A":   // bomb count
                            return score.BombCount.ToString(CultureInfo.CurrentCulture);
                        case "B":   // last spell count
                            return score.LastSpellCount.ToString(CultureInfo.CurrentCulture);
                        case "C":   // pause count
                            return Utils.ToNumberString(score.PauseCount);
                        case "D":   // continue count
                            return score.ContinueCount.ToString(CultureInfo.CurrentCulture);
                        case "E":   // human rate
                            return Utils.Format("{0:F2}%", score.HumanRate / 100.0);
                        case "F":   // got spell cards
                            cardStrings = score.CardFlags
                                .Where(pair => pair.Value > 0)
                                .Select(pair =>
                                {
                                    return Definitions.CardTable.TryGetValue(pair.Key, out CardInfo card)
                                        ? Utils.Format("No.{0:D3} {1}", card.Id, card.Name) : string.Empty;
                                });
                            return string.Join(Environment.NewLine, cardStrings.ToArray());
                        case "G":   // number of got spell cards
                            return score.CardFlags.Values.Count(flag => flag > 0)
                                .ToString(CultureInfo.CurrentCulture);
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

        // %T08C[w][xxx][yy][z]
        private class CareerReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T08C([SP])(\d{{3}})({0})([1-3])", CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CareerReplacer(Th08Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var kind = match.Groups[1].Value.ToUpperInvariant();
                    var number = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    var chara = CharaWithTotalParser.Parse(match.Groups[3].Value);
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    Func<ICardAttack, bool> isValidLevel;
                    Func<ICardAttack, ICardAttackCareer> getCareer;
                    if (kind == "S")
                    {
                        isValidLevel = (attack => Definitions.CardTable[attack.CardId].Level != LevelPractice.LastWord);
                        getCareer = (attack => attack.StoryCareer);
                    }
                    else
                    {
                        isValidLevel = (attack => true);
                        getCareer = (attack => attack.PracticeCareer);
                    }

                    Func<ICardAttack, long> getValue;
                    if (type == 1)
                        getValue = (attack => getCareer(attack).MaxBonuses[chara]);
                    else if (type == 2)
                        getValue = (attack => getCareer(attack).ClearCounts[chara]);
                    else
                        getValue = (attack => getCareer(attack).TrialCounts[chara]);

                    if (number == 0)
                    {
                        return Utils.ToNumberString(
                            parent.allScoreData.CardAttacks.Values.Where(isValidLevel).Sum(getValue));
                    }
                    else if (Definitions.CardTable.ContainsKey(number))
                    {
                        if (parent.allScoreData.CardAttacks.TryGetValue(number, out var attack))
                        {
                            return isValidLevel(attack)
                                ? Utils.ToNumberString(getValue(attack)) : match.ToString();
                        }
                        else
                        {
                            return "0";
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

        // %T08CARD[xxx][y]
        private class CardReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T08CARD(\d{3})([NR])";

            private readonly MatchEvaluator evaluator;

            public CardReplacer(Th08Converter parent, bool hideUntriedCards)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var type = match.Groups[2].Value.ToUpperInvariant();

                    if (Definitions.CardTable.ContainsKey(number))
                    {
                        if (hideUntriedCards)
                        {
                            if (!parent.allScoreData.CardAttacks.TryGetValue(number, out var attack) ||
                                !attack.HasTried())
                                return (type == "N") ? "??????????" : "?????";
                        }

                        if (type == "N")
                        {
                            return Definitions.CardTable[number].Name;
                        }
                        else
                        {
                            var level = Definitions.CardTable[number].Level;
                            var levelName = level.ToLongName();
                            return (levelName.Length > 0) ? levelName : level.ToString();
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

        // %T08CRG[v][w][xx][yy][z]
        private class CollectRateReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T08CRG([SP])({0})({1})({2})([12])",
                LevelPracticeWithTotalParser.Pattern,
                CharaWithTotalParser.Pattern,
                Parsers.StageWithTotalParser.Pattern);

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            private static readonly Func<ICardAttack, CharaWithTotal, string, int, bool> FindByKindTypeImpl =
                (attack, chara, kind, type) =>
                {
                    Func<ICardAttackCareer, int> getCount;
                    if (type == 1)
                        getCount = (career => career.ClearCounts[chara]);
                    else
                        getCount = (career => career.TrialCounts[chara]);

                    if (kind == "S")
                    {
                        return (Definitions.CardTable[attack.CardId].Level != LevelPractice.LastWord)
                            && (getCount(attack.StoryCareer) > 0);
                    }
                    else
                    {
                        return getCount(attack.PracticeCareer) > 0;
                    }
                };

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CollectRateReplacer(Th08Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var kind = match.Groups[1].Value.ToUpperInvariant();
                    var level = LevelPracticeWithTotalParser.Parse(match.Groups[2].Value);
                    var chara = CharaWithTotalParser.Parse(match.Groups[3].Value);
                    var stage = Parsers.StageWithTotalParser.Parse(match.Groups[4].Value);
                    var type = int.Parse(match.Groups[5].Value, CultureInfo.InvariantCulture);

                    if (stage == Th08.StageWithTotal.Extra)
                        return match.ToString();
                    if ((kind == "S") && (level == LevelPracticeWithTotal.LastWord))
                        return match.ToString();

                    Func<ICardAttack, bool> findByKindType =
                        (attack => FindByKindTypeImpl(attack, chara, kind, type));

                    Func<ICardAttack, bool> findByStage;
                    if (stage == Th08.StageWithTotal.Total)
                        findByStage = (attack => true);
                    else
                        findByStage = (attack => Definitions.CardTable[attack.CardId].Stage == (StagePractice)stage);

                    Func<ICardAttack, bool> findByLevel = (attack => true);
                    switch (level)
                    {
                        case LevelPracticeWithTotal.Total:
                            // Do nothing
                            break;
                        case LevelPracticeWithTotal.Extra:
                            findByStage =
                                (attack => Definitions.CardTable[attack.CardId].Stage == StagePractice.Extra);
                            break;
                        case LevelPracticeWithTotal.LastWord:
                            findByStage =
                                (attack => Definitions.CardTable[attack.CardId].Stage == StagePractice.LastWord);
                            break;
                        default:
                            findByLevel = (attack => attack.Level == level);
                            break;
                    }

                    return parent.allScoreData.CardAttacks.Values
                        .Count(Utils.MakeAndPredicate(findByKindType, findByLevel, findByStage))
                        .ToString(CultureInfo.CurrentCulture);
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T08CLEAR[x][yy]
        private class ClearReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T08CLEAR({0})({1})", LevelParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ClearReplacer(Th08Converter parent)
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
                        if ((stageProgress == StageProgress.FourUncanny) ||
                            (stageProgress == StageProgress.FourPowerful))
                        {
                            return "Stage 4";
                        }
                        else if (stageProgress == StageProgress.Extra)
                        {
                            return "Not Clear";
                        }
                        else if (stageProgress == StageProgress.Clear)
                        {
                            if ((level != Level.Extra) &&
                                ((parent.allScoreData.ClearData[(CharaWithTotal)chara].StoryFlags[level]
                                    & PlayableStages.Stage6B) != PlayableStages.Stage6B))
                                return "FinalA Clear";
                            else
                                return stageProgress.ToShortName();
                        }
                        else
                        {
                            return stageProgress.ToShortName();
                        }
                    }
                    else
                    {
                        return "-------";
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T08PLAY[x][yy]
        private class PlayReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T08PLAY({0})({1}|CL|CN|PR)", LevelWithTotalParser.Pattern, CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public PlayReplacer(Th08Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelWithTotalParser.Parse(match.Groups[1].Value);
                    var charaAndMore = match.Groups[2].Value.ToUpperInvariant();

                    var playCount = (level == LevelWithTotal.Total)
                        ? parent.allScoreData.PlayStatus.TotalPlayCount
                        : parent.allScoreData.PlayStatus.PlayCounts[(Level)level];

                    switch (charaAndMore)
                    {
                        case "CL":  // clear count
                            return Utils.ToNumberString(playCount.TotalClear);
                        case "CN":  // continue count
                            return Utils.ToNumberString(playCount.TotalContinue);
                        case "PR":  // practice count
                            return Utils.ToNumberString(playCount.TotalPractice);
                        default:
                            {
                                var chara = CharaWithTotalParser.Parse(match.Groups[2].Value);
                                return Utils.ToNumberString((chara == CharaWithTotal.Total)
                                    ? playCount.TotalTrial : playCount.Trials[(Chara)chara]);
                            }
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T08TIME(ALL|PLY)
        private class TimeReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T08TIME(ALL|PLY)";

            private readonly MatchEvaluator evaluator;

            public TimeReplacer(Th08Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var kind = match.Groups[1].Value.ToUpperInvariant();

                    return (kind == "ALL")
                        ? parent.allScoreData.PlayStatus.TotalRunningTime.ToLongString()
                        : parent.allScoreData.PlayStatus.TotalPlayTime.ToLongString();
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T08PRAC[w][xx][yy][z]
        private class PracticeReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T08PRAC({0})({1})({2})([12])",
                LevelParser.Pattern,
                CharaParser.Pattern,
                Parsers.StageParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public PracticeReplacer(Th08Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var chara = CharaParser.Parse(match.Groups[2].Value);
                    var stage = Parsers.StageParser.Parse(match.Groups[3].Value);
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    if (level == Level.Extra)
                        return match.ToString();
                    if (stage == Th08.Stage.Extra)
                        return match.ToString();

                    if (parent.allScoreData.PracticeScores.ContainsKey(chara))
                    {
                        var scores = parent.allScoreData.PracticeScores[chara];
                        var key = (stage, level);
                        if (type == 1)
                        {
                            return scores.HighScores.ContainsKey(key)
                                ? Utils.ToNumberString(scores.HighScores[key] * 10) : "0";
                        }
                        else
                        {
                            return scores.PlayCounts.ContainsKey(key)
                                ? Utils.ToNumberString(scores.PlayCounts[key]) : "0";
                        }
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
            private readonly Dictionary<(Chara, Level), IReadOnlyList<IHighScore<Chara, Level, StageProgress>>> rankings;
            private readonly Dictionary<CharaWithTotal, IClearData> clearData;
            private readonly Dictionary<int, ICardAttack> cardAttacks;
            private readonly Dictionary<Chara, IPracticeScore> practiceScores;

            public AllScoreData()
            {
                var numCharas = Enum.GetValues(typeof(Chara)).Length;
                var numPairs = numCharas * Enum.GetValues(typeof(Level)).Length;
                this.rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>(numPairs);
                this.clearData =
                    new Dictionary<CharaWithTotal, IClearData>(Enum.GetValues(typeof(CharaWithTotal)).Length);
                this.cardAttacks = new Dictionary<int, ICardAttack>(Definitions.CardTable.Count);
                this.practiceScores = new Dictionary<Chara, IPracticeScore>(numCharas);
            }

            public Header Header { get; private set; }

            public IReadOnlyDictionary<(Chara, Level), IReadOnlyList<IHighScore>> Rankings => this.rankings;

            public IReadOnlyDictionary<CharaWithTotal, IClearData> ClearData => this.clearData;

            public IReadOnlyDictionary<int, ICardAttack> CardAttacks => this.cardAttacks;

            public IReadOnlyDictionary<Chara, IPracticeScore> PracticeScores => this.practiceScores;

            public FLSP Flsp { get; private set; }

            public IPlayStatus PlayStatus { get; private set; }

            public Th07.LastName LastName { get; private set; }

            public Th07.VersionInfo VersionInfo { get; private set; }

            public void Set(Header header) => this.Header = header;

            public void Set(IHighScore score)
            {
                var key = (score.Chara, score.Level);
                if (!this.rankings.ContainsKey(key))
                    this.rankings.Add(key, new List<IHighScore>(InitialRanking));
                var ranking = this.rankings[key].ToList();
                ranking.Add(score);
                ranking.Sort((lhs, rhs) => rhs.Score.CompareTo(lhs.Score));
                ranking.RemoveAt(ranking.Count - 1);
                this.rankings[key] = ranking;
            }

            public void Set(IClearData data)
            {
                if (!this.clearData.ContainsKey(data.Chara))
                    this.clearData.Add(data.Chara, data);
            }

            public void Set(ICardAttack attack)
            {
                if (!this.cardAttacks.ContainsKey(attack.CardId))
                    this.cardAttacks.Add(attack.CardId, attack);
            }

            public void Set(IPracticeScore score)
            {
                if (!this.practiceScores.ContainsKey(score.Chara))
                    this.practiceScores.Add(score.Chara, score);
            }

            public void Set(FLSP flsp) => this.Flsp = flsp;

            public void Set(IPlayStatus status) => this.PlayStatus = status;

            public void Set(Th07.LastName name) => this.LastName = name;

            public void Set(Th07.VersionInfo info) => this.VersionInfo = info;
        }

        private class HighScore : Th06.Chapter, IHighScore   // per character, level, rank
        {
            public const string ValidSignature = "HSCR";
            public const short ValidSize = 0x0168;

            public HighScore(Th06.Chapter chapter)
                : base(chapter, ValidSignature, ValidSize)
            {
                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    reader.ReadUInt32();    // always 0x00000004?
                    this.Score = reader.ReadUInt32();
                    this.SlowRate = reader.ReadSingle();
                    this.Chara = Utils.ToEnum<Chara>(reader.ReadByte());
                    this.Level = Utils.ToEnum<Level>(reader.ReadByte());
                    this.StageProgress = Utils.ToEnum<StageProgress>(reader.ReadByte());
                    this.Name = reader.ReadExactBytes(9);
                    this.Date = reader.ReadExactBytes(6);
                    this.ContinueCount = reader.ReadUInt16();

                    // 01 00 00 00 04 00 09 00 FF FF FF FF FF FF FF FF
                    // 05 00 00 00 01 00 08 00 58 02 58 02
                    reader.ReadExactBytes(0x1C);

                    this.PlayerNum = reader.ReadByte();

                    // NN 03 00 01 01 LL 01 00 02 00 00 ** ** 00 00 00
                    // 00 00 00 00 00 00 00 00 00 00 00 00 01 40 00 00
                    // where NN: PlayerNum, LL: level, **: unknown (0x64 or 0x0A; 0x50 or 0x0A)
                    reader.ReadExactBytes(0x1F);

                    this.PlayTime = reader.ReadUInt32();
                    this.PointItem = reader.ReadInt32();
                    reader.ReadUInt32();    // always 0x00000000?
                    this.MissCount = reader.ReadInt32();
                    this.BombCount = reader.ReadInt32();
                    this.LastSpellCount = reader.ReadInt32();
                    this.PauseCount = reader.ReadInt32();
                    this.TimePoint = reader.ReadInt32();
                    this.HumanRate = reader.ReadInt32();
                    this.CardFlags = Definitions.CardTable.Keys.ToDictionary(key => key, _ => reader.ReadByte());
                    reader.ReadExactBytes(2);
                }
            }

            public HighScore(uint score)    // for InitialRanking only
                : base()
            {
                this.Score = score;
                this.Name = Encoding.Default.GetBytes("--------\0");
                this.Date = Encoding.Default.GetBytes("--/--\0");
                this.CardFlags = new Dictionary<int, byte>();
            }

            public uint Score { get; }      // Divided by 10

            public float SlowRate { get; }

            public Chara Chara { get; }

            public Level Level { get; }

            public StageProgress StageProgress { get; }

            public IEnumerable<byte> Name { get; }  // Null-terminated

            public IEnumerable<byte> Date { get; }  // "mm/dd\0"

            public ushort ContinueCount { get; }

            public byte PlayerNum { get; }  // 0-based

            public uint PlayTime { get; }   // = seconds * 60fps

            public int PointItem { get; }

            public int MissCount { get; }

            public int BombCount { get; }

            public int LastSpellCount { get; }

            public int PauseCount { get; }

            public int TimePoint { get; }

            public int HumanRate { get; }   // Multiplied by 100

            public IReadOnlyDictionary<int, byte> CardFlags { get; }
        }

        private class ClearData : Th06.Chapter, IClearData  // per character-with-total
        {
            public const string ValidSignature = "CLRD";
            public const short ValidSize = 0x0024;

            public ClearData(Th06.Chapter chapter)
                : base(chapter, ValidSignature, ValidSize)
            {
                var levels = Utils.GetEnumerator<Level>();

                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    reader.ReadUInt32();    // always 0x00000004?
                    this.StoryFlags = levels.ToDictionary(level => level, _ => (PlayableStages)reader.ReadUInt16());
                    this.PracticeFlags = levels.ToDictionary(level => level, _ => (PlayableStages)reader.ReadUInt16());
                    reader.ReadByte();      // always 0x00?
                    this.Chara = Utils.ToEnum<CharaWithTotal>(reader.ReadByte());
                    reader.ReadUInt16();    // always 0x0000?
                }
            }

            public IReadOnlyDictionary<Level, PlayableStages> StoryFlags { get; }    // really...?

            public IReadOnlyDictionary<Level, PlayableStages> PracticeFlags { get; } // really...?

            public CharaWithTotal Chara { get; }
        }

        private class CardAttack : Th06.Chapter, ICardAttack    // per card
        {
            public const string ValidSignature = "CATK";
            public const short ValidSize = 0x022C;

            private readonly CardAttackCareer storyCareer;
            private readonly CardAttackCareer practiceCareer;

            public CardAttack(Th06.Chapter chapter)
                : base(chapter, ValidSignature, ValidSize)
            {
                this.storyCareer = new CardAttackCareer();
                this.practiceCareer = new CardAttackCareer();

                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    reader.ReadUInt32();    // always 0x00000003?
                    this.CardId = (short)(reader.ReadInt16() + 1);
                    reader.ReadByte();
                    this.Level = Utils.ToEnum<LevelPracticeWithTotal>(reader.ReadByte());   // Last Word == Normal...
                    this.CardName = reader.ReadExactBytes(0x30);
                    this.EnemyName = reader.ReadExactBytes(0x30);
                    this.Comment = reader.ReadExactBytes(0x80);
                    this.storyCareer.ReadFrom(reader);
                    this.practiceCareer.ReadFrom(reader);
                    reader.ReadUInt32();    // always 0x00000000?
                }
            }

            public short CardId { get; }    // 1-based

            public LevelPracticeWithTotal Level { get; }

            public IEnumerable<byte> CardName { get; }

            public IEnumerable<byte> EnemyName { get; }

            public IEnumerable<byte> Comment { get; }  // Should be splitted by '\0'

            public ICardAttackCareer StoryCareer => this.storyCareer;

            public ICardAttackCareer PracticeCareer => this.practiceCareer;

            public bool HasTried()
            {
                return (this.StoryCareer.TrialCounts[CharaWithTotal.Total] > 0)
                    || (this.PracticeCareer.TrialCounts[CharaWithTotal.Total] > 0);
            }
        }

        private class CardAttackCareer : IBinaryReadable, ICardAttackCareer // per story or practice
        {
            public IReadOnlyDictionary<CharaWithTotal, uint> MaxBonuses { get; private set; }

            public IReadOnlyDictionary<CharaWithTotal, int> TrialCounts { get; private set; }

            public IReadOnlyDictionary<CharaWithTotal, int> ClearCounts { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException(nameof(reader));

                var charas = Utils.GetEnumerator<CharaWithTotal>();
                this.MaxBonuses = charas.ToDictionary(chara => chara, _ => reader.ReadUInt32());
                this.TrialCounts = charas.ToDictionary(chara => chara, _ => reader.ReadInt32());
                this.ClearCounts = charas.ToDictionary(chara => chara, _ => reader.ReadInt32());
            }
        }

        private class PracticeScore : Th06.Chapter, IPracticeScore  // per character
        {
            public const string ValidSignature = "PSCR";
            public const short ValidSize = 0x0178;

            public PracticeScore(Th06.Chapter chapter)
                : base(chapter, ValidSignature, ValidSize)
            {
                var stages = Utils.GetEnumerator<Th08.Stage>();
                var levels = Utils.GetEnumerator<Level>();

                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    //// The fields for Stage.Extra and Level.Extra actually exist...

                    reader.ReadUInt32();        // always 0x00000002?
                    this.PlayCounts = stages.SelectMany(stage => levels.Select(level => (stage, level)))
                        .ToDictionary(pair => pair, _ => reader.ReadInt32());
                    this.HighScores = stages.SelectMany(stage => levels.Select(level => (stage, level)))
                        .ToDictionary(pair => pair, _ => reader.ReadInt32());
                    this.Chara = Utils.ToEnum<Chara>(reader.ReadByte());
                    reader.ReadExactBytes(3);   // always 0x000001?
                }
            }

            public IReadOnlyDictionary<(Th08.Stage, Level), int> PlayCounts { get; }

            public IReadOnlyDictionary<(Th08.Stage, Level), int> HighScores { get; } // Divided by 10

            public Chara Chara { get; }
        }

        private class FLSP : Th06.Chapter    // FIXME
        {
            public const string ValidSignature = "FLSP";
            public const short ValidSize = 0x0020;

            public FLSP(Th06.Chapter chapter)
                : base(chapter, ValidSignature, ValidSize)
            {
                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    reader.ReadExactBytes(0x18);
                }
            }
        }

        private class PlayStatus : Th06.Chapter, IPlayStatus
        {
            public const string ValidSignature = "PLST";
            public const short ValidSize = 0x0228;

            public PlayStatus(Th06.Chapter chapter)
                : base(chapter, ValidSignature, ValidSize)
            {
                var levels = Utils.GetEnumerator<Level>();
                var numLevels = levels.Count();

                using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
                {
                    reader.ReadUInt32();    // always 0x00000002?
                    var hours = reader.ReadInt32();
                    var minutes = reader.ReadInt32();
                    var seconds = reader.ReadInt32();
                    var milliseconds = reader.ReadInt32();
                    this.TotalRunningTime = new Time(hours, minutes, seconds, milliseconds, false);
                    hours = reader.ReadInt32();
                    minutes = reader.ReadInt32();
                    seconds = reader.ReadInt32();
                    milliseconds = reader.ReadInt32();
                    this.TotalPlayTime = new Time(hours, minutes, seconds, milliseconds, false);

                    var playCounts = Utils.GetEnumerator<LevelPracticeWithTotal>().ToDictionary(level => level, _ =>
                    {
                        var playCount = new PlayCount();
                        playCount.ReadFrom(reader);
                        return playCount as IPlayCount;
                    });
                    this.PlayCounts = playCounts
                        .Where(pair => Enum.IsDefined(typeof(Level), (int)pair.Key))
                        .ToDictionary(pair => (Level)pair.Key, pair => pair.Value);
                    this.TotalPlayCount = playCounts[LevelPracticeWithTotal.Total];

                    this.BgmFlags = reader.ReadExactBytes(21);
                    reader.ReadExactBytes(11);
                }
            }

            public Time TotalRunningTime { get; }

            public Time TotalPlayTime { get; }

            public IReadOnlyDictionary<Level, IPlayCount> PlayCounts { get; }

            public IPlayCount TotalPlayCount { get; }

            public IEnumerable<byte> BgmFlags { get; }
        }

        private class PlayCount : IBinaryReadable, IPlayCount   // per level-with-total
        {
            public int TotalTrial { get; private set; }

            public IReadOnlyDictionary<Chara, int> Trials { get; private set; }

            public int TotalClear { get; private set; }

            public int TotalContinue { get; private set; }

            public int TotalPractice { get; private set; }

            public void ReadFrom(BinaryReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException(nameof(reader));

                this.TotalTrial = reader.ReadInt32();
                this.Trials = Utils.GetEnumerator<Chara>().ToDictionary(chara => chara, _ => reader.ReadInt32());
                reader.ReadUInt32();    // always 0x00000000?
                this.TotalClear = reader.ReadInt32();
                this.TotalContinue = reader.ReadInt32();
                this.TotalPractice = reader.ReadInt32();
            }
        }
    }
}
