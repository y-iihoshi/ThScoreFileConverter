//-----------------------------------------------------------------------
// <copyright file="SQInstance.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;
using ThScoreFileConverter.Core.Resources;

namespace ThScoreFileConverter.Squirrel;

internal sealed class SQInstance : SQObject
{
    public SQInstance()
        : base(SQObjectType.Instance)
    {
    }

    public static SQInstance Create(BinaryReader reader, bool skipType = false)
    {
        if (!skipType)
        {
            var type = reader.ReadInt32();
            if (type != (int)SQObjectType.Instance)
                throw new InvalidDataException(ExceptionMessages.InvalidDataExceptionWrongType);
        }

        return new SQInstance();
    }
}
