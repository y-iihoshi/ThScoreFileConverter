//-----------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="None">
//     (c) 2013-2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter
{
    using System.Windows;
    using System.Windows.Media;
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
            this.Resources["FontFamilyKey"] = fontFamily ?? SystemFonts.MessageFontFamily;
            this.Resources["FontSizeKey"] = fontSize ?? SystemFonts.MessageFontSize;
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
            this.UpdateResources(new FontFamily(font.FontFamily.Name), font.Size);
        }
    }
}
