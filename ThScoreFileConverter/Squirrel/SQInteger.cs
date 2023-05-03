//-----------------------------------------------------------------------
// <copyright file="SQInteger.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.IO;
using CommunityToolkit.Diagnostics;
using ThScoreFileConverter.Core.Resources;

namespace ThScoreFileConverter.Squirrel;

internal sealed class SQInteger : SQObject, IEquatable<SQInteger>
{
    public SQInteger(int value = default)
        : base(SQObjectType.Integer)
    {
        this.Value = value;
    }

    public new int Value
    {
        get => (int)base.Value;
        private set => base.Value = value;
    }

    public static implicit operator int(SQInteger sq)
    {
        return sq.Value;
    }

    public static SQInteger Create(BinaryReader reader, bool skipType = false)
    {
        if (!skipType)
        {
            var type = reader.ReadInt32();
            if (type != (int)SQObjectType.Integer)
                ThrowHelper.ThrowInvalidDataException(ExceptionMessages.InvalidDataExceptionWrongType);
        }

        return new SQInteger(reader.ReadInt32());
    }

    public override bool Equals(object? obj)
    {
        return (obj is SQInteger value) && this.Equals(value);
    }

    public override int GetHashCode()
    {
#if NETFRAMEWORK
        return this.Type.GetHashCode() ^ this.Value.GetHashCode();
#else
        return HashCode.Combine(this.Type, this.Value);
#endif
    }

    public bool Equals(SQInteger? other)
    {
        if (other is null)
            return false;

        return (this.Type == other.Type) && (this.Value == other.Value);
    }
}
