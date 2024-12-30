using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using ThScoreFileConverter.Interactivity;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Interactivity;

internal sealed class DerivedAction : CommonDialogAction
{
    protected override void Invoke(object parameter) { }
}

internal sealed class Site : ISite
{
    public IComponent Component => null!;

    public IContainer Container => null!;

    public bool DesignMode => default;

    public string? Name { get; set; } = string.Empty;

    public object GetService(Type serviceType)
    {
        return null!;
    }
}

[TestClass]
public class CommonDialogActionTests
{
    [TestMethod]
    public void OkCommandTest()
    {
        var action = new DerivedAction();
        Assert.IsNull(action.OkCommand);

        var command = ApplicationCommands.NotACommand;
        action.OkCommand = command;
        Assert.AreSame(command, action.OkCommand);
    }

    [TestMethod]
    public void CancelCommandTest()
    {
        var action = new DerivedAction();
        Assert.IsNull(action.CancelCommand);

        var command = ApplicationCommands.NotACommand;
        action.CancelCommand = command;
        Assert.AreSame(command, action.CancelCommand);
    }

    [SkipOrSTATestMethod]
    public void OwnerTest()
    {
        var action = new DerivedAction();
        Assert.IsNull(action.Owner);

        var window = new Window();
        action.Owner = window;
        Assert.AreSame(window, action.Owner);
    }

    [TestMethod]
    public void SiteTest()
    {
        var action = new DerivedAction();
        Assert.IsNull(action.Site);

        var site = new Site();
        action.Site = site;
        Assert.AreSame(site, action.Site);
    }

    [TestMethod]
    public void TagTest()
    {
        var action = new DerivedAction();
        Assert.IsNull(action.Tag);

        var tag = new object();
        action.Tag = tag;
        Assert.AreSame(tag, action.Tag);
    }
}
