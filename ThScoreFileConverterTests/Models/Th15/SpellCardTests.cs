using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th13;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th13.Stubs;
using ThScoreFileConverterTests.Models.Th13.Wrappers;

namespace ThScoreFileConverterTests.Models.Th15
{
    [TestClass]
    public class SpellCardTests
    {
        internal static SpellCardStub<Level> ValidStub { get; } = new SpellCardStub<Level>
        {
            Name = TestUtils.MakeRandomArray<byte>(0x80),
            ClearCount = 1,
            PracticeClearCount = 2,
            TrialCount = 3,
            PracticeTrialCount = 4,
            Id = 5,
            Level = Level.Normal,
            PracticeScore = 6789
        };

        internal static byte[] MakeByteArray(ISpellCard<Level> spellCard)
            => TestUtils.MakeByteArray(
                spellCard.Name,
                spellCard.ClearCount,
                spellCard.PracticeClearCount,
                spellCard.TrialCount,
                spellCard.PracticeTrialCount,
                spellCard.Id - 1,
                TestUtils.Cast<int>(spellCard.Level),
                spellCard.PracticeScore);

        internal static void Validate(ISpellCard<Level> expected, in SpellCardWrapper<Th15Converter, Level> actual)
        {
            CollectionAssert.That.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.ClearCount, actual.ClearCount);
            Assert.AreEqual(expected.PracticeClearCount, actual.PracticeClearCount);
            Assert.AreEqual(expected.TrialCount, actual.TrialCount);
            Assert.AreEqual(expected.PracticeTrialCount, actual.PracticeTrialCount);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Level, actual.Level);
            Assert.AreEqual(expected.PracticeScore, actual.PracticeScore);
        }

        [TestMethod]
        public void Th15SpellCardTest() => TestUtils.Wrap(() =>
        {
            var stub = new SpellCardStub<Level>();
            var spellCard = new SpellCardWrapper<Th15Converter, Level>();

            Validate(stub, spellCard);
            Assert.IsFalse(spellCard.HasTried.Value);
        });

        [TestMethod]
        public void Th15SpellCardReadFromTest() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var spellCard = SpellCardWrapper<Th15Converter, Level>.Create(MakeByteArray(stub));

            Validate(stub, spellCard);
            Assert.IsTrue(spellCard.HasTried.Value);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th15SpellCardReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var spellCard = new SpellCardWrapper<Th15Converter, Level>();

            spellCard.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th15SpellCardReadFromTestShortenedName() => TestUtils.Wrap(() =>
        {
            var stub = new SpellCardStub<Level>(ValidStub);
            stub.Name = stub.Name.SkipLast(1).ToArray();

            _ = SpellCardWrapper<Th15Converter, Level>.Create(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th15SpellCardReadFromTestExceededName() => TestUtils.Wrap(() =>
        {
            var stub = new SpellCardStub<Level>(ValidStub);
            stub.Name = stub.Name.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray();

            _ = SpellCardWrapper<Th15Converter, Level>.Create(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Level));

        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th15SpellCardReadFromTestInvalidLevel(int level) => TestUtils.Wrap(() =>
        {
            var stub = new SpellCardStub<Level>(ValidStub)
            {
                Level = TestUtils.Cast<Level>(level),
            };

            _ = SpellCardWrapper<Th15Converter, Level>.Create(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
