﻿//-----------------------------------------------------------------------
// <copyright file="SQClosure.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;
using CommunityToolkit.Diagnostics;
using ThScoreFileConverter.Core.Resources;

namespace ThScoreFileConverter.Squirrel;

internal sealed class SQClosure : SQObject
{
    public SQClosure()
        : base(SQObjectType.Closure)
    {
    }

    public static SQClosure Create(BinaryReader reader, bool skipType = false)
    {
        if (!skipType)
        {
            var type = reader.ReadInt32();
            if (type != (int)SQObjectType.Closure)
                ThrowHelper.ThrowInvalidDataException(ExceptionMessages.InvalidDataExceptionWrongType);
        }

        return new SQClosure();
    }
}
