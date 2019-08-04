using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th105;
using ThScoreFileConverterTests.Models.Th105.Wrappers;

namespace ThScoreFileConverterTests.Models.Th105
{
    [TestClass]
    public class SpellCardResultTests
    {
        internal struct Properties<TChara, TLevel>
            where TChara : struct, Enum
            where TLevel : struct, Enum
        {
            public TChara enemy;
            public TLevel level;
            public int id;
            public int trialCount;
            public int gotCount;
            public uint frames;
        };

        internal static Properties<TChara, TLevel> GetValidProperties<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => new Properties<TChara, TLevel>()
            {
                enemy = TestUtils.Cast<TChara>(1),
                level = TestUtils.Cast<TLevel>(2),
                id = 3,
                trialCount = 67,
                gotCount = 45,
                frames = 8901
            };

        internal static byte[] MakeByteArray<TChara, TLevel>(in Properties<TChara, TLevel> properties)
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.MakeByteArray(
                TestUtils.Cast<int>(properties.enemy),
                TestUtils.Cast<int>(properties.level),
                properties.id,
                properties.trialCount,
                properties.gotCount,
                properties.frames);

        internal static void Validate<TChara, TLevel>(
            in SpellCardResultWrapper<TChara, TLevel> spellCardResult,
            in Properties<TChara, TLevel> properties)
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => Validate(spellCardResult.Target as SpellCardResult<TChara, TLevel>, properties);

        internal static void Validate<TChara, TLevel>(
            in SpellCardResult<TChara, TLevel> spellCardResult,
            in Properties<TChara, TLevel> properties)
            where TChara : struct, Enum
            where TLevel : struct, Enum
        {
            Assert.AreEqual(properties.enemy, spellCardResult.Enemy);
            Assert.AreEqual(properties.level, spellCardResult.Level);
            Assert.AreEqual(properties.id, spellCardResult.Id);
            Assert.AreEqual(properties.trialCount, spellCardResult.TrialCount);
            Assert.AreEqual(properties.gotCount, spellCardResult.GotCount);
            Assert.AreEqual(properties.frames, spellCardResult.Frames);
        }

        internal static void SpellCardResultTestHelper<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = new Properties<TChara, TLevel>();

                var spellCardResult = new SpellCardResultWrapper<TChara, TLevel>();

                Validate(spellCardResult, properties);
            });

        internal static void ReadFromTestHelper<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties<TChara, TLevel>();

                var spellCardResult =
                    SpellCardResultWrapper<TChara, TLevel>.Create(MakeByteArray(properties));

                Validate(spellCardResult, properties);
            });

        internal static void ReadFromTestNullHelper<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var spellCardResult = new SpellCardResultWrapper<TChara, TLevel>();
                spellCardResult.ReadFrom(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        internal static void ReadFromTestShortenedHelper<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties<TChara, TLevel>();
                var array = MakeByteArray(properties);
                array = array.Take(array.Length - 1).ToArray();

                SpellCardResultWrapper<TChara, TLevel>.Create(array);

                Assert.Fail(TestUtils.Unreachable);
            });

        internal static void ReadFromTestExceededHelper<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties<TChara, TLevel>();
                var array = MakeByteArray(properties).Concat(new byte[1] { 1 }).ToArray();

                var spellCardResult = SpellCardResultWrapper<TChara, TLevel>.Create(array);

                Validate(spellCardResult, properties);
            });

        #region Th105

        [TestMethod]
        public void Th105SpellCardResultTest()
            => SpellCardResultTestHelper<Th105Converter.Chara, Th105Converter.Level>();

        [TestMethod]
        public void Th105SpellCardResultReadFromTest()
            => ReadFromTestHelper<Th105Converter.Chara, Th105Converter.Level>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th105SpellCardResultReadFromTestNull()
            => ReadFromTestNullHelper<Th105Converter.Chara, Th105Converter.Level>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th105SpellCardResultReadFromTestShortened()
            => ReadFromTestShortenedHelper<Th105Converter.Chara, Th105Converter.Level>();

        [TestMethod]
        public void Th105SpellCardResultReadFromTestExceeded()
            => ReadFromTestExceededHelper<Th105Converter.Chara, Th105Converter.Level>();

        #endregion

        #region Th123

        [TestMethod]
        public void Th123SpellCardResultTest()
            => SpellCardResultTestHelper<Th123Converter.Chara, Th123Converter.Level>();

        [TestMethod]
        public void Th123SpellCardResultReadFromTest()
            => ReadFromTestHelper<Th123Converter.Chara, Th123Converter.Level>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th123SpellCardResultReadFromTestNull()
            => ReadFromTestNullHelper<Th123Converter.Chara, Th123Converter.Level>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th123SpellCardResultReadFromTestShortened()
            => ReadFromTestShortenedHelper<Th123Converter.Chara, Th123Converter.Level>();

        [TestMethod]
        public void Th123SpellCardResultReadFromTestExceeded()
            => ReadFromTestExceededHelper<Th123Converter.Chara, Th123Converter.Level>();

        #endregion
    }
}
