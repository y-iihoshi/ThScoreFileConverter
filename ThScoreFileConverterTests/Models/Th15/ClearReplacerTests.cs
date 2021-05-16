using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th15;
using GameMode = ThScoreFileConverter.Models.Th15.GameMode;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverterTests.Models.Th15
{
    [TestClass]
    public class ClearReplacerTests
    {
        private static IEnumerable<IClearData> CreateClearDataList()
        {
            static IScoreData CreateScoreData(LevelWithTotal level, int index)
            {
                var mock = new Mock<IScoreData>();
                _ = mock.SetupGet(s => s.StageProgress).Returns(
                    level == LevelWithTotal.Extra ? StageProgress.Extra : (StageProgress)(5 - (index % 5)));
                _ = mock.SetupGet(s => s.DateTime).Returns((uint)index % 2);
                return mock.Object;
            }

            static IClearDataPerGameMode CreateClearDataPerGameMode()
            {
                var mock = new Mock<IClearDataPerGameMode>();
                _ = mock.SetupGet(c => c.Rankings).Returns(
                    EnumHelper<LevelWithTotal>.Enumerable.ToDictionary(
                        level => level,
                        level => Enumerable.Range(0, 10).Select(index => CreateScoreData(level, index)).ToList()
                            as IReadOnlyList<IScoreData>));
                return mock.Object;
            }

            var mock = new Mock<IClearData>();
            _ = mock.SetupGet(c => c.Chara).Returns(CharaWithTotal.Marisa);
            _ = mock.SetupGet(c => c.GameModeData).Returns(
                new[] { (GameMode.Pointdevice, CreateClearDataPerGameMode()) }.ToDictionary());
            return new[] { mock.Object };
        }

        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            CreateClearDataList().ToDictionary(clearData => clearData.Chara);

        [TestMethod]
        public void ClearReplacerTest()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ClearReplacerTestEmpty()
        {
            var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
            var replacer = new ClearReplacer(dictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTest()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("Stage 5", replacer.Replace("%T15CLEARPHMR"));
        }

        [TestMethod]
        public void ReplaceTestExtra()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("Not Clear", replacer.Replace("%T15CLEARPXMR"));
        }

        [TestMethod]
        public void ReplaceTestExtraClear()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.Marisa)
                         && (m.GameModeData == new Dictionary<GameMode, IClearDataPerGameMode>
                            {
                                {
                                    GameMode.Pointdevice,
                                    Mock.Of<IClearDataPerGameMode>(
                                        c => c.Rankings == EnumHelper<LevelWithTotal>.Enumerable.ToDictionary(
                                            level => level,
                                            level => new[]
                                            {
                                                Mock.Of<IScoreData>(
                                                    s => (s.StageProgress == StageProgress.ExtraClear)
                                                         && (s.DateTime == 1u))
                                            } as IReadOnlyList<IScoreData>))
                                },
                            }))
            }.ToDictionary(element => element.Chara);

            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("All Clear", replacer.Replace("%T15CLEARPXMR"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("-------", replacer.Replace("%T15CLEARPHMR"));
        }

        [TestMethod]
        public void ReplaceTestEmptyGameModes()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.Marisa)
                         && (m.GameModeData == ImmutableDictionary<GameMode, IClearDataPerGameMode>.Empty))
            }.ToDictionary(clearData => clearData.Chara);

            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("-------", replacer.Replace("%T15CLEARPHMR"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRankings()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.Marisa)
                         && (m.GameModeData == new Dictionary<GameMode, IClearDataPerGameMode>
                            {
                                {
                                    GameMode.Pointdevice,
                                    Mock.Of<IClearDataPerGameMode>(
                                        c => c.Rankings == ImmutableDictionary<LevelWithTotal, IReadOnlyList<IScoreData>>.Empty)
                                },
                            })
                    )
            }.ToDictionary(clearData => clearData.Chara);

            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("-------", replacer.Replace("%T15CLEARPHMR"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRanking()
        {
            var dictionary = new[]
            {
                Mock.Of<IClearData>(
                    m => (m.Chara == CharaWithTotal.Marisa)
                         && (m.GameModeData == new Dictionary<GameMode, IClearDataPerGameMode>
                            {
                                {
                                    GameMode.Pointdevice,
                                    Mock.Of<IClearDataPerGameMode>(
                                        c => c.Rankings == EnumHelper<LevelWithTotal>.Enumerable.ToDictionary(
                                            level => level,
                                            level => ImmutableList<IScoreData>.Empty as IReadOnlyList<IScoreData>))
                                },
                            }))
            }.ToDictionary(clearData => clearData.Chara);

            var replacer = new ClearReplacer(dictionary);
            Assert.AreEqual("-------", replacer.Replace("%T15CLEARPHMR"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15XXXXXPHMR", replacer.Replace("%T15XXXXXPHMR"));
        }

        [TestMethod]
        public void ReplaceTestInvalidGameMode()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15CLEARXHMR", replacer.Replace("%T15CLEARXHMR"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15CLEARPYMR", replacer.Replace("%T15CLEARPYMR"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new ClearReplacer(ClearDataDictionary);
            Assert.AreEqual("%T15CLEARPHXX", replacer.Replace("%T15CLEARPHXX"));
        }
    }
}
