//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Core.Models.Th123;
using CardType = ThScoreFileConverter.Core.Models.Th105.CardType;
using StageInfo = ThScoreFileConverter.Core.Models.Th105.StageInfo<ThScoreFileConverter.Core.Models.Th123.Chara>;

namespace ThScoreFileConverter.Models.Th123;

internal static class Definitions
{
    public static IReadOnlyDictionary<int, string> SystemCardNameTable { get; } =
        Core.Models.Th123.Definitions.SystemCardNameTable;

    public static IReadOnlyDictionary<(Chara Chara, int CardId), string> CardNameTable { get; } =
        Core.Models.Th123.Definitions.CardNameTable;

    public static IReadOnlyDictionary<Chara, IReadOnlyDictionary<CardType, IReadOnlyList<int>>> CardOrderTable { get; } =
        new Dictionary<Chara, IReadOnlyDictionary<CardType, IReadOnlyList<int>>>
        {
            {
                Chara.Reimu,
                new Dictionary<CardType, IReadOnlyList<int>>
                {
                    {
                        CardType.Skill,
                        new[] { 100, 104, 108, 103, 107, 111, 101, 105, 109, 102, 106, 110 }
                    },
                    {
                        CardType.Spell,
                        new[] { 208, 200, 204, 209, 207, 214, 206, 210, 201, 219 }
                    },
                }
            },
            {
                Chara.Marisa,
                new Dictionary<CardType, IReadOnlyList<int>>
                {
                    {
                        CardType.Skill,
                        new[] { 103, 107, 111, 101, 105, 109, 100, 104, 108, 102, 106, 110 }
                    },
                    {
                        CardType.Spell,
                        new[] { 208, 205, 211, 215, 200, 206, 209, 212, 204, 214, 202, 203, 207, 219 }
                    },
                }
            },
            {
                Chara.Sakuya,
                new Dictionary<CardType, IReadOnlyList<int>>
                {
                    {
                        CardType.Skill,
                        new[] { 102, 106, 110, 100, 104, 108, 101, 105, 109, 103, 107, 111 }
                    },
                    {
                        CardType.Spell,
                        new[] { 200, 206, 207, 201, 202, 208, 211, 212, 203, 205, 209, 210, 204 }
                    },
                }
            },
            {
                Chara.Alice,
                new Dictionary<CardType, IReadOnlyList<int>>
                {
                    {
                        CardType.Skill,
                        new[] { 100, 104, 108, 101, 105, 109, 102, 106, 110, 103, 107, 111 }
                    },
                    {
                        CardType.Spell,
                        new[] { 200, 201, 202, 206, 209, 203, 208, 204, 205, 207, 210, 211 }
                    },
                }
            },
            {
                Chara.Patchouli,
                new Dictionary<CardType, IReadOnlyList<int>>
                {
                    {
                        CardType.Skill,
                        new[] { 100, 105, 110, 102, 107, 112, 103, 108, 113, 104, 109, 114, 101, 106, 111 }
                    },
                    {
                        CardType.Spell,
                        new[] { 201, 202, 200, 206, 207, 210, 211, 204, 212, 203, 205, 213 }
                    },
                }
            },
            {
                Chara.Youmu,
                new Dictionary<CardType, IReadOnlyList<int>>
                {
                    {
                        CardType.Skill,
                        new[] { 100, 104, 108, 101, 105, 109, 102, 106, 110, 103, 107, 111 }
                    },
                    {
                        CardType.Spell,
                        new[] { 200, 202, 201, 206, 212, 204, 207, 203, 205, 208 }
                    },
                }
            },
            {
                Chara.Remilia,
                new Dictionary<CardType, IReadOnlyList<int>>
                {
                    {
                        CardType.Skill,
                        new[] { 100, 104, 108, 102, 106, 110, 101, 105, 109, 103, 107, 111 }
                    },
                    {
                        CardType.Spell,
                        new[] { 201, 202, 200, 206, 207, 204, 208, 209, 203, 205 }
                    },
                }
            },
            {
                Chara.Yuyuko,
                new Dictionary<CardType, IReadOnlyList<int>>
                {
                    {
                        CardType.Skill,
                        new[] { 101, 105, 109, 103, 107, 111, 100, 104, 108, 102, 106, 110 }
                    },
                    {
                        CardType.Spell,
                        new[] { 201, 200, 202, 206, 208, 219, 203, 205, 207, 204, 209 }
                    },
                }
            },
            {
                Chara.Yukari,
                new Dictionary<CardType, IReadOnlyList<int>>
                {
                    {
                        CardType.Skill,
                        new[] { 100, 104, 108, 101, 105, 109, 102, 106, 110, 103, 107, 111 }
                    },
                    {
                        CardType.Spell,
                        new[] { 202, 204, 207, 200, 201, 205, 206, 203, 208, 215 }
                    },
                }
            },
            {
                Chara.Suika,
                new Dictionary<CardType, IReadOnlyList<int>>
                {
                    {
                        CardType.Skill,
                        new[] { 100, 104, 108, 101, 105, 109, 102, 106, 110, 103, 107, 111 }
                    },
                    {
                        CardType.Spell,
                        new[] { 206, 200, 201, 202, 212, 204, 205, 207, 203, 208 }
                    },
                }
            },
            {
                Chara.Reisen,
                new Dictionary<CardType, IReadOnlyList<int>>
                {
                    {
                        CardType.Skill,
                        new[] { 100, 104, 108, 102, 106, 110, 101, 105, 109, 103, 107, 111 }
                    },
                    {
                        CardType.Spell,
                        new[] { 200, 206, 208, 211, 202, 203, 207, 209, 210, 204, 205 }
                    },
                }
            },
            {
                Chara.Aya,
                new Dictionary<CardType, IReadOnlyList<int>>
                {
                    {
                        CardType.Skill,
                        new[] { 100, 104, 108, 101, 105, 109, 103, 107, 111, 102, 106, 110 }
                    },
                    {
                        CardType.Spell,
                        new[] { 211, 205, 207, 200, 203, 212, 202, 208, 201, 206 }
                    },
                }
            },
            {
                Chara.Komachi,
                new Dictionary<CardType, IReadOnlyList<int>>
                {
                    {
                        CardType.Skill,
                        new[] { 100, 104, 108, 101, 105, 109, 103, 107, 111, 102, 106, 110 }
                    },
                    {
                        CardType.Spell,
                        new[] { 200, 202, 203, 205, 211, 206, 207, 201, 204 }
                    },
                }
            },
            {
                Chara.Iku,
                new Dictionary<CardType, IReadOnlyList<int>>
                {
                    {
                        CardType.Skill,
                        new[] { 100, 104, 108, 101, 105, 109, 103, 107, 111, 102, 106, 110 }
                    },
                    {
                        CardType.Spell,
                        new[] { 200, 208, 206, 201, 202, 203, 209, 207, 211, 210 }
                    },
                }
            },
            {
                Chara.Tenshi,
                new Dictionary<CardType, IReadOnlyList<int>>
                {
                    {
                        CardType.Skill,
                        new[] { 102, 106, 110, 103, 107, 111, 100, 104, 108, 101, 105, 109 }
                    },
                    {
                        CardType.Spell,
                        new[] { 200, 201, 202, 208, 209, 204, 206, 203, 205, 207 }
                    },
                }
            },
            {
                Chara.Sanae,
                new Dictionary<CardType, IReadOnlyList<int>>
                {
                    {
                        CardType.Skill,
                        new[] { 100, 104, 108, 102, 106, 110, 103, 107, 111, 101, 105, 109 }
                    },
                    {
                        CardType.Spell,
                        new[] { 200, 201, 203, 204, 206, 202, 207, 205, 210 }
                    },
                }
            },
            {
                Chara.Cirno,
                new Dictionary<CardType, IReadOnlyList<int>>
                {
                    {
                        CardType.Skill,
                        new[] { 100, 104, 108, 103, 107, 111, 101, 105, 109, 102, 106, 110 }
                    },
                    {
                        CardType.Spell,
                        new[] { 200, 205, 202, 203, 207, 210, 213, 201, 206, 204, 208 }
                    },
                }
            },
            {
                Chara.Meiling,
                new Dictionary<CardType, IReadOnlyList<int>>
                {
                    {
                        CardType.Skill,
                        new[] { 103, 107, 111, 101, 105, 109, 100, 104, 108, 102, 106, 110 }
                    },
                    {
                        CardType.Spell,
                        new[] { 200, 206, 202, 204, 207, 201, 208, 211, 203, 205, 209 }
                    },
                }
            },
            {
                Chara.Utsuho,
                new Dictionary<CardType, IReadOnlyList<int>>
                {
                    {
                        CardType.Skill,
                        new[] { 101, 105, 109, 100, 104, 108, 103, 107, 111, 102, 106, 110 }
                    },
                    {
                        CardType.Spell,
                        new[] { 206, 211, 213, 200, 203, 205, 208, 204, 207, 209, 210, 212, 201, 214 }
                    },
                }
            },
            {
                Chara.Suwako,
                new Dictionary<CardType, IReadOnlyList<int>>
                {
                    {
                        CardType.Skill,
                        new[] { 102, 106, 110, 101, 105, 109, 100, 104, 108, 103, 107, 111 }
                    },
                    {
                        CardType.Spell,
                        new[] { 201, 204, 200, 202, 203, 205, 206, 207, 209, 212, 208 }
                    },
                }
            },
        };

    public static IReadOnlyDictionary<Chara, IReadOnlyList<StageInfo>> StageInfoTable { get; } =
        Core.Models.Th123.Definitions.StageInfoTable;

    public static IReadOnlyDictionary<Chara, IEnumerable<(Chara Enemy, int CardId)>> EnemyCardIdTable { get; } =
        StageInfoTable.ToDictionary(
            stageInfoPair => stageInfoPair.Key,
            stageInfoPair => stageInfoPair.Value.SelectMany(
                stageInfo => stageInfo.CardIds.Select(id => (stageInfo.Enemy, id))));

    public static string FormatPrefix { get; } = "%T123";

    public static bool HasStory(Chara chara)
    {
        return chara is Chara.Sanae or Chara.Cirno or Chara.Meiling;
    }
}
