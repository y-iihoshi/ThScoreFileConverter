using System.Windows;
using System.Windows.Media;
using SysDraw = System.Drawing;

namespace ThScoreFileConverter
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        public void UpdateResources(FontFamily fontFamily, double? fontSize)
        {
            this.Resources["FontFamilyKey"] = fontFamily ?? SystemFonts.MessageFontFamily;
            this.Resources["FontSizeKey"] = fontSize ?? SystemFonts.MessageFontSize;
            this.Resources["FontInfoKey"] =
                this.Resources["FontFamilyKey"] + ", " + this.Resources["FontSizeKey"];
        }

        public void UpdateResources(string fontFamilyName, double? fontSize)
        {
            this.UpdateResources(new FontFamily(fontFamilyName), fontSize);
        }

        public void UpdateResources(SysDraw.Font font)
        {
            this.UpdateResources(new FontFamily(font.FontFamily.Name), font.Size);
        }
    }
}
