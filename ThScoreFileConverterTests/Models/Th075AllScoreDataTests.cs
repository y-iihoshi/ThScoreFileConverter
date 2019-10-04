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
            public Dictionary<Chara, Dictionary<Level, ClearDataTests.Properties>> clearData;
            public StatusTests.Properties status;
        };

        internal static Properties ValidProperties => new Properties()
        {
            clearData = Utils.GetEnumerator<Chara>().ToDictionary(
                chara => chara,
                chara => Utils.GetEnumerator<Level>().ToDictionary(
                    level => level,
                    level => ClearDataTests.ValidProperties)),
            status = StatusTests.ValidProperties
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.clearData.SelectMany(
                    perCharaPair => perCharaPair.Value.SelectMany(
                        perLevelPair => ClearDataTests.MakeByteArray(perLevelPair.Value))).ToArray(),
                Enumerable.Range(1, 4).SelectMany(
                    index => Utils.GetEnumerator<Level>().SelectMany(
                        level => ClearDataTests.MakeByteArray(ClearDataTests.ValidProperties))).ToArray(),
                StatusTests.MakeByteArray(properties.status));

        internal static void Validate(in Th075AllScoreDataWrapper allScoreData, in Properties properties)
        {
            foreach (var perCharaPair in properties.clearData)
            {
                foreach (var perLevelPair in perCharaPair.Value)
                {
                    ClearDataTests.Validate(
                        perLevelPair.Value, allScoreData.ClearData[perCharaPair.Key][perLevelPair.Key]);
                }
            }

            StatusTests.Validate(properties.status, allScoreData.Status);
        }

        [TestMethod]
        public void Th075AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var charas = Utils.GetEnumerator<Chara>();
            var allScoreData = new Th075AllScoreDataWrapper();

            Assert.AreEqual(charas.Count(), allScoreData.ClearData.Count);

            foreach (var chara in charas)
            {
                Assert.AreEqual(0, allScoreData.ClearData[chara].Count);
            }

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
