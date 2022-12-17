using System;
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
        Assert.IsNotNull(adapter);
    }

    [TestMethod]
    public void DispatcherAdapterTestNull()
    {
        _ = Assert.ThrowsException<NullReferenceException>(() => new DispatcherAdapter());
    }

    [TestMethod]
    public void WithAppDispatcherAdapterTestNull()
    {
        var app = SetupApp();
        try
        {
            var adapter = new DispatcherAdapter();
            Assert.IsNotNull(adapter);
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
        Assert.IsNotNull(adapter);

        var numInvoked = 0;
        adapter.Invoke(() =>
        {
            ++numInvoked;
            Assert.AreSame(dispatcher, Dispatcher.CurrentDispatcher);
        });

        Assert.AreEqual(1, numInvoked);
    }
}
