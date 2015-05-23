//-----------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="None">
//     (c) 2013-2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using ThScoreFileConverter.Models;
    using Prop = ThScoreFileConverter.Properties;
    using SysDraw = System.Drawing;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
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
            this.Resources["FontInfoKey"] =
                this.Resources["FontFamilyKey"] + ", " + this.Resources["FontSizeKey"];
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

        /// <summary>
        /// Updates the resources of this application.
        /// </summary>
        /// <param name="font">
        /// The value of the <see cref="System.Drawing.Font"/> type used for the UI of this application.
        /// </param>
        public void UpdateResources(SysDraw.Font font)
        {
            if (font == null)
                throw new ArgumentNullException("font");
            this.UpdateResources(new FontFamily(font.FontFamily.Name), font.Size);
        }

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
        }

        /// <summary>
        /// Handles the <c>Exit</c> event of this application.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void App_Exit(object sender, ExitEventArgs e)
        {
            Settings.Instance.Save(Prop.Resources.strSettingFile);
        }
    }
}
