using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ThScoreFileConverter.Models.Tests
{
    [TestClass()]
    public class TimeTests
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTest_Negative()
        {
            var time = new Time(-1);
        }

        [TestMethod()]
        public void TimeTest_Zero()
        {
            var time = new Time(0);

            Assert.AreEqual(0, time.Hours);
            Assert.AreEqual(0, time.Minutes);
            Assert.AreEqual(0, time.Seconds);
            Assert.AreEqual(-1, time.Milliseconds);
            Assert.AreEqual(0, time.Frames);
            Assert.IsTrue(time.IsFrames);
        }

        [TestMethod()]
        public void TimeTest_MillionAndOne()
        {
            var time = new Time(1000 * 1000 + 1);

            Assert.AreEqual(4, time.Hours);
            Assert.AreEqual(37, time.Minutes);
            Assert.AreEqual(46, time.Seconds);
            Assert.AreEqual(-1, time.Milliseconds);
            Assert.AreEqual(41, time.Frames);
            Assert.IsTrue(time.IsFrames);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTest_Frame_Negative()
        {
            var time = new Time(-1, true);
        }

        [TestMethod()]
        public void TimeTest_Frame_Zero()
        {
            var time = new Time(0, true);

            Assert.AreEqual(0, time.Hours);
            Assert.AreEqual(0, time.Minutes);
            Assert.AreEqual(0, time.Seconds);
            Assert.AreEqual(-1, time.Milliseconds);
            Assert.AreEqual(0, time.Frames);
            Assert.IsTrue(time.IsFrames);
        }

        [TestMethod()]
        public void TimeTest_Frame_MillionAndOne()
        {
            var time = new Time(1000 * 1000 + 1, true);

            Assert.AreEqual(4, time.Hours);
            Assert.AreEqual(37, time.Minutes);
            Assert.AreEqual(46, time.Seconds);
            Assert.AreEqual(-1, time.Milliseconds);
            Assert.AreEqual(41, time.Frames);
            Assert.IsTrue(time.IsFrames);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTest_Millisecond_Negative()
        {
            var time = new Time(-1, false);
        }

        [TestMethod()]
        public void TimeTest_Millisecond_Zero()
        {
            var time = new Time(0, false);

            Assert.AreEqual(0, time.Hours);
            Assert.AreEqual(0, time.Minutes);
            Assert.AreEqual(0, time.Seconds);
            Assert.AreEqual(0, time.Milliseconds);
            Assert.AreEqual(-1, time.Frames);
            Assert.IsFalse(time.IsFrames);
        }

        [TestMethod()]
        public void TimeTest_Millisecond_MillionAndOne()
        {
            var time = new Time(1000 * 1000 + 1, false);

            Assert.AreEqual(0, time.Hours);
            Assert.AreEqual(16, time.Minutes);
            Assert.AreEqual(40, time.Seconds);
            Assert.AreEqual(1, time.Milliseconds);
            Assert.AreEqual(-1, time.Frames);
            Assert.IsFalse(time.IsFrames);
        }

        [TestMethod()]
        public void TimeTest_Time()
        {
            var time = new Time(12, 34, 56, 43);

            Assert.AreEqual(12, time.Hours);
            Assert.AreEqual(34, time.Minutes);
            Assert.AreEqual(56, time.Seconds);
            Assert.AreEqual(-1, time.Milliseconds);
            Assert.AreEqual(43, time.Frames);
            Assert.IsTrue(time.IsFrames);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTest_Time_NegativeHour()
        {
            var time = new Time(-1, 34, 56, 43);
        }

        [TestMethod()]
        public void TimeTest_Time_ExceededHour()
        {
            var time = new Time(24, 34, 56, 43);

            Assert.AreEqual(24, time.Hours);
            Assert.AreEqual(34, time.Minutes);
            Assert.AreEqual(56, time.Seconds);
            Assert.AreEqual(-1, time.Milliseconds);
            Assert.AreEqual(43, time.Frames);
            Assert.IsTrue(time.IsFrames);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTest_Time_NegativeMinute()
        {
            var time = new Time(12, -1, 56, 43);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTest_Time_ExceededMinute()
        {
            var time = new Time(12, 60, 56, 43);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTest_Time_NegativeSecond()
        {
            var time = new Time(12, 34, -1, 43);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTest_Time_ExceededSecond()
        {
            var time = new Time(12, 34, 60, 43);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTest_Time_NegativeFrame()
        {
            var time = new Time(12, 34, 56, -1);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTest_Time_ExceededFrame()
        {
            var time = new Time(12, 34, 56, 60);
        }

        [TestMethod()]
        public void TimeTest_TimeWithFrames()
        {
            var time = new Time(12, 34, 56, 43, true);

            Assert.AreEqual(12, time.Hours);
            Assert.AreEqual(34, time.Minutes);
            Assert.AreEqual(56, time.Seconds);
            Assert.AreEqual(-1, time.Milliseconds);
            Assert.AreEqual(43, time.Frames);
            Assert.IsTrue(time.IsFrames);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTest_TimeWithFrames_NegativeHour()
        {
            var time = new Time(-1, 34, 56, 43, true);
        }

        [TestMethod()]
        public void TimeTest_TimeWithFrames_ExceededHour()
        {
            var time = new Time(24, 34, 56, 43, true);

            Assert.AreEqual(24, time.Hours);
            Assert.AreEqual(34, time.Minutes);
            Assert.AreEqual(56, time.Seconds);
            Assert.AreEqual(-1, time.Milliseconds);
            Assert.AreEqual(43, time.Frames);
            Assert.IsTrue(time.IsFrames);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTest_TimeWithFrames_NegativeMinute()
        {
            var time = new Time(12, -1, 56, 43, true);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTest_TimeWithFrames_ExceededMinute()
        {
            var time = new Time(12, 60, 56, 43, true);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTest_TimeWithFrames_NegativeSecond()
        {
            var time = new Time(12, 34, -1, 43, true);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTest_TimeWithFrames_ExceededSecond()
        {
            var time = new Time(12, 34, 60, 43, true);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTest_TimeWithFrames_NegativeFrame()
        {
            var time = new Time(12, 34, 56, -1, true);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTest_TimeWithFrames_ExceededFrame()
        {
            var time = new Time(12, 34, 56, 60, true);
        }

        [TestMethod()]
        public void TimeTest_TimeWithMilliseconds()
        {
            var time = new Time(12, 34, 56, 789, false);

            Assert.AreEqual(12, time.Hours);
            Assert.AreEqual(34, time.Minutes);
            Assert.AreEqual(56, time.Seconds);
            Assert.AreEqual(789, time.Milliseconds);
            Assert.AreEqual(-1, time.Frames);
            Assert.IsFalse(time.IsFrames);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTest_TimeWithMilliseconds_NegativeHour()
        {
            var time = new Time(-1, 34, 56, 789, false);
        }

        [TestMethod()]
        public void TimeTest_TimeWithMilliseconds_ExceededHour()
        {
            var time = new Time(24, 34, 56, 789, false);

            Assert.AreEqual(24, time.Hours);
            Assert.AreEqual(34, time.Minutes);
            Assert.AreEqual(56, time.Seconds);
            Assert.AreEqual(789, time.Milliseconds);
            Assert.AreEqual(-1, time.Frames);
            Assert.IsFalse(time.IsFrames);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTest_TimeWithMilliseconds_NegativeMinute()
        {
            var time = new Time(12, -1, 56, 789, false);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTest_TimeWithMilliseconds_ExceededMinute()
        {
            var time = new Time(12, 60, 56, 789, false);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTest_TimeWithMilliseconds_NegativeSecond()
        {
            var time = new Time(12, 34, -1, 789, false);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTest_TimeWithMilliseconds_ExceededSecond()
        {
            var time = new Time(12, 34, 60, 789, false);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTest_TimeWithMilliseconds_NegativeMillisecond()
        {
            var time = new Time(12, 34, 56, -1, false);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeTest_TimeWithMilliseconds_ExceededMillisecond()
        {
            var time = new Time(12, 34, 56, 1000, false);
        }

        [TestMethod()]
        public void ToStringTest_OneDigit()
        {
            var time = new Time(1, 2, 3, 0);

            Assert.AreEqual("1:02:03", time.ToString());
        }

        [TestMethod()]
        public void ToStringTest_TwoDigits()
        {
            var time = new Time(12, 34, 56, 0);

            Assert.AreEqual("12:34:56", time.ToString());
        }

        [TestMethod()]
        public void ToLongStringTest_Frame_OneDigit()
        {
            var time = new Time(1, 2, 3, 4, true);

            Assert.AreEqual("1:02:03.04", time.ToLongString());
        }

        [TestMethod()]
        public void ToLongStringTest_Frame_TwoDigits()
        {
            var time = new Time(12, 34, 56, 43, true);

            Assert.AreEqual("12:34:56.43", time.ToLongString());
        }

        [TestMethod()]
        public void ToLongStringTest_Millisecond_OneDigit()
        {
            var time = new Time(1, 2, 3, 4, false);

            Assert.AreEqual("1:02:03.004", time.ToLongString());
        }

        [TestMethod()]
        public void ToLongStringTest_Millisecond_TwoDigits()
        {
            var time = new Time(12, 34, 56, 78, false);

            Assert.AreEqual("12:34:56.078", time.ToLongString());
        }

        [TestMethod()]
        public void ToLongStringTest_Millisecond_ThreeDigits()
        {
            var time = new Time(12, 34, 56, 789, false);

            Assert.AreEqual("12:34:56.789", time.ToLongString());
        }
    }
}