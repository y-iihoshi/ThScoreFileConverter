//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models.Th165;

namespace ThScoreFileConverter.Models.Th165;

internal static class Definitions
{
    public static IReadOnlyDictionary<(Day Day, int Scene), (Enemy[] Enemies, string Card)> SpellCards { get; } =
        Core.Models.Th165.Definitions.SpellCards;

    public static IReadOnlyList<string> Nicknames { get; } = Core.Models.Th165.Definitions.Nicknames;

    public static string FormatPrefix { get; } = "%T165";
}
