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

namespace ThScoreFileConverter.Models.Th145
{
    internal class AllScoreData : IBinaryReadable
    {
        private SQTable allData;

        public AllScoreData()
        {
            this.allData = new SQTable();
            this.StoryClearFlags = ImmutableDictionary<Chara, Levels>.Empty;
            this.BgmFlags = ImmutableDictionary<int, bool>.Empty;
            this.ClearRanks = ImmutableDictionary<Level, IReadOnlyDictionary<Chara, int>>.Empty;
            this.ClearTimes = ImmutableDictionary<Level, IReadOnlyDictionary<Chara, int>>.Empty;
        }

        public int StoryProgress => this.GetValue<int>("story_progress");

        public IReadOnlyDictionary<Chara, Levels> StoryClearFlags { get; private set; }

        public int EndingCount => this.GetValue<int>("ed_count");

        public int Ending2Count => this.GetValue<int>("ed2_count");

        public bool IsEnabledStageTanuki1 => this.GetValue<bool>("enable_stage_tanuki1");

        public bool IsEnabledStageTanuki2 => this.GetValue<bool>("enable_stage_tanuki2");

        public bool IsEnabledStageKokoro => this.GetValue<bool>("enable_stage_kokoro");

        public bool IsEnabledSt27 => this.GetValue<bool>("enable_st27");

        public bool IsEnabledSt28 => this.GetValue<bool>("enable_st28");

        public bool IsPlayableMamizou => this.GetValue<bool>("enable_mamizou");

        public bool IsPlayableKokoro => this.GetValue<bool>("enable_kokoro");

        public IReadOnlyDictionary<int, bool> BgmFlags { get; private set; }

        public IReadOnlyDictionary<Level, IReadOnlyDictionary<Chara, int>> ClearRanks { get; private set; }

        public IReadOnlyDictionary<Level, IReadOnlyDictionary<Chara, int>> ClearTimes { get; private set; }

        public void ReadFrom(BinaryReader reader)
        {
            this.allData = SQTable.Create(reader, true);

            this.ParseStoryClear();
            this.ParseEnableBgm();
            this.ParseClearRank();
            this.ParseClearTime();
        }

        private void ParseStoryClear()
        {
            if (this.allData.Value.TryGetValue(new SQString("story_clear"), out var counts))
            {
                if (counts is SQArray storyClearFlags)
                {
                    this.StoryClearFlags = storyClearFlags.Value
                        .Select((flag, index) => (flag, index))
                        .Where(pair => pair.flag is SQInteger)
                        .ToDictionary(pair => (Chara)pair.index, pair => (Levels)(int)(SQInteger)pair.flag);
                }
            }
        }

        private void ParseEnableBgm()
        {
            if (this.allData.Value.TryGetValue(new SQString("enable_bgm"), out var flags))
            {
                if (flags is SQTable bgmFlags)
                {
                    this.BgmFlags = bgmFlags.Value
                        .Where(pair => (pair.Key is SQInteger) && (pair.Value is SQBool))
                        .ToDictionary(pair => (int)(SQInteger)pair.Key, pair => (bool)(SQBool)pair.Value);
                }
            }
        }

        private void ParseClearRank()
        {
            if (this.allData.Value.TryGetValue(new SQString("clear_rank"), out var ranks))
            {
                if (ranks is SQArray clearRanks)
                {
                    this.ClearRanks = clearRanks.Value
                        .Select((ranksPerChara, index) => (ranksPerChara, index))
                        .Where(levelPair => levelPair.ranksPerChara is SQArray)
                        .ToDictionary(
                            levelPair => (Level)levelPair.index,
                            levelPair => ((SQArray)levelPair.ranksPerChara).Value
                                .Select((rank, index) => (rank, index))
                                .Where(charaPair => charaPair.rank is SQInteger)
                                .ToDictionary(
                                    charaPair => (Chara)charaPair.index,
                                    charaPair => (int)(SQInteger)charaPair.rank)
                                as IReadOnlyDictionary<Chara, int>);
                }
            }
        }

        private void ParseClearTime()
        {
            if (this.allData.Value.TryGetValue(new SQString("clear_time"), out var times))
            {
                if (times is SQArray clearTimes)
                {
                    this.ClearTimes = clearTimes.Value
                        .Select((timesPerChara, index) => (timesPerChara, index))
                        .Where(levelPair => levelPair.timesPerChara is SQArray)
                        .ToDictionary(
                            levelPair => (Level)levelPair.index,
                            levelPair => ((SQArray)levelPair.timesPerChara).Value
                                .Select((time, index) => (time, index))
                                .Where(charaPair => charaPair.time is SQInteger)
                                .ToDictionary(
                                    charaPair => (Chara)charaPair.index,
                                    charaPair => (int)(SQInteger)charaPair.time)
                                as IReadOnlyDictionary<Chara, int>);
                }
            }
        }

        private T GetValue<T>(string key)
            where T : struct
        {
            T result = default;

            if (this.allData.Value.TryGetValue(new SQString(key), out var value))
            {
                switch (value)
                {
                    case SQBool sqbool:
                        if (result is bool)
                            result = (T)(object)(bool)sqbool;
                        break;
                    case SQInteger sqinteger:
                        if (result is int)
                            result = (T)(object)(int)sqinteger;
                        break;
                }
            }

            return result;
        }
    }
}
