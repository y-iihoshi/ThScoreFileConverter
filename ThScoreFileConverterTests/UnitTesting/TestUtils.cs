// #define DEFINE_TEST

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using IBinaryReadable = ThScoreFileConverter.Models.IBinaryReadable;

#if DEFINE_TEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Extensions;
#endif

namespace ThScoreFileConverterTests.UnitTesting
{
#if DEFINE_TEST
    [TestClass]
#endif
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

#if DEFINE_TEST
        private static Dictionary<string, int> Counter { get; } = new();

        [AssemblyCleanup]
        public static void Cleanup()
        {
            foreach (var pair in Counter.OrderByDescending(p => p.Value))
            {
                Console.WriteLine(pair);
            }
        }
#endif

        public static byte[] MakeByteArray(params object[] args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            static bool IsRankOneArray<T>(Array array)
            {
                return (array.Rank == 1) && (array.GetType().GetElementType() == typeof(T));
            }

            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream, CP932Encoding);

            foreach (var arg in args)
            {
#if DEFINE_TEST
                Counter.TryAdd(arg.GetType().Name, 0);
                ++Counter[arg.GetType().Name];
#endif
                switch (arg)
                {
                    case int intArg:
                        writer.Write(intArg);
                        break;
                    case byte[] bytesArg:
                        writer.Write(bytesArg);
                        break;
                    case uint uintArg:
                        writer.Write(uintArg);
                        break;
                    case byte byteArg:
                        writer.Write(byteArg);
                        break;
                    case float floatArg:
                        writer.Write(floatArg);
                        break;
                    case ushort ushortArg:
                        writer.Write(ushortArg);
                        break;
                    case short shortArg:
                        writer.Write(shortArg);
                        break;
                    case char[] charsArg:
                        writer.Write(charsArg);
                        break;
                    case string stringArg:
                        writer.Write(stringArg);
                        break;
                    case Array arrayArg when IsRankOneArray<int>(arrayArg):
                        foreach (var val in (int[])arrayArg)
                            writer.Write(val);
                        break;
                    case Array arrayArg when IsRankOneArray<short>(arrayArg):
                        foreach (var val in (short[])arrayArg)
                            writer.Write(val);
                        break;
                    case Array arrayArg when IsRankOneArray<uint>(arrayArg):
                        foreach (var val in (uint[])arrayArg)
                            writer.Write(val);
                        break;
                    case Array arrayArg when IsRankOneArray<ushort>(arrayArg):
                        foreach (var val in (ushort[])arrayArg)
                            writer.Write(val);
                        break;
                    case IEnumerable<byte> enumerable:
                        foreach (var val in enumerable)
                            writer.Write(val);
                        break;
                    case IEnumerable<int> enumerable:
                        foreach (var val in enumerable)
                            writer.Write(val);
                        break;
                    case IEnumerable<short> enumerable:
                        foreach (var val in enumerable)
                            writer.Write(val);
                        break;
                    case IEnumerable<uint> enumerable:
                        foreach (var val in enumerable)
                            writer.Write(val);
                        break;
                    case IEnumerable<ushort> enumerable:
                        foreach (var val in enumerable)
                            writer.Write(val);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            writer.Flush();
            return stream.ToArray();
        }

        public static TResult[] MakeRandomArray<TResult>(int length)
            where TResult : struct
        {
            var defaultValue = default(TResult);

            Func<object> getNextValue = defaultValue switch
            {
                byte _ => () => Random.Next(byte.MaxValue + 1),
                short _ => () => Random.Next(short.MaxValue + 1),
                ushort _ => () => Random.Next(ushort.MaxValue + 1),
                int _ => () =>
                {
                    var maxValue = ushort.MaxValue + 1;
                    return (Random.Next(maxValue) << 16) | Random.Next(maxValue);
                },
                uint _ => () =>
                {
                    var maxValue = ushort.MaxValue + 1;
                    return ((uint)Random.Next(maxValue) << 16) | (uint)Random.Next(maxValue);
                },
                _ => throw new NotImplementedException(),
            };

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
