//-----------------------------------------------------------------------
// <copyright file="SettingWindow.xaml.cs" company="None">
//     (c) 2013-2015 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Views
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingWindow"/> class.
        /// </summary>
        /// <param name="owner">The instance of the owner window.</param>
        public SettingWindow(Window owner)
            : this()
        {
            this.Owner = owner;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="SettingWindow"/> class from being created.
        /// </summary>
        private SettingWindow()
        {
            this.InitializeComponent();
        }
    }
}
