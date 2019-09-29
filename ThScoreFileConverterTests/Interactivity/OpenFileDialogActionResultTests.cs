using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Actions;
using ThScoreFileConverterTests.Extensions;

namespace ThScoreFileConverterTests.Interactivity
{
    [TestClass]
    public class OpenFileDialogActionResultTests
    {
        [TestMethod]
        public void OpenFileDialogActionResultTest()
        {
            var filename = "score.dat";
            var filenames = new string[] { "bs01_1.dat", "bs01_2.dat" };
            var result = new OpenFileDialogActionResult(filename, filenames);

            Assert.AreEqual(filename, result.FileName);
            CollectionAssert.That.AreEqual(filenames, result.FileNames);

            filenames[0] = "bs02_1.dat";
            CollectionAssert.That.AreNotEqual(filenames, result.FileNames);
        }
    }
}
