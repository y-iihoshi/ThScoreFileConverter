//-----------------------------------------------------------------------
// <copyright file="SQObjectAttributes.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace ThScoreFileConverter.Squirrel
{
    /// <summary>
    /// Represents object attributes defined by Squirrel 3.1.
    /// Refer to https://github.com/albertodemichelis/squirrel/blob/master/include/squirrel.h for details.
    /// </summary>
    [Flags]
    internal enum SQObjectAttributes
    {
#pragma warning disable SA1602 // Enumeration items should be documented
        CanBeFalse = 0x01000000,
        Delegable  = 0x02000000,
        Numeric    = 0x04000000,
        RefCounted = 0x08000000,
#pragma warning restore SA1602 // Enumeration items should be documented
    }
}
