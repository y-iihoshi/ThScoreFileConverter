//-----------------------------------------------------------------------
// <copyright file="AboutWindow.xaml.cs" company="None">
//     (c) 2013-2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Views
{
    using System.Windows;
    using System.Windows.Input;

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
