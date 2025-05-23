﻿//-----------------------------------------------------------------------
// <copyright file="Header.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Models.Th143;

internal sealed class Header : Th095.HeaderBase
{
    public const string ValidSignature = "T341";

    public override bool IsValid => base.IsValid && this.Signature.Equals(ValidSignature, StringComparison.Ordinal);
}
