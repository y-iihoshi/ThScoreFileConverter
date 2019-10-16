//-----------------------------------------------------------------------
// <copyright file="Parsers.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Linq;
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Models.Th095
{
    internal static class Parsers
    {
        public static EnumShortNameParser<Level> LevelParser { get; } =
            new EnumShortNameParser<Level>();

        public static string LevelLongPattern { get; } =
            string.Join("|", Utils.GetEnumerator<Level>().Select(level => level.ToLongName()).ToArray());
    }
}
