//-----------------------------------------------------------------------
// <copyright file="SQInteger.cs" company="None">
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

    internal sealed class SQInteger : SQObject
    {
        public SQInteger(int value = default)
            : base(SQObjectType.Integer)
            => this.Value = value;

        public new int Value
        {
            get => (int)base.Value;
            private set => base.Value = value;
        }

        public static implicit operator int(SQInteger sq) => sq.Value;

        public static SQInteger Create(BinaryReader reader, bool skipType = false)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            if (!skipType)
            {
                var type = reader.ReadInt32();
                if (type != (int)SQObjectType.Integer)
                    throw new InvalidDataException(Resources.InvalidDataExceptionWrongType);
            }

            return new SQInteger(reader.ReadInt32());
        }
    }
}
