using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th143BestShotPairTests
    {
        internal struct Properties
        {
            public string path;
            public Th143BestShotHeaderTests.Properties header;
        };

        internal static Properties GetValidProperties() => new Properties()
        {
            path = @"%AppData%\ShanghaiAlice\th143\savedata\sc10_04.dat",
            header = Th143BestShotHeaderTests.ValidProperties
        };

        internal static void Validate(in Th143BestShotPairWrapper pair, in Properties properties)
        {
            Assert.AreEqual(properties.path, pair.Path);
            Th143BestShotHeaderTests.Validate(pair.Header, properties.header);
        }

        [TestMethod]
        public void Th143BestShotPairTest() => TestUtils.Wrap(() =>
        {
            var properties = GetValidProperties();

            var header = Th143BestShotHeaderWrapper.Create(Th143BestShotHeaderTests.MakeByteArray(properties.header));
            var pair = new Th143BestShotPairWrapper(properties.path, header);

            Validate(pair, properties);
        });
    }
}
