//-----------------------------------------------------------------------
// <copyright file="SQArray.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommunityToolkit.Diagnostics;
using ThScoreFileConverter.Core.Resources;
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Squirrel;

internal sealed class SQArray : SQObject
{
    public SQArray()
        : this(Enumerable.Empty<SQObject>())
    {
    }

    public SQArray(IEnumerable<SQObject> enumerable)
        : base(SQObjectType.Array)
    {
        this.Value = enumerable;
    }

    public new IEnumerable<SQObject> Value
    {
        get => (IEnumerable<SQObject>)base.Value;
        private set => base.Value = value.ToArray();
    }

    public static SQArray Create(BinaryReader reader, bool skipType = false)
    {
        if (!skipType)
        {
            var type = reader.ReadInt32();
            if (type != (int)SQObjectType.Array)
                ThrowHelper.ThrowInvalidDataException(ExceptionMessages.InvalidDataExceptionWrongType);
        }

        var num = reader.ReadInt32();
        if (num < 0)
            ThrowHelper.ThrowInvalidDataException(ExceptionMessages.InvalidDataExceptionNumElementsMustNotBeNegative);

        var dictionary = new Dictionary<int, SQObject>();

        for (var count = 0; count < num; count++)
        {
            var index = SQObject.Create(reader);
            var value = SQObject.Create(reader);

            if (index is SQInteger i)
            {
                if (i >= num)
                    ThrowHelper.ThrowInvalidDataException(ExceptionMessages.InvalidDataExceptionIndexIsOutOfRange);

                dictionary.Add(i, value);
            }
            else
            {
                ThrowHelper.ThrowInvalidDataException(ExceptionMessages.InvalidDataExceptionIndexMustBeAnInteger);
            }
        }

        var sentinel = SQObject.Create(reader);
        if (sentinel is not SQNull)
            ThrowHelper.ThrowInvalidDataException(ExceptionMessages.InvalidDataExceptionWrongSentinel);

        var array = new SQObject[num];
        foreach (var pair in dictionary)
            array[pair.Key] = pair.Value;

        return new SQArray(array);
    }

    public override string? ToString()
    {
        return $"[ {string.Join(", ", this.Value.Select(element => element.ToNonNullString()))} ]";
    }
}
