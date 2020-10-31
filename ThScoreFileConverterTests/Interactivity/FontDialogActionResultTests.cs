using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Interactivity;
using ThScoreFileConverterTests.Models;

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
        [ExpectedException(typeof(ArgumentNullException))]
        public void FontDialogActionResultTestNullFont()
        {
            _ = new FontDialogActionResult(null!, Color.Black);

            Assert.Fail(TestUtils.Unreachable);
        }
    }
}
