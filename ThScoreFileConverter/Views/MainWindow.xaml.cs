//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="None">
//     (c) 2013-2015 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Views
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using System.Windows.Threading;
    using ThScoreFileConverter.Models;
    using Prop = ThScoreFileConverter.Properties;
    using WinForms = System.Windows.Forms;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            try
            {
                var app = App.Current as App;
                if (app != null)
                    app.UpdateResources(Settings.Instance.FontFamilyName, Settings.Instance.FontSize);
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
                throw;
            }
        }

        /// <summary>
        /// Handles the closing event of this window.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void WndMain_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                Settings.Instance.FontFamilyName = App.Current.Resources["FontFamilyKey"].ToString();
                Settings.Instance.FontSize =
                    Convert.ToDouble(App.Current.Resources["FontSizeKey"], CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
                throw;
            }
        }

        #region Best shot directory

        /// <summary>
        /// Handles the <c>Click</c> routed event of the <see cref="btnBestShot"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void BtnBestShot_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var dialog = new WinForms.FolderBrowserDialog())
                {
                    dialog.Description = Prop.Resources.msgSelectBestShotDirectory;
                    if (Directory.Exists(this.txtBestShot.Text))
                        dialog.SelectedPath = this.txtBestShot.Text;

                    var result = dialog.ShowDialog(new Win32Window(this));
                    if (result == WinForms.DialogResult.OK)
                        this.txtBestShot.Text = dialog.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
                throw;
            }
        }

        #endregion

        #region Output directory

        /// <summary>
        /// Handles the <c>Click</c> routed event of the <see cref="btnOutput"/> member.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void BtnOutput_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var dialog = new WinForms.FolderBrowserDialog())
                {
                    dialog.Description = Prop.Resources.msgSelectOutputDirectory;
                    if (Directory.Exists(this.txtOutput.Text))
                        dialog.SelectedPath = this.txtOutput.Text;

                    var result = dialog.ShowDialog(new Win32Window(this));
                    if (result == WinForms.DialogResult.OK)
                        this.txtOutput.Text = dialog.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                this.ShowExceptionMessage(ex);
                throw;
            }
        }

        #endregion

        #region Utility

        /// <summary>
        /// Shows a message that represents the occurred exception.
        /// </summary>
        /// <param name="e">The occurred exception.</param>
        private void ShowExceptionMessage(Exception e)
        {
            var dispatcher = this.Dispatcher;
            if (dispatcher.CheckAccess())
#if DEBUG
                MessageBox.Show(
                    e.ToString(), Prop.Resources.msgTitleError, MessageBoxButton.OK, MessageBoxImage.Error);
#else
                MessageBox.Show(
                    e.Message, Prop.Resources.msgTitleError, MessageBoxButton.OK, MessageBoxImage.Error);
#endif
            else
                dispatcher.Invoke(
                    DispatcherPriority.Normal, new Action<Exception>(this.ShowExceptionMessage), e);
        }

        #endregion
    }
}
