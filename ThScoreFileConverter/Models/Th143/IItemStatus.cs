//-----------------------------------------------------------------------
// <copyright file="IItemStatus.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th143
{
    internal interface IItemStatus : Th095.IChapter
    {
        int AvailableCount { get; }

        int ClearedCount { get; }

        int ClearedScenes { get; }

        int FramesOrRanges { get; }

        ItemWithTotal Item { get; }

        int ItemLevel { get; }

        int UseCount { get; }
    }
}
