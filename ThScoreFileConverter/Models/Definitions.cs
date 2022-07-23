//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Resources;

namespace ThScoreFileConverter.Models;

internal static class Definitions
{
    public static IEnumerable<Work> Works { get; } = new Work[]
    {
        new() { Number = nameof(StringResources.TH06),  IsSupported = true },
        new() { Number = nameof(StringResources.TH07),  IsSupported = true },
        new() { Number = nameof(StringResources.TH08),  IsSupported = true },
        new() { Number = nameof(StringResources.TH09),  IsSupported = true },
        new() { Number = nameof(StringResources.TH095), IsSupported = true },
        new() { Number = nameof(StringResources.TH10),  IsSupported = true },
        new() { Number = nameof(StringResources.TH11),  IsSupported = true },
        new() { Number = nameof(StringResources.TH12),  IsSupported = true },
        new() { Number = nameof(StringResources.TH125), IsSupported = true },
        new() { Number = nameof(StringResources.TH128), IsSupported = true },
        new() { Number = nameof(StringResources.TH13),  IsSupported = true },
        new() { Number = nameof(StringResources.TH14),  IsSupported = true },
        new() { Number = nameof(StringResources.TH143), IsSupported = true },
        new() { Number = nameof(StringResources.TH15),  IsSupported = true },
        new() { Number = nameof(StringResources.TH16),  IsSupported = true },
        new() { Number = nameof(StringResources.TH165), IsSupported = true },
        new() { Number = nameof(StringResources.TH17),  IsSupported = true },
        new() { Number = nameof(StringResources.TH18),  IsSupported = true },
        new() { },
        new() { Number = nameof(StringResources.TH075), IsSupported = true },
        new() { Number = nameof(StringResources.TH105), IsSupported = true },
        new() { Number = nameof(StringResources.TH123), IsSupported = true },
        new() { Number = nameof(StringResources.TH135), IsSupported = true },
        new() { Number = nameof(StringResources.TH145), IsSupported = true },
        new() { Number = nameof(StringResources.TH155), IsSupported = true },
        new() { Number = nameof(StringResources.TH175), IsSupported = true },
    };

    public static bool IsTotal(LevelWithTotal level)
    {
        return level == LevelWithTotal.Total;
    }
}
