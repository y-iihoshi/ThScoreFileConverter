using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SQOT = ThScoreFileConverter.Squirrel.SQObjectType;

namespace ThScoreFileConverterTests.UnitTesting
{
    public static class SquirrelHelper
    {
        public static IEnumerable<byte> MakeByteArray(params object?[] args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            var currentType = typeof(SquirrelHelper);
            var bindingAttributes = BindingFlags.NonPublic | BindingFlags.Static;
            var fromArray = currentType.GetMethod(nameof(MakeByteArrayFromArray), bindingAttributes);
            var fromDictonary = currentType.GetMethod(nameof(MakeByteArrayFromDictionary), bindingAttributes);

            var byteArray = Enumerable.Empty<byte>();

            foreach (var arg in args)
            {
                switch (arg)
                {
                    case int intValue:
                        byteArray = byteArray.Concat(TestUtils.MakeByteArray((int)SQOT.Integer, intValue));
                        break;
                    case float floatValue:
                        byteArray = byteArray.Concat(TestUtils.MakeByteArray((int)SQOT.Float, floatValue));
                        break;
                    case bool boolValue:
                        byteArray = byteArray.Concat(
                            TestUtils.MakeByteArray((int)SQOT.Bool, (byte)(boolValue ? 0x01 : 0x00)));
                        break;
                    case string stringValue:
                        {
                            var bytes = TestUtils.CP932Encoding.GetBytes(stringValue);
                            byteArray = byteArray.Concat(TestUtils.MakeByteArray((int)SQOT.String, bytes.Length, bytes));
                        }
                        break;
                    case Array array:
                        if (array.Rank == 1)
                        {
                            if (fromArray is not null)
                            {
                                var elementType = array.GetType().GetElementType();
                                if (elementType is not null)
                                {
                                    var array2 = fromArray.MakeGenericMethod(elementType)
                                        .Invoke(null, new object[] { array });
                                    if (array2 is IEnumerable<byte> enumerable)
                                        byteArray = byteArray.Concat(enumerable);
                                }
                            }
                        }
                        break;
                    case null:
                        break;
                    default:
                        if (fromDictonary is not null)
                        {
                            var argType = arg.GetType();
                            if (argType.IsGenericType && (argType.GetGenericTypeDefinition() == typeof(Dictionary<,>)))
                            {
                                var array = fromDictonary.MakeGenericMethod(argType.GetGenericArguments())
                                    .Invoke(null, new object[] { arg });
                                if (array is IEnumerable<byte> enumerable)
                                    byteArray = byteArray.Concat(enumerable);
                            }
                        }
                        break;
                }
            }

            return byteArray;
        }

        private static IEnumerable<byte> MakeByteArrayFromArray<T>(in IEnumerable<T> array)
        {
            return TestUtils.MakeByteArray((int)SQOT.Array, array.Count())
                .Concat(array.SelectMany((element, index) => MakeByteArray(index).Concat(MakeByteArray(element))))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null));
        }

        private static IEnumerable<byte> MakeByteArrayFromDictionary<TKey, TValue>(
            in IReadOnlyDictionary<TKey, TValue> dictionary)
            where TKey : notnull
        {
            return TestUtils.MakeByteArray((int)SQOT.Table)
                .Concat(dictionary.SelectMany(pair => MakeByteArray(pair.Key).Concat(MakeByteArray(pair.Value))))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null));
        }
    }
}
