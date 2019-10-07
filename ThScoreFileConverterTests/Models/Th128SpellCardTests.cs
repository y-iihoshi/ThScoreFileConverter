using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th128;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th128.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th128SpellCardTests
    {
        internal static SpellCardStub ValidStub => new SpellCardStub()
        {
            Name = TestUtils.MakeRandomArray<byte>(0x80),
            NoMissCount = 12,
            NoIceCount = 34,
            TrialCount = 56,
            Id = 78,
            Level = Level.Normal
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

        internal static void Validate(ISpellCard expected, in Th128SpellCardWrapper actual)
        {
            CollectionAssert.That.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.NoMissCount, actual.NoMissCount);
            Assert.AreEqual(expected.NoIceCount, actual.NoIceCount);
            Assert.AreEqual(expected.TrialCount, actual.TrialCount);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Level, actual.Level);
        }

        [TestMethod]
        public void Th128SpellCardTest() => TestUtils.Wrap(() =>
        {
            var stub = new SpellCardStub();
            var spellCard = new Th128SpellCardWrapper();

            Validate(stub, spellCard);
            Assert.IsFalse(spellCard.HasTried().Value);
        });

        [TestMethod]
        public void Th128SpellCardReadFromTest() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var spellCard = Th128SpellCardWrapper.Create(MakeByteArray(stub));

            Validate(stub, spellCard);
            Assert.IsTrue(spellCard.HasTried().Value);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th128SpellCardReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var spellCard = new Th128SpellCardWrapper();
            spellCard.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th128SpellCardReadFromTestShortenedName() => TestUtils.Wrap(() =>
        {
            var stub = new SpellCardStub(ValidStub);
            stub.Name = stub.Name.SkipLast(1).ToArray();

            _ = Th128SpellCardWrapper.Create(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th128SpellCardReadFromTestExceededName() => TestUtils.Wrap(() =>
        {
            var stub = new SpellCardStub(ValidStub);
            stub.Name = stub.Name.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray();

            _ = Th128SpellCardWrapper.Create(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Level));

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th128SpellCardReadFromTestInvalidLevel(int level) => TestUtils.Wrap(() =>
        {
            var stub = new SpellCardStub(ValidStub)
            {
                Level = TestUtils.Cast<Level>(level),
            };

            _ = Th128SpellCardWrapper.Create(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
