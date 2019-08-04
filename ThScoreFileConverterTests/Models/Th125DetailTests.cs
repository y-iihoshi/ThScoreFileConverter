using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th125DetailTests
    {
        internal struct Properties
        {
            public bool outputs;
            public string format;
            public string value;
        };

        internal static Properties ValidProperties => new Properties()
        {
            outputs = true,
            format = "Cat Bonus     {0,9}",
            value = "+ 666"
        };

        internal static void Validate(in Th125DetailWrapper detail, in Properties properties)
        {
            Assert.AreEqual(properties.outputs, detail.Outputs);
            Assert.AreEqual(properties.format, detail.Format);
            Assert.AreEqual(properties.value, detail.Value);
        }

        [TestMethod]
        public void Th125DetailTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var spellCardInfo = new Th125DetailWrapper(properties.outputs, properties.format, properties.value);

            Validate(spellCardInfo, properties);
        });
    }
}
