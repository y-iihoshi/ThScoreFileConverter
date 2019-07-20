using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th095BestShotPairTests
    {
        internal struct Properties
        {
            public string path;
            public Th095BestShotHeaderTests.Properties header;
        };

        internal static Properties GetValidProperties() => new Properties()
        {
            path = @"D:\path\to\東方文花帖\bestshot\bs_09_6.dat",
            header = Th095BestShotHeaderTests.ValidProperties
        };

        internal static void Validate(in Th095BestShotPairWrapper pair, in Properties properties)
        {
            Assert.AreEqual(properties.path, pair.Path);
            Th095BestShotHeaderTests.Validate(pair.Header, properties.header);
        }

        [TestMethod]
        public void Th095BestShotPairTest() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();

            var header = Th095BestShotHeaderWrapper.Create(Th095BestShotHeaderTests.MakeByteArray(properties.header));
            var pair = new Th095BestShotPairWrapper(properties.path, header);

            Validate(pair, properties);
        });

        [TestMethod]
        public void DeconstructTest() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();

            var header = Th095BestShotHeaderWrapper.Create(Th095BestShotHeaderTests.MakeByteArray(properties.header));
            var pair = new Th095BestShotPairWrapper(properties.path, header);
            var (actualPath, actualHeader) = pair;

            Assert.AreEqual(properties.path, actualPath);
            Th095BestShotHeaderTests.Validate(actualHeader, properties.header);
        });
    }
}
