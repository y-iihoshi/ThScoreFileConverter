using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Services.Dialogs;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Properties;
using ThScoreFileConverter.ViewModels;

namespace ThScoreFileConverterTests.ViewModels
{
    [TestClass]
    public class AboutWindowViewModelTests
    {
        [TestMethod]
        public void TitleTest()
        {
            var window = new AboutWindowViewModel();
            Assert.AreEqual(Utils.GetLocalizedValues<string>(nameof(Resources.AboutWindowTitle)), window.Title);
        }

        [TestMethod]
        public void IconTest()
        {
            var window = new AboutWindowViewModel();
            Assert.IsNotNull(window.Icon);
        }

        [TestMethod]
        public void NameTest()
        {
            var window = new AboutWindowViewModel();
            Assert.AreEqual(nameof(ThScoreFileConverter), window.Name);
        }

        [TestMethod]
        public void VersionTest()
        {
            var window = new AboutWindowViewModel();
            StringAssert.StartsWith(window.Version, Utils.GetLocalizedValues<string>(nameof(Resources.VersionPrefix)));
        }

        [TestMethod]
        public void CopyrightTest()
        {
            var window = new AboutWindowViewModel();
            Assert.IsFalse(string.IsNullOrEmpty(window.Copyright));
        }

        [TestMethod]
        public void UriTest()
        {
            var window = new AboutWindowViewModel();
            Assert.AreEqual(Resources.ProjectUrl, window.Uri);
        }

        [TestMethod]
        public void CanCloseDialogTest()
        {
            var window = new AboutWindowViewModel();
            Assert.IsTrue(window.CanCloseDialog());
        }

        [TestMethod]
        public void OnDialogClosedTest()
        {
            var window = new AboutWindowViewModel();
            window.OnDialogClosed();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void OnDialogOpenedTest()
        {
            var parameters = new DialogParameters();
            var window = new AboutWindowViewModel();
            window.OnDialogOpened(parameters);
            Assert.IsTrue(true);
        }
    }
}
