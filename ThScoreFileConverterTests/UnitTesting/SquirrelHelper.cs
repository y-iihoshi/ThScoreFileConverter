using System;
using System.Collections.Generic;
using System.IO;
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

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            foreach (var arg in args)
            {
                switch (arg)
                {
                    case int intValue:
                        writer.Write(TestUtils.MakeByteArray((int)SQOT.Integer, intValue));
                        break;
                    case float floatValue:
                        writer.Write(TestUtils.MakeByteArray((int)SQOT.Float, floatValue));
                        break;
                    case bool boolValue:
                        writer.Write(TestUtils.MakeByteArray((int)SQOT.Bool, (byte)(boolValue ? 0x01 : 0x00)));
                        break;
                    case string stringValue:
                        {
                            var bytes = TestUtils.CP932Encoding.GetBytes(stringValue);
                            writer.Write(TestUtils.MakeByteArray((int)SQOT.String, bytes.Length, bytes));
                        }
                        break;
                    case Array { Rank: 1 } array:
                        writer.Write(MakeByteArrayFromArrayReflection(array).ToArray());
                        break;
                    case { } when IsDictionary(arg):
                        writer.Write(MakeByteArrayFromDictionaryReflection(arg).ToArray());
                        break;
                    default:
                        break;
                }
            }

            writer.Flush();
            return stream.ToArray();
        }

        private static IEnumerable<byte> MakeByteArrayFromArrayReflection(Array array)
        {
            return (IEnumerable<byte>)FromArrayMethodInfo.MakeGenericMethod(array.GetType().GetElementType()!)
                .Invoke(null, new object[] { array })!;
        }

        private static IEnumerable<byte> MakeByteArrayFromArray<T>(in IEnumerable<T> array)
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.Write(TestUtils.MakeByteArray((int)SQOT.Array, array.Count()));
            writer.Write(array.SelectMany((element, index) => MakeByteArray(index, element)).ToArray());
            writer.Write(TestUtils.MakeByteArray((int)SQOT.Null));

            writer.Flush();
            return stream.ToArray();
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
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.Write(TestUtils.MakeByteArray((int)SQOT.Table));
            writer.Write(dictionary.SelectMany(pair => MakeByteArray(pair.Key, pair.Value)).ToArray());
            writer.Write(TestUtils.MakeByteArray((int)SQOT.Null));

            writer.Flush();
            return stream.ToArray();
        }
    }
}
