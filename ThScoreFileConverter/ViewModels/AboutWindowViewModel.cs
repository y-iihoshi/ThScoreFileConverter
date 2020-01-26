//-----------------------------------------------------------------------
// <copyright file="AboutWindowViewModel.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using ThScoreFileConverter.Properties;
using Prop = ThScoreFileConverter.Properties;

namespace ThScoreFileConverter.ViewModels
{
    /// <summary>
    /// The view model class for <see cref="Views.AboutWindow"/>.
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Instantiated via ViewModelLocator.")]
    internal class AboutWindowViewModel : BindableBase, IDialogAware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutWindowViewModel"/> class.
        /// </summary>
        public AboutWindowViewModel()
        {
            this.Title = Resources.AboutWindowTitle;

            this.Icon = Imaging.CreateBitmapSourceFromHIcon(
                SystemIcons.Application.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            var thisAsm = Assembly.GetExecutingAssembly();
            var asmName = thisAsm.GetName();
            var gitVerInfoType = thisAsm.GetType("GitVersionInformation");
            var verField = gitVerInfoType.GetField("MajorMinorPatch");
            var attrs = thisAsm.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), true);

            this.Name = asmName.Name;
            this.Version = Prop.Resources.strVersionPrefix + verField.GetValue(null);
            this.Copyright = (attrs[0] is AssemblyCopyrightAttribute attr) ? attr.Copyright : string.Empty;
            this.Uri = Resources.ProjectUrl;
        }

        /// <inheritdoc/>
#pragma warning disable CS0067
        public event Action<IDialogResult>? RequestClose;
#pragma warning restore CS0067

        /// <summary>
        /// Gets a title of the About window.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Gets the icon displaying on the About window.
        /// </summary>
        public ImageSource Icon { get; private set; }

        /// <summary>
        /// Gets a name of this assembly.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets a version string of this assembly.
        /// </summary>
        public string Version { get; private set; }

        /// <summary>
        /// Gets a copyright string of this assembly.
        /// </summary>
        public string Copyright { get; private set; }

        /// <summary>
        /// Gets a URI string.
        /// </summary>
        public string Uri { get; private set; }

        /// <summary>
        /// Gets a command which opens the specified URI.
        /// </summary>
        public DelegateCommand<object> OpenUriCommand { get; } = new DelegateCommand<object>(OpenUri);

        /// <inheritdoc/>
        public bool CanCloseDialog()
        {
            return true;
        }

        /// <inheritdoc/>
        public void OnDialogClosed()
        {
        }

        /// <inheritdoc/>
        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        /// <summary>
        /// Opens the specified URI.
        /// </summary>
        /// <param name="uri">A URI to open.</param>
        private static void OpenUri(object uri)
        {
            var info = new ProcessStartInfo
            {
                FileName = uri as string,
                UseShellExecute = true,
            };

            using var process = Process.Start(info);
        }
    }
}
