using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Actions;

namespace ThScoreFileConverterTests.Actions
{
    [TestClass]
    public class FolderBrowserDialogActionResultTests
    {
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
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
