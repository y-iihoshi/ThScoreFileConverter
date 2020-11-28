//-----------------------------------------------------------------------
// <copyright file="Parsers.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th165
{
    /// <summary>
    /// Provides the parsers used for VD.
    /// </summary>
    internal static class Parsers
    {
        /// <summary>
        /// Gets the parser of <see cref="Day"/>.
        /// </summary>
        public static EnumShortNameParser<Day> DayParser { get; } =
            new EnumShortNameParser<Day>();

        /// <summary>
        /// Gets the pattern used for parsing as a long name of a <see cref="Day"/> enumerator.
        /// </summary>
        public static string DayLongPattern { get; } =
            string.Join("|", EnumHelper.GetEnumerable<Day>().Select(day => day.ToLongName()).ToArray());
    }
}
