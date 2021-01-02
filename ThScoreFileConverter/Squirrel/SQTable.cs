//-----------------------------------------------------------------------
// <copyright file="SQTable.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Properties;

namespace ThScoreFileConverter.Squirrel
{
    internal sealed class SQTable : SQObject
    {
        public SQTable()
            : this(ImmutableDictionary<SQObject, SQObject>.Empty)
        {
        }

        public SQTable(IReadOnlyDictionary<SQObject, SQObject> pairs)
            : base(SQObjectType.Table)
        {
            this.Value = pairs;
        }

        public new IReadOnlyDictionary<SQObject, SQObject> Value
        {
            get => (IReadOnlyDictionary<SQObject, SQObject>)base.Value;
            private set => base.Value = value.ToDictionary();
        }

        public static SQTable Create(BinaryReader reader, bool skipType = false)
        {
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
                if (key is SQNull)
                    break;

                var value = SQObject.Create(reader);

                ((Dictionary<SQObject, SQObject>)((SQObject)table).Value).Add(key, value);
            }

            return table;
        }
    }
}
