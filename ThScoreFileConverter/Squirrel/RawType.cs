//-----------------------------------------------------------------------
// <copyright file="RawType.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Squirrel;

/// <summary>
/// Represents a "raw" object type defined by Squirrel 3.1.
/// Refer to https://github.com/albertodemichelis/squirrel/blob/master/include/squirrel.h for details.
/// </summary>
internal enum RawType
{
#pragma warning disable SA1602 // Enumeration items should be documented
    Null          = 0x00000001,
    Integer       = 0x00000002,
    Float         = 0x00000004,
    Bool          = 0x00000008,
    String        = 0x00000010,
    Table         = 0x00000020,
    Array         = 0x00000040,
    UserData      = 0x00000080,
    Closure       = 0x00000100,
    NativeClosure = 0x00000200,
    Generator     = 0x00000400,
    UserPointer   = 0x00000800,
    Thread        = 0x00001000,
    FuncProto     = 0x00002000,
    Class         = 0x00004000,
    Instance      = 0x00008000,
    WeakRef       = 0x00010000,
    Outer         = 0x00020000,
#pragma warning restore SA1602 // Enumeration items should be documented
}
