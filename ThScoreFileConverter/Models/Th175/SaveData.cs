//-----------------------------------------------------------------------
// <copyright file="SaveData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Squirrel;

namespace ThScoreFileConverter.Models.Th175;

internal class SaveData
{
    public SaveData()
    {
        this.ScoreDictionary = ImmutableDictionary<(Level, Chara), IEnumerable<int>>.Empty;
        this.TimeDictionary = ImmutableDictionary<(Level, Chara), IEnumerable<int>>.Empty;
        this.SpellDictionary = ImmutableDictionary<(Level, Chara), IEnumerable<int>>.Empty;
        this.TutorialCountDictionary = ImmutableDictionary<Chara, int>.Empty;
        this.ReachedStageDictionary = ImmutableDictionary<Chara, int>.Empty;
        this.UseCountDictionary = ImmutableDictionary<Chara, int>.Empty;
        this.RetireCountDictionary = ImmutableDictionary<Chara, int>.Empty;
        this.ClearCountDictionary = ImmutableDictionary<Chara, int>.Empty;
        this.PerfectClearCountDictionary = ImmutableDictionary<Chara, int>.Empty;
        this.EndingCountDictionary = ImmutableDictionary<Chara, int>.Empty;
        this.BgmPlayCountDictionary = ImmutableDictionary<string, int>.Empty;
    }

    public SaveData(SQTable table)
    {
        this.ScoreDictionary = new Dictionary<Level, IDictionary<Chara, IEnumerable<int>>>
        {
            { Level.Easy, GetCharaIntArrayDictionary(table, "score_easy") },
            { Level.Normal, GetCharaIntArrayDictionary(table, "score_normal") },
            { Level.Hard, GetCharaIntArrayDictionary(table, "score_hard") },
            { Level.Rush, GetCharaIntArrayDictionary(table, "score_rush") },
        }.SelectMany(pair1 => pair1.Value.Select(pair2 => (Level: pair1.Key, Chara: pair2.Key, Array: pair2.Value)))
         .ToDictionary(tuple => (tuple.Level, tuple.Chara), tuple => tuple.Array);

        this.TimeDictionary = new Dictionary<Level, IDictionary<Chara, IEnumerable<int>>>
        {
            { Level.Easy, GetCharaIntArrayDictionary(table, "time_easy") },
            { Level.Normal, GetCharaIntArrayDictionary(table, "time_normal") },
            { Level.Hard, GetCharaIntArrayDictionary(table, "time_hard") },
            { Level.Rush, GetCharaIntArrayDictionary(table, "time_rush") },
        }.SelectMany(pair1 => pair1.Value.Select(pair2 => (Level: pair1.Key, Chara: pair2.Key, Array: pair2.Value)))
         .ToDictionary(tuple => (tuple.Level, tuple.Chara), tuple => tuple.Array);

        // FIXME: The actual data structure is still unknown...
        this.SpellDictionary = new Dictionary<Level, IDictionary<Chara, IEnumerable<int>>>
        {
            { Level.Easy, GetCharaIntArrayDictionary(table, "spell_easy") },
            { Level.Normal, GetCharaIntArrayDictionary(table, "spell_normal") },
            { Level.Hard, GetCharaIntArrayDictionary(table, "spell_hard") },
            { Level.Rush, GetCharaIntArrayDictionary(table, "spell_rush") },
        }.SelectMany(pair1 => pair1.Value.Select(pair2 => (Level: pair1.Key, Chara: pair2.Key, Array: pair2.Value)))
         .ToDictionary(tuple => (tuple.Level, tuple.Chara), tuple => tuple.Array);

        this.TutorialCountDictionary = GetCharaIntDictionary(table, "tutorial_count");
        this.ReachedStageDictionary = GetCharaIntDictionary(table, "character_reach_stage");
        this.UseCountDictionary = GetCharaIntDictionary(table, "character_use_count");
        this.RetireCountDictionary = GetCharaIntDictionary(table, "character_retire_count");
        this.ClearCountDictionary = GetCharaIntDictionary(table, "character_clear_count");
        this.PerfectClearCountDictionary = GetCharaIntDictionary(table, "character_perfect_clear_count");
        this.EndingCountDictionary = GetCharaIntDictionary(table, "ending_count");
        this.BgmPlayCountDictionary = GetStringIntDictionary(table, "bgm_play_count");
    }

