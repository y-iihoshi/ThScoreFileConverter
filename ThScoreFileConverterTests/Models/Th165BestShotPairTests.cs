using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th165BestShotPairTests
    {
        internal struct Properties
        {
            public string path;
            public Th165BestShotHeaderTests.Properties header;
        };

        internal static Properties GetValidProperties() => new Properties()
        {
            path = @"%AppData%\ShanghaiAlice\th165\bestshot\bs15_1.dat",
            header = Th165BestShotHeaderTests.ValidProperties
        };

        internal static void Validate(in Th165BestShotPairWrapper pair, in Properties properties)
        {
            Assert.AreEqual(properties.path, pair.Path);
            Th165BestShotHeaderTests.Validate(pair.Header, properties.header);
        }

        [TestMethod]
        public void Th165BestShotPairTest() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();

            var header = Th165BestShotHeaderWrapper.Create(Th165BestShotHeaderTests.MakeByteArray(properties.header));
            var pair = new Th165BestShotPairWrapper(properties.path, header);

            Validate(pair, properties);
        });

        [TestMethod]
        public void DeconstructTest() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();

            var header = Th165BestShotHeaderWrapper.Create(Th165BestShotHeaderTests.MakeByteArray(properties.header));
            var pair = new Th165BestShotPairWrapper(properties.path, header);
            var (actualPath, actualHeader) = pair;

            Assert.AreEqual(properties.path, actualPath);
            Th165BestShotHeaderTests.Validate(actualHeader, properties.header);
        });
    }
}
