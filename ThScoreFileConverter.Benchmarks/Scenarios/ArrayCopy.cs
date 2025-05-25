using BenchmarkDotNet.Attributes;

namespace ThScoreFileConverter.Benchmarks.Scenarios;

public class ArrayCopy
{
    private readonly int[] source = [.. Enumerable.Range(0, 1000)];

    [Benchmark]
    public int[] CopyTo()
    {
        var destination = new int[this.source.Length];
        this.source.CopyTo(destination, 0);
        return destination;
    }

    [Benchmark]
    public int[] CollectionExpression()
    {
        int[] destination = [.. this.source];
        return destination;
    }
}
