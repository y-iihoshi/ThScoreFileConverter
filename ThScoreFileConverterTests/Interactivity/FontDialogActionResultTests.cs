using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Actions;

namespace ThScoreFileConverterTests.Interactivity
{
    [TestClass]
    public class FontDialogActionResultTests
    {
        [TestMethod]
        public void FontDialogActionResultTest()
        {
            var font = SystemFonts.DefaultFont;
            var color = Color.Black;
            var result = new FontDialogActionResult(font, color);

            Assert.AreEqual(font, result.Font);
            Assert.AreEqual(color, result.Color);
        }

        [TestMethod]
        public void FontDialogActionResultTestNullFont()
        {
            var result = new FontDialogActionResult(null, Color.Black);

            Assert.IsNull(result.Font);
        }
    }
}
