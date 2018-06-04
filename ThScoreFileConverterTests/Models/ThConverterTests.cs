using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThScoreFileConverter;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
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
                (sender, e) => Assert.Fail(nameof(converter.ConvertFinished) + ": " + TestUtils.Unreachable);
            converter.ConvertAllFinished +=
                (sender, e) => Assert.Fail(nameof(converter.ConvertAllFinished) + ": " + TestUtils.Unreachable);
            converter.ExceptionOccurred +=
                (sender, e) => Assert.Fail(nameof(converter.ExceptionOccurred) + ": " + TestUtils.Unreachable);

            converter.Convert(null);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod()]
        [ExpectedException(typeof(NullReferenceException))]
        public void ConvertTestInvalidType()
        {
            var pobj = new PrivateObject(typeof(ThConverter));
            var converter = pobj.Target as ThConverter;

            converter.ConvertFinished +=
                (sender, e) => Assert.Fail(nameof(converter.ConvertFinished) + ": " + TestUtils.Unreachable);
            converter.ConvertAllFinished +=
                (sender, e) => Assert.Fail(nameof(converter.ConvertAllFinished) + ": " + TestUtils.Unreachable);
            converter.ExceptionOccurred +=
                (sender, e) => Assert.Fail(nameof(converter.ExceptionOccurred) + ": " + TestUtils.Unreachable);

            converter.Convert(1);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ConvertTestNoSettings()
        {
            var pobj = new PrivateObject(typeof(ThConverter));
            var converter = pobj.Target as ThConverter;

            converter.ConvertFinished +=
                (sender, e) => Assert.Fail(nameof(converter.ConvertFinished) + ": " + TestUtils.Unreachable);
            converter.ConvertAllFinished +=
                (sender, e) => Assert.Fail(nameof(converter.ConvertAllFinished) + ": " + TestUtils.Unreachable);
            converter.ExceptionOccurred +=
                (sender, e) => Assert.Fail(nameof(converter.ExceptionOccurred) + ": " + TestUtils.Unreachable);

            converter.Convert(new SettingsPerTitle());

            Assert.Fail(TestUtils.Unreachable);
        }
    }
}
