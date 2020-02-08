using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using IBinaryReadable = ThScoreFileConverter.Models.IBinaryReadable;
using SQOT = ThScoreFileConverter.Squirrel.SQObjectType;

namespace ThScoreFileConverterTests.Models
{
    public static class TestUtils
    {
        static TestUtils()
        {
            Unreachable = nameof(Unreachable);
            Random = new Random();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            CP932Encoding = Encoding.GetEncoding(932);
        }

        public static string Unreachable { get; }

        public static Random Random { get; }

        public static Encoding CP932Encoding { get; }

        public static byte[] MakeByteArray(params object[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream, CP932Encoding);

            foreach (var arg in args)
            {
                switch (arg)
                {
                    case byte[] bytesArg:
                        writer.Write(bytesArg);
                        break;
                    case char[] charsArg:
                        writer.Write(charsArg);
                        break;
                    case sbyte sbyteArg:
                        writer.Write(sbyteArg);
                        break;
                    case byte byteArg:
                        writer.Write(byteArg);
                        break;
                    case char charArg:
                        writer.Write(charArg);
                        break;
                    case short shortArg:
                        writer.Write(shortArg);
                        break;
                    case ushort ushortArg:
                        writer.Write(ushortArg);
                        break;
                    case int intArg:
                        writer.Write(intArg);
                        break;
                    case uint uintArg:
                        writer.Write(uintArg);
                        break;
                    case long longArg:
                        writer.Write(longArg);
                        break;
                    case ulong ulongArg:
                        writer.Write(ulongArg);
                        break;
                    case ushort[] ushortsArg:
                        foreach (var val in ushortsArg)
                            writer.Write(val);
                        break;
                    case int[] intsArg:
                        foreach (var val in intsArg)
                            writer.Write(val);
                        break;
                    case bool boolArg:
                        writer.Write(boolArg);
                        break;
                    case double doubleArg:
                        writer.Write(doubleArg);
                        break;
                    case float floatArg:
                        writer.Write(floatArg);
                        break;
                    case string stringArg:
                        writer.Write(stringArg);
                        break;
                    case decimal decimalArg:
                        writer.Write(decimalArg);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            writer.Flush();

            var array = new byte[writer.BaseStream.Length];
            writer.BaseStream.Position = 0;
            _ = writer.BaseStream.Read(array, 0, array.Length);

            return array;
        }

        public static IEnumerable<byte> MakeSQByteArray(params object?[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            var currentType = typeof(TestUtils);
            var bindingAttributes = BindingFlags.NonPublic | BindingFlags.Static;
            var fromArray = currentType.GetMethod(nameof(MakeSQByteArrayFromArray), bindingAttributes);
            Debug.Assert(fromArray is object);
            var fromDictonary = currentType.GetMethod(nameof(MakeSQByteArrayFromDictionary), bindingAttributes);
            Debug.Assert(fromDictonary is object);

            var byteArray = Enumerable.Empty<byte>();

            foreach (var arg in args)
            {
                switch (arg)
                {
                    case int intValue:
                        byteArray = byteArray.Concat(MakeByteArray((int)SQOT.Integer, intValue));
                        break;
                    case float floatValue:
                        byteArray = byteArray.Concat(MakeByteArray((int)SQOT.Float, floatValue));
                        break;
                    case bool boolValue:
                        byteArray = byteArray.Concat(
                            MakeByteArray((int)SQOT.Bool, (byte)(boolValue ? 0x01 : 0x00)));
                        break;
                    case string stringValue:
                        {
                            var bytes = CP932Encoding.GetBytes(stringValue);
                            byteArray = byteArray.Concat(MakeByteArray((int)SQOT.String, bytes.Length, bytes));
                        }
                        break;
                    case Array array:
                        if (array.Rank == 1)
                        {
                            var elementType = array.GetType().GetElementType();
                            Debug.Assert(elementType is object);
                            byteArray = byteArray.Concat(
                                fromArray.MakeGenericMethod(elementType)
                                    .Invoke(null, new object[] { array }) as IEnumerable<byte>);
                        }
                        break;
                    case null:
                        break;
                    default:
                        {
                            var argType = arg.GetType();
                            if (argType.IsGenericType && argType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                            {
                                byteArray = byteArray.Concat(
                                    fromDictonary.MakeGenericMethod(argType.GetGenericArguments())
                                        .Invoke(null, new object[] { arg }) as IEnumerable<byte>);
                            }
                        }
                        break;
                }
            }

            return byteArray;
        }

        private static IEnumerable<byte> MakeSQByteArrayFromArray<T>(in IEnumerable<T> array)
            => MakeByteArray((int)SQOT.Array, array.Count())
                .Concat(array.SelectMany((element, index) => MakeSQByteArray(index).Concat(MakeSQByteArray(element))))
                .Concat(MakeByteArray((int)SQOT.Null));

        private static IEnumerable<byte> MakeSQByteArrayFromDictionary<TKey, TValue>(
            in Dictionary<TKey, TValue> dictionary)
            => MakeByteArray((int)SQOT.Table)
                .Concat(dictionary.SelectMany(pair => MakeSQByteArray(pair.Key).Concat(MakeSQByteArray(pair.Value))))
                .Concat(MakeByteArray((int)SQOT.Null));

        public static TResult[] MakeRandomArray<TResult>(int length)
            where TResult : struct
        {
            var defaultValue = default(TResult);
            var maxValue = 0;
            Func<object> getNextValue;

            switch (defaultValue)
            {
                case byte _:
                    getNextValue = () => Random.Next(byte.MaxValue + 1);
                    break;
                case short _:
                    getNextValue = () => Random.Next(short.MaxValue + 1);
                    break;
                case ushort _:
                    getNextValue = () => Random.Next(ushort.MaxValue + 1);
                    break;
                case int _:
                    maxValue = ushort.MaxValue + 1;
                    getNextValue = () => ((Random.Next(maxValue) << 16) | Random.Next(maxValue));
                    break;
                case uint _:
                    maxValue = ushort.MaxValue + 1;
                    getNextValue = () => (((uint)Random.Next(maxValue) << 16) | (uint)Random.Next(maxValue));
                    break;
                default:
                    throw new NotImplementedException();
            }

            return Enumerable
                .Repeat(defaultValue, length)
                .Select(i => (TResult)Convert.ChangeType(getNextValue(), typeof(TResult), CultureInfo.InvariantCulture))
                .ToArray();
        }

        public static T Create<T>(byte[] array)
            where T : IBinaryReadable, new()
        {
            var instance = new T();

            using var stream = new MemoryStream(array);
            using var reader = new BinaryReader(stream);
            instance.ReadFrom(reader);

            return instance;
        }

        public static TResult Cast<TResult>(object value)
            where TResult : struct
        {
            var type = typeof(TResult);
            if (type.IsEnum)
                return (TResult)Enum.ToObject(type, value);

            return (TResult)Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
        }

        public static IEnumerable<object[]> GetInvalidEnumerators(Type type)
        {
            var values = Enum.GetValues(type).Cast<int>();
            yield return new object[] { values.Min() - 1 };
            yield return new object[] { values.Max() + 1 };
        }
    }
}
