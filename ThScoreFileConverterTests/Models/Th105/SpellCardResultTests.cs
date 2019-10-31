using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th105;
using Chara = ThScoreFileConverter.Models.Th105.Chara;
using Level = ThScoreFileConverter.Models.Th105.Level;

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

        internal static Properties<TChara, TLevel> MakeValidProperties<TChara, TLevel>()
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
            in Properties<TChara, TLevel> expected, in SpellCardResult<TChara, TLevel> actual)
            where TChara : struct, Enum
            where TLevel : struct, Enum
        {
            Assert.AreEqual(expected.enemy, actual.Enemy);
            Assert.AreEqual(expected.level, actual.Level);
            Assert.AreEqual(expected.id, actual.Id);
            Assert.AreEqual(expected.trialCount, actual.TrialCount);
            Assert.AreEqual(expected.gotCount, actual.GotCount);
            Assert.AreEqual(expected.frames, actual.Frames);
        }

        internal static void SpellCardResultTestHelper<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = new Properties<TChara, TLevel>();

                var spellCardResult = new SpellCardResult<TChara, TLevel>();

                Validate(properties, spellCardResult);
            });

        internal static void ReadFromTestHelper<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = MakeValidProperties<TChara, TLevel>();

                var spellCardResult = TestUtils.Create<SpellCardResult<TChara, TLevel>>(MakeByteArray(properties));

                Validate(properties, spellCardResult);
            });

        internal static void ReadFromTestNullHelper<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var spellCardResult = new SpellCardResult<TChara, TLevel>();
                spellCardResult.ReadFrom(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        internal static void ReadFromTestShortenedHelper<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = MakeValidProperties<TChara, TLevel>();
                var array = MakeByteArray(properties).SkipLast(1).ToArray();

                _ = TestUtils.Create<SpellCardResult<TChara, TLevel>>(array);

                Assert.Fail(TestUtils.Unreachable);
            });

        internal static void ReadFromTestExceededHelper<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = MakeValidProperties<TChara, TLevel>();
                var array = MakeByteArray(properties).Concat(new byte[1] { 1 }).ToArray();

                var spellCardResult = TestUtils.Create<SpellCardResult<TChara, TLevel>>(array);

                Validate(properties, spellCardResult);
            });

        #region Th105

        [TestMethod]
        public void Th105SpellCardResultTest()
            => SpellCardResultTestHelper<Chara, Level>();

        [TestMethod]
        public void Th105SpellCardResultReadFromTest()
            => ReadFromTestHelper<Chara, Level>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th105SpellCardResultReadFromTestNull()
            => ReadFromTestNullHelper<Chara, Level>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th105SpellCardResultReadFromTestShortened()
            => ReadFromTestShortenedHelper<Chara, Level>();

        [TestMethod]
        public void Th105SpellCardResultReadFromTestExceeded()
            => ReadFromTestExceededHelper<Chara, Level>();

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
