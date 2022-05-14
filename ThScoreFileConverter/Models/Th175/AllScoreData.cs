//-----------------------------------------------------------------------
// <copyright file="AllScoreData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Squirrel;

namespace ThScoreFileConverter.Models.Th175
{
    internal class AllScoreData : IBinaryReadable
    {
        private SQTable allData;

        public AllScoreData()
        {
            this.allData = new SQTable();
            this.SaveDataDictionary = ImmutableDictionary<int, SaveData>.Empty;
        }

        public IReadOnlyDictionary<int, SaveData> SaveDataDictionary { get; private set; }

        public void ReadFrom(BinaryReader reader)
        {
            this.allData = (SQTable)ReadSQObject(reader, SQObjectType.Table);

            this.ParseSaveDataDictionary();
        }

        private static SQObject ReadSQObject(BinaryReader reader, SQObjectType containerType)
        {
            var pairs = new Dictionary<SQObject, SQObject>();
            var elements = new List<SQObject>();

            while (true)
            {
                var str = reader.ReadNullTerminatedString();
                if (str.Length == 0)
                {
                    break;
                }

                var key = new SQString(str);
                var type = (SQObjectType)reader.ReadUInt32();
                var value = type switch
                {
                    SQObjectType.Table => ReadSQObject(reader, SQObjectType.Table),
                    SQObjectType.Array => ReadSQObject(reader, SQObjectType.Array),
                    SQObjectType.Integer => SQInteger.Create(reader, true),
                    _ => SQNull.Instance,
                };

                if (value != SQNull.Instance)
                {
                    if (containerType == SQObjectType.Table)
                    {
                        pairs.Add(key, value);
                    }
                    else if (containerType == SQObjectType.Array)
                    {
                        elements.Add(value);
                    }
                }
            }

            return containerType switch
            {
                SQObjectType.Table => new SQTable(pairs),
                SQObjectType.Array => new SQArray(elements),
                _ => SQNull.Instance,
            };
        }

        private void ParseSaveDataDictionary()
        {
            this.SaveDataDictionary = this.allData.ToDictionary(
                key => (key is SQString keyStr) && int.TryParse(keyStr, out _),
                value => value is SQTable,
                key => int.Parse((SQString)key, CultureInfo.InvariantCulture),
                value => new SaveData((SQTable)value));
        }
    }
}
