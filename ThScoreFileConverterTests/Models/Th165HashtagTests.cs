using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th165HashtagTests
    {
        internal struct Properties
        {
            public bool outputs;
            public string name;
        };

        internal static Properties ValidProperties { get; } = new Properties()
        {
            outputs = true,
            name = "＃座薬ｗｗｗ"
        };

        internal static void Validate(in Th165HashtagWrapper hashtag, in Properties properties)
        {
            Assert.AreEqual(properties.outputs, hashtag.Outputs);
            Assert.AreEqual(properties.name, hashtag.Name);
        }

        [TestMethod]
        public void Th165HashtagTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var spellCardInfo = new Th165HashtagWrapper(properties.outputs, properties.name);

            Validate(spellCardInfo, properties);
        });
    }
}
