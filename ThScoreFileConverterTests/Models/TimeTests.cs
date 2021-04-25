using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class TimeTests
    {
        [TestMethod]
        public void TimeTestNegative()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new Time(-1));
        }

        [TestMethod]
        public void TimeTestZero()
        {
            var time = new Time(0);

            Assert.AreEqual(0, time.Hours);
            Assert.AreEqual(0, time.Minutes);
            Assert.AreEqual(0, time.Seconds);
            Assert.AreEqual(-1, time.Milliseconds);
            Assert.AreEqual(0, time.Frames);
            Assert.IsTrue(time.IsFrames);
        }

        [TestMethod]
        public void TimeTestMillionAndOne()
        {
            var time = new Time((1000 * 1000) + 1);

            Assert.AreEqual(4, time.Hours);
            Assert.AreEqual(37, time.Minutes);
            Assert.AreEqual(46, time.Seconds);
            Assert.AreEqual(-1, time.Milliseconds);
            Assert.AreEqual(41, time.Frames);
            Assert.IsTrue(time.IsFrames);
        }

        [TestMethod]
        public void TimeTestNegativeFrame()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new Time(-1, true));
        }

        [TestMethod]
        public void TimeTestZeroFrame()
        {
            var time = new Time(0, true);

            Assert.AreEqual(0, time.Hours);
            Assert.AreEqual(0, time.Minutes);
            Assert.AreEqual(0, time.Seconds);
            Assert.AreEqual(-1, time.Milliseconds);
            Assert.AreEqual(0, time.Frames);
            Assert.IsTrue(time.IsFrames);
        }

        [TestMethod]
        public void TimeTestMillionAndOneFrames()
        {
            var time = new Time((1000 * 1000) + 1, true);

            Assert.AreEqual(4, time.Hours);
            Assert.AreEqual(37, time.Minutes);
            Assert.AreEqual(46, time.Seconds);
            Assert.AreEqual(-1, time.Milliseconds);
            Assert.AreEqual(41, time.Frames);
            Assert.IsTrue(time.IsFrames);
        }

        [TestMethod]
        public void TimeTestNegativeMillisecond()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new Time(-1, false));
        }

        [TestMethod]
        public void TimeTestZeroMillisecond()
        {
            var time = new Time(0, false);

            Assert.AreEqual(0, time.Hours);
            Assert.AreEqual(0, time.Minutes);
            Assert.AreEqual(0, time.Seconds);
            Assert.AreEqual(0, time.Milliseconds);
            Assert.AreEqual(-1, time.Frames);
            Assert.IsFalse(time.IsFrames);
        }

        [TestMethod]
        public void TimeTestMillionAndOneMilliseconds()
        {
            var time = new Time((1000 * 1000) + 1, false);

            Assert.AreEqual(0, time.Hours);
            Assert.AreEqual(16, time.Minutes);
            Assert.AreEqual(40, time.Seconds);
            Assert.AreEqual(1, time.Milliseconds);
            Assert.AreEqual(-1, time.Frames);
            Assert.IsFalse(time.IsFrames);
        }

        [TestMethod]
        public void TimeTestComponents()
        {
            var time = new Time(12, 34, 56, 43);

            Assert.AreEqual(12, time.Hours);
            Assert.AreEqual(34, time.Minutes);
            Assert.AreEqual(56, time.Seconds);
            Assert.AreEqual(-1, time.Milliseconds);
            Assert.AreEqual(43, time.Frames);
            Assert.IsTrue(time.IsFrames);
        }

        [TestMethod]
        public void TimeTestComponentsNegativeHour()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new Time(-1, 34, 56, 43));
        }

        [TestMethod]
        public void TimeTestComponentsExceededHours()
        {
            var time = new Time(24, 34, 56, 43);

            Assert.AreEqual(24, time.Hours);
            Assert.AreEqual(34, time.Minutes);
            Assert.AreEqual(56, time.Seconds);
            Assert.AreEqual(-1, time.Milliseconds);
            Assert.AreEqual(43, time.Frames);
            Assert.IsTrue(time.IsFrames);
        }

        [TestMethod]
        public void TimeTestComponentsNegativeMinute()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new Time(12, -1, 56, 43));
        }

        [TestMethod]
        public void TimeTestComponentsExceededMinutes()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new Time(12, 60, 56, 43));
        }

        [TestMethod]
        public void TimeTestComponentsNegativeSecond()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new Time(12, 34, -1, 43));
        }

        [TestMethod]
        public void TimeTestComponentsExceededSeconds()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new Time(12, 34, 60, 43));
        }

        [TestMethod]
        public void TimeTestComponentsNegativeFrame()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new Time(12, 34, 56, -1));
        }

        [TestMethod]
        public void TimeTestComponentsExceededFrames()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new Time(12, 34, 56, 60));
        }

        [TestMethod]
        public void TimeTestComponentsWithFrames()
        {
            var time = new Time(12, 34, 56, 43, true);

            Assert.AreEqual(12, time.Hours);
            Assert.AreEqual(34, time.Minutes);
            Assert.AreEqual(56, time.Seconds);
            Assert.AreEqual(-1, time.Milliseconds);
            Assert.AreEqual(43, time.Frames);
            Assert.IsTrue(time.IsFrames);
        }

        [TestMethod]
        public void TimeTestComponentsWithFramesNegativeHour()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new Time(-1, 34, 56, 43, true));
        }

        [TestMethod]
        public void TimeTestComponentsWithFramesExceededHours()
        {
            var time = new Time(24, 34, 56, 43, true);

            Assert.AreEqual(24, time.Hours);
            Assert.AreEqual(34, time.Minutes);
            Assert.AreEqual(56, time.Seconds);
            Assert.AreEqual(-1, time.Milliseconds);
            Assert.AreEqual(43, time.Frames);
            Assert.IsTrue(time.IsFrames);
        }

        [TestMethod]
        public void TimeTestComponentsWithFramesNegativeMinute()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new Time(12, -1, 56, 43, true));
        }

        [TestMethod]
        public void TimeTestComponentsWithFramesExceededMinutes()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new Time(12, 60, 56, 43, true));
        }

        [TestMethod]
        public void TimeTestComponentsWithFramesNegativeSecond()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new Time(12, 34, -1, 43, true));
        }

        [TestMethod]
        public void TimeTestComponentsWithFramesExceededSeconds()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new Time(12, 34, 60, 43, true));
        }

        [TestMethod]
        public void TimeTestComponentsWithFramesNegativeFrame()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new Time(12, 34, 56, -1, true));
        }

        [TestMethod]
        public void TimeTestComponentsWithFramesExceededFrames()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new Time(12, 34, 56, 60, true));
        }

        [TestMethod]
        public void TimeTestComponentsWithMilliseconds()
        {
            var time = new Time(12, 34, 56, 789, false);

            Assert.AreEqual(12, time.Hours);
            Assert.AreEqual(34, time.Minutes);
            Assert.AreEqual(56, time.Seconds);
            Assert.AreEqual(789, time.Milliseconds);
            Assert.AreEqual(-1, time.Frames);
            Assert.IsFalse(time.IsFrames);
        }

        [TestMethod]
        public void TimeTestComponentsWithMillisecondsNegativeHour()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new Time(-1, 34, 56, 789, false));
        }

        [TestMethod]
        public void TimeTestComponentsWithMillisecondsExceededHours()
        {
            var time = new Time(24, 34, 56, 789, false);

            Assert.AreEqual(24, time.Hours);
            Assert.AreEqual(34, time.Minutes);
            Assert.AreEqual(56, time.Seconds);
            Assert.AreEqual(789, time.Milliseconds);
            Assert.AreEqual(-1, time.Frames);
            Assert.IsFalse(time.IsFrames);
        }

        [TestMethod]
        public void TimeTestComponentsWithMillisecondsNegativeMinute()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new Time(12, -1, 56, 789, false));
        }

        [TestMethod]
        public void TimeTestComponentsWithMillisecondsExceededMinutes()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new Time(12, 60, 56, 789, false));
        }

        [TestMethod]
        public void TimeTestComponentsWithMillisecondsNegativeSecond()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new Time(12, 34, -1, 789, false));
        }

        [TestMethod]
        public void TimeTestComponentsWithMillisecondsExceededSeconds()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new Time(12, 34, 60, 789, false));
        }

        [TestMethod]
        public void TimeTestComponentsWithMillisecondsNegativeMillisecond()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new Time(12, 34, 56, -1, false));
        }

        [TestMethod]
        public void TimeTestComponentsWithMillisecondsExceededMilliseconds()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new Time(12, 34, 56, 1000, false));
        }

        [TestMethod]
        public void ToStringTestOneDigit()
        {
            var time = new Time(1, 2, 3, 0);

            Assert.AreEqual("1:02:03", time.ToString());
        }

        [TestMethod]
        public void ToStringTestTwoDigits()
        {
            var time = new Time(12, 34, 56, 0);

            Assert.AreEqual("12:34:56", time.ToString());
        }

        [TestMethod]
        public void ToLongStringTestOneDigitFrames()
        {
            var time = new Time(1, 2, 3, 4, true);

            Assert.AreEqual("1:02:03.04", time.ToLongString());
        }

        [TestMethod]
        public void ToLongStringTestTwoDigitFrames()
        {
            var time = new Time(12, 34, 56, 43, true);

            Assert.AreEqual("12:34:56.43", time.ToLongString());
        }

        [TestMethod]
        public void ToLongStringTestOneDigitMilliseconds()
        {
            var time = new Time(1, 2, 3, 4, false);

            Assert.AreEqual("1:02:03.004", time.ToLongString());
        }

        [TestMethod]
        public void ToLongStringTestTwoDigitMilliseconds()
        {
            var time = new Time(12, 34, 56, 78, false);

            Assert.AreEqual("12:34:56.078", time.ToLongString());
        }

        [TestMethod]
        public void ToLongStringTestThreeDigitMilliseconds()
        {
            var time = new Time(12, 34, 56, 789, false);

            Assert.AreEqual("12:34:56.789", time.ToLongString());
        }
    }
}
