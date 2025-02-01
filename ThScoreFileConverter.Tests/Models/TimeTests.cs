using ThScoreFileConverter.Models;

namespace ThScoreFileConverter.Tests.Models;

[TestClass]
public class TimeTests
{
    [TestMethod]
    public void TimeTestNegative()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => new Time(-1));
    }

    [TestMethod]
    public void TimeTestZero()
    {
        var time = new Time(0);

        time.Hours.ShouldBe(0);
        time.Minutes.ShouldBe(0);
        time.Seconds.ShouldBe(0);
        time.Milliseconds.ShouldBe(-1);
        time.Frames.ShouldBe(0);
        time.IsFrames.ShouldBeTrue();
    }

    [TestMethod]
    public void TimeTestMillionAndOne()
    {
        var time = new Time((1000 * 1000) + 1);

        time.Hours.ShouldBe(4);
        time.Minutes.ShouldBe(37);
        time.Seconds.ShouldBe(46);
        time.Milliseconds.ShouldBe(-1);
        time.Frames.ShouldBe(41);
        time.IsFrames.ShouldBeTrue();
    }

    [TestMethod]
    public void TimeTestNegativeFrame()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => new Time(-1, true));
    }

    [TestMethod]
    public void TimeTestZeroFrame()
    {
        var time = new Time(0, true);

        time.Hours.ShouldBe(0);
        time.Minutes.ShouldBe(0);
        time.Seconds.ShouldBe(0);
        time.Milliseconds.ShouldBe(-1);
        time.Frames.ShouldBe(0);
        time.IsFrames.ShouldBeTrue();
    }

    [TestMethod]
    public void TimeTestMillionAndOneFrames()
    {
        var time = new Time((1000 * 1000) + 1, true);

        time.Hours.ShouldBe(4);
        time.Minutes.ShouldBe(37);
        time.Seconds.ShouldBe(46);
        time.Milliseconds.ShouldBe(-1);
        time.Frames.ShouldBe(41);
        time.IsFrames.ShouldBeTrue();
    }

    [TestMethod]
    public void TimeTestNegativeMillisecond()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => new Time(-1, false));
    }

    [TestMethod]
    public void TimeTestZeroMillisecond()
    {
        var time = new Time(0, false);

        time.Hours.ShouldBe(0);
        time.Minutes.ShouldBe(0);
        time.Seconds.ShouldBe(0);
        time.Milliseconds.ShouldBe(0);
        time.Frames.ShouldBe(-1);
        time.IsFrames.ShouldBeFalse();
    }

    [TestMethod]
    public void TimeTestMillionAndOneMilliseconds()
    {
        var time = new Time((1000 * 1000) + 1, false);

        time.Hours.ShouldBe(0);
        time.Minutes.ShouldBe(16);
        time.Seconds.ShouldBe(40);
        time.Milliseconds.ShouldBe(1);
        time.Frames.ShouldBe(-1);
        time.IsFrames.ShouldBeFalse();
    }

    [TestMethod]
    public void TimeTestComponents()
    {
        var time = new Time(12, 34, 56, 43);

        time.Hours.ShouldBe(12);
        time.Minutes.ShouldBe(34);
        time.Seconds.ShouldBe(56);
        time.Milliseconds.ShouldBe(-1);
        time.Frames.ShouldBe(43);
        time.IsFrames.ShouldBeTrue();
    }

    [TestMethod]
    public void TimeTestComponentsNegativeHour()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => new Time(-1, 34, 56, 43));
    }

    [TestMethod]
    public void TimeTestComponentsExceededHours()
    {
        var time = new Time(24, 34, 56, 43);

        time.Hours.ShouldBe(24);
        time.Minutes.ShouldBe(34);
        time.Seconds.ShouldBe(56);
        time.Milliseconds.ShouldBe(-1);
        time.Frames.ShouldBe(43);
        time.IsFrames.ShouldBeTrue();
    }

    [TestMethod]
    public void TimeTestComponentsNegativeMinute()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => new Time(12, -1, 56, 43));
    }

    [TestMethod]
    public void TimeTestComponentsExceededMinutes()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => new Time(12, 60, 56, 43));
    }

    [TestMethod]
    public void TimeTestComponentsNegativeSecond()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => new Time(12, 34, -1, 43));
    }

    [TestMethod]
    public void TimeTestComponentsExceededSeconds()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => new Time(12, 34, 60, 43));
    }

    [TestMethod]
    public void TimeTestComponentsNegativeFrame()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => new Time(12, 34, 56, -1));
    }

    [TestMethod]
    public void TimeTestComponentsExceededFrames()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => new Time(12, 34, 56, 60));
    }

    [TestMethod]
    public void TimeTestComponentsWithFrames()
    {
        var time = new Time(12, 34, 56, 43, true);

        time.Hours.ShouldBe(12);
        time.Minutes.ShouldBe(34);
        time.Seconds.ShouldBe(56);
        time.Milliseconds.ShouldBe(-1);
        time.Frames.ShouldBe(43);
        time.IsFrames.ShouldBeTrue();
    }

    [TestMethod]
    public void TimeTestComponentsWithFramesNegativeHour()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => new Time(-1, 34, 56, 43, true));
    }

    [TestMethod]
    public void TimeTestComponentsWithFramesExceededHours()
    {
        var time = new Time(24, 34, 56, 43, true);

        time.Hours.ShouldBe(24);
        time.Minutes.ShouldBe(34);
        time.Seconds.ShouldBe(56);
        time.Milliseconds.ShouldBe(-1);
        time.Frames.ShouldBe(43);
        time.IsFrames.ShouldBeTrue();
    }

    [TestMethod]
    public void TimeTestComponentsWithFramesNegativeMinute()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => new Time(12, -1, 56, 43, true));
    }

    [TestMethod]
    public void TimeTestComponentsWithFramesExceededMinutes()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => new Time(12, 60, 56, 43, true));
    }

    [TestMethod]
    public void TimeTestComponentsWithFramesNegativeSecond()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => new Time(12, 34, -1, 43, true));
    }

    [TestMethod]
    public void TimeTestComponentsWithFramesExceededSeconds()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => new Time(12, 34, 60, 43, true));
    }

    [TestMethod]
    public void TimeTestComponentsWithFramesNegativeFrame()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => new Time(12, 34, 56, -1, true));
    }

    [TestMethod]
    public void TimeTestComponentsWithFramesExceededFrames()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => new Time(12, 34, 56, 60, true));
    }

    [TestMethod]
    public void TimeTestComponentsWithMilliseconds()
    {
        var time = new Time(12, 34, 56, 789, false);

        time.Hours.ShouldBe(12);
        time.Minutes.ShouldBe(34);
        time.Seconds.ShouldBe(56);
        time.Milliseconds.ShouldBe(789);
        time.Frames.ShouldBe(-1);
        time.IsFrames.ShouldBeFalse();
    }

    [TestMethod]
    public void TimeTestComponentsWithMillisecondsNegativeHour()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => new Time(-1, 34, 56, 789, false));
    }

    [TestMethod]
    public void TimeTestComponentsWithMillisecondsExceededHours()
    {
        var time = new Time(24, 34, 56, 789, false);

        time.Hours.ShouldBe(24);
        time.Minutes.ShouldBe(34);
        time.Seconds.ShouldBe(56);
        time.Milliseconds.ShouldBe(789);
        time.Frames.ShouldBe(-1);
        time.IsFrames.ShouldBeFalse();
    }

    [TestMethod]
    public void TimeTestComponentsWithMillisecondsNegativeMinute()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => new Time(12, -1, 56, 789, false));
    }

    [TestMethod]
    public void TimeTestComponentsWithMillisecondsExceededMinutes()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => new Time(12, 60, 56, 789, false));
    }

    [TestMethod]
    public void TimeTestComponentsWithMillisecondsNegativeSecond()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => new Time(12, 34, -1, 789, false));
    }

    [TestMethod]
    public void TimeTestComponentsWithMillisecondsExceededSeconds()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => new Time(12, 34, 60, 789, false));
    }

    [TestMethod]
    public void TimeTestComponentsWithMillisecondsNegativeMillisecond()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => new Time(12, 34, 56, -1, false));
    }

    [TestMethod]
    public void TimeTestComponentsWithMillisecondsExceededMilliseconds()
    {
        _ = Should.Throw<ArgumentOutOfRangeException>(() => new Time(12, 34, 56, 1000, false));
    }

    [TestMethod]
    public void ToStringTestOneDigit()
    {
        var time = new Time(1, 2, 3, 0);

        time.ToString().ShouldBe("1:02:03");
    }

    [TestMethod]
    public void ToStringTestTwoDigits()
    {
        var time = new Time(12, 34, 56, 0);

        time.ToString().ShouldBe("12:34:56");
    }

    [TestMethod]
    public void ToLongStringTestOneDigitFrames()
    {
        var time = new Time(1, 2, 3, 4, true);

        time.ToLongString().ShouldBe("1:02:03.04");
    }

    [TestMethod]
    public void ToLongStringTestTwoDigitFrames()
    {
        var time = new Time(12, 34, 56, 43, true);

        time.ToLongString().ShouldBe("12:34:56.43");
    }

    [TestMethod]
    public void ToLongStringTestOneDigitMilliseconds()
    {
        var time = new Time(1, 2, 3, 4, false);

        time.ToLongString().ShouldBe("1:02:03.004");
    }

    [TestMethod]
    public void ToLongStringTestTwoDigitMilliseconds()
    {
        var time = new Time(12, 34, 56, 78, false);

        time.ToLongString().ShouldBe("12:34:56.078");
    }

    [TestMethod]
    public void ToLongStringTestThreeDigitMilliseconds()
    {
        var time = new Time(12, 34, 56, 789, false);

        time.ToLongString().ShouldBe("12:34:56.789");
    }
}
