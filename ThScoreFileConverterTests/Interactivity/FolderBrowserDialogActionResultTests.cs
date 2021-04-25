using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Interactivity;

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
        public void FolderBrowserDialogActionResultTestNull()
        {
            _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new FolderBrowserDialogActionResult(null!));
        }
    }
}
