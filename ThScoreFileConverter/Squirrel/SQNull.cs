//-----------------------------------------------------------------------
// <copyright file="SQNull.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.IO;
using ThScoreFileConverter.Properties;

namespace ThScoreFileConverter.Squirrel
{
    internal sealed class SQNull : SQObject, IEquatable<SQNull>
    {
        private SQNull()
            : base(SQObjectType.Null)
        {
        }

        public static SQNull Instance { get; } = new SQNull();

        public static SQNull Create(BinaryReader reader, bool skipType = false)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            if (!skipType)
            {
                var type = reader.ReadInt32();
                if (type != (int)SQObjectType.Null)
                    throw new InvalidDataException(Resources.InvalidDataExceptionWrongType);
            }

            return Instance;
        }

        public override bool Equals(object? obj)
        {
            return (obj is SQNull value) ? this.Equals(value) : false;
        }

        public override int GetHashCode()
        {
#if NETFRAMEWORK
            return this.Type.GetHashCode();
#else
            return HashCode.Combine(this.Type);
#endif
        }

        public bool Equals(SQNull? other)
        {
            if (other is null)
                return false;

            return (this.Type == other.Type) && (this.Value == other.Value);
        }
    }
}
