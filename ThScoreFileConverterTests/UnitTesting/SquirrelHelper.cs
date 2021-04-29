using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SQOT = ThScoreFileConverter.Squirrel.SQObjectType;

namespace ThScoreFileConverterTests.UnitTesting
{
    public static class SquirrelHelper
    {
        private static readonly BindingFlags BindingAttribute = BindingFlags.NonPublic | BindingFlags.Static;
        private static readonly MethodInfo FromArrayMethodInfo =
            typeof(SquirrelHelper).GetMethod(nameof(MakeByteArrayFromArray), BindingAttribute)!;
        private static readonly MethodInfo FromDictionaryMethodInfo =
            typeof(SquirrelHelper).GetMethod(nameof(MakeByteArrayFromDictionary), BindingAttribute)!;

        public static IEnumerable<byte> MakeByteArray(params object?[] args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            static bool IsDictionary(object arg)
            {
                var argType = arg.GetType();
                return argType.IsGenericType && (argType.GetGenericTypeDefinition() == typeof(Dictionary<,>));
            }

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
                    case Array { Rank: 1 } array:
                        byteArray = byteArray.Concat(MakeByteArrayFromArrayReflection(array));
                        break;
                    case { } when IsDictionary(arg):
                        byteArray = byteArray.Concat(MakeByteArrayFromDictionaryReflection(arg));
                        break;
                    default:
                        break;
                }
            }

            return byteArray;
        }

        private static IEnumerable<byte> MakeByteArrayFromArrayReflection(Array array)
        {
            return (IEnumerable<byte>)FromArrayMethodInfo.MakeGenericMethod(array.GetType().GetElementType()!)
                .Invoke(null, new object[] { array })!;
        }

        private static IEnumerable<byte> MakeByteArrayFromArray<T>(in IEnumerable<T> array)
        {
            return TestUtils.MakeByteArray((int)SQOT.Array, array.Count())
                .Concat(array.SelectMany((element, index) => MakeByteArray(index).Concat(MakeByteArray(element))))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null));
        }

        private static IEnumerable<byte> MakeByteArrayFromDictionaryReflection(object dictionary)
        {
            return (IEnumerable<byte>)FromDictionaryMethodInfo.MakeGenericMethod(dictionary.GetType().GetGenericArguments())
                .Invoke(null, new object[] { dictionary })!;
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
