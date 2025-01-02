using System;
using System.IO.Packaging;
using System.Threading;
using System.Windows;
using ThScoreFileConverter.Core.Resources;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Properties;
using ThScoreFileConverter.ViewModels;

namespace ThScoreFileConverter.Tests.ViewModels;

[TestClass]
public class AboutWindowViewModelTests
{
    // A workaround to enable accessing resources by pack URIs.
    // See: https://stackoverflow.com/questions/3710776
    [ClassInitialize]
    public static void Setup(TestContext _1)
    {
        // No URI scheme except "example" should be used here, I think.
        _ = PackUriHelper.Create(new Uri("example://0"));

        // An UI element must be created by an STA thread.
        var thread = new Thread(() => _ = new FrameworkElement());
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();
    }

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
#pragma warning disable MSTEST0032 // Assertion condition is always true
        Assert.IsNotNull(window.Icon);
#pragma warning restore MSTEST0032 // Assertion condition is always true
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
        StringAssert.StartsWith(
            window.Version,
            Utils.GetLocalizedValues<string>(nameof(Resources.VersionPrefix)),
            StringComparison.CurrentCulture);
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
        Assert.AreEqual(StringResources.ProjectUrl, window.Uri);
    }
}
