//-----------------------------------------------------------------------
// <copyright file="ICardForDeck.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th105;

internal interface ICardForDeck
{
    int Id { get; }

    int MaxNumber { get; }
}
