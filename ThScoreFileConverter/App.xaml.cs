//-----------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media;
using Prism.Ioc;
using Prism.Unity;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.ViewModels;
using ThScoreFileConverter.Views;
using Prop = ThScoreFileConverter.Properties;

namespace ThScoreFileConverter
{
    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : PrismApplication
    {
        /// <summary>
        /// Updates the resources of this application.
        /// </summary>
        /// <param name="fontFamily">The font family used for the UI of this application.</param>
        /// <param name="fontSize">The font size used for the UI of this application.</param>
        public void UpdateResources(FontFamily fontFamily, double? fontSize)
        {
            if (fontFamily != null)
                this.Resources["FontFamilyKey"] = fontFamily;
            if (fontSize.HasValue)
                this.Resources["FontSizeKey"] = fontSize.Value;
        }

        /// <summary>
        /// Updates the resources of this application.
        /// </summary>
        /// <param name="fontFamilyName">The font family name used for the UI of this application.</param>
        /// <param name="fontSize">The font size used for the UI of this application.</param>
        public void UpdateResources(string fontFamilyName, double? fontSize)
        {
            this.UpdateResources(new FontFamily(fontFamilyName), fontSize);
        }

        /// <inheritdoc/>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<AboutWindow>(nameof(AboutWindowViewModel));
            containerRegistry.RegisterDialog<SettingWindow>(nameof(SettingWindowViewModel));
        }

        /// <inheritdoc/>
        protected override Window CreateShell() => this.Container.Resolve<MainWindow>();

        /// <summary>
        /// Handles the <c>Startup</c> event of this application.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void App_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                Settings.Instance.Load(Prop.Resources.strSettingFile);
            }
            catch (InvalidDataException)
            {
                var backup = Path.ChangeExtension(
                    Prop.Resources.strSettingFile, Prop.Resources.strBackupFileExtension);
                File.Delete(backup);
                File.Move(Prop.Resources.strSettingFile, backup);
                var message = Utils.Format(
                    Prop.Resources.msgFmtBrokenSettingFile, Prop.Resources.strSettingFile, backup);
                MessageBox.Show(
                    message, Prop.Resources.msgTitleWarning, MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            this.UpdateResources(Settings.Instance.FontFamilyName, Settings.Instance.FontSize);
        }

        /// <summary>
        /// Handles the <c>Exit</c> event of this application.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void App_Exit(object sender, ExitEventArgs e)
        {
            Settings.Instance.FontFamilyName = this.Resources["FontFamilyKey"].ToString();
            Settings.Instance.FontSize =
                Convert.ToDouble(this.Resources["FontSizeKey"], CultureInfo.InvariantCulture);

            Settings.Instance.Save(Prop.Resources.strSettingFile);
        }
    }
}
