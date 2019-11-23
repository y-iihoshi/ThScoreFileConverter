//-----------------------------------------------------------------------
// <copyright file="Parsers.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Linq;
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Models.Th143
{
    internal static class Parsers
    {
        public static EnumShortNameParser<Day> DayParser { get; } =
            new EnumShortNameParser<Day>();

        public static EnumShortNameParser<ItemWithTotal> ItemWithTotalParser { get; } =
            new EnumShortNameParser<ItemWithTotal>();

        public static string DayLongPattern { get; } =
            string.Join("|", Utils.GetEnumerator<Day>().Select(day => day.ToLongName()).ToArray());
    }
}
