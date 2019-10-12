using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th075;
using Level = ThScoreFileConverter.Models.Th075.Level;

namespace ThScoreFileConverterTests.Models.Th075
{
    [TestClass]
    public class AllScoreDataTests
    {
        internal struct Properties
        {
            public Dictionary<(CharaWithReserved, Level), IClearData> clearData;
            public StatusTests.Properties status;
        };

        internal static Properties ValidProperties { get; } = new Properties()
        {
            clearData = Utils.GetEnumerator<CharaWithReserved>()
                .SelectMany(chara => Utils.GetEnumerator<Level>().Select(level => (chara, level)))
                .ToDictionary(pair => pair, _ => ClearDataTests.ValidStub as IClearData),
            status = StatusTests.ValidProperties
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.clearData.SelectMany(pair => ClearDataTests.MakeByteArray(pair.Value)).ToArray(),
                StatusTests.MakeByteArray(properties.status));

        internal static void Validate(in Properties properties, in AllScoreData allScoreData)
        {
            foreach (var pair in properties.clearData)
            {
                ClearDataTests.Validate(pair.Value, allScoreData.ClearData[pair.Key]);
            }

            StatusTests.Validate(properties.status, allScoreData.Status);
        }

        [TestMethod]
        public void AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new AllScoreData();

            Assert.AreEqual(0, allScoreData.ClearData.Count);
            Assert.IsNull(allScoreData.Status);
        });

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var allScoreData = TestUtils.Create<AllScoreData>(MakeByteArray(properties));

            Validate(properties, allScoreData);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var allScoreData = new AllScoreData();
            allScoreData.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
