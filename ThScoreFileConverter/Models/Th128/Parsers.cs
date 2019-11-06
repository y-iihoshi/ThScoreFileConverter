//-----------------------------------------------------------------------
// <copyright file="Parsers.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th128
{
    internal static class Parsers
    {
        public static EnumShortNameParser<Level> LevelParser { get; } =
            new EnumShortNameParser<Level>();

        public static EnumShortNameParser<LevelWithTotal> LevelWithTotalParser { get; } =
            new EnumShortNameParser<LevelWithTotal>();

        public static EnumShortNameParser<Route> RouteParser { get; } =
            new EnumShortNameParser<Route>();

        public static EnumShortNameParser<RouteWithTotal> RouteWithTotalParser { get; } =
            new EnumShortNameParser<RouteWithTotal>();

        public static EnumShortNameParser<StageWithTotal> StageWithTotalParser { get; } =
            new EnumShortNameParser<StageWithTotal>();
    }
}
