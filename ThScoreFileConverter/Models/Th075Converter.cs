//-----------------------------------------------------------------------
// <copyright file="Th075Converter.cs" company="None">
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
using ThScoreFileConverter.Models.Th075;

namespace ThScoreFileConverter.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th075Converter : ThConverter
    {
        private static readonly Dictionary<Chara, IEnumerable<int>> CardIdTable = InitializeCardIdTable();

        private static new readonly EnumShortNameParser<Level> LevelParser =
            new EnumShortNameParser<Level>();

        private static new readonly EnumShortNameParser<LevelWithTotal> LevelWithTotalParser =
            new EnumShortNameParser<LevelWithTotal>();

        private static readonly EnumShortNameParser<Chara> CharaParser =
            new EnumShortNameParser<Chara>();

        private AllScoreData allScoreData = null;

        public enum Level
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum LevelWithTotal
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("E")] Easy,
            [EnumAltName("N")] Normal,
            [EnumAltName("H")] Hard,
            [EnumAltName("L")] Lunatic,
            [EnumAltName("T")] Total,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum Chara
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("RM")] Reimu,
            [EnumAltName("MR")] Marisa,
            [EnumAltName("SK")] Sakuya,
            [EnumAltName("AL")] Alice,
            [EnumAltName("PC")] Patchouli,
            [EnumAltName("YM")] Youmu,
            [EnumAltName("RL")] Remilia,
            [EnumAltName("YU")] Yuyuko,
            [EnumAltName("YK")] Yukari,
            [EnumAltName("SU")] Suika,
            [EnumAltName("ML")] Meiling,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum CharaWithTotal
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("RM")] Reimu,
            [EnumAltName("MR")] Marisa,
            [EnumAltName("SK")] Sakuya,
            [EnumAltName("AL")] Alice,
            [EnumAltName("PC")] Patchouli,
            [EnumAltName("YM")] Youmu,
            [EnumAltName("RL")] Remilia,
            [EnumAltName("YU")] Yuyuko,
            [EnumAltName("YK")] Yukari,
            [EnumAltName("SU")] Suika,
            [EnumAltName("ML")] Meiling,
            [EnumAltName("TL")] Total,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public enum Stage
        {
#pragma warning disable SA1134 // Attributes should not share line
            [EnumAltName("1")] St1,
            [EnumAltName("2")] St2,
            [EnumAltName("3")] St3,
            [EnumAltName("4")] St4,
            [EnumAltName("5")] St5,
            [EnumAltName("6")] St6,
            [EnumAltName("7")] St7,
#pragma warning restore SA1134 // Attributes should not share line
        }

        public override string SupportedVersions
        {
            get { return "1.11"; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
#if DEBUG
            using (var decoded = new FileStream("th075decoded.dat", FileMode.Create, FileAccess.ReadWrite))
            {
                var size = (int)input.Length;
                var data = new byte[size];
                input.Read(data, 0, size);
                decoded.Write(data, 0, size);
                decoded.Flush();
                decoded.SetLength(decoded.Position);
            }
#endif

            input.Seek(0, SeekOrigin.Begin);
            this.allScoreData = Read(input);

            return this.allScoreData != null;
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
                new CharaReplacer(this),
            };
        }

        private static Dictionary<Chara, IEnumerable<int>> InitializeCardIdTable()
        {
            var charaStageEnemyTable = new Dictionary<Chara, List<(Stage Stage, Chara Enemy)>>
            {
                {
                    Chara.Reimu,
                    new List<(Stage, Chara)>
                    {
                        (Stage.St1, Chara.Marisa),
                        (Stage.St2, Chara.Alice),
                        (Stage.St3, Chara.Youmu),
                        (Stage.St4, Chara.Sakuya),
                        (Stage.St5, Chara.Remilia),
                        (Stage.St6, Chara.Yukari),
                        (Stage.St7, Chara.Suika),
                    }
                },
                {
                    Chara.Marisa,
                    new List<(Stage, Chara)>
                    {
                        (Stage.St1, Chara.Alice),
                        (Stage.St2, Chara.Sakuya),
                        (Stage.St3, Chara.Patchouli),
                        (Stage.St4, Chara.Remilia),
                        (Stage.St5, Chara.Reimu),
                        (Stage.St6, Chara.Yukari),
                        (Stage.St7, Chara.Suika),
                    }
                },
                {
                    Chara.Sakuya,
                    new List<(Stage, Chara)>
                    {
                        (Stage.St1, Chara.Reimu),
                        (Stage.St2, Chara.Alice),
                        (Stage.St3, Chara.Marisa),
                        (Stage.St4, Chara.Youmu),
                        (Stage.St5, Chara.Yuyuko),
                        (Stage.St6, Chara.Yukari),
                        (Stage.St7, Chara.Suika),
                    }
                },
                {
                    Chara.Alice,
                    new List<(Stage, Chara)>
                    {
                        (Stage.St1, Chara.Marisa),
                        (Stage.St2, Chara.Reimu),
                        (Stage.St3, Chara.Sakuya),
                        (Stage.St4, Chara.Patchouli),
                        (Stage.St5, Chara.Youmu),
                        (Stage.St6, Chara.Yukari),
                        (Stage.St7, Chara.Suika),
                    }
                },
                {
                    Chara.Patchouli,
                    new List<(Stage, Chara)>
                    {
                        (Stage.St1, Chara.Marisa),
                        (Stage.St2, Chara.Sakuya),
                        (Stage.St3, Chara.Alice),
                        (Stage.St4, Chara.Youmu),
                        (Stage.St5, Chara.Yuyuko),
                        (Stage.St6, Chara.Yukari),
                        (Stage.St7, Chara.Suika),
                    }
                },
                {
                    Chara.Youmu,
                    new List<(Stage, Chara)>
                    {
                        (Stage.St1, Chara.Reimu),
                        (Stage.St2, Chara.Marisa),
                        (Stage.St3, Chara.Patchouli),
                        (Stage.St4, Chara.Sakuya),
                        (Stage.St5, Chara.Remilia),
                        (Stage.St6, Chara.Yukari),
                        (Stage.St7, Chara.Suika),
                    }
                },
                {
                    Chara.Remilia,
                    new List<(Stage, Chara)>
                    {
                        (Stage.St1, Chara.Sakuya),
                        (Stage.St2, Chara.Marisa),
                        (Stage.St3, Chara.Reimu),
                        (Stage.St4, Chara.Youmu),
                        (Stage.St5, Chara.Yuyuko),
                        (Stage.St6, Chara.Yukari),
                        (Stage.St7, Chara.Suika),
                    }
                },
                {
                    Chara.Yuyuko,
                    new List<(Stage, Chara)>
                    {
                        (Stage.St1, Chara.Youmu),
                        (Stage.St2, Chara.Marisa),
                        (Stage.St3, Chara.Reimu),
                        (Stage.St4, Chara.Sakuya),
                        (Stage.St5, Chara.Remilia),
                        (Stage.St6, Chara.Yukari),
                        (Stage.St7, Chara.Suika),
                    }
                },
                {
                    Chara.Yukari,
                    new List<(Stage, Chara)>
                    {
                        (Stage.St1, Chara.Remilia),
                        (Stage.St2, Chara.Alice),
                        (Stage.St3, Chara.Youmu),
                        (Stage.St4, Chara.Marisa),
                        (Stage.St5, Chara.Reimu),
                        (Stage.St6, Chara.Yuyuko),
                        (Stage.St7, Chara.Suika),
                    }
                },
                {
                    Chara.Suika,
                    new List<(Stage, Chara)>
                    {
                        (Stage.St1, Chara.Sakuya),
                        (Stage.St2, Chara.Alice),
                        (Stage.St3, Chara.Youmu),
                        (Stage.St4, Chara.Patchouli),
                        (Stage.St5, Chara.Marisa),
                        (Stage.St6, Chara.Remilia),
                        (Stage.St7, Chara.Reimu),
                    }
                },
            };

            var cardNumberTable = Definitions.CardTable.ToLookup(pair => pair.Value.Enemy, pair => pair.Key);

            return charaStageEnemyTable.ToDictionary(
                charaStageEnemyPair => charaStageEnemyPair.Key,
                charaStageEnemyPair => charaStageEnemyPair.Value.SelectMany(stageEnemyPair =>
                {
                    switch (stageEnemyPair.Stage)
                    {
                        case Stage.St1:
                        case Stage.St2:
                            return cardNumberTable[stageEnemyPair.Enemy].Take(8);
                        case Stage.St3:
                        case Stage.St4:
                            return cardNumberTable[stageEnemyPair.Enemy].Take(12);
                        case Stage.St5:
                            return cardNumberTable[stageEnemyPair.Enemy].Take(16);
                        case Stage.St6:
                            return cardNumberTable[stageEnemyPair.Enemy].Take(20);
                        case Stage.St7:
                            return cardNumberTable[stageEnemyPair.Enemy].Take(24);
                        default:
                            return null;    // unreachable
                    }
                }));
        }

        private static AllScoreData Read(Stream input)
        {
            using (var reader = new BinaryReader(input, Encoding.UTF8, true))
            {
                var allScoreData = new AllScoreData();

                try
                {
                    allScoreData.ReadFrom(reader);
                }
                catch (EndOfStreamException)
                {
                }

                var numPairs = Enum.GetValues(typeof(Chara)).Length * Enum.GetValues(typeof(Level)).Length;
                if ((allScoreData.ClearData.Sum(data => data.Value.Count) == numPairs) &&
                    (allScoreData.Status != null))
                    return allScoreData;
                else
                    return null;
            }
        }

        // %T75SCR[w][xx][y][z]
        private class ScoreReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T75SCR({0})({1})(\d)([1-3])", LevelParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public ScoreReplacer(Th075Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var chara = CharaParser.Parse(match.Groups[2].Value);
                    var rank = Utils.ToZeroBased(
                        int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                    var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    if (chara == Chara.Meiling)
                        return match.ToString();

                    var score = parent.allScoreData.ClearData[chara][level].Ranking[rank];

                    switch (type)
                    {
                        case 1:     // name
                            return score.Name;
                        case 2:     // score
                            return Utils.ToNumberString(score.Score);
                        case 3:     // date
                            return Utils.Format("{0:D2}/{1:D2}", score.Month, score.Day);
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

        // %T75C[xxx][yy][z]
        private class CareerReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T75C(\d{{3}})({0})([1-4])", CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CareerReplacer(Th075Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var chara = CharaParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    if (chara == Chara.Meiling)
                        return match.ToString();

                    if (type == 4)
                    {
                        if ((number > 0) && (number <= CardIdTable[chara].Count()))
                        {
                            return parent.allScoreData.ClearData[chara].Values
                                .Any(data => data.CardTrulyGot[number - 1] != 0x00) ? "★" : string.Empty;
                        }
                        else
                        {
                            return match.ToString();
                        }
                    }

                    Func<short, int> toInteger = (value => (int)value);
                    Func<ClearData, IEnumerable<int>> getValues;
                    if (type == 1)
                        getValues = (data => data.MaxBonuses);
                    else if (type == 2)
                        getValues = (data => data.CardGotCount.Select(toInteger));
                    else
                        getValues = (data => data.CardTrialCount.Select(toInteger));

                    if (number == 0)
                    {
                        return Utils.ToNumberString(
                            parent.allScoreData.ClearData[chara].Values.Sum(data => getValues(data).Sum()));
                    }
                    else if (number <= CardIdTable[chara].Count())
                    {
                        return Utils.ToNumberString(
                            parent.allScoreData.ClearData[chara].Values.Sum(data =>
                                getValues(data).ElementAt(number - 1)));
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

        // %T75CARD[xxx][yy][z]
        private class CardReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T75CARD(\d{{3}})({0})([NR])", CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public CardReplacer(Th075Converter parent, bool hideUntriedCards)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var chara = CharaParser.Parse(match.Groups[2].Value);
                    var type = match.Groups[3].Value.ToUpperInvariant();

                    if (chara == Chara.Meiling)
                        return match.ToString();

                    if ((number > 0) && (number <= CardIdTable[chara].Count()))
                    {
                        if (hideUntriedCards)
                        {
                            var dataList = parent.allScoreData.ClearData[chara]
                                .Select(pair => pair.Value);
                            if (dataList.All(data => data.CardTrialCount[number - 1] <= 0))
                                return (type == "N") ? "??????????" : "?????";
                        }

                        var cardId = CardIdTable[chara].ElementAt(number - 1);
                        return (type == "N")
                            ? Definitions.CardTable[cardId].Name : Definitions.CardTable[cardId].Level.ToString();
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

        // %T75CRG[x][yy][z]
        private class CollectRateReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T75CRG({0})({1})([1-3])", LevelWithTotalParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CollectRateReplacer(Th075Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelWithTotalParser.Parse(match.Groups[1].Value);
                    var chara = CharaParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    if (chara == Chara.Meiling)
                        return match.ToString();

                    Func<ClearData, IEnumerable<short>> getValues;
                    if (type == 1)
                        getValues = (data => data.CardGotCount);
                    else if (type == 2)
                        getValues = (data => data.CardTrialCount);
                    else
                        getValues = (data => data.CardTrulyGot.Select(got => (short)got));

                    Func<short, bool> isPositive = (value => value > 0);

                    if (level == LevelWithTotal.Total)
                    {
                        return Utils.ToNumberString(
                            parent.allScoreData.ClearData[chara].Values.Sum(data =>
                                getValues(data).Count(isPositive)));
                    }
                    else
                    {
                        var cardIndexIdPairs = CardIdTable[chara]
                            .Select((id, index) => new KeyValuePair<int, int>(index, id))
                            .Where(pair => Definitions.CardTable[pair.Value].Level == (Level)level);
                        return Utils.ToNumberString(
                            getValues(parent.allScoreData.ClearData[chara][Level.Easy])
                                .Where((value, index) => cardIndexIdPairs.Any(pair => pair.Key == index))
                                .Count(isPositive));
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T75CHR[x][yy][z]
        private class CharaReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T75CHR({0})({1})([1-4])", LevelParser.Pattern, CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public CharaReplacer(Th075Converter parent)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = LevelParser.Parse(match.Groups[1].Value);
                    var chara = CharaParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    if (chara == Chara.Meiling)
                        return match.ToString();

                    var data = parent.allScoreData.ClearData[chara][level];
                    switch (type)
                    {
                        case 1:
                            return Utils.ToNumberString(data.UseCount);
                        case 2:
                            return Utils.ToNumberString(data.ClearCount);
                        case 3:
                            return Utils.ToNumberString(data.MaxCombo);
                        case 4:
                            return Utils.ToNumberString(data.MaxDamage);
                        default:
                            return match.ToString();
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        private class AllScoreData : IBinaryReadable
        {
            public AllScoreData()
            {
                var charas = Utils.GetEnumerator<Chara>();
                var numLevels = Enum.GetValues(typeof(Level)).Length;
                this.ClearData = new Dictionary<Chara, Dictionary<Level, ClearData>>(charas.Count());
                foreach (var chara in charas)
                    this.ClearData.Add(chara, new Dictionary<Level, ClearData>(numLevels));
            }

            public Dictionary<Chara, Dictionary<Level, ClearData>> ClearData { get; private set; }

            public Status Status { get; private set; }

            [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "unknownChara", Justification = "Reviewed.")]
            [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "knownLevel", Justification = "Reviewed.")]
            public void ReadFrom(BinaryReader reader)
            {
                var levels = Utils.GetEnumerator<Level>();

                foreach (var chara in Utils.GetEnumerator<Chara>())
                {
                    foreach (var level in levels)
                    {
                        var clearData = new ClearData();
                        clearData.ReadFrom(reader);
                        if (!this.ClearData[chara].ContainsKey(level))
                            this.ClearData[chara].Add(level, clearData);
                    }
                }

                foreach (var unknownChara in Enumerable.Range(1, 4))
                {
                    foreach (var knownLevel in levels)
                        new ClearData().ReadFrom(reader);
                }

                var status = new Status();
                status.ReadFrom(reader);
                this.Status = status;
            }
        }
    }
}
