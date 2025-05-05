using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using ThScoreFileConverter.Models;
using Th095BestShotHeader = ThScoreFileConverter.Models.Th095.BestShotHeader;
using Th125BestShotHeader = ThScoreFileConverter.Models.Th125.BestShotHeader;

namespace ThScoreFileConverter.Tests.Models;

internal static class BitmapExtensions
{
    internal static void ShouldBe(this Bitmap actual, Bitmap expected)
    {
        actual.Width.ShouldBe(expected.Width);
        actual.Height.ShouldBe(expected.Height);
        actual.PixelFormat.ShouldBe(expected.PixelFormat);

        var expectedData = expected.LockBits(
            new Rectangle(0, 0, expected.Width, expected.Height), ImageLockMode.ReadOnly, expected.PixelFormat);
        var actualData = actual.LockBits(
            new Rectangle(0, 0, actual.Width, actual.Height), ImageLockMode.ReadOnly, actual.PixelFormat);
        try
        {
            var expectedBytes = new byte[expectedData.Stride * expectedData.Height];
            var actualBytes = new byte[actualData.Stride * actualData.Height];
            Marshal.Copy(expectedData.Scan0, expectedBytes, 0, expectedBytes.Length);
            Marshal.Copy(actualData.Scan0, actualBytes, 0, actualBytes.Length);
            actualBytes.ShouldBe(expectedBytes);
        }
        finally
        {
            expected.UnlockBits(expectedData);
            actual.UnlockBits(actualData);
        }
    }
}

[TestClass]
public class BestShotDeveloperTests
{
    [TestMethod]
    public void DevelopTestUnsupportedFormat()
    {
        using var input = new MemoryStream();
        using var output = new MemoryStream();
        _ = Should.Throw<NotImplementedException>(
            () => BestShotDeveloper.Develop<Th095BestShotHeader>(input, output, PixelFormat.Format16bppArgb1555));
    }

    [TestMethod]
    public void DevelopTestTh095()
    {
        using var inputFile = new FileStream(@"TestData\th095_bs_09_6.dat", FileMode.Open, FileAccess.Read);
        using var outputFile = new FileStream(@"TestData\th095_bs_09_6.png", FileMode.Open, FileAccess.Read);
        using var expectedBitmap = new Bitmap(outputFile);

        using var actualOutput = new MemoryStream();
        var header = BestShotDeveloper.Develop<Th095BestShotHeader>(inputFile, actualOutput, PixelFormat.Format24bppRgb);

        using var actualBitmap = new Bitmap(actualOutput);
        actualBitmap.ShouldBe(expectedBitmap);

        header.Width.ShouldBe((short)expectedBitmap.Width);
        header.Height.ShouldBe((short)expectedBitmap.Height);
    }

    [TestMethod]
    public void DevelopTestTh125()
    {
        using var inputFile = new FileStream(@"TestData\th125_bs_09_7.dat", FileMode.Open, FileAccess.Read);
        using var outputFile = new FileStream(@"TestData\th125_bs_09_7.png", FileMode.Open, FileAccess.Read);
        using var expectedBitmap = new Bitmap(outputFile);

        using var actualOutput = new MemoryStream();
        var header = BestShotDeveloper.Develop<Th125BestShotHeader>(inputFile, actualOutput, PixelFormat.Format32bppArgb);

        using var actualBitmap = new Bitmap(actualOutput);
        actualBitmap.ShouldBe(expectedBitmap);

        header.Width.ShouldBe((short)expectedBitmap.Width);
        header.Height.ShouldBe((short)expectedBitmap.Height);
    }
}
