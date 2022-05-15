//-----------------------------------------------------------------------
// <copyright file="IChapter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th095;

internal interface IChapter
{
    uint Checksum { get; }

    bool IsValid { get; }

    string Signature { get; }

    int Size { get; }

    ushort Version { get; }
}
