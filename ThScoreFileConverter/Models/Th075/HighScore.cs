//-----------------------------------------------------------------------
// <copyright file="HighScore.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;
using CommunityToolkit.Diagnostics;
using ThScoreFileConverter.Core.Resources;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Models.Th075;

internal sealed class HighScore : IBinaryReadable, IHighScore
{
    public HighScore()
    {
        this.Name = string.Empty;
    }

    public string Name { get; private set; }

    public byte Month { get; private set; }     // 1-based

    public byte Day { get; private set; }       // 1-based

    public int Score { get; private set; }

    public void ReadFrom(BinaryReader reader)
    {
        this.Name = new string(reader.ReadExactBytes(8).Select(ch => Definitions.CharTable[ch]).ToArray());

        this.Month = reader.ReadByte();
        this.Day = reader.ReadByte();
        if ((this.Month == 0) && (this.Day == 0))
        {
            // It's allowed.
        }
        else
        {
            if (this.Month is <= 0 or > 12)
            {
                ThrowHelper.ThrowInvalidDataException(
                    StringHelper.Format(ExceptionMessages.InvalidDataExceptionPropertyIsOutOfRange, nameof(this.Month)));
            }

            if ((this.Day <= 0) || (this.Day > DateTime.DaysInMonth(2000, this.Month)))
            {
                ThrowHelper.ThrowInvalidDataException(
                    StringHelper.Format(ExceptionMessages.InvalidDataExceptionPropertyIsOutOfRange, nameof(this.Day)));
            }
        }

        _ = reader.ReadUInt16();    // always 0x0000?
        this.Score = reader.ReadInt32();
    }
}
