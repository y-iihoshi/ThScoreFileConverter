//-----------------------------------------------------------------------
// <copyright file="SQObject.cs" company="None">
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
    using ThScoreFileConverter.Properties;

    internal class SQObject
    {
        private static readonly Dictionary<SQObjectType, Func<BinaryReader, SQObject>> SQObjectReaders =
            new Dictionary<SQObjectType, Func<BinaryReader, SQObject>>
            {
                { SQObjectType.Null,     reader => SQNull.Create(reader, true) },
                { SQObjectType.Bool,     reader => SQBool.Create(reader, true) },
                { SQObjectType.Integer,  reader => SQInteger.Create(reader, true) },
                { SQObjectType.Float,    reader => SQFloat.Create(reader, true) },
                { SQObjectType.String,   reader => SQString.Create(reader, true) },
                { SQObjectType.Array,    reader => SQArray.Create(reader, true) },
                { SQObjectType.Closure,  reader => SQClosure.Create(reader, true) },
                { SQObjectType.Table,    reader => SQTable.Create(reader, true) },
                { SQObjectType.Instance, reader => SQInstance.Create(reader, true) },
            };

        protected SQObject(SQObjectType type)
        {
            this.Type = type;
            this.Value = default;
        }

        public SQObjectType Type { get; }

        public object Value { get; protected set; }

        public static SQObject Create(BinaryReader reader)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            var type = (SQObjectType)reader.ReadInt32();

            return SQObjectReaders.TryGetValue(type, out var objectReader)
                ? objectReader(reader)
                : throw new InvalidDataException(Resources.InvalidDataExceptionWrongType);
        }
    }
}
