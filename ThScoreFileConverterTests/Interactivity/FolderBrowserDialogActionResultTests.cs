using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Actions;

namespace ThScoreFileConverterTests.Interactivity
{
    [TestClass]
    public class FolderBrowserDialogActionResultTests
    {
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(@"path\to\some\folder")]
        public void FolderBrowserDialogActionResultTest(string path)
        {
            var result = new FolderBrowserDialogActionResult(path);

            Assert.AreEqual(path, result.SelectedPath);
        }
    }
}
