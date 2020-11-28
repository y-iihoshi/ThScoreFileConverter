//-----------------------------------------------------------------------
// <copyright file="AllScoreData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th075
{
    internal class AllScoreData : IBinaryReadable
    {
        public AllScoreData()
        {
            var numCharas = Enum.GetValues(typeof(CharaWithReserved)).Length;
            var numLevels = Enum.GetValues(typeof(Level)).Length;
            this.ClearData = new Dictionary<(CharaWithReserved, Level), IClearData>(numCharas * numLevels);
        }

        public IReadOnlyDictionary<(CharaWithReserved chara, Level level), IClearData> ClearData { get; private set; }

        public Status? Status { get; private set; }

        public void ReadFrom(BinaryReader reader)
        {
            var levels = EnumHelper.GetEnumerable<Level>();

            this.ClearData = EnumHelper.GetEnumerable<CharaWithReserved>()
                .SelectMany(chara => levels.Select(level => (chara, level)))
                .ToDictionary(pair => pair, pair =>
                {
                    var clearData = new ClearData();
                    clearData.ReadFrom(reader);
                    return clearData as IClearData;
                });

            var status = new Status();
            status.ReadFrom(reader);
            this.Status = status;
        }
    }
}
