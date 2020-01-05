using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Actions;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models;

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

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OpenFileDialogActionResultTestNullFilename()
        {
            var filenames = new string[] { "bs01_1.dat", "bs01_2.dat" };
            _ = new OpenFileDialogActionResult(null!, filenames);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OpenFileDialogActionResultTestNullFilenames()
        {
            var filename = "score.dat";
            _ = new OpenFileDialogActionResult(filename, null!);

            Assert.Fail(TestUtils.Unreachable);
        }
    }
}
