using System.IO.Packaging;
using System.Windows;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Resources;
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
        window.Title.ShouldBe(Utils.GetLocalizedValues<string>(nameof(StringResources.AboutWindowTitle)));
    }

    [TestMethod]
    public void IconTest()
    {
        var window = new AboutWindowViewModel();
        _ = window.Icon.ShouldNotBeNull();
    }

    [TestMethod]
    public void NameTest()
    {
        var window = new AboutWindowViewModel();
        window.Name.ShouldBe(nameof(ThScoreFileConverter));
    }

    [TestMethod]
    public void VersionTest()
    {
        var window = new AboutWindowViewModel();
        window.Version.ShouldStartWith(Utils.GetLocalizedValues<string>(nameof(StringResources.VersionPrefix)), Case.Sensitive);
    }

    [TestMethod]
    public void CopyrightTest()
    {
        var window = new AboutWindowViewModel();
        window.Copyright.ShouldNotBeNullOrEmpty();
    }

    [TestMethod]
    public void UriTest()
    {
        var window = new AboutWindowViewModel();
        window.Uri.ShouldBe(Core.Resources.StringResources.ProjectUrl);
    }
}
