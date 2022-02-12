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

namespace ThScoreFileConverter.Models.Th135
{
    internal class AllScoreData : IBinaryReadable
    {
        private SQTable allData;

        public AllScoreData()
        {
            this.allData = new SQTable();
            this.StoryClearFlags = ImmutableDictionary<Chara, Levels>.Empty;
            this.BgmFlags = ImmutableDictionary<int, bool>.Empty;
        }

        public int StoryProgress => this.allData.GetValueOrDefault<int>("story_progress");

        public IReadOnlyDictionary<Chara, Levels> StoryClearFlags { get; private set; }

        public int EndingCount => this.allData.GetValueOrDefault<int>("ed_count");

        public int Ending2Count => this.allData.GetValueOrDefault<int>("ed2_count");

        public bool IsEnabledStageTanuki1 => this.allData.GetValueOrDefault<bool>("enable_stage_tanuki1");

        public bool IsEnabledStageTanuki2 => this.allData.GetValueOrDefault<bool>("enable_stage_tanuki2");

        public bool IsEnabledStageKokoro => this.allData.GetValueOrDefault<bool>("enable_stage_kokoro");

        public bool IsPlayableMamizou => this.allData.GetValueOrDefault<bool>("enable_mamizou");

        public bool IsPlayableKokoro => this.allData.GetValueOrDefault<bool>("enable_kokoro");

        public IReadOnlyDictionary<int, bool> BgmFlags { get; private set; }

        public void ReadFrom(BinaryReader reader)
        {
            this.allData = SQTable.Create(reader, true);

            this.ParseStoryClear();
            this.ParseEnableBgm();
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
                    this.BgmFlags = bgmFlags.ToDictionary(
                        key => key is SQInteger,
                        value => value is SQBool,
                        key => (int)(SQInteger)key,
                        value => (bool)(SQBool)value);
                }
            }
        }
    }
}
