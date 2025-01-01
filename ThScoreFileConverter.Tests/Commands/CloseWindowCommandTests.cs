using System;
using System.Windows;
using ThScoreFileConverter.Commands;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Commands;

[TestClass]
public class CloseWindowCommandTests
{
    [TestMethod]
    public void InstanceTest()
    {
        var instance = CloseWindowCommand.Instance;
        Assert.IsNotNull(instance);
    }

    [SkipOrSTATestMethod]
    public void CanExecuteTest()
    {
        var instance = CloseWindowCommand.Instance;
        var window = new Window();
        Assert.IsTrue(instance.CanExecute(window));
    }

    [TestMethod]
    public void CanExecuteTestNull()
    {
        var instance = CloseWindowCommand.Instance;
        Assert.IsFalse(instance.CanExecute(null));
    }

    [TestMethod]
    public void CanExecuteTestInvalid()
    {
        var instance = CloseWindowCommand.Instance;
        Assert.IsFalse(instance.CanExecute(5));
    }

    [SkipOrSTATestMethod]
    public void ExecuteTest()
    {
        var instance = CloseWindowCommand.Instance;
        var window = new Window();
        var invoked = false;

        void OnClosed(object? sender, EventArgs e)
        {
            invoked = true;
        }

        window.Closed += OnClosed;
        instance.Execute(window);
        Assert.IsTrue(invoked);

        invoked = false;
        window.Closed -= OnClosed;
        instance.Execute(window);
        Assert.IsFalse(invoked);
    }

    [TestMethod]
    public void ExecuteTestNull()
    {
        var instance = CloseWindowCommand.Instance;
        instance.Execute(null);
    }

    [TestMethod]
    public void ExecuteTestInvalid()
    {
        var instance = CloseWindowCommand.Instance;
        instance.Execute(5);
    }

    [SkipOrSTATestMethod]
    public void CanExecuteChangedTest()
    {
        var instance = CloseWindowCommand.Instance;
        instance.CanExecuteChanged += (sender, e) => Assert.Fail(TestUtils.Unreachable);

        instance.Execute(null);
        instance.Execute(new Window());
        instance.Execute(5);
    }
}
