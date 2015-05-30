//-----------------------------------------------------------------------
// <copyright file="AboutWindow.xaml.cs" company="None">
//     (c) 2013-2015 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Views
{
    using System.Windows;

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
    }
}
