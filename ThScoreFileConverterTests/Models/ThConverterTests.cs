using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThScoreFileConverter;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class ThConverterTests
    {
        [TestMethod]
        public void ThConverterTest()
        {
            var converter = new ThConverterWrapper() as ThConverter;

            Assert.IsNull(converter.SupportedVersions);
            Assert.IsFalse(converter.HasBestShotConverter);
            Assert.IsTrue(converter.HasCardReplacer);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ConvertTestNull()
        {
            var converter = new ThConverterWrapper() as ThConverter;

            converter.ConvertFinished +=
                (sender, e) => Assert.Fail(nameof(converter.ConvertFinished) + ": " + TestUtils.Unreachable);
            converter.ConvertAllFinished +=
                (sender, e) => Assert.Fail(nameof(converter.ConvertAllFinished) + ": " + TestUtils.Unreachable);
            converter.ExceptionOccurred +=
                (sender, e) => Console.WriteLine(
                    Utils.Format("{0}: {1}", nameof(converter.ExceptionOccurred), e.Exception.ToString()));

            converter.Convert(null);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ConvertTestInvalidType()
        {
            var converter = new ThConverterWrapper() as ThConverter;

            converter.ConvertFinished +=
                (sender, e) => Assert.Fail(nameof(converter.ConvertFinished) + ": " + TestUtils.Unreachable);
            converter.ConvertAllFinished +=
                (sender, e) => Assert.Fail(nameof(converter.ConvertAllFinished) + ": " + TestUtils.Unreachable);
            converter.ExceptionOccurred +=
                (sender, e) => Console.WriteLine(
                    Utils.Format("{0}: {1}", nameof(converter.ExceptionOccurred), e.Exception.ToString()));

            converter.Convert(1);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConvertTestNoSettings()
        {
            var converter = new ThConverterWrapper() as ThConverter;

            converter.ConvertFinished +=
                (sender, e) => Assert.Fail(nameof(converter.ConvertFinished) + ": " + TestUtils.Unreachable);
            converter.ConvertAllFinished +=
                (sender, e) => Assert.Fail(nameof(converter.ConvertAllFinished) + ": " + TestUtils.Unreachable);
            converter.ExceptionOccurred +=
                (sender, e) => Console.WriteLine(
                    Utils.Format("{0}: {1}", nameof(converter.ExceptionOccurred), e.Exception.ToString()));

            converter.Convert(new SettingsPerTitle());

            Assert.Fail(TestUtils.Unreachable);
        }
    }
}
