using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th10LevelStagePairTests
    {
        internal struct Properties<TLevel, TStage>
            where TLevel : struct, Enum
            where TStage : struct, Enum
        {
            public TLevel level;
            public TStage stage;
        };

        internal static Properties<TLevel, TStage> GetValidProperties<TLevel, TStage>()
            where TLevel : struct, Enum
            where TStage : struct, Enum
            => new Properties<TLevel, TStage>()
            {
                level = TestUtils.Cast<TLevel>(1),
                stage = TestUtils.Cast<TStage>(2)
            };

        internal static void Validate<TParent, TLevel, TStage>(
            in Th10LevelStagePairWrapper<TParent, TLevel, TStage> pair,
            in Properties<TLevel, TStage> properties)
            where TParent : ThConverter
            where TLevel : struct, Enum
            where TStage : struct, Enum
        {
            Assert.AreEqual(properties.level, pair.Level);
            Assert.AreEqual(properties.stage, pair.Stage);
        }

        internal static void Validate<TParent, TLevel, TStage, TLevelArg, TStageArg>(
            in Th10LevelStagePairWrapper<TParent, TLevel, TStage, TLevelArg, TStageArg> pair,
            in Properties<TLevel, TStage> properties)
            where TParent : ThConverter
            where TLevel : struct, Enum
            where TStage : struct, Enum
            where TLevelArg : struct, Enum
            where TStageArg : struct, Enum
        {
            Assert.AreEqual(properties.level, pair.Level);
            Assert.AreEqual(properties.stage, pair.Stage);
        }

        internal static void LevelStagePairTestHelper<TParent, TLevel, TStage>()
            where TParent : ThConverter
            where TLevel : struct, Enum
            where TStage : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties<TLevel, TStage>();

                var pair = new Th10LevelStagePairWrapper<TParent, TLevel, TStage>(
                    properties.level, properties.stage);

                Validate(pair, properties);
            });

        internal static void LevelStagePairTestHelper<TParent, TLevel, TStage, TLevelArg, TStageArg>()
            where TParent : ThConverter
            where TLevel : struct, Enum
            where TStage : struct, Enum
            where TLevelArg : struct, Enum
            where TStageArg : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties<TLevel, TStage>();

                var pair = new Th10LevelStagePairWrapper<TParent, TLevel, TStage, TLevelArg, TStageArg>(
                    properties.level, properties.stage);

                Validate(pair, properties);
            });

        internal static void LevelStagePairTestCastHelper<TParent, TLevel, TStage, TLevelArg, TStageArg>()
            where TParent : ThConverter
            where TLevel : struct, Enum
            where TStage : struct, Enum
            where TLevelArg : struct, Enum
            where TStageArg : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties<TLevel, TStage>();

                var pair = new Th10LevelStagePairWrapper<TParent, TLevel, TStage, TLevelArg, TStageArg>(
                    TestUtils.Cast<TLevelArg>(properties.level), TestUtils.Cast<TStageArg>(properties.stage));

                Validate(pair, properties);
            });

        [TestMethod]
        public void Th10LevelStagePairTest()
            => LevelStagePairTestHelper<Th10Converter, ThConverter.Level, ThConverter.Stage>();

        [TestMethod]
        public void Th11LevelStagePairTest()
            => LevelStagePairTestHelper<Th11Converter, ThConverter.Level, ThConverter.Stage>();

        [TestMethod]
        public void Th12LevelStagePairTest()
            => LevelStagePairTestHelper<Th12Converter, ThConverter.Level, ThConverter.Stage>();

        [TestMethod]
        public void Th13LevelStagePairTest()
            => LevelStagePairTestHelper<Th13Converter, Th13Converter.LevelPractice, Th13Converter.StagePractice>();

        [TestMethod]
        public void Th14LevelStagePairTest()
            => LevelStagePairTestHelper<
                Th14Converter,
                Th14Converter.LevelPractice,
                Th14Converter.StagePractice,
                ThConverter.Level,
                ThConverter.Stage>();

        [TestMethod]
        public void Th14LevelStagePairTestCast()
            => LevelStagePairTestCastHelper<
                Th14Converter,
                Th14Converter.LevelPractice,
                Th14Converter.StagePractice,
                ThConverter.Level,
                ThConverter.Stage>();

        [TestMethod]
        public void Th15LevelStagePairTest()
            => LevelStagePairTestHelper<
                Th15Converter,
                ThConverter.Level,
                Th15Converter.StagePractice,
                ThConverter.Level,
                ThConverter.Stage>();

        [TestMethod]
        public void Th15LevelStagePairTestCast()
            => LevelStagePairTestCastHelper<
                Th15Converter,
                ThConverter.Level,
                Th15Converter.StagePractice,
                ThConverter.Level,
                ThConverter.Stage>();

        [TestMethod]
        public void Th16LevelStagePairTest()
            => LevelStagePairTestHelper<
                Th16Converter,
                ThConverter.Level,
                Th16Converter.StagePractice,
                ThConverter.Level,
                ThConverter.Stage>();

        [TestMethod]
        public void Th16LevelStagePairTestCast()
            => LevelStagePairTestCastHelper<
                Th16Converter,
                ThConverter.Level,
                Th16Converter.StagePractice,
                ThConverter.Level,
                ThConverter.Stage>();
    }
}
