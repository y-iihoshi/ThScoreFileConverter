using System.Windows;
using ThScoreFileConverter.Commands;

namespace ThScoreFileConverter.Tests.Commands;

[TestClass]
public class CloseWindowCommandTests
{
    [TestMethod]
    public void InstanceTest()
    {
        var instance = CloseWindowCommand.Instance;
        _ = instance.ShouldNotBeNull();
    }

    [STATestMethod]
    public void CanExecuteTest()
    {
        var instance = CloseWindowCommand.Instance;
        var window = new Window();
        instance.CanExecute(window).ShouldBeTrue();
    }

    [TestMethod]
    public void CanExecuteTestNull()
    {
        var instance = CloseWindowCommand.Instance;
        instance.CanExecute(null).ShouldBeFalse();
    }

    [TestMethod]
    public void CanExecuteTestInvalid()
    {
        var instance = CloseWindowCommand.Instance;
        instance.CanExecute(5).ShouldBeFalse();
    }

    [STATestMethod]
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
        invoked.ShouldBeTrue();

        invoked = false;
        window.Closed -= OnClosed;
        instance.Execute(window);
        invoked.ShouldBeFalse();
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

    [STATestMethod]
    public void CanExecuteChangedTest()
    {
        var instance = CloseWindowCommand.Instance;
        instance.CanExecuteChanged += (sender, e) => Assert.Fail(TestUtils.Unreachable);

        instance.Execute(null);
        instance.Execute(new Window());
        instance.Execute(5);
    }
}
