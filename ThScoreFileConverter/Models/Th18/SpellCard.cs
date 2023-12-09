//-----------------------------------------------------------------------
// <copyright file="SpellCard.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using ThScoreFileConverter.Core.Models;

namespace ThScoreFileConverter.Models.Th18;

internal sealed class SpellCard : Th13.SpellCardBase<Level>
{
    public SpellCard()
        : base(0xC0)
    {
    }
}
