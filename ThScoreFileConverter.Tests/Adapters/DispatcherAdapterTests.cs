using System.Windows;
using System.Windows.Threading;
using ThScoreFileConverter.Adapters;

namespace ThScoreFileConverter.Tests.Adapters;

[TestClass]
public class DispatcherAdapterTests
{
    private static App SetupApp()
    {
        if (Application.Current is not App app)
        {
            app = new App() { ShutdownMode = ShutdownMode.OnExplicitShutdown };
            app.InitializeComponent();
        }

        return app;
    }

    [TestMethod]
    public void DispatcherAdapter()
    {
        var adapter = new DispatcherAdapter(Dispatcher.CurrentDispatcher);
        _ = adapter.ShouldNotBeNull();
    }

    [TestMethod]
    public void DispatcherAdapterTestNull()
    {
        _ = Should.Throw<NullReferenceException>(() => new DispatcherAdapter());
    }

    [TestMethod]
    public void WithAppDispatcherAdapterTestNull()
    {
        using var app = SetupApp();
        try
        {
            var adapter = new DispatcherAdapter();
            _ = adapter.ShouldNotBeNull();
        }
        finally
        {
            app.Shutdown();
        }
    }

    [TestMethod]
    public void InvokeTest()
    {
        var dispatcher = Dispatcher.CurrentDispatcher;
        var adapter = new DispatcherAdapter(dispatcher);
        _ = adapter.ShouldNotBeNull();

        var numInvoked = 0;
        adapter.Invoke(() =>
        {
            ++numInvoked;
            Dispatcher.CurrentDispatcher.ShouldBeSameAs(dispatcher);
        });

        numInvoked.ShouldBe(1);
    }
}
