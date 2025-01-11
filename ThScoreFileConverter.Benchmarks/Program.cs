using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;

var config = DefaultConfig.Instance.AddDiagnoser(MemoryDiagnoser.Default);
_ = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).RunAll(config, args);
