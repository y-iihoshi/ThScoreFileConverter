using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void CreateTestNull()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => _ = ThConverterFactory.Create(null!));

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
