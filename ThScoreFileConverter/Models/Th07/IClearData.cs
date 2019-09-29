//-----------------------------------------------------------------------
// <copyright file="IClearData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th07
{
    internal interface IClearData : Th06.IChapter
    {
        Chara Chara { get; }

        IReadOnlyDictionary<Level, byte> PracticeFlags { get; }

        IReadOnlyDictionary<Level, byte> StoryFlags { get; }
    }
}
