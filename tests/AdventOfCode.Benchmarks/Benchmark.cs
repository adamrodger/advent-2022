using System.Reflection;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode.Benchmarks
{
    public class Benchmark
    {
        [ParamsSource(nameof(Days))]
        [Params(10)]
        public int Day { get; set; }

        [ParamsSource(nameof(Parts))]
        public int Part { get; set; }

        private object instance;
        private MethodInfo method;
        private object[] arguments;

        public static IEnumerable<int> Days => typeof(DayXX).Assembly.ExportedTypes
                                                            .Where(t => Regex.IsMatch(t.Name, @"Day\d+"))
                                                            .Select(t => t.Name[3..])
                                                            .Select(int.Parse);

        // this keeps the output in order, otherwise it does all part 1s then all part 2s so the output is annoying
        public static IEnumerable<int> Parts => new[] { 1, 2 };

        [GlobalSetup]
        public void Setup()
        {
            DirectoryInfo solutionDir = new DirectoryInfo(".");

            while (solutionDir!.EnumerateFiles().All(f => !string.Equals(f.Name, "AdventOfCode.sln")))
            {
                solutionDir = solutionDir.Parent;
            }

            var input = File.ReadAllLines($"{solutionDir.FullName}/src/AdventOfCode/inputs/day{this.Day}.txt");
            this.arguments = new[] { input };

            Type type = typeof(DayXX).Assembly.ExportedTypes.First(t => t.Name == $"Day{this.Day}");
            this.instance = Activator.CreateInstance(type, true);
            this.method = this.instance!.GetType().GetMethod($"Part{this.Part}");
        }

        [Benchmark]
        public object Solve()
        {
            object result = method.Invoke(instance, arguments);
            return result;
        }
    }
}
