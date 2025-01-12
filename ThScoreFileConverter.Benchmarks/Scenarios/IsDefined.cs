using BenchmarkDotNet.Attributes;
using EnumHelper_ = ThScoreFileConverter.Core.Helpers.EnumHelper;

namespace ThScoreFileConverter.Benchmarks.Scenarios;

#pragma warning disable CA1822 // Mark members as static

public class IsDefined
{
    [GlobalSetup]
    public void Setup()
    {
        _ = Enum.IsDefined(DayOfWeek.Sunday);
        _ = EnumHelper_.IsDefined(DayOfWeek.Sunday);
    }

    [Benchmark]
    public bool NetCore()
    {
        return Enum.IsDefined(DayOfWeek.Sunday);
    }

    [Benchmark]
    public bool EnumHelper()
    {
        return EnumHelper_.IsDefined(DayOfWeek.Sunday);
    }
}
