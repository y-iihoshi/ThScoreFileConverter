using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th075AllScoreDataTests
    {
        internal struct Properties
        {
            public Dictionary<
                Th075Converter.Chara,
                Dictionary<Th075Converter.Level, Th075ClearDataTests.Properties>> clearData;
            public Th075StatusTests.Properties status;
        };

        internal static Properties ValidProperties => new Properties()
        {
            clearData = Utils.GetEnumerator<Th075Converter.Chara>().ToDictionary(
                chara => chara,
                chara => Utils.GetEnumerator<Th075Converter.Level>().ToDictionary(
                    level => level,
                    level => Th075ClearDataTests.ValidProperties)),
            status = Th075StatusTests.ValidProperties
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.clearData.SelectMany(
                    perCharaPair => perCharaPair.Value.SelectMany(
                        perLevelPair => Th075ClearDataTests.MakeByteArray(perLevelPair.Value))).ToArray(),
                Enumerable.Range(1, 4).SelectMany(
                    index => Utils.GetEnumerator<Th075Converter.Level>().SelectMany(
                        level => Th075ClearDataTests.MakeByteArray(Th075ClearDataTests.ValidProperties))).ToArray(),
                Th075StatusTests.MakeByteArray(properties.status));

        internal static void Validate(in Th075AllScoreDataWrapper allScoreData, in Properties properties)
        {
            foreach (var perCharaPair in properties.clearData)
            {
                foreach (var perLevelPair in perCharaPair.Value)
                {
                    Th075ClearDataTests.Validate(
                        allScoreData.ClearDataPerCharaLevel(perCharaPair.Key, perLevelPair.Key), perLevelPair.Value);
                }
            }

            Th075StatusTests.Validate(allScoreData.Status, properties.status);
        }

        [TestMethod]
        public void Th075AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var charas = Utils.GetEnumerator<Th075Converter.Chara>();
            var allScoreData = new Th075AllScoreDataWrapper();

            Assert.AreEqual(charas.Count(), allScoreData.ClearDataCount);

            foreach (var chara in charas)
            {
                Assert.AreEqual(0, allScoreData.ClearDataPerCharaCount(chara));
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
