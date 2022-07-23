//-----------------------------------------------------------------------
// <copyright file="IChapter.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th06;

internal interface IChapter
{
    byte FirstByteOfData { get; }

    string Signature { get; }

    short Size1 { get; }

    short Size2 { get; }
}
