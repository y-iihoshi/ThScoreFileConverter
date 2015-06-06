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
    using System.Windows;
    using System.Windows.Threading;
    using Prop = ThScoreFileConverter.Properties;

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
