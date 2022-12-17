// #define DEBUG_TEST

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CommunityToolkit.Diagnostics;
using IBinaryReadable = ThScoreFileConverter.Models.IBinaryReadable;

#if DEBUG_TEST
using ThScoreFileConverter.Extensions;
#endif

namespace ThScoreFileConverter.Tests.UnitTesting;

#if DEBUG_TEST
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

#if DEBUG_TEST
    private static Dictionary<string, int> Counter { get; } = new();

    [AssemblyCleanup]
    public static void Cleanup()
    {
        Counter.OrderByDescending(static pair => pair.Value).ForEach(static pair => Console.WriteLine(pair));
    }
#endif

    public static byte[] MakeByteArray(params object[] args)
    {
        Guard.IsNotNull(args);

        static bool IsRankOneArray<T>(Array array)
        {
            return (array.Rank == 1) && (array.GetType().GetElementType() == typeof(T));
        }

        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, CP932Encoding);

        foreach (var arg in args)
        {
            _ = arg switch
            {
                int value => Invoke(writer.Write, "int", value),
                uint value => Invoke(writer.Write, "uint", value),
                byte[] array => Invoke(writer.Write, "byte[]", array),
                byte value => Invoke(writer.Write, "byte", value),
                float value => Invoke(writer.Write, "float", value),
                ushort value => Invoke(writer.Write, "ushort", value),
                short value => Invoke(writer.Write, "short", value),
                char[] array => Invoke(writer.Write, "char[]", array),
                string value => Invoke(writer.Write, "string", value),
                IEnumerable<byte[]> enumerable => Invoke(writer.Write, "IEnumerable<byte[]>", enumerable),
                IEnumerable<byte> enumerable => Invoke(writer.Write, "IEnumerable<byte>", enumerable),
                Array array when IsRankOneArray<int>(array) => Invoke(writer.Write, "int[]", (int[])array),
                Array array when IsRankOneArray<uint>(array) => Invoke(writer.Write, "uint[]", (uint[])array),
                IEnumerable<int> enumerable => Invoke(writer.Write, "IEnumerable<int>", enumerable),
                IEnumerable<short> enumerable => Invoke(writer.Write, "IEnumerable<short>", enumerable),
                IEnumerable<uint> enumerable => Invoke(writer.Write, "IEnumerable<uint>", enumerable),
                IEnumerable<ushort> enumerable => Invoke(writer.Write, "IEnumerable<ushort>", enumerable),
                _ => throw new NotImplementedException(),
            };
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
            byte => () => Random.Next(byte.MaxValue + 1),
            short => () => Random.Next(short.MaxValue + 1),
            ushort => () => Random.Next(ushort.MaxValue + 1),
            int => () =>
            {
                var maxValue = ushort.MaxValue + 1;
                return (Random.Next(maxValue) << 16) | Random.Next(maxValue);
            },
            uint => () =>
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
        return Core.Tests.UnitTesting.TestHelper.Cast<TResult>(value);
    }

    public static IEnumerable<object[]> GetInvalidEnumerators(Type type)
    {
        return Core.Tests.UnitTesting.TestHelper.GetInvalidEnumerators(type);
    }

    private static bool Invoke<T>(Action<T> action, string key, T value)
    {
        Countup(key);
        action(value);
        return true;
    }

    private static bool Invoke<T>(Action<T> action, string key, IEnumerable<T> enumerable)
    {
        Countup(key);
        enumerable.ForEach(action);
        return true;
    }

    [Conditional("DEBUG_TEST")]
    private static void Countup(string key)
    {
#if DEBUG_TEST
        Counter.TryAdd(key, 0);
        ++Counter[key];
#endif
    }
}
