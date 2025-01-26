//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models.Th075;

namespace ThScoreFileConverter.Models.Th075;

internal static class Definitions
{
    public static string CharTable { get; } =
        @"ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
        @"abcdefghijklmnopqrstuvwxyz" +
        @"0123456789+-/*=%#!?.,:;_@$" +
        @"(){}[]<>&\|~^             ";

    public static IReadOnlyDictionary<int, SpellCardInfo> CardTable { get; } = Core.Models.Th075.Definitions.CardTable;

    public static IReadOnlyDictionary<Chara, IEnumerable<int>> CardIdTable { get; } = Core.Models.Th075.Definitions.CardIdTable;

    public static string FormatPrefix { get; } = "%T75";
}
