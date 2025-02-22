﻿//-----------------------------------------------------------------------
// <copyright file="SQFloat.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.IO;
using CommunityToolkit.Diagnostics;
using ThScoreFileConverter.Core.Resources;

namespace ThScoreFileConverter.Squirrel;

internal sealed class SQFloat : SQObject, IEquatable<SQFloat>
{
    public SQFloat(float value = default)
        : base(SQObjectType.Float)
    {
        this.Value = value;
    }

    public new float Value
    {
        get => (float)base.Value;
        private set => base.Value = value;
    }

    public static implicit operator float(SQFloat sq)
    {
        return sq.Value;
    }

    public static SQFloat Create(BinaryReader reader, bool skipType = false)
    {
        if (!skipType)
        {
            var type = reader.ReadInt32();
            if (type != (int)SQObjectType.Float)
                ThrowHelper.ThrowInvalidDataException(ExceptionMessages.InvalidDataExceptionWrongType);
        }

        return new SQFloat(reader.ReadSingle());
    }

    public override bool Equals(object? obj)
    {
        return (obj is SQFloat value) && this.Equals(value);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.Type, this.Value);
    }

    public bool Equals(SQFloat? other)
    {
        if (other is null)
            return false;

        return (this.Type == other.Type) && (this.Value == other.Value);
    }
}
