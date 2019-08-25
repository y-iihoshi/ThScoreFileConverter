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

namespace ThScoreFileConverter.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th06Converter : ThConverter
    {
        private AllScoreData allScoreData = null;

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
                @"%T06SCR({0})({1})(\d)([1-3])", Parsers.LevelParser.Pattern, Parsers.CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ScoreReplacer(Th06Converter parent)
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
                    else if (Definitions.CardTable.ContainsKey(number))
                    {
                        if (parent.allScoreData.CardAttacks.TryGetValue(number, out var attack))
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

                    if (Definitions.CardTable.ContainsKey(number))
                    {
                        if (hideUntriedCards)
                        {
                            if (!parent.allScoreData.CardAttacks.TryGetValue(number, out var attack) ||
                                !attack.HasTried())
                                return (type == "N") ? "??????????" : "?????";
                        }

                        return (type == "N")
                            ? Definitions.CardTable[number].Name
                            : string.Join(
                                ", ", Definitions.CardTable[number].Levels.Select(lv => lv.ToString()).ToArray());
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
                @"%T06CRG({0})([12])", Parsers.StageWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CollectRateReplacer(Th06Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var stage = Parsers.StageWithTotalParser.Parse(match.Groups[1].Value);
                    var type = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

                    Func<CardAttack, bool> findByStage;
                    if (stage == StageWithTotal.Total)
                        findByStage = (attack => true);
                    else
                        findByStage = (attack => Definitions.CardTable[attack.CardId].Stage == (Stage)stage);

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
                @"%T06CLEAR({0})({1})", Parsers.LevelParser.Pattern, Parsers.CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ClearReplacer(Th06Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
                    var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);

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
                @"%T06PRAC({0})({1})({2})",
                Parsers.LevelParser.Pattern,
                Parsers.CharaParser.Pattern,
                Parsers.StageParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public PracticeReplacer(Th06Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
                    var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
                    var stage = Parsers.StageParser.Parse(match.Groups[3].Value);

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
                this.CardAttacks = new Dictionary<int, CardAttack>(Definitions.CardTable.Count);
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
    }
}
