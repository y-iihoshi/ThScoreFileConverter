using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Interactivity;
using SysDraw = System.Drawing;

namespace ThScoreFileConverterTests.Interactivity
{
    [TestClass]
    public class FontDialogActionTests
    {
        [TestMethod]
        public void ApplyCommandTest()
        {
            var action = new FontDialogAction();
            Assert.IsNull(action.ApplyCommand);

            var command = ApplicationCommands.NotACommand;
            action.ApplyCommand = command;
            Assert.AreSame(command, action.ApplyCommand);
        }

        [TestMethod]
        public void AllowScriptChangeTest()
        {
            var action = new FontDialogAction();
            Assert.IsTrue(action.AllowScriptChange);

            action.AllowScriptChange = false;
            Assert.IsFalse(action.AllowScriptChange);
        }

        [TestMethod]
        public void AllowSimulationsTest()
        {
            var action = new FontDialogAction();
            Assert.IsTrue(action.AllowSimulations);

            action.AllowSimulations = false;
            Assert.IsFalse(action.AllowSimulations);
        }

        [TestMethod]
        public void AllowVectorFontsTest()
        {
            var action = new FontDialogAction();
            Assert.IsTrue(action.AllowVectorFonts);

            action.AllowVectorFonts = false;
            Assert.IsFalse(action.AllowVectorFonts);
        }

        [TestMethod]
        public void AllowVerticalFontsTest()
        {
            var action = new FontDialogAction();
            Assert.IsTrue(action.AllowVerticalFonts);

            action.AllowVerticalFonts = false;
            Assert.IsFalse(action.AllowVerticalFonts);
        }

        [TestMethod]
        public void ColorTest()
        {
            var action = new FontDialogAction();
            Assert.AreEqual(SysDraw.Color.Black, action.Color);

            var color = SysDraw.Color.White;
            action.Color = color;
            Assert.AreEqual(color, action.Color);
        }

        [TestMethod]
        public void FixedPitchOnlyTest()
        {
            var action = new FontDialogAction();
            Assert.IsFalse(action.FixedPitchOnly);

            action.FixedPitchOnly = true;
            Assert.IsTrue(action.FixedPitchOnly);
        }

        [TestMethod]
        public void FontTest()
        {
            var action = new FontDialogAction();
            Assert.AreEqual(SysDraw.SystemFonts.DefaultFont, action.Font);

            var font = SysDraw.SystemFonts.CaptionFont;
            action.Font = font;
            Assert.AreSame(font, action.Font);
        }

        [TestMethod]
        public void FontMustExistTest()
        {
            var action = new FontDialogAction();
            Assert.IsFalse(action.FontMustExist);

            action.FontMustExist = true;
            Assert.IsTrue(action.FontMustExist);
        }

        [TestMethod]
        public void MaxSizeTest()
        {
            var action = new FontDialogAction();
            Assert.AreEqual(0, action.MaxSize);

            ++action.MaxSize;
            Assert.AreEqual(1, action.MaxSize);
        }

        [TestMethod]
        public void MinSizeTest()
        {
            var action = new FontDialogAction();
            Assert.AreEqual(0, action.MinSize);

            ++action.MinSize;
            Assert.AreEqual(1, action.MinSize);
        }

        [TestMethod]
        public void ScriptsOnlyTest()
        {
            var action = new FontDialogAction();
            Assert.IsFalse(action.ScriptsOnly);

            action.ScriptsOnly = true;
            Assert.IsTrue(action.ScriptsOnly);
        }

        [TestMethod]
        public void ShowApplyTest()
        {
            var action = new FontDialogAction();
            Assert.IsFalse(action.ShowApply);

            action.ShowApply = true;
            Assert.IsTrue(action.ShowApply);
        }

        [TestMethod]
        public void ShowColorTest()
        {
            var action = new FontDialogAction();
            Assert.IsFalse(action.ShowColor);

            action.ShowColor = true;
            Assert.IsTrue(action.ShowColor);
        }

        [TestMethod]
        public void ShowEffectsTest()
        {
            var action = new FontDialogAction();
            Assert.IsTrue(action.ShowEffects);

            action.ShowEffects = false;
            Assert.IsFalse(action.ShowEffects);
        }

        [TestMethod]
        public void ShowHelpTest()
        {
            var action = new FontDialogAction();
            Assert.IsFalse(action.ShowHelp);

            action.ShowHelp = true;
            Assert.IsTrue(action.ShowHelp);
        }
    }
}
