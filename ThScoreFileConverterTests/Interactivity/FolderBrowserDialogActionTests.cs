﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Interactivity;

namespace ThScoreFileConverterTests.Interactivity
{
    [TestClass]
    public class FolderBrowserDialogActionTests
    {
        [TestMethod]
        public void DescriptionTest()
        {
            var action = new FolderBrowserDialogAction();
            Assert.AreEqual(string.Empty, action.Description);

            var description = "description";
            action.Description = description;
            Assert.AreEqual(description, action.Description);
        }

        [TestMethod]
        public void RootFolderTest()
        {
            var action = new FolderBrowserDialogAction();
            Assert.AreEqual(Environment.SpecialFolder.Desktop, action.RootFolder);

            var folder = Environment.SpecialFolder.MyDocuments;
            action.RootFolder = folder;
            Assert.AreEqual(folder, action.RootFolder);
        }

        [TestMethod]
        public void SelectedPathTest()
        {
            var action = new FolderBrowserDialogAction();
            Assert.AreEqual(string.Empty, action.SelectedPath);

            var path = Environment.CurrentDirectory;
            action.SelectedPath = path;
            Assert.AreEqual(path, action.SelectedPath);
        }

        [TestMethod]
        public void ShowNewFolderButtonTest()
        {
            var action = new FolderBrowserDialogAction();
            Assert.IsTrue(action.ShowNewFolderButton);

            action.ShowNewFolderButton = false;
            Assert.IsFalse(action.ShowNewFolderButton);
        }
    }
}
