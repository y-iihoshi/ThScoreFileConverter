using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models.Th105;
using ThScoreFileConverterTests.Models.Th105.Stubs;

namespace ThScoreFileConverterTests.Models.Th105
{
    [TestClass]
    public class SpellCardResultTests
    {
        internal static SpellCardResultStub<TChara, TLevel> MakeValidStub<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => new SpellCardResultStub<TChara, TLevel>()
            {
                Enemy = TestUtils.Cast<TChara>(1),
                Level = TestUtils.Cast<TLevel>(2),
                Id = 3,
                TrialCount = 67,
                GotCount = 45,
                Frames = 8901
            };

        internal static byte[] MakeByteArray<TChara, TLevel>(ISpellCardResult<TChara, TLevel> properties)
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.MakeByteArray(
                TestUtils.Cast<int>(properties.Enemy),
                TestUtils.Cast<int>(properties.Level),
                properties.Id,
                properties.TrialCount,
                properties.GotCount,
                properties.Frames);

        internal static void Validate<TChara, TLevel>(
            ISpellCardResult<TChara, TLevel> expected, ISpellCardResult<TChara, TLevel> actual)
            where TChara : struct, Enum
            where TLevel : struct, Enum
        {
            Assert.AreEqual(expected.Enemy, actual.Enemy);
            Assert.AreEqual(expected.Level, actual.Level);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.TrialCount, actual.TrialCount);
            Assert.AreEqual(expected.GotCount, actual.GotCount);
            Assert.AreEqual(expected.Frames, actual.Frames);
        }

        internal static void SpellCardResultTestHelper<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = new SpellCardResultStub<TChara, TLevel>();

                var spellCardResult = new SpellCardResult<TChara, TLevel>();

                Validate(stub, spellCardResult);
            });

        internal static void ReadFromTestHelper<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = MakeValidStub<TChara, TLevel>();

                var spellCardResult = TestUtils.Create<SpellCardResult<TChara, TLevel>>(MakeByteArray(stub));

                Validate(stub, spellCardResult);
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
                var stub = MakeValidStub<TChara, TLevel>();
                var array = MakeByteArray(stub).SkipLast(1).ToArray();

                _ = TestUtils.Create<SpellCardResult<TChara, TLevel>>(array);

                Assert.Fail(TestUtils.Unreachable);
            });

        internal static void ReadFromTestExceededHelper<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var stub = MakeValidStub<TChara, TLevel>();
                var array = MakeByteArray(stub).Concat(new byte[1] { 1 }).ToArray();

                var spellCardResult = TestUtils.Create<SpellCardResult<TChara, TLevel>>(array);

                Validate(stub, spellCardResult);
            });

        [TestMethod]
        public void SpellCardResultTest() => SpellCardResultTestHelper<Chara, Level>();

        [TestMethod]
        public void ReadFromTest() => ReadFromTestHelper<Chara, Level>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => ReadFromTestNullHelper<Chara, Level>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortened() => ReadFromTestShortenedHelper<Chara, Level>();

        [TestMethod]
        public void ReadFromTestExceeded() => ReadFromTestExceededHelper<Chara, Level>();
    }
}
