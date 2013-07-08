using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace ThScoreFileConverter
{
    /// <summary>
    /// AboutWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class AboutWindow : Window
    {
        private AboutWindow()
        {
            InitializeComponent();

            this.imgIcon.Source = Imaging.CreateBitmapSourceFromHIcon(
                SystemIcons.Application.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            var thisAsm = Assembly.GetExecutingAssembly();
            var asmName = thisAsm.GetName();
            var attrs = thisAsm.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), true);
            this.txtName.Text = asmName.Name;
            this.txtVersion.Text = Properties.Resources.strVersionPrefix + asmName.Version.ToString();
            this.txtCopyright.Text = ((AssemblyCopyrightAttribute)attrs[0]).Copyright;
        }

        public AboutWindow(Window owner)
            : this()
        {
            this.Owner = owner;
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(((Hyperlink)sender).NavigateUri.ToString());
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }
    }
}
