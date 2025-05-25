// #define DEBUG_TEST

using System.Reflection;
using SQOT = ThScoreFileConverter.Squirrel.SQObjectType;

namespace ThScoreFileConverter.Tests.UnitTesting;

#if DEBUG_TEST
[TestClass]
#endif
public static class SquirrelHelper
{
    private static readonly BindingFlags BindingAttribute = BindingFlags.NonPublic | BindingFlags.Static;
    private static readonly MethodInfo FromArrayMethodInfo =
        typeof(SquirrelHelper).GetMethod(nameof(MakeByteArrayFromArray), BindingAttribute)!;
    private static readonly MethodInfo FromDictionaryMethodInfo =
        typeof(SquirrelHelper).GetMethod(nameof(MakeByteArrayFromDictionary), BindingAttribute)!;

#if DEBUG_TEST
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

    public static byte[] MakeByteArray(params ReadOnlySpan<object?> args)
    {
        static bool IsDictionary(object arg)
        {
            var argType = arg.GetType();
            return argType.IsGenericType && (argType.GetGenericTypeDefinition() == typeof(Dictionary<,>));
        }

        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        foreach (var arg in args)
        {
#if DEBUG_TEST
            var key = arg?.GetType()?.Name ?? "(null)";
            Counter.TryAdd(key, 0);
            ++Counter[key];
#endif
            switch (arg)
            {
                case int intValue:
                    writer.Write(TestUtils.MakeByteArray((int)SQOT.Integer, intValue));
                    break;
                case string stringValue:
                    {
                        var bytes = TestUtils.CP932Encoding.GetBytes(stringValue);
                        writer.Write(TestUtils.MakeByteArray((int)SQOT.String, bytes.Length, bytes));
                    }
                    break;
                case bool boolValue:
                    writer.Write(TestUtils.MakeByteArray((int)SQOT.Bool, (byte)(boolValue ? 0x01 : 0x00)));
                    break;
                case { } when IsDictionary(arg):
                    writer.Write(MakeByteArrayFromDictionaryReflection(arg));
                    break;
                case float floatValue:
                    writer.Write(TestUtils.MakeByteArray((int)SQOT.Float, floatValue));
                    break;
                case Array { Rank: 1 } array:
                    writer.Write(MakeByteArrayFromArrayReflection(array));
                    break;
                default:
                    break;
            }
        }

        writer.Flush();
        return stream.ToArray();
    }

    private static byte[] MakeByteArrayFromArrayReflection(Array array)
    {
        return (byte[])FromArrayMethodInfo.MakeGenericMethod(array.GetType().GetElementType()!)
            .Invoke(null, [array])!;
    }

    private static byte[] MakeByteArrayFromArray<T>(in IEnumerable<T> array)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        writer.Write(TestUtils.MakeByteArray((int)SQOT.Array, array.Count()));
        array.Select((element, index) => MakeByteArray(index, element)).ForEach(writer.Write);
        writer.Write(TestUtils.MakeByteArray((int)SQOT.Null));

        writer.Flush();
        return stream.ToArray();
    }

    private static byte[] MakeByteArrayFromDictionaryReflection(object dictionary)
    {
        return (byte[])FromDictionaryMethodInfo.MakeGenericMethod(dictionary.GetType().GetGenericArguments())
            .Invoke(null, [dictionary])!;
    }

    private static byte[] MakeByteArrayFromDictionary<TKey, TValue>(
        in IReadOnlyDictionary<TKey, TValue> dictionary)
        where TKey : notnull
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        writer.Write(TestUtils.MakeByteArray((int)SQOT.Table));
        dictionary.Select(pair => MakeByteArray(pair.Key, pair.Value)).ForEach(writer.Write);
        writer.Write(TestUtils.MakeByteArray((int)SQOT.Null));

        writer.Flush();
        return stream.ToArray();
    }
}
