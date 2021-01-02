//-----------------------------------------------------------------------
// <copyright file="Status.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th075
{
    internal class Status : IBinaryReadable
    {
        public Status()
        {
            this.LastName = string.Empty;
            this.ArcadeScores = ImmutableDictionary<(CharaWithReserved, CharaWithReserved), int>.Empty;
        }

        public string LastName { get; private set; }

        public IReadOnlyDictionary<(CharaWithReserved player, CharaWithReserved enemy), int> ArcadeScores { get; private set; }

        public void ReadFrom(BinaryReader reader)
        {
            var charas = EnumHelper<CharaWithReserved>.Enumerable;

            this.LastName = new string(reader.ReadExactBytes(8).Select(ch => Definitions.CharTable[ch]).ToArray());
            this.ArcadeScores = charas.SelectMany(player => charas.Select(enemy => (player, enemy)))
                .ToDictionary(pair => pair, _ => reader.ReadInt32() - 10);

            // FIXME... BGM flags?
            _ = reader.ReadExactBytes(0x28);

            _ = reader.ReadExactBytes(0x100);
        }
    }
}
