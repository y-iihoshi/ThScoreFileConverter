using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th075;
using ThScoreFileConverterTests.Models.Th075;
using ThScoreFileConverterTests.Models.Wrappers;
using Level = ThScoreFileConverter.Models.Th075.Level;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th075AllScoreDataTests
    {
        internal struct Properties
        {
            public Dictionary<(CharaWithReserved, Level), ClearDataTests.Properties> clearData;
            public StatusTests.Properties status;
        };

        internal static Properties ValidProperties => new Properties()
        {
            clearData = Utils.GetEnumerator<CharaWithReserved>()
                .SelectMany(chara => Utils.GetEnumerator<Level>().Select(level => (chara, level)))
                .ToDictionary(pair => pair, pair => ClearDataTests.ValidProperties),
            status = StatusTests.ValidProperties
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.clearData.SelectMany(pair => ClearDataTests.MakeByteArray(pair.Value)).ToArray(),
                StatusTests.MakeByteArray(properties.status));

        internal static void Validate(in Th075AllScoreDataWrapper allScoreData, in Properties properties)
        {
            foreach (var pair in properties.clearData)
            {
                ClearDataTests.Validate(pair.Value, allScoreData.ClearData[pair.Key]);
            }

            StatusTests.Validate(properties.status, allScoreData.Status);
        }

        [TestMethod]
        public void Th075AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new Th075AllScoreDataWrapper();

            Assert.AreEqual(0, allScoreData.ClearData.Count);
            Assert.IsNull(allScoreData.Status);
        });

        [TestMethod]
        public void Th075AllScoreDataReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var allScoreData = Th075AllScoreDataWrapper.Create(MakeByteArray(properties));

            Validate(allScoreData, properties);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th075AllScoreDataReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var allScoreData = new Th075AllScoreDataWrapper();
            allScoreData.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
