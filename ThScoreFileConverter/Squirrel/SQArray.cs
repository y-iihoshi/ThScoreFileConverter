//-----------------------------------------------------------------------
// <copyright file="SQArray.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Properties;

namespace ThScoreFileConverter.Squirrel
{
    internal sealed class SQArray : SQObject
    {
        public SQArray()
            : this(Enumerable.Empty<SQObject>())
        {
        }

        public SQArray(IEnumerable<SQObject> enumerable)
            : base(SQObjectType.Array)
        {
            this.Value = enumerable;
        }

        public new IEnumerable<SQObject> Value
        {
            get => (IEnumerable<SQObject>)base.Value;
            private set => base.Value = value.ToArray();
        }

        public static SQArray Create(BinaryReader reader, bool skipType = false)
        {
            if (!skipType)
            {
                var type = reader.ReadInt32();
                if (type != (int)SQObjectType.Array)
                    throw new InvalidDataException(Resources.InvalidDataExceptionWrongType);
            }

            var num = reader.ReadInt32();
            if (num < 0)
                throw new InvalidDataException(Resources.InvalidDataExceptionNumElementsMustNotBeNegative);

            var dictionary = new Dictionary<int, SQObject>();

            for (var count = 0; count < num; count++)
            {
                var index = SQObject.Create(reader);
                var value = SQObject.Create(reader);

                if (index is not SQInteger i)
                    throw new InvalidDataException(Resources.InvalidDataExceptionIndexMustBeAnInteger);
                if (i >= num)
                    throw new InvalidDataException(Resources.InvalidDataExceptionIndexIsOutOfRange);

                dictionary.Add(i, value);
            }

            var sentinel = SQObject.Create(reader);
            if (sentinel is not SQNull)
                throw new InvalidDataException(Resources.InvalidDataExceptionWrongSentinel);

            var array = new SQObject[num];
            foreach (var pair in dictionary)
                array[pair.Key] = pair.Value;

            return new SQArray(array);
        }

        public override string? ToString()
        {
            return "[ " + string.Join(", ", this.Value.Select(element => element.ToNonNullString())) + " ]";
        }
    }
}
