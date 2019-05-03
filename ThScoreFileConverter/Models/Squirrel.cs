// <copyright file="Squirrel.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System;

namespace ThScoreFileConverter.Models
{
    /// <summary>
    /// Provides the constants defined by Squirrel 3.1.
    /// Refer to https://github.com/albertodemichelis/squirrel/blob/master/include/squirrel.h for details.
    /// </summary>
    public static class Squirrel
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements should be documented

        [Flags]
        public enum SQObjectType
        {
#pragma warning disable SA1602 // Enumeration items should be documented
            RTNull             = 0x00000001,
            RTInteger          = 0x00000002,
            RTFloat            = 0x00000004,
            RTBool             = 0x00000008,
            RTString           = 0x00000010,
            RTTable            = 0x00000020,
            RTArray            = 0x00000040,
            RTUserData         = 0x00000080,
            RTClosure          = 0x00000100,
            RTNativeClosure    = 0x00000200,
            RTGenerator        = 0x00000400,
            RTUserPointer      = 0x00000800,
            RTThread           = 0x00001000,
            RTFuncProto        = 0x00002000,
            RTClass            = 0x00004000,
            RTInstance         = 0x00008000,
            RTWeakRef          = 0x00010000,
            RTOuter            = 0x00020000,
            SQObjectCanBeFalse = 0x01000000,
            SQObjectDelegable  = 0x02000000,
            SQObjectNumeric    = 0x04000000,
            SQObjectRefCounted = 0x08000000
#pragma warning restore SA1602 // Enumeration items should be documented
        }

        public const SQObjectType OTNull =
            (SQObjectType.RTNull | SQObjectType.SQObjectCanBeFalse);
        public const SQObjectType OTInteger =
            (SQObjectType.RTInteger | SQObjectType.SQObjectNumeric | SQObjectType.SQObjectCanBeFalse);
        public const SQObjectType OTFloat =
            (SQObjectType.RTFloat | SQObjectType.SQObjectNumeric | SQObjectType.SQObjectCanBeFalse);
        public const SQObjectType OTBool =
            (SQObjectType.RTBool | SQObjectType.SQObjectCanBeFalse);
        public const SQObjectType OTString =
            (SQObjectType.RTString | SQObjectType.SQObjectRefCounted);
        public const SQObjectType OTTable =
            (SQObjectType.RTTable | SQObjectType.SQObjectRefCounted | SQObjectType.SQObjectDelegable);
        public const SQObjectType OTArray =
            (SQObjectType.RTArray | SQObjectType.SQObjectRefCounted);
        public const SQObjectType OTUserData =
            (SQObjectType.RTUserData | SQObjectType.SQObjectRefCounted | SQObjectType.SQObjectDelegable);
        public const SQObjectType OTClosure =
            (SQObjectType.RTClosure | SQObjectType.SQObjectRefCounted);
        public const SQObjectType OTNativeClosure =
            (SQObjectType.RTNativeClosure | SQObjectType.SQObjectRefCounted);
        public const SQObjectType OTGenerator =
            (SQObjectType.RTGenerator | SQObjectType.SQObjectRefCounted);
        public const SQObjectType OTUserPointer =
            SQObjectType.RTUserPointer;
        public const SQObjectType OTThread =
            (SQObjectType.RTThread | SQObjectType.SQObjectRefCounted) ;
        public const SQObjectType OTFuncProto =
            (SQObjectType.RTFuncProto | SQObjectType.SQObjectRefCounted);
        public const SQObjectType OTClass =
            (SQObjectType.RTClass | SQObjectType.SQObjectRefCounted);
        public const SQObjectType OTInstance =
            (SQObjectType.RTInstance | SQObjectType.SQObjectRefCounted | SQObjectType.SQObjectDelegable);
        public const SQObjectType OTWeakRef =
            (SQObjectType.RTWeakRef | SQObjectType.SQObjectRefCounted);
        public const SQObjectType OTOuter =
            (SQObjectType.RTOuter | SQObjectType.SQObjectRefCounted);

#pragma warning restore SA1600 // Elements should be documented
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
