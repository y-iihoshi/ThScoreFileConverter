//-----------------------------------------------------------------------
// <copyright file="SQTable.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Resources;
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Squirrel;

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
                throw new InvalidDataException(ExceptionMessages.InvalidDataExceptionWrongType);
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

    public override string? ToString()
    {
        return "{ " + string.Join(", ", this.Value.Select(pair => pair.Key.ToNonNullString() + ": " + pair.Value.ToNonNullString())) + " }";
    }

    public Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
        Func<SQObject, bool> keyPredicate,
        Func<SQObject, bool> valuePredicate,
        Func<SQObject, TKey> keyConverter,
        Func<SQObject, TValue> valueConverter)
        where TKey : notnull
    {
        return this.Value
            .Where(pair => keyPredicate(pair.Key) && valuePredicate(pair.Value))
            .ToDictionary(pair => keyConverter(pair.Key), pair => valueConverter(pair.Value));
    }

    public T GetValueOrDefault<T>(string key)
        where T : struct
    {
        T result = default;

        if (this.Value.TryGetValue(new SQString(key), out var value))
        {
            switch (value)
            {
                case SQBool sqbool:
                    if (result is bool)
                        result = (T)(object)(bool)sqbool;
                    break;
                case SQInteger sqinteger:
                    if (result is int)
                        result = (T)(object)(int)sqinteger;
                    break;
                case SQFloat sqfloat:
                    if (result is float)
                        result = (T)(object)(float)sqfloat;
                    break;
            }
        }

        return result;
    }
}
