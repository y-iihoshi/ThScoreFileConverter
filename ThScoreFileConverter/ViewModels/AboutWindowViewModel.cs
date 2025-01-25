//-----------------------------------------------------------------------
// <copyright file="AboutWindowViewModel.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using MvvmDialogs;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Resources;

namespace ThScoreFileConverter.ViewModels;

/// <summary>
/// The view model class for <see cref="Views.AboutWindow"/>.
/// </summary>
#if !DEBUG
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Instantiated by the DI container.")]
#endif
internal sealed class AboutWindowViewModel : ObservableObject, IModalDialogViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AboutWindowViewModel"/> class.
    /// </summary>
    public AboutWindowViewModel()
    {
        this.Title = Utils.GetLocalizedValues<string>(nameof(StringResources.AboutWindowTitle));

        var thisAsm = Assembly.GetExecutingAssembly();
        var asmName = thisAsm.GetName();
        var gitVerInfoType = thisAsm.GetType("GitVersionInformation");
        var verField = gitVerInfoType?.GetField("MajorMinorPatch");
        var attrs = thisAsm.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), true);

        this.Name = asmName.Name ?? nameof(ThScoreFileConverter);
        this.Version = Utils.GetLocalizedValues<string>(nameof(StringResources.VersionPrefix))
            + (verField?.GetValue(null) ?? string.Empty);
        this.Copyright = (attrs[0] is AssemblyCopyrightAttribute attr) ? attr.Copyright : string.Empty;
        this.Uri = Core.Resources.StringResources.ProjectUrl;

        var uriString = $"pack://application:,,,/{this.Name};component/Resources/ApplicationIcon.ico";
        var decoder = BitmapDecoder.Create(new Uri(uriString), BitmapCreateOptions.DelayCreation, BitmapCacheOption.OnDemand);

        this.Icon = decoder.Frames.OrderByDescending(frame => frame.Width).First();
    }

    /// <summary>
    /// Gets a title of the About window.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets the icon displaying on the About window.
    /// </summary>
    public ImageSource Icon { get; }

    /// <summary>
    /// Gets a name of this assembly.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets a version string of this assembly.
    /// </summary>
    public string Version { get; }

    /// <summary>
    /// Gets a copyright string of this assembly.
    /// </summary>
    public string Copyright { get; }

    /// <summary>
    /// Gets a URI string.
    /// </summary>
    public string Uri { get; }

    /// <inheritdoc/>
    public bool? DialogResult { get; } = true;
}
