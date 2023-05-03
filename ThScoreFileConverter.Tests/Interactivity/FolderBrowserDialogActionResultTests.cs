﻿using System;
using ThScoreFileConverter.Interactivity;

namespace ThScoreFileConverter.Tests.Interactivity;

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
        _ = Assert.ThrowsException<ArgumentNullException>(() => new FolderBrowserDialogActionResult(null!));
    }
}
