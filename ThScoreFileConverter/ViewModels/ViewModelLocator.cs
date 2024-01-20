//-----------------------------------------------------------------------
// <copyright file="ViewModelLocator.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using CommunityToolkit.Mvvm.DependencyInjection;

namespace ThScoreFileConverter.ViewModels;

/// <summary>
/// Provides all view models in the application.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Instantiated as an application resource in App.xaml.")]
internal sealed class ViewModelLocator
{
    /// <summary>
    /// Gets the view model for the main window.
    /// </summary>
#pragma warning disable CA1822 // Mark members as static
    public MainWindowViewModel MainWindow => Ioc.Default.GetRequiredService<MainWindowViewModel>();
#pragma warning restore CA1822 // Mark members as static
}
