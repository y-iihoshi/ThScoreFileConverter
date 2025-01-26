//-----------------------------------------------------------------------
// <copyright file="SQNull.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;
using CommunityToolkit.Diagnostics;
using ThScoreFileConverter.Core.Resources;

namespace ThScoreFileConverter.Squirrel;

internal sealed class SQNull : SQObject, IEquatable<SQNull>
{
    private SQNull()
        : base(SQObjectType.Null)
    {
    }

    public static SQNull Instance { get; } = new SQNull();

    public static SQNull Create(BinaryReader reader, bool skipType = false)
    {
        if (!skipType)
        {
            var type = reader.ReadInt32();
            if (type != (int)SQObjectType.Null)
                ThrowHelper.ThrowInvalidDataException(ExceptionMessages.InvalidDataExceptionWrongType);
        }

        return Instance;
    }

    public override bool Equals(object? obj)
    {
        return (obj is SQNull value) && this.Equals(value);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.Type);
    }

    public bool Equals(SQNull? other)
    {
        if (other is null)
            return false;

        return (this.Type == other.Type) && (this.Value == other.Value);
    }

    public override string? ToString()
    {
        return nameof(SQObjectType.Null);
    }
}
