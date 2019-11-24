//-----------------------------------------------------------------------
// <copyright file="Th15Converter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable 1591
#pragma warning disable SA1600 // ElementsMustBeDocumented

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th15;

namespace ThScoreFileConverter.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th15Converter : ThConverter
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
                        if (!ClearData.CanInitialize(chapter) && !Th13.Status.CanInitialize(chapter))
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
                { ClearData.ValidSignature,   (data, ch) => data.Set(new ClearData(ch))   },
                { Th13.Status.ValidSignature, (data, ch) => data.Set(new Th13.Status(ch)) },
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
                Parsers.GameModeParser.Pattern,
                Parsers.LevelParser.Pattern,
                Parsers.CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ScoreReplacer(Th15Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var mode = Parsers.GameModeParser.Parse(match.Groups[1].Value);
                    var level = (LevelWithTotal)Parsers.LevelParser.Parse(match.Groups[2].Value);
                    var chara = (CharaWithTotal)Parsers.CharaParser.Parse(match.Groups[3].Value);
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
                                return Th13.StageProgress.None.ToShortName();
                            if (ranking.StageProgress == Th13.StageProgress.Extra)
                                return "Not Clear";
                            if (ranking.StageProgress == Th13.StageProgress.ExtraClear)
                                return Th13.StageProgress.Clear.ToShortName();
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
                @"%T15C({0})(\d{{3}})({1})([12])",
                Parsers.GameModeParser.Pattern,
                Parsers.CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CareerReplacer(Th15Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var mode = Parsers.GameModeParser.Parse(match.Groups[1].Value);
                    var number = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[3].Value);
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
                    else if (Definitions.CardTable.ContainsKey(number))
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

                    if (Definitions.CardTable.ContainsKey(number))
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

                            return Definitions.CardTable[number].Name;
                        }
                        else
                        {
                            return Definitions.CardTable[number].Level.ToString();
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
                Parsers.GameModeParser.Pattern,
                Parsers.LevelWithTotalParser.Pattern,
                Parsers.CharaWithTotalParser.Pattern,
                Parsers.StageWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CollectRateReplacer(Th15Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var mode = Parsers.GameModeParser.Parse(match.Groups[1].Value);
                    var level = Parsers.LevelWithTotalParser.Parse(match.Groups[2].Value);
                    var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[3].Value);
                    var stage = Parsers.StageWithTotalParser.Parse(match.Groups[4].Value);
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
                        findByStage = (card => Definitions.CardTable[card.Id].Stage == (Stage)stage);

                    Func<Th13.ISpellCard<Level>, bool> findByLevel = (card => true);
                    switch (level)
                    {
                        case LevelWithTotal.Total:
                            // Do nothing
                            break;
                        case LevelWithTotal.Extra:
                            findByStage = (card => Definitions.CardTable[card.Id].Stage == Stage.Extra);
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
                @"%T15CLEAR({0})({1})({2})",
                Parsers.GameModeParser.Pattern,
                Parsers.LevelParser.Pattern,
                Parsers.CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ClearReplacer(Th15Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var mode = Parsers.GameModeParser.Parse(match.Groups[1].Value);
                    var level = (LevelWithTotal)Parsers.LevelParser.Parse(match.Groups[2].Value);
                    var chara = (CharaWithTotal)Parsers.CharaParser.Parse(match.Groups[3].Value);

#if false   // FIXME
                    if (level == LevelWithTotal.Extra)
                        mode = GameMode.Pointdevice;
#endif

                    var rankings = parent.allScoreData.ClearData[chara].GameModeData[mode].Rankings[level]
                        .Where(ranking => ranking.DateTime > 0);
                    var stageProgress = rankings.Any()
                        ? rankings.Max(ranking => ranking.StageProgress) : Th13.StageProgress.None;

                    if (stageProgress == Th13.StageProgress.Extra)
                        return "Not Clear";
                    else if (stageProgress == Th13.StageProgress.ExtraClear)
                        return Th13.StageProgress.Clear.ToShortName();
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
                @"%T15CHARA({0})({1})([1-3])", Parsers.GameModeParser.Pattern, Parsers.CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CharaReplacer(Th15Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var mode = Parsers.GameModeParser.Parse(match.Groups[1].Value);
                    var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[2].Value);
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
                Parsers.GameModeParser.Pattern,
                Parsers.LevelWithTotalParser.Pattern,
                Parsers.CharaWithTotalParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CharaExReplacer(Th15Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var mode = Parsers.GameModeParser.Parse(match.Groups[1].Value);
                    var level = Parsers.LevelWithTotalParser.Parse(match.Groups[2].Value);
                    var chara = Parsers.CharaWithTotalParser.Parse(match.Groups[3].Value);
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
                @"%T15PRAC({0})({1})({2})",
                Parsers.LevelParser.Pattern,
                Parsers.CharaParser.Pattern,
                Parsers.StageParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public PracticeReplacer(Th15Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
                    var chara = (CharaWithTotal)Parsers.CharaParser.Parse(match.Groups[2].Value);
                    var stage = Parsers.StageParser.Parse(match.Groups[3].Value);

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

                this.Cards = Enumerable.Range(0, Definitions.CardTable.Count).Select(_ =>
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
