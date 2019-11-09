//-----------------------------------------------------------------------
// <copyright file="IScoreData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th16
{
    internal interface IScoreData : Th10.IScoreData<Th13.StageProgress>
    {
        Th16Converter.Season Season { get; }
    }
}
