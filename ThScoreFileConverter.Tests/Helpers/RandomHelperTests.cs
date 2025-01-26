using ThScoreFileConverter.Helpers;

namespace ThScoreFileConverter.Tests.Helpers;

[TestClass]
public class RandomHelperTests
{
    [TestMethod]
    public void ParkMillerRNGTest()
    {
        // Tests whether the RNG complies with the required behavior of C++11 minstd_rand.
        // https://timsong-cpp.github.io/cppwp/n3337/rand.predef

        var state = 1;
        for (var i = 0; i < 10000; ++i)
        {
            state = RandomHelper.ParkMillerRNG(state);
        }

        state.ShouldBe(399268537);
    }
}
