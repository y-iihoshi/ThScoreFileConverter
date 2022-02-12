//-----------------------------------------------------------------------
// <copyright file="AllScoreData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Squirrel;

namespace ThScoreFileConverter.Models.Th155
{
    internal class AllScoreData : IBinaryReadable
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

        private static StoryChara? ParseStoryChara(SQObject obj)
        {
            if (obj is SQString str)
            {
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
            if (this.allData.Value.TryGetValue(new SQString("story"), out var story))
            {
                if (story is SQTable table)
                {
                    this.StoryDictionary = table.Value
                        .Where(pair => ParseStoryChara(pair.Key) is not null)
                        .ToDictionary(pair => ParseStoryChara(pair.Key)!.Value, pair => ParseStory(pair.Value));
                }
            }
        }

        private void ParseCharacterDictionary()
        {
            if (this.allData.Value.TryGetValue(new SQString("character"), out var character))
            {
                if (character is SQTable table)
                {
                    this.CharacterDictionary = table.Value
                        .Where(pair => (pair.Key is SQString) && (pair.Value is SQInteger))
                        .ToDictionary(pair => (string)(SQString)pair.Key, pair => (int)(SQInteger)pair.Value);
                }
            }
        }

        private void ParseBgmDictionary()
        {
            if (this.allData.Value.TryGetValue(new SQString("bgm"), out var bgm))
            {
                if (bgm is SQTable table)
                {
                    this.BgmDictionary = table.Value
                        .Where(pair => (pair.Key is SQInteger) && (pair.Value is SQBool))
                        .ToDictionary(pair => (int)(SQInteger)pair.Key, pair => (bool)(SQBool)pair.Value);
                }
            }
        }

        private void ParseEndingDictionary()
        {
            if (this.allData.Value.TryGetValue(new SQString("ed"), out var ed))
            {
                if (ed is SQTable table)
                {
                    this.EndingDictionary = table.Value
                        .Where(pair => (pair.Key is SQString) && (pair.Value is SQInteger))
                        .ToDictionary(pair => (string)(SQString)pair.Key, pair => (int)(SQInteger)pair.Value);
                }
            }
        }

        private void ParseStageDictionary()
        {
            if (this.allData.Value.TryGetValue(new SQString("stage"), out var stage))
            {
                if (stage is SQTable table)
                {
                    this.StageDictionary = table.Value
                        .Where(pair => (pair.Key is SQInteger) && (pair.Value is SQInteger))
                        .ToDictionary(pair => (int)(SQInteger)pair.Key, pair => (int)(SQInteger)pair.Value);
                }
            }
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
}
