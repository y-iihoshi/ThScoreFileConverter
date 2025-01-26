//-----------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Globalization;
using System.IO;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs;
using Reactive.Bindings;
using Reactive.Bindings.Schedulers;
using ThScoreFileConverter.Adapters;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Resources;
using ThScoreFileConverter.ViewModels;
using WPFLocalizeExtension.Engine;

namespace ThScoreFileConverter;

/// <summary>
/// Interaction logic for App.xaml.
/// </summary>
public sealed partial class App : Application, IDisposable
{
    private readonly Settings settings;
    private ServiceProvider? serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="App"/> class.
    /// </summary>
    public App()
    {
        this.settings = new Settings();
        this.serviceProvider = default;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        this.serviceProvider?.Dispose();
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
            this.settings.Load(StringResources.SettingFileName);
        }
        catch (InvalidDataException)
        {
            var backup = Path.ChangeExtension(StringResources.SettingFileName, StringResources.BackupFileExtension);
            File.Delete(backup);
            File.Move(StringResources.SettingFileName, backup);
            var message = StringHelper.Format(
                Utils.GetLocalizedValues<string>(nameof(StringResources.MessageSettingFileIsCorrupted)),
                StringResources.SettingFileName,
                backup);
            _ = MessageBox.Show(
                message,
                Utils.GetLocalizedValues<string>(nameof(StringResources.MessageTitleWarning)),
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
        this.settings.Save(StringResources.SettingFileName);

        this.serviceProvider?.Dispose();
    }
}
