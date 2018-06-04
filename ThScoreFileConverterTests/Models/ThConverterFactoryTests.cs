using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Properties;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class ThConverterFactoryTests
    {
        [TestMethod()]
        public void CreateTest()
        {
            var converter = ThConverterFactory.Create(Resources.keyTh06);

            Assert.AreEqual(typeof(Th06Converter), converter.GetType());
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "converter")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateTestNull()
        {
            var converter = ThConverterFactory.Create(null);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod()]
        public void CreateTestEmptyKey()
        {
            var converter = ThConverterFactory.Create(string.Empty);

            Assert.IsNull(converter);
        }

        [TestMethod()]
        public void CreateTestInvalidKey()
        {
            var converter = ThConverterFactory.Create("invalidKey");

            Assert.IsNull(converter);
        }
    }
}
