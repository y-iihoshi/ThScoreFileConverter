//-----------------------------------------------------------------------
// <copyright file="IStatus.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models.Th143;

namespace ThScoreFileConverter.Models.Th143;

internal interface IStatus : Th125.IStatus
{
    ItemWithTotal LastMainItem { get; }

    ItemWithTotal LastSubItem { get; }

    IEnumerable<byte> NicknameFlags { get; }
}