    public IReadOnlyDictionary<(Level Level, Chara Chara), IEnumerable<int>> ScoreDictionary { get; private set; }

    public IReadOnlyDictionary<(Level Level, Chara Chara), IEnumerable<int>> TimeDictionary { get; private set; }

    public IReadOnlyDictionary<(Level Level, Chara Chara), IEnumerable<int>> SpellDictionary { get; private set; }

    public IReadOnlyDictionary<Chara, int> TutorialCountDictionary { get; private set; }

    public IReadOnlyDictionary<Chara, int> ReachedStageDictionary { get; private set; }

    public IReadOnlyDictionary<Chara, int> UseCountDictionary { get; private set; }

    public IReadOnlyDictionary<Chara, int> RetireCountDictionary { get; private set; }

    public IReadOnlyDictionary<Chara, int> ClearCountDictionary { get; private set; }

    public IReadOnlyDictionary<Chara, int> PerfectClearCountDictionary { get; private set; }

    public IReadOnlyDictionary<Chara, int> EndingCountDictionary { get; private set; }

    public IReadOnlyDictionary<string, int> BgmPlayCountDictionary { get; private set; }

    private static Chara? ParseStoryChara(SQObject obj)
    {
        if (obj is SQString str)
        {
            return str.Value switch
            {
                "reimu"   => Chara.Reimu,
                "marisa"  => Chara.Marisa,
                "kanako"  => Chara.Kanako,
                "murasa"  => Chara.Minamitsu,
                "jyoon"   => Chara.JoonShion,
                "flandre" => Chara.Flandre,
                _         => null,
            };
        }
        else
        {
            return null;
        }
    }

    private static Dictionary<TKey, TValue> GetDictionary<TKey, TValue>(
        SQTable table,
        string key,
        Func<SQObject, bool> keyPredicate,
        Func<SQObject, bool> valuePredicate,
        Func<SQObject, TKey> keyConverter,
        Func<SQObject, TValue> valueConverter)
        where TKey : notnull
    {
        return table.Value.TryGetValue(new SQString(key), out var value) && (value is SQTable valueTable)
            ? valueTable.ToDictionary(keyPredicate, valuePredicate, keyConverter, valueConverter)
            : new Dictionary<TKey, TValue>();
    }

    private static Dictionary<Chara, int> GetCharaIntDictionary(SQTable table, string key)
    {
        return GetDictionary(
            table,
            key,
            valuePairKey => ParseStoryChara(valuePairKey) is not null,
            valuePairValue => valuePairValue is SQInteger,
            valuePairKey => ParseStoryChara(valuePairKey)!.Value,
            valuePairValue => (int)(SQInteger)valuePairValue);
    }

    private static Dictionary<Chara, IEnumerable<int>> GetCharaIntArrayDictionary(SQTable table, string key)
    {
        return GetDictionary(
            table,
            key,
            valuePairKey => ParseStoryChara(valuePairKey) is not null,
            valuePairValue => (valuePairValue is SQArray array) && (array.Value.FirstOrDefault() is SQInteger),
            valuePairKey => ParseStoryChara(valuePairKey)!.Value,
            valuePairValue => ((SQArray)valuePairValue).Value.Select(element => (int)(SQInteger)element));
    }

    private static Dictionary<string, int> GetStringIntDictionary(SQTable table, string key)
    {
        return GetDictionary(
            table,
            key,
            valuePairKey => valuePairKey is SQString,
            valuePairValue => valuePairValue is SQInteger,
            valuePairKey => (string)(SQString)valuePairKey,
            valuePairValue => (int)(SQInteger)valuePairValue);
    }
}
