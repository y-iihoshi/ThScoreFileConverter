using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class TimeTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTestNegative()
        {
            _ = new Time(-1);
            Assert.Fail(TestUtils.Unreachable);
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
            var time = new Time(1000 * 1000 + 1);

            Assert.AreEqual(4, time.Hours);
            Assert.AreEqual(37, time.Minutes);
            Assert.AreEqual(46, time.Seconds);
            Assert.AreEqual(-1, time.Milliseconds);
            Assert.AreEqual(41, time.Frames);
            Assert.IsTrue(time.IsFrames);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTestNegativeFrame()
        {
            _ = new Time(-1, true);
            Assert.Fail(TestUtils.Unreachable);
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
            var time = new Time(1000 * 1000 + 1, true);

            Assert.AreEqual(4, time.Hours);
            Assert.AreEqual(37, time.Minutes);
            Assert.AreEqual(46, time.Seconds);
            Assert.AreEqual(-1, time.Milliseconds);
            Assert.AreEqual(41, time.Frames);
            Assert.IsTrue(time.IsFrames);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTestNegativeMillisecond()
        {
            _ = new Time(-1, false);
            Assert.Fail(TestUtils.Unreachable);
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
            var time = new Time(1000 * 1000 + 1, false);

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
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTestComponentsNegativeHour()
        {
            _ = new Time(-1, 34, 56, 43);
            Assert.Fail(TestUtils.Unreachable);
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
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTestComponentsNegativeMinute()
        {
            _ = new Time(12, -1, 56, 43);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTestComponentsExceededMinutes()
        {
            _ = new Time(12, 60, 56, 43);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTestComponentsNegativeSecond()
        {
            _ = new Time(12, 34, -1, 43);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTestComponentsExceededSeconds()
        {
            _ = new Time(12, 34, 60, 43);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTestComponentsNegativeFrame()
        {
            _ = new Time(12, 34, 56, -1);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTestComponentsExceededFrames()
        {
            _ = new Time(12, 34, 56, 60);
            Assert.Fail(TestUtils.Unreachable);
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
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTestComponentsWithFramesNegativeHour()
        {
            _ = new Time(-1, 34, 56, 43, true);
            Assert.Fail(TestUtils.Unreachable);
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
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTestComponentsWithFramesNegativeMinute()
        {
            _ = new Time(12, -1, 56, 43, true);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTestComponentsWithFramesExceededMinutes()
        {
            _ = new Time(12, 60, 56, 43, true);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTestComponentsWithFramesNegativeSecond()
        {
            _ = new Time(12, 34, -1, 43, true);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTestComponentsWithFramesExceededSeconds()
        {
            _ = new Time(12, 34, 60, 43, true);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTestComponentsWithFramesNegativeFrame()
        {
            _ = new Time(12, 34, 56, -1, true);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTestComponentsWithFramesExceededFrames()
        {
            _ = new Time(12, 34, 56, 60, true);
            Assert.Fail(TestUtils.Unreachable);
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
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTestComponentsWithMillisecondsNegativeHour()
        {
            _ = new Time(-1, 34, 56, 789, false);
            Assert.Fail(TestUtils.Unreachable);
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
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTestComponentsWithMillisecondsNegativeMinute()
        {
            _ = new Time(12, -1, 56, 789, false);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTestComponentsWithMillisecondsExceededMinutes()
        {
            _ = new Time(12, 60, 56, 789, false);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTestComponentsWithMillisecondsNegativeSecond()
        {
            _ = new Time(12, 34, -1, 789, false);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTestComponentsWithMillisecondsExceededSeconds()
        {
            _ = new Time(12, 34, 60, 789, false);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTestComponentsWithMillisecondsNegativeMillisecond()
        {
            _ = new Time(12, 34, 56, -1, false);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTestComponentsWithMillisecondsExceededMilliseconds()
        {
            _ = new Time(12, 34, 56, 1000, false);
            Assert.Fail(TestUtils.Unreachable);
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
