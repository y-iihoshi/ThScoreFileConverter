//-----------------------------------------------------------------------
// <copyright file="Header.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;

namespace ThScoreFileConverter.Models.Th18;

internal class Header : Th095.HeaderBase
{
    public const string ValidSignature = "TH81";

    public override bool IsValid => base.IsValid && this.Signature.Equals(ValidSignature, StringComparison.Ordinal);
}
