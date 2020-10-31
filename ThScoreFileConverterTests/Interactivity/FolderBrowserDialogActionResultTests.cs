using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Interactivity;
using ThScoreFileConverterTests.Models;

namespace ThScoreFileConverterTests.Interactivity
{
    [TestClass]
    public class FolderBrowserDialogActionResultTests
    {
        [DataTestMethod]
        [DataRow("")]
        [DataRow(@"path\to\some\folder")]
        public void FolderBrowserDialogActionResultTest(string path)
        {
            var result = new FolderBrowserDialogActionResult(path);

            Assert.AreEqual(path, result.SelectedPath);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FolderBrowserDialogActionResultTestNull()
        {
            _ = new FolderBrowserDialogActionResult(null!);

            Assert.Fail(TestUtils.Unreachable);
        }
    }
}
