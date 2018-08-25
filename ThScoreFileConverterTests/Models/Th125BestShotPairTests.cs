using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th125BestShotPairTests
    {
        internal struct Properties
        {
            public string path;
            public Th125BestShotHeaderTests.Properties header;
        };

        internal static Properties GetValidProperties() => new Properties()
        {
            path = @"%AppData%\ShanghaiAlice\th125\bestshot\bs2_09_7.dat",
            header = Th125BestShotHeaderTests.ValidProperties
        };

        internal static void Validate(in Th125BestShotPairWrapper pair, in Properties properties)
        {
            Assert.AreEqual(properties.path, pair.Path);
            Th125BestShotHeaderTests.Validate(pair.Header, properties.header);
        }

        [TestMethod]
        public void Th125BestShotPairTest() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();

            var header = Th125BestShotHeaderWrapper.Create(Th125BestShotHeaderTests.MakeByteArray(properties.header));
            var pair = new Th125BestShotPairWrapper(properties.path, header);

            Validate(pair, properties);
        });
    }
}
