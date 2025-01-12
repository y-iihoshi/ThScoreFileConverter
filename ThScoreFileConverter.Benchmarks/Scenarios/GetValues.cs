using BenchmarkDotNet.Attributes;
using ThScoreFileConverter.Core.Helpers;

namespace ThScoreFileConverter.Benchmarks.Scenarios;

#pragma warning disable CA1822 // Mark members as static

public class GetValues
{
    [GlobalSetup]
    public void Setup()
    {
        _ = Enum.GetValues<DayOfWeek>();
        _ = EnumHelper<DayOfWeek>.Enumerable;
    }

    [Benchmark]
    public DayOfWeek[] NetCore()
    {
        return Enum.GetValues<DayOfWeek>();
    }

    [Benchmark]
    public DayOfWeek[] EnumHelper()
    {
        return (DayOfWeek[])EnumHelper<DayOfWeek>.Enumerable;
    }
}
