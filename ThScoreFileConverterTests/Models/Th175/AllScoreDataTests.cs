using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th175;
using ThScoreFileConverterTests.UnitTesting;
using SQOT = ThScoreFileConverter.Squirrel.SQObjectType;

namespace ThScoreFileConverterTests.Models.Th175
{
    [TestClass]
    public class AllScoreDataTests
    {
        private static readonly byte[] NullChar = new byte[] { 0 };
        private static readonly BindingFlags BindingAttribute = BindingFlags.NonPublic | BindingFlags.Static;
        private static readonly MethodInfo FromArrayMethodInfo =
            typeof(AllScoreDataTests).GetMethod(nameof(MakeByteArrayFromArray), BindingAttribute)!;
        private static readonly MethodInfo FromDictionaryMethodInfo =
            typeof(AllScoreDataTests).GetMethod(nameof(MakeByteArrayFromDictionary), BindingAttribute)!;

        [TestMethod]
        public void AllScoreDataTest()
        {
            var allScoreData = new AllScoreData();

            Assert.IsNotNull(allScoreData);
            Assert.AreEqual(0, allScoreData.SaveDataDictionary.Count);
        }

        [TestMethod]
        public void ReadFromTestEmpty()
        {
            var bytes = Array.Empty<byte>();
            _ = Assert.ThrowsException<EndOfStreamException>(() => TestUtils.Create<AllScoreData>(bytes));
        }

        [TestMethod]
        public void ReadFromTestNullTerminatedEmptyString()
        {
            var allScoreData = TestUtils.Create<AllScoreData>(NullChar);

            Assert.IsNotNull(allScoreData);
            Assert.AreEqual(0, allScoreData.SaveDataDictionary.Count);
        }

        [TestMethod]
        public void ReadFromTestInvalidKeyType()
        {
            var bytes = MakeByteArray(0, new Dictionary<string, int> { { "key2", 12 } }).Concat(NullChar).ToArray();
            var allScoreData = TestUtils.Create<AllScoreData>(bytes);

            Assert.IsNotNull(allScoreData);
            Assert.AreEqual(0, allScoreData.SaveDataDictionary.Count);
        }

        [TestMethod]
        public void ReadFromTestInvalidKey()
        {
            var bytes = MakeByteArray("key", new Dictionary<string, int> { { "key2", 12 } }).Concat(NullChar).ToArray();
            var allScoreData = TestUtils.Create<AllScoreData>(bytes);

            Assert.IsNotNull(allScoreData);
            Assert.AreEqual(0, allScoreData.SaveDataDictionary.Count);
        }

        [TestMethod]
        public void ReadFromTestInvalidValueType()
        {
            var bytes = MakeByteArray("0", 1.2f).Concat(NullChar).ToArray();
            _ = Assert.ThrowsException<EndOfStreamException>(() => TestUtils.Create<AllScoreData>(bytes));
        }

        [TestMethod]
        public void ReadFromTestNoChildContainer()
        {
            var bytes = MakeByteArray("0", 12).Concat(NullChar).ToArray();
            var allScoreData = TestUtils.Create<AllScoreData>(bytes);

            Assert.IsNotNull(allScoreData);
            Assert.AreEqual(0, allScoreData.SaveDataDictionary.Count);
        }

        [TestMethod]
        public void ReadFromTestChildArray()
        {
            var bytes = MakeByteArray("0", new int[] { 12 }).Concat(NullChar).ToArray();
            var allScoreData = TestUtils.Create<AllScoreData>(bytes);

            Assert.IsNotNull(allScoreData);
            Assert.AreEqual(0, allScoreData.SaveDataDictionary.Count);
        }

        [TestMethod]
        public void ReadFromTestEmptyChildDictionary()
        {
            var bytes = MakeByteArray("0", new Dictionary<string, int> { }).Concat(NullChar).ToArray();
            var allScoreData = TestUtils.Create<AllScoreData>(bytes);

            Assert.IsNotNull(allScoreData);
            Assert.AreEqual(1, allScoreData.SaveDataDictionary.Count);
            SaveDataTests.ValidateAsDefault(allScoreData.SaveDataDictionary.Values.First());
        }

        [TestMethod]
        public void ReadFromTestInvalidChildKeyType()
        {
            var bytes = MakeByteArray("0", new Dictionary<int, string> { { 12, "key2" } }).Concat(NullChar).ToArray();
            var allScoreData = TestUtils.Create<AllScoreData>(bytes);

            Assert.IsNotNull(allScoreData);
            Assert.AreEqual(1, allScoreData.SaveDataDictionary.Count);
            SaveDataTests.ValidateAsDefault(allScoreData.SaveDataDictionary.Values.First());
        }

        [TestMethod]
        public void ReadFromTestInvalidChildValueType()
        {
            var bytes = MakeByteArray("0", new Dictionary<string, float> { { "key2", 1.2f } }).Concat(NullChar).ToArray();
            _ = Assert.ThrowsException<EndOfStreamException>(() => TestUtils.Create<AllScoreData>(bytes));
        }

        [TestMethod]
        public void ReadFromTest()
        {
            var bytes = MakeByteArray("0", new Dictionary<string, int> { { "key2", 12 } }).Concat(NullChar).ToArray();
            var allScoreData = TestUtils.Create<AllScoreData>(bytes);

            Assert.IsNotNull(allScoreData);
            Assert.AreEqual(1, allScoreData.SaveDataDictionary.Count);
            SaveDataTests.ValidateAsDefault(allScoreData.SaveDataDictionary.Values.First());
        }

        private static byte[] MakeByteArray(params object?[] args)
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
                var bytes = arg switch
                {
                    string value => TestUtils.MakeByteArray(TestUtils.CP932Encoding.GetBytes(value), NullChar),
                    int value => TestUtils.MakeByteArray((int)SQOT.Integer, value),
                    float value => TestUtils.MakeByteArray((int)SQOT.Float, value),
                    { } dict when IsDictionary(dict) => MakeByteArrayFromDictionaryReflection(dict),
                    Array { Rank: 1 } array => MakeByteArrayFromArrayReflection(array),
                    _ => null,
                };

                if (bytes is not null)
                    writer.Write(bytes);
            }

            writer.Flush();
            return stream.ToArray();
        }

        private static byte[] MakeByteArrayFromArrayReflection(Array array)
        {
            return (byte[])FromArrayMethodInfo.MakeGenericMethod(array.GetType().GetElementType()!)
                .Invoke(null, new object[] { array })!;
        }

        private static byte[] MakeByteArrayFromArray<T>(in IEnumerable<T> array)
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.Write(TestUtils.MakeByteArray((int)SQOT.Array));
            writer.Write(MakeByteArray("-"));
            array.Select(element => MakeByteArray(element)).ForEach(writer.Write);
            writer.Write(NullChar);

            writer.Flush();
            return stream.ToArray();
        }

        private static byte[] MakeByteArrayFromDictionaryReflection(object dictionary)
        {
            return (byte[])FromDictionaryMethodInfo.MakeGenericMethod(dictionary.GetType().GetGenericArguments())
                .Invoke(null, new object[] { dictionary })!;
        }

        private static byte[] MakeByteArrayFromDictionary<TKey, TValue>(
            in IReadOnlyDictionary<TKey, TValue> dictionary)
            where TKey : notnull
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.Write(TestUtils.MakeByteArray((int)SQOT.Table));
            dictionary.Select(pair => MakeByteArray(pair.Key, pair.Value)).ForEach(writer.Write);
            writer.Write(NullChar);

            writer.Flush();
            return stream.ToArray();
        }
    }
}
