using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class ThConverterFactoryTests
    {
        [TestMethod]
        public void CreateTest()
        {
            var converter = ThConverterFactory.Create("TH06");

            Assert.AreEqual(typeof(Th06Converter), converter?.GetType());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateTestNull()
        {
            _ = ThConverterFactory.Create(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void CreateTestEmptyKey()
        {
            var converter = ThConverterFactory.Create(string.Empty);

            Assert.IsNull(converter);
        }

        [TestMethod]
        public void CreateTestInvalidKey()
        {
            var converter = ThConverterFactory.Create("invalidKey");

            Assert.IsNull(converter);
        }
    }
}
