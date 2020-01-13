//-----------------------------------------------------------------------
// <copyright file="Parsers.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Linq;
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Models.Th125
{
    /// <summary>
    /// Provides the parsers used for DS.
    /// </summary>
    internal static class Parsers
    {
        /// <summary>
        /// Gets the parser of <see cref="Level"/>.
        /// </summary>
        public static EnumShortNameParser<Level> LevelParser { get; } =
            new EnumShortNameParser<Level>();

        /// <summary>
        /// Gets the parser of <see cref="Chara"/>.
        /// </summary>
        public static EnumShortNameParser<Chara> CharaParser { get; } =
            new EnumShortNameParser<Chara>();

        /// <summary>
        /// Gets the pattern used for parsing as a long name of a <see cref="Level"/> enumerator.
        /// </summary>
        public static string LevelLongPattern { get; } =
            string.Join("|", Utils.GetEnumerable<Level>().Select(lv => lv.ToLongName()).ToArray());
    }
}
