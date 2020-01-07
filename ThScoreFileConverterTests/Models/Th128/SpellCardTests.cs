using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th128;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th128.Stubs;

namespace ThScoreFileConverterTests.Models.Th128
{
    [TestClass]
    public class SpellCardTests
    {
        internal static SpellCardStub ValidStub { get; } = new SpellCardStub()
        {
            Name = TestUtils.MakeRandomArray<byte>(0x80),
            NoMissCount = 12,
            NoIceCount = 34,
            TrialCount = 56,
            Id = 78,
            Level = Level.Normal,
        };

        internal static byte[] MakeByteArray(ISpellCard spellCard)
        => TestUtils.MakeByteArray(
            spellCard.Name,
            spellCard.NoMissCount,
            spellCard.NoIceCount,
            0u,
            spellCard.TrialCount,
            spellCard.Id - 1,
            (int)spellCard.Level);

        internal static void Validate(ISpellCard expected, ISpellCard actual)
        {
            CollectionAssert.That.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.NoMissCount, actual.NoMissCount);
            Assert.AreEqual(expected.NoIceCount, actual.NoIceCount);
            Assert.AreEqual(expected.TrialCount, actual.TrialCount);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Level, actual.Level);
        }

        [TestMethod]
        public void SpellCardTest()
        {
            var stub = new SpellCardStub();
            var spellCard = new SpellCard();

            Validate(stub, spellCard);
            Assert.IsFalse(spellCard.HasTried());
        }

        [TestMethod]
        public void ReadFromTest()
        {
            var stub = ValidStub;

            var spellCard = TestUtils.Create<SpellCard>(MakeByteArray(stub));

            Validate(stub, spellCard);
            Assert.IsTrue(spellCard.HasTried());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull()
        {
            var spellCard = new SpellCard();
            spellCard.ReadFrom(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortenedName()
        {
            var stub = new SpellCardStub(ValidStub);
            stub.Name = stub.Name.SkipLast(1).ToArray();

            _ = TestUtils.Create<SpellCard>(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestExceededName()
        {
            var stub = new SpellCardStub(ValidStub);
            stub.Name = stub.Name.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray();

            _ = TestUtils.Create<SpellCard>(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        }

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Level));

        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestInvalidLevel(int level)
        {
            var stub = new SpellCardStub(ValidStub)
            {
                Level = TestUtils.Cast<Level>(level),
            };

            _ = TestUtils.Create<SpellCard>(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        }
    }
}
