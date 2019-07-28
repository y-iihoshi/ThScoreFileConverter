//-----------------------------------------------------------------------
// <copyright file="SQNull.cs" company="None">
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

    internal sealed class SQNull : SQObject
    {
        public SQNull()
            : base(SQObjectType.Null)
        {
        }

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

            return new SQNull();
        }
    }
}
