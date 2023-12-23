//-----------------------------------------------------------------------
// <copyright file="AllScoreData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using ThScoreFileConverter.Core.Models.Th155;
using ThScoreFileConverter.Squirrel;

namespace ThScoreFileConverter.Models.Th155;

internal sealed class AllScoreData : IBinaryReadable
{
    private SQTable allData;

    public AllScoreData()
    {
        this.allData = new SQTable();
        this.StoryDictionary = ImmutableDictionary<StoryChara, Story>.Empty;
        this.CharacterDictionary = ImmutableDictionary<string, int>.Empty;
        this.BgmDictionary = ImmutableDictionary<int, bool>.Empty;
        this.EndingDictionary = ImmutableDictionary<string, int>.Empty;
        this.StageDictionary = ImmutableDictionary<int, int>.Empty;
    }

    public IReadOnlyDictionary<StoryChara, Story> StoryDictionary { get; private set; }

    public IReadOnlyDictionary<string, int> CharacterDictionary { get; private set; }

    public IReadOnlyDictionary<int, bool> BgmDictionary { get; private set; }

    public IReadOnlyDictionary<string, int> EndingDictionary { get; private set; }

    public IReadOnlyDictionary<int, int> StageDictionary { get; private set; }

    public int Version { get; private set; }

    public void ReadFrom(BinaryReader reader)
    {
        this.allData = SQTable.Create(reader, true);

        this.ParseStoryDictionary();
        this.ParseCharacterDictionary();
        this.ParseBgmDictionary();
        this.ParseEndingDictionary();
        this.ParseStageDictionary();
        this.ParseVersion();
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
            : [];
    }

    private static StoryChara? ParseStoryChara(SQObject obj)
    {
        if (obj is SQString str)
        {
#pragma warning disable format
            var chara = str.Value switch
            {
                "reimu"   => StoryChara.ReimuKasen,
                "marisa"  => StoryChara.MarisaKoishi,
                "nitori"  => StoryChara.NitoriKokoro,
                "usami"   => StoryChara.SumirekoDoremy,
                "tenshi"  => StoryChara.TenshiShinmyoumaru,
                "miko"    => StoryChara.MikoByakuren,
                "yukari"  => StoryChara.YukariReimu,
                "mamizou" => StoryChara.MamizouMokou,
                "udonge"  => StoryChara.ReisenDoremy,
                "futo"    => StoryChara.FutoIchirin,
                "jyoon"   => StoryChara.JoonShion,
                _         => (StoryChara?)null,
            };
#pragma warning restore format
            return chara;
        }
        else
        {
            return null;
        }
    }

    private static Story ParseStory(SQObject obj)
    {
        return obj is SQTable dict
            ? new Story
            {
                Stage = dict.GetValueOrDefault<int>("stage"),
                Ed = (Levels)dict.GetValueOrDefault<int>("ed"),
                Available = dict.GetValueOrDefault<bool>("available"),
                OverDrive = dict.GetValueOrDefault<int>("overdrive"),
                StageOverDrive = dict.GetValueOrDefault<int>("stage_overdrive"),
            }
            : default;
    }

    private void ParseStoryDictionary()
    {
#pragma warning disable IDE0200 // Remove unnecessary lambda expression
        this.StoryDictionary = GetDictionary(
            this.allData,
            "story",
            key => ParseStoryChara(key) is not null,
            value => true,
            key => ParseStoryChara(key)!.Value,
            value => ParseStory(value));
#pragma warning restore IDE0200 // Remove unnecessary lambda expression
    }

    private void ParseCharacterDictionary()
    {
        this.CharacterDictionary = GetDictionary(
            this.allData,
            "character",
            key => key is SQString,
            value => value is SQInteger,
            key => (string)(SQString)key,
            value => (int)(SQInteger)value);
    }

    private void ParseBgmDictionary()
    {
        this.BgmDictionary = GetDictionary(
            this.allData,
            "bgm",
            key => key is SQInteger,
            value => value is SQBool,
            key => (int)(SQInteger)key,
            value => (bool)(SQBool)value);
    }

    private void ParseEndingDictionary()
    {
        this.EndingDictionary = GetDictionary(
            this.allData,
            "ed",
            key => key is SQString,
            value => value is SQInteger,
            key => (string)(SQString)key,
            value => (int)(SQInteger)value);
    }

    private void ParseStageDictionary()
    {
        this.StageDictionary = GetDictionary(
            this.allData,
            "stage",
            key => key is SQInteger,
            value => value is SQInteger,
            key => (int)(SQInteger)key,
            value => (int)(SQInteger)value);
    }

    private void ParseVersion()
    {
        this.Version = this.allData.GetValueOrDefault<int>("version");
    }

    public struct Story
    {
        public int Stage;
        public Levels Ed;
        public bool Available;
        public int OverDrive;
        public int StageOverDrive;
    }
}
