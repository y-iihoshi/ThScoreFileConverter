//-----------------------------------------------------------------------
// <copyright file="AboutWindow.xaml.cs" company="None">
//     (c) 2013-2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter
{
    using System.Diagnostics;
    using System.Drawing;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Interop;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using Prop = ThScoreFileConverter.Properties;

    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutWindow"/> class.
        /// </summary>
        /// <param name="owner">The owner window of the new instance.</param>
        public AboutWindow(Window owner)
            : this()
        {
            this.Owner = owner;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="AboutWindow"/> class from being created.
        /// </summary>
        private AboutWindow()
        {
            this.InitializeComponent();

            this.imgIcon.Source = Imaging.CreateBitmapSourceFromHIcon(
                SystemIcons.Application.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            var thisAsm = Assembly.GetExecutingAssembly();
            var asmName = thisAsm.GetName();
            var attrs = thisAsm.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), true);
            var copyrightAttr = attrs[0] as AssemblyCopyrightAttribute;

            this.txtName.Text = asmName.Name;
            this.txtVersion.Text = Prop.Resources.strVersionPrefix + asmName.Version.ToString();
            if (copyrightAttr != null)
                this.txtCopyright.Text = copyrightAttr.Copyright;
        }

        /// <summary>
        /// Handles the <c>RequestNavigate</c> routed event of the <see cref="Hyperlink"/> class.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.ToString());
        }

        /// <summary>
        /// Handles the <c>PreviewKeyDown</c> routed event of the current window.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }
    }
}
