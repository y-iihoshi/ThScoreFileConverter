using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class ThConverterTests
    {
        [TestMethod]
        public void ThConverterTest()
        {
            var converter = new Mock<ThConverter> { CallBase = true }.Object;

            Assert.AreEqual(string.Empty, converter.SupportedVersions);
            Assert.IsFalse(converter.HasBestShotConverter);
            Assert.IsTrue(converter.HasCardReplacer);
        }

        [TestMethod]
        public void ConvertTestNull()
        {
            var converter = new Mock<ThConverter> { CallBase = true }.Object;

            converter.ConvertFinished +=
                (sender, e) => Assert.Fail(nameof(converter.ConvertFinished) + ": " + TestUtils.Unreachable);
            converter.ConvertAllFinished +=
                (sender, e) => Assert.Fail(nameof(converter.ConvertAllFinished) + ": " + TestUtils.Unreachable);
            converter.ExceptionOccurred +=
                (sender, e) => Console.WriteLine(
                    Utils.Format("{0}: {1}", nameof(converter.ExceptionOccurred), e.Exception.ToString()));

            _ = Assert.ThrowsException<ArgumentNullException>(() => converter.Convert(null!));
        }

        [TestMethod]
        public void ConvertTestInvalidType()
        {
            var converter = new Mock<ThConverter> { CallBase = true }.Object;

            converter.ConvertFinished +=
                (sender, e) => Assert.Fail(nameof(converter.ConvertFinished) + ": " + TestUtils.Unreachable);
            converter.ConvertAllFinished +=
                (sender, e) => Assert.Fail(nameof(converter.ConvertAllFinished) + ": " + TestUtils.Unreachable);
            converter.ExceptionOccurred +=
                (sender, e) => Console.WriteLine(
                    Utils.Format("{0}: {1}", nameof(converter.ExceptionOccurred), e.Exception.ToString()));

            _ = Assert.ThrowsException<ArgumentException>(() => converter.Convert(1));
        }

        [TestMethod]
        public void ConvertTestNoSettings()
        {
            var converter = new Mock<ThConverter> { CallBase = true }.Object;

            converter.ConvertFinished +=
                (sender, e) => Assert.Fail(nameof(converter.ConvertFinished) + ": " + TestUtils.Unreachable);
            converter.ConvertAllFinished +=
                (sender, e) => Assert.Fail(nameof(converter.ConvertAllFinished) + ": " + TestUtils.Unreachable);
            converter.ExceptionOccurred +=
                (sender, e) => Console.WriteLine(
                    Utils.Format("{0}: {1}", nameof(converter.ExceptionOccurred), e.Exception.ToString()));

            _ = Assert.ThrowsException<ArgumentException>(() => converter.Convert(new SettingsPerTitle()));
        }
    }
}
