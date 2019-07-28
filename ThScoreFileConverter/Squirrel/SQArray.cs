﻿//-----------------------------------------------------------------------
// <copyright file="SQArray.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

namespace ThScoreFileConverter.Squirrel
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using ThScoreFileConverter.Properties;

    internal sealed class SQArray : SQObject
    {
        public SQArray()
            : this(new SQObject[] { })
        {
        }

        public SQArray(IEnumerable<SQObject> enumerable)
            : base(SQObjectType.Array)
            => this.Value = enumerable;

        public new IEnumerable<SQObject> Value
        {
            get => base.Value as IEnumerable<SQObject>;
            private set => base.Value = value.ToArray();
        }

        public static SQArray Create(BinaryReader reader, bool skipType = false)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            if (!skipType)
            {
                var type = reader.ReadInt32();
                if (type != (int)SQObjectType.Array)
                    throw new InvalidDataException(Resources.InvalidDataExceptionWrongType);
            }

            var num = reader.ReadInt32();
            if (num < 0)
                throw new InvalidDataException(Resources.InvalidDataExceptionNumElementsMustNotBeNegative);

            var array = new SQArray(new SQObject[num]);

            for (var count = 0; count < num; count++)
            {
                var index = SQObject.Create(reader);
                if (index is null)
                    throw new InvalidDataException(Resources.InvalidDataExceptionFailedToReadIndex);

                var value = SQObject.Create(reader);
                if (value is null)
                    throw new InvalidDataException(Resources.InvalidDataExceptionFailedToReadValue);

                if (!(index is SQInteger i))
                    throw new InvalidDataException(Resources.InvalidDataExceptionIndexMustBeAnInteger);
                if (i >= num)
                    throw new InvalidDataException(Resources.InvalidDataExceptionIndexIsOutOfRange);

                ((array as SQObject).Value as SQObject[])[i] = value;
            }

            var sentinel = SQObject.Create(reader);
            if (sentinel is null)
                throw new InvalidDataException(Resources.InvalidDataExceptionFailedToReadSentinel);

            return (sentinel is SQNull)
                ? array : throw new InvalidDataException(Resources.InvalidDataExceptionWrongSentinel);
        }
    }
}
