//-----------------------------------------------------------------------
// <copyright file="Th07Converter.cs" company="None">
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
using ThScoreFileConverter.Models.Th07;

namespace ThScoreFileConverter.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th07Converter : ThConverter
    {
        private AllScoreData allScoreData = null;

        public override string SupportedVersions
        {
            get { return "1.00b"; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th07decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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
                            case VersionInfo.ValidSignature:
                                if (chapter.FirstByteOfData != 0x01)
                                    return false;
                                //// th07.exe does something more things here...
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
                { Header.ValidSignature,        (data, ch) => data.Set(new Header(ch))        },
                { HighScore.ValidSignature,     (data, ch) => data.Set(new HighScore(ch))     },
                { ClearData.ValidSignature,     (data, ch) => data.Set(new ClearData(ch))     },
                { CardAttack.ValidSignature,    (data, ch) => data.Set(new CardAttack(ch))    },
                { PracticeScore.ValidSignature, (data, ch) => data.Set(new PracticeScore(ch)) },
                { PlayStatus.ValidSignature,    (data, ch) => data.Set(new PlayStatus(ch))    },
                { LastName.ValidSignature,      (data, ch) => data.Set(new LastName(ch))      },
                { VersionInfo.ValidSignature,   (data, ch) => data.Set(new VersionInfo(ch))   },
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
                    (allScoreData.ClearData.Count == Enum.GetValues(typeof(Chara)).Length) &&
                    //// (allScoreData.cardAttacks.Length == NumCards) &&
                    //// (allScoreData.practiceScores.Count >= 0) &&
                    (allScoreData.PlayStatus != null) &&
                    (allScoreData.LastName != null) &&
                    (allScoreData.VersionInfo != null))
                    return allScoreData;
                else
                    return null;
            }
        }

        // %T07SCR[w][xx][y][z]
        private class ScoreReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T07SCR({0})({1})(\d)([1-5])", Parsers.LevelParser.Pattern, Parsers.CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ScoreReplacer(Th07Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
                    var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
                    var rank = Utils.ToZeroBased(
                        int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    var key = (chara, level);
                    var score = parent.allScoreData.Rankings.ContainsKey(key)
                        ? parent.allScoreData.Rankings[key][rank] : Definitions.InitialRanking[rank];

                    switch (type)
                    {
                        case 1:     // name
                            return Encoding.Default.GetString(score.Name).Split('\0')[0];
                        case 2:     // score
                            return Utils.ToNumberString((score.Score * 10) + score.ContinueCount);
                        case 3:     // stage
                            return score.StageProgress.ToShortName();
                        case 4:     // date
                            return Encoding.Default.GetString(score.Date).TrimEnd('\0');
                        case 5:     // slow rate
                            return Utils.Format("{0:F3}%", score.SlowRate);
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

        // %T07C[xxx][yy][z]
        private class CareerReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T07C(\d{{3}})({0})([1-3])", Parsers.CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CareerReplacer(Th07Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    Func<CardAttack, long> getValue;
                    if (type == 1)
                        getValue = (attack => attack.MaxBonuses[chara]);
                    else if (type == 2)
                        getValue = (attack => attack.ClearCounts[chara]);
                    else
                        getValue = (attack => attack.TrialCounts[chara]);

                    if (number == 0)
                    {
                        return Utils.ToNumberString(parent.allScoreData.CardAttacks.Values.Sum(getValue));
                    }
                    else if (Definitions.CardTable.ContainsKey(number))
                    {
                        if (parent.allScoreData.CardAttacks.TryGetValue(number, out var attack))
                            return Utils.ToNumberString(getValue(attack));
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

        // %T07CARD[xxx][y]
        private class CardReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T07CARD(\d{3})([NR])";

            private readonly MatchEvaluator evaluator;

            public CardReplacer(Th07Converter parent, bool hideUntriedCards)
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

                        return (type == "N")
                            ? Definitions.CardTable[number].Name : Definitions.CardTable[number].Level.ToString();
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

        // %T07CRG[w][xx][yy][z]
        private class CollectRateReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T07CRG({0})({1})({2})([12])",
                Parsers.LevelWithTotalParser.Pattern,
                Parsers.CharaWithTotalParser.Pattern,
                Parsers.StageWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CollectRateReplacer(Th07Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = Parsers.LevelWithTotalParser.Parse(match.Groups[1].Value);
                    var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[2].Value);
                    var stage = Parsers.StageWithTotalParser.Parse(match.Groups[3].Value);
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    if ((stage == Th07.StageWithTotal.Extra) || (stage == Th07.StageWithTotal.Phantasm))
                        return match.ToString();

                    Func<CardAttack, bool> findByStage;
                    if (stage == Th07.StageWithTotal.Total)
                        findByStage = (attack => true);
                    else
                        findByStage = (attack => Definitions.CardTable[attack.CardId].Stage == (Th07.Stage)stage);

                    Func<CardAttack, bool> findByLevel = (attack => true);
                    switch (level)
                    {
                        case Th07.LevelWithTotal.Total:
                            // Do nothing
                            break;
                        case Th07.LevelWithTotal.Extra:
                            findByStage = (attack => Definitions.CardTable[attack.CardId].Stage == Th07.Stage.Extra);
                            break;
                        case Th07.LevelWithTotal.Phantasm:
                            findByStage = (attack => Definitions.CardTable[attack.CardId].Stage == Th07.Stage.Phantasm);
                            break;
                        default:
                            findByLevel = (attack => Definitions.CardTable[attack.CardId].Level == (Th07.Level)level);
                            break;
                    }

                    Func<CardAttack, bool> findByType;
                    if (type == 1)
                        findByType = (attack => attack.ClearCounts[chara] > 0);
                    else
                        findByType = (attack => attack.TrialCounts[chara] > 0);

                    return parent.allScoreData.CardAttacks.Values
                        .Count(Utils.MakeAndPredicate(findByLevel, findByStage, findByType))
                        .ToString(CultureInfo.CurrentCulture);
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T07CLEAR[x][yy]
        private class ClearReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T07CLEAR({0})({1})", Parsers.LevelParser.Pattern, Parsers.CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ClearReplacer(Th07Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
                    var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);

                    var key = (chara, level);
                    if (parent.allScoreData.Rankings.ContainsKey(key))
                    {
                        var stageProgress = parent.allScoreData
                            .Rankings[key].Max(rank => rank.StageProgress);
                        if ((stageProgress == StageProgress.Extra) ||
                            (stageProgress == StageProgress.Phantasm))
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

        // %T07PLAY[x][yy]
        private class PlayReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T07PLAY({0})({1}|CL|CN|PR|RT)",
                Parsers.LevelWithTotalParser.Pattern,
                Parsers.CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public PlayReplacer(Th07Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = Parsers.LevelWithTotalParser.Parse(match.Groups[1].Value);
                    var charaAndMore = match.Groups[2].Value.ToUpperInvariant();

                    var playCount = parent.allScoreData.PlayStatus.PlayCounts[level];
                    switch (charaAndMore)
                    {
                        case "CL":  // clear count
                            return Utils.ToNumberString(playCount.TotalClear);
                        case "CN":  // continue count
                            return Utils.ToNumberString(playCount.TotalContinue);
                        case "PR":  // practice count
                            return Utils.ToNumberString(playCount.TotalPractice);
                        case "RT":  // retry count
                            return Utils.ToNumberString(playCount.TotalRetry);
                        default:
                            {
                                var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[2].Value);
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

        // %T07TIME(ALL|PLY)
        private class TimeReplacer : IStringReplaceable
        {
            private const string Pattern = @"%T07TIME(ALL|PLY)";

            private readonly MatchEvaluator evaluator;

            public TimeReplacer(Th07Converter parent)
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

        // %T07PRAC[w][xx][y][z]
        private class PracticeReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T07PRAC({0})({1})({2})([12])",
                Parsers.LevelParser.Pattern,
                Parsers.CharaParser.Pattern,
                Parsers.StageParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public PracticeReplacer(Th07Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
                    var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
                    var stage = Parsers.StageParser.Parse(match.Groups[3].Value);
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    if ((level == Th07.Level.Extra) || (level == Th07.Level.Phantasm))
                        return match.ToString();
                    if ((stage == Th07.Stage.Extra) || (stage == Th07.Stage.Phantasm))
                        return match.ToString();

                    var key = (chara, level);
                    if (parent.allScoreData.PracticeScores.ContainsKey(key))
                    {
                        var scores = parent.allScoreData.PracticeScores[key];
                        if (type == 1)
                        {
                            return scores.ContainsKey(stage)
                                ? Utils.ToNumberString(scores[stage].HighScore * 10) : "0";
                        }
                        else
                        {
                            return scores.ContainsKey(stage)
                                ? Utils.ToNumberString(scores[stage].TrialCount) : "0";
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
            public AllScoreData()
            {
                var numCharas = Enum.GetValues(typeof(Chara)).Length;
                var numPairs = numCharas * Enum.GetValues(typeof(Th07.Level)).Length;
                this.Rankings = new Dictionary<(Chara, Th07.Level), List<HighScore>>(numPairs);
                this.ClearData = new Dictionary<Chara, ClearData>(numCharas);
                this.CardAttacks = new Dictionary<int, CardAttack>(Definitions.CardTable.Count);
                this.PracticeScores = new Dictionary<(Chara, Th07.Level), Dictionary<Th07.Stage, PracticeScore>>(numPairs);
            }

            public Header Header { get; private set; }

            public Dictionary<(Chara, Th07.Level), List<HighScore>> Rankings { get; private set; }

            public Dictionary<Chara, ClearData> ClearData { get; private set; }

            public Dictionary<int, CardAttack> CardAttacks { get; private set; }

            public Dictionary<(Chara, Th07.Level), Dictionary<Th07.Stage, PracticeScore>> PracticeScores { get; private set; }

            public PlayStatus PlayStatus { get; private set; }

            public LastName LastName { get; private set; }

            public VersionInfo VersionInfo { get; private set; }

            public void Set(Header header) => this.Header = header;

            public void Set(HighScore score)
            {
                var key = (score.Chara, score.Level);
                if (!this.Rankings.ContainsKey(key))
                    this.Rankings.Add(key, new List<HighScore>(Definitions.InitialRanking));
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
                if ((score.Level != Th07.Level.Extra) && (score.Level != Th07.Level.Phantasm) &&
                    (score.Stage != Th07.Stage.Extra) && (score.Stage != Th07.Stage.Phantasm))
                {
                    var key = (score.Chara, score.Level);
                    if (!this.PracticeScores.ContainsKey(key))
                    {
                        var numStages = Utils.GetEnumerator<Th07.Stage>()
                            .Where(st => (st != Th07.Stage.Extra) && (st != Th07.Stage.Phantasm)).Count();
                        this.PracticeScores.Add(key, new Dictionary<Th07.Stage, PracticeScore>(numStages));
                    }

                    var scores = this.PracticeScores[key];
                    if (!scores.ContainsKey(score.Stage))
                        scores.Add(score.Stage, score);
                }
            }

            public void Set(PlayStatus status) => this.PlayStatus = status;

            public void Set(LastName name) => this.LastName = name;

            public void Set(VersionInfo info) => this.VersionInfo = info;
        }
    }
}
