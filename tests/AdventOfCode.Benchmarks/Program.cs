using System.Diagnostics;
using AdventOfCode.Benchmarks;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

IConfig config = Debugger.IsAttached ? new DebugInProcessConfig() : null;
BenchmarkRunner.Run<Benchmark>(config, args);
