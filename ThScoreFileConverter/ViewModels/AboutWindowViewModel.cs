//-----------------------------------------------------------------------
// <copyright file="AboutWindowViewModel.cs" company="None">
//     (c) 2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.ViewModels
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Microsoft.Practices.Prism.Commands;
    using Microsoft.Practices.Prism.Mvvm;
    using Prop = ThScoreFileConverter.Properties;

    /// <summary>
    /// The view model class for <see cref="ThScoreFileConverter.Views.AboutWindow"/>.
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "For binding.")]
    internal class AboutWindowViewModel : BindableBase
    {
        /// <summary>
        /// The command which opens the specified URI.
        /// </summary>
        private DelegateCommand<object> openUriCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutWindowViewModel"/> class.
        /// </summary>
        public AboutWindowViewModel()
        {
            this.Title = "About this tool";     // FIXME

            this.Icon = Imaging.CreateBitmapSourceFromHIcon(
                SystemIcons.Application.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            var thisAsm = Assembly.GetExecutingAssembly();
            var asmName = thisAsm.GetName();
            var attrs = thisAsm.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), true);
            var copyrightAttr = attrs[0] as AssemblyCopyrightAttribute;

            this.Name = asmName.Name;
            this.Version = Prop.Resources.strVersionPrefix + asmName.Version.ToString();
            this.Copyright = (copyrightAttr != null) ? copyrightAttr.Copyright : string.Empty;
            this.Uri = "http://www.colorless-sight.jp/";    // FIXME
        }

        /// <summary>
        /// Gets a title of the About window.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For bindings.")]
        public string Title { get; private set; }

        /// <summary>
        /// Gets the icon displaying on the About window.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For bindings.")]
        public ImageSource Icon { get; private set; }

        /// <summary>
        /// Gets a name of this assembly.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For bindings.")]
        public string Name { get; private set; }

        /// <summary>
        /// Gets a version string of this assembly.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For bindings.")]
        public string Version { get; private set; }

        /// <summary>
        /// Gets a copyright string of this assembly.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For bindings.")]
        public string Copyright { get; private set; }

        /// <summary>
        /// Gets a URI string.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For bindings.")]
        public string Uri { get; private set; }

        /// <summary>
        /// Gets a command which opens the specified URI.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For bindings.")]
        public DelegateCommand<object> OpenUriCommand
        {
            get
            {
                return this.openUriCommand ??
                    (this.openUriCommand = new DelegateCommand<object>(this.OpenUri));
            }
        }

        /// <summary>
        /// Opens the specified URI.
        /// </summary>
        /// <param name="uri">An URI to open.</param>
        private void OpenUri(object uri)
        {
            Process.Start(uri as string);
        }
    }
}
