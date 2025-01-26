// #define DEBUG_TEST

using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
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

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        CP932Encoding = Encoding.GetEncoding(932);
    }

    public static string Unreachable { get; }

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

    public static byte[] MakeRandomArray(int length)
    {
        return RandomNumberGenerator.GetBytes(length);
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
        return type.IsEnum
            ? (TResult)Enum.ToObject(type, value)
            : (TResult)Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
    }

    public static IEnumerable<object[]> GetInvalidEnumerators<TEnum>()
        where TEnum : struct, Enum
    {
        return TestHelper.GetInvalidEnumerators<TEnum>();
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
