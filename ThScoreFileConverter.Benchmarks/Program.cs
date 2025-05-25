using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;

var config = DefaultConfig.Instance.AddDiagnoser(MemoryDiagnoser.Default);
#if false
_ = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).RunAllJoined(config, args);
#else
_ = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);
#endif
