using System;
using System.Drawing;
using ThScoreFileConverter.Interactivity;

namespace ThScoreFileConverter.Tests.Interactivity;

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
        _ = Assert.ThrowsException<ArgumentNullException>(() => new FontDialogActionResult(null!, Color.Black));
    }
}
