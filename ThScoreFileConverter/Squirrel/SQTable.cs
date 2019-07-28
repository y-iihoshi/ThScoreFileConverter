//-----------------------------------------------------------------------
// <copyright file="SQTable.cs" company="None">
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

    internal sealed class SQTable : SQObject
    {
        public SQTable()
            : this(new Dictionary<SQObject, SQObject>())
        {
        }

        public SQTable(IReadOnlyDictionary<SQObject, SQObject> pairs)
            : base(SQObjectType.Table)
            => this.Value = pairs;

        public new IReadOnlyDictionary<SQObject, SQObject> Value
        {
            get => base.Value as IReadOnlyDictionary<SQObject, SQObject>;
            private set => base.Value = value.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public static SQTable Create(BinaryReader reader, bool skipType = false)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            if (!skipType)
            {
                var type = reader.ReadInt32();
                if (type != (int)SQObjectType.Table)
                    throw new InvalidDataException(Resources.InvalidDataExceptionWrongType);
            }

            var table = new SQTable();

            while (true)
            {
                var key = SQObject.Create(reader);
                if (key is null)
                    throw new InvalidDataException(Resources.InvalidDataExceptionFailedToReadKey);
                if (key is SQNull)
                    break;

                var value = SQObject.Create(reader);
                if (value is null)
                    throw new InvalidDataException(Resources.InvalidDataExceptionFailedToReadValue);

                ((table as SQObject).Value as Dictionary<SQObject, SQObject>).Add(key, value);
            }

            return table;
        }
    }
}
