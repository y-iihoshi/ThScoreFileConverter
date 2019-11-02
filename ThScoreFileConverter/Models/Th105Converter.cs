//-----------------------------------------------------------------------
// <copyright file="Th105Converter.cs" company="None">
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
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using ThScoreFileConverter.Models.Th105;
using SpellCardResult = ThScoreFileConverter.Models.Th105.SpellCardResult<
    ThScoreFileConverter.Models.Th105.Chara, ThScoreFileConverter.Models.Th105.Level>;

namespace ThScoreFileConverter.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
    internal class Th105Converter : ThConverter
    {
        private AllScoreData allScoreData = null;

        public override string SupportedVersions
        {
            get { return "1.06a"; }
        }

        protected override bool ReadScoreFile(Stream input)
        {
            using (var decrypted = new MemoryStream())
#if DEBUG
            using (var decoded = new FileStream("th105decoded.dat", FileMode.Create, FileAccess.ReadWrite))
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
                this.allScoreData = Read(decoded);

                return this.allScoreData != null;
            }
        }

        protected override IEnumerable<IStringReplaceable> CreateReplacers(
            bool hideUntriedCards, string outputFilePath)
        {
            return new List<IStringReplaceable>
            {
                new CareerReplacer(this.allScoreData.ClearData),
                new CardReplacer(this.allScoreData.ClearData, hideUntriedCards),
                new CollectRateReplacer(this.allScoreData.ClearData),
                new CardForDeckReplacer(this.allScoreData.SystemCards, this.allScoreData.ClearData, hideUntriedCards),
            };
        }

        private static bool Decrypt(Stream input, Stream output)
        {
            var size = (int)input.Length;
            var inData = new byte[size];
            var outData = new byte[size];

            input.Seek(0, SeekOrigin.Begin);
            input.Read(inData, 0, size);

            for (var index = 0; index < size; index++)
                outData[index] = (byte)((index * 7) ^ inData[size - index - 1]);

            output.Seek(0, SeekOrigin.Begin);
            output.Write(outData, 0, size);

            // See section 2.2 of RFC 1950
            return (outData[0] == 0x78) && (outData[1] == 0x9C);
        }

        private static bool Extract(Stream input, Stream output)
        {
            var extracted = new byte[0x80000];
            var extractedSize = 0;

            // Skip the header bytes of a zlib stream
            input.Seek(2, SeekOrigin.Begin);

            using (var deflate = new DeflateStream(input, CompressionMode.Decompress, true))
                extractedSize = deflate.Read(extracted, 0, extracted.Length);

            output.Seek(0, SeekOrigin.Begin);
            output.Write(extracted, 0, extractedSize);

            return true;
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

                var numCharas = Enum.GetValues(typeof(Chara)).Length;
                if (allScoreData.ClearData.Count == numCharas)
                    return allScoreData;
                else
                    return null;
            }
        }

        // serialNumber: 0-based
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
        private static (Chara Chara, int CardId)? GetCharaCardIdPair(Chara chara, CardType type, int serialNumber)
        {
            if (type == CardType.System)
                return null;

            Func<(Chara Chara, int CardId), bool> matchesCharaAndType;
            if (type == CardType.Skill)
                matchesCharaAndType = (pair => (pair.Chara == chara) && (pair.CardId / 100 == 1));
            else
                matchesCharaAndType = (pair => (pair.Chara == chara) && (pair.CardId / 100 == 2));

            return Definitions.CardNameTable.Keys.Where(matchesCharaAndType).ElementAtOrDefault(serialNumber);
        }

        // %T105C[xxx][yy][z]
        private class CareerReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T105C(\d{{3}})({0})([1-3])", Parsers.CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CareerReplacer(IReadOnlyDictionary<Chara, ClearData<Chara, Th105.Level>> clearDataDictionary)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    Func<SpellCardResult, long> getValue;
                    Func<long, string> toString;
                    if (type == 1)
                    {
                        getValue = (result => result.GotCount);
                        toString = Utils.ToNumberString;
                    }
                    else if (type == 2)
                    {
                        getValue = (result => result.TrialCount);
                        toString = Utils.ToNumberString;
                    }
                    else
                    {
                        getValue = (result => result.Frames);
                        toString = (value =>
                        {
                            var time = new Time(value);
                            return Utils.Format(
                                "{0:D2}:{1:D2}.{2:D3}",
                                (time.Hours * 60) + time.Minutes,
                                time.Seconds,
                                (time.Frames * 1000) / 60);
                        });
                    }

                    var clearData = clearDataDictionary[chara];
                    if (number == 0)
                    {
                        return toString(clearData.SpellCardResults.Values.Sum(getValue));
                    }
                    else
                    {
                        var numLevels = Enum.GetValues(typeof(Th105.Level)).Length;
                        var index = (number - 1) / numLevels;
                        if ((index >= 0) && (index < Definitions.EnemyCardIdTable[chara].Count()))
                        {
                            var (enemy, cardId) = Definitions.EnemyCardIdTable[chara].ElementAt(index);
                            var key = (enemy, (cardId * numLevels) + ((number - 1) % numLevels));
                            return toString(getValue(clearData.SpellCardResults[key]));
                        }
                        else
                        {
                            return match.ToString();
                        }
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T105CARD[xxx][yy][z]
        private class CardReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T105CARD(\d{{3}})({0})([NR])", Parsers.CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public CardReplacer(
                IReadOnlyDictionary<Chara, ClearData<Chara, Th105.Level>> clearDataDictionary, bool hideUntriedCards)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var number = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
                    var type = match.Groups[3].Value.ToUpperInvariant();

                    if (number <= 0)
                        return match.ToString();

                    var numLevels = Enum.GetValues(typeof(Th105.Level)).Length;
                    var index = (number - 1) / numLevels;
                    if ((index >= 0) && (index < Definitions.EnemyCardIdTable[chara].Count()))
                    {
                        var level = (Th105.Level)((number - 1) % numLevels);
                        var enemyCardIdPair = Definitions.EnemyCardIdTable[chara].ElementAt(index);
                        if (hideUntriedCards)
                        {
                            var clearData = clearDataDictionary[chara];
                            var key = (
                                enemyCardIdPair.Enemy,
                                (enemyCardIdPair.CardId * numLevels) + (int)level);
                            if (clearData.SpellCardResults[key].TrialCount <= 0)
                                return (type == "N") ? "??????????" : "?????";
                        }

                        return (type == "N") ? Definitions.CardNameTable[enemyCardIdPair] : level.ToString();
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

        // %T105CRG[x][yy][z]
        private class CollectRateReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T105CRG({0})({1})([1-2])", Parsers.LevelWithTotalParser.Pattern, Parsers.CharaParser.Pattern);

            private readonly MatchEvaluator evaluator;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Reviewed.")]
            public CollectRateReplacer(IReadOnlyDictionary<Chara, ClearData<Chara, Th105.Level>> clearDataDictionary)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var level = Parsers.LevelWithTotalParser.Parse(match.Groups[1].Value);
                    var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
                    var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                    Func<KeyValuePair<(Chara, int), SpellCardResult>, bool> findByLevel;
                    if (level == Th105.LevelWithTotal.Total)
                        findByLevel = (pair => true);
                    else
                        findByLevel = (pair => pair.Value.Level == (Th105.Level)level);

                    Func<KeyValuePair<(Chara, int), SpellCardResult>, bool> countByType;
                    if (type == 1)
                        countByType = (pair => pair.Value.GotCount > 0);
                    else
                        countByType = (pair => pair.Value.TrialCount > 0);

                    return Utils.ToNumberString(clearDataDictionary[chara]
                        .SpellCardResults.Where(findByLevel).Count(countByType));
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }

        // %T105DC[ww][x][yy][z]
        private class CardForDeckReplacer : IStringReplaceable
        {
            private static readonly string Pattern = Utils.Format(
                @"%T105DC({0})({1})(\d{{2}})([NC])", Parsers.CharaParser.Pattern, Parsers.CardTypeParser.Pattern);

            private readonly MatchEvaluator evaluator;

            public CardForDeckReplacer(
                IReadOnlyDictionary<int, CardForDeck> systemCards,
                IReadOnlyDictionary<Chara, ClearData<Chara, Th105.Level>> clearDataDictionary,
                bool hideUntriedCards)
            {
                this.evaluator = new MatchEvaluator(match =>
                {
                    var chara = Parsers.CharaParser.Parse(match.Groups[1].Value);
                    var cardType = Parsers.CardTypeParser.Parse(match.Groups[2].Value);
                    var number = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);
                    var type = match.Groups[4].Value.ToUpperInvariant();

                    if (cardType == CardType.System)
                    {
                        if (Definitions.SystemCardNameTable.ContainsKey(number - 1))
                        {
                            var card = systemCards[number - 1];
                            if (type == "N")
                            {
                                if (hideUntriedCards)
                                {
                                    if (card.MaxNumber <= 0)
                                        return "??????????";
                                }

                                return Definitions.SystemCardNameTable[number - 1];
                            }
                            else
                            {
                                return Utils.ToNumberString(card.MaxNumber);
                            }
                        }
                        else
                        {
                            return match.ToString();
                        }
                    }
                    else
                    {
                        var key = GetCharaCardIdPair(chara, cardType, number - 1);
                        if (key != null)
                        {
                            var card = clearDataDictionary[key.Value.Chara].CardsForDeck[key.Value.CardId];
                            if (type == "N")
                            {
                                if (hideUntriedCards)
                                {
                                    if (card.MaxNumber <= 0)
                                        return "??????????";
                                }

                                return Definitions.CardNameTable[key.Value];
                            }
                            else
                            {
                                return Utils.ToNumberString(card.MaxNumber);
                            }
                        }
                        else
                        {
                            return match.ToString();
                        }
                    }
                });
            }

            public string Replace(string input)
            {
                return Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
            }
        }
    }
}
