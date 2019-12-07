using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th17;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th13.Stubs;
using static ThScoreFileConverterTests.Models.Th13.SpellCardTests;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverterTests.Models.Th17
{
    [TestClass]
    public class SpellCardTests
    {
        internal static SpellCardStub<Level> ValidStub { get; } = GetValidStub<Level>();

        internal static SpellCard Create(byte[] array)
        {
            var spellCard = new SpellCard();

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    spellCard.ReadFrom(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return spellCard;
        }

        internal static void Validate(ISpellCard expected, ISpellCard actual)
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
        public void SpellCardTest()
        {
            var stub = new SpellCardStub<Level>();
            var spellCard = new SpellCard();

            Validate(stub, spellCard);
            Assert.IsFalse(spellCard.HasTried);
        }

        [TestMethod]
        public void ReadFromTest()
        {
            var properties = ValidStub;

            var spellCard = Create(MakeByteArray(properties));

            Validate(properties, spellCard);
            Assert.IsTrue(spellCard.HasTried);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull()
        {
            var spellCard = new SpellCard();

            spellCard.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestShortenedName()
        {
            var stub = ValidStub;
            stub.Name = stub.Name.SkipLast(1).ToArray();

            _ = Create(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestExceededName()
        {
            var stub = ValidStub;
            stub.Name = stub.Name.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray();

            _ = Create(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        }

        public static IEnumerable<object[]> InvalidLevels => Th13.SpellCardTests.InvalidLevels;

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestInvalidLevel(int level)
        {
            var stub = ValidStub;
            stub.Level = TestUtils.Cast<Level>(level);

            _ = Create(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        }
    }
}
