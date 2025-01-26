//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models.Th125;

namespace ThScoreFileConverter.Models.Th125;

internal static class Definitions
{
    public static IReadOnlyDictionary<(Level Level, int Scene), (Enemy Enemy, string Card)> SpellCards { get; } =
        Core.Models.Th125.Definitions.SpellCards;

    public static string FormatPrefix { get; } = "%T125";
}
