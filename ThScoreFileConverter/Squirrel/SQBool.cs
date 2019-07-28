//-----------------------------------------------------------------------
// <copyright file="SQBool.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Squirrel
{
    using System;
    using System.IO;
    using ThScoreFileConverter.Properties;

    internal sealed class SQBool : SQObject
    {
        public SQBool(bool value = default)
            : base(SQObjectType.Bool)
            => this.Value = value;

        public new bool Value
        {
            get => (bool)base.Value;
            private set => base.Value = value;
        }

        public static implicit operator bool(SQBool sq) => sq.Value;

        public static SQBool Create(BinaryReader reader, bool skipType = false)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            if (!skipType)
            {
                var type = reader.ReadInt32();
                if (type != (int)SQObjectType.Bool)
                    throw new InvalidDataException(Resources.InvalidDataExceptionWrongType);
            }

            return new SQBool(reader.ReadByte() != 0x00);
        }
    }
}
