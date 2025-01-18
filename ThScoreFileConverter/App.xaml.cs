//-----------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs;
using Reactive.Bindings;
using Reactive.Bindings.Schedulers;
using ThScoreFileConverter.Adapters;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.ViewModels;
using WPFLocalizeExtension.Engine;
using Prop = ThScoreFileConverter.Properties;

namespace ThScoreFileConverter;

/// <summary>
/// Interaction logic for App.xaml.
/// </summary>
public sealed partial class App : Application, IDisposable
{
    private readonly Settings settings;
    private bool disposed;
    private ServiceProvider? serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="App"/> class.
    /// </summary>
    public App()
    {
        this.settings = new Settings();
        this.disposed = false;
        this.serviceProvider = default;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        this.Dispose(true);
    }

    /// <inheritdoc/>
    protected override void OnStartup(StartupEventArgs e)
    {
        this.serviceProvider?.Dispose();
        this.serviceProvider = new ServiceCollection()
            .AddSingleton<IDialogService, DialogService>()
            .AddSingleton(this.settings)
            .AddSingleton<ISettings>(this.settings)
            .AddTransient<INumberFormatter, NumberFormatter>()
            .AddTransient<IDispatcherAdapter, DispatcherAdapter>()
            .AddTransient<MainWindowViewModel>()
            .BuildServiceProvider();

        Ioc.Default.ConfigureServices(this.serviceProvider);

        try
        {
            this.settings.Load(Prop.Resources.SettingFileName);
        }
        catch (InvalidDataException)
        {
            var backup = Path.ChangeExtension(Prop.Resources.SettingFileName, Prop.Resources.BackupFileExtension);
            File.Delete(backup);
            File.Move(Prop.Resources.SettingFileName, backup);
            var message = StringHelper.Format(
                Utils.GetLocalizedValues<string>(nameof(Prop.Resources.MessageSettingFileIsCorrupted)),
                Prop.Resources.SettingFileName,
                backup);
            _ = MessageBox.Show(
                message,
                Utils.GetLocalizedValues<string>(nameof(Prop.Resources.MessageTitleWarning)),
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }

        var provider = LocalizationProvider.Instance;
        LocalizeDictionary.Instance.SetCurrentValue(LocalizeDictionary.DefaultProviderProperty, provider);
        LocalizeDictionary.Instance.SetCurrentValue(LocalizeDictionary.IncludeInvariantCultureProperty, false);
        LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
#pragma warning disable IDE0045 // Use conditional expression for assignment
        if (provider.AvailableCultures.Any(culture => culture.Name == this.settings.Language))
            LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo(this.settings.Language!);
        else if (provider.AvailableCultures.Any(CultureInfo.CurrentCulture.Equals))
            LocalizeDictionary.Instance.Culture = CultureInfo.CurrentCulture;
        else
            LocalizeDictionary.Instance.Culture = provider.AvailableCultures.First();
#pragma warning restore IDE0045 // Use conditional expression for assignment

        ReactivePropertyScheduler.SetDefault(new ReactivePropertyWpfScheduler(this.Dispatcher));

        base.OnStartup(e);
    }

    /// <inheritdoc/>
    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);

        this.settings.Language = LocalizeDictionary.Instance.Culture.Name;
        this.settings.Save(Prop.Resources.SettingFileName);

        this.serviceProvider?.Dispose();
    }

    /// <summary>
    /// Disposes the resources of the current instance.
    /// </summary>
    /// <param name="disposing">
    /// <see langword="true"/> if calls from the <see cref="Dispose()"/> method; <see langword="false"/> for the finalizer.
    /// </param>
    private void Dispose(bool disposing)
    {
        if (this.disposed)
        {
            return;
        }

        if (disposing)
        {
            this.serviceProvider?.Dispose();
        }

        this.disposed = true;
    }
}
