//-----------------------------------------------------------------------
// <copyright file="Header.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;

namespace ThScoreFileConverter.Models.Th095;

internal class Header : HeaderBase
{
    public const string ValidSignature = "TH95";

    public override bool IsValid => base.IsValid && this.Signature.Equals(ValidSignature, StringComparison.Ordinal);
}
