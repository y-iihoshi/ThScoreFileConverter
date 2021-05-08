//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using ThScoreFileConverter.Properties;

namespace ThScoreFileConverter.Models
{
    internal static class Definitions
    {
        public static IEnumerable<Work> Works { get; } = new Work[]
        {
            new() { Number = nameof(Resources.TH06),  IsSupported = true },
            new() { Number = nameof(Resources.TH07),  IsSupported = true },
            new() { Number = nameof(Resources.TH08),  IsSupported = true },
            new() { Number = nameof(Resources.TH09),  IsSupported = true },
            new() { Number = nameof(Resources.TH095), IsSupported = true },
            new() { Number = nameof(Resources.TH10),  IsSupported = true },
            new() { Number = nameof(Resources.TH11),  IsSupported = true },
            new() { Number = nameof(Resources.TH12),  IsSupported = true },
            new() { Number = nameof(Resources.TH125), IsSupported = true },
            new() { Number = nameof(Resources.TH128), IsSupported = true },
            new() { Number = nameof(Resources.TH13),  IsSupported = true },
            new() { Number = nameof(Resources.TH14),  IsSupported = true },
            new() { Number = nameof(Resources.TH143), IsSupported = true },
            new() { Number = nameof(Resources.TH15),  IsSupported = true },
            new() { Number = nameof(Resources.TH16),  IsSupported = true },
            new() { Number = nameof(Resources.TH165), IsSupported = true },
            new() { Number = nameof(Resources.TH17),  IsSupported = true },
            new() { Number = nameof(Resources.TH18),  IsSupported = true },
            new() { },
            new() { Number = nameof(Resources.TH075), IsSupported = true },
            new() { Number = nameof(Resources.TH105), IsSupported = true },
            new() { Number = nameof(Resources.TH123), IsSupported = true },
            new() { Number = nameof(Resources.TH135), IsSupported = true },
            new() { Number = nameof(Resources.TH145), IsSupported = true },
            new() { Number = nameof(Resources.TH155), IsSupported = true },
            new() { Number = nameof(Resources.TH175), IsSupported = false },
        };
    }
}
