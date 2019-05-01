using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Actions;

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
            CollectionAssert.AreEqual(filenames, result.FileNames.ToArray());

            filenames[0] = "bs02_1.dat";
            CollectionAssert.AreNotEqual(filenames, result.FileNames.ToArray());
        }
    }
}
