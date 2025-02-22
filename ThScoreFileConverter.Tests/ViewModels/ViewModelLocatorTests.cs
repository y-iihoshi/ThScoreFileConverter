﻿using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs;
using NSubstitute;
using ThScoreFileConverter.Adapters;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.ViewModels;

namespace ThScoreFileConverter.Tests.ViewModels;

[TestClass]
public class ViewModelLocatorTests
{
    [TestMethod]
    public void MainWindowTest()
    {
        using var provider = new ServiceCollection()
            .AddSingleton(Substitute.For<IDialogService>())
            .AddSingleton(Substitute.For<IDispatcherAdapter>())
            .AddSingleton(new Settings())
            .AddSingleton(Substitute.For<INumberFormatter>())
            .AddTransient<MainWindowViewModel>()
            .BuildServiceProvider();

        Ioc.Default.ConfigureServices(provider);

        using var window = new ViewModelLocator().MainWindow;
        _ = window.ShouldNotBeNull();

        // NOTE: Ioc.Default can't be configured twice.
    }
}
