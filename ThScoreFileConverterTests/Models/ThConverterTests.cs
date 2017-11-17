using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ThScoreFileConverter.Models.Tests
{
    [TestClass()]
    public class ThConverterTests
    {
        [TestMethod()]
        public void ThConverterTest()
        {
            var pobj = new PrivateObject(typeof(ThConverter));
            var converter = pobj.Target as ThConverter;

            Assert.IsNull(converter.SupportedVersions);
            Assert.IsFalse(converter.HasBestShotConverter);
            Assert.IsTrue(converter.HasCardReplacer);
        }

        [TestMethod()]
        [ExpectedException(typeof(NullReferenceException))]
        public void ConvertTestNull()
        {
            var pobj = new PrivateObject(typeof(ThConverter));
            var converter = pobj.Target as ThConverter;

            converter.ConvertFinished +=
                (sender, e) => Assert.Fail("ConvertFinished: Unreachable");
            converter.ConvertAllFinished +=
                (sender, e) => Assert.Fail("ConvertAllFinished: Unreachable");
            converter.ExceptionOccurred +=
                (sender, e) => Assert.Fail("ExceptionOccurred: Unreachable");

            converter.Convert(null);

            Assert.Fail("Unreachable");
        }

        [TestMethod()]
        [ExpectedException(typeof(NullReferenceException))]
        public void ConvertTestInvalidType()
        {
            var pobj = new PrivateObject(typeof(ThConverter));
            var converter = pobj.Target as ThConverter;

            converter.ConvertFinished +=
                (sender, e) => Assert.Fail("ConvertFinished: Unreachable");
            converter.ConvertAllFinished +=
                (sender, e) => Assert.Fail("ConvertAllFinished: Unreachable");
            converter.ExceptionOccurred +=
                (sender, e) => Assert.Fail("ExceptionOccurred: Unreachable");

            converter.Convert(1);

            Assert.Fail("Unreachable");
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ConvertTestNoSettings()
        {
            var pobj = new PrivateObject(typeof(ThConverter));
            var converter = pobj.Target as ThConverter;

            converter.ConvertFinished +=
                (sender, e) => Assert.Fail("ConvertFinished: Unreachable");
            converter.ConvertAllFinished +=
                (sender, e) => Assert.Fail("ConvertAllFinished: Unreachable");
            converter.ExceptionOccurred +=
                (sender, e) => Assert.Fail("ExceptionOccurred: Unreachable");

            converter.Convert(new SettingsPerTitle());

            Assert.Fail("Unreachable");
        }
    }
}
