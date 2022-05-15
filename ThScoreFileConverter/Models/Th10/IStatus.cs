//-----------------------------------------------------------------------
// <copyright file="IStatus.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th10;

internal interface IStatus : Th095.IStatus
{
    IEnumerable<byte> BgmFlags { get; }
}
