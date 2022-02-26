//-----------------------------------------------------------------------
// <copyright file="SQString.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.IO;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Properties;

namespace ThScoreFileConverter.Squirrel
{
    internal sealed class SQString : SQObject, IEquatable<SQString>
    {
        public SQString(string value = "")
            : base(SQObjectType.String)
        {
            this.Value = value;
        }

        public new string Value
        {
            get => (string)base.Value;
            private set => base.Value = value;
        }

        public static implicit operator string(SQString sq)
        {
            return sq.Value;
        }

        public static SQString Create(BinaryReader reader, bool skipType = false)
        {
            if (!skipType)
            {
                var type = reader.ReadInt32();
                if (type != (int)SQObjectType.String)
                    throw new InvalidDataException(Resources.InvalidDataExceptionWrongType);
            }

            var size = reader.ReadInt32();
            return new SQString(
                (size > 0) ? EncodingHelper.CP932.GetString(reader.ReadExactBytes(size)) : string.Empty);
        }

        public override bool Equals(object? obj)
        {
            return (obj is SQString value) && this.Equals(value);
        }

        public override int GetHashCode()
        {
#if NETFRAMEWORK
            return this.Type.GetHashCode() ^ this.Value.GetHashCode();
#else
            return HashCode.Combine(this.Type, this.Value);
#endif
        }

        public bool Equals(SQString? other)
        {
            if (other is null)
                return false;

            return (this.Type == other.Type) && (this.Value == other.Value);
        }
    }
}
