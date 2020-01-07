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
        internal static SpellCardResultStub<TChara> MakeValidStub<TChara>()
            where TChara : struct, Enum
            => new SpellCardResultStub<TChara>()
            {
                Enemy = TestUtils.Cast<TChara>(1),
                Level = Level.Hard,
                Id = 3,
                TrialCount = 67,
                GotCount = 45,
                Frames = 8901
            };

        internal static byte[] MakeByteArray<TChara>(ISpellCardResult<TChara> properties)
            where TChara : struct, Enum
            => TestUtils.MakeByteArray(
                TestUtils.Cast<int>(properties.Enemy),
                TestUtils.Cast<int>(properties.Level),
                properties.Id,
                properties.TrialCount,
                properties.GotCount,
                properties.Frames);

        internal static void Validate<TChara>(ISpellCardResult<TChara> expected, ISpellCardResult<TChara> actual)
            where TChara : struct, Enum
        {
            Assert.AreEqual(expected.Enemy, actual.Enemy);
            Assert.AreEqual(expected.Level, actual.Level);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.TrialCount, actual.TrialCount);
            Assert.AreEqual(expected.GotCount, actual.GotCount);
            Assert.AreEqual(expected.Frames, actual.Frames);
        }

        internal static void SpellCardResultTestHelper<TChara>()
            where TChara : struct, Enum
        {
            var stub = new SpellCardResultStub<TChara>();

            var spellCardResult = new SpellCardResult<TChara>();

            Validate(stub, spellCardResult);
        }

        internal static void ReadFromTestHelper<TChara>()
            where TChara : struct, Enum
        {
            var stub = MakeValidStub<TChara>();

            var spellCardResult = TestUtils.Create<SpellCardResult<TChara>>(MakeByteArray(stub));

            Validate(stub, spellCardResult);
        }

        internal static void ReadFromTestNullHelper<TChara>()
            where TChara : struct, Enum
        {
            var spellCardResult = new SpellCardResult<TChara>();
            spellCardResult.ReadFrom(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        internal static void ReadFromTestShortenedHelper<TChara>()
            where TChara : struct, Enum
        {
            var stub = MakeValidStub<TChara>();
            var array = MakeByteArray(stub).SkipLast(1).ToArray();

            _ = TestUtils.Create<SpellCardResult<TChara>>(array);

            Assert.Fail(TestUtils.Unreachable);
        }

        internal static void ReadFromTestExceededHelper<TChara>()
            where TChara : struct, Enum
        {
            var stub = MakeValidStub<TChara>();
            var array = MakeByteArray(stub).Concat(new byte[1] { 1 }).ToArray();

            var spellCardResult = TestUtils.Create<SpellCardResult<TChara>>(array);

            Validate(stub, spellCardResult);
        }

        [TestMethod]
        public void SpellCardResultTest() => SpellCardResultTestHelper<Chara>();

        [TestMethod]
        public void ReadFromTest() => ReadFromTestHelper<Chara>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => ReadFromTestNullHelper<Chara>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortened() => ReadFromTestShortenedHelper<Chara>();

        [TestMethod]
        public void ReadFromTestExceeded() => ReadFromTestExceededHelper<Chara>();
    }
}
