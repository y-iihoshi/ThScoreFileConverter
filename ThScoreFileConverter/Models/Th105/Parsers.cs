//-----------------------------------------------------------------------
// <copyright file="Parsers.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th105
{
    internal static class Parsers
    {
        public static EnumShortNameParser<LevelWithTotal> LevelWithTotalParser { get; } =
            new EnumShortNameParser<LevelWithTotal>();

        public static EnumShortNameParser<Chara> CharaParser { get; } =
            new EnumShortNameParser<Chara>();

        public static EnumShortNameParser<CardType> CardTypeParser { get; } =
            new EnumShortNameParser<CardType>();
    }
}
