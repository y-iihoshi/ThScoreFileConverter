using BenchmarkDotNet.Attributes;
using ThScoreFileConverter.Core.Extensions;

namespace ThScoreFileConverter.Benchmarks.Scenarios;

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA1822 // Mark members as static

public class GetName
{
    [GlobalSetup]
    public void Setup()
    {
        _ = DayOfWeek.Sunday.ToString();
        _ = Enum.GetName(DayOfWeek.Sunday);
        _ = Enum.GetName(DayOfWeek.Sunday)?.ToString();
        _ = DayOfWeek.Sunday.ToName();
        _ = DayOfWeek.Sunday.ToName().ToString();
    }

    [Benchmark]
    public string NetCore_ToString()
    {
        return DayOfWeek.Sunday.ToString();
    }

    [Benchmark]
    public string? NetCore_GetName()
    {
        return Enum.GetName(DayOfWeek.Sunday);
    }

    [Benchmark]
    public string? NetCore_GetName_ToString()
    {
        return Enum.GetName(DayOfWeek.Sunday)?.ToString();
    }

    [Benchmark]
    public string NetCore_nameof()
    {
        return nameof(DayOfWeek.Sunday);
    }

    [Benchmark]
    public string EnumHelper_ToName()
    {
        return DayOfWeek.Sunday.ToName();
    }

    [Benchmark]
    public string EnumHelper_ToName_ToString()
    {
        return DayOfWeek.Sunday.ToName().ToString();
    }
}
